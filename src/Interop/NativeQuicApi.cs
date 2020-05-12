using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuicNet.Interop
{
    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct NativeQuicApi
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        public IntPtr SetContext;
        public IntPtr GetContext;
        public IntPtr SetCallbackHandler;

        public IntPtr SetParam;
        public IntPtr GetParam;

        public IntPtr RegistrationOpen;
        public IntPtr RegistrationClose;

        public IntPtr SecConfigCreate;
        public IntPtr SecConfigDelete;

        public IntPtr SessionOpen;
        public IntPtr SessionClose;
        public IntPtr SessionShutdown;

        public IntPtr ListenerOpen;
        public IntPtr ListenerClose;
        public IntPtr ListenerStart;
        public IntPtr ListenerStop;

        public IntPtr ConnectionOpen;
        public IntPtr ConnectionClose;
        public IntPtr ConnectionShutdown;
        public IntPtr ConnectionStart;

        public IntPtr StreamOpen;
        public IntPtr StreamClose;
        public IntPtr StreamStart;
        public IntPtr StreamShutdown;
        public IntPtr StreamSend;
        public IntPtr StreamReceiveComplete;
        public IntPtr StreamReceiveSetEnabled;

        public IntPtr DatagramSend;


#pragma warning restore CA1051 // Do not declare visible instance fields
    }
}
