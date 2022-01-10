using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220103_fuvar
{
    class Fuvar
    {
        public int taxi_id { get; set; }
        public string indulas { get; set; }
        public double idotartam { get; set; }
        public double tavolsag { get; set; }
        public double viteldij { get; set; }
        public double borravalo { get; set; }
        public string fizetes_modja { get; set; }


    }
    internal class Program
    {
        public static List<Fuvar> Fuvarok = new List<Fuvar>();
        public static string Header { get; set; }
        static void Main(string[] args)
        {

            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_06();
            Feladat_07();
            Feladat_08();

            Console.ReadLine();
        }

        private static void Feladat_08()
        {
            var hibas = Fuvarok.Where(x => x.idotartam > 0 && x.viteldij > 0 && x.tavolsag == 0).ToList().OrderBy(x => x.indulas);

            using (var fs = new FileStream("Hibak.txt", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(Header);

                    foreach (var h in hibas)
                    {
                        var line = $"{h.taxi_id};{h.indulas};{h.idotartam};{h.tavolsag};{h.viteldij};{h.borravalo};{h.fizetes_modja}";
                        sw.WriteLine(line);
                    }

                }
            }

            Console.WriteLine($"8. feladat: hibak.txt");
        }

        private static void Feladat_07()
        {
            var leghosszabb = Fuvarok.OrderByDescending(x => x.idotartam).First();

            Console.WriteLine($"6. feladat: Leghosszabb fuvar:");
            Console.WriteLine($"\tFuvar hossza: {leghosszabb.idotartam} másodperc");
            Console.WriteLine($"\tTaxi azonosító: {leghosszabb.taxi_id}");
            Console.WriteLine($"\tMegtett távolság:{leghosszabb.tavolsag * 1,6:#.##} km");
            Console.WriteLine($"\tViteldíj: {leghosszabb.viteldij}$");

        }

        private static void Feladat_06()
        {
            var osszesUt = Fuvarok.Sum(x => x.tavolsag) * 1.6;
            Console.WriteLine($"6. feladat: {osszesUt:#.##} km");
        }

        private static void Feladat_05()
        {
            var fizetesek = Fuvarok.GroupBy(x => x.fizetes_modja)
                .Select(x => new { fizetesiMod = x.Key, count = x.Count() });
            Console.WriteLine("5. feladat:");
            foreach (var m in fizetesek)
            {
                Console.WriteLine($"\t{m.fizetesiMod}: {m.count} fuvar");
            }
        }

        private static void Feladat_04()
        {
            var taxis = Fuvarok.Where(x => x.taxi_id == 6185);
            Console.WriteLine($"4. feladat: {taxis.Count()} fuvar alatt: {taxis.Sum(x => x.borravalo) + taxis.Sum(x => x.viteldij)}$");
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: {Fuvarok.Count} fuvar");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("fuvar.csv", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    Header = sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');

                        var newFuvar = new Fuvar()
                        {
                            taxi_id = int.Parse(line[0]),
                            indulas = line[1],
                            idotartam = Convert.ToDouble(line[2]),
                            tavolsag = Convert.ToDouble(line[3]),
                            viteldij = Convert.ToDouble(line[4]),
                            borravalo = Convert.ToDouble(line[5]),
                            fizetes_modja = line[6]
                        };

                        Fuvarok.Add(newFuvar);

                    }
                }
            }

        }
    }

}
