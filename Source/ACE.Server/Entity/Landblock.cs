using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;
using ACE.Server.Physics.Common;
using ACE.Server.WorldObjects;

using log4net;

using Position = ACE.Entity.Position;

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

        /// <summary>
        /// The clientlib backing store landblock
        /// Eventually these classes could be merged, but for now they are separate...
        /// </summary>
        private readonly Physics.Common.Landblock _landblock;

        public readonly Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();
        private readonly Dictionary<Adjacency, Landblock> adjacencies = new Dictionary<Adjacency, Landblock>();

        /// <summary>
        /// Special needs-broadcast flag.  Cleared in broadcast, but set in other phases.
        /// Must be treated exceptionally carefully to avoid races.  Don't touch unless you /really/ know what your doing
        /// </summary>
        private int broadcastQueued;

        // Can be appended concurrently, will be sent serially
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

        public bool AdjacenciesLoaded;

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
            Id = id;
            //Console.WriteLine("Landblock constructor(" + (id.Raw | 0xFFFF).ToString("X8") + ")");

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

            // create world objects (monster locations, generators)
            var objects = DatabaseManager.World.GetCachedInstancesByLandblock(Id.Landblock);
            var factoryObjects = WorldObjectFactory.CreateNewWorldObjects(objects);
            foreach (var fo in factoryObjects)
            {
                AddWorldObject(fo);
                fo.ActivateLinks();
            }

            // create shard objects (corpses after unloading)
            DatabaseManager.Shard.GetObjectsByLandblock(Id.Landblock, ((List<Biota> biotas) =>
            {
                var shardObjects = (WorldObjectFactory.CreateWorldObjects(biotas));
                foreach (var so in shardObjects)
                    AddWorldObject(so);
            }));

            _landblock = LScape.get_landblock(Id.Raw);

            //LoadMeshes(objects);

            SpawnEncounters();

            UpdateStatus(LandBlockStatusFlag.IdleLoaded);

            // FIXME(ddevec): Goal: get rid of UseTime() function...
            actionQueue.EnqueueAction(new ActionEventDelegate(() => UseTimeWrapper()));

            motionQueue = new NestedActionQueue(WorldManager.MotionQueue);

            LastActiveTime = Timer.CurrentTime;

            QueueNextHeartBeat();
        }

        /// <summary>
        /// Spawns the semi-randomized monsters scattered around the outdoors
        /// </summary>
        public void SpawnEncounters()
        {
            // get the encounter spawns for this landblock
            var encounters = DatabaseManager.World.GetCachedEncountersByLandblock(Id.Landblock);

            foreach (var encounter in encounters)
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(encounter.WeenieClassId);

                if (wo == null) continue;

                var xPos = encounter.CellX * 24.0f;
                var yPos = encounter.CellY * 24.0f;

                var pos = new Physics.Common.Position();
                pos.ObjCellID = (uint)(Id.Landblock << 16) | 1;
                pos.Frame = new Physics.Animation.AFrame(new Vector3(xPos, yPos, 0), Quaternion.Identity);
                pos.adjust_to_outside();

                pos.Frame.Origin.Z = _landblock.GetZ(pos.Frame.Origin);

                wo.Location = new Position(pos.ObjCellID, pos.Frame.Origin, pos.Frame.Orientation);

                if (!worldObjects.ContainsKey(wo.Guid))
                    AddWorldObject(wo);
            }
        }

        /// <summary>
        /// Loads the meshes for the landblock
        /// </summary>
        public void LoadMeshes(List<LandblockInstances> objects)
        {
            CellLandblock = DatManager.CellDat.ReadFromDat<CellLandblock>(Id.Raw >> 16 | 0xFFFF);
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
        public void LoadWeenies(List<LandblockInstances> objects)
        {
            WeenieMeshes = new List<ModelMesh>();

            foreach (var obj in objects)
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(obj.WeenieClassId);
                WeenieMeshes.Add(new ModelMesh(weenie.GetProperty(PropertyDataId.Setup) ?? 0,
                    new DatLoader.Entity.Frame(new Position(obj.ObjCellId, obj.OriginX, obj.OriginY, obj.OriginZ, obj.AnglesX, obj.AnglesY, obj.AnglesZ, obj.AnglesW))));
            }
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
            // envcell tracking handled in PhysicsObj.handle_visible_cells()
            if ((Id.Raw & 0xFFFF) >= 0x100) return;

            Parallel.ForEach(wolist, (o) =>
            {
                player.TrackObject(o);
            });
        }

        public void AddWorldObject(WorldObject wo)
        {
            AddWorldObjectInternal(wo);
        }

        public ActionChain GetAddWorldObjectChain(WorldObject wo, Player noBroadcast = null)
        {
            return new ActionChain(this, () => AddWorldObjectInternal(wo));
        }

        public void AddWorldObjectForPhysics(WorldObject wo)
        {
            AddWorldObjectInternal(wo);
        }

        private void AddWorldObjectInternal(WorldObject wo)
        {
            Log($"adding {wo.Guid}");

            if (!worldObjects.ContainsKey(wo.Guid))
                worldObjects[wo.Guid] = wo;

            wo.SetParent(this);

            if (wo.PhysicsObj == null)
                wo.InitPhysicsObj();

            var success = wo.AddPhysicsObj();
            if (!success)
                return;

            // broadcast to nearby players
            EnqueueActionBroadcast(wo.Location, MaxObjectRange, (Player p) => p.TrackObject(wo));

            // if spawning a player, tell them about nearby objects
            if (wo is Player)
            {
                var objectList = GetWorldObjectsInRange(wo, MaxObjectRange);
                AddPlayerTracking(objectList, wo as Player);
            }
        }

        public void RemoveWorldObject(ObjectGuid objectId, bool adjacencyMove = false)
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
        /// <param name="objectId">The object ID to be removed from the current landblock</param>
        /// <param name="adjacencyMove">Flag indicates if object is moving to an adjacent landblock</param>
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
                worldObjects.Remove(objectId);
            }

            if (wo != null)
            {
                wo.SetParent(null);
                if (wo.PreviousLocation != null)
                {
                    EnqueueActionBroadcast(wo.PreviousLocation, MaxObjectRange, (Player p) => p.StopTrackingObject(wo, false));
                    wo.ClearPreviousLocation();
                }
                else
                    EnqueueActionBroadcast(wo.Location, MaxObjectRange, (Player p) => p.StopTrackingObject(wo, true));

                if (!adjacencyMove)
                {
                    wo.PhysicsObj.leave_cell(false);
                    wo.PhysicsObj.remove_shadows_from_cells();
                    wo.PhysicsObj.remove_visible_cells();
                }
            }
        }

        /// <summary>
        /// Check to see if we are close enough to interact.   Adds a fudge factor of 1.5f
        /// </summary>
        public bool WithinUseRadius(Player player, ObjectGuid targetGuid, out bool validTargetGuid)
        {
            var target = GetObject(targetGuid);

            validTargetGuid = target != null;

            if (target != null)
                return player.IsWithinUseRadiusOf(target);

            return false;
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
            try
            {
                List<Player> allplayers = null;

                var allworldobj = worldObjects.Values;
                allplayers = allworldobj.OfType<Player>().ToList();

                UpdateStatus(allplayers.Count);

                // Handle broadcasts
                SendBroadcasts();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);   // FIXME: multithread
            }
        }

        /// <summary>
        /// Returns landblock objects with physics initialized
        /// </summary>
        public IEnumerable<WorldObject> GetPhysicsWorldObjects()
        {
            return worldObjects.Values.Where(wo => wo.PhysicsObj != null);
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
        public List<Landblock> GetLandblocksInRange(Position pos, float distance)
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
        /// Enqueues a message for broadcast, thread safe
        /// </summary>
        public void EnqueueBroadcast(Position pos, params GameMessage[] msgs)
        {
            // Atomically checks and sets the broadcastQueued bit --
            //    guarantees that if we need a broadcast it will be enqueued in the world-managers broadcast queue exactly once
            if (Interlocked.CompareExchange(ref broadcastQueued, 1, 0) == 0)
            {
                WorldManager.BroadcastQueue.EnqueueAction(new ActionEventDelegate(() => SendBroadcasts()));
            }

            foreach (GameMessage msg in msgs)
            {
                broadcastQueue.Enqueue(new Tuple<Position, float, GameMessage>(pos, MaxObjectRange, msg));
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
                ret.AddRange(lb.worldObjects.Values.Where(x => x.Location.SquaredDistanceTo(pos) < distance * distance).ToList());

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

        /// <summary>
        /// This will return null if the object was not found in the current or adjacent landblocks.
        /// </summary>
        private Landblock GetOwner(ObjectGuid guid)
        {
            if (worldObjects.ContainsKey(guid))
                return this;

            foreach (Landblock lb in adjacencies.Values)
            {
                if (lb != null && lb.worldObjects.ContainsKey(guid))
                    return lb;
            }

            return null;
        }

        public WorldObject GetObject(uint objectId)
        {
            return GetObject(new ObjectGuid(objectId));
        }

        /// <summary>
        /// This will return null if the object was not found in the current or adjacent landblocks.
        /// </summary>
        public WorldObject GetObject(ObjectGuid guid)
        {
            if (worldObjects.TryGetValue(guid, out var worldObject))
                return worldObject;

            foreach (Landblock lb in adjacencies.Values)
            {
                if (lb != null && lb.worldObjects.TryGetValue(guid, out worldObject))
                    return worldObject;
            }

            return null;
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
        /// This will create a RemoveWorldObjectInternal action for objectGuid on the appropriate Landblocks ActionChain, and that action will be added to callersChain.<para />
        /// What that means is, callersChain will start to execute after you queue it accordingly. When it gets to the point where you injected QueueItemRemove, it will block and wait for the objectGuids
        /// landblock to begin executing the RemoveWorldObjectInternal on its own ActionChain. After the landblock finishes that command, it will return and allow callersChain to resume execution.<para />
        /// This will also set the item.Location to null.
        /// </summary>
        public void QueueItemRemove(ActionChain callersChain, ObjectGuid objectGuid)
        {
            // Find owner of wo
            var lb = GetOwner(objectGuid);

            if (lb == null)
            {
                log.Error("Landblock QueueItemRemove failed to GetOwner");
                return;
            }

            callersChain.AddAction(lb.motionQueue, () =>
            {
                var item = GetObject(objectGuid);

                if (item == null)
                {
                    log.Error("Landblock QueueItemRemove failed to GetObject");
                    return;
                }

                RemoveWorldObjectInternal(objectGuid, false);

                item.Location = null;
            });
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

        /// <summary>
        /// Landblocks will be checked for activity every # seconds
        /// </summary>
        public static readonly int HeartbeatInterval = 5;

        public void QueueNextHeartBeat()
        {
            ActionChain nextHeartBeat = new ActionChain();
            nextHeartBeat.AddDelaySeconds(HeartbeatInterval);
            nextHeartBeat.AddAction(this, () => HeartBeat());
            nextHeartBeat.EnqueueChain();
        }

        /// <summary>
        /// Landblocks which have been inactive for this many seconds
        /// will be unloaded
        /// </summary>
        public static readonly int UnloadInterval = 300;

        /// <summary>
        /// Flag indicates if this landblock is permanently loaded
        /// (for example, towns on high-traffic servers)
        /// </summary>
        public bool Permaload = false;

        public void HeartBeat()
        {
            if (IsActive)
            {
                // tick decayable objects

                // tick items sold to vendors
            }

            // TODO: handle perma-loaded landblocks
            if (!Permaload && LastActiveTime + UnloadInterval < Timer.CurrentTime)
                LandblockManager.AddToDestructionQueue(this);
            else
                QueueNextHeartBeat();
        }

        public bool IsActive = true;
        public double LastActiveTime;

        /*/// <summary>
        /// A landblock is active when it contains at least 1 player
        /// </summary>
        public bool IsActive(bool testAdjacents = true)
        {
            // TODO: handle perma-loaded landblocks 

            // for increased performance,
            // if Player ObjectGuids are always within a specific range,
            // those could possibly be checked instead of casting WorldObject to Player here
            var currentActive = worldObjects.Values.FirstOrDefault(wo => wo is Player) != null;

            if (currentActive || !testAdjacents || _landblock.IsDungeon)
                return currentActive;

            // outdoor landblocks:
            // activity is also determined by adjacent landblocks
            foreach (var adjacent in adjacencies.Values)
                if (adjacent.IsActive(false))
                    return true;

            return false;
        }*/

        /// <summary>
        /// Sets a landblock to active state, with the current time
        /// as the LastActiveTime
        /// </summary>
        /// <param name="isAdjacent">Public calls to this function should always set isAdjacent to false</param>
        public void SetActive(bool isAdjacent = false)
        {
            IsActive = true;
            LastActiveTime = Timer.CurrentTime;

            if (isAdjacent || _landblock.IsDungeon) return;

            // for outdoor landblocks, recursively call 1 iteration
            // to set adjacents to active
            foreach (var landblock in adjacencies.Values)
                if (landblock != null)
                    landblock.SetActive(true);
        }

        /// <summary>
        /// Handles the cleanup process for a landblock
        /// This method is called by LandblockManager
        /// </summary>
        public void Unload()
        {
            var landblockID = Id.Raw | 0xFFFF;
            //Console.WriteLine($"Landblock.Unload({landblockID:X})");
            SaveDB();

            // remove all objects
            foreach (var wo in worldObjects.Keys.ToList())
                RemoveWorldObjectInternal(wo, false);

            // remove physics landblock
            LScape.unload_landblock(landblockID);

            // dungeon landblocks do not handle adjacents
            if (_landblock.IsDungeon) return;

            // notify adjacents
            foreach (var adjacent in adjacencies.Where(adj => adj.Value != null))
                adjacent.Value.UnloadAdjacent(AdjacencyHelper.GetInverse(adjacent.Key), this);

            // TODO: cleanup physics landblock references
        }

        /// <summary>
        /// Removes a neighbor landblock from the adjacencies list
        /// </summary>
        public void UnloadAdjacent(Adjacency? adjacency, Landblock landblock)
        {
            if (adjacency == null || adjacencies[adjacency.Value] != landblock)
            {
                Console.WriteLine($"Landblock({Id}).UnloadAdjacent({adjacency}, {landblock.Id}) couldn't find adjacent landblock");
                return;
            }
            adjacencies[adjacency.Value] = null;
            AdjacenciesLoaded = false;
        }

        public void SaveDB()
        {
            // only updates corpses atm
            var biotas = worldObjects.Values.Where(wo => wo is Corpse && (wo as Corpse).IsMonster == false).Select(wo => wo.Biota).ToList();

            DatabaseManager.Shard.SaveBiotas(biotas, result => { });
        }
    }
}
