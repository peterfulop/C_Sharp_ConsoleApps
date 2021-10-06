using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _210602_godrok
{
    class Program
    {
        public static bool deepTest(List<int> list)
        {
            var deeper = true;
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i + 1] >= list[i])
                {
                    if (!deeper)
                    {
                        return false;
                    }
                }
                else
                {
                    deeper = false;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {
            var userTavolsag = 0;
            var max = 0;

            #region 1. Feladat

            List<int> data = new List<int>();

            using (var fs = new FileStream("melyseg.txt", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var element = Convert.ToInt32(sr.ReadLine());
                        data.Add(element);

                    }
                }
            }
            Console.WriteLine($"1. Feladat\n{data.Count()}\n");

            #endregion

            #region 2. Feladat

            Console.WriteLine("Adj meg egy távolsági értéket:");
            var input = Console.ReadLine();

            while (!int.TryParse(input, out int result) || Convert.ToInt32(input) < 1 || Convert.ToInt32(input) > 694)
            {
                Console.WriteLine("Adj meg egy távolsági értéket:");
                input = Console.ReadLine();
            }



            userTavolsag = Convert.ToInt32(input);

            if (data[userTavolsag - 1] != 0)
            {
                Console.WriteLine($"2. Feladat\nEzen a helyen a felszín {data[userTavolsag - 1]} méter mélyen van.\n");
            }
            else
            {
                Console.WriteLine($"2. Feladat\nAz adott helyen nincs gödör.\n");
            }

            #endregion

            #region 3. Feladat

            var nulls = data.Where(item => item == 0).Count();
            var eredemny = (float)100 / data.Count() * nulls;
            Console.WriteLine($"3. Feladat\nAz érintetlen terület aránya {Math.Round(eredemny, 2)}%.\n");


            #endregion

            #region 4. Feladat

            var godorCount = 0;
            using (var fs = new FileStream("test.txt", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] != 0)
                        {
                            sw.Write(data[i] + " ");
                            if (data[i + 1] == 0)
                            {
                                sw.Write("\n");
                                godorCount++;
                            }
                        }
                    }
                }
            }

            #endregion

            #region 5. Feladat

            Console.WriteLine($"5. Feladat\nA gödrök száma: {godorCount}\n");

            #endregion

            #region 6. Feladat

            if (data[userTavolsag - 1] != 0)
            {

                #region 6/A
                var startIndex = userTavolsag;
                var endIndex = userTavolsag;

                while (data[startIndex - 1] != 0)
                {
                    startIndex--;
                }
                while (data[endIndex - 1] != 0)
                {
                    endIndex++;
                }

                var godorSzelesseg = endIndex - (startIndex + 1);
                Console.WriteLine($"6. feladat\na)\nA gödör kezdete: {startIndex + 1} méter, a gödör vége: {endIndex - 1} méter.");
                #endregion

                #region 6/B
                List<int> actualGodor = new List<int>();

                for (int i = startIndex; i < endIndex - 1; i++)
                {
                    actualGodor.Add(data[i]);
                }

                var melyul = deepTest(actualGodor);

                if (!melyul)
                {
                    Console.WriteLine($"b)\nNem mélyül folyamatosan.");
                }
                else
                {
                    Console.WriteLine($"b)\nFolyamatosan mélyül!");
                }

                #endregion

                #region 6/C

                if (actualGodor.Count > 0) max = actualGodor.Max();

                Console.WriteLine($"c)\nA legnagyobb mélysége {max} méter.");
                #endregion

                #region 6/D
                var osszmeret = 0;
                foreach (var item in actualGodor)
                {
                    var meret = item * 1 * 10;
                    osszmeret += meret;
                }
                Console.WriteLine($"d)\nA térfogata {osszmeret} m^3.");
                #endregion

                #region 6/E
                var minus = godorSzelesseg * 1 * 10;
                Console.WriteLine($"e)\nA vízmennyiség {osszmeret - minus} m^3.");
                #endregion
            }

            #endregion

            Console.ReadKey();
        }
    }

}
