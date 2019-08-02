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
        /// <summary>
        /// Deprecated
        /// </summary>
        public static void WriteHeader(BinaryWriter writer, int numEntries)
        {
            var numBits = GetNumBits(numEntries);
            var size = 1 << ((int)numBits - 1);

            writer.Write((ushort)numEntries);
            writer.Write((ushort)size);
        }

        public static void Write(this BinaryWriter writer, List<CharacterPropertiesFillCompBook> fillComps)
        {
            WriteHeader(writer, fillComps.Count);
            foreach (var fillComp in fillComps)
            {
                writer.Write((uint)fillComp.SpellComponentId);
                writer.Write((uint)fillComp.QuantityToRebuy);
            }
        }

        /// <summary>
        /// Returns the number of bits required to store the input number
        /// </summary>
        public static uint GetNumBits(int num)
        {
            return (uint)Math.Log(num, 2) + 1;
        }
    }

    public class PackableHashTable<T, U>
    {
        public Dictionary<T, U> HashTable;

        public PackableHashTable(Dictionary<T, U> hashTable)
        {
            HashTable = hashTable;
        }
    }

    public static class PackableHashTableExtensions<T, U>
    {
        public static void Write(BinaryWriter writer, PackableHashTable<T, U> packableHashTable)
        {
            WriteHeader(writer, packableHashTable);
            WriteTable(writer, packableHashTable);
        }

        public static void WriteHeader(BinaryWriter writer, PackableHashTable<T, U> packableHashTable)
        {
            // ushort - count - number of items in the table
            // ushort - tableSize - max size of the table
            var numEntries = (ushort)packableHashTable.HashTable.Count;
            var numBits = GetNumBits(numEntries);
            var size = 1 << ((int)numBits - 1);

            writer.Write(numEntries);
            writer.Write((ushort)size);
        }

        public static void WriteTable(BinaryWriter writer, PackableHashTable<T, U> packableHashTable)
        {
            // table: vector of length count
            foreach (var kvp in packableHashTable.HashTable)
            {
                //writer.Write(kvp.Key);    // TODO: serializable
                //writer.Write(kvp.Value);
            }
        }

        /// <summary>
        /// Returns the number of bits required to store the input number
        /// </summary>
        public static uint GetNumBits(uint num)
        {
            return (uint)Math.Log(num, 2) + 1;
        }
    }
}
