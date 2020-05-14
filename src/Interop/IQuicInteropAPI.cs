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

        [CheckResult] void SetParam(QuicHandle* handle, QuicParamLevel level, uint param, uint bufferLength, void* buffer);
        [CheckResult] void GetParam(QuicHandle* handle, QuicParamLevel level, uint param, uint* bufferLength, void* buffer);

        [CheckResult] void RegistrationOpen(QuicRegistrationConfig* config, QuicHandle** registration);
        [CheckResult] void RegistrationClose(QuicHandle* registration);

        [CheckResult] void SecConfigCreate(QuicHandle* registration, QuicSecConfigFlags flags, void* certificate, byte* principal, void* context, void* completionHandler);
        [CheckResult] void SecConfigDelete(QuicSecConfig* securityConfig);

        [CheckResult] void SessionOpen(QuicHandle* registration, QuicBuffer* aplnBuffers, uint aplnBufferCount, void* context, QuicHandle** session);
        [CheckResult] void SessionClose(QuicHandle* session);
        [CheckResult] void SessionShutdown(QuicHandle* session, QuicConnectionShutdownFlags flags, ulong errorCode);

        [CheckResult] void ListenerOpen(QuicHandle* session, void* handler, void* context, QuicHandle** listener);
        [CheckResult] void ListenerClose(QuicHandle* listener);
        [CheckResult] void ListenerStart(QuicHandle* listener, void* localAddress);
        [CheckResult] void ListenerStop(QuicHandle* listener);

        [CheckResult] void ConnectionOpen(QuicHandle* session, void* handler, void* context, QuicHandle** connection);
        [CheckResult] void ConnectionClose(QuicHandle* connection);
        [CheckResult] void ConnectionShutdown(QuicHandle* connection, QuicConnectionShutdownFlags flags, ulong errorCode);
        [CheckResult] void ConnectionStart(QuicHandle* connection, QuicAddressFamily family, byte* serverName, ushort serverPort);

        [CheckResult] void StreamOpen(QuicHandle* connection, QuicStreamOpenFlags flags, void* handler, void* context, QuicHandle** stream);
        [CheckResult] void StreamClose(QuicHandle* stream);
        [CheckResult] void StreamStart(QuicHandle* stream, QuicStreamStartFlags flags);
        [CheckResult] void StreamShutdown(QuicHandle* stream, QuicStreamShutdownFlags flags, ulong errorCode);
        [CheckResult] void StreamSend(QuicHandle* stream, QuicBuffer* buffers, uint bufferCount, QuicSendFlags flags, void* clientSendContext);
        [CheckResult] void StreamReceiveComplete(QuicHandle* stream, ulong bufferLength);
        [CheckResult] void StreamReceiveSetEnabled(QuicHandle* stream, byte isEnabled);

        [CheckResult] void DatagramSend(QuicHandle* connection, QuicBuffer* buffers, uint bufferCount, QuicSendFlags flags, void* clientSendContext);
    }
}
