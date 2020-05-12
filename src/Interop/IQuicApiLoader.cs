using QuicNet.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuicNet.Interop
{
    public interface IQuicApiLoader
    {
        IQuicApi Open();
    }
}
