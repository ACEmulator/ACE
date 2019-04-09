using System;
using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class ServerPacket : Packet
    {
        // TODO: I don't know why this value is 464. The reasoning and math needs to be documented here.
        public static int MaxPacketSize { get; } = 464;

        /// <summary>
        /// Make sure you call InitializeBodyWriter() before you use this
        /// </summary>
        public BinaryWriter BodyWriter { get; private set; }

        private uint finalChecksum;
        private uint issacXor;
        private bool issacXorSet;
        public uint IssacXor
        {
            get => issacXor;
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
        }

        /// <summary>
        /// This will initailize BodyWriter for use.
        /// </summary>
        public void InitializeBodyWriter(int initialCapacity = 32) // 32 is the max length I saw in AddPayloadToBuffer()
        {
            if (BodyWriter == null)
            {
                Data = new MemoryStream(initialCapacity);
                BodyWriter = new BinaryWriter(Data);
            }
        }

        public void CreateReadyToSendPacket(byte[] buffer, out int size)
        {
            uint payloadChecksum = 0u;

            int offset = PacketHeader.HeaderSize;

            if (Data != null && Data.Length > 0)
            {
                var body = Data.ToArray();
                Buffer.BlockCopy(body, 0, buffer, offset, body.Length);
                offset += body.Length;

                payloadChecksum += Hash32.Calculate(body, body.Length);
            }

            foreach (ServerPacketFragment fragment in Fragments)
                payloadChecksum += fragment.AddPayloadToBuffer(buffer, ref offset);

            size = offset;

            Header.Size = (ushort)(size - PacketHeader.HeaderSize);

            var headerChecksum = Header.CalculateHash32();
            finalChecksum = headerChecksum + payloadChecksum;
            Header.Checksum = headerChecksum + (payloadChecksum ^ issacXor);
            Header.AddPayloadToBuffer(buffer);
        }

        public override string ToString()
        {
            var c = Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) ? $" CRC: {finalChecksum} XOR: {issacXor}" : "";
            return $">>> {Header}{c}".TrimEnd();
        }
    }
}
