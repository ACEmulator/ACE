using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// HashTable which is packable for network
    /// </summary>
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
                //writer.Write(kvp.Key);    // doh
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
