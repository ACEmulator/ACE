using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    /// <summary>
    /// Handles loading/unloading landblocks, and their adjacencies
    /// </summary>
    public static class LandblockManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Locking mechanism provides concurrent access to collections
        /// </summary>
        private static readonly object landblockMutex = new object();

        /// <summary>
        /// A table of all the landblocks in the world map
        /// Landblocks which aren't currently loaded will be null here
        /// </summary>
        private static readonly Landblock[,] landblocks = new Landblock[255, 255];

        /// <summary>
        /// A lookup table of all the currently loaded landblocks
        /// </summary>
        private static readonly HashSet<Landblock> loadedLandblocks = new HashSet<Landblock>();

        private static readonly List<Landblock> landblockGroupPendingAdditions = new List<Landblock>();
        private static readonly List<LandblockGroup> landblockGroups = new List<LandblockGroup>();

        public static int LandblockGroupsCount
        {
            get
            {
                lock (landblockMutex)
                    return landblockGroups.Count;
            }
        }

        /// <summary>
        /// DestructionQueue is concurrent because it can be added to by multiple threads at once, publicly via AddToDestructionQueue()
        /// </summary>
        private static readonly ConcurrentBag<Landblock> destructionQueue = new ConcurrentBag<Landblock>();

        /// <summary>
        /// Permaloads a list of configurable landblocks if server option is set
        /// </summary>
        public static void PreloadConfigLandblocks()
        {
            if (!ConfigManager.Config.Server.LandblockPreloading)
            {
                log.Info("Preloading Landblocks Disabled...");
                log.Warn("Events may not function correctly as Preloading of Landblocks has disabled.");
                return;
            }

            log.Info("Preloading Landblocks...");

            if (ConfigManager.Config.Server.PreloadedLandblocks == null)
            {
                log.Info("No configuration found for PreloadedLandblocks, please refer to Config.js.example");
                log.Warn("Initializing PreloadedLandblocks with single default for Hebian-To (Global Events)");
                log.Warn("Add a PreloadedLandblocks section to your Config.js file and adjust to meet your needs");
                ConfigManager.Config.Server.PreloadedLandblocks = new List<PreloadedLandblocks> { new PreloadedLandblocks { Id = "E74EFFFF", Description = "Hebian-To (Global Events)", Permaload = true, IncludeAdjacents = true, Enabled = true } };
            }

            log.InfoFormat("Found {0} landblock entries in PreloadedLandblocks configuration, {1} are set to preload.", ConfigManager.Config.Server.PreloadedLandblocks.Count, ConfigManager.Config.Server.PreloadedLandblocks.Count(x => x.Enabled == true));

            foreach (var preloadLandblock in ConfigManager.Config.Server.PreloadedLandblocks)
            {
                if (!preloadLandblock.Enabled)
                {
                    log.DebugFormat("Landblock {0:X4} specified but not enabled in config, skipping", preloadLandblock.Id);
                    continue;
                }

                if (uint.TryParse(preloadLandblock.Id, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out uint landblock))
                {
                    if (landblock == 0)
                    {
                        switch (preloadLandblock.Description)
                        {
                            case "Apartment Landblocks":
                                log.InfoFormat("Preloading landblock group: {0}, IncludeAdjacents = {1}, Permaload = {2}", preloadLandblock.Description, preloadLandblock.IncludeAdjacents, preloadLandblock.Permaload);
                                foreach (var apt in apartmentLandblocks)
                                    PreloadLandblock(apt, preloadLandblock);
                                break;
                        }
                    }
                    else
                        PreloadLandblock(landblock, preloadLandblock);
                }
            }
        }

        private static void PreloadLandblock(uint landblock, PreloadedLandblocks preloadLandblock)
        {
            var landblockID = new LandblockId(landblock);
            GetLandblock(landblockID, preloadLandblock.IncludeAdjacents, preloadLandblock.Permaload);
            log.DebugFormat("Landblock {0:X4}, ({1}) preloaded. IncludeAdjacents = {2}, Permaload = {3}", landblockID.Landblock, preloadLandblock.Description, preloadLandblock.IncludeAdjacents, preloadLandblock.Permaload);
        }

        private static readonly uint[] apartmentLandblocks =
        {
            0x7200FFFF,
            0x7300FFFF,
            0x7400FFFF,
            0x7500FFFF,
            0x7600FFFF,
            0x7700FFFF,
            0x7800FFFF,
            0x7900FFFF,
            0x7A00FFFF,
            0x7B00FFFF,
            0x7C00FFFF,
            0x7D00FFFF,
            0x7E00FFFF,
            0x7F00FFFF,
            0x8000FFFF,
            0x8100FFFF,
            0x8200FFFF,
            0x8300FFFF,
            0x8400FFFF,
            0x8500FFFF,
            0x8600FFFF,
            0x8700FFFF,
            0x8800FFFF,
            0x8900FFFF,
            0x8A00FFFF,
            0x8B00FFFF,
            0x8C00FFFF,
            0x8D00FFFF,
            0x8E00FFFF,
            0x8F00FFFF,
            0x9000FFFF,
            0x9100FFFF,
            0x9200FFFF,
            0x9300FFFF,
            0x9400FFFF,
            0x9500FFFF,
            0x9600FFFF,
            0x9700FFFF,
            0x9800FFFF,
            0x9900FFFF,
            0x5360FFFF,
            0x5361FFFF,
            0x5362FFFF,
            0x5363FFFF,
            0x5364FFFF,
            0x5365FFFF,
            0x5366FFFF,
            0x5367FFFF,
            0x5368FFFF,
            0x5369FFFF
        };

        private static void ProcessPendingLandblockGroupAdditions()
        {
            if (landblockGroupPendingAdditions.Count == 0)
                return;

            lock (landblockMutex)
            {
                for (int i = landblockGroupPendingAdditions.Count - 1; i >= 0; i--)
                {
                    if (landblockGroupPendingAdditions[i].IsDungeon)
                    {
                        // Each dungeon exists in its own group
                        var landblockGroup = new LandblockGroup(landblockGroupPendingAdditions[i]);
                        landblockGroups.Add(landblockGroup);
                    }
                    else
                    {
                        // Find out how many groups this landblock is eligible for
                        var landblockGroupsIndexMatchesByDistance = new List<int>();

                        for (int j = 0; j < landblockGroups.Count; j++)
                        {
                            if (landblockGroups[j].IsDungeon)
                                continue;

                            var distance = landblockGroups[j].BoundaryDistance(landblockGroupPendingAdditions[i]);

                            if (distance < LandblockGroup.LandblockGroupMinSpacing)
                                landblockGroupsIndexMatchesByDistance.Add(j);
                        }

                        if (landblockGroupsIndexMatchesByDistance.Count > 0)
                        {
                            // Add the landblock to the first eligible group
                            landblockGroups[landblockGroupsIndexMatchesByDistance[0]].Add(landblockGroupPendingAdditions[i]);

                            if (landblockGroupsIndexMatchesByDistance.Count > 1)
                            {
                                // Merge the additional eligible groups into the first one
                                for (int j = landblockGroupsIndexMatchesByDistance.Count - 1; j > 0; j--)
                                {
                                    // Copy the j down into 0
                                    foreach (var landblock in landblockGroups[landblockGroupsIndexMatchesByDistance[j]])
                                        landblockGroups[landblockGroupsIndexMatchesByDistance[0]].Add(landblock);

                                    landblockGroups.RemoveAt(landblockGroupsIndexMatchesByDistance[j]);
                                }
                            }
                        }
                        else
                        {
                            // No close groups were found
                            var landblockGroup = new LandblockGroup(landblockGroupPendingAdditions[i]);
                            landblockGroups.Add(landblockGroup);
                        }
                    }

                    landblockGroupPendingAdditions.RemoveAt(i);
                }

                // Debugging todo: comment this out after enough testing
                var count = 0;
                foreach (var group in landblockGroups)
                    count += group.Count;
                if (count != loadedLandblocks.Count)
                    log.Error($"[LANDBLOCK GROUP] ProcessPendingAdditions count ({count}) != loadedLandblocks.Count ({loadedLandblocks.Count})");
            }
        }

        public static void Tick(double portalYearTicks)
        {
            // update positions through physics engine
            ServerPerformanceMonitor.RestartEvent(ServerPerformanceMonitor.MonitorType.LandblockManager_TickPhysics);
            TickPhysics(portalYearTicks);
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.LandblockManager_TickPhysics);

            // Tick all of our Landblocks and WorldObjects (Work that can be multi-threaded)
            ServerPerformanceMonitor.RestartEvent(ServerPerformanceMonitor.MonitorType.LandblockManager_TickMultiThreadedWork);
            TickMultiThreadedWork();
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.LandblockManager_TickMultiThreadedWork);

            // Tick all of our Landblocks and WorldObjects (Work that must be single threaded)
            ServerPerformanceMonitor.RestartEvent(ServerPerformanceMonitor.MonitorType.LandblockManager_TickSingleThreadedWork);
            TickSingleThreadedWork();
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.LandblockManager_TickSingleThreadedWork);

            // clean up inactive landblocks
            UnloadLandblocks();
        }

        /// <summary>
        /// Used to debug cross-landblock group (and potentially cross-thread) operations
        /// </summary>
        public static bool CurrentlyTickingLandblockGroupsMultiThreaded { get; private set; }

        /// <summary>
        /// Used to debug cross-landblock group (and potentially cross-thread) operations
        /// </summary>
        public static readonly ThreadLocal<LandblockGroup> CurrentMultiThreadedTickingLandblockGroup = new ThreadLocal<LandblockGroup>();

        /// <summary>
        /// Processes physics objects in all active landblocks for updating
        /// </summary>
        private static void TickPhysics(double portalYearTicks)
        {
            ProcessPendingLandblockGroupAdditions();

            var movedObjects = new ConcurrentBag<WorldObject>();

            if (ConfigManager.Config.Server.Threading.MultiThreadedLandblockGroupPhysicsTicking)
            {
                CurrentlyTickingLandblockGroupsMultiThreaded = true;

                Parallel.ForEach(landblockGroups, ConfigManager.Config.Server.Threading.LandblockManagerParallelOptions, landblockGroup =>
                {
                    CurrentMultiThreadedTickingLandblockGroup.Value = landblockGroup;

                    foreach (var landblock in landblockGroup)
                        landblock.TickPhysics(portalYearTicks, movedObjects);

                    CurrentMultiThreadedTickingLandblockGroup.Value = null;
                });

                CurrentlyTickingLandblockGroupsMultiThreaded = false;
            }
            else
            {
                foreach (var landblockGroup in landblockGroups)
                {
                    foreach (var landblock in landblockGroup)
                        landblock.TickPhysics(portalYearTicks, movedObjects);
                }
            }

            // iterate through objects that have changed landblocks
            foreach (var movedObject in movedObjects)
            {
                // NOTE: The object's Location can now be null, if a player logs out, or an item is picked up
                if (movedObject.Location == null)
                    continue;

                // assume adjacency move here?
                RelocateObjectForPhysics(movedObject, true);
            }
        }

        private static void TickMultiThreadedWork()
        {
            ProcessPendingLandblockGroupAdditions();

            if (ConfigManager.Config.Server.Threading.MultiThreadedLandblockGroupTicking)
            {
                CurrentlyTickingLandblockGroupsMultiThreaded = true;

                Parallel.ForEach(landblockGroups, ConfigManager.Config.Server.Threading.LandblockManagerParallelOptions, landblockGroup =>
                {
                    CurrentMultiThreadedTickingLandblockGroup.Value = landblockGroup;

                    foreach (var landblock in landblockGroup)
                        landblock.TickMultiThreadedWork(Time.GetUnixTime());

                    CurrentMultiThreadedTickingLandblockGroup.Value = null;
                });

                CurrentlyTickingLandblockGroupsMultiThreaded = false;
            }
            else
            {
                foreach (var landblockGroup in landblockGroups)
                {
                    foreach (var landblock in landblockGroup)
                        landblock.TickMultiThreadedWork(Time.GetUnixTime());
                }
            }
        }

        private static void TickSingleThreadedWork()
        {
            ProcessPendingLandblockGroupAdditions();

            foreach (var landblockGroup in landblockGroups)
            {
                foreach (var landblock in landblockGroup)
                    landblock.TickSingleThreadedWork(Time.GetUnixTime());
            }
        }

        /// <summary>
        /// Adds a WorldObject to the landblock defined by the object's location
        /// </summary>
        /// <param name="loadAdjacents">If TRUE, ensures all of the adjacent landblocks for this WorldObject are loaded</param>
        public static bool AddObject(WorldObject worldObject, bool loadAdjacents = false)
        {
            var block = GetLandblock(worldObject.Location.LandblockId, loadAdjacents);

            return block.AddWorldObject(worldObject);
        }

        /// <summary>
        /// Relocates an object to the appropriate landblock -- Should only be called from physics/worldmanager -- not player!
        /// </summary>
        public static void RelocateObjectForPhysics(WorldObject worldObject, bool adjacencyMove)
        {
            var oldBlock = worldObject.CurrentLandblock;
            var newBlock = GetLandblock(worldObject.Location.LandblockId, true);

            if (newBlock.IsDormant && worldObject is SpellProjectile)
            {
                worldObject.PhysicsObj.set_active(false);
                worldObject.Destroy();
                return;
            }

            // Remove from the old landblock -- force
            if (oldBlock != null)
                oldBlock.RemoveWorldObjectForPhysics(worldObject.Guid, adjacencyMove);
            // Add to the new landblock
            newBlock.AddWorldObjectForPhysics(worldObject);
        }

        public static bool IsLoaded(LandblockId landblockId)
        {
            lock (landblockMutex)
                return landblocks[landblockId.LandblockX, landblockId.LandblockY] != null;
        }

        /// <summary>
        /// Returns a reference to a landblock, loading the landblock if not already active
        /// </summary>
        public static Landblock GetLandblock(LandblockId landblockId, bool loadAdjacents, bool permaload = false)
        {
            Landblock landblock;

            lock (landblockMutex)
            {
                bool setAdjacents = false;

                landblock = landblocks[landblockId.LandblockX, landblockId.LandblockY];

                if (landblock == null)
                {
                    // load up this landblock
                    landblock = landblocks[landblockId.LandblockX, landblockId.LandblockY] = new Landblock(landblockId);

                    if (!loadedLandblocks.Add(landblock))
                    {
                        log.Error($"LandblockManager: failed to add {landblock.Id.Raw:X8} to active landblocks!");
                        return landblock;
                    }

                    landblockGroupPendingAdditions.Add(landblock);

                    landblock.Init();

                    setAdjacents = true;
                }

                if (permaload)
                    landblock.Permaload = true;

                // load adjacents, if applicable
                if (loadAdjacents)
                {
                    var adjacents = GetAdjacentIDs(landblock);
                    foreach (var adjacent in adjacents)
                        GetLandblock(adjacent, false, permaload);

                    setAdjacents = true;
                }

                // cache adjacencies
                if (setAdjacents)
                    SetAdjacents(landblock, true, true);
            }

            return landblock;
        }

        /// <summary>
        /// Returns the list of all loaded landblocks
        /// </summary>
        public static List<Landblock> GetLoadedLandblocks()
        {
            lock (landblockMutex)
                return loadedLandblocks.ToList();
        }

        /// <summary>
        /// Returns the active, non-null adjacents for a landblock
        /// </summary>
        private static List<Landblock> GetAdjacents(Landblock landblock)
        {
            var adjacentIDs = GetAdjacentIDs(landblock);

            var adjacents = new List<Landblock>();

            foreach (var adjacentID in adjacentIDs)
            {
                var adjacent = landblocks[adjacentID.LandblockX, adjacentID.LandblockY];
                if (adjacent != null)
                    adjacents.Add(adjacent);
            }

            return adjacents;
        }

        /// <summary>
        /// Returns the list of adjacent landblock IDs for a landblock
        /// </summary>
        private static List<LandblockId> GetAdjacentIDs(Landblock landblock)
        {
            var adjacents = new List<LandblockId>();

            if (landblock.IsDungeon)
                return adjacents;   // dungeons have no adjacents

            var north = GetAdjacentID(landblock.Id, Adjacency.North);
            var south = GetAdjacentID(landblock.Id, Adjacency.South);
            var west = GetAdjacentID(landblock.Id, Adjacency.West);
            var east = GetAdjacentID(landblock.Id, Adjacency.East);
            var northwest = GetAdjacentID(landblock.Id, Adjacency.NorthWest);
            var northeast = GetAdjacentID(landblock.Id, Adjacency.NorthEast);
            var southwest = GetAdjacentID(landblock.Id, Adjacency.SouthWest);
            var southeast = GetAdjacentID(landblock.Id, Adjacency.SouthEast);

            if (north != null)
                adjacents.Add(north.Value);
            if (south != null)
                adjacents.Add(south.Value);
            if (west != null)
                adjacents.Add(west.Value);
            if (east != null)
                adjacents.Add(east.Value);
            if (northwest != null)
                adjacents.Add(northwest.Value);
            if (northeast != null)
                adjacents.Add(northeast.Value);
            if (southwest != null)
                adjacents.Add(southwest.Value);
            if (southeast != null)
                adjacents.Add(southeast.Value);

            return adjacents;
        }

        /// <summary>
        /// Returns an adjacent landblock ID for a landblock
        /// </summary>
        private static LandblockId? GetAdjacentID(LandblockId landblock, Adjacency adjacency)
        {
            int lbx = landblock.LandblockX;
            int lby = landblock.LandblockY;

            switch (adjacency)
            {
                case Adjacency.North:
                    lby += 1;
                    break;
                case Adjacency.South:
                    lby -= 1;
                    break;
                case Adjacency.West:
                    lbx -= 1;
                    break;
                case Adjacency.East:
                    lbx += 1;
                    break;
                case Adjacency.NorthWest:
                    lby += 1;
                    lbx -= 1;
                    break;
                case Adjacency.NorthEast:
                    lby += 1;
                    lbx += 1;
                    break;
                case Adjacency.SouthWest:
                    lby -= 1;
                    lbx -= 1;
                    break;
                case Adjacency.SouthEast:
                    lby -= 1;
                    lbx += 1;
                    break;
            }

            if (lbx < 0 || lbx > 254 || lby < 0 || lby > 254)
                return null;

            return new LandblockId((byte)lbx, (byte)lby);
        }

        /// <summary>
        /// Rebuilds the adjacency cache for a landblock, and optionally rebuilds the adjacency caches
        /// for the adjacent landblocks if traverse is true
        /// </summary>
        private static void SetAdjacents(Landblock landblock, bool traverse = true, bool pSync = false)
        {
            landblock.Adjacents = GetAdjacents(landblock);

            if (pSync)
                landblock.PhysicsLandblock.SetAdjacents(landblock.Adjacents);

            if (traverse)
            {
                foreach (var adjacent in landblock.Adjacents)
                    SetAdjacents(adjacent, false);
            }
        }

        /// <summary>
        /// Queues a landblock for thread-safe unloading
        /// </summary>
        public static void AddToDestructionQueue(Landblock landblock)
        {
            destructionQueue.Add(landblock);
        }

        private static readonly System.Diagnostics.Stopwatch swTrySplitEach = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Processes the destruction queue in a thread-safe manner
        /// </summary>
        private static void UnloadLandblocks()
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
                        if (loadedLandblocks.Remove(landblock))
                        {
                            landblocks[landblock.Id.LandblockX, landblock.Id.LandblockY] = null;

                            // remove from landblock group
                            for (int i = landblockGroups.Count - 1; i >= 0 ; i--)
                            {
                                if (landblockGroups[i].Remove(landblock))
                                {
                                    if (landblockGroups[i].Count == 0)
                                        landblockGroups.RemoveAt(i);
                                    else if (ConfigManager.Config.Server.Threading.MultiThreadedLandblockGroupPhysicsTicking || ConfigManager.Config.Server.Threading.MultiThreadedLandblockGroupTicking) // Only try to split if multi-threading is enabled
                                    {
                                        swTrySplitEach.Restart();
                                        var splits = landblockGroups[i].TryThrottledSplit();
                                        swTrySplitEach.Stop();

                                        if (swTrySplitEach.Elapsed.TotalMilliseconds > 3)
                                            log.Warn($"[LANDBLOCK GROUP] TrySplit for {landblockGroups[i]} took: {swTrySplitEach.Elapsed.TotalMilliseconds:N2} ms");
                                        else if (swTrySplitEach.Elapsed.TotalMilliseconds > 1)
                                            log.Debug($"[LANDBLOCK GROUP] TrySplit for {landblockGroups[i]} took: {swTrySplitEach.Elapsed.TotalMilliseconds:N2} ms");

                                        if (splits != null)
                                        {
                                            if (splits.Count > 0)
                                            {
                                                log.Debug($"[LANDBLOCK GROUP] TrySplit resulted in {splits.Count} split(s) and took: {swTrySplitEach.Elapsed.TotalMilliseconds:N2} ms");
                                                log.Debug($"[LANDBLOCK GROUP] split for old: {landblockGroups[i]}");
                                            }

                                            foreach (var split in splits)
                                            {
                                                landblockGroups.Add(split);
                                                log.Debug($"[LANDBLOCK GROUP] split and new: {split}");
                                            }
                                        }
                                    }

                                    break;
                                }
                            }

                            NotifyAdjacents(landblock);
                        }
                        else
                            unloadFailed = true;
                    }

                    if (unloadFailed)
                        log.Error($"LandblockManager: failed to unload {landblock.Id.Raw:X8}");
                }
            }
        }

        /// <summary>
        /// Notifies the adjacent landblocks to rebuild their adjacency cache
        /// Called when a landblock is unloaded
        /// </summary>
        private static void NotifyAdjacents(Landblock landblock)
        {
            var adjacents = GetAdjacents(landblock);

            foreach (var adjacent in adjacents)
                SetAdjacents(adjacent, false, true);
        }

        /// <summary>
        /// Used on server shutdown
        /// </summary>
        public static void AddAllActiveLandblocksToDestructionQueue()
        {
            lock (landblockMutex)
            {
                foreach (var landblock in loadedLandblocks)
                    AddToDestructionQueue(landblock);
            }
        }

        public static EnvironChangeType? GlobalFogColor;

        private static void SetGlobalFogColor(EnvironChangeType environChangeType)
        {
            if (environChangeType.IsFog())
            {
                if (environChangeType == EnvironChangeType.Clear)
                    GlobalFogColor = null;
                else
                    GlobalFogColor = environChangeType;

                foreach (var landblock in loadedLandblocks)
                    landblock.SendCurrentEnviron();
            }
        }

        private static void SendGlobalEnvironSound(EnvironChangeType environChangeType)
        {
            if (environChangeType.IsSound())
            {
                foreach (var landblock in loadedLandblocks)
                    landblock.SendEnvironChange(environChangeType);
            }
        }

        public static void DoEnvironChange(EnvironChangeType environChangeType)
        {
            lock (landblockMutex)
            {
                if (environChangeType.IsFog())
                    SetGlobalFogColor(environChangeType);
                else
                    SendGlobalEnvironSound(environChangeType);
            }
        }
    }
}
