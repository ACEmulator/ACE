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

        private const uint InvalidGuid = uint.MaxValue;
        private const uint LowIdLimit = 0x1000;

        private const uint weenieMin = 0x00000001;

        private const uint weenieMax = 0x0001FFFF;

        // Npc / Doors / Portals / world items that get loaded from DB Etc max 268,369,919
        // took Turbine 17 years to get to ‭177,447‬
        // these will be added by developers and not in game
        // Nothing in this range is persisted by the game. Read in only   Only developers or content creators can create them to be persisted.
        // Fragmentation: None
        // TODO Fix this range once we defragment the world this is a bandaid for now it takes almost all of our range for nothing. Og II
        // Proposed real range for static would be
        // private const uint staticObjectMin = 0x00020000;
        // private const uint staticObjectMax = 0x0FFFFFFF;

        private const uint staticObjectMin = 0x70000000;
        private const uint staticObjectMax = 0xDFFFFFFF;

        // At Server startup read current max from the DB
        private static uint staticObject = 0x0002B527;

        // Monsters / Summoned portals - any non-static item max  ‭1,073,741,823‬
        // If the server ran for 30 days without a restart, we would need to be creating over 24,854 spawns per minute or 414 per second to exhaust
        // Wow does a server restart once per week.   I can't imagine this would be any type of real limitation even on a heavily populated server with
        // a lot of macro activity.    We could easily build a warning when a server was down to 50k free for a restart.
        // Fragmentation: None - N/A
        private const uint nonStaticMin = 0x00020000;

        private const uint nonStaticMax = 0x4FFFFFFF;

        // Never read from the database - resets at server restart each time
        // Nothing in this range is ever persisted.
        private static uint nonStaticObject = 0x10000001;

        // players max 268,345,454
        // Fragmentation: None - to very light
        // We should get these on player creation.   It would be a very edge case that we threw one away.
        // Again, given probable server populations and available accounts and players - this can pose no real limitation.
        private static uint PlayerMin { get; } = 0x50000001;

        private static uint PlayerMax { get; } = 0x5FFFFFFF;

        private static object playerLock = new object();

        private static uint player = 0x50000001;

        // Items max ‭2,684,354,559‬
        // Many items created - relatively few persisted.   Anything that could end up in player inventory ie persisted comes from this range.
        // Fragmentation: Heavy
        // Number Recovery Strategy - to be implemented later - but we can make is one function pull from a recycle pool that we create via
        // a server operator process.   As far as the server is concerned - it just calls GetNewItemGuid.
        // TODO Fix this once we defrag.
        // Proposed real range.
        // private const uint itemMin = 0x6000000;
        // private const uint itemMax = 0xFFFFFFF;
        private const uint itemMin = 0xE0000000;

        private const uint itemMax = 0xFFFFFFF;

        // At Server startup read current max from the DB
        private static uint item = 0xDD3B018C;

        private static readonly object ShardLock = new object();

        public static uint GetCurrentDbValueAsync(Action<Action<uint>> dbCall)
        {
            uint ret = InvalidGuid;
            dbCall.Invoke((dbVal) =>
            {
                lock (ShardLock)
                {
                    ret = dbVal;
                    Monitor.Pulse(ShardLock);
                }
            });

            lock (ShardLock)
            {
                while (ret == InvalidGuid)
                {
                    Monitor.Wait(ShardLock);
                }
            }

            // NOTE: + 1 -- we don't want to reassign our current guid
            return ret + 1;
        }

        // FIXME(ddevec): @Og -- hopefully you can use this
        public static void Initialize()
        {
            lock (playerLock)
            {
                player = GetCurrentDbValueAsync(Database.DatabaseManager.Shard.GetMaxPlayerId);
            }
        }

        /// <summary>
        /// Returns New Player Guid
        /// </summary>
        /// <returns></returns>
        public static uint NewPlayerGuid()
        {
            lock (playerLock)
            {
                if (player == PlayerMax)
                {
                    log.Fatal("Out of player Guids!");
                }

                if (player == PlayerMax - LowIdLimit)
                {
                    log.Warn("Running dangerously low on player Ids, need to defrag");
                }

                return player;
            }
        }

        /// <summary>
        /// Returns New Guid for NPCs, Doors, World Portals, etc
        /// </summary>
        /// <returns></returns>
        public static uint NewStaticObjectGuid()
        {
            staticObject++;
            return staticObject;
        }

        /// <summary>
        /// Returns New Guid for Monsters
        /// </summary>
        /// <returns></returns>
        public static uint NewNonStaticGuid()
        {
            nonStaticObject++;
            return nonStaticObject;
        }

        /// <summary>
        /// Returns New Guid for Items / Player Items
        /// </summary>
        /// <returns></returns>
        public static uint NewItemGuid()
        {
            item++;
            return item;
        }
    }
}
