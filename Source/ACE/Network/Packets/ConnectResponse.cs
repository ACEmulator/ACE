using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Packets
{
    public class ConnectResponse
    {
        public ulong Check { get; private set; }

        public ConnectResponse(ClientPacket packet)
        {
            Check = packet.Payload.ReadUInt64(); // 13626398284849559039 - sent in previous packet
        }
    }
}
