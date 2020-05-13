using System;
using System.Collections.Generic;
using System.Text;

namespace QuicNet.Interop
{
    public enum QuicExecutionProfile
    {
        LowLatency,
        MaxThroughput,
        Scavenger,
        RealTime
    }

    public enum QuicLoadBalancingMode
    {
        Disabled,
        ServerIdIp
    }

    [Flags]
    public enum QuicSecConfigFlags
    {
        None = 0x0,
        CertificateHash = 0x1,
        CertificateHashStore = 0x2,
        CertificateContext = 0x4,
        CertificateFile = 0x8,
        EnableOcsp = 0x10
    }

    [Flags]
    public enum QuicCertificateHashStoreFlags
    {
        None = 0,
        MachineStore = 1
    }

    [Flags]
    public enum QuicConnectionShutdownFlags
    {
        None = 0,
        Silent = 1
    }

    public enum QuicStreamSchedulingScheme
    {
        Fifo = 0,
        RoundRobin = 1,
        Count
    }

    [Flags]
    public enum QuicStreamOpenFlags
    {
        None = 0,
        Unidirectional = 0x1,
        ZeroRtt = 0x2,
    }




    [Flags]
    public enum QuicStreamStartFlags
    {
        None = 0x0,
        FailBlocked = 0x1,
        Immediate = 0x2,
        Async = 0x4
    }

    [Flags]
    public enum QuicStreamShutdownFlags
    {
        None = 0x0,
        Graceful = 0x1,
        AbortSend = 0x2,
        AbortReceive = 0x4,
        Abort = 0x6,
        Immediate = 0x8,
    }

    [Flags]
    public enum QuicReceiveFlags
    {
        None = 0x0,
        ZeroRtt = 0x1,
        Fin = 0x2
    }

    [Flags]
    public enum QuicSendFlags
    {
        None = 0x0,
        AllowZeroRtt = 0x1,
        Fin = 0x2,
        DgramPriority = 0x4
    }

    public enum QuicDatagramSendState
    {
        Sent,
        LostSuspect,
        LostDiscarded,
        Acknowledged,
        AcknowledgedSpurious,
        Canceled
    }



    public enum QuicParamLevel
    {
        Global,
        Registration,
        Session,
        Listener,
        Connection,
        Tls,
        Stream
    }

    public enum QuicListenerEventType
    {
        NewConnection
    }

    public enum QuicConnectionEventType
    {
        Connected = 0,
        ShutdownInitiatedByTransport = 1,
        ShutdownInitiatedByPeer = 2,
        ShutdownComplete = 3,
        LocalAddressChanged = 4,
        PeerAddressChanged = 5,
        PeerStreamStarted = 6,
        StreamsAvailable = 7,
        PeerNeedsStreams = 8,
        IdealProcessorChanged = 9,
        DatagramStateChanged = 10,
        DatagramReceived = 11,
        DatagramSendStateChanged = 12
    }

    public enum QuicStreamEventType
    {
        StartComplete,
        Receive,
        SendComplete,
        PeerSendShutdown,
        PeerSendAborted,
        PeerReceiveAborted,
        SendShutdownComplete,
        ShutdownComplete,
        IdealSendBufferSize
    }

#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum QuicAddressFamily : ushort
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        Unspecified = 0,
        Inet = 2,
        Inet6 = 23
    }
}
