using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _211203_helyjegy
{
      
    class Jegy
    {
        public int Id { get; set; }
        public int Ules { get; set; }
        public double Start { get; set; }
        public double Stop { get; set; }
        public double Km { get; set; }
        public int Dij { get; set; }
    }
    class Program
    {
        public static List<Jegy> Jegyek = new List<Jegy>() { };
        public static int JegyekSzama { get; set; }
        public static int VonalHossza { get; set; }
        public static int Tarifa { get; set; }

        static void Main(string[] args)
        {
            Feladat_01();
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
            Console.Write("Kérlek adj meg egy km-t:");
            var km = Console.ReadLine();

            while (!int.TryParse(km,out int a) || int.Parse(km)> VonalHossza)
            {
                Console.Write("Kérlek adj meg egy km-t:");
                km = Console.ReadLine();
            }

            var list = Jegyek.Where(x => x.Start < int.Parse(km) && x.Stop > int.Parse(km)).ToList();

            using (var fs = new FileStream("kihol.txt",FileMode.Create))
            {
                using (var sw = new StreamWriter(fs,Encoding.UTF8))
                {
                    for (int i = 0; i < 48; i++)
                    {
                        var utas = list.FirstOrDefault(x => x.Ules == i);

                        var data = "";

                        if (utas is null)
                        {
                            data = "üres";
                        }
                        else
                        {
                            data = $"{utas.Id}. utas";
                        }

                        sw.WriteLine($"{i + 1}. ülés: {data}");
                    }
                }
            }
            Console.WriteLine($"File kiírása kész!");
        }

        private static void Feladat_06()
        {
            var egyediStart = Jegyek.Select(x=>x.Start).Distinct();
            Console.WriteLine($"6. feladat: Megállóhelyek száma: {egyediStart.Count()}");
        }

        private static void Feladat_05()
        {
            var megallok = Jegyek.Select(x => x.Start).Distinct().OrderBy(x=>x).ToList();
            var utolsoElotti = megallok[megallok.Count()-2];

            var leszallo = Jegyek.Where(x => x.Stop == utolsoElotti).ToList();
            var felszallo = Jegyek.Where(x => x.Start == utolsoElotti).ToList();

            Console.WriteLine($"5. feladat: Felszállók száma: {felszallo.Count()}, leszállók száma: {leszallo.Count()}");
        }

        private static void Feladat_04()
        {
            var bevetel = Jegyek.Sum(x => x.Dij);
            Console.WriteLine($"\n4. feladat: A társaság bevétele: {bevetel} Ft");
        }

        private static void Feladat_03()
        {
            var vegig = Jegyek.FindAll(x => x.Start == 0 && x.Stop == VonalHossza);
            Console.Write($"3. feladat: Akik a vonalat végig utazták: ");
            vegig.ForEach(x => Console.Write($"{x.Id} "));
        }

        private static void Feladat_02()
        {
            Console.WriteLine($"2. feladat: Utolsó utas ülése:{Jegyek.Last().Ules}, megtett km:{Jegyek.Last().Km}");
        }

        private static void Feladat_01()
        {
            using (var fs = new FileStream("eladott.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    var data = sr.ReadLine().Split(' ');
                    JegyekSzama = int.Parse(data[0]);
                    VonalHossza = int.Parse(data[1]);
                    Tarifa = Convert.ToInt32(data[2]);

                    var counter = 1;

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(' ');
                        var kmPer = Math.Ceiling((Convert.ToDouble(line[2]) - Convert.ToDouble(line[1])) /10);

                        var jegy = new Jegy()
                        {
                            Id = counter,
                            Ules = int.Parse(line[0]),
                            Start = int.Parse(line[1]),
                            Stop = int.Parse(line[2]),
                            Km = Convert.ToDouble(line[2]) - Convert.ToDouble(line[1]),
                            Dij = RoundPrice((int)kmPer * Tarifa)
                        };

                        Jegyek.Add(jegy);

                        counter++;

                    }
                }
            }
        }

        private static int RoundPrice(int kmAr)
        {
            switch (kmAr.ToString().Last())
            {
                case '1':
                     kmAr -= 1;
                    break;
                case '2':
                     kmAr -= 2;
                    break;
                case '3':
                     kmAr += 2;
                    break;
                case '4':
                     kmAr += 1;
                    break;
                case '5':
                    kmAr+=0;
                    break;
                case '6':
                     kmAr -= 1;
                    break;
                case '7':
                     kmAr -= 2;
                    break;
                case '8':
                     kmAr += 2;
                    break;
                case '9':
                     kmAr += 1;
                    break;
            }
            return kmAr;
        }
    }
}
