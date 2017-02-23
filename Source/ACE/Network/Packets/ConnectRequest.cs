using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ACE.Common.Cryptography;

namespace ACE.Network.Packets
{
    public class ConnectRequest : ServerPacket2
    {
        private byte[] isaacServerSeed;
        private byte[] isaacClientSeed;

        public ConnectRequest(byte[] isaacServerSeed, byte[] isaacClientSeed) : base(0u)
        {
            this.Header.Flags = PacketHeaderFlags.ConnectRequest;
            this.isaacServerSeed = isaacServerSeed;
            this.isaacClientSeed = isaacClientSeed;
        }

        protected override void WriteBody()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(0u);
                    writer.Write(0u);
                    writer.Write(13626398284849559039ul); // some sort of check value?
                    writer.Write((ushort)0);
                    writer.Write((ushort)0);
                    writer.Write(isaacServerSeed);
                    writer.Write(isaacClientSeed);
                    writer.Write(0u);
                    SetBody(stream.ToArray());
                }
            }
        }
    }
}
