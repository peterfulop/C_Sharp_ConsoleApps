using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _211103_sudoku
{


    class Sudoku
    {
        public int Szam { get; set; }
        public int Sor { get; set; }
        public int Oszlop { get; set; }
        public int Mezo { get; set; }


    }


    class Program
    {


        public static List<Sudoku> Sudokus = new List<Sudoku>();
        public static List<Sudoku> Players = new List<Sudoku>();

        public static string getFileName()
        {
            Console.WriteLine("1. feladat");
            Console.Write("Adja meg a bemeneti fájl nevét! ");
            var fileName =  Console.ReadLine();
            while (string.IsNullOrWhiteSpace(fileName))
            {
                Console.Write("Adja meg a bemeneti fájl nevét! ");
                fileName = Console.ReadLine();
            }

            return fileName;
        }

        public static void readFileClass(string fileName)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs, Encoding.UTF8))
                    {

                        for (int i = 0; i < 9; i++)
                        {
                            var row=sr.ReadLine().Split(' ');

                            for (int j = 0; j < row.Length; j++)
                            {
                                var sudo = new Sudoku
                                {
                                    Szam = Convert.ToInt32(row[j]),
                                    Sor = i + 1,
                                    Oszlop = j + 1,
                                    Mezo = 3 * (i / 3) + (j / 3) + 1
                                };

                                Sudokus.Add(sudo);
                            }
                        }

                        while (!sr.EndOfStream)
                        {
                            var row = sr.ReadLine().Split(' ');

                            var sudo = new Sudoku
                            {
                                Szam = Convert.ToInt32(row[0]),
                                Sor = Convert.ToInt32(row[1]),
                                Oszlop = Convert.ToInt32(row[2]),
                                Mezo = 3 * (Convert.ToInt32(row[1]) / 3) + (Convert.ToInt32(row[2]) / 3) + 1
                            };

                            Players.Add(sudo);
                        }

                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public static int getRowIndex()
        {
            Console.Write("Adja meg egy sor számát! ");
            return Convert.ToInt32(Console.ReadLine());
        }
        public static int getColIndex()
        {
            Console.Write("Adja meg egy oszlop számát! ");
            return Convert.ToInt32(Console.ReadLine());
        }


       public static void Feldat_03(int row, int col)
        {
            Console.WriteLine("\n3. feladat");

            var data = Sudokus.FirstOrDefault(e => e.Sor == row && e.Sor == col);

            if (data is null)
            {
                Console.WriteLine("Az adott helyet még nem töltötték ki.");
            }
            else
            {
                Console.WriteLine($"Az adott helyen szereplő szám: {data.Szam}\n" +
                    $"A hely a(z) {data.Mezo} résztáblázathoz tartozik.");
            }
        }

        public static void Feladat_4()
        {

           double nulls = Sudokus.Count(x => x.Szam == 0);
           double arany = Math.Round(nulls / 81*100,1);

            Console.WriteLine("\n4. feladat");
            Console.WriteLine($"Az üres helyek aránya: {arany}%");
        }


        public static void Feladat_05()
        {
            Console.WriteLine("\n5. feladat");


            foreach (var item in Players)
            {
                var data = Sudokus.FirstOrDefault(x=> x.Sor == item.Sor && x.Oszlop == item.Oszlop);

                var letezikMezoben = Sudokus.FirstOrDefault(x => x.Mezo == item.Mezo && x.Szam == item.Szam);


                var letezikSorban = Sudokus.FindAll(x => x.Sor == item.Sor)
                                                .Exists(x => x.Szam == item.Szam);
                
                var letezikOszlopban = Sudokus.FindAll(x => x.Oszlop == item.Oszlop)
                                                .Exists(x => x.Szam == item.Szam);


                Console.WriteLine($"A kiválasztott sor: {item.Sor} oszlop: {item.Oszlop} a szám: {item.Szam}");

                if (data.Szam != 0)
                {
                    Console.WriteLine("A helyet már kitöltötték.");
                }
                else if (letezikSorban)
                {
                    Console.WriteLine("Az adott sorban már szerepel a szám.");
                }
                else if(letezikOszlopban)
                {
                    Console.WriteLine("Az adott oszlopban már szerepel a szám.");
                }
                else if(letezikMezoben.Szam > 0)
                {
                    Console.WriteLine("Az adott résztáblázatban már szerepel a szám.");
                }

                else if(letezikMezoben.Szam > 0 && !letezikSorban && !letezikOszlopban)
                {
                    Console.WriteLine("A lépés megtehető.");
                }

                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {

            //var fileName = getFileName();
            readFileClass("konnyu.txt");


            var rowIndex = getRowIndex();
            var colIndex = getColIndex();

            Feldat_03(rowIndex, colIndex);

            Feladat_4();

            Feladat_05();

            Console.ReadLine();

        }
    }
}
