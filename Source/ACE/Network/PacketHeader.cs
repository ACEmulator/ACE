using ACE.Common.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketHeader
    {
        public static uint HeaderSize { get; } = 20u;

        public uint Sequence { get; set; }
        public PacketHeaderFlags Flags { get; set; }
        public uint Checksum { get; set; }
        public ushort Id { get; set; }
        public ushort Time { get; set; }
        public ushort Size { get; set; }
        public ushort Table { get; set; }

        public PacketHeader() { }

        public PacketHeader(BinaryReader payload)
        {
            Sequence = payload.ReadUInt32();
            Flags = (PacketHeaderFlags)payload.ReadUInt32();
            Checksum = payload.ReadUInt32();
            Id = payload.ReadUInt16();
            Time = payload.ReadUInt16();
            Size = payload.ReadUInt16();
            Table = payload.ReadUInt16();
        }

        public byte[] GetRaw()
        {
            var headerHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
            try
            {
                byte[] bytes = new byte[Marshal.SizeOf(typeof(PacketHeader))];
                Marshal.Copy(headerHandle.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                headerHandle.Free();
            }
        }

        public void CalculateHash32(out uint checksum)
        {
            uint original = Checksum;

            Checksum = 0x0BADD70DD;
            byte[] rawHeader = GetRaw();
            checksum = Hash32.Calculate(rawHeader, rawHeader.Length);
            Checksum = original;
        }

        public bool HasFlag(PacketHeaderFlags flags) { return (flags & Flags) != 0; }
    }
}
