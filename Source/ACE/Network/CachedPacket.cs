using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class CachedPacket
    {
        public uint IssacXor { get; }
        public ServerPacket Packet { get; }

        public CachedPacket(uint issacXor, ServerPacket packet)
        {
            IssacXor = issacXor;
            Packet = packet;
        }
    }
}
