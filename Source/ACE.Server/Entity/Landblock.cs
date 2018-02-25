using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.WorldObjects;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Entity
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

        private readonly Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();
        private readonly Dictionary<Adjacency, Landblock> adjacencies = new Dictionary<Adjacency, Landblock>();

        /// <summary>
        /// Special needs-broadcast flag.  Cleared in broadcast, but set in other phases.
        /// Must be treated exceptionally carefully to avoid races.  Don't touch unless you /really/ know what your doing
        /// </summary>
        private int broadcastQueued;

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

        public LandblockId Id { get; }

        public CellLandblock CellLandblock;
        public LandblockInfo LandblockInfo;

        /// <summary>
        /// The landblock static meshes for
        /// collision detection and physics simulation
        /// </summary>
        public LandblockMesh LandblockMesh;
        public List<ModelMesh> LandObjects;
        public List<ModelMesh> Buildings;
        public List<ModelMesh> WeenieMeshes;
        public List<ModelMesh> Scenery;

        public Landblock(LandblockId id)
        {
            this.Id = id;

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

            var objects = DatabaseManager.World.GetCachedWeenieInstancesByLandblock(Id.Landblock); // Instances

            var factoryObjects = WorldObjectFactory.CreateWorldObjects(objects);
            factoryObjects.ForEach(fo =>
            {
                if (!worldObjects.ContainsKey(fo.Guid))
                {
                    worldObjects.Add(fo.Guid, fo);
                    fo.SetParent(this);
                }
            });

            LoadMeshes(objects);

            UpdateStatus(LandBlockStatusFlag.IdleLoaded);

            // FIXME(ddevec): Goal: get rid of UseTime() function...
            actionQueue.EnqueueAction(new ActionEventDelegate(() => UseTimeWrapper()));

            motionQueue = new NestedActionQueue(WorldManager.MotionQueue);
        }

        /// <summary>
        /// Loads the meshes for the landblock
        /// </summary>
        public void LoadMeshes(List<AceObject> objects)
        {
            CellLandblock = DatManager.CellDat.ReadFromDat<CellLandblock>(Id.Raw | 0xFFFF);
            LandblockInfo = DatManager.CellDat.ReadFromDat<LandblockInfo>((uint)Id.Landblock << 16 | 0xFFFE);

            LandblockMesh = new LandblockMesh(Id);
            LoadLandObjects();
            LoadBuildings();
            LoadWeenies(objects);
            LoadScenery();
        }

        /// <summary>
        /// Loads the meshes for the static landblock objects,
        /// also known as obstacles
        /// </summary>
        public void LoadLandObjects()
        {
            LandObjects = new List<ModelMesh>();

            foreach (var obj in LandblockInfo.Objects)
                LandObjects.Add(new ModelMesh(obj.Id, obj.Frame));
        }

        /// <summary>
        /// Loads the meshes for the buildings on the landblock
        /// </summary>
        public void LoadBuildings()
        {
            Buildings = new List<ModelMesh>();

            foreach (var obj in LandblockInfo.Buildings)
                Buildings.Add(new ModelMesh(obj.ModelId, obj.Frame));
        }

        /// <summary>
        /// Loads the meshes for the weenies on the landblock
        /// </summary>
        public void LoadWeenies(List<AceObject> objects)
        {
            WeenieMeshes = new List<ModelMesh>();

            foreach (var obj in objects)
                WeenieMeshes.Add(new ModelMesh(obj.SetupDID.Value,
                    new DatLoader.Entity.Frame(obj.AceObjectPropertiesPositions.Values.LastOrDefault())));
        }

        /// <summary>
        /// Loads the meshes for the scenery on the landblock
        /// </summary>
        public void LoadScenery()
        {
            Scenery = Entity.Scenery.Load(this);
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
                if (o is Creature)
                {
                    if (((Creature)o).IsAlive)
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
            // EnqueueAction(new ActionEventDelegate(() => AddWorldObjectInternal(wo)));
            AddWorldObjectInternal(wo);
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
            Log($"adding {wo.Guid.Full:X}");

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
                AddPlayerTracking(wolist, ((Player)wo));
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

                if (!(wo is Creature))
                    worldObjects.Remove(objectId);
            }

            if (wo != null)
            {
                wo.SetParent(null);
                EnqueueActionBroadcast(wo.Location, MaxObjectRange, (Player p) => p.StopTrackingObject(wo, true));
            }
        }

        /// <summary>
        /// Check to see if we are close enough to interact.   Adds a fudge factor of 1.5f
        /// </summary>
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
            var csetup = DatManager.PortalDat.ReadFromDat<SetupModel>(wo.SetupTableId);
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
        /// Every game-loop iteration work.  Ideally this wouldn't exist, but we haven't finished
        /// fully transitioning landblocks to an event-based system.
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
            // TODO: Diagnostics uses WinForms, which is not supported in .net standard/core.
            // TODO: We need a better way to expose diagnostic information moving forward.
            // Diagnostics.Diagnostics.SetLandBlockKey(id.LandblockX, id.LandblockY, Status);
        }

        private void UpdateStatus(int pcount)
        {
            Status.PlayerCount = pcount;
            if (pcount > 0)
            {
                Status.LandBlockStatusFlag = LandBlockStatusFlag.InUseLow;
                // TODO: Diagnostics uses WinForms, which is not supported in .net standard/core.
                // TODO: We need a better way to expose diagnostic information moving forward.
                // Diagnostics.Diagnostics.SetLandBlockKey(id.LandblockX, id.LandblockY, Status);
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
        public void EnqueueBroadcastSystemChat(WorldObject wo, string message, ChatMessageType type)
        {
            // wo must exist on us
            if (wo.CurrentLandblock != this)
            {
                log.Error("ERROR: Broadcasting chat from object not on our landblock");
            }

            GameMessageSystemChat chatMsg = new GameMessageSystemChat(message, type);

            EnqueueBroadcast(wo.Location, MaxObjectRange, chatMsg);
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
                bool success = broadcastQueue.TryDequeue(out var tuple);
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

        public WorldObject GetObject(ObjectGuid guid)
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

        public WeenieType GetWeenieType(ObjectGuid guid)
        {
            return worldObjects.ContainsKey(guid) ? worldObjects[guid].WeenieType : 0;
        }

        /*
        public void ChainOnObject(ActionChain chain, ObjectGuid woGuid, Action<WorldObject> action)
        {
            WorldObject wo = GetObject(woGuid);
            if (wo == null)
            {
                return;
            }

            chain.AddAction(wo, () => action(wo));
        }
        */

        /// <summary>
        /// Intended for when moving an item directly to a player's container (which is not visible to the landblock)
        /// </summary>
        /// <param name="chain"></param>
        /// <param name="wo"></param>
        /// <param name="container"></param>
        /// <param name="placeent"></param>
        public void ScheduleItemTransferInContainer(ActionChain chain, ObjectGuid wo, Container container, uint placeent = 0)
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

        public void QueueItemTransfer(ActionChain chain, ObjectGuid wo, ObjectGuid container, int placement = 0)
        {
            // Find owner of wo
            Landblock lb = GetOwner(wo);

            if (lb != null)
            {
                chain.AddAction(lb.motionQueue, () => ItemTransferInternal(wo, container, placement));
            }
            else
            {
                // I find some of our debug commands have races between create and use -- This warning will trigger then
                log.Warn("Schedule transfer to item that doesn't exist -- ignoring");
            }
        }

        private void ItemTransferContainerInternal(ObjectGuid woGuid, Container container, int placement = 0)
        {
            WorldObject wo = GetObject(woGuid);

            if (container == null || wo == null)
            {
                return;
            }

            RemoveWorldObjectInternal(woGuid, false);
            wo.ContainerId = (int)container.Guid.Full;

            // We are coming off the world we need to be ready to save.
            wo.Location = null;
            wo.InitializeAceObjectForSave();
            container.AddToInventory(wo, placement);

            // Was Item controlled by a generator?
            // TODO: Should this be happening this way? Should the landblock notify the object of pickup or the generator...

            if (wo.GeneratorId > 0)
            {
                WorldObject generator = GetObject(new ObjectGuid((uint)wo.GeneratorId));

                wo.GeneratorId = null;

                generator.NotifyGeneratorOfPickup(wo.Guid.Full);
            }
        }

        private void ItemTransferInternal(ObjectGuid woGuid, ObjectGuid containerGuid, int placement = 0)
        {
            Container container = GetObject(containerGuid) as Container;

            ItemTransferContainerInternal(woGuid, container, placement);
        }

        private void Log(string message)
        {
            log.Debug($"LB {Id.Landblock:X}: {message}");
        }

        public void ResendObjectsInRange(WorldObject wo)
        {
            List<WorldObject> wolist = null;
            wolist = GetWorldObjectsInRange(wo, MaxObjectRange);
            AddPlayerTracking(wolist, (wo as Player));
        }
    }
}
