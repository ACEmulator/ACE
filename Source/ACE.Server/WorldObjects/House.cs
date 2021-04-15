using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.Structure;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class House : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // TODO now that the new biota model uses a dictionary for this, see if we can remove this duplicate dictionary
        public Dictionary<ObjectGuid, bool> Guests;

        public static int MaxGuests = 128;

        /// <summary>
        /// house open/closed status
        /// 0 = closed, 1 = open
        /// </summary>
        public bool OpenStatus { get => OpenToEveryone; set => OpenToEveryone = value; }

        /// <summary>
        /// For linking mansions
        /// </summary>
        public List<House> LinkedHouses;

        public SlumLord SlumLord { get => ChildLinks.FirstOrDefault(l => l as SlumLord != null) as SlumLord; }
        public List<Hook> Hooks { get => ChildLinks.OfType<Hook>().ToList(); }
        public List<Storage> Storage { get => ChildLinks.OfType<Storage>().ToList(); }
        public HashSet<ObjectGuid> StorageAccess => Guests.Where(i => i.Value).Select(i => i.Key).ToHashSet();
        public WorldObject BootSpot => ChildLinks.FirstOrDefault(i => i.WeenieType == WeenieType.BootSpot);

        public HousePortal HousePortal { get => ChildLinks.FirstOrDefault(l => l as HousePortal != null) as HousePortal;  }
        public List<WorldObject> Linkspots => ChildLinks.Where(l => l.WeenieType == WeenieType.Generic && l.WeenieClassName.Equals("portaldestination")).ToList();

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public House(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            InitializePropertyDictionaries();
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public House(Biota biota) : base(biota)
        {
            InitializePropertyDictionaries();
            SetEphemeralValues();
        }

        private void InitializePropertyDictionaries()
        {
            if (Biota.HousePermissions == null)
                Biota.HousePermissions = new Dictionary<uint, bool>();
        }

        private void SetEphemeralValues()
        {
            DefaultScriptId = (uint)PlayScript.RestrictionEffectBlue;

            BuildGuests();

            LinkedHouses = new List<House>();
        }

        /// <summary>
        /// Builds a HouseData structure for this house
        /// This is used to populate the info in the House panel
        /// </summary>
        public HouseData GetHouseData(IPlayer owner)
        {
            var houseData = new HouseData();
            houseData.Position = Location;
            houseData.Type = HouseType;

            if (SlumLord == null)
            {
                //Console.WriteLine($"No slumlord found for {Name} ({Guid})");
                log.Warn($"[HOUSE] No slumlord found for {Name} ({Guid})");
            }
            else
            {
                houseData.SetBuyItems(SlumLord.GetBuyItems());
                houseData.SetRentItems(SlumLord.GetRentItems());
            }

            if (owner != null)
            {
                houseData.BuyTime = (uint)(owner.HousePurchaseTimestamp ?? 0);
                houseData.RentTime = GetRentTimestamp(houseData.BuyTime);
                houseData.SetPaidItems(SlumLord);
            }

            if (HouseStatus == HouseStatus.InActive)
                houseData.MaintenanceFree = true;

            return houseData;
        }

        public static House Load(uint houseGuid, bool isBasement = false)
        {
            var landblock = (ushort)((houseGuid >> 12) & 0xFFFF);

            var biota = DatabaseManager.Shard.BaseDatabase.GetBiota(houseGuid);
            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblock);

            if (biota == null)
            {
                if (instances != null)
                {
                    var houseInstance = instances.FirstOrDefault(h => h.Guid == houseGuid);

                    if (houseInstance != null)
                    {
                        var weenie = DatabaseManager.World.GetCachedWeenie(houseInstance.WeenieClassId);
                        var objectGuid = new ObjectGuid(houseInstance.Guid);

                        var newWorldObject = WorldObjectFactory.CreateWorldObject(weenie, objectGuid);

                        biota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(newWorldObject.Biota);
                    }
                }
            }

            var linkedHouses = WorldObjectFactory.CreateNewWorldObjects(instances, new List<ACE.Database.Models.Shard.Biota> { biota }, biota.WeenieClassId);

            foreach (var linkedHouse in linkedHouses)
                linkedHouse.ActivateLinks(instances, new List<ACE.Database.Models.Shard.Biota> { biota }, linkedHouses[0]);

            var house = (House)linkedHouses[0];

            if (isBasement)
                return house;

            // load slumlord biota for rent
            if (house.SlumLord == null)
            {
                // this can happen for basement dungeons
                //Console.WriteLine($"House.Load({houseGuid:X8}): couldn't find slumlord!");
                return null;
            }

            var slumlordGuid = house.SlumLord.Guid.Full;
            var slumlordBiota = DatabaseManager.Shard.BaseDatabase.GetBiota(slumlordGuid);
            if (slumlordBiota != null)
            {
                var slumlord = WorldObjectFactory.CreateWorldObject(slumlordBiota);
                house.SetLinkProperties(slumlord);

                house.ChildLinks.Remove(house.SlumLord);
                house.ChildLinks.Add(slumlord);
            }
            return house;
        }

        /// <summary>
        /// The client automatically adds this amount of time to the beginning of the current maintenance period
        /// </summary>
        public static TimeSpan RentInterval = TimeSpan.FromDays(30);

        /// <summary>
        /// Returns the beginning of the current maintenance period
        /// </summary>
        public uint GetRentTimestamp(uint purchaseTime)
        {
            // get the purchaseTime -> currentTime offset
            //var purchaseTime = (uint)(owner.HousePurchaseTimestamp ?? 0);

            var currentTime = (uint)Time.GetUnixTime();
            var offset = currentTime - purchaseTime;

            // calculate # of full periods in offset
            var rentIntervalSecs = (uint)RentInterval.TotalSeconds;
            if (IsApartment)
                rentIntervalSecs *= 3;      // apartment maintenance every 90 days
            var periods = offset / rentIntervalSecs;

            // return beginning of current period
            return purchaseTime + (rentIntervalSecs * periods);
        }

        /// <summary>
        /// Returns the end of the current maintenance period
        /// </summary>
        public uint GetRentDue(uint purchaseTime)
        {
            var currentPeriod = GetRentTimestamp(purchaseTime);

            var rentIntervalSecs = (uint)RentInterval.TotalSeconds;
            if (IsApartment)
                rentIntervalSecs *= 3;      // apartment maintenance every 90 days

            var rentDue = currentPeriod + rentIntervalSecs;
            return rentDue;
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            // for house dungeons, link to outdoor house properties
            var house = this;
            if (CurrentLandblock != null && CurrentLandblock.HasDungeon && HouseType != HouseType.Apartment)
            {
                var biota = DatabaseManager.Shard.BaseDatabase.GetBiotasByWcid(WeenieClassId).Where(bio => bio.BiotaPropertiesPosition.Count > 0).FirstOrDefault(b => b.BiotaPropertiesPosition.FirstOrDefault(p => p.PositionType == (ushort)PositionType.Location).ObjCellId >> 16 != Location.Landblock);
                if (biota != null)
                {
                    house = WorldObjectFactory.CreateWorldObject(biota) as House;
                    HouseOwner = house.HouseOwner;
                    HouseOwnerName = house.HouseOwnerName;
                }
            }

            //Console.WriteLine($"House.SetLinkProperties({wo.Name}) (0x{wo.Guid}): WeenieType {wo.WeenieType} | HouseId:{house.HouseId} | HouseOwner: {house.HouseOwner} | HouseOwnerName: {house.HouseOwnerName}");

            wo.HouseId = house.HouseId;
            wo.HouseOwner = house.HouseOwner;
            //wo.HouseInstance = house.HouseInstance;
            //wo.HouseOwnerName = house.HouseOwnerName;

            if (house.HouseOwner != null && wo is SlumLord)
                wo.CurrentMotionState = new Motion(MotionStance.Invalid, MotionCommand.On);

            // the inventory items haven't been loaded yet
            if (wo is Hook hook)
            {
                if (hook.HasItem)
                {
                    hook.NoDraw = false;
                    hook.UiHidden = false;
                    hook.Ethereal = false;
                }
                else if (!(house.HouseHooksVisible ?? true))
                {
                    hook.NoDraw = true;
                    hook.UiHidden = true;
                    hook.Ethereal = true;
                }
            }

            //if (wo.IsLinkSpot)
            //{
            //    var housePortals = GetHousePortals();
            //    if (housePortals.Count == 0)
            //    {
            //        Console.WriteLine($"{Name}.SetLinkProperties({wo.Name}): found LinkSpot, but empty HousePortals");
            //        return;
            //    }
            //    var i = housePortals[0];
            //    var destination = new Position(i.ObjCellId, new Vector3(i.OriginX, i.OriginY, i.OriginZ), new Quaternion(i.AnglesX, i.AnglesY, i.AnglesZ, i.AnglesW));

            //    wo.SetPosition(PositionType.Destination, destination);
            //}

            //if (HouseOwner != null)
            //Console.WriteLine($"{Name}.SetLinkProperties({wo.Name}) - houseID: {HouseId:X8}, owner: {HouseOwner:X8}, instance: {HouseInstance:X8}");
        }

        public override void UpdateLinkProperties(WorldObject wo)
        {
            if (wo.HouseOwner != HouseOwner)
            {
                //Console.WriteLine($"{Name}.UpdateLinkProperties({wo.Name} - {wo.Guid}) - HouseOwner: {HouseOwner:X8}");
                wo.EnqueueBroadcast(new GameMessagePublicUpdateInstanceID(wo, PropertyInstanceId.HouseOwner, new ObjectGuid(HouseOwner ?? 0)));
            }

            SetLinkProperties(wo);
        }

        public bool IsApartment => HouseType == HouseType.Apartment;

        /// <summary>
        /// Returns TRUE if this player has guest or storage access
        /// </summary>
        public bool HasPermission(Player player, bool storage = false)
        {
            if (HouseOwner == null)
                return false;

            if (player.Guid.Full == HouseOwner.Value)
                return true;

            var owner = PlayerManager.FindByGuid(HouseOwner.Value);

            if (owner != null && owner.Account != null && owner.Account.AccountId == player.Account.AccountId)
                return true;

            // handle allegiance permissions
            if (MonarchId != null && player.Allegiance != null && player.Allegiance.MonarchId == MonarchId)
            {
                if (storage)
                {
                    if (StorageAccess.Contains(new ObjectGuid(MonarchId.Value)))
                        return true;
                }
                else
                {
                    if (Guests.ContainsKey(new ObjectGuid(MonarchId.Value)))
                        return true;
                }
            }

            if (storage)
                return StorageAccess.Contains(player.Guid);
            else
                return OpenToEveryone || Guests.ContainsKey(player.Guid);
        }

        public bool? HouseHooksVisible
        {
            get => GetProperty(PropertyBool.HouseHooksVisible);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.HouseHooksVisible); else SetProperty(PropertyBool.HouseHooksVisible, value.Value); }
        }

        public void BuildGuests()
        {
            Guests = new Dictionary<ObjectGuid, bool>();

            var housePermissions = Biota.CloneHousePermissions(BiotaDatabaseLock);

            var deleted = new List<uint>();

            foreach (var kvp in housePermissions)
            {
                var player = PlayerManager.FindByGuid(kvp.Key);
                if (player == null)
                {
                    //Console.WriteLine($"{Name}.BuildGuests(): couldn't find guest {kvp.Key:X8}");
                    log.Warn($"[HOUSE] {Name}.BuildGuests(): couldn't find guest {kvp.Key:X8}");

                    // character has been deleted -- automatically remove?
                    deleted.Add(kvp.Key);
                    continue;
                }
                Guests.Add(player.Guid, kvp.Value);
            }

            if (deleted.Count > 0)
            {
                foreach (var guid in deleted)
                    Biota.RemoveHouseGuest(guid, BiotaDatabaseLock);

                ChangesDetected = true;

                SaveBiotaToDatabase();
            }
        }

        public void AddGuest(IPlayer guest, bool storage)
        {
            Biota.AddOrUpdateHouseGuest(guest.Guid.Full, storage, BiotaDatabaseLock);
            ChangesDetected = true;

            BuildGuests();
            UpdateRestrictionDB();

            if (CurrentLandblock == null)
                SaveBiotaToDatabase();
        }

        public void ModifyGuest(IPlayer guest, bool storage)
        {
            var existingStorage = Biota.GetHouseGuestStoragePermission(guest.Guid.Full, BiotaDatabaseLock);

            if (existingStorage == null)
            {
                //Console.WriteLine($"{Name}.FindGuest({guest.Guid}): couldn't find {guest.Name}");
                log.Warn($"[HOUSE] {Name}.FindGuest({guest.Guid}): couldn't find {guest.Name}");

                return;
            }

            if (existingStorage == storage)
                return;

            Biota.AddOrUpdateHouseGuest(guest.Guid.Full, storage, BiotaDatabaseLock);
            ChangesDetected = true;

            BuildGuests();
            UpdateRestrictionDB();

            if (CurrentLandblock == null)
                SaveBiotaToDatabase();
        }

        public void RemoveGuest(IPlayer guest)
        {
            if (!Biota.RemoveHouseGuest(guest.Guid.Full, BiotaDatabaseLock))
                return;

            ChangesDetected = true;

            BuildGuests();
            UpdateRestrictionDB();

            if (CurrentLandblock == null)
                SaveBiotaToDatabase();
        }

        public void ClearPermissions()
        {
            foreach (var guest in Guests.Keys)
            {
                var player = PlayerManager.FindByGuid(guest);
                if (player == null)
                {
                    //Console.WriteLine($"{Name}.ClearPermissions(): couldn't find {guest}");
                    log.Warn($"[HOUSE] {Name}.ClearPermissions(): couldn't find {guest}");
                    continue;
                }
                RemoveGuest(player);
            }
            OpenToEveryone = true;
        }

        /// <summary>
        /// Returns the database HousePortals for a HouseID
        /// </summary>
        public List<Database.Models.World.HousePortal> GetHousePortals()
        {
            // the database house portals are different from the HousePortal weenie objects
            // the db info contains the portal destinations

            return DatabaseManager.World.GetCachedHousePortals(HouseId.Value);
        }

        public bool HasDungeon => HousePortal != null;

        private uint? _dungeonLandblockID;

        public uint DungeonLandblockID
        {
            get
            {
                if (_dungeonLandblockID == null)
                {
                    var rootHouseBlock = RootHouse.Location.LandblockId.Raw | 0xFFFF;

                    var housePortals = GetHousePortals();

                    var dungeonPortal = housePortals.FirstOrDefault(i => (i.ObjCellId | 0xFFFF) != rootHouseBlock);

                    if (dungeonPortal == null)
                        return 0;

                    _dungeonLandblockID = dungeonPortal.ObjCellId | 0xFFFF;
                }
                return _dungeonLandblockID.Value;
            }
        }

        private uint? _dungeonHouseGuid;

        public uint DungeonHouseGuid
        {
            get
            {
                if (_dungeonHouseGuid == null)
                {
                    if (DungeonLandblockID == 0)
                        return 0;

                    var landblock = (ushort)((DungeonLandblockID >> 16) & 0xFFFF);

                    var basementGuid = DatabaseManager.World.GetCachedBasementHouseGuid(landblock);

                    if (basementGuid == 0)
                        return 0;

                    _dungeonHouseGuid = basementGuid;

                }
                return _dungeonHouseGuid.Value;
            }
        }

        //private House _rootHouse;

        /// <summary>
        /// For villas and mansions, the basement dungeons contain their own House weenie
        /// This dungeon House needs to reference the main outdoor house for various operations,
        /// such as returning the permissions list.
        /// </summary>
        public House RootHouse
        {
            get
            {
                //if (HouseType == ACE.Entity.Enum.HouseType.Apartment || HouseType == ACE.Entity.Enum.HouseType.Cottage)
                    //return this;

                var landblock = (ushort)((RootGuid.Full >> 12) & 0xFFFF);

                var landblockId = new LandblockId((uint)(landblock << 16 | 0xFFFF));
                var isLoaded = LandblockManager.IsLoaded(landblockId);

                if (!isLoaded)
                {
                    //if (_rootHouse == null)
                    //_rootHouse = Load(RootGuid.Full);

                    //return _rootHouse;  // return offline copy
                    // do not cache, in case permissions have changed
                    return Load(RootGuid.Full);
                }
                   
                var loaded = LandblockManager.GetLandblock(landblockId, false);
                return loaded.GetObject(RootGuid) as House;
            }
        }

        private ObjectGuid? _rootGuid;

        public ObjectGuid RootGuid
        {
            get
            {
                if (_rootGuid == null)
                {
                    if (HouseCell.RootGuids.TryGetValue(Guid.Full, out var rootGuid))
                        _rootGuid = new ObjectGuid(rootGuid);
                    else
                    {
                        log.Error($"House.RootGuid - couldn't find root guid for house guid {Guid}");
                        _rootGuid = Guid;
                    }
                }
                return _rootGuid.Value;
            }
        }

        public bool? GetAllegianceAccessLevel()
        {
            if (MonarchId == null)
                return null;

            if (!Guests.TryGetValue(new ObjectGuid(MonarchId.Value), out bool storage))
                return null;

            return storage;
        }

        public static House GetHouse(uint houseGuid)
        {
            var landblock = (ushort)((houseGuid >> 12) & 0xFFFF);

            var landblockId = new LandblockId((uint)(landblock << 16 | 0xFFFF));
            var isLoaded = LandblockManager.IsLoaded(landblockId);

            if (!isLoaded)
                return House.Load(houseGuid);

            var loaded = LandblockManager.GetLandblock(landblockId, false);
            return loaded.GetObject(new ObjectGuid(houseGuid)) as House;
        }

        public House GetDungeonHouse()
        {
            var landblockId = new LandblockId(DungeonLandblockID);
            var isLoaded = LandblockManager.IsLoaded(landblockId);

            if (!isLoaded)
                return House.Load(DungeonHouseGuid, true);

            var loaded = LandblockManager.GetLandblock(landblockId, false);
            var wos = loaded.GetWorldObjectsForPhysicsHandling();
            return wos.FirstOrDefault(wo => wo.WeenieClassId == WeenieClassId) as House;
        }

        public bool OnProperty(Player player)
        {
            if (Location == null)
                return false;

            if (HouseType == HouseType.Apartment)
                return player.Location.Cell == Location.Cell;

            if (player.Location.GetOutdoorCell() == Location.GetOutdoorCell())
                return true;

            foreach (var linkedHouse in LinkedHouses)
                if (player.Location.GetOutdoorCell() == linkedHouse.Location.GetOutdoorCell())
                    return true;

            if (HasDungeon)
            {
                if ((player.Location.Cell | 0xFFFF) == DungeonLandblockID && (player.Location.Cell & 0xFFFF) >= 0x100)
                    return true;
            }
            return false;
        }

        public int BootAll(Player booter, bool guests = true, bool allegianceHouse = false)
        {
            var players = PlayerManager.GetAllOnline();

            var booted = 0;
            foreach (var player in players)
            {
                // exclude booter
                if (player.Equals(booter)) continue;

                if (!OnProperty(player)) continue;

                // keep guests if closing house
                if (!guests && HasPermission(player, false))
                    continue;

                booter.HandleActionBoot(player.Name, allegianceHouse);
                booted++;
            }
            return booted;
        }

        public void UpdateRestrictionDB(RestrictionDB restrictions = null)
        {
            // get restrictions for root house
            if (restrictions == null) restrictions = new RestrictionDB(this);

            SendRestrictionDB(restrictions);

            // for mansions, update the linked houses
            foreach (var linkedHouse in LinkedHouses)
                linkedHouse.SendRestrictionDB(restrictions);

            // update house dungeon

            if (HasDungeon)
            {
                var dungeonHouse = GetDungeonHouse();
                if (dungeonHouse == null || dungeonHouse.PhysicsObj == null) return;

                dungeonHouse.SendRestrictionDB(restrictions);
            }
        }

        public void SendRestrictionDB(RestrictionDB restrictions)
        {
            if (PhysicsObj == null)
                return;

            var nearbyPlayers = PhysicsObj.ObjMaint.GetKnownPlayersValuesAsPlayer();
            foreach (var player in nearbyPlayers)
                player.Session.Network.EnqueueSend(new GameMessagePublicUpdateInstanceID(this, PropertyInstanceId.HouseOwner, new ObjectGuid(restrictions.HouseOwner)),
                                                   new GameEventHouseUpdateRestrictions(player.Session, this, restrictions));
        }

        public void ClearRestrictions()
        {
            if (PhysicsObj == null) return;

            var restrictionDB = new RestrictionDB();

            UpdateRestrictionDB(restrictionDB);
        }

        public bool OpenToEveryone
        {
            get => (GetProperty(PropertyInt.OpenToEveryone) ?? 0) == 1;
            set { if (!value) RemoveProperty(PropertyInt.OpenToEveryone); else SetProperty(PropertyInt.OpenToEveryone, 1); }
        }

        public int HouseMaxHooksUsable
        {
            get => GetProperty(PropertyInt.HouseMaxHooksUsable) ?? 25;
            set { if (value == 25) RemoveProperty(PropertyInt.HouseMaxHooksUsable); else SetProperty(PropertyInt.HouseMaxHooksUsable, value); }
        }

        public int HouseCurrentHooksUsable
        {
            get => GetProperty(PropertyInt.HouseCurrentHooksUsable) ?? HouseMaxHooksUsable;
            set { if (value == HouseMaxHooksUsable) RemoveProperty(PropertyInt.HouseCurrentHooksUsable); else SetProperty(PropertyInt.HouseCurrentHooksUsable, value); }
        }

        public static Dictionary<HouseType, Dictionary<HookGroupType, int>> HookGroupLimits = new Dictionary<HouseType, Dictionary<HookGroupType, int>>()
        {
            { HouseType.Undef, new Dictionary<HookGroupType, int> {
                { HookGroupType.Undef,                          -1 },
                { HookGroupType.NoisemakingItems,               -1 },
                { HookGroupType.TestItems,                      -1 },
                { HookGroupType.PortalItems,                    -1 },
                { HookGroupType.WritableItems,                  -1 },
                { HookGroupType.SpellCastingItems,              -1 },
                { HookGroupType.SpellTeachingItems,             -1 } }
            },
            { HouseType.Cottage, new Dictionary<HookGroupType, int> {
                { HookGroupType.Undef,                          -1 },
                { HookGroupType.NoisemakingItems,               -1 },
                { HookGroupType.TestItems,                      -1 },
                { HookGroupType.PortalItems,                    -1 },
                { HookGroupType.WritableItems,                   1 },
                { HookGroupType.SpellCastingItems,               5 },
                { HookGroupType.SpellTeachingItems,              0 } }
            },
            { HouseType.Villa, new Dictionary<HookGroupType, int> {
                { HookGroupType.Undef,                          -1 },
                { HookGroupType.NoisemakingItems,               -1 },
                { HookGroupType.TestItems,                      -1 },
                { HookGroupType.PortalItems,                    -1 },
                { HookGroupType.WritableItems,                   1 },
                { HookGroupType.SpellCastingItems,              10 },
                { HookGroupType.SpellTeachingItems,              0 } }
            },
            { HouseType.Mansion, new Dictionary<HookGroupType, int> {
                { HookGroupType.Undef,                          -1 },
                { HookGroupType.NoisemakingItems,               -1 },
                { HookGroupType.TestItems,                      -1 },
                { HookGroupType.PortalItems,                    -1 },
                { HookGroupType.WritableItems,                   3 },
                { HookGroupType.SpellCastingItems,              15 },
                { HookGroupType.SpellTeachingItems,              1 } }
            },
            { HouseType.Apartment, new Dictionary<HookGroupType, int> {
                { HookGroupType.Undef,                          -1 },
                { HookGroupType.NoisemakingItems,               -1 },
                { HookGroupType.TestItems,                      -1 },
                { HookGroupType.PortalItems,                     0 },
                { HookGroupType.WritableItems,                   0 },
                { HookGroupType.SpellCastingItems,              -1 },
                { HookGroupType.SpellTeachingItems,              0 } }
            }
        };

        public int GetHookGroupCurrentCount(HookGroupType hookGroupType) => Hooks.Count(h => h.HasItem && (h.Item?.HookGroup ?? HookGroupType.Undef) == hookGroupType);

        public int GetHookGroupMaxCount(HookGroupType hookGroupType) => HookGroupLimits[HouseType][hookGroupType];
    }
}
