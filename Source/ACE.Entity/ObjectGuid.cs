namespace ACE.Entity
{
    public enum GuidType
    {
        Undef,
        Weenie,
        Static,
        Generator,
        Player,
        Creature,
        Item
    }

    public struct ObjectGuid
    {
        public static readonly ObjectGuid Invalid = new ObjectGuid(0);

        public static uint WeenieMin { get; } = 0x00000001;
        public static uint WeenieMax { get; } = 0x000F423F; // 999,999

        public static uint StaticObjectMin { get; } = 0x70000000;
        public static uint StaticObjectMax { get; } = 0xDFFFFFFF;

        /* NOTE(ddevec): Taking out static object allocation -- we never allocate "static" objects, right?
        // We should never allocate from our static object pool?
        private static uint staticObject = staticObjectMax;
        */

        // Removed old comments -- looks like we are going in a different direction here and they were just confusing.   Og II

        public static uint GeneratorMin { get; } = 0x000F4240; // 1,000,000
        public static uint GeneratorMax { get; } = 0x001E847F; // 1,999,999

        // FIXME(ddevec): Currently
        public static uint NonStaticMin { get; } = 0x001E8480; // 2,000,000
        public static uint NonStaticMax { get; } = 0x4FFFFFFF;

        public static uint PlayerMin { get; } = 0x50000001;
        public static uint PlayerMax { get; } = 0x5FFFFFFF;

        public static uint ItemMin { get; } = 0xE0000000;
        // Ends at E because uint.Max is reserved for "invalid"
        public static uint ItemMax { get; } = 0xFFFFFFFE;

        public uint Full { get; }
        public uint Low => Full & 0xFFFFFF;
        public uint High => (Full >> 24);
        public GuidType Type { get; private set; }

        public ObjectGuid(uint full)
        {
            Full = full;

            if (Full >= WeenieMin && Full <= WeenieMax)
                Type = GuidType.Weenie;
            else if (Full >= StaticObjectMin && Full <= StaticObjectMax)
                Type = GuidType.Static;
            else if (Full >= GeneratorMin && Full <= GeneratorMax)
                Type = GuidType.Generator;
            else if (Full >= NonStaticMin && Full <= NonStaticMax)
                Type = GuidType.Creature;
            else if (Full >= PlayerMin && Full <= PlayerMax)
                Type = GuidType.Player;
            else if (Full >= ItemMin && Full <= ItemMax)
                Type = GuidType.Item;
            else
                Type = GuidType.Undef;
        }

        public bool IsPlayer()
        {
            if (Type == GuidType.Player)
                return true;
            return false;
        }

        public bool IsCreature()
        {
            if (Type == GuidType.Creature)
                return true;
            return false;
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
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
