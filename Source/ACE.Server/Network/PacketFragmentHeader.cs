using System;
using System.Buffers.Binary;
using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class PacketFragmentHeader
    {
        public const int HeaderSize = 16;

        public uint Sequence { get; set; }
        public uint Id { get; set; }
        public ushort Count { get; set; }
        public ushort Size { get; set; }
        public ushort Index { get; set; }
        public ushort Queue { get; set; }

        public void Unpack(BinaryReader reader)
        {
            Sequence    = reader.ReadUInt32();
            Id          = reader.ReadUInt32();
            Count       = reader.ReadUInt16();
            Size        = reader.ReadUInt16();
            Index       = reader.ReadUInt16();
            Queue       = reader.ReadUInt16();
        }

        public void Unpack(byte[] buffer, int offset = 0)
        {
            Sequence    =   (uint)(buffer[offset++] | (buffer[offset++] << 8) | (buffer[offset++] << 16) | (buffer[offset++] << 24));
            Id          =   (uint)(buffer[offset++] | (buffer[offset++] << 8) | (buffer[offset++] << 16) | (buffer[offset++] << 24));
            Count       = (ushort)(buffer[offset++] | (buffer[offset++] << 8));
            Size        = (ushort)(buffer[offset++] | (buffer[offset++] << 8));
            Index       = (ushort)(buffer[offset++] | (buffer[offset++] << 8));
            Queue       = (ushort)(buffer[offset++] | (buffer[offset++] << 8));
        }

        public void Pack(Span<byte> buffer, int offset = 0)
        {
            Pack(buffer, ref offset);
        }

        public void Pack(Span<byte> buffer, ref int offset)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset), Sequence);
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset + 4), Id);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset + 8), Count);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset + 10), Size);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset + 12), Index);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset + 14), Queue);

            offset += 16;
        }

        /// <summary>
        /// Returns the Hash32 of the payload added to buffer
        /// </summary>
        public uint PackAndReturnHash32(byte[] buffer, ref int offset)
        {
            Pack(buffer, ref offset);

            return Hash32.Calculate(buffer, offset - HeaderSize, HeaderSize);
        }
    }
}
