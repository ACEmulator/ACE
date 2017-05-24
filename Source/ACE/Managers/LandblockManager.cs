﻿using System;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameMessages.Messages;
using log4net;
using ACE.Common;
using ACE.Entity.Events;

namespace ACE.Managers
{
    public static class LandblockManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object landblockMutex = new object();

        // debating during this into a dictionary keyed on LandblockId.  would be functionally equivalent
        // and get rid of the 2D array shenanigans
        private static readonly Landblock[,] landblocks = new Landblock[256, 256];

        public static async void PlayerEnterWorld(Session session)
        {
            var task = DatabaseManager.Character.LoadCharacter(session.Player.Guid.Low);
            task.Wait();
            Character c = task.Result;

            await session.Player.Load(c);

            Landblock block = GetLandblock(c.Location.LandblockId, true);
            block.AddWorldObject(session.Player);

            session.Network.EnqueueSend(new GameMessageSystemChat("Welcome to Asheron's Call", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("  powered by ACEmulator  ", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("For more information on commands supported by this server, type @acehelp", ChatMessageType.Broadcast));
        }

        public static void AddObject(WorldObject worldObject)
        {
            Landblock block = GetLandblock(worldObject.Location.LandblockId, true);
            block.AddWorldObject(worldObject);
        }

        // TODO: Need to be able to read the position of an object on the landblock and get information about that object CFS

        public static void RemoveObject(WorldObject worldObject, bool neighbors)
        {
            Landblock block = GetLandblock(worldObject.Location.LandblockId, neighbors);
            block.RemoveWorldObject(worldObject.Guid, neighbors);
        }

        /// <summary>
        /// Relocates an object to the appropriate landblock
        /// </summary>
        public static void RelocateObject(WorldObject worldObject)
        {
            var block = GetLandblock(worldObject.Location.LandblockId, true);
            block.AddWorldObject(worldObject);
        }

        /// <summary>
        /// return wo in preparation to take it off the landblock and put it in a container.
        /// </summary>
        public static WorldObject GetWorldObject(Session source, ObjectGuid targetId)
        {
            var block = GetLandblock(source.Player.Location.LandblockId, true);
            return block.GetWorldObject(targetId);
        }

        public static void BroadcastByLandblockID(BroadcastEventArgs args, bool propogate, Quadrant quadrant, LandblockId landBlockId)
        {
            Landblock block = GetLandblock(landBlockId, false);
            block.Broadcast(args, propogate, quadrant);
        }

        public static Position GetWorldObjectPositionByLandblockID(ObjectGuid objectId, LandblockId landBlockId)
        {
            Landblock block = GetLandblock(landBlockId, false);
            return block.GetWorldObjectPosition(objectId);
        }

        public static float GetWorldObjectEffectiveUseRadiusByLandblockID(ObjectGuid objectId, LandblockId landBlockId)
        {
            Landblock block = GetLandblock(landBlockId, false);
            return block.GetWorldObjectEffectiveUseRadius(objectId);
        }

        /// gets the landblock specified, creating it if it is not already loaded.  will create all
        /// adjacent landblocks if propagate is true (outdoor world roaming).
        /// </summary>
        private static Landblock GetLandblock(LandblockId landblockId, bool propogate)
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
                        Landblock block = new Landblock(landblockId);

                        landblocks[x, y] = block;
                        bool autoLoad = propogate && landblockId.MapScope == Entity.Enum.MapScope.Outdoors;

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
                        block.StartUseTime();
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

            Landblock lb1 = landblocks[landblock1.LandblockX, landblock1.LandblockY];
            Landblock lb2 = landblocks[landblock2.LandblockX, landblock2.LandblockY];

            if (autoLoad && lb2 == null)
                lb2 = GetLandblock(landblock2, false);

            lb1.SetAdjacency(adjacency, lb2);

            if (lb2 != null)
            {
                int inverse = (((int)adjacency) + 4) % 8; // go halfway around the horn (+4) and mod 8 to wrap around
                Adjacency inverseAdjacency = (Adjacency)Enum.ToObject(typeof(Adjacency), inverse);
                lb2.SetAdjacency(inverseAdjacency, lb1);
            }
        }
    }
}
