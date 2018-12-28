using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.Structure;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class House: WorldObject
    {
        public Dictionary<IPlayer, bool> Guests;

        public static int MaxGuests = 32;

        /// <summary>
        /// house open/closed status
        /// 0 = closed, 1 = open
        /// </summary>
        public bool OpenStatus
        {
            get => Convert.ToBoolean(HouseStatus);
            set => HouseStatus = Convert.ToInt32(value);
        }

        public SlumLord SlumLord { get => ChildLinks.FirstOrDefault(l => l as SlumLord != null) as SlumLord; }
        public List<Hook> Hooks { get => ChildLinks.Where(l => l is Hook).Select(l => l as Hook).ToList(); }
        public List<Storage> Storage { get => ChildLinks.Where(l => l is Storage).Select(l => l as Storage).ToList(); }
        public WorldObject BootSpot => ChildLinks.FirstOrDefault(i => i.WeenieType == WeenieType.BootSpot);

        public HashSet<IPlayer> StorageAccess => Guests.Where(i => i.Value).Select(i => i.Key).ToHashSet();

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
        }

        /// <summary>
        /// Builds a HouseData structure for this house
        /// </summary>
        public HouseData GetHouseData(Player owner)
        {
            var houseData = new HouseData();
            houseData.Position = Location;
            houseData.Type = (HouseType)HouseType;

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
                houseData.RentTime = GetRentTimestamp(owner);
            }
            return houseData;
        }

        /// <summary>
        /// The client automatically adds this amount of time to the beginning of the current maintenance period
        /// </summary>
        public static TimeSpan RentInterval = TimeSpan.FromDays(30);

        /// <summary>
        /// Returns the beginning of the current maintenance period
        /// </summary>
        public uint GetRentTimestamp(Player owner)
        {
            // get the purchaseTime -> currentTime offset
            var purchaseTime = (uint)(owner.HousePurchaseTimestamp ?? 0);

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

        public override void SetLinkProperties(WorldObject wo)
        {
            wo.HouseId = HouseId;
            wo.HouseOwner = HouseOwner;
            wo.HouseInstance = HouseInstance;

            if (HouseOwner != null && wo is SlumLord)
                wo.CurrentMotionState = new Motion(MotionStance.Invalid, MotionCommand.On);

            // the inventory items haven't been loaded yet
            if (wo is Hook && !(HouseHooksVisible ?? true))
            {
                wo.NoDraw = true;
                wo.UiHidden = true;
            }

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

        public bool IsApartment => HouseType != null && (HouseType)HouseType.Value == ACE.Entity.Enum.HouseType.Apartment;

        /// <summary>
        /// Returns TRUE if this player has storage access
        /// </summary>
        public bool HasPermission(Player player)
        {
            if (HouseOwner == null)
                return false;

            if (player.Guid.Full == HouseOwner.Value)
                return true;

            return StorageAccess.Contains(player);
        }

        public bool? HouseHooksVisible
        {
            get => GetProperty(PropertyBool.HouseHooksVisible);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.HouseHooksVisible); else SetProperty(PropertyBool.HouseHooksVisible, value.Value); }
        }

        public void BuildGuests()
        {
            Guests = new Dictionary<IPlayer, bool>();

            var housePermissions = Biota.GetHousePermission(BiotaDatabaseLock);

            foreach (var housePermission in Biota.HousePermission)
            {
                var player = PlayerManager.FindByGuid(housePermission.PlayerGuid);
                if (player == null)
                {
                    Console.WriteLine($"{Name}.BuildGuests(): couldn't find guest {housePermission.PlayerGuid}");
                    continue;
                }
                Guests.Add(player, housePermission.Storage);
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
        }

        public void UpdateGuest(IPlayer guest, bool storage)
        {
            var existing = FindGuest(guest);

            if (existing == null || existing.Storage == storage)
                return;

            existing.Storage = storage;
            ChangesDetected = true;
        }

        public void RemoveGuest(IPlayer guest)
        {
            Biota.TryRemoveHousePermission(guest.Guid.Full, out var entity, BiotaDatabaseLock);
            ChangesDetected = true;
        }

        public HousePermission FindGuest(IPlayer guest)
        {
            var housePermissions = Biota.GetHousePermission(BiotaDatabaseLock);

            var existing = housePermissions.FirstOrDefault(i => i.PlayerGuid == guest.Guid.Full);

            if (existing == null)
                Console.WriteLine($"{Name}.FindGuest({guest.Guid}): couldn't find {guest.Name}");

            return existing;
        }
    }
}
