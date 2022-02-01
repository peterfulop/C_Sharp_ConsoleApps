using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220201_hotellift
{
    class Lift
    {
        public DateTime Datum { get; set; }
        public int KartyaId { get; set; }
        public int StartPont { get; set; }
        public int EndPont { get; set; }
    }

    internal class Program
    {
        public static List<Lift> LiftList = new List<Lift>();

        static void Main(string[] args)
        {
            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            int[] inp = Feladat_06();
            Feladat_07(inp[0],inp[1]);
            Feladat_08();
            Console.ReadLine();
        }

        private static void Feladat_08()
        {
            Console.WriteLine($"8. feladat: Statisztika");
            LiftList.GroupBy(x => x.Datum)
                .Select(x => new { datum = x.Key, count = x.Count() })
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.datum.ToShortDateString().Replace(" ","")} - {x.count}x"));
        }

        private static void Feladat_07(int kartya, int emelet)
        {
            var valasz = LiftList.Exists(x => x.KartyaId == kartya && x.EndPont == emelet) ? "utaztak" : "nem utaztak";
            Console.WriteLine($"7. feladat: A(z) {kartya}. kártyával {valasz} a(z) {emelet}. emeletre!");
        }

        private static int[] Feladat_06()
        {
            Console.WriteLine($"6. feladat:");
            int[] ret = new int[2];
            try
            {
                var kartya = GetKartyaId();
                var emelet = GetEmelet();
                ret[0] = int.Parse(kartya);
                ret[1] = int.Parse(emelet);
            }
            catch (Exception)
            {
                ret[0] = 5;
                ret[1] = 5;
            }
            return ret;
        }

        private static string GetEmelet()
        {
            Console.Write($"\tCélszint száma: ");
            return Console.ReadLine();
        }

        private static string GetKartyaId()
        {
            Console.Write($"\tKártya száma: ");
            return Console.ReadLine();
        }

        private static void Feladat_05()
        {
            Console.WriteLine($"5. feladat: Célszint max: {LiftList.Max(x => x.EndPont)}");
        }

        private static void Feladat_04()
        {
            Console.WriteLine($"4. feladat: Időszak: {LiftList.Min(x=>x.Datum).ToShortDateString()} - {LiftList.Max(x => x.Datum).ToShortDateString()}");
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: Összes lifthasználat: {LiftList.Count}");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("lift.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(' ');
                        var ujLift = new Lift()
                        {
                            Datum = Convert.ToDateTime(line[0]),
                            KartyaId = int.Parse(line[1]),
                            StartPont = int.Parse(line[2]),
                            EndPont = int.Parse(line[3])
                        };
                        LiftList.Add(ujLift);
                    }
                }
            }
        }
    }
}
