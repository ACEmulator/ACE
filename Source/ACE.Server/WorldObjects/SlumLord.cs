using System;
using System.Collections.Generic;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    public class SlumLord : Container
    {
        /// <summary>
        /// The house this slumlord is linked to
        /// </summary>
        public House House { get => ParentLink as House; }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public SlumLord(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public SlumLord(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            ItemCapacity = 120;
        }

        public bool HouseRequiresMonarch
        {
            get => GetProperty(PropertyBool.HouseRequiresMonarch) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.HouseRequiresMonarch); else SetProperty(PropertyBool.HouseRequiresMonarch, value); }
        }

        public int? AllegianceMinLevel
        {
            get => GetProperty(PropertyInt.AllegianceMinLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AllegianceMinLevel); else SetProperty(PropertyInt.AllegianceMinLevel, value.Value); }
        }

        public override void ActOnUse(WorldObject worldObject)
        {
            //Console.WriteLine($"SlumLord.ActOnUse({worldObject.Name})");

            var player = worldObject as Player;
            if (player == null) return;

            // sent house profile
            var houseProfile = GetHouseProfile();

            player.Session.Network.EnqueueSend(new GameEventHouseProfile(player.Session, Guid, houseProfile));
        }

        public HouseProfile GetHouseProfile()
        {
            var houseProfile = new HouseProfile();

            houseProfile.DwellingID = HouseId.Value;

            if (House != null)
            {
                houseProfile.Type = House.HouseType;

                if (House.HouseStatus == HouseStatus.Disabled)
                    houseProfile.Bitmask &= ~HouseBitfield.Active;

                if (House.HouseStatus == HouseStatus.InActive)
                    houseProfile.MaintenanceFree = true;
            }

            if (HouseRequiresMonarch)
                houseProfile.Bitmask |= HouseBitfield.RequiresMonarch;

            if (MinLevel != null)
                houseProfile.MinLevel = MinLevel.Value;

            if (AllegianceMinLevel != null)
                houseProfile.MinAllegRank = AllegianceMinLevel.Value;

            if (HouseOwner != null)
            {
                var ownerId = HouseOwner.Value;
                var owner = PlayerManager.FindByGuid(ownerId);

                houseProfile.OwnerID = new ObjectGuid(ownerId);
                houseProfile.OwnerName = owner?.Name;
            }

            houseProfile.SetBuyItems(GetBuyItems());
            houseProfile.SetRentItems(GetRentItems());
            houseProfile.SetPaidItems(this);

            return houseProfile;
        }

        /// <summary>
        /// Returns the list of items required to purchase this dwelling
        /// </summary>
        public List<WorldObject> GetBuyItems()
        {
            var buyList = GetCreateListForSlumLord(DestinationType.HouseBuy);

            buyList.ForEach(item => item.Destroy(false));

            return buyList;
        }

        /// <summary>
        /// Returns the list of items required to rent this dwelling
        /// </summary>
        public List<WorldObject> GetRentItems()
        {
            var rentList = GetCreateListForSlumLord(DestinationType.HouseRent);

            rentList.ForEach(item => item.Destroy(false));

            return rentList;
        }

        /// <summary>
        /// Returns TRUE if rent is already paid for current maintenance period
        /// </summary>
        public bool IsRentPaid()
        {
            if (House != null && House.HouseStatus == HouseStatus.InActive)
                return true;

            var houseProfile = GetHouseProfile();

            foreach (var rentItem in houseProfile.Rent)
            {
                if (rentItem.Paid < rentItem.Num)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns TRUE if this player has the minimum requirements to purchase / rent this house
        /// </summary>
        public bool HasRequirements(Player player)
        {
            if (!PropertyManager.GetBool("house_purchase_requirements").Item)
                return true;

            if (AllegianceMinLevel == null)
                return true;

            var allegianceMinLevel = PropertyManager.GetLong("mansion_min_rank", -1).Item;
            if (allegianceMinLevel == -1)
                allegianceMinLevel = AllegianceMinLevel.Value;

            if (allegianceMinLevel > 0 && (player.Allegiance == null || player.AllegianceNode.Rank < allegianceMinLevel))
            {
                Console.WriteLine($"{Name}.HasRequirements({player.Name}) - allegiance rank {player.AllegianceNode?.Rank ?? 0} < {allegianceMinLevel}");
                return false;
            }
            return true;
        }

        public int GetAllegianceMinLevel()
        {
            if (AllegianceMinLevel == null)
                return 0;

            var allegianceMinLevel = PropertyManager.GetLong("mansion_min_rank", -1).Item;
            if (allegianceMinLevel == -1)
                allegianceMinLevel = AllegianceMinLevel.Value;

            return (int)allegianceMinLevel;
        }

        protected override void OnInitialInventoryLoadCompleted()
        {
            HouseManager.OnInitialInventoryLoadCompleted(this);
        }

        public void On()
        {
            var on = new Motion(MotionStance.Invalid, MotionCommand.On);

            SetAndBroadcastMotion(on);
        }

        public void Off()
        {
            var off = new Motion(MotionStance.Invalid, MotionCommand.Off);

            if (CurrentLandblock != null)
                SetAndBroadcastMotion(off);
        }

        private void SetAndBroadcastMotion(Motion motion)
        {
            CurrentMotionState = motion;
            EnqueueBroadcastMotion(motion);
        }

        public void SetAndBroadcastName(string houseOwnerName = null)
        {
            if (string.IsNullOrWhiteSpace(houseOwnerName))
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(WeenieClassId);

                if (weenie != null)
                    Name = weenie.GetProperty(PropertyString.Name);
                else
                    Name = House.HouseType.ToString();
            }
            else
                Name = $"{houseOwnerName}'s {Name}";

            if (CurrentLandblock != null)
                EnqueueBroadcast(new GameMessagePublicUpdatePropertyString(this, PropertyString.Name, Name));
        }

        /// <summary>
        /// This event is raised when HouseManager removes item for rent
        /// </summary>
        protected override void OnRemoveItem(WorldObject removedItem)
        {
            //Console.WriteLine("Slumlord.OnRemoveItem()");

            // Here we explicitly remove the payment from the database to avoid storing unneeded objects and free guid.
            if (!removedItem.IsDestroyed)
                removedItem.Destroy();
        }
    }
}
