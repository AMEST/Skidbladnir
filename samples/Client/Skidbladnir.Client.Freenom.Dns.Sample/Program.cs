using System;
using System.Linq;
using System.Threading.Tasks;

namespace Skidbladnir.Client.Freenom.Dns.Sample
{
    class Program
    {
        private static string Email = "";
        private static string Password = "";

        static void Main(string[] args)
        {
            Console.WriteLine("Creating client");
            var dnsClient = FreenomClientFactory.Create();
            var authStatus = dnsClient.IsAuthenticated().GetAwaiter().GetResult();
            Console.WriteLine($"Auth status: {authStatus}");
            Console.WriteLine("Login in");
            dnsClient.SignIn(Email, Password).GetAwaiter().GetResult();
            authStatus = dnsClient.IsAuthenticated().GetAwaiter().GetResult();
            Console.WriteLine($"Auth status: {authStatus}");
            var zones = dnsClient.GetZones().GetAwaiter().GetResult();
            Console.WriteLine("===========Zones============");
            foreach (var zone in zones)
            {
                Console.WriteLine(
                    $"Zone: {zone.ZoneId} \t {zone.Name} \t {zone.ExpireDate} \t {zone.RegistrationDate} \t {zone.Status} \t {zone.Type}\n");
            }

            Console.WriteLine("===========Records============");

            foreach (var zone in zones)
            {
                var records = dnsClient.GetDnsRecords(zone).GetAwaiter().GetResult();
                Console.WriteLine($"DnsZone: {zone.Name}");
                Console.WriteLine($"№ \t NAME \t TYPE \t TTL \t VALUE \t PRIORITY\n");
                foreach (var dnsRecord in records)
                    Console.WriteLine(
                        $"{dnsRecord.Id} \t {dnsRecord.Name} \t {dnsRecord.Type} \t {dnsRecord.Ttl} \t {dnsRecord.Value} \t {dnsRecord.Priority}\n");

                Console.WriteLine("=======================");
            }


            var neededZone = zones.LastOrDefault();
            Console.WriteLine("===========Adding record============");
            var record = new DnsRecord("localhost")
            {
                Ttl = 600,
                Value = "127.0.0.1"
            };
            dnsClient.AddDnsRecord(neededZone, record).GetAwaiter().GetResult();
            var dnsRecords = dnsClient.GetDnsRecords(neededZone).GetAwaiter().GetResult();
            Console.WriteLine($"№ \t NAME \t TYPE \t TTL \t VALUE \t PRIORITY\n");
            foreach (var dnsRecord in dnsRecords)
                Console.WriteLine(
                    $"{dnsRecord.Id} \t {dnsRecord.Name} \t {dnsRecord.Type} \t {dnsRecord.Ttl} \t {dnsRecord.Value} \t {dnsRecord.Priority}\n");

            Task.Delay(TimeSpan.FromMinutes(1)).Wait();

            Console.WriteLine("===========Removing record============");
            dnsClient.RemoveDnsRecord(neededZone,
                dnsRecords.FirstOrDefault(r => r.Name.Equals(record.Name, StringComparison.OrdinalIgnoreCase)
                                               && r.Value.Equals(record.Value)));
            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }
    }
}