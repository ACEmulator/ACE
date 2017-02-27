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
        public uint Timestamp { get; private set; }
        public string Account { get; private set; }
        public string GlsTicket { get; private set; }

        public PacketInboundLoginRequest(ClientPacket packet)
        {
            string someString = packet.Payload.ReadString16L();
            packet.Payload.ReadUInt32(); // data length left in packet including ticket
            packet.Payload.ReadUInt32();
            packet.Payload.ReadUInt32();
            Timestamp = packet.Payload.ReadUInt32();
            Account = packet.Payload.ReadString16L();
            packet.Payload.ReadUInt32();
            GlsTicket = packet.Payload.ReadString32L();
        }
    }
}
