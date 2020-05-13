#pragma warning disable CA1051 // Do not declare visible instance fields
#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable CA1034 // Nested types should not be visible

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuicNet.Interop
{
    public unsafe struct QuicRegistrationConfig
    {
        public byte* AppName; //UTF8 string
        public QuicExecutionProfile ExecutionProfile;
    }

    public unsafe struct QuicCertificateHash
    {
        public fixed byte ShaHash[20];
    }

    public unsafe struct QuicCertificateHashStore
    {
        public QuicCertificateHashStoreFlags Flags;
        public fixed byte ShaHash[20];
        public fixed byte StoreName[128];
    }

    public unsafe struct QuicCertificateFile
    {
        public byte* PrivateKeyFile;
        public byte* CertificateFile;
    }

    public unsafe struct QuicBuffer
    {
        public uint Length;
        public byte* Buffer;
    }

    public unsafe struct QuicNewConnectionInfo
    {
        public uint QuicVersion;
        public QuicAddr* LocalAddress;
        public QuicAddr* RemoteAddress;
        public uint CryptoBufferLength;
        public ushort ClientAplnListLength;
        public ushort ServerNameLength;
        public byte NegotiatedAplnLength;
        public byte* CryptoBuffer;
        public byte* ClientAlpnList;
        public byte* NegotiatedAlpn;
        public byte* ServerName;
    }



    public unsafe struct QuicStatistics
    {

        public unsafe struct QuicStatisticsTiming
        {
            public ulong Start;
            public ulong InitialFlightEnd;
            public ulong HandshakeFlightEnd;
        }

        public unsafe struct QuicStatisticsHandshake
        {
            public uint ClientFlight1Bytes;
            public uint ServerFlight1Bytes;
            public uint ClientFlight2Bytes;
        }

        public unsafe struct QuicStatisticsSend
        {
            public ushort PathMtu;
            public ulong TotalPackets;
            public ulong RetransmittablePackets;
            public ulong SuspectedLostPackets;
            public ulong SpuriousLostPackets;
            public ulong TotalBytes;
            public ulong TotalStreamBytes;
            public uint CongestionCount;
            public uint PersistentCongestionCount;
        }

        public unsafe struct QuicStatisticsRecv
        {
            public ulong TotalPackets;
            public ulong ReorderedPackets;
            public ulong DroppedPackets;
            public ulong DuplicatePackets;
            public ulong TotalBytes;
            public ulong TotalStreamBytes;
            public ulong DecryptionFailures;
        }

        public unsafe struct QuicStatisticsMisc
        {
            public uint KeyUpdateCount;
        }

        public ulong CorrelationId;
        public uint Bitfield; // Todo, make this bitfield correct
        public uint Rtt;
        public uint MinRtt;
        public uint MaxRtt;
        public QuicStatisticsTiming Timing;
        public QuicStatisticsHandshake Handshake;
        public QuicStatisticsSend Send;
        public QuicStatisticsRecv Recv;
        public QuicStatisticsMisc Misc;
    }



    public unsafe struct QuicListenerStatistics
    {
        public unsafe struct QuicListenerStatisticsRecv
        {
            public ulong DroppedPackets;
        }

        public unsafe struct QuicListenerStatisticsBinding
        {
            public QuicListenerStatisticsRecv Recv;
        }

        public ulong TotalAcceptedConnections;
        public ulong TotalRejectedConnections;
        public QuicListenerStatisticsBinding Binding;
    }

    public struct QuicListenerEvent
    {
        public unsafe struct QuicListenerEventNewConnection
        {
            public QuicNewConnectionInfo* Info;
            public QuicHandle* Connection;
            public QuicSecConfig* SecurityConfiguration;
        }

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct QuicListenerEventUnion
        {
            [FieldOffset(0)]
            public QuicListenerEventNewConnection NewConnection;
        }


        public QuicListenerEventType Type;
        public QuicListenerEventUnion Data;
    }

    public struct QuicConnectionEvent
    {
        public QuicConnectionEventType Type;
        public QuicConnectionEventUnion Data;

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct QuicConnectionEventUnion
        {
            [FieldOffset(0)]
            public QuicConnectionEventConnected Connected;
            [FieldOffset(0)]
            public QuicConnectionEventShutdownInitiatedByTransport ShutdownInitiatedByTransport;
            [FieldOffset(0)]
            public QuicConnectionEventShutdownInitiatedByPeer ShutdownInitiatedByPeer;
            [FieldOffset(0)]
            public QuicConnectionEventShutdownComplete ShutdownComplete;
            [FieldOffset(0)]
            public QuicConnectionEventLocalAddressChanged LocalAddressChanged;
            [FieldOffset(0)]
            public QuicConnectionEventPeerAddressChanged PeerAddressChanged;
            [FieldOffset(0)]
            public QuicConnectionEventPeerStreamStarted PeerStreamStarted;
            [FieldOffset(0)]
            public QuicConnectionEventStreamsAvailable StreamsAvailable;
            [FieldOffset(0)]
            public QuicConnectionEventIdealProcessorChanged IdealProcessorChanged;
            [FieldOffset(0)]
            public QuicConnectionEventDatagramStateChanged DatagramStateChanged;
            [FieldOffset(0)]
            public QuicConnectionEventDatagramReceived DatagramReceived;
            [FieldOffset(0)]
            public QuicConnectionEventDatagramSendStateChanged DatagramSendStateChanged;

            public unsafe struct QuicConnectionEventConnected
            {
                public byte SessionResumed;
                public byte NegotiatedAlpnLength;
                public byte* NegotiatedAlpn;
            }

            public unsafe struct QuicConnectionEventShutdownInitiatedByTransport
            {
                public int Status;
            }

            public unsafe struct QuicConnectionEventShutdownInitiatedByPeer
            {
                public ulong ErrorCode;
            }

            public unsafe struct QuicConnectionEventShutdownComplete
            {
                public byte PeerAcknowledgedShutdown;
            }

            public unsafe struct QuicConnectionEventLocalAddressChanged
            {
                public void* Address;
            }

            public unsafe struct QuicConnectionEventPeerAddressChanged
            {
                public void* Address;
            }

            public unsafe struct QuicConnectionEventPeerStreamStarted
            {
                public QuicHandle* Stream;
                public QuicStreamOpenFlags Flags;
            }

            public unsafe struct QuicConnectionEventStreamsAvailable
            {
                public ushort BidirectionalCount;
                public ushort UnidirectionalCount;
            }

            public unsafe struct QuicConnectionEventIdealProcessorChanged
            {
                public byte IdealProcessor;
            }

            public unsafe struct QuicConnectionEventDatagramStateChanged
            {
                public byte SendEnabled;
                public ushort MaxSendLength;
            }

            public unsafe struct QuicConnectionEventDatagramReceived
            {
                public QuicBuffer* Buffer;
                public QuicReceiveFlags Flags;
            }

            public unsafe struct QuicConnectionEventDatagramSendStateChanged
            {
                public void* ClientContext;
                public QuicDatagramSendState State;
            }
        }
    }

    public unsafe struct QuicStreamEvent
    {
        public QuicStreamEventType Type;
        public QuicStreamEventUnion Data;

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct QuicStreamEventUnion
        {
            [FieldOffset(0)]
            public QuicStreamEventStartComplete StartComplete;
            [FieldOffset(0)]
            public QuicStreamEventReceive Receive;
            [FieldOffset(0)]
            public QuicStreamEventSendComplete SendComplete;
            [FieldOffset(0)]
            public QuicStreamEventPeerSendAborted PeerSendAborted;
            [FieldOffset(0)]
            public QuicStreamEventPeerReceiveAborted PeerReceiveAborted;
            [FieldOffset(0)]
            public QuicStreamEventSendShutdownComplete SendShutdownComplete;
            [FieldOffset(0)]
            public QuicStreamEventIdealSendBufferSize IdealSendBufferSize;


            public unsafe struct QuicStreamEventStartComplete
            {
                public int Status;
                public ulong Id;
            }

            public unsafe struct QuicStreamEventReceive
            {
                public ulong AbsoluteOffset;
                public ulong TotalBufferLength;
                public QuicBuffer* Buffers;
                public uint BufferCount;
                public QuicReceiveFlags Flags;
            }

            public unsafe struct QuicStreamEventSendComplete
            {
                public byte Canceled;
                public void* ClientContext;
            }

            public unsafe struct QuicStreamEventPeerSendAborted
            {
                public ulong ErrorCode;
            }

            public unsafe struct QuicStreamEventPeerReceiveAborted
            {
                public ulong ErrorCode;
            }

            public unsafe struct QuicStreamEventSendShutdownComplete
            {
                public byte Graceful;
            }

            public unsafe struct QuicStreamEventIdealSendBufferSize
            {
                public ulong ByteCount;
            }
        }
    }

    public struct QuicAddr
    {

    }

    // Empty struct, really an opaque ptr
    public struct QuicSecConfig
    {

    }
}
