using System;
using System.Collections.Generic;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    public class SlumLord : WorldObject
    {
        /// <summary>
        /// The house this slumlord is linked to
        /// </summary>
        public WorldObject House { get => ParentLink; }

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

        /// <summary>
        /// Verifies the use requirements for the Slumlord
        /// </summary>
        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            if (!(activator is Player player))
                return new ActivationResult(false);

            if (!PropertyManager.GetBool("house_purchase_requirements").Item)
                return new ActivationResult(true);

            // ensure player doesn't already own a house?

            var baseRequirements = base.CheckUseRequirements(activator);
            if (!baseRequirements.Success)
                return baseRequirements;

            if (MinLevel != null)
            {
                var playerLevel = player.Level ?? 1;
                if (playerLevel < MinLevel)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouMustBeAboveLevel_ToBuyHouse, MinLevel.ToString()));
            }

            if (HouseRequiresMonarch)
            {
                if (player.Allegiance == null || player.Allegiance.MonarchId != player.Guid.Full)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat("You must be a monarch to purchase this dwelling.", ChatMessageType.Broadcast));
                    return new ActivationResult(false);
                }
            }

            if (AllegianceMinLevel != null)
            {
                var allegianceMinLevel = PropertyManager.GetLong("mansion_min_rank", -1).Item;
                if (allegianceMinLevel == -1)
                    allegianceMinLevel = AllegianceMinLevel.Value;

                if (player.Allegiance == null || player.AllegianceNode.Rank < allegianceMinLevel)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouMustBeAboveAllegianceRank_ToBuyHouse, allegianceMinLevel.ToString()));
            }

            return new ActivationResult(true);
        }

        public override void ActOnUse(WorldObject worldObject)
        {
            //Console.WriteLine($"SlumLord.ActOnUse({worldObject.Name})");

            var player = worldObject as Player;
            if (player == null) return;

            // sent house profile
            var houseProfile = new HouseProfile();
            houseProfile.DwellingID = HouseId.Value;
            if (HouseOwner != null)
            {
                var ownerId = HouseOwner.Value;
                var owner = PlayerManager.FindByGuid(ownerId);

                houseProfile.OwnerID = new ObjectGuid(ownerId);
                houseProfile.OwnerName = owner.Name;
            }
            houseProfile.SetBuyItems(GetBuyItems());
            houseProfile.SetRentItems(GetRentItems());

            player.Session.Network.EnqueueSend(new GameEventHouseProfile(player.Session, Guid, houseProfile));
        }

        /// <summary>
        /// Returns the list of items required to purchase this dwelling
        /// </summary>
        public List<WorldObject> GetBuyItems()
        {
            return GetCreateList(DestinationType.HouseBuy);
        }

        /// <summary>
        /// Returns the list of items required to rent this dwelling
        /// </summary>
        public List<WorldObject> GetRentItems()
        {
            return GetCreateList(DestinationType.HouseRent);
        }
    }
}
