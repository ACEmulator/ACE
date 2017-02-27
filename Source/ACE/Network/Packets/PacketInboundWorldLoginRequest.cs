using ACE.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Packets
{
    public class PacketInboundWorldLoginRequest
    {
        public ulong ConnectionKey { get; private set; }

        public PacketInboundWorldLoginRequest(ClientPacket packet)
        {
            ConnectionKey = packet.Payload.ReadUInt64();
        }
    }
}
