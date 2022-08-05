using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
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
        protected void AddBiotasToEquippedObjects(IEnumerable<ACE.Database.Models.Shard.Biota> wieldedItems)
        {
            foreach (var biota in wieldedItems)
            {
                var worldObject = WorldObjectFactory.CreateWorldObject(biota);
                EquippedObjects[worldObject.Guid] = worldObject;

                AddItemToEquippedItemsRatingCache(worldObject);

                EncumbranceVal += (worldObject.EncumbranceVal ?? 0);
            }

            EquippedObjectsLoaded = true;

            SetChildren();
        }

        public bool WieldedLocationIsAvailable(WorldObject item, EquipMask wieldedLocation)
        {
            // filtering to just armor here, or else trinkets and dual wielding breaks
            // update: cannot repro the break anymore?
            //var existing = this is Player ? GetEquippedClothingArmor(item.ClothingPriority ?? 0) : GetEquippedItems(item, wieldedLocation);
            var existing = GetEquippedItems(item, wieldedLocation);

            // TODO: handle overlap from MeleeWeapon / MissileWeapon / Held

            return existing.Count == 0;
        }

        public bool HasEquippedItem(ObjectGuid objectGuid)
        {
            return EquippedObjects.ContainsKey(objectGuid);
        }

        /// <summary>
        /// Get Wielded Item. Returns null if not found.
        /// </summary>
        public WorldObject GetEquippedItem(uint objectGuid)
        {
            return EquippedObjects.TryGetValue(new ObjectGuid(objectGuid), out var item) ? item : null; // todo fix this so it doesn't instantiate a new ObjectGuid
        }

        /// <summary>
        /// Get Wielded Item. Returns null if not found.
        /// </summary>
        public WorldObject GetEquippedItem(ObjectGuid objectGuid)
        {
            return EquippedObjects.TryGetValue(objectGuid, out var item) ? item : null;
        }

        /// <summary>
        /// Returns a list of equipped clothing/armor with any coverage overlap
        /// </summary>
        public List<WorldObject> GetEquippedClothingArmor(CoverageMask coverageMask)
        {
            return EquippedObjects.Values.Where(i => i.ClothingPriority != null && (i.ClothingPriority & coverageMask) != 0).ToList();
        }

        /// <summary>
        /// Returns a list of equipped items with any overlap with input locations
        /// </summary>
        public List<WorldObject> GetEquippedItems(WorldObject item, EquipMask wieldedLocation)
        {
            if (IsWeaponSlot(wieldedLocation))
            {
                // TODO: change to coalesced CurrentWieldedLocation
                GetPlacementLocation(item, wieldedLocation, out var placement, out var parentLocation);
                return EquippedObjects.Values.Where(i => i.ParentLocation != null && i.ParentLocation == parentLocation && i.CurrentWieldedLocation != EquipMask.MissileAmmo).ToList();
            }

            if (item is Clothing)
                return GetEquippedClothingArmor(item.ClothingPriority ?? 0);
            else
                return EquippedObjects.Values.Where(i => i.CurrentWieldedLocation != null && (i.CurrentWieldedLocation & wieldedLocation) != 0).ToList();
        }

        /// <summary>
        /// Returns the currently equipped primary weapon
        /// </summary>
        public WorldObject GetEquippedWeapon(bool forceMainHand = false)
        {
            var meleeWeapon = GetEquippedMeleeWeapon(forceMainHand);
            return meleeWeapon ?? GetEquippedMissileWeapon();
        }

        /// <summary>
        /// Returns the current equipped active melee weapon
        /// This will normally be the primary melee weapon, but if dual wielding, this will be the weapon for the next attack
        /// </summary>
        public WorldObject GetEquippedMeleeWeapon(bool forceMainHand = false)
        {
            if (!IsDualWieldAttack || DualWieldAlternate || forceMainHand)
                return EquippedObjects.Values.FirstOrDefault(e => e.ParentLocation == ACE.Entity.Enum.ParentLocation.RightHand && (e.CurrentWieldedLocation == EquipMask.MeleeWeapon || e.CurrentWieldedLocation == EquipMask.TwoHanded));

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
        /// This can be either a missile launcher (bow, crossbow, atlatl) or stackable thrown weapons directly in the main hand slot
        /// </summary>
        public WorldObject GetEquippedMissileWeapon()
        {
            return EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.MissileWeapon);
        }

        /// <summary>
        /// Returns the currently equipped missile launcher
        /// </summary>
        public WorldObject GetEquippedMissileLauncher()
        {
            return EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.MissileWeapon && e is MissileLauncher);
        }

        /// <summary>
        /// Returns the current equipped weapon in main hand
        /// </summary>
        public WorldObject GetEquippedMainHand()
        {
            return GetEquippedMeleeWeapon(true) ?? GetEquippedMissileWeapon() ?? GetEquippedWand();
        }

        /// <summary>
        /// Returns either a shield, an off-hand weapon, or null
        /// </summary>
        public WorldObject GetEquippedOffHand()
        {
            return EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.Shield);
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

            if (weapon != null && weapon.IsAmmoLauncher)
                return GetEquippedAmmo();

            return weapon;
        }

        /// <summary>
        /// This is initialized the first time an item is equipped that has a rating. If it is null, there are no equipped items with ratings.
        /// </summary>
        private Dictionary<PropertyInt, int> equippedItemsRatingCache;

        private void AddItemToEquippedItemsRatingCache(WorldObject wo)
        {
            if ((wo.GearDamage ?? 0) == 0 && (wo.GearDamageResist ?? 0) == 0 && (wo.GearCritDamage ?? 0) == 0 && (wo.GearCritDamageResist ?? 0) == 0 && (wo.GearHealingBoost ?? 0) == 0 && (wo.GearMaxHealth ?? 0) == 0 && (wo.GearPKDamageRating ?? 0) == 0 && (wo.GearPKDamageResistRating ?? 0) == 0)
                return;

            if (equippedItemsRatingCache == null)
            {
                equippedItemsRatingCache = new Dictionary<PropertyInt, int>
                {
                    { PropertyInt.GearDamage, 0 },
                    { PropertyInt.GearDamageResist, 0 },
                    { PropertyInt.GearCritDamage, 0 },
                    { PropertyInt.GearCritDamageResist, 0 },
                    { PropertyInt.GearHealingBoost, 0 },
                    { PropertyInt.GearMaxHealth, 0 },
                    { PropertyInt.GearPKDamageRating, 0 },
                    { PropertyInt.GearPKDamageResistRating, 0 },
                };
            }

            equippedItemsRatingCache[PropertyInt.GearDamage] += (wo.GearDamage ?? 0);
            equippedItemsRatingCache[PropertyInt.GearDamageResist] += (wo.GearDamageResist ?? 0);
            equippedItemsRatingCache[PropertyInt.GearCritDamage] += (wo.GearCritDamage ?? 0);
            equippedItemsRatingCache[PropertyInt.GearCritDamageResist] += (wo.GearCritDamageResist ?? 0);
            equippedItemsRatingCache[PropertyInt.GearHealingBoost] += (wo.GearHealingBoost ?? 0);
            equippedItemsRatingCache[PropertyInt.GearMaxHealth] += (wo.GearMaxHealth ?? 0);
            equippedItemsRatingCache[PropertyInt.GearPKDamageRating] += (wo.GearPKDamageRating ?? 0);
            equippedItemsRatingCache[PropertyInt.GearPKDamageResistRating] += (wo.GearPKDamageResistRating ?? 0);
        }

        private void RemoveItemFromEquippedItemsRatingCache(WorldObject wo)
        {
            if (equippedItemsRatingCache == null)
                return;

            equippedItemsRatingCache[PropertyInt.GearDamage] -= (wo.GearDamage ?? 0);
            equippedItemsRatingCache[PropertyInt.GearDamageResist] -= (wo.GearDamageResist ?? 0);
            equippedItemsRatingCache[PropertyInt.GearCritDamage] -= (wo.GearCritDamage ?? 0);
            equippedItemsRatingCache[PropertyInt.GearCritDamageResist] -= (wo.GearCritDamageResist ?? 0);
            equippedItemsRatingCache[PropertyInt.GearHealingBoost] -= (wo.GearHealingBoost ?? 0);
            equippedItemsRatingCache[PropertyInt.GearMaxHealth] -= (wo.GearMaxHealth ?? 0);
            equippedItemsRatingCache[PropertyInt.GearPKDamageRating] -= (wo.GearPKDamageRating ?? 0);
            equippedItemsRatingCache[PropertyInt.GearPKDamageResistRating] -= (wo.GearPKDamageResistRating ?? 0);
        }

        public int GetEquippedItemsRatingSum(PropertyInt rating)
        {
            if (equippedItemsRatingCache == null)
                return 0;

            if (equippedItemsRatingCache.TryGetValue(rating, out var value))
                return value;

            log.Error($"Creature_Equipment.GetEquippedItemsRatingsSum() does not support {rating}");
            return 0;
        }

        /// <summary>
        /// Try to wield an object for non-player creatures
        /// </summary>
        /// <returns></returns>
        public bool TryWieldObject(WorldObject worldObject, EquipMask wieldedLocation)
        {
            // check wield requirements?
            if (!TryEquipObject(worldObject, wieldedLocation))
                return false;

            // enqueue to ensure parent object has spawned,
            // and spell fx are visible
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(0.1);
            actionChain.AddAction(this, () => TryActivateItemSpells(worldObject));
            actionChain.EnqueueChain();

            return true;
        }

        /// <summary>
        /// Tries to activate item spells for a non-player creature
        /// </summary>
        private void TryActivateItemSpells(WorldObject item)
        {
            if (!Attackable)
                return;

            // check activation requirements?
            foreach (var spell in item.Biota.GetKnownSpellsIds(BiotaDatabaseLock))
                CreateItemSpell(item, (uint)spell);
        }

        /// <summary>
        /// This will set the CurrentWieldedLocation property to wieldedLocation and the Wielder property to this guid and will add it to the EquippedObjects dictionary.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        public bool TryEquipObject(WorldObject worldObject, EquipMask wieldedLocation)
        {
            // todo: verify wielded location is valid location
            if (!WieldedLocationIsAvailable(worldObject, wieldedLocation))
                return false;

            worldObject.CurrentWieldedLocation = wieldedLocation;
            worldObject.WielderId = Biota.Id;
            worldObject.Wielder = this;

            EquippedObjects[worldObject.Guid] = worldObject;

            AddItemToEquippedItemsRatingCache(worldObject);

            EncumbranceVal += (worldObject.EncumbranceVal ?? 0);
            Value += (worldObject.Value ?? 0);

            TrySetChild(worldObject);

            worldObject.OnWield(this);

            return true;
        }

        protected bool TryWieldObjectWithBroadcasting(WorldObject worldObject, EquipMask wieldedLocation)
        {
            // check wield requirements?
            if (!TryEquipObjectWithBroadcasting(worldObject, wieldedLocation))
                return false;

            TryActivateItemSpells(worldObject);

            return true;
        }

        /// <summary>
        /// This will set the CurrentWieldedLocation property to wieldedLocation and the Wielder property to this guid and will add it to the EquippedObjects dictionary.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        protected bool TryEquipObjectWithBroadcasting(WorldObject worldObject, EquipMask wieldedLocation)
        {
            if (!TryEquipObject(worldObject, wieldedLocation))
                return false;

            if (IsInChildLocation(worldObject)) // Is this equipped item visible to others?
                EnqueueBroadcast(false, new GameMessageSound(Guid, Sound.WieldObject));

            if (worldObject.ParentLocation != null)
                EnqueueBroadcast(new GameMessageParentEvent(this, worldObject));

            EnqueueBroadcast(new GameMessageObjDescEvent(this));

            // Notify viewers in the area that we've equipped the item
            EnqueueActionBroadcast(p => p.TrackEquippedObject(this, worldObject));

            return true;
        }

        /// <summary>
        /// This will remove the Wielder and CurrentWieldedLocation properties on the item and will remove it from the EquippedObjects dictionary.<para />
        /// It does not add it to inventory as you could be unwielding to the ground or a chest.<para />
        /// It will also decrease the EncumbranceVal and Value.
        /// </summary>
        public bool TryDequipObject(ObjectGuid objectGuid, out WorldObject worldObject, out EquipMask wieldedLocation)
        {
            if (!EquippedObjects.Remove(objectGuid, out worldObject))
            {
                wieldedLocation = 0;
                return false;
            }

            RemoveItemFromEquippedItemsRatingCache(worldObject);

            wieldedLocation = worldObject.CurrentWieldedLocation ?? EquipMask.None;

            worldObject.RemoveProperty(PropertyInt.CurrentWieldedLocation);
            worldObject.RemoveProperty(PropertyInstanceId.Wielder);
            worldObject.Wielder = null;

            worldObject.OnSpellsDeactivated();

            EncumbranceVal -= (worldObject.EncumbranceVal ?? 0);
            Value -= (worldObject.Value ?? 0);

            ClearChild(worldObject);

            var wo = worldObject;
            Children.Remove(Children.Find(s => s.Guid == wo.Guid.Full));

            worldObject.OnUnWield(this);

            return true;
        }

        /// <summary>
        /// Called by non-player creatures to unwield an item,
        /// removing any spells casted by the item
        /// </summary>
        public bool TryUnwieldObjectWithBroadcasting(ObjectGuid objectGuid, out WorldObject worldObject, out EquipMask wieldedLocation, bool droppingToLandscape = false)
        {
            if (!TryDequipObjectWithBroadcasting(objectGuid, out worldObject, out wieldedLocation, droppingToLandscape))
                return false;

            // remove item spells
            foreach (var spell in worldObject.Biota.GetKnownSpellsIds(BiotaDatabaseLock))
                RemoveItemSpell(worldObject, (uint)spell, true);

            return true;
        }

        /// <summary>
        /// This will remove the Wielder and CurrentWieldedLocation properties on the item and will remove it from the EquippedObjects dictionary.<para />
        /// It does not add it to inventory as you could be unwielding to the ground or a chest.<para />
        /// It will also decrease the EncumbranceVal and Value.
        /// </summary>
        protected bool TryDequipObjectWithBroadcasting(ObjectGuid objectGuid, out WorldObject worldObject, out EquipMask wieldedLocation, bool droppingToLandscape = false)
        {
            if (!TryDequipObject(objectGuid, out worldObject, out wieldedLocation))
                return false;

            if ((wieldedLocation & EquipMask.Selectable) != 0) // Is this equipped item visible to others?
                EnqueueBroadcast(false, new GameMessageSound(Guid, Sound.UnwieldObject));

            EnqueueBroadcast(new GameMessageObjDescEvent(this));

            // If item has any spells, remove them from the registry on unequip
            if (worldObject.Biota.PropertiesSpellBook != null)
            {
                foreach (var spell in worldObject.Biota.PropertiesSpellBook)
                {
                    if (worldObject.HasProcSpell((uint)spell.Key))
                        continue;

                    RemoveItemSpell(worldObject, (uint)spell.Key, true);
                }
            }

            if (!droppingToLandscape)
            {
                // This should only be called if the object is going to the private storage, not when dropped on the landscape
                var wo = worldObject;
                EnqueueActionBroadcast(p => p.RemoveTrackedEquippedObject(this, wo));
            }

            return true;
        }


        protected bool IsInChildLocation(WorldObject item)
        {
            if (item.CurrentWieldedLocation == null)
                return false;

            if (((EquipMask)item.CurrentWieldedLocation & EquipMask.Selectable) != 0)
                return true;

            if (((EquipMask)item.CurrentWieldedLocation & EquipMask.MissileAmmo) != 0)
            {
                var wielder = item.Wielder;

                if (wielder != null && wielder is Creature creature)
                {
                    var weapon = creature.GetEquippedMissileWeapon();

                    if (weapon == null)
                        return false;

                    if (creature.CombatMode == CombatMode.Missile && weapon.WeenieType == WeenieType.MissileLauncher)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This method sets properties needed for items that will be child items.<para />
        /// Items here are only items equipped in the hands.<para />
        /// This deals with the orientation and positioning for visual appearance of the child items held by the parent.<para />
        /// If the item isn't in a valid child state (CurrentWieldedLocation), the child properties will be cleared. (Placement, ParentLocation, Location).
        /// </summary>
        private bool TrySetChild(WorldObject item)
        {
            if (!IsInChildLocation(item))
            {
                ClearChild(item);
                return false;
            }

            GetPlacementLocation(item, item.CurrentWieldedLocation ?? 0, out var placement, out var parentLocation);

            Children.Add(new HeldItem(item.Guid.Full, (int)parentLocation, (EquipMask)item.CurrentWieldedLocation));

            item.Placement = placement;
            item.ParentLocation = parentLocation;
            item.Location = Location;

            return true;
        }

        private static void GetPlacementLocation(WorldObject item, EquipMask wieldedLocation, out Placement placement, out ParentLocation parentLocation)
        {
            switch (wieldedLocation)
            {
                case EquipMask.MeleeWeapon:
                case EquipMask.Held:
                case EquipMask.TwoHanded:
                    placement = ACE.Entity.Enum.Placement.RightHandCombat;
                    parentLocation = ACE.Entity.Enum.ParentLocation.RightHand;
                    break;

                case EquipMask.Shield:
                    if (item.ItemType == ItemType.Armor)
                    {
                        placement = ACE.Entity.Enum.Placement.Shield;
                        parentLocation = ACE.Entity.Enum.ParentLocation.Shield;
                    }
                    else
                    {
                        placement = ACE.Entity.Enum.Placement.RightHandNonCombat;
                        parentLocation = ACE.Entity.Enum.ParentLocation.LeftWeapon;
                    }
                    break;

                case EquipMask.MissileWeapon:
                    if (item.DefaultCombatStyle == CombatStyle.Bow || item.DefaultCombatStyle == CombatStyle.Crossbow)
                    {
                        placement = ACE.Entity.Enum.Placement.LeftHand;
                        parentLocation = ACE.Entity.Enum.ParentLocation.LeftHand;
                    }
                    else
                    {
                        placement = ACE.Entity.Enum.Placement.RightHandCombat;
                        parentLocation = ACE.Entity.Enum.ParentLocation.RightHand;
                    }
                    break;

                default:
                    placement = ACE.Entity.Enum.Placement.Default;
                    parentLocation = ACE.Entity.Enum.ParentLocation.None;
                    break;
            }
        }

        private static bool IsWeaponSlot(EquipMask equipMask)
        {
            switch (equipMask)
            {
                case EquipMask.MeleeWeapon:
                case EquipMask.Held:
                case EquipMask.TwoHanded:
                case EquipMask.Shield:
                case EquipMask.MissileWeapon:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// This clears the child properties:<para />
        /// Placement = ACE.Entity.Enum.Placement.Resting<para />
        /// ParentLocation = null<para />
        /// Location = null
        /// </summary>
        protected void ClearChild(WorldObject item)
        {
            item.Placement = ACE.Entity.Enum.Placement.Resting;
            item.ParentLocation = null;
            item.Location = null;
        }

        /// <summary>
        /// Removes an existing object from Children if exists,
        /// and resets to new Child position
        /// </summary>
        public void ResetChild(WorldObject item)
        {
            Children.Remove(Children.Find(s => s.Guid == item.Guid.Full));
            TrySetChild(item);
        }

        /// <summary>
        /// This is called prior to SendSelf to load up the child list for wielded items that are held in a hand.
        /// </summary>
        private void SetChildren()
        {
            Children.Clear();

            foreach (var item in EquippedObjects.Values)
            {
                if (item.CurrentWieldedLocation != null)
                    TrySetChild(item);
            }
        }

        public void GenerateWieldList()
        {
            if (Biota.PropertiesCreateList == null)
                return;

            var wielded = Biota.PropertiesCreateList.Where(i => (i.DestinationType & DestinationType.Wield) != 0).ToList();

            var items = CreateListSelect(wielded);

            foreach (var item in items)
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(item);

                if (wo == null) continue;

                //if (wo.ValidLocations == null || (ItemCapacity ?? 0) > 0)
                {
                    if (!TryAddToInventory(wo))
                        wo.Destroy();
                }
                //else
                    //TryWieldObject(wo, (EquipMask)wo.ValidLocations);
            }
        }

        public static List<PropertiesCreateList> CreateListSelect(List<PropertiesCreateList> createList)
        {
            var trophy_drop_rate = PropertyManager.GetDouble("trophy_drop_rate").Item;
            if (trophy_drop_rate != 1.0)
                return CreateListSelect(createList, (float)trophy_drop_rate);

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            var totalProbability = 0.0f;
            var rngSelected = false;

            var results = new List<PropertiesCreateList>();

            foreach (var item in createList)
            {
                var destinationType = (DestinationType)item.DestinationType;
                var useRNG = destinationType.HasFlag(DestinationType.Treasure) && item.Shade != 0;

                var shadeOrProbability = item.Shade;

                if (useRNG)
                {
                    // handle sets in 0-1 chunks
                    if (totalProbability >= 1.0f)
                    {
                        totalProbability = 0.0f;
                        rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                        rngSelected = false;
                    }

                    var probability = shadeOrProbability;

                    totalProbability += probability;

                    if (rngSelected || rng >= totalProbability)
                        continue;

                    rngSelected = true;
                }

                results.Add(item);
            }

            return results;
        }

        public static List<PropertiesCreateList> CreateListSelect(List<PropertiesCreateList> _createList, float dropRateMod)
        {
            var createList = new CreateList(_createList);
            CreateListSetModifier modifier = null;

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            var totalProbability = 0.0f;
            var rngSelected = false;

            var results = new List<PropertiesCreateList>();

            for (var i = 0; i < _createList.Count; i++)
            {
                var item = _createList[i];

                var destinationType = (DestinationType)item.DestinationType;
                var useRNG = destinationType.HasFlag(DestinationType.Treasure) && item.Shade != 0;

                var shadeOrProbability = item.Shade;

                if (useRNG)
                {
                    // handle sets in 0-1 chunks
                    if (totalProbability == 0.0f || totalProbability >= 1.0f)
                    {
                        totalProbability = 0.0f;
                        rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                        rngSelected = false;

                        modifier = createList.GetSetModifier(i, dropRateMod);
                    }

                    var probability = shadeOrProbability * (item.WeenieClassId != 0 ? modifier.TrophyMod : modifier.NoneMod);

                    totalProbability += probability;

                    if (rngSelected || rng >= totalProbability)
                        continue;

                    rngSelected = true;
                }

                results.Add(item);
            }

            return results;
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

                return null;
            }
        }

        public void GenerateWieldedTreasure()
        {
            if (WieldedTreasure == null) return;

            //var table = new TreasureWieldedTable(WieldedTreasure);

            var wieldedTreasure = GenerateWieldedTreasureSets(WieldedTreasure);

            if (wieldedTreasure == null)
                return;

            foreach (var item in wieldedTreasure)
            {
                //if (item.ValidLocations == null || (ItemCapacity ?? 0) > 0)
                {
                    if (!TryAddToInventory(item))
                        item.Destroy();
                }
                //else
                    //TryWieldObject(item, (EquipMask)item.ValidLocations);
            }
        }

        public uint? InventoryTreasureType
        {
            get => GetProperty(PropertyDataId.InventoryTreasureType);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.InventoryTreasureType); else SetProperty(PropertyDataId.InventoryTreasureType, value.Value); }
        }

        public void GenerateInventoryTreasure()
        {
            if (InventoryTreasureType == null || InventoryTreasureType.Value <= 0) return;

            // based on property name found in older data, this property was only found 5 weenies (entirely contained in Focusing Stone quest)
            // guessing that the value might have possibly allowed for either Death or Wielded treasure, but technically it might have only been the former.
            // so for now, coded for checking both types.
            // Although the property's name seemingly was removed, either it's value was still used in code OR its value was moved into DeathTreasureType/CreateList
            // because pcaps for these 5 objects do show similar, if not exact, results on corpses.

            var treasureDeath = DatabaseManager.World.GetCachedDeathTreasure(InventoryTreasureType.Value);
            var treasureWielded = DatabaseManager.World.GetCachedWieldedTreasure(InventoryTreasureType.Value);

            var treasure = new List<WorldObject>();
            if (treasureDeath != null)
            {
                treasure = LootGenerationFactory.CreateRandomLootObjects(treasureDeath);
            }
            else if (treasureWielded != null)
            {
                treasure = GenerateWieldedTreasureSets(treasureWielded);
            }

            foreach (var item in treasure)
            {
                item.DestinationType = DestinationType.Treasure;
                // add this flag so item can move over to corpse upon death
                // (ACE logic: it is likely all inventory of a creature was moved over without reservation (bonded rules enforced), but ACE is slightly different in how it handles it for net same result)

                if (!TryAddToInventory(item))
                    item.Destroy();
            }
        }
    }
}
