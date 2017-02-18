using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class ServerPacket : Packet
    {
        public BinaryWriter Payload { get; }

        public ServerPacket(ushort id, PacketHeaderFlags flags = PacketHeaderFlags.None)
        {
            Direction = PacketDirection.Server;

            Data = new MemoryStream((int)MaxPacketSize);
            Payload = new BinaryWriter(Data);
            Header = new PacketHeader()
            {
                Id = id,
                Flags = flags
            };

        }
    }
}