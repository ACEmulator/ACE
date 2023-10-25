using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network.Packets
{
    public class PacketInboundLoginRequest
    {
        public NetAuthType NetAuthType { get; }

        public uint Timestamp { get; }    

        public string Account { get; }

        public string Password { get; }

        public string GlsTicket { get; }

        public string ClientVersion { get; }

        public PacketInboundLoginRequest(ClientPacket packet)
        {
            ClientVersion = packet.DataReader.ReadString16L();      // should be "1802" for end of retail client
            uint len = packet.DataReader.ReadUInt32();                     // data length left in packet including ticket
            NetAuthType = (NetAuthType)packet.DataReader.ReadUInt32();
            var authFlags = (AuthFlags)packet.DataReader.ReadUInt32();
            Timestamp = packet.DataReader.ReadUInt32();                    // sequence
            Account = packet.DataReader.ReadString16L();
            string accountToLoginAs = packet.DataReader.ReadString16L();   // special admin only, AuthFlags has 2

            if (NetAuthType == NetAuthType.AccountPassword)
                Password = packet.DataReader.ReadString32L();
            else if (NetAuthType == NetAuthType.GlsTicket)
                GlsTicket = packet.DataReader.ReadString32L();
        }
    }
}
