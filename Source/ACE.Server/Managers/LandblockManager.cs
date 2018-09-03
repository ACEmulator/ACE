using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public static class LandblockManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object landblockMutex = new object();
        private static readonly Landblock[,] landblocks = new Landblock[256, 256];

        private static readonly HashSet<Landblock> activeLandblocks = new HashSet<Landblock>();

        /// <summary>
        /// DestructionQueue is concurrent because it can be added to by multiple threads at once, publicly via AddToDestructionQueue()
        /// </summary>
        private static readonly ConcurrentBag<Landblock> destructionQueue = new ConcurrentBag<Landblock>();

        public static void AddObject(WorldObject worldObject, bool propegate = false)
        {
            var block = GetLandblock(worldObject.Location.LandblockId, propegate);
            block.AddWorldObject(worldObject);
        }

        public static void RemoveObject(WorldObject worldObject)
        {
            var block = GetLandblock(worldObject.Location.LandblockId, false);
            block.RemoveWorldObject(worldObject.Guid, false);
        }

        /// <summary>
        /// Relocates an object to the appropriate landblock -- Should only be called from physics/worldmanager -- not player!
        /// </summary>
        public static void RelocateObjectForPhysics(WorldObject worldObject, bool adjacencyMove)
        {
            var oldBlock = worldObject.CurrentLandblock;
            var newBlock = GetLandblock(worldObject.Location.LandblockId, true);
            // Remove from the old landblock -- force
            if (oldBlock != null)
                oldBlock.RemoveWorldObjectForPhysics(worldObject.Guid, adjacencyMove);
            // Add to the new landblock
            newBlock.AddWorldObjectForPhysics(worldObject);
        }

        public static List<Landblock> GetActiveLandblocks()
        {
            lock (landblockMutex)
                return activeLandblocks.ToList();
        }

        /// <summary>
        /// gets the landblock specified, creating it if it is not already loaded.  will create all
        /// adjacent landblocks if propagate is true (outdoor world roaming).
        /// </summary>
        private static Landblock GetLandblock(LandblockId landblockId, bool propagate)
        {
            lock (landblockMutex)
            {
                var landblock = landblocks[landblockId.LandblockX, landblockId.LandblockY];
                var autoLoad = propagate && landblockId.MapScope == MapScope.Outdoors;

                // standard check/lock/recheck pattern
                if (landblock == null || autoLoad && !landblock.AdjacenciesLoaded)
                {
                    landblock = landblocks[landblockId.LandblockX, landblockId.LandblockY];
                    if (landblock == null || autoLoad && !landblock.AdjacenciesLoaded)
                    {
                        if (landblock == null)
                        {
                            // load up this landblock
                            landblock = landblocks[landblockId.LandblockX, landblockId.LandblockY] = new Landblock(landblockId);

                            if (!activeLandblocks.Add(landblock))
                            {
                                log.Error("LandblockManager: failed to add " + (landblock.Id.Raw | 0xFFFF).ToString("X8") + " to active landblocks!");
                                return landblock;
                            }
                        }
                        SetAdjacencies(landblockId, autoLoad);
                        if (autoLoad)
                            landblock.AdjacenciesLoaded = true;
                    }
                }

                return landblock;
            }
        }

        /// <summary>
        /// Sets the adjacencies for a landblock
        /// </summary>
        /// <param name="landblockId">The landblock to set the adjacencies for</param>
        /// <param name="autoLoad">Flag indicates if unloaded adjacencies should be instantiated</param>
        private static void SetAdjacencies(LandblockId landblockId, bool autoLoad)
        {
            int x = landblockId.LandblockX;
            int y = landblockId.LandblockY;

            if (x > 0)
            {
                SetAdjacency(landblockId, landblockId.West, Adjacency.West, autoLoad);

                if (y > 0)
                    SetAdjacency(landblockId, landblockId.SouthWest, Adjacency.SouthWest, autoLoad);

                if (y < 254)
                    SetAdjacency(landblockId, landblockId.NorthWest, Adjacency.NorthWest, autoLoad);
            }

            if (x < 254)
            {
                SetAdjacency(landblockId, landblockId.East, Adjacency.East, autoLoad);

                if (y > 0)
                    SetAdjacency(landblockId, landblockId.SouthEast, Adjacency.SouthEast, autoLoad);

                if (y < 254)
                    SetAdjacency(landblockId, landblockId.NorthEast, Adjacency.NorthEast, autoLoad);
            }

            if (y > 0)
                SetAdjacency(landblockId, landblockId.South, Adjacency.South, autoLoad);

            if (y < 254)
                SetAdjacency(landblockId, landblockId.North, Adjacency.North, autoLoad);
        }

        /// <summary>
        /// sets the adjacencies of the specified landblocks.  nulls are allowed in the use case of deleting
        /// or unloading a landblock.  Landblock2 is {adjacency} of Landblock1.  if autoLoad is true, and
        /// landblock2 is null, it will be auto loaded.
        ///
        /// NOTE: ASSUMES A LOCK ON landblockMutex
        /// </summary>
        /// <param name="landblock1">a landblock</param>
        /// <param name="landblock2">a landblock</param>
        /// <param name="adjacency">the adjacency of landblock2 relative to landblock1</param>
        /// <param name="autoLoad">Will load landBlock2 if it's not loaded already</param>
        private static void SetAdjacency(LandblockId landblock1, LandblockId landblock2, Adjacency adjacency, bool autoLoad = false)
        {
            // suppress adjacency logic for indoor areas
            if (landblock1.MapScope != MapScope.Outdoors || landblock2.MapScope != MapScope.Outdoors)
                return;

            var lb1 = landblocks[landblock1.LandblockX, landblock1.LandblockY];
            var lb2 = landblocks[landblock2.LandblockX, landblock2.LandblockY];

            if (autoLoad && lb2 == null)
                lb2 = GetLandblock(landblock2, false);

            lb1.SetAdjacency(adjacency, lb2);

            if (lb2 != null)
            {
                var inverse = (((int)adjacency) + 4) % 8; // go halfway around the horn (+4) and mod 8 to wrap around
                var inverseAdjacency = (Adjacency)Enum.ToObject(typeof(Adjacency), inverse);
                lb2.SetAdjacency(inverseAdjacency, lb1);
            }
        }

        /// <summary>
        /// This function is NOT thread safe. Using it will likely result in concurrency issues with WorldManager.UpdateWorld.
        /// You should only use this for debugging/development purposes.
        /// </summary>
        public static void ForceLoadLandBlock(LandblockId blockid)
        {
            GetLandblock(blockid, false);
        }

        /// <summary>
        /// Queues a landblock for thread-safe unloading
        /// </summary>
        public static void AddToDestructionQueue(Landblock landblock)
        {
            destructionQueue.Add(landblock);
        }

        /// <summary>
        /// Processes the destruction queue in a thread-safe manner
        /// </summary>
        public static void UnloadLandblocks()
        {
            while (!destructionQueue.IsEmpty)
            {
                if (destructionQueue.TryTake(out Landblock landblock))
                {
                    landblock.Unload();

                    bool unloadFailed = false;

                    lock (landblockMutex)
                    {
                        // remove from list of managed landblocks
                        if (activeLandblocks.Remove(landblock))
                            landblocks[landblock.Id.LandblockX, landblock.Id.LandblockY] = null;
                        else
                            unloadFailed = true;
                    }

                    if (unloadFailed)
                        log.Error("LandblockManager: failed to unload " + (landblock.Id.Raw | 0xFFFF).ToString("X8"));
                }
            }
        }
    }
}
