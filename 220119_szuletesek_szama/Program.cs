using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220119_szuletesek_szama
{
    class SzemelyiSzam
    {
        public string M { get; set; }
        public string EEHHNN { get; set; }
        public string SSSK { get; set; }
        public string SSS { get; set; }
        public string K { get; set; }
        public string Nem { get; set; }
        public DateTime Szuletes { get; set; }
        public bool Valid { get; set; }

    }
    internal class Program
    {
        public static List<SzemelyiSzam> SzemelyeiSzamok = new List<SzemelyiSzam>();

        static void Main(string[] args)
        {

            Feladat_02();
            
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
            Console.WriteLine($"9. feladat: Statisztika");
            
            SzemelyeiSzamok.GroupBy(x=>x.Szuletes.Year)
                .Select(x => new {Ev = x.Key, Count = x.Count()})
                .ToList()
                .ForEach(x => Console.WriteLine($"\t{x.Ev} - {x.Count} fő"));
        }

        private static void Feladat_08()
        {
            var szuletett = SzemelyeiSzamok.Exists(x=>x.Szuletes.Month == 2 && x.Szuletes.Day == 14) ? "született" : "nem született";
            Console.WriteLine($"8. feladat: Szökőnapon {szuletett} baba!");
        }

        private static void Feladat_07()
        {
            Console.WriteLine($"7. feladat: Vizsgált időszak: {SzemelyeiSzamok.Min(x=>x.Szuletes.Year)} - {SzemelyeiSzamok.Max(x => x.Szuletes.Year)}");
        }

        private static void Feladat_06()
        {
            Console.WriteLine($"6. feladat: Fiúk száma: {SzemelyeiSzamok.Count(x=>x.Nem=="f")}");
        }

        private static void Feladat_05()
        {
            Console.WriteLine($"5. feladat: Vas megyében a vizsgált évek alatt {SzemelyeiSzamok.Count} csecsemő született.");
        }

        private static void Feladat_04()
        {
            Console.WriteLine("4. feladat: Ellenőrzés");

            foreach (var item in SzemelyeiSzamok)
            {
                var azonosito = $"{item.M}{item.EEHHNN}{item.SSSK}";
                var valid = CdvEll(azonosito);

                if (!valid)
                {
                    Console.WriteLine($"\tHibás a {item.M}-{item.EEHHNN}-{item.SSSK} személyi azonosító!");
                    item.Valid = false;
                }
                else
                {
                    item.Valid = true;
                }
            }

            SzemelyeiSzamok.RemoveAll(x => !x.Valid);
       }

        private static bool CdvEll(string azonosito)
        {
            var k10 = azonosito[10];
            var k11 = 0;
            var szorzo = 10;

            for (int i = 0; i < azonosito.Length-1; i++)
            {
              k11 += Convert.ToInt32(azonosito[i]) * szorzo;
              szorzo--;
            }

            return $"{k11 % 11}" == $"{k10}"; 
        }

        private static void Feladat_02()
        {
            Console.WriteLine("2. feladat: Adatok beolvasása, tárolása");

            using (var fs = new FileStream("vas.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split('-');

                        var nem = line[0] == "1" || line[0] == "3" ? "f" : "n";

                        var evStart = line[0] == "1" || line[0] == "2" ? "19" : "20";
                        var evEnd = $"{line[1][0]}{line[1][1]}";
                        var ev = $"{evStart}{evEnd}";
                        var honap = $"{line[1][2]}{line[1][3]}";
                        var nap = $"{line[1][4]}{line[1][5]}";
                        var szuletes = $"{ev}-{honap}-{nap}";

                        var ujSzemely = new SzemelyiSzam()
                        {

                            M = line[0],
                            EEHHNN = line[1],
                            SSSK = line[2],
                            SSS = $"{line[2][0]}{line[2][1]}{line[2][2]}",
                            K = $"{line[2][3]}",
                            Nem = nem,
                            Szuletes = Convert.ToDateTime(szuletes)
                        };

                        SzemelyeiSzamok.Add(ujSzemely);
                    }
                }
            }
            //foreach (var i in SzemelyeiSzamok)
            //{
            //    Console.WriteLine($"{i.Azonosito}");
            //}
        }
    }
}
