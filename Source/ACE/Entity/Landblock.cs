using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using ACE.Entity.Events;
using ACE.Managers;

using log4net;
using ACE.Database;
using ACE.Network.GameEvent.Events;
using ACE.Network;

namespace ACE.Entity
{
    /// <summary>
    /// the gist of a landblock is that, generally, everything on it publishes
    /// to and subscribes to everything else in the landblock.  x/y in an outdoor
    /// landblock goes from 0 to 192.  "indoor" (dungeon) landblocks have no
    /// functional limit as players can't freely roam in/out of them
    /// </summary>
    internal class Landblock
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const float adjacencyLoadRange = 96f;
        private const float outDoorChatRange = 75f;
        private const float indoorChatRange = 25f;
        private const float maxXY = 192f;

        private LandblockId id;

        private readonly ConcurrentDictionary<ObjectGuid, WorldObject> worldObjects = new ConcurrentDictionary<ObjectGuid, WorldObject>();
        private readonly ConcurrentDictionary<Adjacency, Landblock> adjacencies = new ConcurrentDictionary<Adjacency, Landblock>();

        // private byte cellGridMaxX = 8; // todo: load from cell.dat
        // private byte cellGridMaxY = 8; // todo: load from cell.dat

        // not sure if a full object is necessary here.  I don't think a Landcell has any
        // inherent functionality that needs to be modelled in an object.
        // private Landcell[,] cellGrid; // todo: load from cell.dat

        private bool running = false;

        public LandblockId Id
        {
            get { return id; }
        }

        public Landblock(LandblockId id)
        {
            this.id = id;

            // initialize adjacency array
            this.adjacencies.TryAdd(Adjacency.North, null);
            this.adjacencies.TryAdd(Adjacency.NorthEast, null);
            this.adjacencies.TryAdd(Adjacency.East, null);
            this.adjacencies.TryAdd(Adjacency.SouthEast, null);
            this.adjacencies.TryAdd(Adjacency.South, null);
            this.adjacencies.TryAdd(Adjacency.SouthWest, null);
            this.adjacencies.TryAdd(Adjacency.West, null);
            this.adjacencies.TryAdd(Adjacency.NorthWest, null);

            // TODO: Load cell.dat contents
            //   1. landblock cell structure
            //   2. terrain data
            // TODO: Load portal.dat contents (as/if needed)
            // TODO: Load spawn data

            // TODO: load objects from world database such as lifestones, doors, player corpses, NPCs, Vendors
            var objects = DatabaseManager.World.GetObjectsByLandblock(this.id.Landblock);
            foreach (var o in objects)
            {
                ImmutableWorldObject iwo = new ImmutableWorldObject(o);
                worldObjects.TryAdd(iwo.Guid, iwo);
            }

            var creatures = DatabaseManager.World.GetCreaturesByLandblock(this.id.Landblock);
            foreach (var c in creatures)
            {
                Creature cwo = new Creature(c);
                worldObjects.TryAdd(cwo.Guid, cwo);
            }
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

        public void StartUseTime()
        {
            running = true;

            new Thread(UseTime).Start();
        }

        public void AddWorldObject(params WorldObject[] wolist)
        {   
            List<WorldObject> allObjects;

                allObjects = this.worldObjects.Values.ToList();
                foreach (WorldObject wo in wolist)
                {
                    Log($"adding {wo.Guid.Full.ToString("X")}");
                    this.worldObjects[wo.Guid] = wo;
                    var args = BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, wo);
                    Broadcast(args, true, Quadrant.All);

                    // if this is a player, tell them about everything else we have.
                    if (wo is Player)
                    {
                        // send them the initial burst of objects
                        Log($"blasting player \"{(wo as Player).Name}\" with {allObjects.Count} objects.");
                        Parallel.ForEach(allObjects, (o) => (wo as Player).TrackObject(o));
                    }
                }
        }

        public void RemoveWorldObject(bool adjacencyMove, params ObjectGuid[] objectIdlist)
        {
            WorldObject wo = null;

                foreach (ObjectGuid objectId in objectIdlist)
                {
                    Log($"removing {objectId.Full.ToString("X")}");
                    if (this.worldObjects.ContainsKey(objectId))
                    {
                        WorldObject rwo = null;
                        wo = this.worldObjects[objectId];
                        this.worldObjects.TryRemove(objectId, out rwo);
                        if (rwo == null)
                            Log($"failed to remove {objectId.Full.ToString("X")}");
                    }
                }

            // suppress broadcasting when it's just an adjacency move.  clients will naturally just stop
            // tracking stuff if they're too far, or the new landblock will broadcast to them if they're
            // close enough.
            if (!adjacencyMove && this.id.MapScope == Enum.MapScope.Outdoors && wo != null)
            {
                var args = BroadcastEventArgs.CreateAction(BroadcastAction.Delete, wo);
                Broadcast(args, true, Quadrant.All);
            }
        }

        public WorldObject GetWorldObject(ObjectGuid objectId)
        {
           Log($"Getting WorldObject {objectId.Full:X}");
               return this.worldObjects.ContainsKey(objectId) ? this.worldObjects[objectId] : null;
        }

        /// <summary>
        /// Handle the QueryHealth action between the source Object and its target
        /// </summary>
        public void HandleQueryHealth(Session source, ObjectGuid targetId)
        {
            if (targetId.IsPlayer())
            {
                Player pl = null;

                    if (this.worldObjects.ContainsKey(targetId))
                        pl = (Player)this.worldObjects[targetId];
                if (pl == null)
                {
                    // check adjacent landblocks for the targetId
                    foreach (var block in adjacencies)
                    {
                            if (block.Value.worldObjects.ContainsKey(targetId))
                                pl = (Player)this.worldObjects[targetId];
                    }
                }
                if (pl != null)
                {
                    float healthPercentage = (float)pl.Health.Current / (float)pl.Health.MaxValue;

                    var updateHealth = new GameEventUpdateHealth(source, targetId.Full, healthPercentage);
                    source.Network.EnqueueSend(updateHealth);
                }
            }
        }

        public void SendChatMessage(WorldObject sender, ChatMessageArgs chatMessage)
        {
            // only players receive this
            List<Player> players = null;

                players = this.worldObjects.Values.OfType<Player>().ToList();

            BroadcastEventArgs args = BroadcastEventArgs.CreateChatAction(sender, chatMessage);
            Broadcast(args, true, Quadrant.All);
        }

        /// <summary>
        /// handles broadcasting an event to the players in this landblock and to the proper adjacencies
        /// </summary>
        private void Broadcast(BroadcastEventArgs args, bool propogate, Quadrant quadrant)
        {
            WorldObject wo = args.Sender;
            List<Player> players = null;

            Log($"broadcasting object {args.Sender.Guid.Full.ToString("X")} - {args.ActionType}");

                players = this.worldObjects.Values.OfType<Player>().ToList();

            // filter to applicable players
            players = players.Where(p => p.Position?.IsInQuadrant(quadrant) ?? false).ToList();

            switch (args.ActionType)
            {
                case BroadcastAction.Delete:
                    {
                        Parallel.ForEach(players, p => p.StopTrackingObject(wo.Guid));
                        break;
                    }
                case BroadcastAction.AddOrUpdate:
                    {
                        // players never need an update of themselves
                        players = players.Where(p => p.Guid != args.Sender.Guid).ToList();
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
            if (!propogate || wo?.Position?.LandblockId.MapScope != Enum.MapScope.Outdoors)
                return;

            if (propogate)
            {
                Log($"propogating broadcasting object {args.Sender.Guid.Full.ToString("X")} - {args.ActionType} to adjacencies");

                if (wo.Position.PositionX < adjacencyLoadRange)
                {
                    WestAdjacency?.Broadcast(args, false, Quadrant.NorthEast | Quadrant.SouthEast);

                    if (wo.Position.PositionY < adjacencyLoadRange)
                        SouthWestAdjacency?.Broadcast(args, false, Quadrant.NorthEast);

                    if (wo.Position.PositionY > (maxXY - adjacencyLoadRange))
                        NorthWestAdjacency?.Broadcast(args, false, Quadrant.SouthEast);
                }

                if (wo.Position.PositionY < adjacencyLoadRange)
                    SouthAdjacency?.Broadcast(args, false, Quadrant.NorthEast | Quadrant.NorthWest);

                if (wo.Position.PositionX > (maxXY - adjacencyLoadRange))
                {
                    EastAdjacency?.Broadcast(args, false, Quadrant.NorthWest | Quadrant.SouthWest);

                    if (wo.Position.PositionY < adjacencyLoadRange)
                        SouthEastAdjacency?.Broadcast(args, false, Quadrant.NorthWest);

                    if (wo.Position.PositionY > (maxXY - adjacencyLoadRange))
                        NorthEastAdjacency?.Broadcast(args, false, Quadrant.SouthWest);
                }

                if (wo.Position.PositionY > (maxXY - adjacencyLoadRange))
                    NorthAdjacency?.Broadcast(args, false, Quadrant.SouthEast | Quadrant.SouthWest);
            }
        }

        /// <summary>
        /// main game loop
        /// </summary>
        public void UseTime()
        {
            while (running)
            {      
                // Expire World Objects By Time.
                List<WorldObject> worldObjects = null;
                DateTime now = DateTime.Now;
                worldObjects = this.worldObjects.Values.OfType<WorldObject>().ToList();
                worldObjects = worldObjects.Where(p => !p.DieFlag & p.DieOn >= now).ToList();
                if (worldObjects != null & worldObjects.Count > 0)
                     worldObjects.ForEach(m => this.RemoveWorldObject(false, m.Guid));
                
                // here we'd move server objects in motion (subject to landscape) and do physics collision detection

                // for now, we'll move players around
                List<MutableWorldObject> movedObjects = null;

                movedObjects = this.worldObjects.Values.OfType<MutableWorldObject>().ToList();

                movedObjects = movedObjects.Where(p => p.LastUpdatedTicks >= p.LastMovementBroadcastTicks).ToList();

                // flag them as updated now in order to reduce chance of missing an update
                movedObjects.ForEach(m => m.LastMovementBroadcastTicks = WorldManager.PortalYearTicks);

                if (this.id.MapScope == Enum.MapScope.Outdoors)
                {
                    // check to see if a player or other mutable object "roamed" to an adjacent landblock
                    var objectsToRelocate = movedObjects.Where(m => m.Position.LandblockId.IsAdjacentTo(this.id) && m.Position.LandblockId != this.id).ToList();

                    // so, these objects moved to an adjacent block.  they could have recalled to that block, died and bounced to a lifestone on that block, or
                    // just simply walked accross the border line.  in any case, we won't delete them, we'll just transfer them.  the trick, though, is to
                    // figure out how to treat it in adjacent landblocks.  if the player walks across the southern border, the north adjacency needs to remove
                    // them, but the south is actually getting them.  we need to avoid sending Delete+Create to clients that already know about it, though.

                    objectsToRelocate.ForEach(o => Log($"attempting to relocate object {o.Name} ({o.Guid.Full.ToString("X")})"));

                    // RelocateObject will put them in the right landblock
                    objectsToRelocate.ForEach(o => LandblockManager.RelocateObject(o));

                    // Remove has logic to make sure it doesn't double up the delete+create when "true" is passed.
                    objectsToRelocate.ForEach(o => RemoveWorldObject(true, o.Guid));
                }

                // broadcast
                Parallel.ForEach(movedObjects, mo =>
                {
                    if (mo.Position.LandblockId == this.id)
                    {
                        // update if it's still here
                        Broadcast(BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, mo), true, Quadrant.All);
                    }
                    else
                    {
                        // remove and readd if it's not
                        this.RemoveWorldObject(false, mo.Guid);
                        LandblockManager.AddObject(mo);
                    }
                });

                // TODO: figure out if this landblock can be unloaded

                Thread.Sleep(1);
            }

            // TODO: release resources
        }

        private void Log(string message)
        {
            log.Debug($"LB {id.Landblock.ToString("X")}: {message}");
        }
    }
}
