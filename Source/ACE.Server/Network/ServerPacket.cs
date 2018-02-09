using System;
using System.IO;
using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class ServerPacket : Packet
    {
        public BinaryWriter BodyWriter { get; }

        private uint issacXor;
        private bool issacXorSet;
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
                    foreach (ServerPacketFragment fragment in Fragments)
                    {
                        fragmentChecksum += fragment.GetPayload(writer);
                    }

                    Header.Size = (ushort)(stream.Length - PacketHeader.HeaderSize);
                    headerChecksum = Header.CalculateHash32();
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
