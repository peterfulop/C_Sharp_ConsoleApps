using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220110_nyelvvizsga
{
    class Vizsga
    {
        public string Nyelv { get; set; }
        public int Ev { get; set; }
        public int Osszeg { get; set; }
        public bool Sikeres { get; set; }
    }

    internal class Program
    {
        public static List<int> Evek = new List<int>();
        public static List<Vizsga> Vizsgak = new List<Vizsga>();
        static void Main(string[] args)
        {
            Feladat_01();
            Feladat_02();
            var ev = Feladat_03();
            Feladat_04(ev);
            Feladat_05(ev);
            Feladat_06();
            Console.ReadLine();
        }
        private static void Feladat_06()
        {
            var atlag = Vizsgak.GroupBy(x => x.Nyelv)
                .Select(x => new {
                    Key = x.Key,
                    sum = x.Sum(y => y.Osszeg),
                    sikeres = x.Where(y => y.Sikeres).Sum(y => y.Osszeg),
                    sikertelen = x.Where(z => z.Sikeres == false).Sum(z => z.Osszeg)
                });

            using (var fs = new FileStream("osszesites.csv", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var v in atlag)
                    {
                        float arany = (float)v.sikertelen / v.sum * 100;
                        var line = $"{v.Key};{v.sum};{arany:0.00}%";
                        sw.WriteLine(line);
                    }
                }
            }
        }
        private static void Feladat_05(int ev)
        {
            Console.WriteLine("5. feladat:");
            var nemvolt = Vizsgak.Where(x => x.Ev == ev && x.Osszeg == 0).GroupBy(x => x.Nyelv).ToList();

            if (nemvolt.Count() == 0)
            {
                Console.WriteLine("Minden nyelvből volt vizsgázó!");
            }
            else
            {
                nemvolt.ForEach(x => Console.WriteLine($"\t{x.Key}"));
            }
        }
        private static void Feladat_04(int ev)
        {
            Console.WriteLine("4. feladat:");

            var sikertelen = Vizsgak.Where(x => x.Sikeres == false && x.Ev == ev)
                .OrderByDescending(x => x.Osszeg).First();

            var osszesen = Vizsgak.FirstOrDefault(x => x.Nyelv == sikertelen.Nyelv && x.Ev == ev).Osszeg;

            double arany = (double)sikertelen.Osszeg / (sikertelen.Osszeg + osszesen) * 100;

            Console.WriteLine($"{ev}-ben {sikertelen.Nyelv} nyelvből a sikertelen vizsgák aránya {arany:0.00} %");
        }
        private static int Feladat_03()
        {
            Console.WriteLine("3. feladat:");
            Console.Write("\tVizsgálandó év:");
            var ev = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(ev) || int.Parse(ev) < 2009 || int.Parse(ev) > 2017)
            {
                Console.WriteLine("2009 és 2017 közötti értéket adj meg!");
                Console.Write("Vizsgálandó év:");
                ev = Console.ReadLine();
            }
            return int.Parse(ev);
        }
        private static void Feladat_02()
        {
            var leg = Vizsgak
                .GroupBy(x => x.Nyelv)
                .Select(x => new { Key = x.Key, sum = x.Sum(y => y.Osszeg) })
                .OrderByDescending(x => x.sum)
                .ToList();

            Console.WriteLine("2. feladat: A legnépszerűbb nyelvek:");

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"\t{leg[i].Key}");
            }
        }
        private static void Feladat_01()
        {
            fileReading("sikeres.csv", true);
            fileReading("sikertelen.csv", false);
        }
        private static void fileReading(string fileName, bool sikeres)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    var header = sr.ReadLine().Split(';');

                    foreach (var item in header)
                    {
                        if (item.StartsWith("2"))
                        {
                            Evek.Add(int.Parse(item));
                        }
                    }

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');

                        for (int i = 1; i < line.Length; i++)
                        {
                            var newVizsga = new Vizsga();
                            newVizsga.Nyelv = line[0];
                            newVizsga.Sikeres = sikeres;
                            newVizsga.Ev = Evek[i - 1];
                            newVizsga.Osszeg = int.Parse(line[i]);
                            Vizsgak.Add(newVizsga);
                        }
                    }
                }
            }
        }
    }
}
