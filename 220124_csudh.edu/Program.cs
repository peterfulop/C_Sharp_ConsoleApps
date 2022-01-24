using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220124_csudh.edu
{
    class Address
    {
        public int Nr { get; set; }
        public string DomainName { get; set; }
        public string IpAddress { get; set; }
    }
    internal class Program
    {
        public static List<Address> Addresses = new List<Address>();
        static void Main(string[] args)
        {

            Feladat_02();
            Feladat_03();
            Feladat_05();
            Feladat_06();

            Console.ReadLine();
        }

        private static void Feladat_06()
        {
            using (var fs = new FileStream("table.html",FileMode.Create))
            {
                using (var sw = new StreamWriter(fs,Encoding.UTF8))
                {
                    sw.WriteLine($"<table>");
                    sw.WriteLine($"<thead>");
                    sw.WriteLine($"<tr>");
                    sw.WriteLine($"<th style='text-align:left'>Ssz</th>");
                    sw.WriteLine($"<th style='text-align:left'>Host domainneve</th>");
                    sw.WriteLine($"<th style='text-align:left'>Host IP címe</th>");
                    sw.WriteLine($"<th style='text-align:left'>1. szint</th>");
                    sw.WriteLine($"<th style='text-align:left'>2. szint</th>");
                    sw.WriteLine($"<th style='text-align:left'>3. szint</th>");
                    sw.WriteLine($"<th style='text-align:left'>4. szint</th>");
                    sw.WriteLine($"<th style='text-align:left'>5. szint</th>");
                    sw.WriteLine($"</tr>");
                    sw.WriteLine($"</thead>");
                    sw.WriteLine($"<tbody>");
                    foreach (var a in Addresses)
                    {
                        sw.WriteLine($"<tr>");
                        sw.WriteLine($"<th style='text-align:left'>{a.Nr}</th>");
                        sw.WriteLine($"<td>{a.DomainName}</td>");
                        sw.WriteLine($"<td>{a.IpAddress}</td>");

                        for (int i = 0; i < 5; i++)
                        {
                            var res = Domain(a.DomainName, i+1);
                            sw.WriteLine($"<td>{res}</td>");
                        }
                        sw.WriteLine($"</tr>");
                    }
                    sw.WriteLine($"</tbody>");
                    sw.WriteLine($"</table>");
                }
            }
        }

        private static void Feladat_05()
        {
            Console.WriteLine($"5. feladat: Az első domain felépítése:");
            
            var firstIp = Addresses.First().DomainName;

            for (int i = 0; i < 5; i++)
            {
                var res = Domain(firstIp, i+1);
                Console.WriteLine($"\t{i+1}. szint: {res}");
            }
        }

        private static string Domain(string Ip, int range)
        {
            if (range < 1 || range > 5) return "Hiba! A szint 1 és 5 közötti érték lehet!";

            var Range = "";
            var IpArray = Ip.Split('.');
            try
            {
                switch (range)
                {
                    case 1:
                        Range = IpArray[IpArray.Length - 1];
                        break;
                    case 2:
                        Range = IpArray[IpArray.Length - 2];
                        break;
                    case 3:
                        Range = IpArray[IpArray.Length - 3];
                        break;
                    case 4:
                        Range = IpArray[IpArray.Length - 4];
                        break;
                    case 5:
                        Range = IpArray[IpArray.Length - 5];
                        break;
                }
            }
            catch (Exception)
            {
                return "nincs";
                throw;
            }

            return Range;
        }

        private static void Feladat_03()
        {
            Console.WriteLine($"3. feladat: Domainek száma: {Addresses.Count}");
        }

        private static void Feladat_02()
        {
            using (var fs = new FileStream("csudh.txt",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    var counter = 1;
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var newAddress = new Address()
                        {
                            DomainName = line[0],
                            IpAddress = line[1],
                            Nr = counter
                        };
                        Addresses.Add(newAddress);
                        counter++;
                    }
                }
            }
        }
    }
}
