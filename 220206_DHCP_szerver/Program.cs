using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _220206_DHCP_szerver
{


    class DHCP
    {
        public string Mac { get; set; }
        public string IP { get; set; }
        public bool Excluded { get; set; }
        public bool Reserved { get; set; }
    }
    internal class Program
    {
        public static List<DHCP> DHCPs = new List<DHCP>();

        public static List<string> Excluded = new List<string>();
        public static List<string> EnabledIps = new List<string>();


        static void Main(string[] args)
        {
            // 192.168.10.100 - 192.168.10.199

            ReadExcluded();
            ReadReserved();
            ReadActual();

            EnabledIps = CreateIpList();

            DHCPs.ForEach(dhcp => Console.WriteLine($"{dhcp.Mac}, {dhcp.IP}, {dhcp.Excluded}, {dhcp.Reserved}"));

            Console.ReadLine();
        }

        private static List<string> CreateIpList()
        {
            var list = new List<string>();
            for (int i = 100; i < 200; i++)
            {
                var newIp = $"192.168.10.{i}";

                if (!Excluded.Contains(newIp))
                {
                    list.Add(newIp);
                }
            }
            return list;
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
                        var ujDHCP = new DHCP()
                        {
                            Mac = line[0],
                            IP = line[1],
                            Excluded = false,
                            Reserved = false
                        };
                        DHCPs.Add(ujDHCP);
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
                        var ujDHCP = new DHCP()
                        {
                            Mac = line[0],
                            IP = line[1],
                            Excluded = false,
                            Reserved = true
                        };
                        DHCPs.Add(ujDHCP);
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
