using System;
using System.Collections.Generic;
using System.Text;

namespace QuicNet.Interop
{
    public static class Helpers
    {
        public static bool QuicDatagramSendStateIsFinal(QuicDatagramSendState state) => state > QuicDatagramSendState.LostDiscarded;


    }
}
