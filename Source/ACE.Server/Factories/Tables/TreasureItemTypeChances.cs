using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureItemTypeChances
    {
        // converting to a percentage base roll for items (to better align with retail drop rate data from Magnus) - HarliQ 11/11/19
        // Gems 14%
        // Armor 24%
        // Weapons 30%
        // Clothing 13%
        // Cloaks 1%
        // Jewelry 18%
        // - jewelry - 10%
        // - last 8% - is magical, more jewelry, else dinnerware

        public static readonly List<TreasureItemTypeChance> DefaultMagical = new List<TreasureItemTypeChance>()
        {
            new TreasureItemTypeChance(TreasureItemType.Gem, 0.14f),
            new TreasureItemTypeChance(TreasureItemType.Armor, 0.24f),
            new TreasureItemTypeChance(TreasureItemType.Weapon, 0.30f),
            new TreasureItemTypeChance(TreasureItemType.Clothing, 0.13f),
            new TreasureItemTypeChance(TreasureItemType.Cloak, 0.01f),
            new TreasureItemTypeChance(TreasureItemType.Jewelry, 0.18f)
        };

        public static readonly List<TreasureItemTypeChance> DefaultNonMagical = new List<TreasureItemTypeChance>()
        {
            new TreasureItemTypeChance(TreasureItemType.Gem, 0.14f),
            new TreasureItemTypeChance(TreasureItemType.Armor, 0.24f),
            new TreasureItemTypeChance(TreasureItemType.Weapon, 0.30f),
            new TreasureItemTypeChance(TreasureItemType.Clothing, 0.13f),
            new TreasureItemTypeChance(TreasureItemType.Cloak, 0.01f),
            new TreasureItemTypeChance(TreasureItemType.Jewelry, 0.10f),
            new TreasureItemTypeChance(TreasureItemType.Dinnerware, 0.08f)
        };

        // LootBias.Armor
        public static readonly List<TreasureItemTypeChance> Armor = new List<TreasureItemTypeChance>()
        {
            new TreasureItemTypeChance(TreasureItemType.Armor, 1.0f)
        };

        // LootBias.Weapons
        public static readonly List<TreasureItemTypeChance> Weapons = new List<TreasureItemTypeChance>()
        {
            new TreasureItemTypeChance(TreasureItemType.Weapon, 1.0f)
        };

        // LootBias.Jewelry
        public static readonly List<TreasureItemTypeChance> Jewelry = new List<TreasureItemTypeChance>()
        {
            new TreasureItemTypeChance(TreasureItemType.Jewelry, 1.0f)
        };

        // from Magnus loot logs - Container_Legendary_Chest_T8 (5,826 items)
        // ~32.7% weapons, 35.6% armor, 14.1% clothing, 7.8% jewelry, 4.37% gems, 5.3% misc

        // LootBias.MixedEquipment
        // LootBias.MagicEquipment
        /*public static readonly List<TreasureItemTypeChance> MixedMagicEquipment = new List<TreasureItemTypeChance>()
        {
            new TreasureItemTypeChance(TreasureItemType.Weapon, 0.36f),
            new TreasureItemTypeChance(TreasureItemType.Armor, 0.40f),
            new TreasureItemTypeChance(TreasureItemType.Clothing, 0.15f),
            new TreasureItemTypeChance(TreasureItemType.Jewelry, 0.08f),
            //new TreasureItemTypeChance(TreasureItemType.Gem, 0.05f),    // 100% aetheria
            //new TreasureItemTypeChance(TreasureItemType.Dinnerware, 0.05f)
            new TreasureItemTypeChance(TreasureItemType.Cloak, 0.01f),
        };*/

        public static readonly List<TreasureItemTypeChance> MixedMagicEquipment = new List<TreasureItemTypeChance>()
        {
            new TreasureItemTypeChance(TreasureItemType.Armor, 0.30f),
            new TreasureItemTypeChance(TreasureItemType.Weapon, 0.35f),
            new TreasureItemTypeChance(TreasureItemType.Jewelry, 0.20f),
            new TreasureItemTypeChance(TreasureItemType.Clothing, 0.14f),
            //new TreasureItemTypeChance(TreasureItemType.Gem, 0.05f),    // 100% aetheria
            //new TreasureItemTypeChance(TreasureItemType.Dinnerware, 0.05f)
            new TreasureItemTypeChance(TreasureItemType.Cloak, 0.01f),
        };

        public static TreasureItemType Roll(List<TreasureItemTypeChance> chances)
        {
            var total = 0.0f;

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            foreach (var chance in chances)
            {
                total += chance.Chance;

                if (rng < total)
                    return chance.TreasureItemType;
            }

            // shouldn't happen, floating point imprecision?
            return chances.Last().TreasureItemType;
        }
    }
}
