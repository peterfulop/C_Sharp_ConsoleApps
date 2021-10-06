using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _210922_hianyzasok
{
    class Log
    {
        public string Date { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

    }

    class Program
    {
        public static List<Log> Logs = new List<Log>();
      
        static void Main(string[] args)
        {

            Feladat_1();

            Feladat_2();

            Feladat_3();

            Feladat_4_5();

            Feladat_6();

            Feladat_7();

            Console.ReadLine();

        }

        public static void Feladat_1()
        {
            using (var fs = new FileStream("naplo.txt", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {

                    string dateStamp = "";

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        Log log = new Log();

                        if (line.StartsWith("#"))
                        {
                            dateStamp = line.Trim('#').Trim();
                        }
                        else
                        {
                            log.Month = Convert.ToInt32(dateStamp.Remove(dateStamp.Length - 3));
                            log.Day = Convert.ToInt32(dateStamp.Substring(dateStamp.Length - 2));
                            log.Name = line.Remove(line.Length - 8).Trim();
                            log.Info = line.Substring(line.Length - 7).Trim();
                            Logs.Add(log);
                        }
                    }
                }
            }
        }

        public static void Feladat_2()
        {
            Console.WriteLine($"2. feladat\nA naplóban {Logs.Count} bejegyzés van.\n");
        }

        public static void Feladat_3()
        {

            var igazolt = 0;
            var igazolatlan = 0;

            foreach (var log in Logs)
            {
                for (int i = 0; i < log.Info.Length; i++)
                {
                    if (log.Info[i] == 'X') igazolt++;
                    else if (log.Info[i] == 'I') igazolatlan++;
                }
            }

            Console.WriteLine($"3. feladat\nAz igazolt hiányzások száma {igazolt}, az igazolatlanoké {igazolatlan} óra.\n");
        }

        public static void Feladat_4_5()
        {
            Console.WriteLine("5. feladat");

            string userInputMonth;
            string userInputDay;

            do
            {
                Console.Write("A hónap sorszáma= ");
                userInputMonth = Console.ReadLine();

            } while (userInputMonth == "");

            do
            {
                Console.Write("A nap sorszáma= ");
                userInputDay = Console.ReadLine();

            } while (userInputDay == "");


            hetnapja(Convert.ToInt32(userInputMonth), Convert.ToInt32(userInputDay));

        }

        public static void Feladat_6()
        {

            Console.WriteLine("6. feladat");
            string userInputDay;
            string userInputClass;

            do
            {
                Console.Write("A nap neve= ");
                userInputDay = Console.ReadLine();

            } while (userInputDay == "");

            do
            {
                Console.Write("Az óra sorszáma= ");
                userInputClass = Console.ReadLine();

            } while (userInputClass == "");

            var counter = 0;

            foreach (var log in Logs)
            {
                var day = hetnapja(log.Month, log.Day, false);
                var cls = Convert.ToInt32(userInputClass) - 1;

                if (day == userInputDay)
                {
                    if (log.Info[cls] == 'I' || log.Info[cls] == 'X')
                    {
                        counter++;
                    }
                }
            }

            Console.WriteLine($"Ekkor összesen {counter} óra hiányzás történt.\n");


        }

        public static void Feladat_7()
        {
            Console.WriteLine("7. feladat");

            var missList = new List<string>();

            foreach (var log in Logs)
            {
                for (int i = 0; i < log.Info.Length; i++)
                {
                    if (log.Info[i] == 'I' || log.Info[i] == 'X')
                    {
                        missList.Add(log.Name);
                    }
                }
            }

            var namesList = missList.GroupBy(name => name)
                                    .Select(g => new { Name = g.Key, Count = g.Count() })
                                    .OrderByDescending(x => x.Count);


            var TopMiss = namesList.Max(x => x.Count);

            Console.Write($"A legtöbbet hiányzó tanulók: ");

            foreach (var stundet in namesList)
            {
                if (stundet.Count == TopMiss)
                {
                    Console.Write($"{stundet.Name} ");
                }
            }

        }

        public static string hetnapja(int month, int day, bool cw = true)
        {
            var year = 2018;
            var napNev = new List<string> { "vasarnap", "hetfo", "kedd", "szerda", "csutortok", "pentek", "szombat" };

            DateTime dt = new DateTime(year, month, day);
            int dayIndex = (int)dt.DayOfWeek;

            if (cw) Console.WriteLine($"Azon a napon {napNev[dayIndex]} volt.\n");

            return napNev[dayIndex];

        }

    }
}
