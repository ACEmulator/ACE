using System.Collections.Generic;
using System.IO;

namespace ACE.Network
{
    public abstract class Packet
    {
        public static uint MaxPacketSize { get; } = 484u;

        public static uint MaxPacketDataSize { get; } = 464u;

        public PacketHeader Header { get; protected set; }
        public MemoryStream Data { get; protected set; }
        public List<PacketFragment> Fragments { get; } = new List<PacketFragment>();
    }
}
