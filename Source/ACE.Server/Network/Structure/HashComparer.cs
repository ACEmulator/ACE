using System.Collections.Generic;

using ACE.Entity.Enum.Properties;

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

    // can't implement with generics, and the alternative is to key all of the originals by ints,
    // instead of the more semantic enums

    public class PropertyIntComparer : IComparer<PropertyInt>
    {
        public int Compare(PropertyInt a, PropertyInt b)
        {
            var keyA = (int)a % 16;
            var keyB = (int)b % 16;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyInt64Comparer : IComparer<PropertyInt64>
    {
        public int Compare(PropertyInt64 a, PropertyInt64 b)
        {
            var keyA = (int)a % 8;
            var keyB = (int)b % 8;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyBoolComparer : IComparer<PropertyBool>
    {
        public int Compare(PropertyBool a, PropertyBool b)
        {
            var keyA = (int)a % 8;
            var keyB = (int)b % 8;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyFloatComparer : IComparer<PropertyFloat>
    {
        public int Compare(PropertyFloat a, PropertyFloat b)
        {
            var keyA = (int)a % 8;
            var keyB = (int)b % 8;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyStringComparer : IComparer<PropertyString>
    {
        public int Compare(PropertyString a, PropertyString b)
        {
            var keyA = (int)a % 8;
            var keyB = (int)b % 8;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyDataIdComparer : IComparer<PropertyDataId>
    {
        public int Compare(PropertyDataId a, PropertyDataId b)
        {
            var keyA = (int)a % 8;
            var keyB = (int)b % 8;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }
}
