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
        int ConnectionStart(QuicHandle* connection, void* family, byte* serverName, ushort serverPort);

        int StreamOpen();
        int StreamClose();
        int StreamStart();
        int StreamShutdown();
        int StreamSend();
        int StreamReceiveComplete();
        int StreamReceiveSetEnabled();

        int DatagramSend();
    }
}
