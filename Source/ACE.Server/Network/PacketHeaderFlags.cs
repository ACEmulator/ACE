using System;

namespace ACE.Server.Network
{
    [Flags]
    public enum PacketHeaderFlags : uint
    {
        None              = 0x00000000,
        Retransmission    = 0x00000001,
        EncryptedChecksum = 0x00000002, // can't be paired with 0x00000001, see FlowQueue::DequeueAck
        BlobFragments     = 0x00000004,
        ServerSwitch      = 0x00000100,
        Referral          = 0x00000800,
        RequestRetransmit = 0x00001000,
        RejectRetransmit  = 0x00002000,
        AckSequence       = 0x00004000,
        Disconnect        = 0x00008000,
        LoginRequest      = 0x00010000,
        WorldLoginRequest = 0x00020000,
        ConnectRequest    = 0x00040000,
        ConnectResponse   = 0x00080000,
        CICMDCommand      = 0x00400000,
        TimeSynch         = 0x01000000,
        EchoRequest       = 0x02000000,
        EchoResponse      = 0x04000000,
        Flow              = 0x08000000
    }
}
