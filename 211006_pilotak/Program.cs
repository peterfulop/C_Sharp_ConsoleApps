using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _211006_pilotak
{
    class Pilota
    {
        public string Nev { get; set; }
        public DateTime Datum { get; set; }
        public string Nemzetiseg { get; set; }
        public int Rajtszam { get; set; }

    }
    class Program
    {
        public static List<Pilota> Pilotak = new List<Pilota>();

        static void Main(string[] args)
        {
            Feladat_1_2();
            Feladat_3();
            Feladat_4();
            Feladat_5();
            Feladat_6();
            Feladat_7();

            Console.ReadLine();
        }

        public static void Feladat_1_2()
        {
            using (var fs = new FileStream("pilotak.csv", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {

                        string[] line = sr.ReadLine().Split(';');
                        string[] datum = line[1].Split('.');

                        var pilota = new Pilota
                        {
                            Nev = line[0],
                            Datum = new DateTime(
                            Convert.ToInt32(datum[0]),
                            Convert.ToInt32(datum[1]),
                            Convert.ToInt32(datum[2])
                            ),
                            Nemzetiseg = line[2],
                            Rajtszam = line[3].Length > 0 ? Convert.ToInt32(line[3]) : default
                        };

                        Pilotak.Add(pilota);
                    }
                }
            }
        }

        public static void Feladat_3()
        {
            Console.WriteLine($"3.Feladat: {Pilotak.Count}");
        }

        public static void Feladat_4()
        {
            Console.WriteLine($"4.Feladat: {Pilotak.LastOrDefault().Nev}");
        }

        public static void Feladat_5()
        {
            Console.WriteLine($"5.Feladat:");
            Pilotak.Where(a => a.Datum.Year < 1901)
                .ToList()
                .ForEach(p => Console.WriteLine($"\t{p.Nev} ({p.Datum.ToShortDateString()})"));
        }

        public static void Feladat_6()
        {
            var p = Pilotak.Where(a => a.Rajtszam > 0)
                .OrderBy(a => a.Rajtszam)
                .FirstOrDefault().Nemzetiseg;
            Console.WriteLine($"6.Feladat: {p}");
        }

        public static void Feladat_7()
        {
            Console.Write("7.feladat: ");

            var duplak = Pilotak.GroupBy(a => a.Rajtszam)
                                   .Where(b => b.Key > 0)
                                   .Select(c => new { Szam = c.Key, Count = c.Count() })
                                   .Where(d => d.Count > 1)
                                   .Select(e => e.Szam)
                                   .ToArray();

            Console.WriteLine(String.Join(", ", duplak));

        }

    }
}
