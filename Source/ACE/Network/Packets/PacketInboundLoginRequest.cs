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

        public string ClientAccountString { get; }

        /// <summary>
        /// formerly the GLS ticket.  We don't have GLS, so we're going to use Jwt.
        /// </summary>
        public string JwtToken { get; }

        public PacketInboundLoginRequest(ClientPacket packet)
        {
            string someString = packet.Payload.ReadString16L();
            packet.Payload.ReadUInt32(); // data length left in packet including ticket
            packet.Payload.ReadUInt32();
            packet.Payload.ReadUInt32();
            Timestamp = packet.Payload.ReadUInt32();
            ClientAccountString = packet.Payload.ReadString16L();
            packet.Payload.ReadUInt32();
            // there's 2 more byte before the next string actually starts
            packet.Payload.ReadBytes(2);
            JwtToken = packet.Payload.ReadString32L();
        }
    }
}
