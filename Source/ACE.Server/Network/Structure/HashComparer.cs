using System.Collections.Generic;

namespace ACE.Server.Network.Structure
{
    public class HashComparer : IComparer<uint>
    {
        public ushort NumBuckets;

        public HashComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(uint a, uint b)
        {
            var keyA = a % NumBuckets;
            var keyB = b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }
}
