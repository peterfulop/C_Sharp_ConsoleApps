using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _211117_opening_weekend
{

    class Film
    {
        public string EredetiCim { get; set; }
        public string MagyarCim { get; set; }
        public DateTime Bemutato { get; set; }
        public string Forgalmazo { get; set; }
        public Int64 Bevetel { get; set; }

        public Int64 Latogato { get; set; }
    }

    class Program
    {
        public static List<Film> Filmek = new List<Film>();
        static void Main(string[] args)
        {
            Feladat_02();

            Feladat_03();

            Feladat_04();

            Feladat_05();

            Feladat_06();

            Feladat_07();

            Feladat_08();

            Console.ReadLine();
        }

        private static void Feladat_08()
        {
            var interCom = Filmek.Where(x => x.Forgalmazo == "InterCom").OrderBy(x=>x.Bemutato).ToList();

            var max = 0;

            for (int i = 0; i < interCom.Count()-1; i++)
            {
                var actual = new DateTime(interCom[i].Bemutato.Year, interCom[i].Bemutato.Month, interCom[i].Bemutato.Day);
                var next = new DateTime(interCom[i+1].Bemutato.Year, interCom[i + 1].Bemutato.Month, interCom[i + 1].Bemutato.Day);
                var diff = (next - actual).Days;

                if (max < diff) max = diff;
            }

            Console.WriteLine($"8. feladat: A leghosszabb időszak két InterCom-os bemutató között: {max} nap");
        }

        private static void Feladat_07()
        {
            var file = "stat.csv";

            var list = Filmek.GroupBy(x => x.Forgalmazo)
                             .Select(y => new { y.Key, count = y.Count() })
                             .Where(z => z.count > 1)
                             .ToList();

            using (var fs = new FileStream(file,FileMode.Create))
            {
                using (var sw = new StreamWriter(fs,Encoding.UTF8))
                {
                    sw.WriteLine("forgalazo;filmekSzama");

                    foreach (var item in list)
                    {
                        sw.WriteLine($"{item.Key};{item.count}");
                       
                    }
                }
            }

        }

        private static void Feladat_06()
        {

            var egyezes = false;

            foreach (var f in Filmek)
            {

                var ecim = f.EredetiCim.ToLower().Split(' ');
                var mcim = f.MagyarCim.ToLower().Split(' ');

                for (int i = 0; i < ecim.Length; i++)
                {
                    if (ecim[i].ToString().StartsWith("w"))
                    {
                        egyezes = true;
                    }
                    else
                    {
                        egyezes = false;
                        break;
                    }
                }

                for (int j = 0; j < mcim.Length; j++)
                {
                    if (mcim[j].ToString().StartsWith("w"))
                    {
                        egyezes = true;
                    }
                    else
                    {
                        egyezes = false;
                        break;
                    }
                }

                if (egyezes)
                {
                    Console.WriteLine("6. feladat: Ilyen film volt!");
                    break;
                }
            }

        }

        private static void Feladat_05()
        {
            var TOP = Filmek.OrderByDescending(x => x.Latogato).First() ;

            Console.WriteLine($"5. feladat: Legtöbb látogató az első héten:");
            Console.WriteLine($"\tEredeti cím: {TOP.EredetiCim}\n" +
                $"\tMagyar cím: {TOP.MagyarCim}\n" +
                $"\tForgalmazó: {TOP.Forgalmazo}\n" +
                $"\tBevétel az első héten: {TOP.Bevetel} Ft\n" +
                $"\tLátogatók száma: {TOP.Latogato} fő");
        }

        private static void Feladat_04()
        {
            var UIP = Filmek.Where(x => x.Forgalmazo.Equals("UIP")).Sum(x=>x.Bevetel);

             Console.WriteLine($"4. feladat: UIP Duna Film forgalmazó 1. hetes bevételeinek összege: {UIP: # ###} Ft");

        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: Filmek száma az állományban: {Filmek.Count()} db");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("nyitohetvege.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        var sor = sr.ReadLine().Split(';');
                        var datum = sor[2].Split('.');

                        var film = new Film() {
                            EredetiCim = sor[0],
                            MagyarCim = sor[1],
                            Bemutato = new DateTime(Convert.ToInt32(datum[0]), Convert.ToInt32(datum[1]), Convert.ToInt32(datum[2])),
                            Forgalmazo = sor[3],
                            Bevetel = Convert.ToInt64(sor[4]),
                            Latogato = Convert.ToInt64(sor[5])
                        };

                        Filmek.Add(film);

                    }
                }
            }
        }
    }
}
