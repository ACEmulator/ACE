using System;
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
using ACE.Network.Motion;
using ACE.Network.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Sequence;
using ACE.Factories;

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
            var factoryObjects = GenericObjectFactory.CreateWorldObjects(objects);
            factoryObjects.ForEach(fo => worldObjects.Add(fo.Guid, fo));

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
                allObjects = worldObjects.Values.ToList();
                if (!worldObjects.ContainsKey(wo.Guid))                                
                    worldObjects[wo.Guid] = wo;                
            }

            var args = BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, wo);
            Broadcast(args, true, Quadrant.All);

            // if this is a player, tell them about everything else we have.
            if (wo is Player)
            {
                // send them the initial burst of objects
                Log($"blasting player \"{(wo as Player).Name}\" with {allObjects.Count} objects.");
                Parallel.ForEach(allObjects, (o) =>
                {
                    if (o.Guid.IsCreature())
                    {
                        if ((o as Creature).IsAlive)
                            (wo as Player).TrackObject(o);
                    }
                    else
                    {
                        (wo as Player).TrackObject(o);
                    }
                });
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

        public void HandleMovementEvent(WorldObject sender, GeneralMotion motion)
        {
            BroadcastEventArgs args = BroadcastEventArgs.CreateMovementEvent(sender, motion);
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
                        Parallel.ForEach(players, p => p.StopTrackingObject(wo));
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
                case BroadcastAction.MovementEvent:
                    {
                        Parallel.ForEach(players, p => p.SendMovementEvent(args.Motion, args.Sender));
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
                List<WorldObject> movedObjects = null;
                List<Player> players = null;

                lock (objectCacheLocker)
                {
                    movedObjects = this.worldObjects.Values.OfType<WorldObject>().ToList();
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
                case GameActionType.PutItemInContainer:
                    {
                        var playerId = new ObjectGuid(action.ObjectId);
                        var inventoryId = new ObjectGuid(action.SecondaryObjectId);
                        if (playerId.IsPlayer())
                        {
                            Player aPlayer = null;
                            WorldObject inventoryItem = null;

                            if (worldObjects.ContainsKey(playerId) && worldObjects.ContainsKey(inventoryId))
                            {
                                aPlayer = (Player)worldObjects[playerId];
                                inventoryItem = worldObjects[inventoryId];                                
                            }

                            if ((aPlayer != null) && (inventoryItem != null))
                            {                                
                                var motion = new GeneralMotion(MotionStance.Standing);
                                motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;                                
                                aPlayer.Session.Network.EnqueueSend(new GameMessageUpdatePosition(aPlayer), 
                                    new GameMessageUpdateMotion(aPlayer, aPlayer.Session, motion),
                                    new GameMessageSound(aPlayer.Guid, Sound.PickUpItem, (float)1.0));
                                
                                // Add to the inventory list.
                                aPlayer.AddToInventory(inventoryItem);
                                LandblockManager.RemoveObject(inventoryItem);

                                motion = new GeneralMotion(MotionStance.Standing);
                                aPlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(aPlayer.Session,
                                       PropertyInt.EncumbVal,
                                       aPlayer.GameData.Burden), 
                                       new GameMessagePutObjectInContainer(aPlayer.Session, aPlayer, inventoryId),
                                       new GameMessageUpdateMotion(aPlayer, aPlayer.Session, motion),
                                       new GameMessageUpdateInstanceId(inventoryId, playerId),
                                       new GameMessagePickupEvent(aPlayer.Session, inventoryItem));

                                aPlayer.TrackObject(inventoryItem);
                            }
                        }
                        break;
                    }
                case GameActionType.DropItem:
                    {
                        var g = new ObjectGuid(action.ObjectId);
                        // ReSharper disable once InconsistentlySynchronizedField
                        if (worldObjects.ContainsKey(g))
                        {
                            var playerId = new ObjectGuid(action.ObjectId);
                            var inventoryId = new ObjectGuid(action.SecondaryObjectId);
                            if (playerId.IsPlayer())
                            {
                                Player aPlayer = null;
                                WorldObject inventoryItem = null;

                                if (worldObjects.ContainsKey(playerId))
                                {
                                    aPlayer = (Player)worldObjects[playerId];
                                    inventoryItem = aPlayer.GetInventoryItem(inventoryId);
                                    aPlayer.RemoveFromInventory(inventoryId);
                                }

                                if ((aPlayer != null) && (inventoryItem != null))
                                {
                                    var targetContainer = new ObjectGuid(0);
                                    aPlayer.Session.Network.EnqueueSend(
                                        new GameMessagePrivateUpdatePropertyInt(
                                            aPlayer.Session,
                                            PropertyInt.EncumbVal,
                                            (uint)aPlayer.Session.Player.GameData.Burden));

                                    var motion = new GeneralMotion(MotionStance.Standing);
                                    motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;
                                    aPlayer.Session.Network.EnqueueSend(
                                        new GameMessageUpdateMotion(aPlayer, aPlayer.Session, motion),
                                        new GameMessageUpdateInstanceId(inventoryId, targetContainer));

                                    motion = new GeneralMotion(MotionStance.Standing);
                                    aPlayer.Session.Network.EnqueueSend(
                                        new GameMessageUpdateMotion(aPlayer, aPlayer.Session, motion),
                                        new GameMessagePutObjectIn3d(aPlayer.Session, aPlayer, inventoryId),
                                        new GameMessageSound(aPlayer.Guid, Sound.DropItem, (float)1.0),
                                        new GameMessageUpdateInstanceId(inventoryId, targetContainer));

                                    // This is the sequence magic - adds back into 3d space seem to be treated like teleport.   
                                    inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                                    inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
                                    LandblockManager.AddObject(inventoryItem);
                                    aPlayer.Session.Network.EnqueueSend(new GameMessageUpdatePosition(inventoryItem));
                                }
                            }
                        }
                        break;
                    }
                case GameActionType.MovementEvent:
                    {
                        var g = new ObjectGuid(action.ObjectId);
                        WorldObject obj = (WorldObject)player;
                        if (worldObjects.ContainsKey(g))
                        {
                            obj = worldObjects[g];
                        }
                        var motion = action.Motion;
                        HandleMovementEvent(obj, motion);
                        break;
                    }
                case GameActionType.ObjectCreate:
                    {
                        this.AddWorldObject(action.WorldObject);
                        break;
                    }
                case GameActionType.ObjectDelete:
                    {
                        this.RemoveWorldObject(action.WorldObject.Guid, false);
                        break;
                    }
                case GameActionType.QueryHealth:
                    {
                        if (action.ObjectId == 0)
                        {
                            // Deselect the formerly selected Target
                            player.SelectedTarget = 0;
                            break;
                        }

                        object target = null;
                        var targetId = new ObjectGuid(action.ObjectId);

                        // Remember the selected Target
                        player.SelectedTarget = action.ObjectId;

                        // TODO: once items are implemented check if there are items that can trigger
                        //       the QueryHealth event. So far I believe it only gets triggered for players and creatures
                        if (targetId.IsPlayer() || targetId.IsCreature())
                        {
                            if (this.worldObjects.ContainsKey(targetId))
                                target = this.worldObjects[targetId];

                            if (target == null)
                            {
                                // check adjacent landblocks for the targetId
                                foreach (var block in adjacencies)
                                {
                                    if (block.Value != null)
                                        if (block.Value.worldObjects.ContainsKey(targetId))
                                            target = block.Value.worldObjects[targetId];
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
                                case Enum.ObjectType.Portal:
                                    {
                                        // validate within use range :: set to a fixed value as static Portals are normally OnCollide usage
                                        float rangeCheck = 5.0f;

                                        if (player.Location.SquaredDistanceTo(obj.Location) < rangeCheck)
                                        {
                                            PortalDestination portalDestination = DatabaseManager.World.GetPortalDestination(obj.WeenieClassid);

                                            if (portalDestination != null)
                                            {
                                                player.Session.Player.Teleport(portalDestination.Position);
                                                // always send useDone event
                                                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                                                player.Session.Network.EnqueueSend(sendUseDoneEvent);
                                            }
                                            else
                                            {
                                                string serverMessage = "Portal destination for portal ID " + obj.WeenieClassid + " not yet implemented!";
                                                var usePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                                                // always send useDone event
                                                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                                                player.Session.Network.EnqueueSend(usePortalMessage, sendUseDoneEvent);
                                            }
                                        }
                                        else
                                        {
                                            // always send useDone event
                                            var sendUseDoneEvent = new GameEventUseDone(player.Session);
                                            player.Session.Network.EnqueueSend(sendUseDoneEvent);
                                        }

                                        break;
                                    }
                                case Enum.ObjectType.LifeStone:
                                    {
                                        string serverMessage = null;
                                        // validate within use range
                                        float radiusSquared = obj.GameData.UseRadius * obj.GameData.UseRadius;

                                        var motionSanctuary = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.Sanctuary));

                                        var animationEvent = new GameMessageUpdateMotion(player, player.Session, motionSanctuary);

                                        // This event was present for a pcap in the training dungeon.. Why? The sound comes with animationEvent...
                                        var soundEvent = new GameMessageSound(obj.Guid, Sound.LifestoneOn, 1);

                                        if (player.Location.SquaredDistanceTo(obj.Location) >= radiusSquared)
                                        {
                                            serverMessage = "You wandered too far to attune with the Lifestone!";
                                        }
                                        else
                                        {
                                            player.SetCharacterPosition(PositionType.Sanctuary, player.Location);

                                            // create the outbound server message
                                            serverMessage = "You have attuned your spirit to this Lifestone. You will resurrect here after you die.";
                                            player.EnqueueMovementEvent(motionSanctuary, player.Guid);
                                            player.Session.Network.EnqueueSend(soundEvent);
                                        }

                                        var lifestoneBindMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.Magic);
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
