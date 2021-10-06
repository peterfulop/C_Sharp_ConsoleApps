using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _210929_tarsalgo
{
    class Tarsalgo
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int PersonId { get; set; }
        public string Status { get; set; }
        public bool IsInside { get; set; }

        public Tarsalgo(int hour, int minute, int personId, string status, bool inside)
        {
            this.Hour = hour;
            this.Minute = minute;
            this.PersonId = personId;
            this.Status = status;
            this.IsInside = inside;
        }
    }

    class Program
    {
        public static List<Tarsalgo> Tarsalgo = new List<Tarsalgo>();

        static void Main(string[] args)
        {

            Feladat_1();

            Feladat_2();

            Feladat_3();

            Feladat_4();

            Feladat_5();

            Feladat_6_7_8();

            Console.ReadLine();

        }

        public static void Feladat_1()
        {
            using (var fs = new FileStream("ajto.txt", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {

                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split(' ');

                        var newLog = new Tarsalgo(
                            Convert.ToInt32(line[0]),
                            Convert.ToInt32(line[1]),
                            Convert.ToInt32(line[2]),
                            line[3],
                            line[3] == "be" ? true : false
                            );

                        Tarsalgo.Add(newLog);
                    }
                }
            }
        }
        public static void Feladat_2()
        {
            var firstIn = Tarsalgo.FirstOrDefault(a => a.IsInside).PersonId;
            var lastOut = Tarsalgo.LastOrDefault(a => !a.IsInside).PersonId;

            Console.WriteLine("2.feladat");
            Console.WriteLine($"Az első belépő: {firstIn}");
            Console.WriteLine($"Az utolsó belépő: {lastOut}");
        }
        public static void Feladat_3()
        {
            var persons = Tarsalgo.GroupBy(a => a.PersonId)
                                  .Select(g => new { PersonId = g.Key, Count = g.Count() })
                                  .OrderBy(c => c.PersonId);

            using (var fs = new FileStream("athaladas.txt", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var person in persons)
                    {
                        sw.WriteLine($"{person.PersonId} {person.Count}");
                    }
                }
            }
        }
        public static void Feladat_4()
        {
            Console.WriteLine("\n4. feladat");

            var persons = Tarsalgo.GroupBy(a => a.PersonId)
                      .Select(g => new { PersonId = g.Key, Count = g.Count() })
                      .OrderBy(c => c.PersonId)
                      .Where(p => p.Count % 2 != 0);

            Console.Write("A végén a társalgóban voltak:");

            foreach (var person in persons)
            {
                Console.Write($"{person.PersonId} ");
            }
        }
        public static void Feladat_5()
        {

            var count = 0;
            var max = 0;
            var timeStamp = "";

            foreach (var log in Tarsalgo)
            {
                if (log.IsInside)
                {
                    count++;
                }
                else
                {
                    count--;
                }
                if (max < count)
                {
                    max = count;
                    timeStamp = log.Hour + ":" + log.Minute;
                }
            }
            Console.WriteLine("\n\n5. feladat");
            Console.WriteLine($"Például {timeStamp}-kor voltak a legtöbben a társalgóban.");
        }
        public static void Feladat_6_7_8()
        {

            Console.WriteLine("\n6. feladat");
            Console.Write("Adja meg a személy azonosítóját! ");
            var userInput = Convert.ToInt32(Console.ReadLine());


            Console.WriteLine("\n7. feladat");
            var timeIn = new DateTime();
            var diff = 0;
            var counter = 0;

            foreach (var log in Tarsalgo)
            {
                if (log.PersonId == userInput)
                {
                    var text = "";
                    counter++;
                    if (log.IsInside)
                    {
                        text = log.Hour + ":" + log.Minute + "-";
                        timeIn = new DateTime(2000, 10, 10, log.Hour, log.Minute, 0);
                    }
                    else
                    {
                        text = text + log.Hour + ":" + log.Minute + '\n';
                        var timeOut = new DateTime(2000, 10, 10, log.Hour, log.Minute, 0);
                        diff += timeOut.Subtract(timeIn).Minutes;
                    }
                    Console.Write(text);
                }
            }

            var inside = false;

            if (counter % 2 != 0)
            {
                inside = true;
                var timeOut = new DateTime(2000, 10, 10, 15, 0, 0);
                diff += timeOut.Subtract(timeIn).Minutes;
            }

            var insideText = inside ? "a társalgóban volt" : "nem volt a társalgóban";


            Console.WriteLine("\n\n8. feladat");
            Console.WriteLine($"A(z) {userInput}. személy összesen {diff} percet volt bent, a megfigyelés végén {insideText}.");

        }

    }
}
