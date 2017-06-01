using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ACE.Entity.Events;
using ACE.Managers;
using log4net;
using ACE.Database;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.GameAction;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Network.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Sequence;
using ACE.Factories;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    /// <summary>
    /// the gist of a landblock is that, generally, everything on it publishes
    /// to and subscribes to everything else in the landblock.  x/y in an outdoor
    /// landblock goes from 0 to 192.  "indoor" (dungeon) landblocks have no
    /// functional limit as players can't freely roam in/out of them
    /// </summary>
    public class Landblock
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const float adjacencyLoadRange = 96f;
        private const float outDoorChatRange = 75f;
        private const float indoorChatRange = 25f;
        private const float maxXY = 192f;
        private const float maxobjectRange = 20000;
        private const float maxobjectGhostRange = 40000;

        private LandblockId id;

        private readonly object objectCacheLocker = new object();
        private readonly Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();
        private readonly Dictionary<Adjacency, Landblock> adjacencies = new Dictionary<Adjacency, Landblock>();

        // Can be appeneded concurrently, will be sent serially
        // NOTE: Broadcasts have read-only access to landblocks, and EnqueueSend is thread-safe within Session.
        //    -- Therefore broadcasting between landblocks doesn't require locking O_o
        private readonly ConcurrentQueue<Tuple<Position, float, GameMessage>> broadcastQueue = new ConcurrentQueue<Tuple<Position, float, GameMessage>>();

        private ConcurrentQueue<Action> actionQueue = new ConcurrentQueue<Action>();

        // private byte cellGridMaxX = 8; // todo: load from cell.dat
        // private byte cellGridMaxY = 8; // todo: load from cell.dat

        // not sure if a full object is necessary here.  I don't think a Landcell has any
        // inherent functionality that needs to be modelled in an object.
        // private Landcell[,] cellGrid; // todo: load from cell.dat

        public LandBlockStatus Status = new LandBlockStatus();

        public LandblockId Id
        {
            get { return id; }
        }

        public Landblock(LandblockId id)
        {
            this.id = id;

            UpdateStatus(LandBlockStatusFlag.IdleUnloaded);

            // initialize adjacency array
            adjacencies.Add(Adjacency.North, null);
            adjacencies.Add(Adjacency.NorthEast, null);
            adjacencies.Add(Adjacency.East, null);
            adjacencies.Add(Adjacency.SouthEast, null);
            adjacencies.Add(Adjacency.South, null);
            adjacencies.Add(Adjacency.SouthWest, null);
            adjacencies.Add(Adjacency.West, null);
            adjacencies.Add(Adjacency.NorthWest, null);

            UpdateStatus(LandBlockStatusFlag.IdleLoading);

            // TODO: Load cell.dat contents
            //   1. landblock cell structure
            //   2. terrain data
            // TODO: Load portal.dat contents (as/if needed)
            // TODO: Load spawn data

            // TODO: load objects from world database such as lifestones, doors, player corpses, NPCs, Vendors
            // TODO: Object Overhaul Og II uncomment once working
            var objects = DatabaseManager.World.GetObjectsByLandblock(this.id.Landblock);
            if (objects != null)
            {
                var factoryObjects = GenericObjectFactory.CreateWorldObjects(objects);
                factoryObjects.ForEach(fo => worldObjects.Add(fo.Guid, fo));
            }
            // Load static creature spawns from DB
            // var creatures = DatabaseManager.World.GetCreaturesByLandblock(this.id.Landblock);
            // foreach (var c in creatures)
            // {
            //    Creature cwo = new Creature(c);
            //    worldObjects.Add(cwo.Guid, cwo);
            // }

            //// Load generator creature spawns from DB
            // var creatureGenerators = DatabaseManager.World.GetCreatureGeneratorsByLandblock(this.id.Landblock);
            // foreach (var cg in creatureGenerators)
            // {
            //    List<Creature> creatureList = MonsterFactory.SpawnCreaturesFromGenerator(cg);
            //    foreach (var c in creatureList)
            //    {
            //        worldObjects.Add(c.Guid, c);
            //    }
            // }

            UpdateStatus(LandBlockStatusFlag.IdleLoaded);
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

        private void AddPlayerTracking(List<WorldObject> wolist, Player player)
        {
            Parallel.ForEach(wolist, (o) =>
            {
                if (o.Guid.IsCreature())
                {
                    if ((o as Creature).IsAlive)
                        player.TrackObject(o);
                }
                else
                {
                    player.TrackObject(o);
                }
            });
        }

        private void RemovePlayerTracking(List<ObjectGuid> wolist, Player player)
        {
            Parallel.ForEach(wolist, (o) =>
            {
                // Find What Landblock Controls this object because we have to ref it
                // to find its sequance id..
                List<WorldObject> foundwo = new List<WorldObject>();
                foundwo = GetWorldObjectsByGuid(o, true);
                Parallel.ForEach(foundwo, (f) =>
                {
                    player.StopTrackingObject(f, false);
                });
            });
        }

        public void AddWorldObject(WorldObject wo)
        {
            Log($"adding {wo.Guid.Full:X}");

            lock (objectCacheLocker)
            {
                if (!worldObjects.ContainsKey(wo.Guid))
                    worldObjects[wo.Guid] = wo;
            }

            var args = BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, wo);
            Broadcast(args, true, Quadrant.All);

            // if this is a player, tell them about everything else we have in range of them.
            if (wo is Player)
            {
                List<WorldObject> wolist = null;
                wolist = GetWorldObjectsInRange(wo, maxobjectRange, true);
                AddPlayerTracking(wolist, ((Player)wo));
            }
        }

        public void RemoveWorldObject(ObjectGuid objectId, bool adjacencyMove)
        {
            WorldObject wo = null;

            Log($"removing {objectId.Full:X}");

            lock (objectCacheLocker)
            {
                if (worldObjects.ContainsKey(objectId))
                {
                    wo = worldObjects[objectId];
                    if (!objectId.IsCreature())
                        worldObjects.Remove(objectId);
                }
            }

            // suppress broadcasting when it's just an adjacency move.  clients will naturally just stop
            // tracking stuff if they're too far, or the new landblock will broadcast to them if they're
            // close enough.
            if (!adjacencyMove && id.MapScope == Enum.MapScope.Outdoors && wo != null)
            {
                var args = BroadcastEventArgs.CreateAction(BroadcastAction.Delete, wo);
                Broadcast(args, true, Quadrant.All);
            }
        }

        public List<WorldObject> GetWorldObjectsInRange(WorldObject wo, float maxrange, bool neighbors)
        {
            List<WorldObject> allworldobj;

            lock (objectCacheLocker)
            {
                allworldobj = worldObjects.Values.ToList();
            }
            allworldobj = allworldobj.Where(o => o.Location.SquaredDistanceTo(wo.Location) < maxrange).ToList();

            if (neighbors)
            {
                foreach (var block in adjacencies)
                {
                    if (block.Value != null)
                    {
                        List<WorldObject> wol = null;
                        wol = block.Value.GetWorldObjectsInRange(wo, maxrange, false);
                        allworldobj.AddRange(wol);
                    }
                }
            }
            return allworldobj;
        }

        public List<WorldObject> GetWorldObjectsByGuid(ObjectGuid objectguid, bool neighbors)
        {
            List<WorldObject> allworldobj;
            lock (objectCacheLocker)
            {
                allworldobj = worldObjects.Values.ToList();
            }
            allworldobj = allworldobj.Where(o => o.Guid == objectguid).ToList();

            // todo: verify if a object can only exist on one landblock at a time..
            // if so then we can stop right here, we found it, for now we will resume and assume the worst.
            // a object can be on any landblock around you.

            if (neighbors)
            {
                foreach (var block in adjacencies)
                {
                    if (block.Value != null)
                    {
                        var wol = block.Value.GetWorldObjectsByGuid(objectguid, false);
                        allworldobj.AddRange(wol);
                    }
                }
            }
            return allworldobj;
        }

        public WorldObject GetWorldObject(ObjectGuid objectId)
        {
            Log($"Getting WorldObject {objectId.Full:X}");

            lock (objectCacheLocker)
            {
                return worldObjects.ContainsKey(objectId) ? worldObjects[objectId] : null;
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

        public void HandleMovementEvent(WorldObject sender, UniversalMotion motion)
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
                players = worldObjects.Values.OfType<Player>().ToList();
            }

            BroadcastEventArgs args = BroadcastEventArgs.CreateChatAction(sender, chatMessage);
            Broadcast(args, true, Quadrant.All);
        }

        public void HandleDeathMessage(WorldObject sender, DeathMessageArgs deathMessageArgs)
        {
            BroadcastEventArgs args = BroadcastEventArgs.CreateDeathMessage(sender, deathMessageArgs);
            Broadcast(args, false, Quadrant.All);
        }

        /// <summary>
        /// handles broadcasting an event to the players in this landblock and to the proper adjacencies
        /// </summary>
        private void Broadcast(BroadcastEventArgs args, bool propagate, Quadrant quadrant)
        {
            WorldObject wo = args.Sender;
            List<Player> players = null;

            Log($"broadcasting object {args.Sender.Guid.Full.ToString("X")} - {args.ActionType}");

            lock (objectCacheLocker)
            {
                players = worldObjects.Values.OfType<Player>().ToList();
            }

            switch (args.ActionType)
            {
                case BroadcastAction.Delete:
                    {
                        players = players.Where(p => p.Location?.IsInQuadrant(quadrant) ?? false).ToList();
                        Parallel.ForEach(players, p => p.StopTrackingObject(wo, true));
                        break;
                    }
                case BroadcastAction.AddOrUpdate:
                    {
                        // supresss updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, maxobjectRange, true).OfType<Player>().ToList();
                        // players never need an update of themselves
                        players = players.Where(p => p.Guid != args.Sender.Guid).ToList();

                        Parallel.ForEach(players, p => p.TrackObject(wo));
                        break;
                    }
                case BroadcastAction.LocalChat:
                    {
                        // supresss updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, maxobjectRange, true).OfType<Player>().ToList();
                        Parallel.ForEach(players, p => p.ReceiveChat(wo, args.ChatMessage));
                        break;
                    }
                case BroadcastAction.PlaySound:
                    {
                        // supresss updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, maxobjectRange, true).OfType<Player>().ToList();
                        Parallel.ForEach(players, p => p.PlaySound(args.Sound, args.Sender.Guid));
                        break;
                    }
                case BroadcastAction.PlayParticleEffect:
                    {
                        // supresss updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, maxobjectRange, true).OfType<Player>().ToList();
                        Parallel.ForEach(players, p => p.PlayParticleEffect(args.Effect, args.Sender.Guid));
                        break;
                    }
                case BroadcastAction.MovementEvent:
                    {
                        // suppress updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, maxobjectRange, true).OfType<Player>().ToList();
                        Parallel.ForEach(players, p => p.SendMovementEvent(args.Motion, args.Sender));
                        break;
                    }
                case BroadcastAction.BroadcastDeath:
                    {
                        // players never need an update of themselves
                        // TODO: Filter to players in range and include adjacencies
                        players = players.Where(p => p.Guid != args.Sender.Guid).ToList();
                        Parallel.ForEach(players, p => p.BroadcastPlayerDeath(args.DeathMessage.Message, args.DeathMessage.Victim, args.DeathMessage.Killer));
                        break;
                    }
            }
        }

        /// <summary>
        /// main game loop
        /// </summary>
        public void UseTime(double tickTime)
        {
            // here we'd move server objects in motion (subject to landscape) and do physics collision detection

            List<Player> allplayers = null;
            List<Creature> allcreatures = null;
            List<WorldObject> despawnObjects = null;
            List<Creature> deadCreatures = null;
            List<WorldObject> movedObjects = null;

            var allworldobj = worldObjects.Values;

            // all players on this land block
            allplayers = allworldobj.OfType<Player>().ToList();
            allcreatures = allworldobj.OfType<Creature>().ToList();

            despawnObjects = allworldobj.ToList();
            despawnObjects = despawnObjects.Where(x => x.DespawnTime > -1).ToList();

            deadCreatures = allworldobj.OfType<Creature>().ToList();
            deadCreatures = deadCreatures.Where(x => x.IsAlive == false).ToList();

            movedObjects = allworldobj.Where(p => p.LastUpdatedTicks >= p.LastMovementBroadcastTicks).ToList();
            movedObjects.ForEach(m => m.LastMovementBroadcastTicks = WorldManager.PortalYearTicks);

            // Run the enqueued actions
            while (!actionQueue.IsEmpty)
            {
                Action actn;
                bool success = actionQueue.TryDequeue(out actn);
                if (!success)
                {
                    break;
                }

                actn();
            }

            // FIXME(ddevec): Players are objects -- "Actors" should call "Act" to do all needed updates
            // for all players on landblock.
            Parallel.ForEach(allplayers, player =>
            {
                // Process Action Queue for player.
                QueuedGameAction action = player.ActionQueuePop();
                if (action != null)
                    HandleGameAction(action, player);

                // Process Examination Queue for player
                QueuedGameAction examination = player.ExaminationQueuePop();
                if (examination != null)
                    HandleGameAction(examination, player);
            });
            UpdateStatus(allplayers.Count);

            // FIXME(ddevec): Objects should be "Actors" should call "Act" to do all needed updates
            // per-creature update on landblock.
            Parallel.ForEach(allworldobj, wo =>
            {
                // Process the creatures
                wo.Tick(tickTime);
            });

            // FIXME(ddevec): Objects should be "Actors" should call "Act" to do all needed updates
            Parallel.ForEach(movedObjects, mo =>
            {
                // detect all world objects in ghost range
                List<WorldObject> woproxghost = new List<WorldObject>();
                woproxghost.AddRange(GetWorldObjectsInRange(mo, maxobjectGhostRange, true));

                // for all objects in rang of this moving object or in ghost range of moving object update them.
                Parallel.ForEach(woproxghost, wo =>
                {
                    if (mo.Guid.IsPlayer())
                    {
                        // if world object is in active zone then.
                        if (wo.Location.SquaredDistanceTo(mo.Location) <= maxobjectRange)
                        {
                            // if world object is in active zone.
                            if (!(mo as Player).GetTrackedObjectGuids().Contains(wo.Guid))
                                (mo as Player).TrackObject(wo);
                        }
                        // if world object is in ghost zone and outside of active zone
                        else
                            if ((mo as Player).GetTrackedObjectGuids().Contains(wo.Guid))
                            (mo as Player).StopTrackingObject(wo, true);
                    }
                });

                if (mo.Location.LandblockId == id)
                {
                    // update if it's still here
                    Broadcast(BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, mo), true, Quadrant.All);
                }
                else
                {
                    // remove and read if it's not
                    RemoveWorldObject(mo.Guid, false);
                    LandblockManager.AddObject(mo);
                }
            });

            // despawn objects
            despawnObjects.ForEach(deo =>
            {
                if (deo.DespawnTime < WorldManager.PortalYearTicks)
                {
                    RemoveWorldObject(deo.Guid, false);
                }
            });

            // respawn creatures
            deadCreatures.ForEach(dc =>
            {
                if (dc.RespawnTime < WorldManager.PortalYearTicks)
                {
                    dc.IsAlive = true;
                    // HandleParticleEffectEvent(dc, PlayScript.Create);
                    AddWorldObject(dc);
                }
            });

            // Handle broadcasts
            SendBroadcasts();
        }

        private void HandleGameAction(QueuedGameAction action, Player player)
        {
            switch (action.ActionType)
            {
                case GameActionType.TalkDirect:
                    {
                        // TODO: remove this hack (using TalkDirect) ASAP
                        var g = new ObjectGuid(action.ObjectId);
                        WorldObject obj = (WorldObject)player;
                        if (worldObjects.ContainsKey(g))
                        {
                            obj = worldObjects[g];
                        }
                        DeathMessageArgs d = new DeathMessageArgs(action.ActionBroadcastMessage, new ObjectGuid(action.ObjectId), new ObjectGuid(action.SecondaryObjectId));
                        HandleDeathMessage(obj, d);
                        break;
                    }
                case GameActionType.TeleToHouse:
                case GameActionType.TeleToLifestone:
                case GameActionType.TeleToMansion:
                case GameActionType.TeleToMarketPlace:
                case GameActionType.TeleToPkArena:
                case GameActionType.TeleToPklArena:
                    {
                        player.Teleport(action.ActionLocation);
                        break;
                    }
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
                case GameActionType.IdentifyObject:
                    {
                        // TODO: Throttle this request. The live servers did this, likely for a very good reason, so we should, too.
                        var g = new ObjectGuid(action.ObjectId);
                        WorldObject obj = (WorldObject)player;
                        if (worldObjects.ContainsKey(g))
                        {
                            obj = worldObjects[g];
                        }
                        var identifyResponse = new GameEventIdentifyObjectResponse(player.Session, action.ObjectId, obj);
                        player.Session.Network.EnqueueSend(identifyResponse);
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
                                if (aPlayer.PhysicsData.Position.SquaredDistanceTo(inventoryItem.PhysicsData.Position)
                                    > Math.Pow((inventoryItem.UseRadius ?? 0.00), 2))
                                {
                                    // This is where I need to hook in the move to object code.
                                    // TODO: Og II work on this soon.
                                }
                                var motion = new UniversalMotion(MotionStance.Standing);
                                motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;
                                aPlayer.Session.Network.EnqueueSend(new GameMessageUpdatePosition(aPlayer),
                                    new GameMessageUpdateMotion(aPlayer, aPlayer.Session, motion),
                                    new GameMessageSound(aPlayer.Guid, Sound.PickUpItem, (float)1.0));

                                // Add to the inventory list.
                                LandblockManager.RemoveObject(inventoryItem);
                                aPlayer.AddToInventory(inventoryItem);

                                motion = new UniversalMotion(MotionStance.Standing);
                                aPlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(aPlayer.Session,
                                       PropertyInt.EncumbranceVal,
                                       (aPlayer.Burden ?? 0)),
                                       new GameMessagePutObjectInContainer(aPlayer.Session, aPlayer, inventoryId),
                                       new GameMessageUpdateMotion(aPlayer, aPlayer.Session, motion),
                                       new GameMessageUpdateInstanceId(inventoryId, playerId),
                                       new GameMessagePickupEvent(aPlayer.Session, inventoryItem));

                                aPlayer.TrackObject(inventoryItem);
                                // This may not be needed when we fix landblock update object -
                                // TODO: Og II - check this later to see if it is still required.
                                // aPlayer.Session.Network.EnqueueSend(new GameMessageUpdateObject(inventoryItem));
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
                                            PropertyInt.EncumbranceVal,
                                            (uint)aPlayer.Session.Player.Burden));

                                    var motion = new UniversalMotion(MotionStance.Standing);
                                    motion.MovementData.ForwardCommand = (ushort)MotionCommand.Pickup;
                                    aPlayer.Session.Network.EnqueueSend(
                                        new GameMessageUpdateMotion(aPlayer, aPlayer.Session, motion),
                                        new GameMessageUpdateInstanceId(inventoryId, targetContainer));

                                    motion = new UniversalMotion(MotionStance.Standing);
                                    aPlayer.Session.Network.EnqueueSend(
                                        new GameMessageUpdateMotion(aPlayer, aPlayer.Session, motion),
                                        new GameMessagePutObjectIn3d(aPlayer.Session, aPlayer, inventoryId),
                                        new GameMessageSound(aPlayer.Guid, Sound.DropItem, (float)1.0),
                                        new GameMessageUpdateInstanceId(inventoryId, targetContainer));

                                    // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                                    inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                                    inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
                                    LandblockManager.AddObject(inventoryItem);

                                    // This may not be needed when we fix landblock update object -
                                    // TODO: Og II - check this later to see if it is still required.
                                    aPlayer.Session.Network.EnqueueSend(new GameMessageUpdateObject(inventoryItem));

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
                        AddWorldObject(action.WorldObject);
                        break;
                    }
                case GameActionType.ObjectDelete:
                    {
                        RemoveWorldObject(action.WorldObject.Guid, false);
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
                            if (worldObjects.ContainsKey(targetId))
                                target = worldObjects[targetId];

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

                            if ((obj.DescriptionFlags & ObjectDescriptionFlag.LifeStone) != 0)
                                (obj as Lifestone).OnUse(player);
                            else if ((obj.DescriptionFlags & ObjectDescriptionFlag.Portal) != 0)
                                // TODO: When Physics collisions are implemented, this logic should be switched there, as normal portals are not onUse.
                                (obj as Portal).OnCollide(player);
                            else if ((obj.DescriptionFlags & ObjectDescriptionFlag.Door) != 0)
                                (obj as Door).OnUse(player);

                            // switch (obj.Type)
                            // {
                            //    case Enum.ObjectType.Portal:
                            //        {
                            //            // TODO: When Physics collisions are implemented, this logic should be switched there, as normal portals are not onUse.
                            //
                            //            (obj as Portal).OnCollide(player);
                            //
                            //            break;
                            //        }
                            //    case Enum.ObjectType.LifeStone:
                            //        {
                            //            (obj as Lifestone).OnUse(player);
                            //            break;
                            //        }
                            // }
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Intended only for use in physics.
        /// TBD: Actual interface for this-- this is just a filler for now
        /// </summary>
        public IEnumerable<WorldObject> GetPhysicsWorldObjects()
        {
            return worldObjects.Values;
        }

        private void UpdateStatus(LandBlockStatusFlag flag)
        {
            Status.LandBlockStatusFlag = flag;
            Diagnostics.Diagnostics.SetLandBlockKey(id.LandblockX, id.LandblockY, Status);
        }

        private void UpdateStatus(int pcount)
        {
            Status.PlayerCount = pcount;
            if (pcount > 0)
            {
                Status.LandBlockStatusFlag = LandBlockStatusFlag.InUseLow;
                Diagnostics.Diagnostics.SetLandBlockKey(id.LandblockX, id.LandblockY, Status);
            }
            else
            {
                Status.LandBlockStatusFlag = LandBlockStatusFlag.IdleLoaded;
                UpdateStatus(Status.LandBlockStatusFlag);
            }
        }

        private List<Landblock> GetLandblocksInRange(Position pos, float distance)
        {
            List<Landblock> inRange = new List<Landblock>();

            inRange.Add(this);

            float highX = pos.PositionX + distance;
            float lowX = pos.PositionX - distance;
            float highY = pos.PositionY + distance;
            float lowY = pos.PositionY - distance;
            // Check East
            if (Position.XYToLandblock(highX, pos.PositionY) != Id)
            {
                inRange.Add(EastAdjacency);
            }
            // North East
            if (Position.XYToLandblock(highX, highY) != Id)
            {
                inRange.Add(NorthEastAdjacency);
            }

            // North 
            if (Position.XYToLandblock(pos.PositionX, highY) != Id)
            {
                inRange.Add(NorthAdjacency);
            }

            // North West
            if (Position.XYToLandblock(lowX, highY) != Id)
            {
                inRange.Add(NorthWestAdjacency);
            }

            // West
            if (Position.XYToLandblock(lowX, pos.PositionY) != Id)
            {
                inRange.Add(WestAdjacency);
            }

            // South West
            if (Position.XYToLandblock(lowX, lowY) != Id)
            {
                inRange.Add(SouthWestAdjacency);
            }

            // South
            if (Position.XYToLandblock(pos.PositionX, lowY) != Id)
            {
                inRange.Add(SouthAdjacency);
            }

            // South East
            if (Position.XYToLandblock(highX, lowY) != Id)
            {
                inRange.Add(SouthEastAdjacency);
            }

            return inRange;
        }

        /// <summary>
        /// Enqueues a message for broadcast, thread safe -- but cannot happen in parallel with 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <param name="msg"></param>
        public void EnqueueBroadcast(Position pos, float distance, GameMessage msg)
        {
            broadcastQueue.Enqueue(new Tuple<Position, float, GameMessage>(pos, distance, msg));
        }

        /// <summary>
        /// NOTE: Cannot be sent while objects are moving! depends on object positions not changing, and objects not moving between landblocks
        ///   Last method called from Landblock::Act()  (FIXME(ddevec) UseTime instead of Act() for now)
        /// </summary>
        private void SendBroadcasts()
        {
            while (!broadcastQueue.IsEmpty)
            {
                Tuple<Position, float, GameMessage> tuple;
                bool success = broadcastQueue.TryDequeue(out tuple);
                if (!success)
                {
                    log.Error("Unexpected TryDequeue Failure!");
                    break;
                }
                Position pos = tuple.Item1;
                float distance = tuple.Item2;
                GameMessage msg = tuple.Item3;

                // NOTE: Doesn't need locking -- players cannot change while in "Act" SendBroadcasts is the last thing done in Act
                // foreach player within range, do send

                List<Landblock> landblocksInRange = GetLandblocksInRange(pos, distance);
                foreach (Landblock lb in landblocksInRange)
                {
                    List<Player> allPlayers = lb.worldObjects.Values.OfType<Player>().ToList();
                    foreach (Player p in allPlayers)
                    {
                        if (p.PhysicsData.Position.SquaredDistanceTo(pos) < distance * distance)
                        {
                            p.Session.Network.EnqueueSend(msg);
                        }
                    }
                }
            }
        }

        public void EnqueueAction(Action actn)
        {
            actionQueue.Enqueue(actn);
        }

        // FIXME(ddevec):  Build action system...
        public void EnqueueActionBroadcast(Action<Player> delegateAction) {
        }

        private void Log(string message)
        {
            log.Debug($"LB {id.Landblock.ToString("X")}: {message}");
        }
    }
}
