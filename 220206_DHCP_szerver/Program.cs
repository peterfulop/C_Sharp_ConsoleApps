using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220206_DHCP_szerver
{
    class Test
    {
        public string Parameter { get; set; }
        public string Value { get; set; }
    }
    class DHCP
    {
        public string Mac { get; set; }
        public string IP { get; set; }
    }
    internal class Program
    {

        public static List<string> Excluded = new List<string>();
        public static List<string> EnabledIps = new List<string>();

        public static List<DHCP> Reserved = new List<DHCP>();
        public static List<DHCP> Actual = new List<DHCP>();

        public static List<Test> TestList = new List<Test>();


        static void Main(string[] args)
        {
            ReadSourceFiles();
            RunDHCP();
            WriteActualsToFile();

            Console.ReadLine();
        }


        private static void WriteActualsToFile()
        {
            using (var fs = new FileStream("dhcp_kesz.csv", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var l in Actual.OrderBy(x=>x.IP))
                    {
                        sw.WriteLine($"{l.Mac};{l.IP}");
                    }
                }
            }

            Console.WriteLine("3. feladat: Adatok kiírva!");
        }

        private static void RunDHCP()
        {
            Console.WriteLine("2. feladat: testlista feldolgozása...");

            foreach (var i in TestList)
            {
                if (i.Parameter == "request")
                {
                    if (!IsExcluded(i.Value) && IsReserved(i.Value))
                    {
                        var getReservedIP = Reserved.FirstOrDefault(x => x.Mac == i.Value).IP;
                        Actual.Add(new DHCP() { IP = getReservedIP, Mac = i.Value });
                    }
                    else
                    {
                        if (EnabledIps.Count != 0)
                        {
                            var firstEnabledIP = EnabledIps.OrderBy(x => x).First();
                            Actual.Add(new DHCP() { IP = firstEnabledIP, Mac = i.Value });
                            var remove = EnabledIps.FirstOrDefault(x => x == firstEnabledIP);
                            EnabledIps.Remove(remove);
                        }
                    }
                }
                else
                {
                    if (!IsExcluded(i.Value))
                    {
                        var remove = Actual.FirstOrDefault(x => x.IP == i.Value);
                        Actual.Remove(remove);

                        if (!IsReserved(i.Value, false))
                        {
                            EnabledIps.Add(i.Value);
                            EnabledIps.OrderBy(x => x);
                        }
                    }
                }
            }
        }

        private static bool IsReserved(string parameter, bool mac=true)
        {
            if (mac)
            {
                return Reserved.Select(x=>x.Mac).Contains(parameter);
            }
            else
            {
                return Reserved.Select(x => x.IP).Contains(parameter);
            }
        }

        private static bool IsExcluded(string parameter)
        {
            return Excluded.Contains(parameter);
        }

        private static List<string> GetEnabledIPs()
        {
            var list = new List<string>();
            for (int i = 100; i < 200; i++)
            {
                var newIp = $"192.168.10.{i}";

                if (!Excluded.Contains(newIp) && !Reserved.Select(x=>x.IP).Contains(newIp) && !Actual.Select(x => x.IP).Contains(newIp))
                {
                    list.Add(newIp);
                }
            }
            return list;
        }

        private static void ReadSourceFiles()
        {
            Console.WriteLine("1. feladat: forrásfileok beolvasása...");
            ReadExcluded();
            ReadReserved();
            ReadActual();
            ReadTest();
            EnabledIps = GetEnabledIPs();

        }

        private static void ReadTest()
        {
            using (var fs = new FileStream("test.csv", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var test = new Test()
                        {
                            Parameter = line[0],
                            Value = line[1],
                        };
                        TestList.Add(test);
                    }
                }
            }
        }
        private static void ReadActual()
        {
            using (var fs = new FileStream("dhcp.csv", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujActual = new DHCP()
                        {
                            Mac = line[0],
                            IP = line[1],
                        };
                        Actual.Add(ujActual);
                    }

                }
            }
        }
        private static void ReadReserved()
        {
            using (var fs = new FileStream("reserved.csv", FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(';');
                        var ujReserved = new DHCP()
                        {
                            Mac = line[0],
                            IP = line[1],
                        };
                        Reserved.Add(ujReserved);
                    }

                }
            }
        }
        private static void ReadExcluded()
        {
            using (var fs = new FileStream("excluded.csv",FileMode.Open))
            {
                using (var sr = new StreamReader(fs,Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    { 
                        Excluded.Add(sr.ReadLine());
                    }

                }
            }
        }
    }
}
