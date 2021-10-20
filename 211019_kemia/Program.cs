using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _211019_kemia
{

    class Elem
    {
        public string Ev { get; set; }
        public string Nev { get; set; }
        public string Vegyjel { get; set; }
        public int Rendszam { get; set; }
        public string Felfedeo { get; set; }
    }

    class Program
    {

        public static List<Elem> Elemek = new List<Elem>();

        public static void Feladat_01_02()
        {
            using (var fs = new FileStream("felfedezesek.csv", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split(';');

                        var ujElem = new Elem();

                        ujElem.Ev = line[0];
                        ujElem.Nev = line[1];
                        ujElem.Vegyjel = line[2];
                        ujElem.Rendszam = Convert.ToInt32(line[3]);
                        ujElem.Felfedeo = line[4];

                        Elemek.Add(ujElem);
                    }
                }
            }
        }

        public static void Feladat_03()
        {
            Console.WriteLine($"3. Feladat: Elemek száma:{Elemek.Count()}");
        }

        public static void Feladat_04()
        {
            var okorban = Elemek.Where(e => e.Ev.ToLower() == "ókor");
            Console.WriteLine($"4. Feladat: Felfedezések száma az ókorban: {okorban.Count()}");
            Console.WriteLine($"4. Feladat: Felfedezések száma az ókorban: {Elemek.Count(e => e.Ev.ToLower() == "ókor")}");
        }

        public static bool isExists(string userInput)
        {
            var abc = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            var inputArray = userInput.ToCharArray();
            var exists = false;
            foreach (var ch in inputArray)
            {
                if (!abc.Contains(ch))
                {
                    exists = false;
                    break;
                }
                else
                {
                    exists = true;
                }
            }
            return exists;
        }

        public static string Feladat_05()
        {
            Console.Write("5. Feladat: Kérek egy vegyjelet: ");
            var vegyjel = Console.ReadLine().ToLower().Trim();
            var valid = isExists(vegyjel);

            while (vegyjel.Length < 1 || vegyjel.Length > 2 || !valid)
            {
                Console.Write("5. Feladat: Kérek egy vegyjelet: ");
                vegyjel = Console.ReadLine().ToLower().Trim();
                valid = isExists(vegyjel);
            }

            // REGEX megoldással:
            var regex = Regex.IsMatch(vegyjel, "^[a-zA-z]*$");

            //while (string.IsNullOrWhiteSpace(vegyjel)|| vegyjel.Length > 2 || !Regex.IsMatch(vegyjel,"^[a-zA-z]*$"))
            //{
            //    Console.Write("5. Feladat: Kérek egy vegyjelet: ");
            //    vegyjel = Console.ReadLine().ToLower().Trim();
            //}


            return vegyjel;
        }

        public static void Feladat_06(string vegyjel)
        {
            Console.WriteLine("6. Feladat: Keresés");

            var elem = Elemek.FirstOrDefault(e => e.Vegyjel.ToLower() == vegyjel);

            if (elem is null)
            {
                Console.WriteLine("Nincs ilyen elem az adatforrásban!");
                return;
            }

            Console.WriteLine(
                $"\tAz elem vegyjele: {elem.Vegyjel}\n" +
                $"\tAz elem neve: {elem.Nev}\n" +
                $"\tRendszáma: {elem.Rendszam}\n" +
                $"\tFelfedezés éve: {elem.Ev}\n" +
                $"\tFelfedező: {elem.Felfedeo}");
        }

        public static void Feladat_07( )
        {
            var evek = Elemek
                .Where(a => a.Ev.ToLower() != "ókor")
                .Select(b=>int.Parse(b.Ev))
                .OrderBy(x=>x)
                .ToList();

            var max = 0;

            for (int i = 0; i < evek.Count()-1; i++)
            {
                var kulonbseg = evek[i + 1] - evek[i];
                max = kulonbseg > max ? kulonbseg : max;
            }

            Console.WriteLine($"7. Feladat: {max} év volt a leghosszabb időszak két elem felfedezése között.");
        }

        public static void Feladat_08()
        {
            Console.WriteLine("8. Feladat: Statisztika");

            //var stat = Elemek
            //    .FindAll(a => a.Ev.ToLower() != "ókor")
            //    .GroupBy(b => b.Ev)
            //    .Select(c => new { Ev = c.Key, elofordulas = c.Count() })
            //    .Where(d => d.elofordulas > 3)
            //    .ToList();

            //foreach (var item in stat)
            //{
            //    Console.WriteLine($"\t{item.Ev}: {item.elofordulas} db");
            //}
            
            Elemek.FindAll(a => a.Ev.ToLower() != "ókor")
                  .GroupBy(b => b.Ev)
                  .Select(c => new { Ev = c.Key, elofordulas = c.Count() })
                  .Where(d => d.elofordulas > 3)
                  .ToList()
                  .ForEach(e => Console.WriteLine($"\t{e.Ev}: {e.elofordulas} db"));

        }


        static void Main(string[] args)
        {

            Feladat_01_02();

            Feladat_03();

            Feladat_04();

            var vegyjel = Feladat_05();

            Feladat_06(vegyjel);

            Feladat_07();

            Feladat_08();

            Console.ReadLine();

        }
    }
}
