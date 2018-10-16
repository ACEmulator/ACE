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

        public static void PreloadCommonLandblocks()
        {
            foreach (var rawLandblockId in rawLandblockIds)
            {
                var landBlockId = new LandblockId(rawLandblockId);
                ForceLoadLandBlock(landBlockId, true, true);
                log.DebugFormat("Landblock {0:X4} preloaded", landBlockId.Landblock);
            }
        }

        // TODO: Change the RawLandblockId list used for preloading defined landblocks to some other, more easily-modified format, rather than a compiled uint array
        private static readonly uint[] rawLandblockIds = {   0x8603ffff, // Training Academy - Holtburg Starting Location
                                                            0x8c04ffff, // Training Academy - Yaraq Starting Location
                                                            0x7f03ffff, // Training Academy - Shoushi Starting Location
                                                            0x7203ffff, // Training Academy - Sanamar Starting Location
                                                            0x0007ffff,	// Town Network
                                                            0xce94ffff,	// Eastham
                                                            0xda55ffff,	// Shoushi
                                                            0xdb54ffff,	// Shoushi
                                                            0xa9b4ffff,	// Holtburg
                                                            0xabb2ffff,	// Holtburg
                                                            0xaab3ffff,	// Holtburg
                                                            0x7d64ffff,	// Yaraq
                                                            0x7e64ffff,	// Yaraq
                                                            0xe64effff,	// Hebian-to
                                                            0xe74effff,	// Hebian-to
                                                            0xbb9fffff,	// Cragstone
                                                            0xbc9fffff,	// Cragstone
                                                            0xc6a9ffff,	// Arwic
                                                            0xe63effff,	// Nanto
                                                            0xe632ffff,	// Mayoi
                                                            0xc341ffff,	// Baishi
                                                            0xc98cffff,	// Rithwic
                                                            0x977bffff,	// Samsur
                                                            0x8f58ffff,	// Al-Arqas
                                                            0x33d9ffff,	// Sanamar
                                                            0x17b2ffff	// Redspire
                                                            };

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
        /// Loads the specified list of landblocks and optionally their adjacents.
        /// </summary>
        public static void ForceLoadLandBlock(LandblockId blockid, bool propagate, bool permaload)
        {
            GetLandblock(blockid, propagate, permaload);
        }

        /// <summary>
        /// gets the landblock specified, creating it if it is not already loaded.  will create all
        /// adjacent landblocks if propagate is true (outdoor world roaming).
        /// </summary>
        public static Landblock GetLandblock(LandblockId landblockId, bool propagate, bool permaload = false)
        {
            lock (landblockMutex)
            {
                var landblock = landblocks[landblockId.LandblockX, landblockId.LandblockY];
                var autoLoad = propagate && landblockId.MapScope == MapScope.Outdoors;

                if (landblock == null || autoLoad && !landblock.AdjacenciesLoaded)
                {
                    if (landblock == null)
                    {
                        // load up this landblock
                        landblock = landblocks[landblockId.LandblockX, landblockId.LandblockY] = new Landblock(landblockId);

                        // Set Permaload flag, as required, for new landblock to be loaded
                        landblock.Permaload = permaload;

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

        /// <summary>
        /// Used on server shutdown
        /// </summary>
        public static void AddAllActiveLandblocksToDestructionQueue()
        {
            lock (landblockMutex)
            {
                foreach (var landblock in activeLandblocks)
                    AddToDestructionQueue(landblock);
            }
        }
    }
}
