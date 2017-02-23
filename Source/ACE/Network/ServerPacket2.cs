using ACE.Common.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class ServerPacket2
    {
        public PacketHeader Header { get; private set; }
        private byte[] body = null;
        private List<ServerPacketFragment2> fragments = new List<ServerPacketFragment2>();
        private uint issacXor;

        public ServerPacket2(uint issacXor)
        {
            Header = new PacketHeader();
            this.issacXor = issacXor;
        }

        public void SetBody(byte[] body)
        {
            this.body = body;
        }

        public void AddFragment(ServerPacketFragment2 fragment)
        {
            fragments.Add(fragment);
        }

        protected virtual void WriteBody()
        {

        }

        public byte[] GetPayload()
        {
            WriteBody();
            uint headerChecksum = 0u;
            uint bodyChecksum = 0u;
            uint fragmentChecksum = 0u;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Seek((int)PacketHeader.HeaderSize, SeekOrigin.Begin);
                    if (body != null)
                    {
                        writer.Write(body);
                        bodyChecksum = Hash32.Calculate(body, body.Length);
                    }
                    foreach (ServerPacketFragment2 fragment in fragments)
                    {
                        fragmentChecksum += fragment.GetPayload(writer);
                    }

                    Header.Size = (ushort)(stream.Length - PacketHeader.HeaderSize);
                    Header.CalculateHash32(out headerChecksum);
                    uint payloadChecksum = bodyChecksum + fragmentChecksum;
                    Header.Checksum = headerChecksum + (payloadChecksum ^ issacXor);
                    writer.Seek(0, SeekOrigin.Begin);
                    writer.Write(Header.GetRaw());
                    writer.Flush();
                    return stream.ToArray();
                }
            }
        }
    }
}