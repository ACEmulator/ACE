using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Physics.Common;
using ACE.Server.WorldObjects;

using Position = ACE.Entity.Position;

namespace ACE.Server.Entity
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


        public LandblockId Id { get; }

        /// <summary>
        /// Flag indicates if this landblock is permanently loaded (for example, towns on high-traffic servers)
        /// </summary>
        public bool Permaload = false;

        private DateTime lastActiveTime;

        public bool AdjacenciesLoaded { get; internal set; }


        public readonly Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>(); // TODO Make this private
        private readonly Dictionary<Adjacency, Landblock> adjacencies = new Dictionary<Adjacency, Landblock>();


        private readonly ActionQueue actionQueue = new ActionQueue();

        /// <summary>
        /// Landblocks will be checked for activity every # seconds
        /// </summary>
        private static readonly TimeSpan heartbeatInterval = TimeSpan.FromSeconds(5);

        private DateTime lastHeartBeat = DateTime.MinValue;

        /// <summary>
        /// Landblocks which have been inactive for this many seconds will be unloaded
        /// </summary>
        private static readonly TimeSpan unloadInterval = TimeSpan.FromMinutes(5);


        /// <summary>
        /// The clientlib backing store landblock
        /// Eventually these classes could be merged, but for now they are separate...
        /// </summary>
        public readonly Physics.Common.Landblock _landblock;

        public CellLandblock CellLandblock { get; private set; }
        public LandblockInfo LandblockInfo { get; private set; }

        /// <summary>
        /// The landblock static meshes for
        /// collision detection and physics simulation
        /// </summary>
        public LandblockMesh LandblockMesh { get; private set; }
        public List<ModelMesh> LandObjects { get; private set; }
        public List<ModelMesh> Buildings { get; private set; }
        public List<ModelMesh> WeenieMeshes { get; private set; }
        public List<ModelMesh> Scenery { get; private set; }


        public Landblock(LandblockId id)
        {
            Id = id;

            // initialize adjacency array
            adjacencies.Add(Adjacency.North, null);
            adjacencies.Add(Adjacency.NorthEast, null);
            adjacencies.Add(Adjacency.East, null);
            adjacencies.Add(Adjacency.SouthEast, null);
            adjacencies.Add(Adjacency.South, null);
            adjacencies.Add(Adjacency.SouthWest, null);
            adjacencies.Add(Adjacency.West, null);
            adjacencies.Add(Adjacency.NorthWest, null);

            _landblock = LScape.get_landblock(Id.Raw);

            lastActiveTime = DateTime.UtcNow;

            Task.Run(() => CreateWorldObjects());

            Task.Run(() => SpawnDynamicShardObjects());

            Task.Run(() => SpawnEncounters());

            //LoadMeshes(objects);
        }

        /// <summary>
        /// Monster Locations, Generators<para />
        /// This will be called from a separate task from our constructor. Use thread safety when interacting with this landblock.
        /// </summary>
        private void CreateWorldObjects()
        {
            var objects = DatabaseManager.World.GetCachedInstancesByLandblock(Id.Landblock);
            var shardObjects = DatabaseManager.Shard.GetStaticObjectsByLandblock(Id.Landblock);
            var factoryObjects = WorldObjectFactory.CreateNewWorldObjects(objects, shardObjects);

            actionQueue.EnqueueAction(new ActionEventDelegate(() =>
            {
                foreach (var fo in factoryObjects)
                {
                    AddWorldObject(fo);
                    fo.ActivateLinks(objects, shardObjects);
                }
            }));
        }

        /// <summary>
        /// Corpses<para />
        /// This will be called from a separate task from our constructor. Use thread safety when interacting with this landblock.
        /// </summary>
        private void SpawnDynamicShardObjects()
        {
            var corpses = DatabaseManager.Shard.GetObjectsByLandblock(Id.Landblock);
            var factoryShardObjects = WorldObjectFactory.CreateWorldObjects(corpses);

            actionQueue.EnqueueAction(new ActionEventDelegate(() =>
            {
                foreach (var fso in factoryShardObjects)
                    AddWorldObject(fso);
            }));
        }

        /// <summary>
        /// Spawns the semi-randomized monsters scattered around the outdoors<para />
        /// This will be called from a separate task from our constructor. Use thread safety when interacting with this landblock.
        /// </summary>
        private void SpawnEncounters()
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

                actionQueue.EnqueueAction(new ActionEventDelegate(() =>
                {
                    if (!worldObjects.ContainsKey(wo.Guid))
                        AddWorldObject(wo);
                }));
            }
        }

        /// <summary>
        /// Loads the meshes for the landblock
        /// </summary>
        private void LoadMeshes(List<LandblockInstance> objects)
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
        private void LoadLandObjects()
        {
            LandObjects = new List<ModelMesh>();

            foreach (var obj in LandblockInfo.Objects)
                LandObjects.Add(new ModelMesh(obj.Id, obj.Frame));
        }

        /// <summary>
        /// Loads the meshes for the buildings on the landblock
        /// </summary>
        private void LoadBuildings()
        {
            Buildings = new List<ModelMesh>();

            foreach (var obj in LandblockInfo.Buildings)
                Buildings.Add(new ModelMesh(obj.ModelId, obj.Frame));
        }

        /// <summary>
        /// Loads the meshes for the weenies on the landblock
        /// </summary>
        private void LoadWeenies(List<LandblockInstance> objects)
        {
            WeenieMeshes = new List<ModelMesh>();

            foreach (var obj in objects)
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(obj.WeenieClassId);
                WeenieMeshes.Add(
                    new ModelMesh(weenie.GetProperty(PropertyDataId.Setup) ?? 0,
                    new DatLoader.Entity.Frame(new Position(obj.ObjCellId, obj.OriginX, obj.OriginY, obj.OriginZ, obj.AnglesX, obj.AnglesY, obj.AnglesZ, obj.AnglesW))));
            }
        }

        /// <summary>
        /// Loads the meshes for the scenery on the landblock
        /// </summary>
        private void LoadScenery()
        {
            Scenery = Entity.Scenery.Load(this);
        }

        public void Tick(double currentUnixTime)
        {
            actionQueue.RunActions();

            var wos = worldObjects.Values.ToList();

            foreach (var wo in wos)
                wo.Tick(currentUnixTime);

            // Heartbeat
            if (lastHeartBeat + heartbeatInterval <= DateTime.UtcNow)
            {
                // TODO: handle perma-loaded landblocks
                if (!Permaload && lastActiveTime + unloadInterval < DateTime.UtcNow)
                    LandblockManager.AddToDestructionQueue(this);

                lastHeartBeat = DateTime.UtcNow;
            }
        }

        public void SetAdjacency(Adjacency adjacency, Landblock landblock)
        {
            adjacencies[adjacency] = landblock;
        }

        private Landblock NorthAdjacency => adjacencies[Adjacency.North];
        private Landblock NorthEastAdjacency => adjacencies[Adjacency.NorthEast];
        private Landblock EastAdjacency => adjacencies[Adjacency.East];
        private Landblock SouthEastAdjacency => adjacencies[Adjacency.SouthEast];
        private Landblock SouthAdjacency => adjacencies[Adjacency.South];
        private Landblock SouthWestAdjacency => adjacencies[Adjacency.SouthWest];
        private Landblock WestAdjacency => adjacencies[Adjacency.West];
        private Landblock NorthWestAdjacency => adjacencies[Adjacency.NorthWest];

        private void AddPlayerTracking(List<WorldObject> wolist, Player player)
        {
            foreach (var wo in wolist)
                player.AddTrackedObject(wo);
        }

        public void AddWorldObject(WorldObject wo)
        {
            AddWorldObjectInternal(wo);
        }

        public void AddWorldObjectForPhysics(WorldObject wo)
        {
            AddWorldObjectInternal(wo);
        }

        private void AddWorldObjectInternal(WorldObject wo)
        {
            if (!worldObjects.ContainsKey(wo.Guid))
                worldObjects[wo.Guid] = wo;

            wo.CurrentLandblock = this;

            if (wo.PhysicsObj == null)
                wo.InitPhysicsObj();

            if (wo.PhysicsObj.CurCell == null)
            { 
                var success = wo.AddPhysicsObj();
                if (!success)
                {
                    log.Warn($"AddWorldObjectInternal: couldn't spawn {wo.Name}");
                    return;
                }
            }

            // if adding a player to this landblock,
            // tell them about other nearby objects
            if (wo is Player)
            {
                var newlyVisible = wo.PhysicsObj.handle_visible_cells();
                wo.PhysicsObj.enqueue_objs(newlyVisible);
            }

            // broadcast to nearby players
            wo.NotifyPlayers();
        }

        public void RemoveWorldObject(ObjectGuid objectId, bool adjacencyMove = false)
        {
            RemoveWorldObjectInternal(objectId, adjacencyMove);
        }

        /// <summary>
        /// Should only be called by physics/relocation engines -- not from player
        /// </summary>
        /// <param name="objectId">The object ID to be removed from the current landblock</param>
        /// <param name="adjacencyMove">Flag indicates if object is moving to an adjacent landblock</param>
        public void RemoveWorldObjectForPhysics(ObjectGuid objectId, bool adjacencyMove = false)
        {
            RemoveWorldObjectInternal(objectId, adjacencyMove);
        }

        /// <summary>
        /// This will also set the item.Location to null.
        /// </summary>
        public bool RemoveWorldObjectFromPickup(ObjectGuid objectGuid)
        {
            // Find owner of wo
            var lb = GetOwner(objectGuid);

            if (lb == null)
            {
                log.Error("Landblock QueueItemRemove failed to GetOwner");
                return false;
            }

            var item = GetObject(objectGuid);

            if (item == null)
            {
                log.Error("Landblock QueueItemRemove failed to GetObject");
                return false;
            }

            RemoveWorldObjectInternal(objectGuid, true);

            item.Location = null;

            return true;
        }

        private void RemoveWorldObjectInternal(ObjectGuid objectId, bool adjacencyMove = false)
        {
            //log.Debug($"LB {Id.Landblock:X}: removing {objectId.Full:X}");

            worldObjects.Remove(objectId, out var wo);

            if (wo == null) return;

            wo.CurrentLandblock = null;

            if (!adjacencyMove)
            {
                // really remove it - send message to client to remove object
                wo.EnqueueActionBroadcast((Player p) => p.RemoveTrackedObject(wo, true));

                wo.PhysicsObj.DestroyObject();
                wo.IsDestroyed = true;
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

        /// <summary>
        /// Returns landblock objects with physics initialized
        /// </summary>
        public List<WorldObject> GetPhysicsWorldObjects()
        {
            return worldObjects.Values.Where(wo => wo.PhysicsObj != null).ToList();
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
                    inRange.Add(EastAdjacency);
            }

            // North East
            if (!highXInLandblock && !highYInLandblock)
            {
                if (NorthEastAdjacency != null)
                    inRange.Add(NorthEastAdjacency);
            }

            // North
            if (!highYInLandblock)
            {
                if (NorthAdjacency != null)
                    inRange.Add(NorthAdjacency);
            }

            // North West
            if (!lowXInLandblock && !highYInLandblock)
            {
                if (NorthWestAdjacency != null)
                    inRange.Add(NorthWestAdjacency);
            }

            // West
            if (!lowXInLandblock)
            {
                if (WestAdjacency != null)
                    inRange.Add(WestAdjacency);
            }

            // South West
            if (!lowXInLandblock && !lowYInLandblock)
            {
                if (SouthWestAdjacency != null)
                    inRange.Add(SouthWestAdjacency);
            }

            // South
            if (!lowYInLandblock)
            {
                if (SouthAdjacency != null)
                    inRange.Add(SouthAdjacency);
            }

            // South East
            if (!highXInLandblock && !lowYInLandblock)
            {
                if (SouthEastAdjacency != null)
                    inRange.Add(SouthEastAdjacency);
            }

            return inRange;
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

        /// <summary>
        /// Searches this landblock (and possibly adjacents) for an ObjectGuid wielded by a creature
        /// </summary>
        public WorldObject GetWieldedObject(ObjectGuid guid, bool searchAdjacents = true)
        {
            // search creature wielded items in current landblock
            var creatures = worldObjects.Values.Where(wo => wo is Creature);
            foreach (var creature in creatures)
            {
                var wieldedItem = (creature as Creature).GetWieldedItem(guid);
                if (wieldedItem != null)
                    return wieldedItem;     // found it
            }

            // try searching adjacent landblocks if not found
            if (searchAdjacents)
            {
                foreach (var adjacent in adjacencies.Values)
                {
                    if (adjacent == null) continue;

                    var wieldedItem = adjacent.GetWieldedObject(guid, false);
                    if (wieldedItem != null)
                        return wieldedItem;
                }
            }
            return null;
        }

        public void ResendObjectsInRange(WorldObject wo)
        {
            // this could need reworked a bit for consistency..
            wo.PhysicsObj.ObjMaint.RemoveAllObjects();
            wo.PhysicsObj.handle_visible_cells();
        }

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
        /// Sets a landblock to active state, with the current time as the LastActiveTime
        /// </summary>
        /// <param name="isAdjacent">Public calls to this function should always set isAdjacent to false</param>
        public void SetActive(bool isAdjacent = false)
        {
            lastActiveTime = DateTime.UtcNow;

            if (isAdjacent || _landblock.IsDungeon) return;

            // for outdoor landblocks, recursively call 1 iteration to set adjacents to active
            foreach (var landblock in adjacencies.Values)
            {
                if (landblock != null)
                    landblock.SetActive(true);
            }
        }

        /// <summary>
        /// Handles the cleanup process for a landblock
        /// This method is called by LandblockManager
        /// </summary>
        public void Unload()
        {
            var landblockID = Id.Raw | 0xFFFF;

            log.Debug($"Landblock.Unload({landblockID:X})");

            SaveDB();

            // remove all objects
            foreach (var wo in worldObjects.Keys.ToList())
                RemoveWorldObjectInternal(wo);

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
        private void UnloadAdjacent(Adjacency? adjacency, Landblock landblock)
        {
            if (adjacency == null || adjacencies[adjacency.Value] != landblock)
            {
                log.Error($"Landblock({Id}).UnloadAdjacent({adjacency}, {landblock.Id}) couldn't find adjacent landblock");
                return;
            }
            adjacencies[adjacency.Value] = null;
            AdjacenciesLoaded = false;
        }

        private void SaveDB()
        {
            var biotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();

            foreach (var wo in worldObjects.Values)
            {
                // only updates corpses atm
                if (wo is Corpse corpse && !corpse.IsMonster)
                    biotas.Add((corpse.Biota, corpse.BiotaDatabaseLock));
            }

            DatabaseManager.Shard.SaveBiotas(biotas, result => { });
        }
    }
}
