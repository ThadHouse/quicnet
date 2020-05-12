using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuicNet.Interop
{
    public enum QuicHandleType
    {
        Registration,
        Session,
        Listener,
        Client,
        Child,
        Stream
    }

    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct QuicHandle
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        public QuicHandleType Type;
        public IntPtr ClientContext;
#pragma warning restore CA1051 // Do not declare visible instance fields
    }
}
