using System;
using System.Buffers.Binary;
using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class PacketHeader
    {
        public const int HeaderSize = 20;

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

        public void Pack(Span<byte> buffer, int offset = 0)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset), Sequence);
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset + 4), (uint)Flags);
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset + 8), Checksum);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset + 12), Id);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset + 14), Time);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset + 16), Size);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset + 18), Iteration);
        }

        public uint CalculateHash32()
        {
            Span<byte> buffer = stackalloc byte[HeaderSize];

            uint original = Checksum;
            Checksum = 0xBADD70DD;

            Pack(buffer);

            var checksum = Hash32.Calculate(buffer, HeaderSize);
            Checksum = original;

            return checksum;
        }

        public bool HasFlag(PacketHeaderFlags flags) { return (flags & Flags) != 0; }

        public override string ToString()
        {
            var c = HasFlag(PacketHeaderFlags.EncryptedChecksum) ? "X" : "";

            return $"Seq: {Sequence} Id: {Id} Iter: {Iteration} {c}CRC: {Checksum} {PacketHeaderFlagsUtil.UnfoldFlags(Flags)}";
        }
    }
}
