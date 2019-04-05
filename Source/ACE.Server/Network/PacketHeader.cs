using System.Buffers;
using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class PacketHeader
    {
        public static int HeaderSize { get; } = 20;

        public uint Sequence { get; set; }
        public PacketHeaderFlags Flags { get; set; }
        public uint Checksum { get; set; }
        public ushort Id { get; set; }
        public ushort Time { get; set; }
        public ushort Size { get; set; }
        public ushort Iteration { get; set; }

        public PacketHeader() { }

        public PacketHeader(BinaryReader payload)
        {
            Sequence = payload.ReadUInt32();
            Flags = (PacketHeaderFlags)payload.ReadUInt32();
            Checksum = payload.ReadUInt32();
            Id = payload.ReadUInt16();
            Time = payload.ReadUInt16();
            Size = payload.ReadUInt16();
            Iteration = payload.ReadUInt16();
        }

        public void AddPayloadToBuffer(byte[] buffer)
        {
            int offset = 0;

            buffer[offset++] = (byte)Sequence;
            buffer[offset++] = (byte)(Sequence >> 8);
            buffer[offset++] = (byte)(Sequence >> 16);
            buffer[offset++] = (byte)(Sequence >> 24);

            buffer[offset++] = (byte)Flags;
            buffer[offset++] = (byte)((int)Flags >> 8);
            buffer[offset++] = (byte)((int)Flags >> 16);
            buffer[offset++] = (byte)((int)Flags >> 24);

            buffer[offset++] = (byte)Checksum;
            buffer[offset++] = (byte)(Checksum >> 8);
            buffer[offset++] = (byte)(Checksum >> 16);
            buffer[offset++] = (byte)(Checksum >> 24);

            buffer[offset++] = (byte)Id;
            buffer[offset++] = (byte)(Id >> 8);

            buffer[offset++] = (byte)Time;
            buffer[offset++] = (byte)(Time >> 8);

            buffer[offset++] = (byte)Size;
            buffer[offset++] = (byte)(Size >> 8);

            buffer[offset++] = (byte)Iteration;
            buffer[offset++] = (byte)(Iteration >> 8);
        }

        public uint CalculateHash32()
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(HeaderSize);

            try
            {
                uint original = Checksum;
                Checksum = 0xBADD70DD;

                AddPayloadToBuffer(buffer);

                var checksum = Hash32.Calculate(buffer, HeaderSize);
                Checksum = original;
                return checksum;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public bool HasFlag(PacketHeaderFlags flags) { return (flags & Flags) != 0; }

        public override string ToString()
        {
            var c = HasFlag(PacketHeaderFlags.EncryptedChecksum) ? "X" : "";
            return $"Seq: {Sequence} Id: {Id} Iter: {Iteration} {c}CRC: {Checksum} {PacketHeaderFlagsUtil.UnfoldFlags(Flags)}";
        }
    }
}
