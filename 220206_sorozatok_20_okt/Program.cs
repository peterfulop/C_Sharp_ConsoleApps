using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220206_sorozatok_20_okt
{
    class Sorozat
    {
        public string Adasban { get; set; }
        public string Cim { get; set; }
        public int Evad { get; set; }
        public int Epizod { get; set; }
        public int Hossz { get; set; }
        public int Latta { get; set; }
    }


    internal class Program
    {

        public static List<Sorozat> Sorozatok = new List<Sorozat>();    

        static void Main(string[] args)
        {
            Feladat_01();
            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_07();
            Feladat_08();

            Console.ReadLine();
        }

        private static void Feladat_08()
        {
            var export = Sorozatok.GroupBy(x => x.Cim)
                                  .Select(x => new { cim = x.Key, count = x.Count(), sum = x.Sum(y => y.Hossz) })
                                  .ToList();
            using (var fs = new FileStream("summa.txt",FileMode.Create))
            {
                using (var sw = new StreamWriter(fs,Encoding.UTF8))
                {
                    foreach (var e in export)
                    {
                        sw.WriteLine($"{e.cim} {e.sum} {e.count}");
                    }
                }
            }
        }

        private static void Feladat_07()
        {
            Console.Write("\n7. feladat\nAdja meg a hét napját (például cs)! Nap= ");
            var nap = Console.ReadLine();

            var res = Sorozatok.Where(x => x.Adasban != "NI")
                               .Where(x => Hetnapja(2020, 10, int.Parse(x.Adasban.Split('.')[2])) == nap)
                               .Select(x=>x.Cim)
                               .Distinct()
                               .ToList();

            if(res.Count == 0)
            {
                Console.WriteLine("Az adott napon nem került adásba sorozat.");
                return;
            }
            res.ForEach(x => Console.WriteLine($"{x}"));
        }

        private static string Hetnapja(int ev, int ho,int nap)
        {
            string [] napok = {"v","h","k","sze","cs","p","szo" };
            int[] honapok = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
            if (ho < 3) ev -= 1;
            return napok[(ev + ev / 4 - ev / 100 + ev / 400 + honapok[ho - 1] + nap) % 7];
        }

        private static void Feladat_05()
        {
            Console.Write("\nAdjon egy egy dátumot! Dátum= ");
            var datum = Console.ReadLine();

            var res = Sorozatok.Where(x => x.Adasban != "NI" && x.Latta == 0)
                .Where(x => Convert.ToDateTime(x.Adasban) <= Convert.ToDateTime(datum))
                .ToList();

            res.ForEach(x => Console.WriteLine($"{x.Evad}x{x.Epizod}\t{x.Cim}"));
        }

        private static void Feladat_04()
        {
            var percek = Sorozatok.Where(x => x.Latta == 1).Sum(x=>x.Hossz);
            var time = TimeSpan.FromMinutes(percek);
            Console.WriteLine($"\n4. feladat\nSorozatnézéssel {time.Days} napot {time.Hours} órát és {time.Minutes} percet töltött.");
        }

        private static void Feladat_03()
        {
            var latta = Sorozatok.Average(x => x.Latta);
            Console.WriteLine($"\n3. feladat\nA listában lévő epizódok {latta*100:0.00}%-át látta.");
        }

        private static void Feladat_02()
        {
            var count = Sorozatok.Count(x => x.Adasban != "NI");
            Console.WriteLine($"2. feladat\nA listában {count} db vetítési dátummal rendelkező epizód van.");
        }

        private static void Feladat_01()
        {
            using (var fs = new FileStream("lista.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] puffer = new string[5];

                        for (int i = 0; i < 5; i++)
                        {
                            puffer[i] = sr.ReadLine();
                        }

                        var ujSorozat = new Sorozat()
                        {
                            Adasban = puffer[0],
                            Cim = puffer[1],
                            Evad = int.Parse(puffer[2].Split('x')[0]),
                            Epizod = int.Parse(puffer[2].Split('x')[1]),
                            Hossz = int.Parse(puffer[3]),
                            Latta = int.Parse(puffer[4])
                        };
                        Sorozatok.Add(ujSorozat);
                    }
                }
            }
        }
    }
}
