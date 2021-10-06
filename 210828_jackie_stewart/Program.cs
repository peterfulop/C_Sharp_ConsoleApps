using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _210828_jackie_stewart
{
    class Jackie
    {
        public int Year { get; set; }
        public int Races { get; set; }
        public int Wins { get; set; }
        public int Podiums { get; set; }
        public int Poles { get; set; }
        public int Fastests { get; set; }
        public string Evtized { get; set; }

    }

    class Program
    {

        public static List<Jackie> StatList = new List<Jackie>();

        static void Main(string[] args)
        {
            Feladat_01();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_06();

            Console.ReadLine();
        }

        private static void Feladat_01()
        {
            using (var fs = new FileStream("jackie.txt", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string[] puffer = sr.ReadLine().Split('\t');
                        var newLine = new Jackie
                        {
                            Year = Convert.ToInt32(puffer[0]),
                            Races = Convert.ToInt32(puffer[1]),
                            Wins = Convert.ToInt32(puffer[2]),
                            Podiums = Convert.ToInt32(puffer[3]),
                            Poles = Convert.ToInt32(puffer[4]),
                            Fastests = Convert.ToInt32(puffer[5]),
                            Evtized = string.Concat(puffer[0][2], "0")
                        };
                        StatList.Add(newLine);
                    }
                }
            }
        }
        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: {StatList.Count}");
        }
        private static void Feladat_04()
        {
            var topRaces = StatList.OrderByDescending(a => a.Races).ToList();
            Console.WriteLine($"4. feladat: {topRaces[0].Year}");
        }
        private static void Feladat_05()
        {
            var topRaces = StatList.GroupBy(a => a.Evtized)
                .Select(s => new { Evtized = s.Key, Osszesen = s.Sum(a => a.Wins) });

            Console.WriteLine($"5. feladat:");
            foreach (var race in topRaces)
            {
                Console.WriteLine($"{race.Evtized}-es évek: {race.Osszesen} megnyert verseny");
            }
        }
        private static void Feladat_06()
        {
            using (var fs = new FileStream("jackie.html", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine("<!doctype html>");
                    sw.WriteLine("<hmtl>");
                    sw.WriteLine("<head></head>");
                    sw.WriteLine("<style>td {border: 1px solid black;}</style>");
                    sw.WriteLine("<body>");
                    sw.WriteLine("<h1>Jackie Stewart</h1>");
                    sw.WriteLine("<table>");
                    foreach (var item in StatList)
                    {
                        sw.WriteLine($"<tr><td>{item.Year}</td><td>{item.Races}</td><td>{item.Wins}</td><tr>");
                    }
                    sw.WriteLine("</table>");
                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");
                }
            }
        }
    }
}
