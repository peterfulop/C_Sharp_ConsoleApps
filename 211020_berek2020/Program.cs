using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _211020_berek2020
{

    class Dolgozo
    {
        public string Nev { get; set; }
        public string Nem { get; set; }
        public string Reszleg { get; set; }
        public int Belepes { get; set; }
        public int Ber { get; set; }
    }

    class Program
    {
        public static List<Dolgozo> Dolgozok = new List<Dolgozo>();

        public static void Feladat_01_02()
        {
            using (var fs = new FileStream("berek2020.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujDolgozo = new Dolgozo
                        {
                            Nev = line[0],
                            Nem = line[1],
                            Reszleg = line[2],
                            Belepes = Convert.ToInt32(line[3]),
                            Ber = Convert.ToInt32(line[4])
                        };

                        Dolgozok.Add(ujDolgozo);
                    }
                }
            }
        }

        public static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: Dolgozók száma: {Dolgozok.Count()} fő");
        }

        public static void Feladat_04()
        {
            var atlag = Dolgozok.Average(x => x.Ber)/1000;
            Console.WriteLine($"4. feladat: Bérek átlaga: {atlag:00.0} eFt");
        }

        public static string Feladat_05()
        {
            Console.Write($"5. feladat: Kérem egy részleg nevét: ");
            return Console.ReadLine().ToLower();
        }

        public static void Feladat_06(string reszleg)
        {
            if (!Dolgozok.Exists(x => x.Reszleg == reszleg))
            {
                Console.WriteLine("6. feladat: A megadott részleg nem létezik a cégnél!");
                return;
            }
            var topKereso = Dolgozok
                    .Where(a => a.Reszleg.ToLower() == reszleg)
                    .OrderByDescending(b => b.Ber)
                    .First();

            Console.WriteLine($"6.feladat: A legtöbbet kereső dolgozó a megadott részlegen\n" +
                    $"\tNév: {topKereso.Nev}\n" +
                    $"\tNem: {topKereso.Nem}\n" +
                    $"\tBelépés: {topKereso.Belepes}\n" +
                    $"\tBér: {topKereso.Ber:### ###} Forint");
        }

        public static void Feladat_07()
        {
            Console.WriteLine($"7. feladat: Statisztika: ");
            Dolgozok.GroupBy(a => a.Reszleg)
                    .ToList()
                    .ForEach(x => Console.WriteLine($"\t{x.Key} - {x.Count()} fő"));
        }

        static void Main(string[] args)
        {
            Feladat_01_02();

            Feladat_03();

            Feladat_04();

            var reszleg = Feladat_05();

            Feladat_06(reszleg);

            Feladat_07();

            Console.ReadLine();

        }
    }
}
