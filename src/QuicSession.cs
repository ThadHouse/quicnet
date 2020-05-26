using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using QuicNet.Interop;

namespace QuicNet
{
    public sealed class QuicSession : IDisposable
    {
        private readonly IQuicInteropApi m_nativeApi;
        internal readonly unsafe QuicHandle* m_handle;

        internal unsafe QuicSession(IQuicInteropApi nativeApi, QuicRegistration registration, byte[][] apln)
        {
            m_nativeApi = nativeApi;

            QuicHandle* handle = null;

            QuicNativeBuffer* buffers = stackalloc QuicNativeBuffer[apln.Length];

            try
            {
                for (int i = 0; i < apln.Length; i++)
                {
                    byte* allocated = (byte*)Marshal.AllocHGlobal((IntPtr)apln[i].Length);
                    buffers[i].Buffer = allocated;
                    buffers[i].Length = (uint)apln[i].Length;
                    apln[i].AsSpan().CopyTo(new Span<byte>(allocated, apln[i].Length));
                    
                }

                m_nativeApi.SessionOpen(registration.m_handle, buffers, (uint)apln.Length, null, &handle);
                m_handle = handle;
            }
            finally
            {
                for (int i = 0; i < apln.Length; i++)
                {
                    Marshal.FreeHGlobal((IntPtr)buffers[i].Buffer);
                }
            }
        }

        public unsafe void Shutdown(QuicConnectionShutdownFlags flags, ulong errorCode)
        {
            m_nativeApi.SessionShutdown(m_handle, flags, errorCode);
        }


        #region IDisposable Support
        public unsafe void Dispose()
        {
            m_nativeApi.SessionClose(m_handle);
        }
        #endregion
    }
}
