using System.IO;
using System.Runtime.InteropServices;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketFragmentHeader
    {
        public static int HeaderSize { get; } = 16;

        public uint Sequence { get; set; }
        public uint Id { get; set; }
        public ushort Count { get; set; }
        public ushort Size { get; set; }
        public ushort Index { get; set; }
        public ushort Queue { get; set; }

        public PacketFragmentHeader() { }

        public PacketFragmentHeader(BinaryReader payload)
        {
            Sequence = payload.ReadUInt32();
            Id = payload.ReadUInt32();
            Count = payload.ReadUInt16();
            Size = payload.ReadUInt16();
            Index = payload.ReadUInt16();
            Queue = payload.ReadUInt16();
        }

        public byte[] GetRaw()
        {
            var headerHandle = GCHandle.Alloc(this, GCHandleType.Pinned);

            try
            {
                byte[] bytes = new byte[Marshal.SizeOf(typeof(PacketFragmentHeader))];
                Marshal.Copy(headerHandle.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                headerHandle.Free();
            }
        }

        /// <summary>
        /// Returns the Hash32 of the payload added to buffer
        /// </summary>
        public uint AddPayloadToBuffer(byte[] buffer, ref int offset)
        {
            buffer[offset++] = (byte)Sequence;
            buffer[offset++] = (byte)(Sequence >> 8);
            buffer[offset++] = (byte)(Sequence >> 16);
            buffer[offset++] = (byte)(Sequence >> 24);

            buffer[offset++] = (byte)Id;
            buffer[offset++] = (byte)(Id >> 8);
            buffer[offset++] = (byte)(Id >> 16);
            buffer[offset++] = (byte)(Id >> 24);

            buffer[offset++] = (byte)Count;
            buffer[offset++] = (byte)(Count >> 8);

            buffer[offset++] = (byte)Size;
            buffer[offset++] = (byte)(Size >> 8);

            buffer[offset++] = (byte)Index;
            buffer[offset++] = (byte)(Index >> 8);

            buffer[offset++] = (byte)Queue;
            buffer[offset++] = (byte)(Queue >> 8);

            return Hash32.Calculate(buffer, offset - HeaderSize, HeaderSize);
        }
    }
}
