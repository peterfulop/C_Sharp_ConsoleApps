using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220119_ACB_kosarlabda
{
    class Eredmeny
    {
        public string Hazai { get; set; }
        public string Idegen { get; set; }
        public int HazaiPont { get; set; }
        public int IdegenPont { get; set; }
        public string Helyszin { get; set; }
        public string Idopont { get; set; }
    }

    internal class Program
    {

        private static List<Eredmeny> Eredmenyek = new List<Eredmeny>();

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
            Console.WriteLine($"6. feladat:");
            Eredmenyek.GroupBy(x => x.Helyszin)
                .Select(x => new { Stadion =x.Key, Count = x.Count() })
                .Where(x => x.Count > 20)
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Stadion}: {x.Count}"));            
        }

        private static void Feladat_06()
        {
            Console.WriteLine($"6. feladat:");
            Eredmenyek.Where(x => x.Idopont == "2004-11-21")
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Hazai}-{x.Idegen} ({x.HazaiPont}:{x.IdegenPont})"));
        }

        private static void Feladat_05()
        {
            var barcelona = Eredmenyek.FirstOrDefault(x => x.Hazai.ToLower().Contains("barcelona")).Hazai;
            Console.WriteLine($"5. feladat: barcelonai csapat neve: {barcelona}");
        }

        private static void Feladat_04()
        {
            var dontetlen = Eredmenyek.Exists(x => x.HazaiPont == x.IdegenPont) ? "volt" : "nem";
            Console.WriteLine($"4. feladat: Volt döntetlen? {dontetlen}");
        }

        private static void Feladat_03()
        {
            var realHazai = Eredmenyek.Count(x => x.Hazai == "Real Madrid");
            var realIdegen = Eredmenyek.Count(x => x.Idegen == "Real Madrid");
            Console.WriteLine($"3. feladat: Real Madrid: Hazai: {realHazai}, Idegen: {realIdegen}");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("eredmenyek.csv",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujEredmeny = new Eredmeny()
                        {
                            Hazai = line[0],
                            Idegen = line[1],
                            HazaiPont = int.Parse(line[2]),
                            IdegenPont = int.Parse(line[3]),
                            Helyszin = line[4],
                            Idopont = line[5]
                        };

                        Eredmenyek.Add(ujEredmeny);
                    }
                }
            }
        }
    }
}
