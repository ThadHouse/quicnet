using QuicNet;
using QuicNet.Interop;
using System;

namespace quicnet.dev
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            using var api = new QuicApi();
            using var registration = api.OpenRegistration();
            Console.WriteLine("Hello World!");
        }
    }
}
