using QuicNet;
using QuicNet.Interop;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace quicnet.dev
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var random = new Random();
            byte[] hashBytes = new byte[20];
            random.NextBytes(hashBytes);

            using var api = new QuicApi();
            using var registration = api.OpenRegistration(new QuicRegistrationConfig("QuicTest"));

            using var sec = await registration.CreateSecurityConfiguration(@"C:\Users\thadh\Documents\GitHub\thadhouse\quicnet\MyCertificate.crt", @"C:\Users\thadh\Documents\GitHub\thadhouse\quicnet\MyKey.key");
            using var session = registration.CreateSession();
            
            Console.WriteLine("Hello World!");
        }
    }
}
