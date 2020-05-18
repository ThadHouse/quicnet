using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuicNet.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void QuicSecConfigCreateComplete(void* context, int status, QuicNativeSecConfig* securityConfig);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int QuicListenerCallback(QuicHandle* listener, void* context, QuicNativeListenerEvent* evnt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int QuicConnectionCallback(QuicHandle connection, void* context, QuicNativeConnectionEvent* evnt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int QuicStreamCallback(QuicHandle* stream, void* context, QuicNativeStreamEvent* evnt);


}
