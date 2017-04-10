﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ACE.Entity.Events;
using ACE.Managers;

using log4net;
using ACE.Database;
using ACE.Network.GameEvent.Events;
using ACE.Network;
using ACE.Network.GameAction;
using ACE.Entity.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Enum;

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

        private readonly object objectCacheLocker = new object();
        private readonly Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();

        private readonly Dictionary<Adjacency, Landblock> adjacencies = new Dictionary<Adjacency, Landblock>();

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
            var objects = DatabaseManager.World.GetObjectsByLandblock(this.id.Landblock);
            foreach (var o in objects)
            {
                ImmutableWorldObject iwo = new ImmutableWorldObject(o);
                worldObjects.Add(iwo.Guid, iwo);
            }

            var creatures = DatabaseManager.World.GetCreaturesByLandblock(this.id.Landblock);
            foreach (var c in creatures)
            {
                Creature cwo = new Creature(c);
                worldObjects.Add(cwo.Guid, cwo);
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

        public void AddWorldObject(WorldObject wo)
        {
            List<WorldObject> allObjects;

            Log($"adding {wo.Guid.Full.ToString("X")}");

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
                Log($"blasting player \"{(wo as Player).Name}\" with {allObjects.Count} objects.");
                Parallel.ForEach(allObjects, (o) => (wo as Player).TrackObject(o));
            }
        }

        public void RemoveWorldObject(ObjectGuid objectId, bool adjacencyMove)
        {
            WorldObject wo = null;

            Log($"removing {objectId.Full.ToString("X")}");

            lock (objectCacheLocker)
            {
                if (this.worldObjects.ContainsKey(objectId))
                {
                    wo = this.worldObjects[objectId];
                    this.worldObjects.Remove(objectId);
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

            lock (objectCacheLocker)
            {
                return this.worldObjects.ContainsKey(objectId) ? this.worldObjects[objectId] : null;
            }
        }

        /// <summary>
        /// Handle the QueryHealth action between the source Object and its target
        /// </summary>
        public void HandleQueryHealth(Session source, ObjectGuid targetId)
        {
            if (targetId.IsPlayer())
            {
                Player pl = null;

                lock (objectCacheLocker)
                {
                    if (this.worldObjects.ContainsKey(targetId))
                        pl = (Player)this.worldObjects[targetId];
                }
                if (pl == null)
                {
                    // check adjacent landblocks for the targetId
                    foreach (var block in adjacencies)
                    {
                        lock (block.Value.objectCacheLocker)
                        {
                            if (block.Value.worldObjects.ContainsKey(targetId))
                                pl = (Player)this.worldObjects[targetId];
                        }
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

        public void HandleSoundEvent(WorldObject sender, Sound soundEvent)
        {
            BroadcastEventArgs args = BroadcastEventArgs.CreateSoundAction(sender, soundEvent);
            Broadcast(args, true, Quadrant.All);
        }

        public void HandleParticleEffectEvent(WorldObject sender, PlayScript effect)
        {
            BroadcastEventArgs args = BroadcastEventArgs.CreateEffectAction(sender, effect);
            Broadcast(args, true, Quadrant.All);
        }

        public void SendChatMessage(WorldObject sender, ChatMessageArgs chatMessage)
        {
            // only players receive this
            List<Player> players = null;

            lock (objectCacheLocker)
            {
                players = this.worldObjects.Values.OfType<Player>().ToList();
            }

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

            lock (objectCacheLocker)
            {
                players = this.worldObjects.Values.OfType<Player>().ToList();
            }

            // filter to applicable players
            players = players.Where(p => p.Location?.IsInQuadrant(quadrant) ?? false).ToList();

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
                case BroadcastAction.PlaySound:
                    {
                        Parallel.ForEach(players, p => p.PlaySound(args.Sound, args.Sender.Guid));
                        break;
                    }
                case BroadcastAction.PlayParticleEffect:
                    {
                        Parallel.ForEach(players, p => p.PlayParticleEffect(args.Effect, args.Sender.Guid));
                        break;
                    }
            }

            // short circuit when there's no functional adjacency
            if (!propogate || wo?.Location?.LandblockId.MapScope != Enum.MapScope.Outdoors)
                return;

            if (propogate)
            {
                Log($"propogating broadcasting object {args.Sender.Guid.Full.ToString("X")} - {args.ActionType} to adjacencies");

                if (wo.Location.PositionX < adjacencyLoadRange)
                {
                    WestAdjacency?.Broadcast(args, false, Quadrant.NorthEast | Quadrant.SouthEast);

                    if (wo.Location.PositionY < adjacencyLoadRange)
                        SouthWestAdjacency?.Broadcast(args, false, Quadrant.NorthEast);

                    if (wo.Location.PositionY > (maxXY - adjacencyLoadRange))
                        NorthWestAdjacency?.Broadcast(args, false, Quadrant.SouthEast);
                }

                if (wo.Location.PositionY < adjacencyLoadRange)
                    SouthAdjacency?.Broadcast(args, false, Quadrant.NorthEast | Quadrant.NorthWest);

                if (wo.Location.PositionX > (maxXY - adjacencyLoadRange))
                {
                    EastAdjacency?.Broadcast(args, false, Quadrant.NorthWest | Quadrant.SouthWest);

                    if (wo.Location.PositionY < adjacencyLoadRange)
                        SouthEastAdjacency?.Broadcast(args, false, Quadrant.NorthWest);

                    if (wo.Location.PositionY > (maxXY - adjacencyLoadRange))
                        NorthEastAdjacency?.Broadcast(args, false, Quadrant.SouthWest);
                }

                if (wo.Location.PositionY > (maxXY - adjacencyLoadRange))
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
                // here we'd move server objects in motion (subject to landscape) and do physics collision detection

                // for now, we'll move players around
                List<MutableWorldObject> movedObjects = null;
                List<Player> players = null;

                lock (objectCacheLocker)
                {
                    movedObjects = this.worldObjects.Values.OfType<MutableWorldObject>().ToList();
                    players = this.worldObjects.Values.OfType<Player>().ToList();
                }

                movedObjects = movedObjects.Where(p => p.LastUpdatedTicks >= p.LastMovementBroadcastTicks).ToList();

                // flag them as updated now in order to reduce chance of missing an update
                movedObjects.ForEach(m => m.LastMovementBroadcastTicks = WorldManager.PortalYearTicks);

                if (this.id.MapScope == Enum.MapScope.Outdoors)
                {
                    // check to see if a player or other mutable object "roamed" to an adjacent landblock
                    var objectsToRelocate = movedObjects.Where(m => m.Location.LandblockId.IsAdjacentTo(this.id) && m.Location.LandblockId != this.id).ToList();

                    // so, these objects moved to an adjacent block.  they could have recalled to that block, died and bounced to a lifestone on that block, or
                    // just simply walked accross the border line.  in any case, we won't delete them, we'll just transfer them.  the trick, though, is to
                    // figure out how to treat it in adjacent landblocks.  if the player walks across the southern border, the north adjacency needs to remove
                    // them, but the south is actually getting them.  we need to avoid sending Delete+Create to clients that already know about it, though.

                    objectsToRelocate.ForEach(o => Log($"attempting to relocate object {o.Name} ({o.Guid.Full.ToString("X")})"));

                    // RelocateObject will put them in the right landblock
                    objectsToRelocate.ForEach(o => LandblockManager.RelocateObject(o));

                    // Remove has logic to make sure it doesn't double up the delete+create when "true" is passed.
                    objectsToRelocate.ForEach(o => RemoveWorldObject(o.Guid, true));
                }

                // broadcast
                Parallel.ForEach(movedObjects, mo =>
                {
                    if (mo.Location.LandblockId == this.id)
                    {
                        // update if it's still here
                        Broadcast(BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, mo), true, Quadrant.All);
                    }
                    else
                    {
                        // remove and readd if it's not
                        this.RemoveWorldObject(mo.Guid, false);
                        LandblockManager.AddObject(mo);
                    }
                });

                // TODO: figure out if this landblock can be unloaded

                // process player action queues
                foreach (Player p in players)
                {
                    QueuedGameAction action = p.ActionQueuePop();

                    if (action != null)
                        HandleGameAction(action, p);
                }

                Thread.Sleep(1);
            }

            // TODO: release resources
        }

        private void HandleGameAction(QueuedGameAction action, Player player)
        {
            switch (action.ActionType)
            {
                case GameActionType.ApplyVisualEffect:
                    {
                        var g = new ObjectGuid(action.ObjectId);
                        WorldObject obj = (WorldObject)player;
                        if (worldObjects.ContainsKey(g))
                        {
                            obj = worldObjects[g];
                        }
                        var particleEffect = (PlayScript)action.SecondaryObjectId;
                        HandleParticleEffectEvent(obj, particleEffect);
                        break;
                    }
                case GameActionType.ApplySoundEffect:
                    {
                        var g = new ObjectGuid(action.ObjectId);
                        WorldObject obj = (WorldObject)player;
                        if (worldObjects.ContainsKey(g))
                        {
                            obj = worldObjects[g];
                        }
                        var soundEffect = (Sound)action.SecondaryObjectId;
                        HandleSoundEvent(obj, soundEffect);
                        break;
                    }
                case GameActionType.QueryHealth:
                    {
                        object target = null;
                        var targetId = new ObjectGuid(action.ObjectId);

                        if (targetId.IsPlayer() || targetId.IsCreature())
                        {
                            if (this.worldObjects.ContainsKey(targetId))
                                target = this.worldObjects[targetId];

                            if (target == null)
                            {
                                // check adjacent landblocks for the targetId
                                foreach (var block in adjacencies)
                                {
                                    if (block.Value.worldObjects.ContainsKey(targetId))
                                        target = this.worldObjects[targetId];                                    
                                }
                            }
                            if (target != null)
                            {
                                float healthPercentage = 0;

                                if (targetId.IsPlayer())
                                {
                                    Player tmpTarget = (Player)target;
                                    healthPercentage = (float)tmpTarget.Health.Current / (float)tmpTarget.Health.MaxValue;
                                }
                                if (targetId.IsCreature())
                                {
                                    Creature tmpTarget = (Creature)target;
                                    healthPercentage = (float)tmpTarget.Health.Current / (float)tmpTarget.Health.MaxValue;
                                }
                                var updateHealth = new GameEventUpdateHealth(player.Session, targetId.Full, healthPercentage);
                                player.Session.Network.EnqueueSend(updateHealth);
                            }
                        }

                        break;
                    }
                case GameActionType.Use:
                    {
                        var g = new ObjectGuid(action.ObjectId);
                        if (worldObjects.ContainsKey(g))
                        {
                            WorldObject obj = worldObjects[g];

                            switch (obj.Type)
                            {
                                case Enum.ObjectType.LifeStone:
                                    {
                                        string serverMessage = null;
                                        // validate within use range
                                        float radiusSquared = obj.GameData.UseRadius * obj.GameData.UseRadius;

                                        if (player.Location.SquaredDistanceTo(obj.Location) >= radiusSquared)
                                        {
                                            serverMessage = "You wandered too far to attune with the lifestone!";
                                        }
                                        else
                                        {
                                            player.SetCharacterPosition(PositionType.Sanctuary, player.Location);

                                            // create the outbound server message
                                            serverMessage = "You have attuned your spirit to this lifestone. You will ressurect here after you die.";
                                        }

                                        var lifestoneBindMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.Advancement);
                                        // always send useDone event
                                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                                        player.Session.Network.EnqueueSend(lifestoneBindMessage, sendUseDoneEvent);

                                        break;
                                    }
                            }
                        }
                        break;
                    }
            }
        }

        private void Log(string message)
        {
            log.Debug($"LB {id.Landblock.ToString("X")}: {message}");
        }
    }
}
