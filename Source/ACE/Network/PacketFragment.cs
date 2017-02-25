using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public abstract class PacketFragment
    {
        public static uint MaxFragementSize { get; } = 464u; // Packet.MaxPacketSize - PacketHeader.HeaderSize
        public static uint MaxFragmentDataSize { get; } = 448u; // Packet.MaxPacketSize - PacketHeader.HeaderSize - PacketFragmentHeader.HeaderSize

        public PacketFragmentHeader Header { get; protected set; }
    }
}
