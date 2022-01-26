using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220126_koridok
{

    class Verseny
    {
        public string Csapat { get; set; }
        public string Versenyzo { get; set; }
        public int Eletkor { get; set; }
        public string Palya { get; set; }
        public DateTime Korido { get; set; }
        public int Kor { get; set; }

    }

    internal class Program
    {
        public static List<Verseny> VersenyLista = new List<Verseny>();
        static void Main(string[] args)
        {
            Feladat_02();

            Feladat_03();

            Feladat_04();

            var versenyzo = Feladat_05();

            Feladat_06(versenyzo);

            Console.ReadLine();
        }

        private static void Feladat_06(string versenyzo)
        {
            if (!VersenyLista.Exists(x => x.Versenyzo == versenyzo))
            {
                Console.WriteLine("6. feladat: Nincs ilyen versenyző az állományban!");
                return;
            }
            var best = VersenyLista.Where(x=>x.Versenyzo == versenyzo)
                .OrderBy(x=>x.Korido)
                .First();

            Console.WriteLine($"6. feladat: {best.Versenyzo}, {best.Korido.ToLongTimeString()}");
        }

        private static string Feladat_05()
        {
            Console.WriteLine("Kérem egy versenyző nevét:");
            return Console.ReadLine();
        }

        private static void Feladat_04()
        {
            var korido = VersenyLista.First(x => x.Versenyzo == "Fürge Ferenc" &&
            x.Palya == "Gran Prix Circuit" &&
            x.Kor == 3)
                .Korido;

            Console.WriteLine($"4. feladat: {korido.Hour*60*60+ korido.Minute*60 + korido.Second} másodperc");
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: {VersenyLista.Count}");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("autoverseny.csv",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var verseny = new Verseny()
                        {
                            Csapat = line[0],
                            Versenyzo = line[1],
                            Eletkor = int.Parse(line[2]),
                            Palya = line[3],
                            Korido = Convert.ToDateTime(line[4]),
                            Kor = int.Parse(line[5]),
                        };

                        VersenyLista.Add(verseny);
                    }
                }
            }
        }
    }
}
