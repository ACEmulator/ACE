using System.Threading;

using log4net;

using ACE.Entity;

namespace ACE.Server.Managers
{
    /// <summary>
    /// Used to assign global guids and ensure they are unique to server.
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

        /// <summary>
        /// Is equal to uint.MaxValue
        /// </summary>
        public static uint InvalidGuid { get; } = uint.MaxValue;

        private const uint LowIdLimit = 0x1000;

        private class GuidAllocator
        {
            private readonly uint max;
            private uint current;
            private readonly string name;

            public GuidAllocator(uint min, uint max, string name)
            {
                this.max = max;

                // Read current value out of ShardDatabase
                lock (this)
                {
                    bool done = false;
                    Database.DatabaseManager.Shard.GetMaxGuidFoundInRange(min, max, (dbVal) =>
                    {
                        lock (this)
                        {
                            current = dbVal;
                            done = true;
                            Monitor.Pulse(this);
                        }
                    });

                    while (!done)
                        Monitor.Wait(this);

                    if (current == InvalidGuid)
                        current = min;
                    else
                        // Need to start allocating at current value in db +1
                        current++;

                    if ((max - current) < LowIdLimit)
                        log.Warn($"Dangerously low on {name} guids : {current:X} of {max:X}");
                }

                // Now read current from WorldDatabase
                uint worldMax = Database.DatabaseManager.World.GetMaxGuidFoundInRange(min, max);

                if (worldMax != InvalidGuid && worldMax >= current)
                    current = worldMax + 1;

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
                        log.Warn($"Running dangerously low on {name} Ids, need to defrag");

                    uint ret = current;
                    current += 1;

                    return ret;
                }
            }

            public uint Current()
            {
                return current;
            }
        }

        private static GuidAllocator playerAlloc;
        private static GuidAllocator nonStaticAlloc;

        public static void Initialize()
        {
            playerAlloc = new GuidAllocator(ObjectGuid.PlayerMin, ObjectGuid.PlayerMax, "player");
            nonStaticAlloc = new GuidAllocator(ObjectGuid.DynamicMin, ObjectGuid.DynamicMax, "non-static");
        }

        /// <summary>
        /// Returns New Player Guid
        /// </summary>
        public static ObjectGuid NewPlayerGuid()
        {
            return new ObjectGuid(playerAlloc.Alloc());
        }

        /// <summary>
        /// These represent items are generated in the world.
        /// Some of them will be saved to the Shard db.
        /// They can be monsters, loot, etc..
        /// </summary>
        public static ObjectGuid NewDynamicGuid()
        {
            return new ObjectGuid(nonStaticAlloc.Alloc());
        }


        /// <summary>
        /// Returns GuidAllocator.Current which is the Next Guid to be Alloc'd for Players, to be used only for informational purposes.
        /// </summary>
        public static ObjectGuid NextPlayerGuid()
        {
            return new ObjectGuid(playerAlloc.Current());
        }

        /// <summary>
        /// Returns GuidAllocator.Current which is the Next Guid to be Alloc'd for world generated Items, to be used only for informational purposes.
        /// </summary>
        public static ObjectGuid NextDynamicGuid()
        {
            return new ObjectGuid(nonStaticAlloc.Current());
        }
    }
}
