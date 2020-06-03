using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using QuicNet.Interop;

namespace QuicNet
{
    public sealed class QuicListener : IDisposable
    {
        private readonly IQuicInteropApi m_nativeApi;
        private readonly unsafe QuicHandle* m_handle;
        private readonly QuicSession m_session;

        private readonly GCHandle m_gcHandle;
        private static unsafe readonly QuicListenerCallback m_listenerCallback = StaticHandler;
        private static unsafe readonly void* m_nativeListenerCallback = (void*)Marshal.GetFunctionPointerForDelegate(m_listenerCallback);

        private readonly Channel<QuicConnection> connectionQueue;

        internal unsafe QuicListener(IQuicInteropApi nativeApi, QuicSession session)
        {
            m_nativeApi = nativeApi;
            m_session = session;

            connectionQueue = Channel.CreateBounded<QuicConnection>(new BoundedChannelOptions(128)
            {
                SingleWriter = true,
                SingleReader = true,
            });

            m_gcHandle = GCHandle.Alloc(this);

            QuicHandle* handle = null;

            m_nativeApi.ListenerOpen(session.m_handle, m_nativeListenerCallback, (void*)GCHandle.ToIntPtr(m_gcHandle), &handle);

            m_handle = handle;

        }

        private unsafe int Handler(QuicNativeListenerEvent* evnt)
        {
            try
            {
                switch (evnt->Type)
                {
                    case QuicListenerEventType.NewConnection:
                        connectionQueue.Writer.TryWrite(new QuicConnection(m_nativeApi, &evnt->Data.NewConnection));
                        break;
                    default:
                        break;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {

            }
            return 0;
        }

        private unsafe static int StaticHandler(QuicHandle* handle, void* context, QuicNativeListenerEvent* evnt)
        {
            var managedHandle = GCHandle.FromIntPtr((IntPtr)context);
            var listener = (QuicListener)managedHandle.Target;
            return listener.Handler(evnt);
        }

        public async ValueTask<QuicConnection> AcceptConnectionAsync(CancellationToken token = default)
        {
            return await connectionQueue.Reader.ReadAsync(token).ConfigureAwait(false);

        }

        public unsafe void Start()
        {
            m_nativeApi.ListenerStart(m_handle, null);
        }

        #region IDisposable Support
        public unsafe void Dispose()
        {
            connectionQueue.Writer.TryComplete();
            m_nativeApi.ListenerStop(m_handle);
            m_nativeApi.ListenerClose(m_handle);
            m_gcHandle.Free();
        }
        #endregion
    }
}
