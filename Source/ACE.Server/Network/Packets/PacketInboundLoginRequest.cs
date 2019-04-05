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

        public PacketInboundLoginRequest(ClientPacket packet)
        {
            string someString = packet.Payload.ReadString16L();         // always "1802"
            uint len = packet.Payload.ReadUInt32();                     // data length left in packet including ticket
            NetAuthType = (NetAuthType)packet.Payload.ReadUInt32();
            var authFlags = (AuthFlags)packet.Payload.ReadUInt32();
            Timestamp = packet.Payload.ReadUInt32();                    // sequence
            Account = packet.Payload.ReadString16L();
            string accountToLoginAs = packet.Payload.ReadString16L();   // special admin only, AuthFlags has 2

            // int consumed = (someString.Length + 2) + 4 * sizeof(uint) + (ClientAccountString.Length + 2) + (unknown3.Length + 2);

            // this packet header has 2 bytes that are proving hard to decipher.  sometimes they occur
            // before the length DWORD at the start of the 32L string, sometimes they are after, in which
            // case the string len is increased by 2 and the 2 bytes are prepended to the string (as
            // garbage and possibly invalid input)
            if (NetAuthType == NetAuthType.AccountPassword)
                Password = packet.Payload.ReadString32L();
            else if(NetAuthType == NetAuthType.GlsTicket)
                GlsTicket = packet.Payload.ReadString32L();
        }
    }
}
