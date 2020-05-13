using QuicNet.Interop;
using System;

namespace quicnet.dev
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            var nativeApi = new NativeQuicApi();
            var api = ApiGenerator.CreateApiImplementation(&nativeApi, (NativeQuicApi* api) =>
            {
                ;
            });
            api.Dispose();
            Console.WriteLine("Hello World!");
        }
    }
}
