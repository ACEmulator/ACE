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
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Physics.Common;
using ACE.Server.Network.GameMessages;
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

        private readonly Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();

        public List<Landblock> Adjacents = new List<Landblock>();

        private readonly ActionQueue actionQueue = new ActionQueue();

        /// <summary>
        /// Landblocks heartbeat every 5 seconds
        /// </summary>
        private static readonly TimeSpan heartbeatInterval = TimeSpan.FromSeconds(5);

        private DateTime lastHeartBeat = DateTime.MinValue;

        /// <summary>
        /// Landblock items will be saved to the database every 5 minutes
        /// </summary>
        private static readonly TimeSpan databaseSaveInterval = TimeSpan.FromMinutes(5);

        private DateTime lastDatabaseSave = DateTime.MinValue;

        /// <summary>
        /// Landblocks which have been inactive for this many seconds will be unloaded
        /// </summary>
        private static readonly TimeSpan unloadInterval = TimeSpan.FromMinutes(5);


        /// <summary>
        /// The clientlib backing store landblock
        /// Eventually these classes could be merged, but for now they are separate...
        /// </summary>
        public Physics.Common.Landblock _landblock { get; private set; }

        public CellLandblock CellLandblock { get; }
        public LandblockInfo LandblockInfo { get; }

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

            CellLandblock = DatManager.CellDat.ReadFromDat<CellLandblock>(Id.Raw >> 16 | 0xFFFF);
            LandblockInfo = DatManager.CellDat.ReadFromDat<LandblockInfo>((uint)Id.Landblock << 16 | 0xFFFE);

            lastActiveTime = DateTime.UtcNow;

            Task.Run(() =>
            {
                _landblock = LScape.get_landblock(Id.Raw);

                CreateWorldObjects();

                SpawnDynamicShardObjects();

                SpawnEncounters();
            });

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

            // for mansion linking
            var houses = new List<House>();

            actionQueue.EnqueueAction(new ActionEventDelegate(() =>
            {
                foreach (var fo in factoryObjects)
                {
                    WorldObject parent = null;
                    if (fo.WeenieType == ACE.Entity.Enum.WeenieType.House && fo.HouseType == ACE.Entity.Enum.HouseType.Mansion)
                    {
                        var house = fo as House;
                        houses.Add(house);
                        house.LinkedHouses.Add(houses[0]);

                        if (houses.Count > 1)
                        {
                            houses[0].LinkedHouses.Add(house);
                            parent = houses[0];
                        }
                    }

                    AddWorldObject(fo);
                    fo.ActivateLinks(objects, shardObjects, parent);
                }
            }));
        }

        /// <summary>
        /// Corpses<para />
        /// This will be called from a separate task from our constructor. Use thread safety when interacting with this landblock.
        /// </summary>
        private void SpawnDynamicShardObjects()
        {
            var dynamics = DatabaseManager.Shard.GetDynamicObjectsByLandblock(Id.Landblock);
            var factoryShardObjects = WorldObjectFactory.CreateWorldObjects(dynamics);

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

                var xPos = Math.Clamp(encounter.CellX * 24.0f, 0.5f, 191.5f);
                var yPos = Math.Clamp(encounter.CellY * 24.0f, 0.5f, 191.5f);

                var pos = new Physics.Common.Position();
                pos.ObjCellID = (uint)(Id.Landblock << 16) | 1;
                pos.Frame = new Physics.Animation.AFrame(new Vector3(xPos, yPos, 0), Quaternion.Identity);
                pos.adjust_to_outside();

                pos.Frame.Origin.Z = _landblock.GetZ(pos.Frame.Origin);

                wo.Location = new Position(pos.ObjCellID, pos.Frame.Origin, pos.Frame.Orientation);

                actionQueue.EnqueueAction(new ActionEventDelegate(() =>
                {
                    AddWorldObject(wo);
                }));
            }
        }

        /// <summary>
        /// Loads the meshes for the landblock<para />
        /// This isn't used by ACE, but we still retain it for the following reason:<para />
        /// its useful, concise, high level overview code for everything needed to load landblocks, all their objects, scenery, polygons
        /// without getting into all of the low level methods that acclient uses to do it
        /// </summary>
        private void LoadMeshes(List<LandblockInstance> objects)
        {
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

            // When a WorldObject Ticks, it can end up adding additional WorldObjects to this landblock
            foreach (var wo in wos)
                wo.Tick(currentUnixTime);

            // Heartbeat
            if (lastHeartBeat + heartbeatInterval <= DateTime.UtcNow)
            {
                var thisHeartBeat = DateTime.UtcNow;

                // Decay world objects
                foreach (var wo in wos)
                {
                    if (wo.IsDecayable())
                        wo.Decay(thisHeartBeat - lastHeartBeat);
                }

                if (!Permaload && lastActiveTime + unloadInterval < thisHeartBeat)
                    LandblockManager.AddToDestructionQueue(this);

                lastHeartBeat = thisHeartBeat;
            }

            // Database Save
            if (lastDatabaseSave + databaseSaveInterval <= DateTime.UtcNow)
            {
                SaveDB();
                lastDatabaseSave = DateTime.UtcNow;
            }
        }

        private void AddPlayerTracking(List<WorldObject> wolist, Player player)
        {
            foreach (var wo in wolist)
                player.AddTrackedObject(wo);
        }

        /// <summary>
        /// This will fail if the wo doesn't have a valid location.
        /// </summary>
        public bool AddWorldObject(WorldObject wo)
        {
            if (wo.Location == null)
            {
                log.DebugFormat("Landblock 0x{0} failed to add 0x{1:X8} {2}. Invalid Location", Id, wo.Biota.Id, wo.Name);
                return false;
            }

            AddWorldObjectInternal(wo);

            return true;
        }

        public void AddWorldObjectForPhysics(WorldObject wo)
        {
            AddWorldObjectInternal(wo);
        }

        private void AddWorldObjectInternal(WorldObject wo)
        {
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
            if (wo is Player || wo is CombatPet)
            {
                var newlyVisible = wo.PhysicsObj.handle_visible_cells();
                wo.PhysicsObj.enqueue_objs(newlyVisible);
            }

            // broadcast to nearby players
            wo.NotifyPlayers();
        }

        public void RemoveWorldObject(ObjectGuid objectId, bool adjacencyMove = false, bool fromPickup = false)
        {
            RemoveWorldObjectInternal(objectId, adjacencyMove, fromPickup);
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

        private void RemoveWorldObjectInternal(ObjectGuid objectId, bool adjacencyMove = false, bool fromPickup = false)
        {
            //log.Debug($"LB {Id.Landblock:X}: removing {objectId.Full:X}");

            worldObjects.Remove(objectId, out var wo);

            if (wo == null) return;

            wo.CurrentLandblock = null;

            // Weenies can come with a default of 0 or -1. If they still have that value, we want to retain it.
            if (wo.TimeToRot.HasValue && wo.TimeToRot != 0 && wo.TimeToRot != -1)
                wo.TimeToRot = null;

            if (!adjacencyMove)
            {
                // really remove it - send message to client to remove object
                wo.EnqueueActionBroadcast(p => p.RemoveTrackedObject(wo, fromPickup));

                wo.PhysicsObj.DestroyObject();
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
        public List<WorldObject> GetWorldObjectsForPhysicsHandling()
        {
            // If a missile is destroyed when it runs it's UpdateObjectPhysics(), it will remove itself from the landblock, thus, modifying the worldObjects collection.
            return worldObjects.Values.ToList();
        }

        public List<WorldObject> GetAllWorldObjectsForDiagnostics()
        {
            return worldObjects.Values.ToList();
        }

        /// <summary>
        /// This will return null if the object was not found in the current or adjacent landblocks.
        /// </summary>
        private Landblock GetOwner(ObjectGuid guid)
        {
            if (worldObjects.ContainsKey(guid))
                return this;

            foreach (Landblock lb in Adjacents)
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

            foreach (Landblock lb in Adjacents)
            {
                if (lb != null && lb.worldObjects.TryGetValue(guid, out worldObject))
                    return worldObject;
            }

            return null;
        }

        public WorldObject GetWieldedObject(uint objectGuid, bool searchAdjacents = true)
        {
            return GetWieldedObject(new ObjectGuid(objectGuid), searchAdjacents); // todo fix
        }

        /// <summary>
        /// Searches this landblock (and possibly adjacents) for an ObjectGuid wielded by a creature
        /// </summary>
        public WorldObject GetWieldedObject(ObjectGuid guid, bool searchAdjacents = true)
        {
            // search creature wielded items in current landblock
            var creatures = worldObjects.Values.OfType<Creature>();
            foreach (var creature in creatures)
            {
                var wieldedItem = creature.GetEquippedItem(guid);
                if (wieldedItem != null)
                {
                    if ((wieldedItem.CurrentWieldedLocation & EquipMask.Selectable) != 0)
                        return wieldedItem;

                    return null;
                }
            }

            // try searching adjacent landblocks if not found
            if (searchAdjacents)
            {
                foreach (var adjacent in Adjacents)
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
            wo.PhysicsObj.ObjMaint.RemoveAllObjects();

            var visibleObjs = wo.PhysicsObj.handle_visible_cells();
            wo.PhysicsObj.enqueue_objs(visibleObjs);
        }

        /// <summary>
        /// Sets a landblock to active state, with the current time as the LastActiveTime
        /// </summary>
        /// <param name="isAdjacent">Public calls to this function should always set isAdjacent to false</param>
        public void SetActive(bool isAdjacent = false)
        {
            lastActiveTime = DateTime.UtcNow;

            if (isAdjacent || _landblock == null || _landblock.IsDungeon) return;

            // for outdoor landblocks, recursively call 1 iteration to set adjacents to active
            foreach (var landblock in Adjacents)
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
        }

        private void SaveDB()
        {
            var biotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();

            foreach (var wo in worldObjects.Values)
            {
                if (wo.IsStaticThatShouldPersistToShard() || wo.IsDecayableThatShouldPersistToShard())
                    AddWorldObjectToBiotasSaveCollection(wo, biotas);
            }

            DatabaseManager.Shard.SaveBiotasInParallel(biotas, result => { });
        }

        private void AddWorldObjectToBiotasSaveCollection(WorldObject wo, Collection<(Biota biota, ReaderWriterLockSlim rwLock)> biotas)
        {
            if (wo.ChangesDetected)
            {
                wo.SaveBiotaToDatabase(false);
                biotas.Add((wo.Biota, wo.BiotaDatabaseLock));
            }

            if (wo is Container container)
            {
                foreach (var item in container.Inventory.Values)
                    AddWorldObjectToBiotasSaveCollection(item, biotas);
            }
        }

        /// <summary>
        /// This is only used for very specific instances, such as broadcasting player deaths to the destination lifestone block
        /// This is a rarely used method to broadcast network messages to all of the players within a landblock,
        /// and possibly the adjacent landblocks.
        /// </summary>
        public void EnqueueBroadcast(ICollection<Player> excludeList, bool adjacents, params GameMessage[] msgs)
        {
            var players = worldObjects.Values.OfType<Player>();

            // for landblock death broadcasts:
            // exclude players that have already been broadcast to within range of the death
            if (excludeList != null)
                players = players.Except(excludeList);

            // broadcast messages to player in this landblock
            foreach (var player in players)
                player.Session.Network.EnqueueSend(msgs);

            // if applicable, iterate into adjacent landblocks
            if (adjacents)
            {
                foreach (var adjacent in this.Adjacents.Where(adj => adj != null))
                    adjacent.EnqueueBroadcast(excludeList, false, msgs);
            }
        }

        private bool? isDungeon;

        /// <summary>
        /// Returns TRUE if this landblock is a dungeon
        /// </summary>
        public bool IsDungeon
        {
            get
            {
                // return cached value
                if (isDungeon != null)
                    return isDungeon.Value;

                // a dungeon landblock is determined by:
                // - all heights being 0
                // - having at least 1 EnvCell (0x100+)
                // - contains no buildings
                foreach (var height in CellLandblock.Height)
                {
                    if (height != 0)
                    {
                        isDungeon = false;
                        return isDungeon.Value;
                    }
                }
                isDungeon = LandblockInfo != null && LandblockInfo.NumCells > 0 && LandblockInfo.Buildings != null && LandblockInfo.Buildings.Count == 0;
                return isDungeon.Value;
            }
        }

        private bool? isHouseDungeon;

        public bool IsHouseDungeon
        {
            get
            {
                // return cached value
                if (isHouseDungeon != null)
                    return isHouseDungeon.Value;

                isHouseDungeon = IsDungeon ? DatabaseManager.World.GetHousePortalsByLandblock(Id.Landblock).Count > 0 : false;

                return isHouseDungeon.Value;
            }
        }
    }
}
