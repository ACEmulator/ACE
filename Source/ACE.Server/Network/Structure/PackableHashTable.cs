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

        public static void WriteOld(this BinaryWriter writer, List<CharacterPropertiesFillCompBook> fillComps)
        {
            const int numBuckets = 256; // constant from retail pcaps

            WriteHeader(writer, fillComps.Count, numBuckets);

            var sorted = fillComps.OrderBy(i => i.SpellComponentId % numBuckets);

            foreach (var fillComp in sorted)
            {
                writer.Write(fillComp.SpellComponentId);
                writer.Write(fillComp.QuantityToRebuy);
            }
        }
    }
}
