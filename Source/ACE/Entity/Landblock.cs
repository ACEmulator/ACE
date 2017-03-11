using ACE.Entity.Events;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACE.Entity
{
    /// <summary>
    /// the gist of a landblock is that, generally, everything on it publishes
    /// to and subscribes to everything else in the landblock.  x/y in an outdoor
    /// landblock goes from 0 to 192.  "indoor" (dungeon) landblocks have no
    /// functional limit as players can't freely roam in/out of them
    /// </summary>
    internal class Landblock : IDisposable
    {
        private const float adjacencyLoadRange = 96f;
        private const float outDoorChatRange = 75f;
        private const float indoorChatRange = 25f;
        private const float maxXY = 192f;

        private LandblockId id;

        private object objectCacheLocker = new object();
        private Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();
        
        private Dictionary<Adjacency, Landblock> adjacencies = new Dictionary<Adjacency, Landblock>();

        private byte cellGridMaxX = 8; // todo: load from cell.dat
        private byte cellGridMaxY = 8; // todo: load from cell.dat

        // not sure if a full object is necessary here.  I don't think a Landcell has any
        // inherent functionality that needs to be modelled in an object.
        private Landcell[,] cellGrid; // todo: load from cell.dat

        public LandblockId Id
        {
            get { return id; }
        }

        public Landblock(LandblockId id)
        {
            this.id = id;

            // initialize adjacency array
            this.adjacencies.Add(Adjacency.North, null);
            this.adjacencies.Add(Adjacency.NorthEast, null);
            this.adjacencies.Add(Adjacency.East, null);
            this.adjacencies.Add(Adjacency.SouthEast, null);
            this.adjacencies.Add(Adjacency.South, null);
            this.adjacencies.Add(Adjacency.SouthWest, null);
            this.adjacencies.Add(Adjacency.West, null);
            this.adjacencies.Add(Adjacency.NorthWest, null);

            // TODO: Load cell.dat contents
            //   1. landblock cell structure
            //   2. terrain data
            // TODO: Load portal.dat contents (as/if needed)
            // TODO: Load spawn data

            // TODO: load objects from world database such as lifestones, doors, player corpses, NPCs, Vendors
        }

        public void SetAdjacency(Adjacency adjacency, Landblock landblock)
        {
            adjacencies[adjacency] = landblock;
        }

        public Landblock NorthAdjacency
        {
            get { return adjacencies[Adjacency.North]; }
            set { adjacencies[Adjacency.North] = value; }
        }

        public Landblock NorthEastAdjacency
        {
            get { return adjacencies[Adjacency.NorthEast]; }
            set { adjacencies[Adjacency.NorthEast] = value; }
        }

        public Landblock EastAdjacency
        {
            get { return adjacencies[Adjacency.East]; }
            set { adjacencies[Adjacency.East] = value; }
        }

        public Landblock SouthEastAdjacency
        {
            get { return adjacencies[Adjacency.SouthEast]; }
            set { adjacencies[Adjacency.SouthEast] = value; }
        }

        public Landblock SouthAdjacency
        {
            get { return adjacencies[Adjacency.South]; }
            set { adjacencies[Adjacency.South] = value; }
        }

        public Landblock SouthWestAdjacency
        {
            get { return adjacencies[Adjacency.SouthWest]; }
            set { adjacencies[Adjacency.SouthWest] = value; }
        }

        public Landblock WestAdjacency
        {
            get { return adjacencies[Adjacency.West]; }
            set { adjacencies[Adjacency.West] = value; }
        }

        public Landblock NorthWestAdjacency
        {
            get { return adjacencies[Adjacency.NorthWest]; }
            set { adjacencies[Adjacency.NorthWest] = value; }
        }

        /// <summary>
        /// called when a landblock is to be unloaded
        /// </summary>
        public void Dispose()
        {
            // save all mutable objects, release memory
            // TODO: implement
        }

        public void AddWorldObject(WorldObject wo)
        {
            List<WorldObject> allObjects;

            lock (objectCacheLocker)
            {
                allObjects = this.worldObjects.Values.ToList();
                this.worldObjects[wo.Guid] = wo;
            }
            
            var args = BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, wo);
            Broadcast(args, true, Quadrant.All);

            // if this is a player, tell them about everything else we have.
            if (wo is Player)
            {
                // send them the initial burst of objects
                Parallel.ForEach(allObjects, (o) => (wo as Player).TrackObject(o));
            }
        }
        
        public void SendChatMessage(WorldObject sender, ChatMessageArgs chatMessage)
        {
            // only players receive this
            List<Player> players = null;

            lock (objectCacheLocker)
            {
                players = this.worldObjects.OfType<Player>().ToList();
            }
            
            BroadcastEventArgs args = BroadcastEventArgs.CreateChatAction(sender, chatMessage);
            Broadcast(args, true, Quadrant.All);
        }

        public void RemoveWorldObject(ObjectGuid objectId, bool adjacencyMove)
        {
            WorldObject wo = null;

            lock (objectCacheLocker)
            {
                if (this.worldObjects.ContainsKey(objectId))
                {
                    wo = this.worldObjects[objectId];
                    this.worldObjects.Remove(objectId);
                }
            }

            var args = BroadcastEventArgs.CreateAction(BroadcastAction.Delete, wo);
            Broadcast(args, true, Quadrant.All);
        }

        /// <summary>
        /// handles broadcasting an event to the players in this landblock and to the proper adjacencies
        /// </summary>
        private void Broadcast(BroadcastEventArgs args, bool propogate, Quadrant quadrant)
        {
            WorldObject wo = args.Sender;
            List<Player> players = null;

            lock (objectCacheLocker)
            {
                players = this.worldObjects.OfType<Player>().ToList();
            }

            // filter to applicable players
            players = players.Where(p => p.Position?.IsInQuadrant(quadrant) ?? false).ToList();

            switch (args.ActionType)
            {
                case BroadcastAction.Delete:
                    {
                        Parallel.ForEach(players, p => p.StopTrackingObject(wo));
                        break;
                    }
                case BroadcastAction.AddOrUpdate:
                    {
                        Player player = args.Sender as Player;
                        if (player != null)
                        {
                            player.SendUpdatePosition();
                            players = players.Where(p => p.Guid != args.Sender.Guid).ToList();
                        }
                        Parallel.ForEach(players, p => p.TrackObject(wo));
                        break;
                    }
                case BroadcastAction.LocalChat:
                    {
                        // TODO: implement range dectection for chat events
                        Parallel.ForEach(players, p => p.ReceiveChat(wo, args.ChatMessage));
                        break;
                    }
            }

            // short circuit when there's no functional adjacency
            if (!propogate || wo?.Position?.LandblockId.MapScope == Enum.MapScope.IndoorsSmall)
                return;

            if (propogate)
            {
                if (wo.Position.Offset.X < adjacencyLoadRange)
                {
                    WestAdjacency?.Broadcast(args, false, Quadrant.NorthEast | Quadrant.SouthEast);

                    if (wo.Position.Offset.Y < adjacencyLoadRange)
                        SouthWestAdjacency?.Broadcast(args, false, Quadrant.NorthEast);

                    if (wo.Position.Offset.Y > (maxXY - adjacencyLoadRange))
                        NorthWestAdjacency?.Broadcast(args, false, Quadrant.SouthEast);
                }

                if (wo.Position.Offset.Y < adjacencyLoadRange)
                    SouthAdjacency?.Broadcast(args, false, Quadrant.NorthEast | Quadrant.NorthWest);

                if (wo.Position.Offset.X > (maxXY - adjacencyLoadRange))
                {
                    EastAdjacency?.Broadcast(args, false, Quadrant.NorthWest | Quadrant.SouthWest);

                    if (wo.Position.Offset.Y < adjacencyLoadRange)
                        SouthEastAdjacency?.Broadcast(args, false, Quadrant.NorthWest);

                    if (wo.Position.Offset.Y > (maxXY - adjacencyLoadRange))
                        NorthEastAdjacency?.Broadcast(args, false, Quadrant.SouthWest);
                }

                if (wo.Position.Offset.Y > (maxXY - adjacencyLoadRange))
                    NorthAdjacency?.Broadcast(args, false, Quadrant.SouthEast | Quadrant.SouthWest);
            }
        }

        /// <summary>
        /// main game loop
        /// </summary>
        public void UseTime(double lastTick)
        {
            // here we'd move server objects in motion (subject to landscape) and do physics collision detection

            // for now, we'll move players around
            List<MutableWorldObject> movedObjects = null;

            lock (objectCacheLocker)
            {
                // TODO: Improve the efficiency of this line
                movedObjects = this.worldObjects.Values.Where(o => (o as MutableWorldObject) != null).Cast<MutableWorldObject>().ToList();
            }

            movedObjects = movedObjects.Where(p => p.LastMovedTicks > p.LastMovementBroadcastTicks).ToList();

            // broadcast
            Parallel.ForEach(movedObjects, mo => Broadcast(BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, mo), true, Quadrant.All));
        }

        /// <summary>
        /// would be used to find out if a landblock was idle
        /// </summary>
        public int GetPlayerCount()
        {
            return this.worldObjects.Values.OfType<Player>().Count();
        }
    }
}
