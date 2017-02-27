using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ACE.Common.Cryptography;

namespace ACE.Network.Packets
{
    public class PacketOutboundConnectRequest : ServerPacket
    {
        public PacketOutboundConnectRequest(byte[] isaacServerSeed, byte[] isaacClientSeed) : base()
        {
            this.Header.Flags = PacketHeaderFlags.ConnectRequest;
            BodyWriter.Write(0u);
            BodyWriter.Write(0u);
            BodyWriter.Write(13626398284849559039ul); // some sort of check value?
            BodyWriter.Write((ushort)0);
            BodyWriter.Write((ushort)0);
            BodyWriter.Write(isaacServerSeed);
            BodyWriter.Write(isaacClientSeed);
            BodyWriter.Write(0u);
        }
    }
}
