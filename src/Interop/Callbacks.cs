using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuicNet.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void QuicSecConfigCreateComplete(void* context, int status, QuicSecConfig* securityConfig);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int QuicListenerCallback(QuicHandle* listener, void* context, QuicListenerEvent* evnt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int QuicConnectionCallback(QuicHandle connection, void* context, QuicConnectionEvent* evnt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int QuicStreamCallback(QuicHandle* stream, void* context, QuicStreamEvent* evnt);


}
