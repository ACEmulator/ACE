using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Determines the monster inventory items to wield
        /// </summary>
        public List<WorldObject> SelectWieldedTreasure()
        {
            /*foreach (var item in Inventory.Values)
                Console.WriteLine($"{item.Name} - {item.WeenieType}");*/

            var wieldedClothing = SelectWieldedClothing();
            var wieldedArmor = SelectWieldedArmor();

            var wieldedWeapons = SelectWieldedWeapons();
            var wieldedShield = SelectWieldedShield();

            if (wieldedShield != null && (wieldedWeapons.Count == 0 || !wieldedWeapons[0].IsRanged))
                wieldedWeapons.Add(wieldedShield);

            return wieldedWeapons;
        }

        /// <summary>
        /// Selects the clothing to wear from a monster's inventory
        /// </summary>
        public List<WorldObject> SelectWieldedClothing()
        {
            var clothing = GetInventoryItemsOfTypeWeenieType(WeenieType.Clothing).Where(c => ((uint)(c.ClothingPriority ?? 0) & (uint)CoverageMaskHelper.Underwear) != 0).ToList();

            if (clothing.Count == 0) return new List<WorldObject>();

            // sort by # of areas covered
            // prioritize clothing that covers more areas
            // ie., a shirt that covers both upper arms and lower arms
            clothing.Sort(ValidLocationComparer);
            clothing.Reverse();

            var shirts = clothing.Where(c => ((uint)(c.ClothingPriority ?? 0) & (uint)CoverageMaskHelper.UnderwearShirt) != 0).ToList();
            var pants = clothing.Where(c => ((uint)(c.ClothingPriority ?? 0) & (uint)CoverageMaskHelper.UnderwearPants) != 0).ToList();

            /*Console.WriteLine("\nSelectWieldedClothing\nShirts:");
            foreach (var item in shirts)
                DebugArmorClothing(item);

            Console.WriteLine("Pants:");
            foreach (var item in pants)
                DebugArmorClothing(item);*/

            // try to equip the clothing at top of lists
            var equipped = new List<WorldObject>();

            if (pants.Count > 0)
            {
                var item = pants[0];
                TryRemoveFromInventory(item.Guid);
                if (TryWieldObjectWithBroadcasting(item, item.ValidLocations ?? 0))
                    equipped.Add(item);
            }

            if (shirts.Count > 0)
            {
                var item = shirts[0];
                TryRemoveFromInventory(item.Guid);
                if (TryWieldObjectWithBroadcasting(item, item.ValidLocations ?? 0))
                    equipped.Add(item);
            }
            return equipped;
        }

        public List<WorldObject> SelectWieldedArmor()
        {
            // technically selecting all outerwear,
            // includes things like hats and slippers
            var armor = GetInventoryItemsOfTypeWeenieType(WeenieType.Clothing).Where(a => ((uint)(a.ClothingPriority ?? 0) & (uint)CoverageMaskHelper.Outerwear) != 0).ToList();

            if (armor.Count == 0) return new List<WorldObject>();

            // sort by # of areas covered, and then AL
            // prioritize AL first, and then armor that covers more areas
            armor.Sort(ValidLocationComparer);
            armor.Reverse();
            armor.Sort(ArmorLevelComparer);
            armor.Reverse();

            // divide up into slots?
            // use CoverageMask or EquipMask?
            // boots / lowerLegs:
            // Boots - Locations: LowerLegWear, FootWear, Coverage: Feet
            // coverage mask data seems like it could be buggy / inconsistent in PY16...

            var head = armor.Where(c => ((uint)(c.ValidLocations ?? 0) & (uint)EquipMask.HeadWear) != 0).ToList();
            var chest = armor.Where(c => ((uint)(c.ValidLocations ?? 0) & (uint)(EquipMask.ChestArmor | EquipMask.ChestWear)) != 0).ToList();
            var upperArms = armor.Where(c => ((uint)(c.ValidLocations ?? 0) & (uint)(EquipMask.UpperArmArmor | EquipMask.UpperArmWear)) != 0).ToList();    // this will also grab chest pieces that also cover upper arms etc.
            var lowerArms = armor.Where(c => ((uint)(c.ValidLocations ?? 0) & (uint)(EquipMask.LowerArmArmor | EquipMask.LowerArmWear)) != 0).ToList();
            var hands = armor.Where(c => ((uint)(c.ValidLocations ?? 0) & (uint)EquipMask.HandWear) != 0).ToList();
            // excluding abdomen, as that can be covered by potentially chest or pants
            var upperLegs = armor.Where(c => ((uint)(c.ValidLocations ?? 0) & (uint)(EquipMask.UpperLegArmor | EquipMask.UpperLegWear)) != 0).ToList();
            var lowerLegs = armor.Where(c => ((uint)(c.ValidLocations ?? 0) & (uint)(EquipMask.LowerLegArmor | EquipMask.LowerLegWear)) != 0).ToList();
            var feet = armor.Where(c => ((uint)(c.ValidLocations ?? 0) & (uint)EquipMask.FootWear) != 0).ToList();

            /*Console.WriteLine("\nSelectWieldedArmor");
            foreach (var item in armor)
                DebugArmorClothing(item);*/

            // try to equip the clothing at top of lists
            var equipped = new List<WorldObject>();

            var sorted = head.Concat(chest).Concat(upperArms).Concat(lowerArms).Concat(hands).Concat(upperLegs).Concat(lowerLegs).Concat(feet).ToList();

            foreach (var item in sorted)
            {
                TryRemoveFromInventory(item.Guid);
                if (TryWieldObjectWithBroadcasting(item, item.ValidLocations ?? 0))
                    equipped.Add(item);
            }

            return equipped;
        }

        /// <summary>
        /// Displays information about a a piece of armor / clothing
        /// </summary>
        public void DebugArmorClothing(WorldObject item)
        {
            var locations = item.ValidLocations;
            var coverage = item.ClothingPriority;

            Console.WriteLine($"{item.Name} - Locations: {locations}, Coverage: {coverage}");
        }

        /// <summary>
        /// Compares the number of body parts covered by 2 pieces of clothing
        /// </summary>
        public int ValidLocationComparer(WorldObject a, WorldObject b)
        {
            return EnumHelper.NumFlags((uint)a.ValidLocations).CompareTo(EnumHelper.NumFlags((uint)b.ValidLocations));
        }

        /// <summary>
        /// Compares the number of body parts covered by 2 pieces of clothing
        /// </summary>
        public int ArmorLevelComparer(WorldObject a, WorldObject b)
        {
            return ((uint)a.ArmorLevel).CompareTo((uint)b.ArmorLevel);
        }

        public List<WorldObject> SelectWieldedWeapons()
        {
            //Console.WriteLine($"{Name}.SelectWieldedWeapons()");

            var meleeWeapons = GetInventoryItemsOfTypeWeenieType(WeenieType.MeleeWeapon);
            var missileWeapons = GetInventoryItemsOfTypeWeenieType(WeenieType.MissileLauncher);
            var missiles = GetInventoryItemsOfTypeWeenieType(WeenieType.Missile);
            missileWeapons.AddRange(missiles);
            var ammo = GetInventoryItemsOfTypeWeenieType(WeenieType.Ammunition);

            var allWeapons = meleeWeapons.Concat(missileWeapons).ToList();
            //var allWeapons = missileWeapons;

            if (allWeapons.Count == 0) return new List<WorldObject>();

            //DebugTreasure();

            /*Console.WriteLine("\nSelectWieldedCombatItems");
            Console.WriteLine("Melee Weapons: " + meleeWeapons.Count);
            Console.WriteLine("Missile weapons: " + missileWeapons.Count);
            Console.WriteLine("Ammunition: " + ammo.Count);*/

            // select the best weapon available
            while (true)
            {
                var weapon = FindInventoryWeapon(allWeapons);
                if (weapon == null) return new List<WorldObject>();

                // does this weapon require ammo?
                if (weapon.IsAmmoLauncher)
                {
                    // find the best ammo for this weapon
                    var ammoType = ammo.Where(i => i.AmmoType == weapon.AmmoType).ToList();
                    var curAmmo = FindInventoryWeapon(ammoType);

                    if (curAmmo == null)
                    {
                        allWeapons.Remove(weapon);  // remove from possible selections
                        continue;   // find next best weapon
                    }

                    //Console.WriteLine("Ammo type: " + (AmmoType)(weapon.AmmoType ?? 0));

                    return new List<WorldObject>() { weapon, curAmmo };
                }

                // CombatUse / DefaultCombatStyle / ValidLocations?
                if (AiAllowedCombatStyle.HasFlag(CombatStyle.DualWield))
                {
                    if (weapon.WeenieType == WeenieType.MeleeWeapon && !weapon.IsTwoHanded)
                    {
                        var dualWield = meleeWeapons.FirstOrDefault(i => i.AutoWieldLeft);
                        if (dualWield != null)
                            return new List<WorldObject> { weapon, dualWield };
                    }
                }

                return new List<WorldObject>() { weapon };
            }
        }

        public WorldObject FindInventoryWeapon(List<WorldObject> weapons)
        {
            if (weapons == null) return null;

            //var highestMax = weapons.OrderByDescending(w => w.GetBaseDamage().Max).FirstOrDefault();
            //var highestAvg = weapons.OrderByDescending(w => w.GetBaseDamage().Avg).FirstOrDefault();

            //return highestMax;

            // did monsters select best weapons, or just a random weapon?
            // see: lugians (wielded treasure table 439), 100% spawn with rocks if most damage potential selected
            weapons.Shuffle();
            return weapons.FirstOrDefault(i => !i.AutoWieldLeft);

            /*var rng = ThreadSafeRandom.Next(0, weapons.Count);
            if (rng == weapons.Count)
                return null;    // choose no weapon? lugians should have ~33% chance to select rock, according to retail pcaps

            return weapons[rng];*/
        }

        public WorldObject SelectWieldedShield()
        {
            var shields = Inventory.Values.Where(wo => wo.IsShield).ToList();

            if (shields.Count == 0)
                return null;

            // select a random shield
            var rng = ThreadSafeRandom.Next(0, shields.Count - 1);
            return shields[rng];
        }

        public void DebugTreasure()
        {
            if (WieldedTreasure == null) return;

            Console.WriteLine($"{Name} possible wielded treasure:");
            foreach (var item in WieldedTreasure)
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(item.WeenieClassId);
                var probability = Math.Round(item.Probability * 100, 2);
                Console.WriteLine($"{weenie.ClassName} - {probability}%");
            }

            var totalProbability = Math.Round(GetTotalProbability(WieldedTreasure) * 100, 2);
            Console.WriteLine($"Probability of spawning any items: {totalProbability}%");
        }

        public float GetTotalProbability(List<TreasureWielded> items)
        {
            if (items == null) return 0.0f;

            var prob = items.Select(i => i.Probability).ToList();

            var totalSum = prob.Sum();
            var totalProduct = prob.Product();

            return totalSum - totalProduct;
        }

        public void EquipInventoryItems(bool weaponsOnly = false)
        {
            var items = weaponsOnly ? SelectWieldedWeapons() : SelectWieldedTreasure();
            if (items != null)
            {
                foreach (var item in items)
                {
                    //Console.WriteLine($"{Name} equipping {item.Name}");

                    if (item.ValidLocations != null)
                    {
                        TryRemoveFromInventory(item.Guid);
                        var result = TryWieldObjectWithBroadcasting(item, item.ValidLocations ?? 0);
                        //Console.WriteLine($"{Name} tried to equip {item.Name}, result={result}");
                    }
                }
            }
        }
    }
}
