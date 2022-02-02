using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220202_derekszogu_haromszgogek
{
    class DHaromszog
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        private bool EllDerekszogu { get;  }
        private bool EllMegszerkesztheto { get;  }
        private bool EllNovekvoSorrend { get;  }
        public string ErrMessage { get; set; }

        public string SorSzama
        {
            get;
            set;
        }
        public double Kerulet
        {
            get
            {
                return (A + B + C);
            }
        }
        public double Terulet
        {
            get
            {
                return ((A * B) / 2);
            }
        }

        
        public DHaromszog(string sor, double a, double b, double c)
        {
            EllNovekvoSorrend = (a <= b && b <= c);
            EllMegszerkesztheto = (a + b > c);
            EllDerekszogu = ((c * c) == (a * a) + (b * b));

            this.SorSzama = $"{sor}. sor";
            this.A = a;
            this.B = b;
            this.C = c;

            SetErrorMessages();
        }

        private string IsZeroOrNeg()
        {
            if (this.A <= 0)
            {
                return "a";
            }
            else if (this.B <= 0)
            {
                return "b";

            }
            else if (this.C <= 0)
            {
                return "c";

            }
            return null;
        }

        private void SetErrorMessages()
        {
            var hibasOldal = IsZeroOrNeg();
            if (hibasOldal != null)
            {
                ErrMessage = $"{this.SorSzama}: A(z) '{hibasOldal}' oldal nem lehet nulla vagy negatív!";
                return;
            }
            if (!EllNovekvoSorrend)
            {
                ErrMessage = $"{this.SorSzama}: Az adatok nincsenek növekvő sorrendben!";
                return;
            }
            else if (!EllMegszerkesztheto)
            {
                ErrMessage = $"{this.SorSzama}: A háromszöget nem lehet megszerkeszteni!";
                return;
            }
            else if (!EllDerekszogu)
            {
                ErrMessage = $"{this.SorSzama}: A háromszög nem derékszögű!";
                return;
            }
        }
    }

    internal class Program
    {

        public static List<DHaromszog> DHaromszogek = new List<DHaromszog>();

        static void Main(string[] args)
        {
            ReadData();
            GetDerekszogek();
            Console.ReadLine();
        }

        private static void GetDerekszogek()
        {
            Console.WriteLine("\nDerékszögű háromszögek:");
            DHaromszogek.ForEach(x => Console.WriteLine($"{x.SorSzama}: a={x.A} b={x.B} c={x.C}\n\tKerület={x.Kerulet}\tTerület={x.Terulet}\n"));
        }
        private static void ReadData()
        {
            Console.WriteLine("Hibák a kiválasztott állományban:");
            using (var fs = new FileStream("haromszogek.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    var counter = 1;

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(' ');
                        var ujHaromszog = new DHaromszog(counter.ToString(), double.Parse(line[0]), double.Parse(line[1]), double.Parse(line[2]));

                        if (ujHaromszog.ErrMessage != null)
                        {
                            Console.WriteLine(ujHaromszog.ErrMessage);
                        }
                        else
                        {
                            DHaromszogek.Add(ujHaromszog);
                        }
                        counter++;
                    }
                }
            }
        }
    }
}
