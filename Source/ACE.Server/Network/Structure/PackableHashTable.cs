using System;
using System.Collections.Generic;
using System.IO;

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

        public static void WriteOld(this BinaryWriter writer, List<CharacterPropertiesFillCompBook> fillComps)
        {
            WriteHeaderOld(writer, fillComps.Count);
            foreach (var fillComp in fillComps)
            {
                writer.Write((uint)fillComp.SpellComponentId);
                writer.Write((uint)fillComp.QuantityToRebuy);
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
