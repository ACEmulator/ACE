using ACE.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Packets
{
    public class PacketInboundLoginRequest
    {
        public uint Timestamp { get; }    

        public string Account { get; }

        public string GlsTicket { get; }

        public PacketInboundLoginRequest(ClientPacket packet)
        {
            string someString = packet.Payload.ReadString16L(); // always "1802"
            uint len = packet.Payload.ReadUInt32(); // data length left in packet including ticket
            uint unknown1 = packet.Payload.ReadUInt32();
            uint unknown2 = packet.Payload.ReadUInt32();
            Timestamp = packet.Payload.ReadUInt32();
            Account = packet.Payload.ReadString16L();
            string unknown3 = packet.Payload.ReadString16L();

            // int consumed = (someString.Length + 2) + 4 * sizeof(uint) + (ClientAccountString.Length + 2) + (unknown3.Length + 2);

            // this packet header has 2 bytes that are proving hard to decipher.  sometimes they occur
            // before the length DWORD at the start of the 32L string, sometimes they are after, in which
            // case the string len is increased by 2 and the 2 bytes are prepended to the string (as
            // garbage and possibly invalid input)

            GlsTicket = packet.Payload.ReadString32L();
        }
    }
}
