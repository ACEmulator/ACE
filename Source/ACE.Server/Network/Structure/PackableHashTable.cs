using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database.Models.Shard;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// HashTable which is packable for network
    /// </summary>
    public static class PackableHashTable
    {
        public static void WriteHeader(BinaryWriter writer, int count, int numBuckets)
        {
            writer.Write((ushort)count);
            writer.Write((ushort)numBuckets);
        }

        /// <summary>
        /// Deprecated -- this should be in PHashTable
        /// </summary>
        public static void WriteHeaderOld(BinaryWriter writer, int numEntries)
        {
            var numBits = GetNumBits(numEntries);
            var size = 1 << ((int)numBits - 1);

            writer.Write((ushort)numEntries);
            writer.Write((ushort)size);
        }

        private static readonly HashComparer FillCompsComparer = new HashComparer(256);

        public static void Write(this BinaryWriter writer, List<CharacterPropertiesFillCompBook> _fillComps)
        {
            WriteHeader(writer, _fillComps.Count, FillCompsComparer.NumBuckets);

            var fillComps = _fillComps.ToDictionary(i => (uint)i.SpellComponentId, i => (uint)i.QuantityToRebuy);

            var sorted = new SortedDictionary<uint, uint>(fillComps, FillCompsComparer);

            foreach (var fillComp in sorted)
            {
                writer.Write(fillComp.Key);
                writer.Write(fillComp.Value);
            }
        }

        /// <summary>
        /// Returns the number of bits required to store the input number
        /// </summary>
        private static uint GetNumBits(int num)
        {
            return (uint)Math.Log(num, 2) + 1;
        }
    }
}
