using System.Collections.Generic;
using System.IO;

namespace ACE.Server.Network
{
    public abstract class Packet
    {
        public PacketHeader Header { get; } = new PacketHeader();
        public MemoryStream Data { get; internal set; }
        public List<PacketFragment> Fragments { get; } = new List<PacketFragment>();
    }
}
