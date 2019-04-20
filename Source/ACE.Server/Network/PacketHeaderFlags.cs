using System;

namespace ACE.Server.Network
{
    [Flags]
    public enum PacketHeaderFlags : uint  //ACE
    {
        None                = 0x00000000,
        Retransmission      = 0x00000001,
        EncryptedChecksum   = 0x00000002,   // can't be paired with 0x00000001, see FlowQueue::DequeueAck
        BlobFragments       = 0x00000004,
        ServerSwitch        = 0x00000100,   // Server Switch
        LogonServerAddr     = 0x00000200,   // Logon Server Addr
        EmptyHeader1        = 0x00000400,   // Empty Header 1
        Referral            = 0x00000800,   // Referral
        RequestRetransmit   = 0x00001000,   // Nak
        RejectRetransmit    = 0x00002000,   // Empty Ack
        AckSequence         = 0x00004000,   // Pak
        Disconnect          = 0x00008000,   // Empty Header 2
        LoginRequest        = 0x00010000,   // Login
        WorldLoginRequest   = 0x00020000,   // ULong 1
        ConnectRequest      = 0x00040000,   // Connect
        ConnectResponse     = 0x00080000,   // ULong 2
        NetError            = 0x00100000,   // Net Error
        NetErrorDisconnect  = 0x00200000,   // Net Error Disconnect
        CICMDCommand        = 0x00400000,   // ICmd
        TimeSync            = 0x01000000,   // Time Sync
        EchoRequest         = 0x02000000,   // Echo Request
        EchoResponse        = 0x04000000,   // Echo Response
        Flow                = 0x08000000    // Flow
    }
}
