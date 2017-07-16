using System.Diagnostics;

namespace ACE.Entity
{
    public enum GuidType
    {
        Undef,
        Player,
        Creature
    }

    public struct ObjectGuid
    {
        public static readonly ObjectGuid Invalid = new ObjectGuid(0);

        public uint Full { get; }
        public uint Low => Full & 0xFFFFFF;
        public uint High => (Full >> 24);
        public GuidType Type { get; private set; }

        public ObjectGuid(uint full)
        {
            Full = full;
            Type = GuidType.Undef;
        }

        public ObjectGuid(uint full, GuidType type)
        {
            Full = full;
            Type = type;
        }
        
        public bool IsPlayer()
        {
            if (Type == GuidType.Player)
                return true;
            else
                return false;
        }

        public bool IsCreature()
        {
            if (Type == GuidType.Creature)
                return true;
            else
                return false;
        }

        public void ChangeGuidType(GuidType type)
        {
            Type = type;
        }

        public static bool operator ==(ObjectGuid g1, ObjectGuid g2)
        {
            return g1.Full == g2.Full;
        }

        public static bool operator !=(ObjectGuid g1, ObjectGuid g2)
        {
            return g1.Full != g2.Full;
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjectGuid)
                return ((ObjectGuid)obj) == this;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
