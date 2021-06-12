using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;
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
        public ushort NumBuckets;

        public PropertyIntComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(PropertyInt a, PropertyInt b)
        {
            var keyA = (int)a % NumBuckets;
            var keyB = (int)b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyInt64Comparer : IComparer<PropertyInt64>
    {
        public ushort NumBuckets;

        public PropertyInt64Comparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(PropertyInt64 a, PropertyInt64 b)
        {
            var keyA = (int)a % NumBuckets;
            var keyB = (int)b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyBoolComparer : IComparer<PropertyBool>
    {
        public ushort NumBuckets;

        public PropertyBoolComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(PropertyBool a, PropertyBool b)
        {
            var keyA = (int)a % NumBuckets;
            var keyB = (int)b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyFloatComparer : IComparer<PropertyFloat>
    {
        public ushort NumBuckets;

        public PropertyFloatComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(PropertyFloat a, PropertyFloat b)
        {
            var keyA = (int)a % NumBuckets;
            var keyB = (int)b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyStringComparer : IComparer<PropertyString>
    {
        public ushort NumBuckets;

        public PropertyStringComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(PropertyString a, PropertyString b)
        {
            var keyA = (int)a % NumBuckets;
            var keyB = (int)b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyDataIdComparer : IComparer<PropertyDataId>
    {
        public ushort NumBuckets;

        public PropertyDataIdComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(PropertyDataId a, PropertyDataId b)
        {
            var keyA = (int)a % NumBuckets;
            var keyB = (int)b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class PropertyInstanceIdComparer : IComparer<PropertyInstanceId>
    {
        public ushort NumBuckets;

        public PropertyInstanceIdComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(PropertyInstanceId a, PropertyInstanceId b)
        {
            var keyA = (int)a % NumBuckets;
            var keyB = (int)b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class SkillComparer : IComparer<Skill>
    {
        public ushort NumBuckets;

        public SkillComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(Skill a, Skill b)
        {
            var keyA = (int)a % NumBuckets;
            var keyB = (int)b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class SpellComparer : IComparer<int>
    {
        public ushort NumBuckets;

        public SpellComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(int a, int b)
        {
            var keyA = a % NumBuckets;
            var keyB = b % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.CompareTo(b);

            return result;
        }
    }

    public class GuidComparer : IComparer<ObjectGuid>
    {
        public ushort NumBuckets;

        public GuidComparer(ushort numBuckets)
        {
            NumBuckets = numBuckets;
        }

        public int Compare(ObjectGuid a, ObjectGuid b)
        {
            var keyA = a.Full % NumBuckets;
            var keyB = b.Full % NumBuckets;

            var result = keyA.CompareTo(keyB);

            if (result == 0)
                result = a.Full.CompareTo(b.Full);

            return result;
        }
    }
}
