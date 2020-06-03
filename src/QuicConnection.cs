using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QuicNet.Interop;

namespace QuicNet
{
    public sealed class QuicConnection : IDisposable
    {
        private readonly IQuicInteropApi m_nativeApi;
        private unsafe readonly QuicHandle* m_handle;

        internal unsafe QuicConnection(IQuicInteropApi api, QuicNativeListenerEvent.QuicListenerEventNewConnection* newConnection)
        {
            m_nativeApi = api;
        }

        internal unsafe QuicConnection(IQuicInteropApi api, QuicSession session)
        {
            m_nativeApi = api;
            QuicHandle* handle = null;
            api.ConnectionOpen(session.m_handle, null, null, &handle);
            m_handle = handle;
        }

        private unsafe int Handler(QuicNativeConnectionEvent* evnt)
        {
            GC.KeepAlive(m_nativeApi);
            void* v = (void*)evnt;
            return 0;
        }

        private unsafe static int StaticHandler(QuicHandle* handle, void* context, QuicNativeConnectionEvent* evnt)
        {
            var managedHandle = GCHandle.FromIntPtr((IntPtr)context);
            var connection = (QuicConnection)managedHandle.Target;
            return connection.Handler(evnt);
        }

        public unsafe void Connect(QuicAddressFamily addressFamily, string serverName, ushort serverPort)
        {
            if (serverName == null) throw new ArgumentNullException(nameof(serverName));
            var serverNameMaxLength = Encoding.UTF8.GetMaxByteCount(serverName.Length);
            Span<byte> serverNameSpan = serverNameMaxLength < 256 ? stackalloc byte[serverNameMaxLength] : new byte[serverNameMaxLength];

            fixed (byte* serverNamePtr = serverNameSpan)
            {
                fixed (char* serverNameStrPtr = serverName)
                {
                    var actualLength = Encoding.UTF8.GetBytes(serverNameStrPtr, serverName.Length, serverNamePtr, serverNameMaxLength);
                    serverNameSpan[actualLength] = 0;
                }
                m_nativeApi.ConnectionStart(m_handle, addressFamily, serverNamePtr, serverPort);
            }
        }

        public unsafe void Shutdown(QuicConnectionShutdownFlags flags, ulong error)
        {
            m_nativeApi.ConnectionShutdown(m_handle, flags, error);
        }

        public unsafe void Dispose()
        {
            m_nativeApi.ConnectionClose(m_handle);
        }
    }
}
