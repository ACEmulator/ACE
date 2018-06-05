using System.Collections.Generic;
using System.IO;

namespace ACE.Server.Network
{
    public abstract class Packet
    {
        public static uint MaxPacketSize { get; } = 1024;

        public static uint MaxPacketDataSize { get; } = 464u;

        public PacketHeader Header { get; protected set; }
        public MemoryStream Data { get; internal set; }
        public List<PacketFragment> Fragments { get; } = new List<PacketFragment>();
    }
}
