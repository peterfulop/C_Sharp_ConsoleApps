using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _210921_tesztverseny
{
    class userData
    {
        public string UserId { get; set; }
        public string UserAnswers { get; set; }
        public int Scores { get; set; }

        public userData(string userId, string userAnswers)
        {
            this.UserId = userId;
            this.UserAnswers = userAnswers;
            this.Scores = 0;
        }

    }

    class Program
    {
        public static string CorrectAnswers { get; set; }
        public static List<userData> myUsers = new List<userData>();
        public static List<userData> selectedUser = new List<userData>();
        static void Main(string[] args)
        {

            Feladat_1();

            Feladat_2();

            Feladat_3();

            Feladat_4();

            Feladat_5();

            Feladat_6();

            Feladat_7();

            Console.ReadLine();
        }

        public static void Feladat_1()
        {
            Console.WriteLine("1. feladat: Az adatok beolvasása\n");

            using (FileStream fs = new FileStream("valaszok.txt", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    CorrectAnswers = sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string[] puffer = sr.ReadLine().Split(' ');
                        userData user = new userData(puffer[0], puffer[1]);
                        myUsers.Add(user);
                    }
                }
            }
        }

        public static void Feladat_2()
        {
            Console.WriteLine($"2. feladat: A vetélkedőn {myUsers.Count} versenyző indult.\n");
        }

        public static void Feladat_3()
        {
            string userInput;

            do
            {
                Console.Write("3. feladat: A versenyző azonosítója = ");
                userInput = Console.ReadLine();

            } while (userInput == "");

            foreach (var user in myUsers)
            {
                if (user.UserId == userInput)
                {
                    selectedUser.Add(user);
                    Console.WriteLine($"{user.UserAnswers}\t(a versenyző válasza)\n");
                }
            }
        }

        public static void Feladat_4()
        {

            Console.Write($"4. feladat:\n{CorrectAnswers}\t(a helyes megoldás)\n");

            var answers = selectedUser[0].UserAnswers;

            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i] == CorrectAnswers[i])
                {
                    Console.Write("+");
                }
                else
                {
                    Console.Write(" ");
                }
            }
        }

        public static void Feladat_5()
        {
            string userInput;

            do
            {
                Console.Write("\n\n5. feladat: A feladat sorszáma = ");
                userInput = Console.ReadLine();

            } while (userInput == "");

            var solution = CorrectAnswers[int.Parse(userInput) - 1];

            int correctCounter = 0;

            foreach (var item in myUsers)
            {
                if (item.UserAnswers[int.Parse(userInput) - 1] == solution)
                {
                    correctCounter++;
                }
            }

            float percent = (float)correctCounter / myUsers.Count * 100;
            Console.WriteLine($"A feladatra {correctCounter} fő, a versenyzők {Math.Round(percent, 2)}%-a adott helyes választ.\n");

        }

        public static void Feladat_6()
        {

            /* 1 2 3 4 5 6 7 8 9  10 11 12 13 14
             * B C C C D B B B B  C   D  A  A  A
             * 0 1 2 3 4 5 6 7 8  9  10 11 12 13
             * 
                1-5 : 3 pont 0-4
                6-10 : 4 pont 5-9
                11-13: 5 pont 10-12
                14: 6 pont 13
             */


            Console.WriteLine("6. feladat: A versenyzők pontszámának meghatározása\n");

            foreach (var user in myUsers)
            {
                var userScores = 0;
                for (int i = 0; i < CorrectAnswers.Length; i++)
                {
                    if (user.UserAnswers[i] == CorrectAnswers[i])
                    {
                        if (i < 5)
                        {
                            userScores += 3;
                        }
                        else if (i < 10)
                        {
                            userScores += 4;
                        }
                        else if (i < 13)
                        {
                            userScores += 5;
                        }
                        else
                        {
                            userScores += 6;
                        }
                    }
                }
                user.Scores = userScores;
            }

            using (var fs = new FileStream("pontok.txt", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var user in myUsers)
                    {
                        sw.WriteLine($"{user.UserId} {user.Scores}");
                    }
                }
            }
        }

        public static void Feladat_7()
        {

            Console.WriteLine("7. feladat: A verseny legjobbjai:");

            var usersByScore = myUsers.OrderByDescending(item => item.Scores);
            var puffer = new List<int>();

            foreach (var user in usersByScore)
            {
                if (puffer.Count() < 3)
                {
                    var highScore = user.Scores;
                    if (!puffer.Contains(highScore))
                    {
                        puffer.Add(highScore);
                    }
                    Console.WriteLine($"{puffer.Count}. díj({user.Scores} pont) : {user.UserId}");
                }
            }
        }
    }
}
