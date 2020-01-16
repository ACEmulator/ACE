using System;
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

        public void Unpack(BinaryReader reader)
        {
            Sequence    = reader.ReadUInt32();
            Flags       = (PacketHeaderFlags)reader.ReadUInt32();
            Checksum    = reader.ReadUInt32();
            Id          = reader.ReadUInt16();
            Time        = reader.ReadUInt16();
            Size        = reader.ReadUInt16();
            Iteration   = reader.ReadUInt16();
        }

        public void Unpack(byte[] buffer, int offset = 0)
        {
            Sequence    =              (uint)(buffer[offset++] | (buffer[offset++] << 8) | (buffer[offset++] << 16) | (buffer[offset++] << 24));
            Flags       = (PacketHeaderFlags)(buffer[offset++] | (buffer[offset++] << 8) | (buffer[offset++] << 16) | (buffer[offset++] << 24));
            Checksum    =              (uint)(buffer[offset++] | (buffer[offset++] << 8) | (buffer[offset++] << 16) | (buffer[offset++] << 24));

            Id          = (ushort)(buffer[offset++] | (buffer[offset++] << 8));
            Time        = (ushort)(buffer[offset++] | (buffer[offset++] << 8));
            Size        = (ushort)(buffer[offset++] | (buffer[offset++] << 8));
            Iteration   = (ushort)(buffer[offset++] | (buffer[offset++] << 8));
        }

        public void Pack(byte[] buffer, int offset = 0)
        {
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

                Pack(buffer);

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
