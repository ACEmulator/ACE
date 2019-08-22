using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
        public Dictionary<ObjectGuid, bool> Guests;

        public static int MaxGuests = 128;

        /// <summary>
        /// house open/closed status
        /// 0 = closed, 1 = open
        /// </summary>
        public bool OpenStatus { get => IsOpen; set => IsOpen = value; }

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
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public House(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            DefaultScriptId = (uint)ACE.Entity.Enum.PlayScript.RestrictionEffectBlue;

            BuildGuests();

            LinkedHouses = new List<House>();
        }

        /// <summary>
        /// Builds a HouseData structure for this house
        /// This is used to populate the info in the House panel
        /// </summary>
        public HouseData GetHouseData(Player owner)
        {
            var houseData = new HouseData();
            houseData.Position = Location;
            houseData.Type = HouseType;

            if (SlumLord == null)
            {
                Console.WriteLine($"No slumlord found for {Name} ({Guid})");
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

        public static House Load(uint houseGuid)
        {
            var landblock = (ushort)((houseGuid >> 12) & 0xFFFF);

            var biota = DatabaseManager.Shard.GetBiota(houseGuid);
            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblock);

            if (biota == null)
            {
                if (instances != null)
                {
                    var houseInstance = instances.Where(h => h.Guid == houseGuid).FirstOrDefault();

                    if (houseInstance != null)
                        biota = WorldObjectFactory.CreateWorldObject(DatabaseManager.World.GetCachedWeenie(houseInstance.WeenieClassId), new ObjectGuid(houseInstance.Guid)).Biota;
                }
            }

            var linkedHouses = WorldObjectFactory.CreateNewWorldObjects(instances, new List<Biota>() { biota }, biota.WeenieClassId);

            foreach (var linkedHouse in linkedHouses)
                linkedHouse.ActivateLinks(instances, new List<Biota>() { biota }, linkedHouses[0]);

            var house = (House)linkedHouses[0];

            // load slumlord biota for rent
            if (house.SlumLord == null)
            {
                // this can happen for basement dungeons
                //Console.WriteLine($"House.Load({houseGuid:X8}): couldn't find slumlord!");
                return null;
            }

            var slumlordGuid = house.SlumLord.Guid.Full;
            var slumlordBiota = DatabaseManager.Shard.GetBiota(slumlordGuid);
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
            if (CurrentLandblock != null && CurrentLandblock.IsDungeon && HouseType != HouseType.Apartment)
            {
                var biota = DatabaseManager.Shard.GetBiotasByWcid(WeenieClassId).FirstOrDefault(b => b.BiotaPropertiesPosition.FirstOrDefault(p => p.PositionType == (ushort)PositionType.Location).ObjCellId >> 16 != Location.Landblock);
                if (biota != null)
                {
                    house = WorldObjectFactory.CreateWorldObject(biota) as House;
                    HouseOwner = house.HouseOwner;
                }
            }

            wo.HouseId = house.HouseId;
            wo.HouseOwner = house.HouseOwner;
            wo.HouseInstance = house.HouseInstance;

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
                else if (!(HouseHooksVisible ?? true))
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

            if (owner != null && owner.Account.AccountId == player.Account.AccountId)
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
                return Guests.ContainsKey(player.Guid);
        }

        public bool? HouseHooksVisible
        {
            get => GetProperty(PropertyBool.HouseHooksVisible);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.HouseHooksVisible); else SetProperty(PropertyBool.HouseHooksVisible, value.Value); }
        }

        public void BuildGuests()
        {
            Guests = new Dictionary<ObjectGuid, bool>();

            var housePermissions = Biota.GetHousePermission(BiotaDatabaseLock);

            foreach (var housePermission in Biota.HousePermission)
            {
                var player = PlayerManager.FindByGuid(housePermission.PlayerGuid);
                if (player == null)
                {
                    Console.WriteLine($"{Name}.BuildGuests(): couldn't find guest {housePermission.PlayerGuid:X8}");
                    continue;
                }
                Guests.Add(player.Guid, housePermission.Storage);
            }
        }

        public void AddGuest(IPlayer guest, bool storage)
        {
            var housePermission = new HousePermission();
            housePermission.HouseId = Guid.Full;
            housePermission.PlayerGuid = guest.Guid.Full;
            housePermission.Storage = storage;

            Biota.AddHousePermission(housePermission, BiotaDatabaseLock);
            ChangesDetected = true;

            BuildGuests();
            UpdateRestrictionDB();

            if (CurrentLandblock == null)
                SaveBiotaToDatabase();
        }

        public void ModifyGuest(IPlayer guest, bool storage)
        {
            var existing = FindGuest(guest);

            if (existing == null || existing.Storage == storage)
                return;

            existing.Storage = storage;
            ChangesDetected = true;

            BuildGuests();
            UpdateRestrictionDB();

            if (CurrentLandblock == null)
                SaveBiotaToDatabase();
        }

        public void RemoveGuest(IPlayer guest)
        {
            Biota.TryRemoveHousePermission(guest.Guid.Full, out var entity, BiotaDatabaseLock);
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
                    Console.WriteLine($"{Name}.ClearPermissions(): couldn't find {guest}");
                    continue;
                }
                RemoveGuest(player);
            }
        }

        public HousePermission FindGuest(IPlayer guest)
        {
            var housePermissions = Biota.GetHousePermission(BiotaDatabaseLock);

            var existing = housePermissions.FirstOrDefault(i => i.PlayerGuid == guest.Guid.Full);

            if (existing == null)
                Console.WriteLine($"{Name}.FindGuest({guest.Guid}): couldn't find {guest.Name}");

            return existing;
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
                    var housePortals = GetHousePortals();

                    if (housePortals.Count == 0)
                        return 0;

                    _dungeonHouseGuid = housePortals[0].Id;

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
                if (HouseType == HouseType.Apartment || HouseType == HouseType.Cottage)
                {
                    _rootGuid = Guid;
                    return Guid;
                }

                if (_rootGuid != null)
                    return _rootGuid.Value;

                // CurrentLandblock == null should only happen when the player is in a villa/mansion dungeon basement,
                // and the outdoor house landblock is still unloaded. the reference to the outdoor House will be a shallow reference at that point,
                // and this should only happen for outdoor landblocks

                if (CurrentLandblock == null || !CurrentLandblock.IsDungeon)
                {
                    _rootGuid = Guid;
                    return Guid;
                }

                var biota = DatabaseManager.Shard.GetBiotasByWcid(WeenieClassId).FirstOrDefault(b => b.BiotaPropertiesPosition.FirstOrDefault(p => p.PositionType == (ushort)PositionType.Location).ObjCellId >> 16 != Location?.Landblock);
                if (biota == null)
                {
                    var instance = DatabaseManager.World.GetLandblockInstancesByWcid(WeenieClassId).FirstOrDefault(w => w.ObjCellId >> 16 != Location?.Landblock);
                    if (instance != null)
                    {
                        _rootGuid = new ObjectGuid(instance.Guid);
                        return _rootGuid.Value;
                    }

                    Console.WriteLine($"{Name}.RootGuid: couldn't find root guid for {WeenieClassId} on landblock {Location.Landblock:X8}");

                    _rootGuid = Guid;
                    return Guid;
                }

                _rootGuid = new ObjectGuid(biota.Id);
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
                return null;

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
                if ((player.Location.Cell | 0xFFFF) == DungeonLandblockID)
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

        public void UpdateRestrictionDB()
        {
            // get restrictions for root house
            var restrictions = new RestrictionDB(this);

            UpdateRestrictionDB(restrictions);

            // for mansions, update the linked houses
            foreach (var linkedHouse in LinkedHouses)
                linkedHouse.UpdateRestrictionDB(restrictions);

            // update house dungeon

            // TODO: handle this more gracefully: player in house dungeon,
            // but outdoor house landblock is unloaded, and player is evicted
            if (CurrentLandblock != null && HasDungeon)
            {
                var dungeonHouse = GetDungeonHouse();
                if (dungeonHouse == null || dungeonHouse.PhysicsObj == null) return;

                dungeonHouse.UpdateRestrictionDB(restrictions);
            }
        }

        public void UpdateRestrictionDB(RestrictionDB restrictions)
        {
            if (PhysicsObj == null)
                return;

            var nearbyPlayers = PhysicsObj.ObjMaint.KnownPlayers.Values.Select(v => v.WeenieObj.WorldObject).OfType<Player>().ToList();
            foreach (var player in nearbyPlayers)
                player.Session.Network.EnqueueSend(new GameEventHouseUpdateRestrictions(player.Session, this, restrictions));
        }

        public void ClearRestrictions()
        {
            if (PhysicsObj == null) return;

            var restrictionDB = new RestrictionDB();

            var nearbyPlayers = PhysicsObj.ObjMaint.KnownPlayers.Values.Select(v => v.WeenieObj.WorldObject).OfType<Player>().ToList();
            foreach (var player in nearbyPlayers)
            {
                // clear house owner
                player.Session.Network.EnqueueSend(new GameMessagePublicUpdateInstanceID(this, PropertyInstanceId.HouseOwner, ObjectGuid.Invalid));
                player.Session.Network.EnqueueSend(new GameEventHouseUpdateRestrictions(player.Session, this, restrictionDB));
            }
        }
    }
}
