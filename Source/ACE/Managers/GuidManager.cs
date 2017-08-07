using System;
using System.Threading;
using log4net;
using ACE.Entity;

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

            public uint Current()
            {
                return current;
            }
        }

        public static uint InvalidGuid { get; } = uint.MaxValue;
        private const uint LowIdLimit = 0x1000;

        // private static GuidAllocator weenieAlloc;
        private static GuidAllocator staticObjectAlloc;
        private static GuidAllocator generatorAlloc;
        private static GuidAllocator nonStaticAlloc;
        private static GuidAllocator playerAlloc;
        private static GuidAllocator itemAlloc;

        // TODO: Finish out this work tieing this into the database
        public static void Initialize()
        {
            playerAlloc = new GuidAllocator(ObjectGuid.PlayerMin, ObjectGuid.PlayerMax, "player");
            itemAlloc = new GuidAllocator(ObjectGuid.ItemMin, ObjectGuid.ItemMax, "item");
            nonStaticAlloc = new GuidAllocator(ObjectGuid.NonStaticMin, ObjectGuid.NonStaticMax, "non-static");
            staticObjectAlloc = new GuidAllocator(ObjectGuid.StaticObjectMin, ObjectGuid.StaticObjectMax, "static");
            generatorAlloc = new GuidAllocator(ObjectGuid.GeneratorMin, ObjectGuid.GeneratorMax, "generator");
            // weenieAlloc = new GuidAllocator(ObjectGuid.WeenieMin, ObjectGuid.WeenieMax, "weenie");
        }

        /// <summary>
        /// Returns New Player Guid
        /// </summary>
        /// <returns></returns>
        public static ObjectGuid NewPlayerGuid()
        {
            return new ObjectGuid(playerAlloc.Alloc());
        }

        /// <summary>
        /// Returns New Guid for NPCs, Doors, World Portals, etc
        /// </summary>
        /// <returns></returns>
        public static ObjectGuid NewStaticObjectGuid()
        {
            return new ObjectGuid(staticObjectAlloc.Alloc());
        }

        /// <summary>
        /// Returns New Guid for Monsters
        /// </summary>
        /// <returns></returns>
        public static ObjectGuid NewGeneratorGuid()
        {
            return new ObjectGuid(generatorAlloc.Alloc());
        }

        /// <summary>
        /// Returns New Guid for Monsters
        /// </summary>
        /// <returns></returns>
        public static ObjectGuid NewNonStaticGuid()
        {
            return new ObjectGuid(nonStaticAlloc.Alloc());
        }

        /// <summary>
        /// Returns New Guid for Items / Player Items
        /// </summary>
        /// <returns></returns>
        public static ObjectGuid NewItemGuid()
        {
            return new ObjectGuid(itemAlloc.Alloc());
        }

        /// <summary>
        /// Returns Most Recently assigned Guid for Items / Player Items, to be used only for informational purposes.
        /// </summary>
        /// <returns></returns>
        public static ObjectGuid CurrentItemGuid()
        {
            return new ObjectGuid(itemAlloc.Current());
        }

        /// <summary>
        /// Returns Most Recently assigned Guid for Players, to be used only for informational purposes.
        /// </summary>
        /// <returns></returns>
        public static ObjectGuid CurrentPlayerGuid()
        {
            return new ObjectGuid(playerAlloc.Current());
        }
    }
}
