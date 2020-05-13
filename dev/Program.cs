using QuicNet.Interop;
using System;

namespace quicnet.dev
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            var api = ApiGenerator.CreateApiImplementation(new NativeQuicApi(), (NativeQuicApi* api) =>
            {
                ;
            });
            api.Dispose();
            Console.WriteLine("Hello World!");
        }
    }
}
