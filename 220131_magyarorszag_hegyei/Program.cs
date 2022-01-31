using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220131_magyarorszag_hegyei
{
    class Hegy
    {
        public string HegycsucsNeve { get; set; }
        public string Hegyseg { get; set; }
        public int Magassag { get; set; }
        public double Lab { get; set; }
    }
    internal class Program
    {
        public static List<Hegy> Hegyek = new List<Hegy>();

        static void Main(string[] args)
        {
            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_06();
            Feladat_07();
            Feladat_08();
            Feladat_09();

            Console.ReadLine();
        }

        private static void Feladat_09()
        {
            Console.WriteLine("9. feladat: bukk-videk.txt");

            using (var fs = new FileStream("bukk-videk.txt",FileMode.Create))
            {
                using (var sw = new StreamWriter(fs,Encoding.UTF8))
                {
                    sw.WriteLine("Hegycsúcs neve;Magasság láb");

                    foreach (var h in Hegyek.Where(x=>x.Hegyseg =="Bükk-vidék"))
                    {
                        var magassag = h.Lab.ToString("0.0").Replace(',', '.');
                        var m = magassag.Split('.');
                        var formatted = m[1] == "0" ? m[0] : magassag;
                        sw.WriteLine($"{h.HegycsucsNeve};{formatted}");
                    }
                }
            }
        }

        private static void Feladat_08()
        {
            Console.WriteLine("8. feladat: Hegység statisztika: ");
            Hegyek.GroupBy(x=>x.Hegyseg)
                .Select(x => new {Hegyseg=x.Key, Count = x.Count()})
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Hegyseg} - {x.Count} db"));
        }

        private static void Feladat_07()
        {
            var lab = Hegyek.Count(x=>x.Lab > 3000);
            Console.WriteLine($"7. feladat: 3000 lábnál magasabb hegycsúcsok száma: {lab}");
        }


        private static void Feladat_06()
        {
            Console.Write("6. feladat: Kérek egy magasságot: ");
            var magassag = int.Parse(Console.ReadLine());
            var res = Hegyek.Exists(x=>x.Magassag > magassag && x.Hegyseg =="Börzsöny") ? "Van" : "Nincs";
            Console.WriteLine($"\t{res} {magassag}m-nél magasabb hegycsúcs a Börzsönyben!");
        }

        private static void Feladat_05()
        {
            Console.WriteLine("5. feladat: A legmagasabb hegycsúcs adatai:");
            var top= Hegyek.OrderByDescending(x => x.Magassag)
                .First();
            Console.WriteLine($"\tNév: {top.HegycsucsNeve}\n\tHegység: {top.Hegyseg}\n\tMagasság: {top.Magassag} m");
        }

        private static void Feladat_04()
        {
            Console.WriteLine($"4. feladat: Hegycsúcsok átlagos magassága: {Hegyek.Average(x=>x.Magassag):0.00} m");
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: Hegycsúcsok száma: {Hegyek.Count} db");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("hegyekMo.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8) )
                {
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujHegy = new Hegy()
                        {
                            HegycsucsNeve = line[0],
                            Hegyseg = line[1],
                            Magassag = int.Parse(line[2]),
                            Lab = Convert.ToDouble(line[2]) * 3.280839895
                        };

                        Hegyek.Add(ujHegy);
                    }
                }
            }
        }
    }
}
