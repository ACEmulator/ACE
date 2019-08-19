using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public static bool MultiThreadedLandblockGroupTicking = true;

        private static bool threadSeparatedLandblockGroupsNeedsRecalculating = true;

        private static readonly List<List<Landblock>> threadSeparatedLandblockGroups = new List<List<Landblock>>();

        public static int ThreadSeparatedLandblockGroupsCount
        {
            get
            {
                lock (landblockMutex)
                    return threadSeparatedLandblockGroups.Count;
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

        private static void AddLandblockToLandblockGroup(HashSet<Landblock> landblocksAdded, List<Landblock> workingSet, Landblock workingLandblock)
        {
            if (landblocksAdded.Add(workingLandblock))
            {
                workingSet.Add(workingLandblock);

                foreach (var adjacent in workingLandblock.Adjacents)
                    AddLandblockToLandblockGroup(landblocksAdded, workingSet, adjacent);
            }
        }

        private static void CheckIfLandblockGroupsNeedRecalculating()
        {
            if (!threadSeparatedLandblockGroupsNeedsRecalculating)
                return;

            // TODO: Change this so it's recalculated only when a landblock is added, not removed

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            lock (landblockMutex)
            {
                threadSeparatedLandblockGroups.Clear();

                var landblocksAdded = new HashSet<Landblock>(loadedLandblocks.Count);

                foreach (var loadedLandblock in loadedLandblocks)
                {
                    if (!landblocksAdded.Contains(loadedLandblock))
                    {
                        var workingSet = new List<Landblock>();

                        AddLandblockToLandblockGroup(landblocksAdded, workingSet, loadedLandblock);

                        threadSeparatedLandblockGroups.Add(workingSet);
                    }
                }

                // Go through the landblock groups and see if any are close enough to be merged
                // This helps avoid multi-threading issues when two different landblock groups try to interact with the same adjacent
                //
                // To make this as performant as possible, we break down each landblock group into it's outer most square.
                // Then, we use these squares to determine if a group overlaps or is close enough to another group and should be merged
                var groupBoundaries =
                    new List<(List<Landblock> group, int xMin, int xMax, int yMin, int yMax, int xCenter, int yCenter,
                        int width, int height)>();

                foreach (var group in threadSeparatedLandblockGroups)
                {
                    if (group[0].IsDungeon)
                        continue;

                    var xMin = int.MaxValue;
                    var xMax = int.MinValue;
                    var yMin = int.MaxValue;
                    var yMax = int.MinValue;

                    foreach (var landblock in group)
                    {
                        if (landblock.Id.LandblockX < xMin) xMin = landblock.Id.LandblockX;
                        if (landblock.Id.LandblockX > xMax) xMax = landblock.Id.LandblockX;
                        if (landblock.Id.LandblockY < yMin) yMin = landblock.Id.LandblockY;
                        if (landblock.Id.LandblockY > yMax) yMax = landblock.Id.LandblockY;
                    }

                    int xCenter = xMin + ((xMax - xMin) / 2);
                    int yCenter = yMin + ((yMax - yMin) / 2);

                    groupBoundaries.Add((group, xMin, xMax, yMin, yMax, xCenter, yCenter, xMax - xMin, yMax - yMin));
                }

                for (int i = groupBoundaries.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < i; j++)
                    {
                        var distance = Math.Max(
                            Math.Abs(groupBoundaries[i].xCenter - groupBoundaries[j].xCenter) -
                            (groupBoundaries[i].width + groupBoundaries[j].width) / 2,
                            Math.Abs(groupBoundaries[i].yCenter - groupBoundaries[j].yCenter) -
                            (groupBoundaries[i].height + groupBoundaries[j].height) / 2);

                        if (distance <= 10)
                        {
                            // We're close enough, copy i into j and remove i
                            groupBoundaries[j].group.AddRange(groupBoundaries[i].group);

                            threadSeparatedLandblockGroups.Remove(groupBoundaries[i].group);

                            // j needs to be recalculated
                            var xMin = int.MaxValue;
                            var xMax = int.MinValue;
                            var yMin = int.MaxValue;
                            var yMax = int.MinValue;

                            foreach (var landblock in groupBoundaries[j].group)
                            {
                                if (landblock.Id.LandblockX < xMin) xMin = landblock.Id.LandblockX;
                                if (landblock.Id.LandblockX > xMax) xMax = landblock.Id.LandblockX;
                                if (landblock.Id.LandblockY < yMin) yMin = landblock.Id.LandblockY;
                                if (landblock.Id.LandblockY > yMax) yMax = landblock.Id.LandblockY;
                            }

                            int xCenter = xMin + ((xMax - xMin) / 2);
                            int yCenter = yMin + ((yMax - yMin) / 2);

                            groupBoundaries[j] = (groupBoundaries[j].group, xMin, xMax, yMin, yMax, xCenter, yCenter,
                                xMax - xMin, yMax - yMin);

                            break;
                        }
                    }
                }

                // Debugging
                if (landblocksAdded.Count != loadedLandblocks.Count)
                    log.Error(
                        $"landblocksAdded.Count: ({landblocksAdded.Count}) != loadedLandblocks.Count ({loadedLandblocks.Count})");
                var count = 0;
                foreach (var group in threadSeparatedLandblockGroups)
                    count += group.Count;
                if (count != loadedLandblocks.Count)
                    log.Error($"count ({count}) != loadedLandblocks.Count ({loadedLandblocks.Count})");

                threadSeparatedLandblockGroupsNeedsRecalculating = false;
            }

            sw.Stop();
            if (sw.ElapsedMilliseconds > 1)
                log.Warn($"sw.ElapsedMilliseconds: {sw.ElapsedMilliseconds}");
        }

        public static void Tick(double portalYearTicks)
        {
            // update positions through physics engine
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.LandblockManager_TickPhysics);
            TickPhysics(portalYearTicks);
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.LandblockManager_TickPhysics);

            // Tick all of our Landblocks and WorldObjects
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.LandblockManager_Tick);
            Tick();
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.LandblockManager_Tick);

            // clean up inactive landblocks
            UnloadLandblocks();
        }

        /// <summary>
        /// Processes physics objects in all active landblocks for updating
        /// </summary>
        private static void TickPhysics(double portalYearTicks)
        {
            CheckIfLandblockGroupsNeedRecalculating();

            var movedObjects = new ConcurrentBag<WorldObject>();

            if (false && MultiThreadedLandblockGroupTicking) // Disabled for now...
            {
                Parallel.ForEach(threadSeparatedLandblockGroups, landblockGroup =>
                {
                    foreach (var landblock in landblockGroup)
                        landblock.TickPhysics(portalYearTicks, movedObjects);
                });
            }
            else
            {
                foreach (var landblockGroup in threadSeparatedLandblockGroups)
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

        private static void Tick()
        {
            CheckIfLandblockGroupsNeedRecalculating();

            if (MultiThreadedLandblockGroupTicking)
            {
                Parallel.ForEach(threadSeparatedLandblockGroups, landblockGroup =>
                {
                    foreach (var landblock in landblockGroup)
                        landblock.Tick(Time.GetUnixTime());
                });
            }
            else
            {
                foreach (var landblockGroup in threadSeparatedLandblockGroups)
                {
                    foreach (var landblock in landblockGroup)
                        landblock.Tick(Time.GetUnixTime());
                }
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
            Landblock landblock = null;

            lock (landblockMutex)
            {
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

                    threadSeparatedLandblockGroupsNeedsRecalculating = true;
                }

                if (permaload)
                    landblock.Permaload = true;
            }

            // load adjacents, if applicable
            if (loadAdjacents)
            {
                var adjacents = GetAdjacentIDs(landblock);
                foreach (var adjacent in adjacents)
                    GetLandblock(adjacent, false, permaload);
            }

            // cache adjacencies
            SetAdjacents(landblock, true, true);

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
        /// Returns the list of all active landblocks. This is just all loaded landblocks that are !IsDormant
        /// </summary>
        public static List<Landblock> GetActiveLandblocks()
        {
            lock (landblockMutex)
                return loadedLandblocks.Where(r => !r.IsDormant).ToList();
        }

        public static List<Landblock> GetAdjacents(LandblockId landblockID)
        {
            Landblock landblock;

            lock (landblockMutex)
                landblock = landblocks[landblockID.LandblockX, landblockID.LandblockY];

            if (landblock == null)
                return null;

            return GetAdjacents(landblock);
        }

        /// <summary>
        /// Returns the active, non-null adjacents for a landblock
        /// </summary>
        private static List<Landblock> GetAdjacents(Landblock landblock)
        {
            var adjacentIDs = GetAdjacentIDs(landblock);

            var adjacents = new List<Landblock>();

            lock (landblockMutex)
            {
                foreach (var adjacentID in adjacentIDs)
                {
                    var adjacent = landblocks[adjacentID.LandblockX, adjacentID.LandblockY];
                    if (adjacent != null)
                        adjacents.Add(adjacent);
                }
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
            {
                var pLandblock = Physics.Common.LScape.get_landblock(landblock.Id.Raw | 0xFFFF);
                pLandblock.get_adjacents(true); // TODO: change this to a new function rebuild_adjacents
            }

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
                        if (loadedLandblocks.Remove(landblock))
                        {
                            landblocks[landblock.Id.LandblockX, landblock.Id.LandblockY] = null;
                            threadSeparatedLandblockGroupsNeedsRecalculating = true;
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

        public static void SetGlobalFogColor(EnvironChangeType environChangeType)
        {
            if (environChangeType.IsFog())
            {
                if (environChangeType == EnvironChangeType.Clear)
                    GlobalFogColor = null;
                else
                    GlobalFogColor = environChangeType;

                foreach (var landblock in loadedLandblocks)
                {
                    landblock.SendCurrentEnviron();
                }
            }
        }

        public static void SendGlobalEnvironSound(EnvironChangeType environChangeType)
        {
            if (environChangeType.IsSound())
            {
                foreach (var landblock in loadedLandblocks)
                {
                    landblock.SendEnvironChange(environChangeType);
                }
            }
        }

        public static void DoEnvironChange(EnvironChangeType environChangeType)
        {
            if (environChangeType.IsFog())
                SetGlobalFogColor(environChangeType);
            else
                SendGlobalEnvironSound(environChangeType);
        }
    }
}
