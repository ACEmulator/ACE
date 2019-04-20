using System;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public static class PHashTable
    {
        /// <summary>
        /// Writes a PHashTable header to the network stream
        /// </summary>
        /// <param name="count">The number of entries in the HashTable</param>
        public static void WriteHeader(BinaryWriter writer, int count)
        {
            // uint uint uint - packedSize - write: (buckets) | (count & 0xFFFFFF)
            // uint - buckets - read: 1 << (packedSize >> 24)
            // uint - count - read: packedSize & 0xFFFFFF
            var bucketShift = GetNumBits((uint)count)/* - 1*/;
            //var maxSize = 1 << ((int)bucketShift - 1);
            var packedSize = (bucketShift << 24) | ((uint)count & 0xFFFFFF);
            //var packedSize = ((uint)maxSize << 24) | ((uint)count & 0xFFFFFF);
            writer.Write(packedSize);
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
