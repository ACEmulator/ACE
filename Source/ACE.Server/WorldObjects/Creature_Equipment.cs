using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public bool EquippedObjectsLoaded { get; private set; }

        /// <summary>
        /// Use EquipObject() and DequipObject() to manipulate this dictionary..<para />
        /// Do not manipulate this dictionary directly.
        /// </summary>
        public Dictionary<ObjectGuid, WorldObject> EquippedObjects { get; } = new Dictionary<ObjectGuid, WorldObject>();

        /// <summary>
        /// The only time this should be used is to populate EquippedObjects from the ctor.
        /// </summary>
        protected void AddBiotasToEquippedObjects(IEnumerable<Biota> wieldedItems)
        {
            foreach (var biota in wieldedItems)
            {
                var worldObject = WorldObjectFactory.CreateWorldObject(biota);
                EquippedObjects[worldObject.Guid] = worldObject;

                EncumbranceVal += worldObject.EncumbranceVal;
            }

            EquippedObjectsLoaded = true;
        }

        public bool WieldedLocationIsAvailable(int wieldedLocation)
        {
            // todo actually check and stuff
            return true;
        }

        public bool HasWieldedItem(ObjectGuid objectGuid)
        {
            return EquippedObjects.ContainsKey(objectGuid);
        }

        /// <summary>
        /// Get Wielded Item. Returns null if not found.
        /// </summary>
        public WorldObject GetWieldedItem(ObjectGuid objectGuid)
        {
            return EquippedObjects.TryGetValue(objectGuid, out var item) ? item : null;
        }

        /// <summary>
        /// Returns the currently equipped weapon
        /// </summary>
        public WorldObject GetEquippedWeapon()
        {
            var meleeWeapon = EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.MeleeWeapon);
            var missileWeapon = EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.MissileWeapon);

            return meleeWeapon != null ? meleeWeapon : missileWeapon;
        }

        /// <summary>
        /// Returns the currently equipped shield
        /// </summary>
        /// <returns></returns>
        public WorldObject GetEquippedShield()
        {
            return EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.Shield);
        }

        /// <summary>
        /// Returns the currently equipped missile ammo
        /// </summary>
        /// <returns></returns>
        public WorldObject GetEquippedAmmo()
        {
            return EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.MissileAmmo);
        }

        /// <summary>
        /// This will set the CurrentWieldedLocation property to wieldedLocation and the Wielder property to this guid and will add it to the EquippedObjects dictionary.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        public bool TryEquipObject(WorldObject worldObject, int wieldedLocation)
        {
            if (!WieldedLocationIsAvailable(wieldedLocation))
                return false;

            worldObject.CurrentWieldedLocation = (EquipMask)wieldedLocation;
            worldObject.WielderId = Biota.Id;
            EquippedObjects[worldObject.Guid] = worldObject;

            EncumbranceVal += worldObject.EncumbranceVal;
            Value += worldObject.Value;

            if (CurrentLandblock != null)
                CurrentLandblock.EnqueueActionBroadcast(Location, Landblock.MaxObjectRange, (Player p) => p.TrackObject(this));

            return true;
        }

        /// <summary>
        /// This will remove the Wielder and CurrentWieldedLocation properties on the item and will remove it from the EquippedObjects dictionary.<para />
        /// It does not add it to inventory as you could be unwielding to the ground or a chest. Og II<para />
        /// It will also decrease the EncumbranceVal and Value.
        /// </summary>
        public bool TryDequipObject(ObjectGuid objectGuid)
        {
            return TryDequipObject(objectGuid, out _);
        }

        /// <summary>
        /// This will remove the Wielder and CurrentWieldedLocation properties on the item and will remove it from the EquippedObjects dictionary.<para />
        /// It does not add it to inventory as you could be unwielding to the ground or a chest. Og II<para />
        /// It will also decrease the EncumbranceVal and Value.
        /// </summary>
        public bool TryDequipObject(ObjectGuid objectGuid, out WorldObject worldObject)
        {
            if (EquippedObjects.Remove(objectGuid, out worldObject))
            {
                worldObject.RemoveProperty(PropertyInt.CurrentWieldedLocation);
                worldObject.RemoveProperty(PropertyInstanceId.Wielder);

                EncumbranceVal -= worldObject.EncumbranceVal;
                Value -= worldObject.Value;

                if (CurrentLandblock != null)
                    CurrentLandblock.EnqueueActionBroadcast(Location, Landblock.MaxObjectRange, (Player p) => p.TrackObject(this));

                return true;
            }

            return false;
        }


        /// <summary>
        /// This method sets properties needed for items that will be child items.
        /// Items here are only items equipped in the hands.
        /// This deals with the orientation and positioning for visual appearance of the child items held by the parent. Og II
        /// </summary>
        /// <param name="item">The child item - we link them together</param>
        /// <param name="placementPosition">Where is this on the parent - where is it equipped</param>
        /// <param name="placementId">out parameter - this deals with the orientation of the child item as it relates to parent model</param>
        /// <param name="parentLocation">out parameter - this is another part of the orientation data for correct visual display</param>
        public void SetChild(WorldObject item, int placementPosition, out int placementId, out int parentLocation)
        {
            placementId = 0;
            parentLocation = 0;

            // TODO: I think there is a state missing - it is one of the edge cases. I need to revist this.   Og II
            switch ((EquipMask)placementPosition)
            {
                case EquipMask.MeleeWeapon:
                    placementId = (int)ACE.Entity.Enum.Placement.RightHandCombat;
                    parentLocation = (int)ACE.Entity.Enum.ParentLocation.RightHand;
                    break;

                case EquipMask.Shield:
                    if (item.ItemType == ItemType.Armor)
                    {
                        placementId = (int)ACE.Entity.Enum.Placement.Shield;
                        parentLocation = (int)ACE.Entity.Enum.ParentLocation.Shield;
                    }
                    else
                    {
                        placementId = (int)ACE.Entity.Enum.Placement.RightHandCombat;
                        parentLocation = (int)ACE.Entity.Enum.ParentLocation.LeftWeapon;
                    }
                    break;

                case EquipMask.MissileWeapon:
                    if (item.DefaultCombatStyle == CombatStyle.Bow ||
                        item.DefaultCombatStyle == CombatStyle.Crossbow)
                    {
                        placementId = (int)ACE.Entity.Enum.Placement.LeftHand;
                        parentLocation = (int)ACE.Entity.Enum.ParentLocation.LeftHand;
                    }
                    else
                    {
                        placementId = (int)ACE.Entity.Enum.Placement.RightHandCombat;
                        parentLocation = (int)ACE.Entity.Enum.ParentLocation.RightHand;
                    }
                    break;

                case EquipMask.MissileAmmo:
                    // quiver = 5 for arrows/bolts?
                    placementId = (int)ACE.Entity.Enum.Placement.RightHandCombat;
                    parentLocation = (int)ACE.Entity.Enum.ParentLocation.RightHand;
                    break;

                case EquipMask.Held:
                    placementId = (int)ACE.Entity.Enum.Placement.RightHandCombat;
                    parentLocation = (int)ACE.Entity.Enum.ParentLocation.RightHand;
                    break;

                default:
                    placementId = (int)ACE.Entity.Enum.Placement.Default;
                    parentLocation = (int)ACE.Entity.Enum.ParentLocation.None;
                    break;
            }

            if (item.CurrentWieldedLocation != null)
                Children.Add(new HeldItem(item.Guid.Full, parentLocation, (EquipMask)item.CurrentWieldedLocation));

            item.Placement = (Placement)placementId;
            item.ParentLocation = (ParentLocation)parentLocation;
            item.Location = Location;
        }


        public void GenerateWieldList()
        {
            foreach (var item in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (int)DestinationType.Wield || x.DestinationType == (int)DestinationType.WieldTreasure))
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                if (wo != null)
                {
                    if (item.Palette > 0)
                        wo.PaletteTemplate = item.Palette;

                    if (item.Shade > 0)
                        wo.Shade = item.Shade;

                    if (wo.ValidLocations != null)
                        TryEquipObject(wo, (int)wo.ValidLocations.Value);
                }
            }
        }

        public uint? WieldedTreasureType
        {
            get => GetProperty(PropertyDataId.WieldedTreasureType);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.WieldedTreasureType); else SetProperty(PropertyDataId.WieldedTreasureType, value.Value); }
        }

        public List<TreasureWielded> WieldedTreasure
        {
            get
            {
                if (WieldedTreasureType.HasValue)
                    return DatabaseManager.World.GetWieldedTreasure(WieldedTreasureType.Value);
                else
                    return null;
            }
        }

        public void GenerateWieldedTreasure()
        {
            if (WieldedTreasure is null)
                return;

            foreach (var item in WieldedTreasure)
            {
                var rng = Physics.Common.Random.RollDice(0f, 1f);

                if (rng < item.Probability)
                {
                    var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                    if (wo != null)
                    {
                        if (item.PaletteId > 0)
                            wo.PaletteTemplate = (int)item.PaletteId;

                        if (item.Shade > 0)
                            wo.Shade = item.Shade;

                        if (item.StackSize > 0)
                            wo.StackSize = (ushort)item.StackSize;

                        TryAddToInventory(wo);
                        //TryEquipObject(wo, (int)wo.ValidLocations);
                    }
                }
            }
        }
    }
}
