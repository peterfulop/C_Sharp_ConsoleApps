using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220114_operatorok
{

    class Kifejezes
    {
        public double Op_1 { get; set; }
        public string Operator { get; set; }
        public double Op_2 { get; set; }
    }

    internal class Program
    {
        public static List<Kifejezes> Kifejezesek = new List<Kifejezes>();
        static void Main(string[] args)
        {
            Feladat_01();
            Feladat_02();
            Feladat_03();
            Feladat_04();
            Feladat_05();
            Feladat_07();
            Feladat_08();

            Console.ReadLine();
        }

        private static void Feladat_08()
        {
            Console.WriteLine("8. feladat: eredmenyek.txt");
            using (var fs = new FileStream("eredmenyek.txt",FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var k in Kifejezesek)
                    {
                        var eredmeny = szamolo(k.Op_1, k.Op_2, k.Operator);
                        sw.WriteLine(eredmeny);
                    }
                }
            }
        }

        private static void Feladat_07()
        {
            Console.Write("Kérek egy kifejezést (pl.: 1 + 1): ");
            var valasz = Console.ReadLine();

            while (valasz != "vege")
            {
                var elemek = valasz.Split(' ');
                var op_1 = Convert.ToDouble(elemek[0]);
                var op_2 = Convert.ToDouble(elemek[2]);
                var oper = elemek[1];

                var eredmeny = szamolo(op_1, op_2, oper);
                Console.WriteLine($"\t{eredmeny}");

                Console.Write("Kérek egy kifejezést (pl.: 1 + 1): ");
                 valasz = Console.ReadLine();
            }
        }

        private static string szamolo(double op_1, double op_2, string oper)
        {
            double eredmeny;
            try
            {
                if (oper == "+")
                {
                    eredmeny = op_1 + op_2;
                }
                else if (oper == "-")
                {
                    eredmeny = op_1 - op_2;
                }
                else if (oper == "*")
                {
                    eredmeny = op_1 * op_2;
                }
                else if (oper == "/" || oper == "div")
                {
                    if (op_2 == 0) throw new Exception();
                    eredmeny = op_1 / op_2;
                }
                else if (oper == "mod")
                {
                    eredmeny = op_1 % op_2;
                }
                else
                {
                    return $"{op_1} {oper} {op_2} = Hibás operátor!";
                }
            }
            catch (Exception)
            {
                return $"{op_1} {oper} {op_2} = Egyéb hiba!";
            }

            return $"{op_1} {oper} {op_2} = {eredmeny}";
        }

        private static void Feladat_05()
        {
            var stat = Kifejezesek.Where(x => x.Operator == "mod" ||
            x.Operator == "/" ||
            x.Operator == "div" ||
            x.Operator == "-" ||
            x.Operator == "*" ||
            x.Operator == "+")
                .GroupBy(x => x.Operator)
                .Select(x => new { Operator = x.Key,Count = x.Count()})
                .ToList();

            Console.WriteLine("5. feladat: Statisztika");
            stat.ForEach(x => Console.WriteLine($"\t{x.Operator.PadRight(3)} -> {x.Count} db"));
        }

        private static void Feladat_04()
        {
            bool letezik = false;

            foreach (var k in Kifejezesek)
            {
                if (k.Op_1.ToString().Last() == '0' && k.Op_2.ToString().Last() == '0')
                {
                    letezik = true;
                    break;
                }
            }

            var valasz = letezik == true ? "Van" : "Nincs";
            Console.WriteLine($"4. feladat: {valasz} ilyen kifejezés!");

        }

        private static void Feladat_03()
        {
            var maradekos = Kifejezesek.Count(x => x.Operator == "mod");
            Console.WriteLine($"3. feladat: Kifejezések maradékos osztással: {maradekos}");

        }

        private static void Feladat_02()
        {
            Console.WriteLine($"2. feladat: Kifejezések száma: {Kifejezesek.Count}");
        }

        private static void Feladat_01()
        {

            using (var fs = new FileStream("kifejezesek.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(' ');
                        var ujKifejezes = new Kifejezes()
                        {
                            Op_1 = int.Parse(line[0]),
                            Operator = line[1],
                            Op_2 = int.Parse(line[2])
                        };

                        Kifejezesek.Add(ujKifejezes);
                    }
                }
            }

        }
    }
}
