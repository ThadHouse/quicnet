using QuicNet.Interop;
using System;

namespace quicnet.dev
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            NativeQuicApi* nativeApi = null;
            var success = QuicNativeMethods.MsQuicOpen(&nativeApi);
            if (success > 0)
            {
                Console.WriteLine("API Failure");
                return;
            }
            var api = ApiGenerator.CreateApiImplementation(nativeApi, QuicNativeMethods.MsQuicClose);
            api.Dispose();
            Console.WriteLine("Hello World!");
        }
    }
}
