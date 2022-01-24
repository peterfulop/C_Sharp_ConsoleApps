using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220124_snooker_vilagbajnoksag
{

    class Jatekos
    {
        public int Helyezes { get; set; }
        public string Nev { get; set; }
        public string Orszag { get; set; }
        public int Nyeremeny { get; set; }
    }

    internal class Program
    {
        public static List<Jatekos> Ranglista = new List<Jatekos>();

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
            Ranglista.GroupBy(x => x.Orszag)
                .Select(x => new { Orszag = x.Key, Count = x.Count() })
                .Where(x => x.Count > 4)
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Orszag} - {x.Count} fő"));
        }

        private static void Feladat_06()
        {
            var norveg = Ranglista.Exists(x=>x.Orszag=="Norvégia") ? "van" : "nincs";
            Console.WriteLine($"6. feladat: A versenyzők között {norveg} norvég versenyző.");
        }

        private static void Feladat_05()
        {
            var kinai = Ranglista.Where(x => x.Orszag == "Kína")
                .OrderByDescending(x => x.Nyeremeny)
                .First();

            Console.WriteLine($"5. feladat: A legjobban kereső kínai versenyző:");
            Console.WriteLine($"\tHelyezés: {kinai.Helyezes}\n" +
                $"\tNév: {kinai.Nev}\n" +
                $"\tOrszág: {kinai.Orszag}\n" +
                $"\tNyeremény összege: {(kinai.Nyeremeny * 380):#,##0} Ft");
        }

        private static void Feladat_04()
        {
            Console.WriteLine($"4. feladat: A versenyzők átlagosan {Ranglista.Average(x=>x.Nyeremeny):0.00} fontot kerestek");
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: A világranglistán {Ranglista.Count} versenyző szerepel");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("snooker.txt", FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujVersenyzo = new Jatekos()
                        {
                            Helyezes = int.Parse(line[0]),
                            Nev = line[1],
                            Orszag = line[2],
                            Nyeremeny = int.Parse(line[3])
                        };
                        Ranglista.Add(ujVersenyzo);
                    }
                }
            }
        }
    }
}
