using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220118_mukorcsolya
{
    class Versenyzo
    {
        public string Nev { get; set; }
        public string Orszag { get; set; }
        public double Technikai { get; set; }
        public double Komponens { get; set; }
        public double Levonas { get; set; }
        public double Pontszam { get; set; }

        public Versenyzo(string nev, string orszag, double technikai, double komponens, double levonas)
        {
            this.Nev = nev;
            this.Orszag = orszag;
            this.Technikai = technikai;
            this.Komponens = komponens;
            this.Levonas = levonas;
            this.Pontszam = technikai + komponens - levonas;
        }
    }

    internal class Program
    {
        public static List<Versenyzo> Rovid = new List<Versenyzo>();
        public static List<Versenyzo> Donto = new List<Versenyzo>();
        static void Main(string[] args)
        {
           
            Feladat_01();

            Feladat_02();

            Feladat_03();

            Feladat_05();

            Feladat_07();

            Feladat_08();

            Console.ReadLine();
        }

        private static void Feladat_08()
        {
            var all = Rovid.Concat(Donto)
                .GroupBy(x=>x.Nev)
                .Select(x=>new {Nev = x.Key,
                    Orszag = x.Select(y=>y.Orszag).First(),
                    Pontszam = x.Sum(y=>y.Pontszam)})
                .OrderByDescending(x=>x.Pontszam)
                .ToList();

            using (var fs = new FileStream("vegeredmeny.csv",FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    var helyezes = 1;

                    foreach (var item in all)
                    {
                        sw.WriteLine($"{helyezes};{item.Nev};{item.Orszag};{item.Pontszam}");
                        helyezes++;
                    }
                }
            }
        }

        private static void Feladat_07()
        {
            Console.WriteLine($"7. feladat");

            Donto.GroupBy(x => x.Orszag)
                .Select(x => new {Orszag = x.Key, Count = x.Count()})
                .Where(x=>x.Count > 1)
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Orszag}: {x.Count} versenyző"));
        }

        private static void Feladat_05()
        {
            Console.Write($"5. feladat\n\tKérem a versenyző nevét: ");
            var nev = Console.ReadLine();
            
            var res = Rovid.FirstOrDefault(x=>x.Nev == nev);
            if(res is null)
            {
                Console.WriteLine("\tIlyen nevű induló nem volt");
                return;
            }

            ÖsszPontszam(nev);
        }

        private static void ÖsszPontszam(string versenyzoNeve)
        {
            var donto = Donto.FirstOrDefault(x=>x.Nev == versenyzoNeve);
            var rovid = Rovid.FirstOrDefault(x => x.Nev == versenyzoNeve);

            double pontok = donto != null ? donto.Pontszam + rovid.Pontszam : rovid.Pontszam;

            Console.WriteLine($"6. feladat\n\tA versenyző összpontszáma: {pontok.ToString().Replace(',','.')}");
        }

        private static void Feladat_03()
        {
            var hun = Donto.Count(x => x.Orszag.Contains("HUN"));
            var msg = hun > 0 ? "bejutott" : "nem jutott be";
            Console.WriteLine($"3. feladat\n\tA magyar versenyző {msg} a kűrbe");
        }

        private static void Feladat_02()
        {
            Console.WriteLine($"2. feladat\n\tA rövidprogramban {Rovid.Count} induló volt");
        }

        private static void ReadFile(string fileName, List<Versenyzo> MyList)
        {
            using (var fs = new FileStream(fileName,FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var technikai = Convert.ToDouble(line[2].Replace('.', ','));
                        var komponens = Convert.ToDouble(line[3].Replace('.', ','));
                        var levonas = Convert.ToDouble(line[4].Replace('.', ','));

                        var ujVersenyzo = new Versenyzo(line[0], line[1], technikai, komponens, levonas);

                        MyList.Add(ujVersenyzo);
                    }
                }
            }
        }

        private static void Feladat_01()
        {
            ReadFile("rovidprogram.csv", Rovid);
            ReadFile("donto.csv", Donto);
        }
    }
}
