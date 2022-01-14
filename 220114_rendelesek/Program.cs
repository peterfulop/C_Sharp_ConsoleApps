using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220114_rendelesek
{
    class Termek
    {
        public string Cikkszam { get; set; }
        public string Nev { get; set; }
        public int Ar { get; set; }
        public int Keszlet { get; set; }
        public int KeszletKalkulacio { get; set; }
    }

    class Rendeles
    {
        public DateTime Datum { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Cikkszam { get; set; }
        public int Mennyiseg { get; set; }
        public int Ar { get; set; }
        public bool Status { get;  set; }
    }

    internal class Program
    {
        public static List<Termek> Termekek = new List<Termek>();
        public static List<Rendeles> Rendelesek = new List<Rendeles>();
        
        static void Main(string[] args)
        {
            ImportRaktar();
            ImportRendeles();
            SetFinalStatus();
            SetKeszlet();
            CreateEmailList();
            CreateBeszerzesList();
            Console.WriteLine("Rendelesek OKJ T 54 213 05/1/1 : Művelet vége.");

            Console.ReadLine();
        }
        private static void PrintRendelesek()
        {
            foreach (var item in Rendelesek)
            {
                Console.WriteLine($"{item.Id}, {item.Email}, {item.Cikkszam}, {item.Mennyiseg}, {item.Status}");
            }
        }

        private static void PrintRaktar()
        {
            foreach (var t in Termekek.OrderBy(x => x.Cikkszam))
            {
                Console.WriteLine($"{t.Cikkszam}, {t.Nev}, {t.Ar}, {t.Keszlet}");
            }
        }

        private static void CreateBeszerzesList()
        {
            using (var fs = new FileStream("beszerzes.csv", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    var beszerzes = Rendelesek.Where(x=>!x.Status)
                        .GroupBy(x => x.Cikkszam)
                        .Select(x => new {
                           Cikkszam = x.Key,
                           Igeny = x.Sum(y => y.Mennyiseg)
                       })
                       .OrderBy(x=>x.Cikkszam)
                       .ToList();

                    foreach (var b in beszerzes)
                    {
                        var igeny = b.Igeny - GetKeszletInfo(b.Cikkszam);
                        if (igeny > 0)
                        {
                            sw.WriteLine($"{b.Cikkszam};{igeny}");
                        }
                    }
                }
            }
        }

        private static void CreateEmailList()
        {
            using (var fs = new FileStream("levek.csv", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    var email = Rendelesek.GroupBy(x => x.Id)
                        .Select(x => new {
                            Status = x.Select(y => y.Status).First(),
                            Email = x.Select(y => y.Email).First(),
                            Sum = x.Sum(y => y.Ar)
                        })
                        .ToList();

                    foreach (var e in email)
                    {
                        var statMsg = e.Status == true ?
                            $"A rendelését 2 napon belül szállítjuk! A rendelés értéke: {e.Sum} Ft" :
                            "A rendelése függő állapotba került. Hamarosan értesítjük a szállítás időpontjáról.";

                        sw.WriteLine($"{e.Email};{statMsg}");
                    }
                }
            }
        }

        private static void SetKeszlet()
        {
            var statusOK = Rendelesek.Where(x => x.Status).ToList();

            foreach (var item in statusOK)
            {
                foreach (var t in Termekek)
                {
                    if (item.Cikkszam == t.Cikkszam)
                    {
                        t.Keszlet -= item.Mennyiseg;
                    }
                }
            }
        }

        private static void SetFinalStatus()
        {
            var getFalseIds = Rendelesek.Where(x => !x.Status)
                .GroupBy(x => x.Id)
                .Select(x => x.Key)
                .ToArray();

            foreach (var r in Rendelesek)
            {
                r.Status = getFalseIds.Contains(r.Id) ? false : true;
            }
        }

        private static int GetPrice(string cikkszam)
        {
            return Termekek.FirstOrDefault(x => x.Cikkszam == cikkszam).Ar;
        }

        private static int GetKeszletInfo(string cikkszam)
        {
            return Termekek.FirstOrDefault(x => x.Cikkszam == cikkszam).Keszlet;
        }

        private static bool CheckKeszlet(string cikkszam,int mennyiseg)
        {
            var termek = Termekek.FirstOrDefault(x => x.Cikkszam == cikkszam);

            if (termek.KeszletKalkulacio >= mennyiseg)
            {
                termek.KeszletKalkulacio -= mennyiseg;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ImportRendeles()
        {
            using (var fs = new FileStream("rendeles.csv", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    var Datum = new DateTime();
                    var Id = 0;
                    var Email = "";
                    var Cikkszam = "";
                    var Mennyiseg = 0;

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujRendeles = new Rendeles();
                     
                        if (line[0] == "M")
                        {
                            Datum = Convert.ToDateTime(line[1]);
                            Id = int.Parse(line[2]);
                            Email = line[3];
                        }
                        else
                        {
                            Cikkszam = line[2];
                            Mennyiseg = int.Parse(line[3]);

                            ujRendeles.Datum = Datum;
                            ujRendeles.Id = Id;
                            ujRendeles.Email = Email;
                            ujRendeles.Cikkszam = Cikkszam;
                            ujRendeles.Mennyiseg = Mennyiseg;
                            ujRendeles.Ar = Mennyiseg * GetPrice(Cikkszam);
                            ujRendeles.Status = CheckKeszlet(Cikkszam, Mennyiseg);

                            Rendelesek.Add(ujRendeles);
                        }
                    }
                }
            }
        }

        private static void ImportRaktar()
        {
            using (var fs = new FileStream("raktar.csv",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujTermek = new Termek()
                        {
                            Cikkszam = line[0],
                            Nev = line[1],
                            Ar = int.Parse(line[2]),
                            Keszlet = int.Parse(line[3]),
                            KeszletKalkulacio = int.Parse(line[3])
                        };
                        Termekek.Add(ujTermek);
                    }
                }
            }
        }
    }

    
}
