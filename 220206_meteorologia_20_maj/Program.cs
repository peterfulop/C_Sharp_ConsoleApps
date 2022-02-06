using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220206_meteorologia_20_maj
{
    class Tavirat
    {
        public string Telepules { get; set; }
        public string Ido { get; set; }
        public string Ora { get; set; }
        public string Perc { get; set; }
        public string SzelIrany { get; set; }
        public int SzelErosseg { get; set; }
        public int Homerseklet { get; set; }
    }


    internal class Program {

        public static List<Tavirat> Taviratok = new List<Tavirat>();
    
        static void Main(string[] args)
        {

            Feladat_01();
            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_06();

            Console.ReadLine();
        }

        private static void Feladat_06()
        {
            Console.WriteLine("6. feladat");
            var export = Taviratok.GroupBy(x => x.Telepules)
                .Select(x => new { telepules = x.Key,
                   meresek = x.Where(y=>y.Telepules == x.Key)
                })
                .ToList();

            foreach (var t in export)
            {
                var fileName = $"{t.telepules}.txt";
                using (var fs = new FileStream(fileName, FileMode.Create))
                {
                    using (var sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine(t.telepules);
                        foreach (var m in t.meresek)
                        {
                            var szelero = "";
                            for (int i = 0; i < m.SzelErosseg; i++)
                            {
                                szelero += "#";
                            }
                            sw.WriteLine($"{m.Ora}:{m.Perc} {szelero}");
                        }
                    }
                }
            }
            Console.WriteLine("A fileok elkészültek");
        }

        private static void Feladat_05()
        {
            Console.WriteLine("5. feladat");

            var atlag = Taviratok.GroupBy(x => x.Telepules)
                                 .Select(x => new
                                 {
                                     telepules = x.Key,
                                     count = x.Count(),
                                     kozephomerseklet = x.Where(y => y.Ora == "01" || y.Ora == "07" || y.Ora == "13" || y.Ora == "19")
                                     .Select(y => y.Ora).Distinct().Count()==4
                                     ?
                                     $"Középhőmérséklet: {x.Average(y => y.Homerseklet):0}" : "NA",
                                     ingadozas = x.Max(y => y.Homerseklet) - x.Min(y => y.Homerseklet)
                                 })
                                 .ToList();

            atlag.ForEach(x => Console.WriteLine($"{x.telepules} {x.kozephomerseklet}; Hőmérséklet-ingadozás: {x.ingadozas}"));
        }

        private static void Feladat_04()
        {
            Console.WriteLine("4. feladat");
            var szelcsend = Taviratok.Where(x => x.SzelErosseg == 0 && x.SzelIrany == "000")
                                     .ToList();

            if(szelcsend.Count == 0)
            {
                Console.WriteLine("Nem volt szélcsend a mérés idején");
                return;
            }
            szelcsend.ForEach(x => Console.WriteLine($"{x.Telepules} {x.Ora}:{x.Perc}"));
        }

        private static void Feladat_03()
        {
            Console.WriteLine("3. feladat");
            var ordered = Taviratok.OrderBy(x => x.Homerseklet).ToList();

            var first = ordered.First();
            var last = ordered.Last();

            Console.WriteLine($"A legalacsonyabb hőmérséklet: {first.Telepules} {first.Ora}:{first.Perc} {first.Homerseklet} fok.");
            Console.WriteLine($"A legmagasabb hőmérséklet: {last.Telepules} {last.Ora}:{last.Perc} {last.Homerseklet} fok.");
        }

        private static void Feladat_02()
        {
            Console.Write("2. feladat\nAdja meg egy település kódját! Település: ");
            var varos = Console.ReadLine();

            var filtered = Taviratok.Where(x => x.Telepules.ToLower() == varos.ToLower())
                                  .OrderByDescending(x => x.Ido)
                                  .First();
    
            Console.WriteLine($"At utolsó mérési adat a megadott településről {filtered.Ora}:{filtered.Perc}-kor érkezett.");

        }

        private static void Feladat_01()
        {
            using (var fs  =new FileStream("tavirathu13.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(' ');
                        var ujTavirat = new Tavirat()
                        {
                            Telepules = line[0],
                            Ido = line[1],
                            SzelIrany = $"{line[2][0]}{line[2][1]}{line[2][2]}",
                            SzelErosseg = int.Parse($"{line[2][3]}{line[2][4]}"),
                            Homerseklet = int.Parse(line[3]),
                            Ora = $"{line[1][0]}{line[1][1]}",
                            Perc = $"{line[1][2]}{line[1][3]}",
                        };
                        Taviratok.Add(ujTavirat);
                    }
                }
            }
        }
    }
}
