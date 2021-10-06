using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _210929_kerites
{
    class Telek
    {
        public bool Paros { get; set; }
        public int Hossz { get; set; }
        public char Kerites { get; set; }
        public int Hazszam { get; set; }
    }

    class Program
    {
        public static List<Telek> Telkek = new List<Telek>();


        static void Main(string[] args)
        {
            Feladat_1();

            Feladat_2();

            Feladat_3();

            Feladat_4();

            Feladat_5();

            Feladat_6();

            Console.ReadLine();
        }

        public static void Feladat_1()
        {
            using (var fs = new FileStream("kerites.txt", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {

                    var paros = 2;
                    var paratlan = 1;

                    while (!sr.EndOfStream)
                    {
                        var telek = new Telek();

                        string[] puffer = sr.ReadLine().Split(' ');

                        telek.Paros = Convert.ToInt32(puffer[0]) == 0 ? true : false;
                        telek.Hossz = Convert.ToInt32(puffer[1]);
                        telek.Kerites = Convert.ToChar(puffer[2]);

                        if (Convert.ToInt32(puffer[0]) == 0)
                        {
                            telek.Hazszam = paros;
                            paros += 2;
                        }
                        else
                        {
                            telek.Hazszam = paratlan;
                            paratlan += 2;
                        }

                        Telkek.Add(telek);
                    }
                }
            }
        }

        public static void Feladat_2()
        {
            Console.WriteLine($"2. feladat \nAz eladott telkek száma: {Telkek.Count}");
        }

        public static void Feladat_3()
        {
            Console.WriteLine("\n3. feladat");

            var utolsoTelek = Telkek.LastOrDefault();
            var oldal = utolsoTelek.Paros ? "páros" : "páratlan";

            Console.WriteLine($"A {oldal} oldalon adták el az utolsó telket.");
            Console.WriteLine($"Az utolsó telek házszáma: {utolsoTelek.Hazszam}");

        }

        public static void Feladat_4()
        {
            Console.WriteLine("\n4. feladat");
            var paratlanok = Telkek.FindAll(a => a.Paros == false && (a.Kerites != ':' && a.Kerites != '#'));

            var hazszam = 0;

            for (int i = 0; i < paratlanok.Count - 1; i++)
            {
                var kerites = paratlanok[i].Kerites;
                var szam = paratlanok[i].Hazszam;

                if (paratlanok[i + 1].Kerites == kerites && hazszam == 0)
                {
                    hazszam = paratlanok[i].Hazszam;
                    Console.WriteLine($"A szomszédossal egyezik a kerítés színe: {hazszam}\n");
                }
            }
        }

        public static void Feladat_5()
        {
            Console.WriteLine("5. feladat");
            Console.Write("Adjon meg egy házszámot! ");
            var input = Convert.ToInt32(Console.ReadLine());

            var parosOldal = Convert.ToInt32(input) % 2 > 0 ? false : true;

            var colors = Telkek.Where(c => c.Kerites != ':' && c.Kerites != '#')
                                .GroupBy(a => a.Kerites)
                                .Distinct()
                                .OrderBy(x => x.Key)
                                .ToList();

            var telekOldal = Telkek.FindAll(a => a.Paros == parosOldal).ToList();
            var telekIndex = telekOldal.FindIndex(a => a.Hazszam == input);

            char aktualis = telekOldal[telekIndex].Kerites;
            char szomszed_1 = telekOldal[telekIndex + 1].Kerites;
            char szomszed_2 = telekOldal[telekIndex - 1].Kerites;

            var color = colors.FirstOrDefault(v => v.Key != aktualis && v.Key != szomszed_1 && v.Key != szomszed_2).Key;

            Console.WriteLine($"A kerítés színe / állapota: {telekOldal[telekIndex].Kerites}");
            Console.WriteLine($"Egy lehetséges festési szín: {color}");

        }

        public static void Feladat_6()
        {

            var file = "utcakep.txt";
            var paratlanOldal = Telkek.FindAll(a => a.Paros == false).ToList();

            using (var fs = new FileStream(file, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var item in paratlanOldal)
                    {
                        for (int i = 0; i < item.Hossz; i++)
                        {
                            sw.Write(item.Kerites);
                        }
                    }

                    sw.WriteLine();

                    foreach (var item in paratlanOldal)
                    {
                        var hossz = Convert.ToInt32(item.Hazszam.ToString().Length);
                        for (int i = 0; i < (item.Hossz - hossz) + 1; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write(item.Hazszam);
                            }
                            else
                            {
                                sw.Write(" ");
                            }
                        }
                    }
                }
            }
        }

    }
}
