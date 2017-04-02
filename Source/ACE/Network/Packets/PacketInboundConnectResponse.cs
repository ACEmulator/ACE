using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Packets
{
    public class PacketInboundConnectResponse
    {
        public ulong Check { get; }

        public PacketInboundConnectResponse(ClientPacket packet)
        {
            Check = packet.Payload.ReadUInt64();
        }
    }
}
