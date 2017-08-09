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
using ACE.Network.Sequence;
using ACE.Network.GameAction;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Network.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Factories;
using ACE.Entity.Enum;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Actions;

namespace ACE.Entity
{
    /// <summary>
    /// the gist of a landblock is that, generally, everything on it publishes
    /// to and subscribes to everything else in the landblock.  x/y in an outdoor
    /// landblock goes from 0 to 192.  "indoor" (dungeon) landblocks have no
    /// functional limit as players can't freely roam in/out of them
    /// </summary>
    public class Landblock : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static float AdjacencyLoadRange { get; } = 96f;
        public static float OutdoorChatRange { get; } = 75f;
        public static float IndoorChatRange { get; } = 25f;
        public static float MaxXY { get; } = 192f;
        public static float MaxObjectRange { get; } = 192f;
        public static float MaxObjectGhostRange { get; } = 250f;

        private LandblockId id;

        private readonly Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();
        private readonly Dictionary<Adjacency, Landblock> adjacencies = new Dictionary<Adjacency, Landblock>();

        /// <summary>
        /// Special needs-broadcast flag.  Cleared in broadcast, but set in other phases.
        /// Must be treated exceptionally carefully to avoid races.  Don't touch unless you /really/ know what your doing
        /// </summary>
        private int broadcastQueued = 0;

        // Can be appeneded concurrently, will be sent serially
        // NOTE: Broadcasts have read-only access to landblocks, and EnqueueSend is thread-safe within Session.
        //    -- Therefore broadcasting between landblocks doesn't require locking O_o
        private readonly ConcurrentQueue<Tuple<Position, float, GameMessage>> broadcastQueue = new ConcurrentQueue<Tuple<Position, float, GameMessage>>();

        // private byte cellGridMaxX = 8; // todo: load from cell.dat
        // private byte cellGridMaxY = 8; // todo: load from cell.dat

        // not sure if a full object is necessary here.  I don't think a Landcell has any
        // inherent functionality that needs to be modelled in an object.
        // private Landcell[,] cellGrid; // todo: load from cell.dat

        public LandBlockStatus Status = new LandBlockStatus();

        private NestedActionQueue actionQueue;
        private NestedActionQueue motionQueue;

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

            actionQueue = new NestedActionQueue(WorldManager.ActionQueue);

            // TODO: Load cell.dat contents
            //   1. landblock cell structure
            //   2. terrain data
            // TODO: Load portal.dat contents (as/if needed)
            // TODO: Load spawn data

            var objects = DatabaseManager.World.GetObjectsByLandblock(this.id.Landblock);
            var factoryObjects = WorldObjectFactory.CreateWorldObjects(objects);
            factoryObjects.ForEach(fo =>
            {
                if (!worldObjects.ContainsKey(fo.Guid))
                {
                    worldObjects.Add(fo.Guid, fo);
                    fo.SetParent(this);
                }
            });

            UpdateStatus(LandBlockStatusFlag.IdleLoaded);

            // FIXME(ddevec): Goal: get rid of UseTime() function...
            actionQueue.EnqueueAction(new ActionEventDelegate(() => UseTimeWrapper()));

            motionQueue = new NestedActionQueue(WorldManager.MotionQueue);
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

        public void AddWorldObject(WorldObject wo)
        {
            EnqueueAction(new ActionEventDelegate(() => AddWorldObjectInternal(wo)));
        }

        public ActionChain GetAddWorldObjectChain(WorldObject wo, Player noBroadcast = null)
        {
            return new Actions.ActionChain(this, () => AddWorldObjectInternal(wo));
        }

        public void AddWorldObjectForPhysics(WorldObject wo)
        {
            AddWorldObjectInternal(wo);
        }

        private void AddWorldObjectInternal(WorldObject wo)
        {
            Log($"adding {wo.Guid.Full.ToString("X")}");

            if (!worldObjects.ContainsKey(wo.Guid))
                worldObjects[wo.Guid] = wo;

            wo.SetParent(this);

            // var args = BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, wo);
            // Broadcast(args, true, Quadrant.All);
            // Alert all nearby players of the object
            EnqueueActionBroadcast(wo.Location, MaxObjectRange, (Player p) => p.TrackObject(wo));

            // if this is a player, tell them about everything else we have in range of them.
            if (wo is Player)
            {
                List<WorldObject> wolist = null;
                wolist = GetWorldObjectsInRange(wo, MaxObjectRange);
                AddPlayerTracking(wolist, (wo as Player));
            }
        }

        public void RemoveWorldObject(ObjectGuid objectId, bool adjacencyMove)
        {
            ActionChain removeChain = GetRemoveWorldObjectChain(objectId, adjacencyMove);
            if (removeChain != null)
            {
                removeChain.EnqueueChain();
            }
        }

        public ActionChain GetRemoveWorldObjectChain(ObjectGuid objectId, bool adjacencyMove)
        {
            Landblock owner = GetOwner(objectId);
            if (owner != null)
            {
                ActionChain chain = new ActionChain(owner, new ActionEventDelegate(() => RemoveWorldObjectInternal(objectId, adjacencyMove)));
                return chain;
            }
            return null;
        }

        /// <summary>
        /// Should only be called by physics/relocation engines -- not from player
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="adjacencyMove"></param>
        public void RemoveWorldObjectForPhysics(ObjectGuid objectId, bool adjacencyMove)
        {
            RemoveWorldObjectInternal(objectId, adjacencyMove);
        }

        private void RemoveWorldObjectInternal(ObjectGuid objectId, bool adjacencyMove)
        {
            WorldObject wo = null;

            Log($"removing {objectId.Full:X}");

            if (worldObjects.ContainsKey(objectId))
            {
                wo = worldObjects[objectId];
                if (!objectId.IsCreature())
                    worldObjects.Remove(objectId);
            }

            // XXX(ddevec): Should this null check be needed?
            if (wo != null)
            {
                wo.SetParent(null);
            }

            // suppress broadcasting when it's just an adjacency move.  clients will naturally just stop
            // tracking stuff if they're too far, or the new landblock will broadcast to them if they're
            // close enough.
            if (!adjacencyMove && id.MapScope == Enum.MapScope.Outdoors && wo != null)
            {
                /*
                var args = BroadcastEventArgs.CreateAction(BroadcastAction.Delete, wo);
                Broadcast(args, true, Quadrant.All);
                */
                EnqueueActionBroadcast(wo.Location, MaxObjectRange, (Player p) => p.StopTrackingObject(wo, true));
            }
        }

        /// <summary>
        /// Check to see if we are close enough to interact.   Adds a fudge factor of 1.5f
        /// </summary>
        /// <param name="playerGuid"></param>
        /// <param name="targetGuid"></param>
        /// <param name="arrivedRadiusSquared"></param>
        /// <param name="arrivedRadiusSquared"></param>
        /// <returns></returns>
        public bool WithinUseRadius(ObjectGuid playerGuid, ObjectGuid targetGuid, out float arrivedRadiusSquared, out bool validGuids)
        {
            var playerPosition = GetWorldObjectPosition(playerGuid);
            var targetPosition = GetWorldObjectPosition(targetGuid);
            if (playerPosition != null && targetPosition != null)
            {
                validGuids = true;
                arrivedRadiusSquared = GetWorldObjectEffectiveUseRadius(targetGuid);
                return (playerPosition.SquaredDistanceTo(targetPosition) <= arrivedRadiusSquared);
            }
            arrivedRadiusSquared = 0.00f;
            validGuids = false;
            return false;
        }

        private Position GetWorldObjectPosition(ObjectGuid objectId)
        {
            Log($"Getting WorldObject Position {objectId.Full:X}");

            return worldObjects.ContainsKey(objectId) ? worldObjects[objectId].Location : null;
        }

        public float GetWorldObjectEffectiveUseRadius(ObjectGuid objectId)
        {
            Log($"Getting WorldObject Effective Use Radius {objectId.Full:X}");

            WorldObject wo = worldObjects.ContainsKey(objectId) ? worldObjects[objectId] : null;
            if (wo?.SetupTableId == null) return 0.00f;
            SetupModel csetup = SetupModel.ReadFromDat(wo.SetupTableId.Value);
            if (wo.UseRadius != null)
                return (float)Math.Pow(wo.UseRadius.Value + csetup.Radius + 1.5, 2);
            return (float)Math.Pow(0.25 + csetup.Radius + 1.5, 2);
        }

        // FIXME(ddevec): Hacky kludge -- trying to get rid of UseTime
        private void UseTimeWrapper()
        {
            UseTime(WorldManager.PortalYearTicks);
            actionQueue.EnqueueAction(new ActionEventDelegate(() => UseTimeWrapper()));
        }

        /// <summary>
        /// main game loop
        /// </summary>
        public void UseTime(double tickTime)
        {
            // here we'd move server objects in motion (subject to landscape) and do physics collision detection

            List<Player> allplayers = null;

            var allworldobj = worldObjects.Values;
            allplayers = allworldobj.OfType<Player>().ToList();

            UpdateStatus(allplayers.Count);

            // Handle broadcasts
            SendBroadcasts();
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

        /// <summary>
        /// Gets all landblocks in range of a position.  (for indoors positions that is just this landblock)
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private List<Landblock> GetLandblocksInRange(Position pos, float distance)
        {
            List<Landblock> inRange = new List<Landblock>();

            inRange.Add(this);

            if (pos.Indoors)
            {
                return inRange;
            }

            float highX = pos.PositionX + distance;
            float lowX = pos.PositionX - distance;
            float highY = pos.PositionY + distance;
            float lowY = pos.PositionY - distance;

            bool highXInLandblock = (highX < MaxXY);
            bool highYInLandblock = (highY < MaxXY);
            bool lowXInLandblock = (lowX > 0);
            bool lowYInLandblock = (lowY > 0);

            // Check East
            if (!highXInLandblock)
            {
                if (EastAdjacency != null)
                {
                    inRange.Add(EastAdjacency);
                }
            }

            // North East
            if (!highXInLandblock && !highYInLandblock)
            {
                if (NorthEastAdjacency != null)
                {
                    inRange.Add(NorthEastAdjacency);
                }
            }

            // North
            if (!highYInLandblock)
            {
                if (NorthAdjacency != null)
                {
                    inRange.Add(NorthAdjacency);
                }
            }

            // North West
            if (!lowXInLandblock && !highYInLandblock)
            {
                if (NorthWestAdjacency != null)
                {
                    inRange.Add(NorthWestAdjacency);
                }
            }

            // West
            if (!lowXInLandblock)
            {
                if (WestAdjacency != null)
                {
                    inRange.Add(WestAdjacency);
                }
            }

            // South West
            if (!lowXInLandblock && !lowYInLandblock)
            {
                if (SouthWestAdjacency != null)
                {
                    inRange.Add(SouthWestAdjacency);
                }
            }

            // South
            if (!lowYInLandblock)
            {
                if (SouthAdjacency != null)
                {
                    inRange.Add(SouthAdjacency);
                }
            }

            // South East
            if (!highXInLandblock && !lowYInLandblock)
            {
                if (SouthEastAdjacency != null)
                {
                    inRange.Add(SouthEastAdjacency);
                }
            }

            return inRange;
        }

        /// <summary>
        /// Enqueues a message for broadcast, thread safe
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <param name="msg"></param>
        public void EnqueueBroadcast(Position pos, float distance, params GameMessage[] msgs)
        {
            // Atomically checks and sets the broadcastQueued bit --
            //    guarantees that if we need a broadcast it will be enqueued in the world-managers broadcast queue exactly once
            if (Interlocked.CompareExchange(ref broadcastQueued, 1, 0) == 0)
            {
                WorldManager.BroadcastQueue.EnqueueAction(new ActionEventDelegate(() => SendBroadcasts()));
            }

            foreach (GameMessage msg in msgs)
            {
                broadcastQueue.Enqueue(new Tuple<Position, float, GameMessage>(pos, distance, msg));
            }
        }

        /// <summary>
        /// Convenience wrapper to EnqueueBroadcast to broadcast a motion.
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="motion"></param>
        public void EnqueueBroadcastMotion(WorldObject wo, UniversalMotion motion)
        {
            // wo must exist on us
            if (wo.CurrentLandblock != this)
            {
                log.Error("ERROR: Broadcasting motion from object not on our landblock");
            }

            EnqueueBroadcast(wo.Location, MaxObjectRange,
                new GameMessageUpdateMotion(wo.Guid,
                    wo.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    wo.Sequences, motion));
        }

        /// <summary>
        /// Convenience wrapper to EnqueueBroadcast to broadcast a sound.
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="sound"></param>
        /// <param name="volume"></param>
        public void EnqueueBroadcastSound(WorldObject wo, Sound sound, float volume = 1f)
        {
            // wo must exist on us
            if (wo.CurrentLandblock != this)
            {
                log.Error("ERROR: Broadcasting sound from object not on our landblock");
            }

            EnqueueBroadcast(wo.Location, MaxObjectRange,
                new GameMessageSound(wo.Guid,
                    sound,
                    volume));
        }

        /// <summary>
        /// Convenience wrapper to EnqueueBroadcast to broadcast local chat.
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="message"></param>
        public void EnqueueBroadcastLocalChat(WorldObject wo, string message)
        {
            // wo must exist on us
            if (wo.CurrentLandblock != this)
            {
                log.Error("ERROR: Broadcasting chat from object not on our landblock");
            }

            GameMessageCreatureMessage creatureMessage = new GameMessageCreatureMessage(message, wo.Name, wo.Guid.Full, ChatMessageType.Speech);

            EnqueueBroadcast(wo.Location, MaxObjectRange, creatureMessage);
        }

        /// <summary>
        /// Convenience wrapper to EnqueueBroadcast to broadcast local chat emotes.
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="emote"></param>
        public void EnqueueBroadcastLocalChatEmote(WorldObject wo, string emote)
        {
            // wo must exist on us
            if (wo.CurrentLandblock != this)
            {
                log.Error("ERROR: Broadcasting emote from object not on our landblock");
            }

            GameMessageEmoteText creatureMessage = new GameMessageEmoteText(wo.Guid.Full, wo.Name, emote);

            EnqueueBroadcast(wo.Location, MaxObjectRange, creatureMessage);
        }

        /// <summary>
        /// Convenience wrapper to EnqueueBroadcast to broadcast local soul emotes.
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="emote"></param>
        public void EnqueueBroadcastLocalChatSoulEmote(WorldObject wo, string emote)
        {
            // wo must exist on us
            if (wo.CurrentLandblock != this)
            {
                log.Error("ERROR: Broadcasting soul emote from object not on our landblock");
            }

            GameMessageSoulEmote creatureMessage = new GameMessageSoulEmote(wo.Guid.Full, wo.Name, emote);

            EnqueueBroadcast(wo.Location, MaxObjectRange, creatureMessage);
        }

        /// <summary>
        /// NOTE: Cannot be sent while objects are moving (the physics/motion portion of WorldManager)! depends on object positions not changing, and objects not moving between landblocks
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
                        if (p.Location.SquaredDistanceTo(pos) < distance * distance)
                        {
                            p.Session.Network.EnqueueSend(msg);
                        }
                    }
                }
            }

            // Sets broadcastQueued to 0, so we're ready to re-queue broadcasts later
            broadcastQueued = 0;
        }

        // Wrappers so landblocks can be treated as actors and actions
        // FIXME(ddevec): Once cludgy UseTime function removed, I can probably remove the action interface from landblock...?
        public LinkedListNode<IAction> EnqueueAction(IAction actn)
        {
            // Ugh enqueue stuff...
            return actionQueue.EnqueueAction(actn);
        }

        public void DequeueAction(LinkedListNode<IAction> node)
        {
            actionQueue.DequeueAction(node);
        }

        public void RunActions()
        {
            actionQueue.RunActions();
        }
        // End wrappers

        /// <summary>
        /// Runs an action on all players within a certain distance from a point.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <param name="delegateAction"></param>
        public void EnqueueActionBroadcast(Position pos, float distance, Action<Player> delegateAction)
        {
            List<Landblock> landblocksInRange = GetLandblocksInRange(pos, distance);

            foreach (Landblock lb in landblocksInRange)
            {
                List<Player> allPlayers = lb.worldObjects.Values.OfType<Player>().ToList();
                foreach (Player p in allPlayers)
                {
                    if (p.Location.SquaredDistanceTo(pos) < distance * distance)
                    {
                        p.EnqueueAction(new ActionEventDelegate(() => delegateAction(p)));
                    }
                }
            }
        }

        private List<WorldObject> GetWorldObjectsInRange(Position pos, float distance)
        {
            List<Landblock> landblocksInRange = GetLandblocksInRange(pos, distance);

            List<WorldObject> ret = new List<WorldObject>();
            foreach (Landblock lb in landblocksInRange)
            {
                ret.AddRange(lb.worldObjects.Values.Where(x => x.Location.SquaredDistanceTo(pos) < distance * distance).ToList());
            }

            return ret;
        }

        private List<WorldObject> GetWorldObjectsInRange(WorldObject wo, float distance)
        {
            return GetWorldObjectsInRange(wo.Location, distance);
        }

        /// <summary>
        /// Should only be called by the physics engine / WorldManager!
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<WorldObject> GetWorldObjectsInRangeForPhysics(WorldObject wo, float distance)
        {
            return GetWorldObjectsInRange(wo, distance);
        }

        private Landblock GetOwner(ObjectGuid guid)
        {
            if (worldObjects.ContainsKey(guid))
            {
                return this;
            }

            foreach (Landblock lb in adjacencies.Values)
            {
                if (lb != null && lb.worldObjects.ContainsKey(guid))
                {
                    return lb;
                }
            }

            return null;
        }

        private WorldObject GetObject(ObjectGuid guid)
        {
            Landblock lb = GetOwner(guid);
            if (lb == null)
            {
                return null;
            }
            return lb.worldObjects[guid];
        }

        public IActor GetActor(ObjectGuid guid)
        {
            Landblock lb = GetOwner(guid);
            if (lb == null)
            {
                return null;
            }
            return lb.worldObjects[guid];
        }

        public Position GetPosition(ObjectGuid guid)
        {
            Landblock lb = GetOwner(guid);
            if (lb == null)
            {
                return null;
            }
            return lb.worldObjects[guid].Location;
        }

        public void ChainOnObject(ActionChain chain, ObjectGuid woGuid, Action<WorldObject> action)
        {
            WorldObject wo = GetObject(woGuid);
            if (wo == null)
            {
                return;
            }

            chain.AddAction(wo, () => action(wo));
        }

        /// <summary>
        /// Intended for when moving an item directly to a player's container (which is not visible to the landblock)
        /// </summary>
        /// <param name="chain"></param>
        /// <param name="wo"></param>
        /// <param name="container"></param>
        public void ScheduleItemTransferInContainer(ActionChain chain, ObjectGuid wo, Container container)
        {
            // Find owner of wo
            Landblock lb = GetOwner(wo);

            if (lb != null)
            {
                chain.AddAction(lb.motionQueue, () => ItemTransferContainerInternal(wo, container));
            }
            else
            {
                // I find some of our debug commands have races between create and use -- This warning will trigger then
                log.Warn("Schedule transfer to item that doesn't exist -- ignoring");
            }
        }

        public void ScheduleItemTransfer(ActionChain chain, ObjectGuid wo, ObjectGuid container)
        {
            // Find owner of wo
            Landblock lb = GetOwner(wo);

            if (lb != null)
            {
                chain.AddAction(lb.motionQueue, () => ItemTransferInternal(wo, container));
            }
            else
            {
                // I find some of our debug commands have races between create and use -- This warning will trigger then
                log.Warn("Schedule transfer to item that doesn't exist -- ignoring");
            }
        }

        private void ItemTransferContainerInternal(ObjectGuid woGuid, Container container)
        {
            WorldObject wo = GetObject(woGuid);

            if (container == null || wo == null)
            {
                return;
            }

            RemoveWorldObjectInternal(woGuid, false);
            wo.ContainerId = container.Guid.Full;
            container.AddToInventory(wo);
        }

        private void ItemTransferInternal(ObjectGuid woGuid, ObjectGuid containerGuid)
        {
            Container container = GetObject(containerGuid) as Container;

            ItemTransferContainerInternal(woGuid, container);
        }

        private void Log(string message)
        {
            log.Debug($"LB {id.Landblock.ToString("X")}: {message}");
        }
    }
}
