using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220126_EU_tagallamok
{
    class EU
    {
        public string Orszag { get; set; }
        public DateTime Csatlakozas { get; set; }
    }
    internal class Program
    {
        public static List<EU> EUtagok = new List<EU>();

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
            Console.WriteLine($"8. feladat: Statisztika");
            EUtagok.GroupBy(x=>x.Csatlakozas)
                .Select(x => new {Ev = x.Key.Year, Count = x.Count()})
                .Where(x => x.Count > 0)
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Ev} - {x.Count} ország"));
        }

        private static void Feladat_07()
        {
            Console.WriteLine($"7. feladat: Legutoljára csatlakozott ország: {EUtagok.OrderBy(x=>x.Csatlakozas).Last().Orszag}");
        }

        private static void Feladat_06()
        {
            var majus = EUtagok.Exists(x => x.Csatlakozas.Month == 5) ? "volt" : "nem volt";
            Console.WriteLine($"6. feladat: Májusban {majus} csatlakozás!");
        }

        private static void Feladat_05()
        {
            Console.WriteLine($"5. feladat: Magyarország csatlakozásának dátuma: {EUtagok.First(x => x.Orszag == "Magyarország").Csatlakozas.ToShortDateString()}");
        }

        private static void Feladat_04()
        {
            Console.WriteLine($"4. feladat: 2007-ben {EUtagok.Count(x=>x.Csatlakozas.Year ==2007)} ország csatlakozott.");
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: EU tagállamok száma: {EUtagok.Count} db");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("EUcsatlakozas.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujTag = new EU()
                        {
                            Orszag = line[0],
                            Csatlakozas = Convert.ToDateTime(line[1])
                        };
                        EUtagok.Add(ujTag);
                    }                   
                }
            }
        }
    }
}
