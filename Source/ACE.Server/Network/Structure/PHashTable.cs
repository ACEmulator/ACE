using System;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public static class PHashTable
    {
        /// <summary>
        /// Deprecated
        /// </summary>
        public static void WriteHeader(BinaryWriter writer, int count)
        {
            var numBits = GetNumBits(count);
            var numBuckets = 1 << (numBits - 1);

            writer.Write((ushort)count);
            writer.Write((ushort)numBuckets);
        }

        /// <summary>
        /// Returns the number of bits required to store the input number
        /// </summary>
        private static int GetNumBits(int num)
        {
            return (int)Math.Log(num, 2) + 1;
        }
    }
}
