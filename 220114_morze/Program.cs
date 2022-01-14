using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220114_morze
{

    class ABC
    {
        public string Karakter { get; set; }
        public string Kod { get; set; }
    }

    class Idezet
    {
        public string Szerzo { get; set;}
        public string Szoveg { get; set;}
        public string SzerzoKAR { get; set;}
        public string SzovegKAR { get; set;}
        public int Hossz { get; set; }
    }

    internal class Program
    {

        public static List<ABC> ABC = new List<ABC>();
        public static List<Idezet> Idezetek = new List<Idezet>();

        static void Main(string[] args)
        {
            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_07();
            Feladat_08();
            Feladat_09();
            Feladat_10();

            Console.ReadLine();
        }

        private static void Feladat_10()
        {
            using (var fs = new FileStream("forditas.txt",FileMode.Create))
            {
                using (var sw = new StreamWriter(fs,Encoding.UTF8))
                {
                    foreach (var id in Idezetek)
                    {
                        sw.WriteLine($"{id.SzerzoKAR}:{id.SzovegKAR}.");
                    }
                }
            }
        }

        private static void Feladat_09()
        {
            var filter = Idezetek.Where(x => x.SzerzoKAR == "ARISZTOTELÉSZ").ToList();
            Console.WriteLine($"9. feladat: Arisztotelész idézetei:");
            filter.ForEach(x => Console.WriteLine($"\t- {Morze2Szoveg(x.Szoveg)}."));
        }

        private static void Feladat_08()
        {
            var max = Idezetek.OrderByDescending(x => x.Hossz).ToList().First();
            Console.WriteLine($"8. feladat: A leghosszabb idézet szerzője és az idézet: {Morze2Szoveg(max.Szerzo)}: {Morze2Szoveg(max.Szoveg)}.");
        }

        private static void Feladat_07()
        {
            var szerzo = Morze2Szoveg(Idezetek.FirstOrDefault().Szerzo);
            Console.WriteLine($"7. feladat: Az első idézet szerzője: {szerzo}");
        }

        public static string Morze2Szoveg(string morze)
        {
            var kod ="";
            var space = 0;
            var szoveg = "";

            foreach (var item in morze)
            {
                if(item!=' ')
                {
                    kod += item;
                    space = 0;
                }
                else
                {
                    space++;

                    if (kod.Length > 0)
                    {
                        var talalat = ABC.FirstOrDefault(x => x.Kod == kod);
                        szoveg += talalat.Karakter;
                    }
                    else
                    {
                        if(space == 6)
                        {
                            szoveg += " ";

                        }
                    }
                    kod = "";
                }
            }

            return szoveg;

        }

        private static void Feladat_05()
        {
            using (var fs = new FileStream("morze.txt", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujIdezet = new Idezet()
                        {
                            Szerzo = line[0],
                            Szoveg = line[1],
                            SzerzoKAR = Morze2Szoveg(line[0]),
                            SzovegKAR = Morze2Szoveg(line[1]),
                            Hossz = Morze2Szoveg(line[1]).Length,
                        };
                        Idezetek.Add(ujIdezet);
                    }
                }
            }
        }

        private static void Feladat_04()
        {
            Console.Write($"4. feladat: Kérek egy karaktert:");
            var karakter = Console.ReadLine().ToUpper();

            var talalat = ABC.FirstOrDefault(x => x.Karakter == karakter);

            if(talalat == null)
            {
                Console.WriteLine("Nem található a kódtáblában ilyen karakter!");
            }
            else
            {
                Console.WriteLine($"\tA {karakter} morze kódja: {talalat.Kod}");
            }
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: A morze abc {ABC.Count} db karakter kódját tartalmazza.");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("morzeabc.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split('\t');
                        var ujElem = new ABC()
                        {
                            Karakter = line[0],
                            Kod = line[1],
                        };

                        ABC.Add(ujElem);
                    }
                }
            }
        }
    }
}
