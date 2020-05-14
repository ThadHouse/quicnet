using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuicNet.Interop
{
#pragma warning disable CA1060 // Move pinvokes to native methods class
#pragma warning disable CA1401 // P/Invokes should not be visible
    public static class QuicNativeMethods
    {
        public const string QuicDllName = "msquic.dll";

        [DllImport(QuicDllName, CallingConvention = CallingConvention.Cdecl)]

        public static unsafe extern int MsQuicOpen(NativeQuicApi** api);

        [DllImport(QuicDllName, CallingConvention = CallingConvention.Cdecl)]

        public static unsafe extern void MsQuicClose(NativeQuicApi* api);
    }
}
