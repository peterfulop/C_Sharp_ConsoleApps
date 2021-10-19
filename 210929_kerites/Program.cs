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
            Console.Write("Adjon meg egy házszámot!");

            var input = Convert.ToInt32(Console.ReadLine());
            var aktualisTelek = Telkek.FirstOrDefault(a => a.Hazszam == input);
            
            if (aktualisTelek == null)
            {
                Console.WriteLine("A házszám még nem létezik, a telket nem adták el!");
                return;
            }

            var vizsgalandoTelkek = Telkek.Where(a => a.Hazszam == input + 2 || a.Hazszam == input - 2 || a.Hazszam == input)
                                    .Select(b => b.Kerites)
                                    .ToList();

            var firstColor = Telkek.Where(c => c.Kerites != ':' && c.Kerites != '#')
                      .GroupBy(x => x.Kerites)
                      .OrderBy(x => x.Key)
                      .Select(y => y.Key)
                      .Except(vizsgalandoTelkek)
                      .FirstOrDefault();

            Console.WriteLine($"A kerítés színe / állapota: {aktualisTelek.Kerites}");
            Console.WriteLine($"Egy lehetséges festési szín: {firstColor}");

        }

        public static void Feladat_6()
        {

            var file = "utcakep.txt";

            var paratlanOldal = Telkek.FindAll(a => a.Paros == false);

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
                        var hossz = item.Hazszam.ToString().Length+1;
                        sw.Write(item.Hazszam);
                        for (int i = 0; i <= item.Hossz - hossz; i++)
                        {
                            sw.Write(" ");
                            //if (i == 0)
                            //{
                            //    sw.Write(item.Hazszam);
                            //}
                            //else
                            //{
                            //    sw.Write(" ");
                            //}
                        }
                    }
                }
            }
        }

    }
}
