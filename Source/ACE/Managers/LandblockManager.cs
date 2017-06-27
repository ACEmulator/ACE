using System;
using System.Threading.Tasks;
using ACE.Common;
using System.Collections.Generic;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using log4net;
using System.Diagnostics;

namespace ACE.Managers
{
    public static class LandblockManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object landblockMutex = new object();

        // FIXME(ddevec): Does making this volatile really make double-check locking safe?
        private volatile static Landblock[,] landblocks = new Landblock[256, 256];

        /// <summary>
        /// This list of all currently active landblocks may only be accessed externally from locations in which the landblocks /CANNOT/ be concurrently modified
        ///   e.g. -- the WorldManager update loop
        /// Landblocks should not be directly accessed by world objects, or world-object associated handlers.
        ///   Instead use: FIXME(ddevec): TBD, interface a work in progress
        /// </summary>
        public static List<Landblock> ActiveLandblocks { get; } = new List<Landblock>();

        public static async void PlayerEnterWorld(Session session, ObjectGuid guid)
        {
            AceCharacter c = DatabaseManager.Shard.GetCharacter(guid.Full);

            session.Player = new Player(session, c);
            await Task.Run(() => session.Player.Load(c));

            // check the value of the welcome message. Only display it if it is not empty
            if (!String.IsNullOrEmpty(ConfigManager.Config.Server.Welcome))
            {
                session.Network.EnqueueSend(new GameEventPopupString(session, ConfigManager.Config.Server.Welcome));
            }
            Landblock block = GetLandblock(c.Location.LandblockId, true);
            // Must enqueue add world object -- this is called from a message handler context
            block.AddWorldObject(session.Player);

            session.Network.EnqueueSend(new GameMessageSystemChat("Welcome to Asheron's Call", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("  powered by ACEmulator  ", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("For more information on commands supported by this server, type @acehelp", ChatMessageType.Broadcast));
        }

        public static void AddObject(WorldObject worldObject)
        {
            var block = GetLandblock(worldObject.Location.LandblockId, true);
            block.AddWorldObject(worldObject);
        }

        public static ActionChain GetAddObjectChain(WorldObject worldObject)
        {
            Landblock block = GetLandblock(worldObject.Location.LandblockId, true);
            return block.GetAddWorldObjectChain(worldObject);
        }

        // TODO: Need to be able to read the position of an object on the landblock and get information about that object CFS

        public static void RemoveObject(WorldObject worldObject)
        {
            var block = GetLandblock(worldObject.Location.LandblockId, true);
            block.RemoveWorldObject(worldObject.Guid, false);
        }

        /// <summary>
        /// Relocates an object to the appropriate landblock -- Should only be called from physics/worldmanager -- not player!
        /// </summary>
        public static void RelocateObjectForPhysics(WorldObject worldObject)
        {
            var oldBlock = worldObject.CurrentLandblock;
            var newBlock = GetLandblock(worldObject.Location.LandblockId, true);
            // Remove from the old landblock -- force
            if (oldBlock != null)
            {
                oldBlock.RemoveWorldObjectForPhysics(worldObject.Guid, true);
            }
            // Add to the new landblock
            newBlock.AddWorldObjectForPhysics(worldObject);
        }

        /// <summary>
        /// gets the landblock specified, creating it if it is not already loaded.  will create all
        /// adjacent landblocks if propagate is true (outdoor world roaming).
        /// </summary>
        private static Landblock GetLandblock(LandblockId landblockId, bool propagate)
        {
            int x = landblockId.LandblockX;
            int y = landblockId.LandblockY;

            // standard check/lock/recheck pattern
            if (landblocks[x, y] == null)
            {
                lock (landblockMutex)
                {
                    if (landblocks[x, y] == null)
                    {
                        // load up this landblock
                        var block = new Landblock(landblockId);

                        landblocks[x, y] = block;
                        var autoLoad = propagate && landblockId.MapScope == Entity.Enum.MapScope.Outdoors;

                        if (x > 0)
                        {
                            SetAdjacency(landblockId, landblockId.West, Adjacency.West, autoLoad);

                            if (y > 0)
                                SetAdjacency(landblockId, landblockId.SouthWest, Adjacency.SouthWest, autoLoad);

                            if (y < 255)
                                SetAdjacency(landblockId, landblockId.NorthWest, Adjacency.NorthWest, autoLoad);
                        }

                        if (x < 255)
                        {
                            SetAdjacency(landblockId, landblockId.East, Adjacency.East, autoLoad);

                            if (y > 0)
                                SetAdjacency(landblockId, landblockId.SouthEast, Adjacency.SouthEast, autoLoad);

                            if (y < 255)
                                SetAdjacency(landblockId, landblockId.NorthEast, Adjacency.NorthEast, autoLoad);
                        }

                        if (y > 0)
                            SetAdjacency(landblockId, landblockId.South, Adjacency.South, autoLoad);

                        if (y < 255)
                            SetAdjacency(landblockId, landblockId.North, Adjacency.North, autoLoad);

                        // kick off the landblock use time thread
                        // block.StartUseTime();
                        ActiveLandblocks.Add(landblocks[x, y]);
                    }
                }
            }

            return landblocks[x, y];
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
            if (landblock1.MapScope != Entity.Enum.MapScope.Outdoors || landblock2.MapScope != Entity.Enum.MapScope.Outdoors)
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

        public static void ForceLoadLandBlock(LandblockId blockid)
        {
            Stopwatch sw = Stopwatch.StartNew();
            GetLandblock(blockid, false);
            sw.Stop();
            log.DebugFormat("Loaded Landblock {0} in {1} milliseconds ", blockid.Landblock.ToString("X4"), sw.ElapsedMilliseconds);
            Console.WriteLine("Loaded Landblock {0} in {1} milliseconds ", blockid.Landblock.ToString("X4"), sw.ElapsedMilliseconds);
        }

        public static void FinishedForceLoading()
        {
            log.DebugFormat("Finished Forceloading Landblocks");
            Console.WriteLine("Finished Forceloading Landblocks");
        }
    }
}
