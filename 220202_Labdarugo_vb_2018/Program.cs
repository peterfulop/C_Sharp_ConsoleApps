using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220202_Labdarugo_vb_2018
{

    class Stadion
    {
        public string Varos { get; set; }
        public string Nev { get; set; }
        public string AlternativNev { get; set; }
        public int Ferohely { get; set; }
    }

    internal class Program
    {
        public static List<Stadion> Stadionok = new List<Stadion>();
        static void Main(string[] args)
        {
            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_06();
            var varos = Feladat_07();
            Feladat_08(varos);
            Feladat_09();

            Console.ReadLine();
        }

        private static void Feladat_09()
        {
            Console.WriteLine($"9. feladat: {Stadionok.Select(x=>x.Varos).Distinct().Count()} különböző városban voltak mérkőzések.");
        }

        private static void Feladat_08(string varos)
        {
            var isVaros = Stadionok.Exists(x=>x.Varos.ToLower() == varos) ? "VB" : "nem VB";
            Console.WriteLine($"8. feladat: A megadott város {isVaros} helyszín.");
        }

        private static string Feladat_07()
        {
            Console.Write("7. feladat: Kérem a város nevét:");
            var varos = Console.ReadLine();

            while (varos.Length <3)
            {
                Console.Write("7. feladat: Kérem a város nevét:");
                varos = Console.ReadLine();
            }

            return varos.ToLower();
        }

        private static void Feladat_06()
        {
            Console.WriteLine($"6. feladat: Két néven is ismert stadionok száma: {Stadionok.Count(x=>x.AlternativNev != "n.a.")}");
        }

        private static void Feladat_05()
        {
            Console.WriteLine($"5. feladat: Átlagos férőhelyszám: {Stadionok.Average(x=>x.Ferohely):0.0}");
        }

        private static void Feladat_04()
        {
            Console.WriteLine($"4. feladat: A legkevesebb férőhely:");
            var min = Stadionok.OrderBy(x=>x.Ferohely).First();
            Console.WriteLine($"\tVáros: {min.Varos}\n\tStadion neve: {min.Nev}\n\tFérőhely: {min.Ferohely}");
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: Stadionok száma: {Stadionok.Count}");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("vb2018.txt", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujStadion = new Stadion()
                        {
                            Varos = line[0],
                            Nev = line[1],
                            AlternativNev = line[2],
                            Ferohely = int.Parse(line[3])
                        };

                        Stadionok.Add(ujStadion);
                    }
                }
            }
        }
    }
}
