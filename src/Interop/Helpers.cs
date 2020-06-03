using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuicNet.Interop
{
    public static class Helpers
    {
        public static bool QuicDatagramSendStateIsFinal(QuicDatagramSendState state) => state > QuicDatagramSendState.LostDiscarded;

        public static void CheckException(int result)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // generate check for success = >= 0
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                if (result < 0) throw Marshal.GetExceptionForHR(result);
            }
            else
            {
                // generate check for success = <= 0
                if (result > 0) throw new Exception("Quic Exception"); // TODO make this a better exception
            }
        }
    }
}
