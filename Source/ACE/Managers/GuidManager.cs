using System;
using System.Threading;
using log4net;

namespace ACE.Managers
{
    /// <summary>
    /// Used to assign global guids and ensure they are unique to server.
    /// todo:// use and reuse .. keep track of who using what and release ids..
    /// </summary>
    public static class GuidManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Running server is guid master - database only read as startup to get current max per range.
        // weenie class templates Max 65,535 - took Turbine 17 years to get to 10K
        // these will be added by developers and not in game.
        // Nothing in this range is persisted by the game.   Only developers or content creators can create them to be persisted.
        // this is only here for documentation purposes.
        // Fragmentation: None

        private class GuidAllocator
        {
            private uint min;
            private uint max;
            private uint current;
            private string name;

            public GuidAllocator(uint min, uint max, string name)
            {
                this.min = min;
                this.max = max;

                // Read current value out of ShardDatabase
                lock (this)
                {
                    bool done = false;
                    Database.DatabaseManager.Shard.GetCurrentId(min, max, (dbVal) =>
                    {
                        lock (this)
                        {
                            current = dbVal;
                            done = true;
                            Monitor.Pulse(this);
                        }
                    });

                    while (!done)
                    {
                        Monitor.Wait(this);
                    }

                    if (current == InvalidGuid)
                    {
                        current = min;
                    }
                    else
                    {
                        // Need to start allocating at current value in db +1
                        current++;
                    }

                    if ((max - current) < LowIdLimit)
                    {
                        log.Warn($"Dangerously low on {name} guids : {current:X} of {max:X}");
                    }
                }

                // Now read current from WorldDatabase
                uint worldMax = Database.DatabaseManager.World.GetCurrentId(min, max);
                if (worldMax != InvalidGuid && worldMax >= current)
                {
                    current = worldMax + 1;
                }

                this.name = name;
            }

            public uint Alloc()
            {
                lock (this)
                {
                    if (current == max)
                    {
                        log.Fatal($"Out of {name} Guids!");
                        return InvalidGuid;
                    }

                    if (current == max - LowIdLimit)
                    {
                        log.Warn($"Running dangerously low on {name} Ids, need to defrag");
                    }

                    uint ret = current;
                    current += 1;

                    return ret;
                }
            }
        }

        public static uint InvalidGuid { get; } = uint.MaxValue;
        private const uint LowIdLimit = 0x1000;

        public static uint WeenieMin { get; } = 0x00000001;
        public static uint WeenieMax { get; } = 0x0001FFFF;

        // private static GuidAllocator weenieAlloc;

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

        private static GuidAllocator staticObjectAlloc;

        /* NOTE(ddevec): Taking out static object allocation -- we never allocate "static" objects, right?
        // We should never allocate from our static object pool?
        private static uint staticObject = staticObjectMax;
        */

        // Monsters / Summoned portals - any non-static item max  ‭1,073,741,823‬
        // If the server ran for 30 days without a restart, we would need to be creating over 24,854 spawns per minute or 414 per second to exhaust
        // Wow does a server restart once per week.   I can't imagine this would be any type of real limitation even on a heavily populated server with
        // a lot of macro activity.    We could easily build a warning when a server was down to 50k free for a restart.
        // Fragmentation: None - N/A
        // FIXME(ddevec): Currently 
        public static uint NonStaticMin { get; } = 0x000F4240;
        public static uint NonStaticMax { get; } = 0x4FFFFFFF;
        private static GuidAllocator nonStaticAlloc;

        // players max 268,345,454
        // Fragmentation: None - to very light
        // We should get these on player creation.   It would be a very edge case that we threw one away.
        // Again, given probable server populations and available accounts and players - this can pose no real limitation.
        public static uint PlayerMin { get; } = 0x50000001;
        public static uint PlayerMax { get; } = 0x5FFFFFFF;

        private static GuidAllocator playerAlloc;

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

        // At Server startup read current max from the DB
        private static GuidAllocator itemAlloc;

        // TODO: Finish out this work tieing this into the database
        public static void Initialize()
        {
            playerAlloc = new GuidAllocator(PlayerMin, PlayerMax, "player");
            itemAlloc = new GuidAllocator(ItemMin, ItemMax, "item");
            nonStaticAlloc = new GuidAllocator(NonStaticMin, NonStaticMax, "non-static");
            staticObjectAlloc = new GuidAllocator(StaticObjectMin, StaticObjectMax, "static");
            // weenieAlloc = new GuidAllocator(WeenieMin, WeenieMax, "static");
        }

        /// <summary>
        /// Returns New Player Guid
        /// </summary>
        /// <returns></returns>
        public static uint NewPlayerGuid()
        {
            return playerAlloc.Alloc();
        }

        /// <summary>
        /// Returns New Guid for NPCs, Doors, World Portals, etc
        /// </summary>
        /// <returns></returns>
        public static uint NewStaticObjectGuid()
        {
            return staticObjectAlloc.Alloc();
        }

        /// <summary>
        /// Returns New Guid for Monsters
        /// </summary>
        /// <returns></returns>
        public static uint NewNonStaticGuid()
        {
            return nonStaticAlloc.Alloc();
        }

        /// <summary>
        /// Returns New Guid for Items / Player Items
        /// </summary>
        /// <returns></returns>
        public static uint NewItemGuid()
        {
            return itemAlloc.Alloc();
        }
    }
}