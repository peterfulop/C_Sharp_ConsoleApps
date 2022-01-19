using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220119_FIFA_vilagranglista
{

    class Rang
    {
        public string Csapat { get; set; }
        public int Helyezes { get; set; }
        public int Valtozas { get; set; }
        public int Pontszam { get; set; }
    }

    internal class Program
    {
        public static List<Rang> Ranglista = new List<Rang>();

        static void Main(string[] args)
        {
            Feladat_02();

            Feladat_03();

            Feladat_04();

            Feladat_05();

            Feladat_06();

            Feladat_07();

            Console.ReadLine();
        }

        private static void Feladat_07()
        {
            Console.WriteLine($"7. feladat: Statisztika");

            Ranglista.GroupBy(x => x.Valtozas)
                .Select(x => new { x.Key, Count = x.Count() })
                .Where(x=>x.Count > 1)
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Key,-2} helyet változott: {x.Count} csapat"));
        }

        private static void Feladat_06()
        {
            var vanHun = Ranglista.Exists(x => x.Csapat == "Magyarország") ? "van" : "nincs";
            Console.WriteLine($"6. feladat: A csapatok között {vanHun} Magyarország");
        }

        private static void Feladat_05()
        {
            Console.WriteLine($"5. feladat: A legtöbbet javító csapat:");
            var top = Ranglista.OrderByDescending(x => x.Valtozas).First();
            Console.WriteLine($"\tHelyezés: {top.Helyezes}\n\tCsapat: {top.Csapat}\n\tPontszám: {top.Pontszam}");
        }

        private static void Feladat_04()
        {
            Console.WriteLine($"4. feladat: A csapatok átlagos pontszáma: {Ranglista.Average(x=>x.Pontszam):0.00} pont");
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: A világranglistában {Ranglista.Count} csapat szerepel");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("fifa.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujRang = new Rang()
                        {
                            Csapat = line[0],
                            Helyezes = int.Parse(line[1]),
                            Valtozas = int.Parse(line[2]),
                            Pontszam = int.Parse(line[3])
                        };

                        Ranglista.Add(ujRang);
                    }
                }
            }
        }
    }
}
