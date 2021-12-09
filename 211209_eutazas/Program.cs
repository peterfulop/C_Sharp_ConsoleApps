using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _211209_eutazas
{
    class Utas
    {
        public int Megallo { get; set; }
        public DateTime Felszallas { get; set; }
        public string Id { get; set; }
        public string Kategoria { get; set; }
        public bool Berlet { get; set; }
        public DateTime Ervenyes { get; set; }
        public int Alkalom { get; set; }
    }
    class Program
    {
       public static List<Utas> Utasok = new List<Utas>();
        static void Main(string[] args)
        {
            Feladat_01();
            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_07();

            Console.ReadLine();
        }

        private static void Feladat_07()
        {
            var alert = Utasok.Where(x => napokszama(x.Felszallas.Year, x.Felszallas.Month, x.Felszallas.Day, x.Ervenyes.Year, x.Ervenyes.Month, x.Ervenyes.Day) == 3);
            using (var fs = new FileStream("figyelmeztetes.txt",FileMode.Create))
            {
                using (var sw = new StreamWriter(fs,Encoding.UTF8))
                {
                    foreach (var item in alert)
                    {
                        sw.WriteLine($"{item.Id}\t{item.Ervenyes.Year}-{item.Ervenyes.Month}-{item.Ervenyes.Day}");
                    }
                }
            }

            Console.WriteLine("7. feladat\nFile kiírása kész!");
        }
        private static int napokszama(int e1, int h1, int n1, int e2, int h2, int n2)
        {
            h1 = (h1 + 9) / 12;
            e1 = e1 - h1 % 10;
            var d1 = 365 * e1 + e1 % 4 - e1 % 100 + e1 % 400 + (h1 * 306 + 5) % 10 + n1 - 1;

            h2 = (h2 + 9) / 12;
            e2 = e2 - h2 % 10;
            var d2 = 365 * e2 + e2 % 4 - e2 % 100 + e2 % 400 + (h2 * 306 + 5) % 10 + n2 - 1;

            return d2 - d1;
        }
        private static void Feladat_05()
        {
            Console.WriteLine("5. feladat");

            var ingyenes = Utasok.Where(x => x.Kategoria == "RVS" || x.Kategoria == "GYK" || x.Kategoria == "NYP").Count();
            var kedvezmenyes = Utasok.Where(x => x.Berlet && x.Kategoria == "TAB" || x.Kategoria == "NYB");

            var lejartKedvezmenyesBerlet = kedvezmenyes.Where(x => x.Berlet && (new DateTime(x.Ervenyes.Year, x.Ervenyes.Month, x.Ervenyes.Day) - new DateTime(x.Felszallas.Year, x.Felszallas.Month, x.Felszallas.Day)).TotalDays < 0).Count();

            Console.WriteLine($"Ingyenesen utazók száma: {ingyenes} fő");
            Console.WriteLine($"A kedvezményesen utazók száma: { kedvezmenyes.Count() - lejartKedvezmenyesBerlet} fő");

        }
        private static void Feladat_04()
        {
            Console.WriteLine("4. feladat");

            var legtobb = Utasok.GroupBy(x => x.Megallo)
                                .Select(x => new { megallo = x.Key, count = x.Count() })
                                .OrderByDescending(x=>x.count)
                                .First();

            Console.WriteLine($"A legtöbb utas {legtobb.count} fő a {legtobb.megallo}. megállóban próbált felszállni.");
        }
        private static void Feladat_03()
        {
            Console.WriteLine("3. feladat");

            var lejartJegy = Utasok.Count(x => x.Alkalom == 0);
            var lejartBerlet = Utasok.Where(x => x.Berlet  && (new DateTime(x.Ervenyes.Year, x.Ervenyes.Month, x.Ervenyes.Day) - new DateTime(x.Felszallas.Year, x.Felszallas.Month, x.Felszallas.Day)).TotalDays < 0 ).Count();

            Console.WriteLine($"A buszra {lejartJegy+ lejartBerlet} utas nem szállhatott fel.");
        }
        private static void Feladat_02()
        {
            Console.WriteLine("2. feladat");
            Console.WriteLine($"A buszra {Utasok.Count()} utas felszállni.");
        }
        public static DateTime generalDateTime(string datum)
        {
            if (datum.Contains("-"))
            {
                var puffer = datum.Split('-');
                var ev = $"{puffer[0][0]}{puffer[0][1]}{puffer[0][2]}{puffer[0][3]}";
                var honap = $"{puffer[0][4]}{puffer[0][5]}";
                var nap = $"{puffer[0][6]}{puffer[0][7]}";
                var ora = $"{puffer[1][0]}{puffer[1][1]}";
                var perc = $"{puffer[1][2]}{puffer[1][3]}";
                var myDate = Convert.ToDateTime($"{ev}.{honap}.{nap} {ora}:{perc}");
                return myDate;
            }
            else
            {
                var ev = $"{datum[0]}{datum[1]}{datum[2]}{datum[3]}";
                var honap = $"{datum[4]}{datum[5]}";
                var nap = $"{datum[6]}{datum[7]}";
                var myDate = Convert.ToDateTime($"{ev}.{honap}.{nap}");
                return myDate;
            }

        }
        private static void Feladat_01()
        {
            using (var fs = new FileStream("utasadat.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var utas = new Utas();

                        var line = sr.ReadLine().Split(' ');
                        utas.Megallo = int.Parse(line[0]);
                        utas.Felszallas = generalDateTime(line[1]);
                        utas.Id = line[2];
                        utas.Kategoria = line[3];

                        if (line[4].Length > 2)
                        {
                            utas.Ervenyes = generalDateTime(line[4]);
                            utas.Alkalom = -1;
                            utas.Berlet = true;
                        }
                        else
                        {
                            utas.Alkalom = int.Parse(line[4]);
                            utas.Berlet = false;
                        }

                        Utasok.Add(utas);
                    }
                }
            }
        }
    }
}
