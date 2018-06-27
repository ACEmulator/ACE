using System;
using System.IO;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// is this really different from PackableHashTable?
    /// </summary>
    public static class PHashTable
    {
        // seems to be a different header format -
        // the main difference seems to be the maxSize sent as # of bits to shift?
        public static void WriteHeader(BinaryWriter writer, int count)
        {
            // uint uint uint - packedSize - write: (buckets) | (count & 0xFFFFFF)
            // uint - buckets - read: 1 << (packedSize >> 24)
            // uint - count - read: packedSize & 0xFFFFFF
            var bucketShift = GetNumBits((uint)count)/* - 1*/;
            //var maxSize = 1 << ((int)bucketShift - 1);
            //writer.Write((ushort)bucketShift);
            //writer.Write((ushort)count);
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
