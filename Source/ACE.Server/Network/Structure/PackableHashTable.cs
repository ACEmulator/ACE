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

        private static readonly HashComparer fillCompsComparer = new HashComparer(256);

        public static void Write(this BinaryWriter writer, List<CharacterPropertiesFillCompBook> _fillComps)
        {
            WriteHeader(writer, _fillComps.Count, fillCompsComparer.NumBuckets);

            var table = _fillComps.ToDictionary(i => (uint)i.SpellComponentId, i => (uint)i.QuantityToRebuy);

            var sorted = new SortedDictionary<uint, uint>(table, fillCompsComparer);

            foreach (var fillComp in sorted)
            {
                writer.Write(fillComp.Key);
                writer.Write(fillComp.Value);
            }
        }
    }
}
