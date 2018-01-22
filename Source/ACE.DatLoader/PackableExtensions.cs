using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    static class PackableExtensions
    {
        /// <summary>
        /// Aligns the stream to the next DWORD boundary.
        /// </summary>
        public static void AlignBoundary(this BinaryReader reader)
        {
            // Aligns the DatReader to the next DWORD boundary.
            long alignDelta = reader.BaseStream.Position % 4;

            if (alignDelta != 0)
                reader.BaseStream.Position += (int)(4 - alignDelta);
        }


        /// <summary>
        /// A Compressed UInt32 can be 1, 2, or 4 bytes.<para />
        /// If the first MSB (0x80) is 0, it is one byte.<para />
        /// If the first MSB (0x80) is set and the second MSB (0x40) is 0, it's 2 bytes.<para />
        /// If both (0x80) and (0x40) are set, it's 4 bytes.
        /// </summary>
        public static uint ReadCompressedUInt32(this BinaryReader reader)
        {
            var b0 = reader.ReadByte();
            if ((b0 & 0x80) == 0)
                return b0;

            var b1 = reader.ReadByte();
            if ((b0 & 0x40) == 0)
                return (uint)(((b0 & 0x7F) << 8) | b1);

            var s = reader.ReadUInt16();
            return (uint)(((((b0 & 0x3F) << 8) | b1) << 16) | s);
        }

        /// <summary>
        /// First reads a UInt16. If the MSB is set, it will be masked with 0x3FFF, shifted left 2 bytes, and then OR'd with the next UInt16. The sum is then added to knownType.
        /// </summary>
        public static uint ReadAsDataIDOfKnownType(this BinaryReader reader, uint knownType)
        {
            var value = reader.ReadUInt16();

            if ((value & 0x8000) != 0)
            {
                var lower = reader.ReadUInt16();
                var higher = (value & 0x3FFF) << 16;

                return (uint)(knownType + (higher | lower));
            }

            return (knownType + value);
        }


        /// <summary>
        /// A SmartArray uses a Compressed UInt32 for the length.
        /// </summary>
        public static void UnpackSmartArray(this List<int> value, BinaryReader reader)
        {
            var totalObjects = reader.ReadCompressedUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var item = reader.ReadInt32();
                value.Add(item);
            }
        }

        /// <summary>
        /// A SmartArray uses a Compressed UInt32 for the length.
        /// </summary>
        public static void UnpackSmartArray(this List<uint> value, BinaryReader reader)
        {
            var totalObjects = reader.ReadCompressedUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var item = reader.ReadUInt32();
                value.Add(item);
            }
        }

        /// <summary>
        /// A SmartArray uses a Compressed UInt32 for the length.
        /// </summary>
        public static void UnpackSmartArray<T>(this List<T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadCompressedUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var item = new T();
                item.Unpack(reader);
                value.Add(item);
            }
        }

        public static void Unpack<T>(this List<T> value, BinaryReader reader, int fixedQuantity) where T : IUnpackable, new()
        {
            for (int i = 0; i < fixedQuantity; i++)
            {
                var item = new T();
                item.Unpack(reader);
                value.Add(item);
            }
        }


        /// <summary>
        /// A SmartArray uses a Compressed UInt32 for the length.
        /// </summary>
        public static void UnpackSmartArray<T>(this Dictionary<int, T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadCompressedUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadInt32();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
            }
        }

        /// <summary>
        /// A SmartArray uses a Compressed UInt32 for the length.
        /// </summary>
        public static void UnpackSmartArray<T>(this Dictionary<uint, T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadCompressedUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt32();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
            }
        }
    }
}
