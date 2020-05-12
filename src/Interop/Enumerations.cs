using System;
using System.Collections.Generic;
using System.Text;

namespace QuicNet.Interop
{
    public enum QuicParamLevel
    {
        Global,
        Registration,
        Session,
        Listener,
        Connection,
        Tls,
        Stream
    }

    [Flags]
    public enum QuicSecConfigFlags
    {
    }

    [Flags]
    public enum QuicConnectionShutdownFlags
    {

    }
}
