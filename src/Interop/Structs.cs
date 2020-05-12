using System;
using System.Collections.Generic;
using System.Text;

namespace QuicNet.Interop
{
    public struct QuicRegistrationConfig
    {

    }

    public struct QuicSecConfig
    {

    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public unsafe struct QuicBuffer
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        public uint Length;
        public byte* Buffer;
#pragma warning restore CA1051 // Do not declare visible instance fields
    }
}
