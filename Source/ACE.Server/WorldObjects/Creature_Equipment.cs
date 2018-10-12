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
using ACE.Server.Network.GameMessages.Messages;

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
        /// Returns the currently equipped primary weapon
        /// </summary>
        public WorldObject GetEquippedWeapon()
        {
            var meleeWeapon = GetEquippedMeleeWeapon();
            return meleeWeapon != null ? meleeWeapon : GetEquippedMissileWeapon();
        }

        /// <summary>
        /// Returns the current equipped active melee weapon
        /// This will normally be the primary melee weapon, but if dual wielding, this will be the weapon for the next attack
        /// </summary>
        public WorldObject GetEquippedMeleeWeapon()
        {
            if (!IsDualWieldAttack || DualWieldAlternate)
                return EquippedObjects.Values.FirstOrDefault(e => e.ParentLocation == ACE.Entity.Enum.ParentLocation.RightHand && (e.CurrentWieldedLocation == EquipMask.MeleeWeapon || e.CurrentWieldedLocation == EquipMask.TwoHanded));
            else
                return GetDualWieldWeapon();
        }

        /// <summary>
        /// Returns the currently equipped secondary weapon
        /// </summary>
        public WorldObject GetDualWieldWeapon()
        {
            return EquippedObjects.Values.FirstOrDefault(e => !e.IsShield && e.CurrentWieldedLocation == EquipMask.Shield);
        }

        /// <summary>
        /// Returns the currently equipped wand
        /// </summary>
        public WorldObject GetEquippedWand()
        {
            return EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.Held);
        }

        /// <summary>
        /// Returns the currently equipped missile weapon
        /// </summary>
        public WorldObject GetEquippedMissileWeapon()
        {
            return EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.MissileWeapon);
        }

        /// <summary>
        /// Returns the currently equipped shield
        /// </summary>
        public WorldObject GetEquippedShield()
        {
            return EquippedObjects.Values.FirstOrDefault(e => e.IsShield && e.CurrentWieldedLocation == EquipMask.Shield);
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
        /// Returns the ammo slot item for bows / atlatls,
        /// or the missile weapon for thrown weapons
        /// </summary>
        public WorldObject GetMissileAmmo()
        {
            var weapon = GetEquippedMissileWeapon();

            if (weapon.IsAmmoLauncher)
                return GetEquippedAmmo();
            else
                return weapon;
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

            EnqueueActionBroadcast((Player p) => p.TrackObject(this));

            worldObject.EmoteManager.OnWield(this);

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

                //EnqueueActionBroadcast((Player p) => p.TrackObject(this));
                EnqueueBroadcast(new GameMessagePickupEvent(worldObject));

                worldObject.EmoteManager.OnUnwield(this);

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
            foreach (var item in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (int) DestinationType.Wield || x.DestinationType == (int) DestinationType.WieldTreasure))
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                if (wo != null)
                {
                    if (item.Palette > 0)
                        wo.PaletteTemplate = item.Palette;

                    if (item.Shade > 0)
                        wo.Shade = item.Shade;

                    if (wo.ValidLocations != null)
                        TryEquipObject(wo, (int) wo.ValidLocations.Value);
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
                    return DatabaseManager.World.GetCachedWieldedTreasure(WieldedTreasureType.Value);
                else
                    return null;
            }
        }

        public void GenerateWieldedTreasure()
        {
            if (WieldedTreasure == null) return;

            var table = new TreasureWieldedTable(WieldedTreasure);

            foreach (var set in table.Sets)
                GenerateWieldedTreasureSet(set);
        }

        public void GenerateWieldedTreasureSet(TreasureWieldedSet set)
        {
            var rng = Physics.Common.Random.RollDice(0.0f, set.TotalProbability);
            var probability = 0.0f;

            foreach (var item in set.Items)
            {
                probability += item.Item.Probability;
                if (rng > probability) continue;

                // item roll successful, spawn item in creature inventory
                var success = CreateWieldedTreasure(item.Item);
                if (!success) continue;

                // traverse into possible subsets
                if (item.Subset != null)
                    GenerateWieldedTreasureSet(item.Subset);

                break;
            }
        }

        public bool CreateWieldedTreasure(TreasureWielded item)
        {
            var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);
            if (wo == null) return false;

            if (item.PaletteId > 0)
                wo.PaletteTemplate = (int)item.PaletteId;

            if (item.Shade > 0)
                wo.Shade = item.Shade;

            if (item.StackSize > 0)
            {
                var stackSize = item.StackSize;

                var hasVariance = item.StackSizeVariance > 0;
                if (hasVariance)
                {
                    var minStack = (int)Math.Round(item.StackSize * item.StackSizeVariance);
                    var maxStack = item.StackSize;
                    stackSize = Physics.Common.Random.RollDice(minStack, maxStack);
                }
                wo.StackSize = (ushort)stackSize;
            }

            return TryAddToInventory(wo);
        }
    }
}
