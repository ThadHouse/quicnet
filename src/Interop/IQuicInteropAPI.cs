using System;
using System.Collections.Generic;
using System.Text;

namespace QuicNet.Interop
{
    public unsafe interface IQuicInteropApi : IDisposable
    {
        void SetContext(QuicHandle* handle, void* context);
        void* GetContext(QuicHandle* handle);
        void SetCallbackHandler(QuicHandle* handle, void* handler, void* context);

        int SetParam(QuicHandle* handle, QuicParamLevel level, uint param, uint bufferLength, void* buffer);
        int GetParam(QuicHandle* handle, QuicParamLevel level, uint param, uint* bufferLength, void* buffer);

        int RegistrationOpen(QuicRegistrationConfig* config, QuicHandle** registration);
        int RegistrationClose(QuicHandle* registration);

        int SecConfigCreate(QuicHandle* registration, QuicSecConfigFlags flags, void* certificate, byte* principal, void* context, void* completionHandler);
        int SecConfigDelete(QuicSecConfig* securityConfig);

        int SessionOpen(QuicHandle* registration, QuicBuffer* aplnBuffers, uint aplnBufferCount, void* context, QuicHandle** session);
        int SessionClose(QuicHandle* session);
        int SessionShutdown(QuicHandle* session, QuicConnectionShutdownFlags flags, ulong errorCode);

        int ListenerOpen(QuicHandle* session, void* handler, void* context, QuicHandle** listener);
        int ListenerClose(QuicHandle* listener);
        int ListenerStart(QuicHandle* listener, void* localAddress);
        int ListenerStop(QuicHandle* listener);

        int ConnectionOpen(QuicHandle* session, void* handler, void* context, QuicHandle** connection);
        int ConnectionClose(QuicHandle* connection);
        int ConnectionShutdown(QuicHandle* connection, QuicConnectionShutdownFlags flags, ulong errorCode);
        int ConnectionStart(QuicHandle* connection, QuicAddressFamily family, byte* serverName, ushort serverPort);

        int StreamOpen(QuicHandle* connection, QuicStreamOpenFlags flags, void* handler, void* context, QuicHandle** stream);
        int StreamClose(QuicHandle* stream);
        int StreamStart(QuicHandle* stream, QuicStreamStartFlags flags);
        int StreamShutdown(QuicHandle* stream, QuicStreamShutdownFlags flags, ulong errorCode);
        int StreamSend(QuicHandle* stream, QuicBuffer* buffers, uint bufferCount, QuicSendFlags flags, void* clientSendContext);
        int StreamReceiveComplete(QuicHandle* stream, ulong bufferLength);
        int StreamReceiveSetEnabled(QuicHandle* stream, byte isEnabled);

        int DatagramSend(QuicHandle* connection, QuicBuffer* buffers, uint bufferCount, QuicSendFlags flags, void* clientSendContext);
    }
}
