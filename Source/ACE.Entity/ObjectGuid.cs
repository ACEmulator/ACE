using System.Diagnostics;

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
        public static uint WeenieMax { get; } = 0x000F423F;

        // Npc / Doors / Portals / world items that get loaded from DB Etc max 268,369,919
        // took Turbine 17 years to get to ‭177,447‬
        // these will be added by developers and not in game
        // Nothing in this range is persisted by the game. Read in only   Only developers or content creators can create them to be persisted.
        // Fragmentation: None
        // TODO Fix this range once we defragment the world this is a bandaid for now it takes almost all of our range for nothing. Og II
        // Proposed real range for static would be
        // private const uint staticObjectMin = 0x00020000;
        // private const uint staticObjectMax = 0x0FFFFFFF;

        public static uint StaticObjectMin { get; } = 0x70000000;
        public static uint StaticObjectMax { get; } = 0xDFFFFFFF;

        /* NOTE(ddevec): Taking out static object allocation -- we never allocate "static" objects, right?
        // We should never allocate from our static object pool?
        private static uint staticObject = staticObjectMax;
        */

        public static uint GeneratorMin { get; } = 0x000F4240;
        public static uint GeneratorMax { get; } = 0x001E847F;

        // Monsters / Summoned portals - any non-static item max  ‭1,073,741,823‬
        // If the server ran for 30 days without a restart, we would need to be creating over 24,854 spawns per minute or 414 per second to exhaust
        // Wow does a server restart once per week.   I can't imagine this would be any type of real limitation even on a heavily populated server with
        // a lot of macro activity.    We could easily build a warning when a server was down to 50k free for a restart.
        // Fragmentation: None - N/A
        // FIXME(ddevec): Currently 
        public static uint NonStaticMin { get; } = 0x001E8480;
        public static uint NonStaticMax { get; } = 0x4FFFFFFF;

        // players max 268,345,454
        // Fragmentation: None - to very light
        // We should get these on player creation.   It would be a very edge case that we threw one away.
        // Again, given probable server populations and available accounts and players - this can pose no real limitation.
        public static uint PlayerMin { get; } = 0x50000001;
        public static uint PlayerMax { get; } = 0x5FFFFFFF;

        // Items max ‭2,684,354,559‬
        // Many items created - relatively few persisted.   Anything that could end up in player inventory ie persisted comes from this range.
        // Fragmentation: Heavy
        // Number Recovery Strategy - to be implemented later - but we can make is one function pull from a recycle pool that we create via
        // a server operator process.   As far as the server is concerned - it just calls GetNewItemGuid.
        // TODO Fix this once we defrag.
        // Proposed real range.
        // private const uint itemMin = 0x6000000;
        // private const uint itemMax = 0xFFFFFFE;
        public static uint ItemMin { get; } = 0xE0000000;
        // Ends at E because uint.Max is reserved for "invalid"
        public static uint ItemMax { get; } = 0xFFFFFFFE;

        public uint Full { get; }
        public uint Low => Full & 0xFFFFFF;
        public uint High => (Full >> 24);
        public GuidType Type { get; private set; }
        ////public GuidType Type
        ////{
        ////    get;
        ////    private set;
        ////}

        public ObjectGuid(uint full)
        {
            Full = full;
            ////Type = GuidType.Undef;

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

        ////public ObjectGuid(uint full, GuidType type)
        ////{
        ////    Full = full;
        ////    Type = type;
        ////}
        
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

        ////public void ChangeGuidType(GuidType type)
        ////{
        ////    Type = type;
        ////}

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
