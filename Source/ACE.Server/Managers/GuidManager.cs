using System;
using System.Collections.Generic;
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

        private class PlayerGuidAllocator
        {
            private readonly uint max;
            private uint current;
            private readonly string name;

            public PlayerGuidAllocator(uint min, uint max, string name)
            {
                this.max = max;

                // Read current value out of ShardDatabase
                lock (this)
                {
                    bool done = false;
                    Database.DatabaseManager.Shard.GetMaxGuidFoundInRange(min, max, dbVal =>
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
                        log.Warn($"Dangerously low on {name} GUIDs : {current:X} of {max:X}");
                }

                this.name = name;
            }

            public uint Alloc()
            {
                lock (this)
                {
                    if (current == max)
                    {
                        log.Fatal($"Out of {name} GUIDs!");
                        return InvalidGuid;
                    }

                    if (current == max - LowIdLimit)
                        log.Warn($"Running dangerously low on {name} GUIDs, need to defrag");

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

        private class DynamnicGuidAllocator
        {
            private readonly uint max;
            private uint current;
            private readonly string name;

            private static readonly TimeSpan recycleTime = TimeSpan.FromMinutes(30);

            private readonly Queue<Tuple<DateTime, uint>> recycledGuids = new Queue<Tuple<DateTime, uint>>();

            private LinkedList<(uint start, uint end)> availableIDs = new LinkedList<(uint start, uint end)>();

            public DynamnicGuidAllocator(uint min, uint max, string name)
            {
                this.max = max;

                // Read current value out of ShardDatabase
                lock (this)
                {
                    bool done = false;
                    Database.DatabaseManager.Shard.GetMaxGuidFoundInRange(min, max, dbVal =>
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
                        log.Warn($"Dangerously low on {name} GUIDs : {current:X} of {max:X}");
                }

                // Get available ids in the form of sequence gaps
                lock (this)
                {
                    // todo: Long term, if this query is taking too long, this magic number of 10000000 might want to come from a config file.
                    // todo: The idea behind this number is to pull enough free id's from the database so that the server runs (under typical load) for at least the duration of a typical restart period, before new (higher) id's start being generated
                    // todo: The objective is to use available id's which helps prevent incrementing the current max.
                    bool done = false;
                    Database.DatabaseManager.Shard.GetSequenceGaps(ObjectGuid.DynamicMin, 10000000, gaps =>
                    {
                        lock (this)
                        {
                            availableIDs = new LinkedList<(uint start, uint end)>(gaps);
                            done = true;
                            Monitor.Pulse(this);
                        }
                    });

                    while (!done)
                        Monitor.Wait(this);
                }

                this.name = name;
            }

            public uint Alloc()
            {
                lock (this)
                {
                    // First, try to use a recycled Guid
                    if (recycledGuids.TryPeek(out var result) && DateTime.UtcNow - result.Item1 > recycleTime)
                    {
                        recycledGuids.Dequeue();
                        return result.Item2;
                    }

                    // Second, try to use a known available Guid
                    if (availableIDs.First != null)
                    {
                        var id = availableIDs.First.Value.start;

                        if (availableIDs.First.Value.start == availableIDs.First.Value.end)
                        {
                            availableIDs.RemoveFirst();

                            //if (availableIDs.First == null)
                            //    log.Warn($"Sequence gap GUIDs depleted on {name}");
                        }
                        else
                            availableIDs.First.Value = (availableIDs.First.Value.start + 1, availableIDs.First.Value.end);

                        return id;
                    }

                    // Lastly, use an id that increments our max
                    if (current == max)
                    {
                        log.Fatal($"Out of {name} GUIDs!");
                        return InvalidGuid;
                    }

                    if (current == max - LowIdLimit)
                        log.Warn($"Running dangerously low on {name} GUIDs, need to defrag");

                    uint ret = current;
                    current += 1;

                    return ret;
                }
            }

            public uint Current()
            {
                return current;
            }

            public void Recycle(uint guid)
            {
                lock (this)
                    recycledGuids.Enqueue(new Tuple<DateTime, uint>(DateTime.UtcNow, guid));
            }
        }

        private static PlayerGuidAllocator playerAlloc;
        private static DynamnicGuidAllocator dynamicAlloc;

        public static void Initialize()
        {
            playerAlloc = new PlayerGuidAllocator(ObjectGuid.PlayerMin, ObjectGuid.PlayerMax, "player");
            dynamicAlloc = new DynamnicGuidAllocator(ObjectGuid.DynamicMin, ObjectGuid.DynamicMax, "dynamic");
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
            return new ObjectGuid(dynamicAlloc.Alloc());
        }

        /// <summary>
        /// Guid will be added to the recycle queue, and available for use in GuidAllocator.recycleTime
        /// </summary>
        /// <param name="guid"></param>
        public static void RecycleDynamicGuid(ObjectGuid guid)
        {
            dynamicAlloc.Recycle(guid.Full);
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
            return new ObjectGuid(dynamicAlloc.Current());
        }
    }
}
