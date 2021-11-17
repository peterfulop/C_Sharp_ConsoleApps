using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _211110_balkezesek
{

    class Balkezes
    {
        public string Nev { get; set; }
        public DateTime Elso { get; set; }
        public DateTime Utolso { get; set; }
        public int Suly { get; set; }
        public int Magassag { get; set; }
        public double Cm { get; set; }
    }

    class Program
    {
        public static List<Balkezes> Balkezesek = new List<Balkezes>();

        static void Main(string[] args)
        {

            Feladat_02();
            Feladat_03();
            Feladat_04();
            var evszam = Feladat_05();
            Feladat_06(evszam);

            Console.ReadLine();
    
        }

        private static void Feladat_06(int evszam)
        {
          var atlagsuly = Balkezesek.Where(x => x.Elso.Year<= evszam && x.Utolso.Year>=evszam).Average(x=>x.Suly);
          Console.WriteLine($"6. feladat: {atlagsuly:0.00} font");
        }
        private static int Feladat_05()
        {
            Console.WriteLine("5. feladat");

            Console.Write("Kérek egy 1990 és 1999 közötti évszámot!: ");
            var evszam = Convert.ToInt32(Console.ReadLine());

            while (evszam<1990 || evszam>1999)
            {
                Console.Write("Hibás adat!Kérek egy 1990 és 1999 közötti évszámot!: ");
                evszam = Convert.ToInt32(Console.ReadLine());
            }

            return evszam;
        }
        private static void Feladat_04()
        {
            Console.WriteLine($"4. feladat:");

            var utoljara = Balkezesek.Where(x => x.Utolso.Year == 1999 && x.Utolso.Month == 10);

            foreach (var b in utoljara)
            {
                Console.WriteLine($"\t{b.Nev}, {b.Cm:0.0} cm");
            }
        }
        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: {Balkezesek.Count()}");
        }
        private static void Feladat_02()
        {
            using (var fs = new FileStream("balkezesek.csv",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {

                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        var sor = sr.ReadLine().Split(';');
                        var el = sor[1].Split('-');
                        var ut = sor[2].Split('-');

                        var ujAdat = new Balkezes
                        {
                            Nev = sor[0],
                            Elso = new DateTime(Convert.ToInt32(el[0]), Convert.ToInt32(el[1]), Convert.ToInt32(el[2])),
                            Utolso = new DateTime(Convert.ToInt32(ut[0]), Convert.ToInt32(ut[1]), Convert.ToInt32(ut[2])),
                            Suly = Convert.ToInt32(sor[3]),
                            Magassag = Convert.ToInt32(sor[4]),
                            Cm = Convert.ToDouble(sor[4])*2.54
                        };

                        Balkezesek.Add(ujAdat);
                    }
                }
            }

            //Balkezesek.ForEach(x =>
            //{
            //    Console.WriteLine($"{x.Nev},{x.Elso},{x.Utolso},{x.Suly},{x.Magassag}");
            //});

        }
    }
}
