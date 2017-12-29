using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ACE.Managers;
using log4net;
using ACE.Database;
using ACE.Network.GameMessages;
using ACE.Network.Sequence;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
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
    public class Landblock
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

        // private byte cellGridMaxX = 8; // todo: load from cell.dat
        // private byte cellGridMaxY = 8; // todo: load from cell.dat

        // not sure if a full object is necessary here.  I don't think a Landcell has any
        // inherent functionality that needs to be modelled in an object.
        // private Landcell[,] cellGrid; // todo: load from cell.dat

        public LandBlockStatus Status = new LandBlockStatus();

        public bool Loaded { get; private set; } = false;

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
        }

        public void Unload()
        {
            if (!Loaded)
            {
                return;
            }

            worldObjects.Clear(); ;

            Loaded = false;
        }

        public async Task Load()
        {
            if (Loaded)
            {
                return;
            }

            // TODO: Load cell.dat contents
            //   1. landblock cell structure
            //   2. terrain data
            // TODO: Load portal.dat contents (as/if needed)
            // TODO: Load spawn data

            var objects = await DatabaseManager.World.GetWeenieInstancesByLandblock(this.id.Landblock); // Instances
            // FIXME: Likely the next line should be eliminated after generators have been refactored into the instance structure, if that ends up making the most sense
            //        I don't know for sure however that it does yet. More research on them is required -Ripley
            objects.AddRange(await DatabaseManager.World.GetObjectsByLandblock(this.id.Landblock)); // Generators

            var factoryObjects = await WorldObjectFactory.CreateWorldObjects(objects);
            factoryObjects.ForEach(fo =>
            {
                if (!worldObjects.ContainsKey(fo.Guid))
                {
                    worldObjects.Add(fo.Guid, fo);
                    fo.SetParent(this);
                }
            });

            UpdateStatus(LandBlockStatusFlag.IdleLoaded);

            Loaded = true;
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

        public async Task AddWorldObject(WorldObject wo)
        {
            await AddWorldObjectInternal(wo);
        }

        private async Task AddWorldObjectInternal(WorldObject wo)
        {
            Log($"adding {wo.Guid.Full.ToString("X")}");

            if (!Loaded)
            {
                await Load();
            }

            if (!worldObjects.ContainsKey(wo.Guid))
                worldObjects[wo.Guid] = wo;

            wo.SetParent(this);

            // Alert all nearby players of the object
            EnqueueActionBroadcast(wo.Location, MaxObjectRange, (Player p) => p.TrackObject(wo));

            // if this is a player, tell them about everything else we have in range of them.
            if (wo is Player)
            {
                List<WorldObject> wolist = null;
                wolist = await GetWorldObjectsInRange(wo, MaxObjectRange);
                AddPlayerTracking(wolist, (wo as Player));
            }
        }

        public void RemoveWorldObject(ObjectGuid objectId, bool adjacencyMove)
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
            SetupModel csetup = SetupModel.ReadFromDat(wo.SetupTableId.Value);
            if (wo.UseRadius != null)
                return (float)Math.Pow(wo.UseRadius.Value + csetup.Radius + 1.5, 2);
            return (float)Math.Pow(0.25 + csetup.Radius + 1.5, 2);
        }

        // FIXME(ddevec): Hacky kludge -- trying to get rid of UseTime
        private void UseTimeWrapper()
        {
            UseTime(WorldManager.PortalYearTicks);
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
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <param name="msg"></param>
        public void EnqueueBroadcast(Position pos, float distance, params GameMessage[] msgs)
        {
            SendBroadcast(pos, distance, msgs);
        }

        // FIXME(ddevec): Remove this after I fix tracking...
        public void EnqueueActionBroadcast(Position pos, float distance, Action<Player> act)
        {
            List<Landblock> landblocksInRange = GetLandblocksInRange(pos, distance);
            foreach (Landblock lb in landblocksInRange)
            {
                List<Player> allPlayers = lb.worldObjects.Values.OfType<Player>().ToList();
                foreach (Player p in allPlayers)
                {
                    if (p.Location.SquaredDistanceTo(pos) < distance * distance)
                    {
                        act(p);
                    }
                }
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
        private void SendBroadcast(Position pos, float distance, GameMessage[] msgs)
        {
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
                        p.Session.Network.EnqueueSend(msgs);
                    }
                }
            }
        }

        private async Task<List<WorldObject>> GetWorldObjectsInRange(Position pos, float distance)
        {
            List<Landblock> landblocksInRange = GetLandblocksInRange(pos, distance);

            List<WorldObject> ret = new List<WorldObject>();
            foreach (Landblock lb in landblocksInRange)
            {
                if (!lb.Loaded)
                {
                    await lb.Load();
                }

                ret.AddRange(lb.worldObjects.Values.Where(x => x.Location.SquaredDistanceTo(pos) < distance * distance).ToList());
            }

            return ret;
        }

        private async Task<List<WorldObject>> GetWorldObjectsInRange(WorldObject wo, float distance)
        {
            return await GetWorldObjectsInRange(wo.Location, distance);
        }

        /// <summary>
        /// Should only be called by the physics engine / WorldManager!
        /// </summary>
        public async Task<List<WorldObject>> GetWorldObjectsInRangeForPhysics(WorldObject wo, float distance)
        {
            return await GetWorldObjectsInRange(wo, distance);
        }

        private async Task<Landblock> GetOwner(ObjectGuid guid)
        {
            Landblock ret = null;
            if (worldObjects.ContainsKey(guid))
            {
                ret = this;
            }
            else
            {
                foreach (Landblock lb in adjacencies.Values)
                {
                    if (lb != null && lb.worldObjects.ContainsKey(guid))
                    {
                        ret = lb;
                        break;
                    }
                }
            }

            if (ret != null && !ret.Loaded)
            {
                await ret.Load();
            }

            return ret;
        }

        public async Task<WorldObject> GetObject(ObjectGuid guid)
        {
            Landblock lb = await GetOwner(guid);
            if (lb == null)
            {
                return null;
            }
            return lb.worldObjects[guid];
        }

        public async Task<Position> GetPosition(ObjectGuid guid)
        {
            Landblock lb = await GetOwner(guid);
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
        public async Task ItemTransferInContainer(ObjectGuid wo, Container container, uint placeent = 0)
        {
            // Find owner of wo
            Landblock lb = await GetOwner(wo);

            if (lb != null)
            {
                await ItemTransferContainerInternal(wo, container);
            }
            else
            {
                // I find some of our debug commands have races between create and use -- This warning will trigger then
                log.Warn("Schedule transfer to item that doesn't exist -- ignoring");
            }
        }

        public async Task QueueItemTransfer(ObjectGuid wo, ObjectGuid container, int placement = 0)
        {
            // Find owner of wo
            Landblock lb = await GetOwner(wo);

            if (lb != null)
            {
                await ItemTransferInternal(wo, container, placement);
            }
            else
            {
                // I find some of our debug commands have races between create and use -- This warning will trigger then
                log.Warn("Schedule transfer to item that doesn't exist -- ignoring");
            }
        }

        private async Task ItemTransferContainerInternal(ObjectGuid woGuid, Container container, int placement = 0)
        {
            WorldObject wo = await GetObject(woGuid);

            if (container == null || wo == null)
            {
                return;
            }

            RemoveWorldObjectInternal(woGuid, false);
            wo.ContainerId = container.Guid.Full;

            // We are coming off the world we need to be ready to save.
            wo.Location = null;
            wo.InitializeAceObjectForSave();
            container.AddToInventory(wo, placement);
        }

        private async Task ItemTransferInternal(ObjectGuid woGuid, ObjectGuid containerGuid, int placement = 0)
        {
            Container container = await GetObject(containerGuid) as Container;

            await ItemTransferContainerInternal(woGuid, container, placement);
        }

        private void Log(string message)
        {
            log.Debug($"LB {id.Landblock.ToString("X")}: {message}");
        }
    }
}
