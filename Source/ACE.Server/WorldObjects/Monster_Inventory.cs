using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public void BuildInventoryForMonster()
        {
            if (WieldedTreasure == null) return;

            // roll for each item in WieldedTreasure table
            foreach (var item in WieldedTreasure)
            {
                var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);
                if (rng < item.Probability)
                    TryAddToInventory(item);
            }
        }

        /// <summary>
        /// Adds a wielded treasure to monster's inventory
        /// </summary>
        /// <param name="item">The wielded treasure that was RNG selected to add</param>
        /// <returns>TRUE if item was successfully added</returns>
        public bool TryAddToInventory(TreasureWielded item)
        {
            var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

            if (item.PaletteId != 0)
                wo.PaletteTemplate = (int)item.PaletteId;

            if (item.Shade != 0)
                wo.Shade = item.Shade;

            if (item.StackSize != 0)
                wo.StackSize = (ushort)item.StackSize;

            //Console.WriteLine($"Adding {wo.Name} to {Name}'s inventory");

            return TryAddToInventory(wo);
        }

        /// <summary>
        /// Determines the monster inventory items to wield
        /// </summary>
        public List<WorldObject> SelectWieldedTreasure()
        {
            /*foreach (var item in Inventory.Values)
                Console.WriteLine($"{item.Name} - {item.WeenieType}");*/

            var meleeWeapons = GetInventoryItemsOfTypeWeenieType(WeenieType.MeleeWeapon);
            var missileWeapons = GetInventoryItemsOfTypeWeenieType(WeenieType.MissileLauncher);
            var missiles = GetInventoryItemsOfTypeWeenieType(WeenieType.Missile);
            missileWeapons.AddRange(missiles);
            var ammo = GetInventoryItemsOfTypeWeenieType(WeenieType.Ammunition);

            var allWeapons = meleeWeapons.Concat(missileWeapons).ToList();

            if (allWeapons.Count == 0) return null;

            //DebugTreasure();

            /*Console.WriteLine("SelectWieldedTreasure");
            Console.WriteLine("Melee Weapons: " + meleeWeapons.Count);
            Console.WriteLine("Missile weapons: " + missileWeapons.Count);
            Console.WriteLine("Ammunition: " + ammo.Count);*/

            // select the best weapon available
            while (true)
            {
                var weapon = FindBestWeapon(allWeapons);
                if (weapon == null) return null;

                // does this weapon require ammo?
                if (!weapon.IsAmmoLauncher)
                    return new List<WorldObject>() { weapon };

                // find the best ammo for this weapon
                var ammoType = ammo.Where(a => (a.AmmoType ?? 0) == (weapon.AmmoType ?? 0)).ToList();
                var curAmmo = FindBestWeapon(ammoType);

                if (curAmmo == null)
                {
                    allWeapons.Remove(weapon);  // remove from possible selections
                    continue;   // find next best weapon
                }

                //Console.WriteLine("Ammo type: " + (AmmoType)(weapon.AmmoType ?? 0));

                return new List<WorldObject>() { weapon, curAmmo };
            }
        }

        public WorldObject FindBestWeapon(List<WorldObject> weapons)
        {
            if (weapons == null) return null;

            var highestMax = weapons.OrderByDescending(w => w.GetBaseDamage().Max).FirstOrDefault();
            //var highestAvg = weapons.OrderByDescending(w => w.GetBaseDamage().Avg).FirstOrDefault();

            return highestMax;
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
    }
}
