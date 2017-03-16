using System;
using System.Collections.Generic;
using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Network
{
    public class ServerPacket : Packet
    {
        //public PacketHeader Header { get; private set; }
        //public MemoryStream Data { get; } = new MemoryStream();

        public BinaryWriter BodyWriter { get; private set; }

        private List<ServerPacketFragment> fragments = new List<ServerPacketFragment>();

        private uint issacXor = 0u;
        private bool issacXorSet = false;
        public uint IssacXor
        {
            get
            {
                return issacXor;
            }
            set
            {
                if (issacXorSet)
                    throw new InvalidOperationException("IssacXor can only be set once!");

                issacXorSet = true;
                issacXor = value;
            }
        }

        public ServerPacket()
        {
            Header = new PacketHeader();
            Data = new MemoryStream();
            BodyWriter = new BinaryWriter(Data);
        }

        public void AddFragment(ServerPacketFragment fragment)
        {
            fragments.Add(fragment);
        }

        public byte[] GetPayload()
        {
            uint headerChecksum = 0u;
            uint bodyChecksum = 0u;
            uint fragmentChecksum = 0u;

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Seek((int)PacketHeader.HeaderSize, SeekOrigin.Begin);

                    if (Data.Length > 0)
                    {
                        var body = Data.ToArray();
                        writer.Write(body);
                        bodyChecksum = Hash32.Calculate(body, body.Length);
                    }
                    foreach (ServerPacketFragment fragment in fragments)
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