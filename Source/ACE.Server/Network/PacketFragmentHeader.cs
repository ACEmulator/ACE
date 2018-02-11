using System.IO;
using System.Runtime.InteropServices;

namespace ACE.Server.Network
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketFragmentHeader
    {
        public static uint HeaderSize { get; } = 16u;

        public uint Sequence { get; set; }
        public uint Id { get; set; }
        public ushort Count { get; set; }
        public ushort Size { get; set; }
        public ushort Index { get; set; }
        public ushort Group { get; set; }

        public PacketFragmentHeader() { }

        public PacketFragmentHeader(BinaryReader payload)
        {
            Sequence = payload.ReadUInt32();
            Id = payload.ReadUInt32();
            Count = payload.ReadUInt16();
            Size = payload.ReadUInt16();
            Index = payload.ReadUInt16();
            Group = payload.ReadUInt16();
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
    }
}
