using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    static class UnpackableExtensions
    {
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


        /// <summary>
        /// A SmartArray uses a Compressed UInt32 for the length.
        /// </summary>
        public static void UnpackSmartArray<T>(this Dictionary<ushort, T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadCompressedUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt16();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
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


        /// <summary>
        /// A PackedHashTable uses a UInt16 for length, and a UInt16 for bucket size.
        /// We don't need to worry about the bucket size with C#.
        /// </summary>
        public static void UnpackPackedHashTable(this Dictionary<uint, uint> value, BinaryReader reader)
        {
            var totalObjects = reader.ReadUInt16();
            /*var bucketSize = */
            reader.ReadUInt16();

            for (int i = 0; i < totalObjects; i++)
                value.Add(reader.ReadUInt32(), reader.ReadUInt32());
        }

        /// <summary>
        /// A PackedHashTable uses a UInt16 for length, and a UInt16 for bucket size.
        /// We don't need to worry about the bucket size with C#.
        /// </summary>
        public static void UnpackPackedHashTable<T>(this Dictionary<uint, T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadUInt16();
            /*var bucketSize = */reader.ReadUInt16();

            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt32();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
            }
        }

        /// <summary>
        /// A PackedHashTable uses a UInt16 for length, and a UInt16 for bucket size.
        /// We don't need to worry about the bucket size with C#.
        /// </summary>
        public static void UnpackPackedHashTable<T>(this SortedDictionary<uint, T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadUInt16();
            /*var bucketSize = */
            reader.ReadUInt16();

            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt32();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
            }
        }

        /// <summary>
        /// A list that uses a Int32 for the length.
        /// </summary>
        public static void Unpack(this List<uint> value, BinaryReader reader)
        {
            var totalObjects = reader.ReadInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var item = reader.ReadUInt32();
                value.Add(item);
            }
        }

        /// <summary>
        /// A list that uses a UInt32 for the length.
        /// </summary>
        public static void Unpack<T>(this List<T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var item = new T();
                item.Unpack(reader);
                value.Add(item);
            }
        }

        public static void Unpack<T>(this List<T> value, BinaryReader reader, uint fixedQuantity) where T : IUnpackable, new()
        {
            for (int i = 0; i < fixedQuantity; i++)
            {
                var item = new T();
                item.Unpack(reader);
                value.Add(item);
            }
        }


        public static void Unpack<T>(this Dictionary<ushort, T> value, BinaryReader reader, uint fixedQuantity) where T : IUnpackable, new()
        {
            for (int i = 0; i < fixedQuantity; i++)
            {
                var key = reader.ReadUInt16();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
            }
        }

        /// <summary>
        /// A Dictionary that uses a Int32 for the length.
        /// </summary>
        public static void Unpack<T>(this Dictionary<int, T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadInt32();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
            }
        }

        /// <summary>
        /// A Dictionary that uses a Int32 for the length.
        /// </summary>
        public static void Unpack<T>(this Dictionary<uint, T> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt32();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
            }
        }

        public static void Unpack<T>(this Dictionary<uint, T> value, BinaryReader reader, uint fixedQuantity) where T : IUnpackable, new()
        {
            for (int i = 0; i < fixedQuantity; i++)
            {
                var key = reader.ReadUInt32();

                var item = new T();
                item.Unpack(reader);
                value.Add(key, item);
            }
        }

        /// <summary>
        /// A Dictionary that uses a Int32 for the length.
        /// </summary>
        public static void Unpack<T>(this Dictionary<uint, Dictionary<uint, T>> value, BinaryReader reader) where T : IUnpackable, new()
        {
            var totalObjects = reader.ReadUInt32();

            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt32();

                var values = new Dictionary<uint, T>();
                values.Unpack(reader);

                value.Add(key, values);
            }
        }
    }
}
