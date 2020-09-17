using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureMundaneItemTypeChances_Orig
    {
        // indexed by TreasureDeath.MundaneItemTypeSelectionChances

        private static readonly ChanceTable<TreasureItemType_Orig> mundaneItemTypeChancesGroup1 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.0f ),
            ( TreasureItemType_Orig.Consumable,     1.0f ),
            ( TreasureItemType_Orig.HealKit,        0.0f ),
            ( TreasureItemType_Orig.Lockpick,       0.0f ),
            ( TreasureItemType_Orig.SpellComponent, 0.0f ),
            ( TreasureItemType_Orig.ManaStone,      0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> mundaneItemTypeChancesGroup2 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.0f ),
            ( TreasureItemType_Orig.Consumable,     0.0f ),
            ( TreasureItemType_Orig.HealKit,        1.0f ),
            ( TreasureItemType_Orig.Lockpick,       0.0f ),
            ( TreasureItemType_Orig.SpellComponent, 0.0f ),
            ( TreasureItemType_Orig.ManaStone,      0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> mundaneItemTypeChancesGroup3 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.0f ),
            ( TreasureItemType_Orig.Consumable,     0.0f ),
            ( TreasureItemType_Orig.HealKit,        0.0f ),
            ( TreasureItemType_Orig.Lockpick,       1.0f ),
            ( TreasureItemType_Orig.SpellComponent, 0.0f ),
            ( TreasureItemType_Orig.ManaStone,      0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> mundaneItemTypeChancesGroup4 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.0f ),
            ( TreasureItemType_Orig.Consumable,     0.0f ),
            ( TreasureItemType_Orig.HealKit,        0.0f ),
            ( TreasureItemType_Orig.Lockpick,       0.0f ),
            ( TreasureItemType_Orig.SpellComponent, 1.0f ),
            ( TreasureItemType_Orig.ManaStone,      0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> mundaneItemTypeChancesGroup5 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.0f ),
            ( TreasureItemType_Orig.Consumable,     0.0f ),
            ( TreasureItemType_Orig.HealKit,        0.0f ),
            ( TreasureItemType_Orig.Lockpick,       0.0f ),
            ( TreasureItemType_Orig.SpellComponent, 0.0f ),
            ( TreasureItemType_Orig.ManaStone,      1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> mundaneItemTypeChancesGroup6 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         1.0f ),
            ( TreasureItemType_Orig.Consumable,     0.0f ),
            ( TreasureItemType_Orig.HealKit,        0.0f ),
            ( TreasureItemType_Orig.Lockpick,       0.0f ),
            ( TreasureItemType_Orig.SpellComponent, 0.0f ),
            ( TreasureItemType_Orig.ManaStone,      0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> mundaneItemTypeChancesGroup7 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.17f ),
            ( TreasureItemType_Orig.Consumable,     0.17f ),
            ( TreasureItemType_Orig.HealKit,        0.16f ),
            ( TreasureItemType_Orig.Lockpick,       0.16f ),
            ( TreasureItemType_Orig.SpellComponent, 0.17f ),
            ( TreasureItemType_Orig.ManaStone,      0.17f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> mundaneItemTypeChancesGroup8 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.34f ),
            ( TreasureItemType_Orig.Consumable,     0.0f ),
            ( TreasureItemType_Orig.HealKit,        0.0f ),
            ( TreasureItemType_Orig.Lockpick,       0.0f ),
            ( TreasureItemType_Orig.SpellComponent, 0.33f ),
            ( TreasureItemType_Orig.ManaStone,      0.33f ),
        };

        /// <summary>
        /// TreasureDeath.MundaneItemTypeSelectionChances indexes into these profiles
        /// </summary>
        public static List<ChanceTable<TreasureItemType_Orig>> mundaneItemTypeChancesGroups = new List<ChanceTable<TreasureItemType_Orig>>()
        {
            mundaneItemTypeChancesGroup1,
            mundaneItemTypeChancesGroup2,
            mundaneItemTypeChancesGroup3,
            mundaneItemTypeChancesGroup4,
            mundaneItemTypeChancesGroup5,
            mundaneItemTypeChancesGroup6,
            mundaneItemTypeChancesGroup7,
            mundaneItemTypeChancesGroup8,
        };

        /// <summary>
        /// Rolls for a TreasureItemType for a TreasureItemCategory.MundaneItem
        /// </summary>
        /// <param name="mundaneItemTypeChancesIdx">From TreasureDeath.MundaneItemTypeSelectionChances</param>
        public static TreasureItemType_Orig Roll(int mundaneItemTypeChancesIdx)
        {
            if (mundaneItemTypeChancesIdx < 1 || mundaneItemTypeChancesIdx > mundaneItemTypeChancesGroups.Count)
                return TreasureItemType_Orig.Undef;

            return mundaneItemTypeChancesGroups[mundaneItemTypeChancesIdx - 1].Roll();
        }
    }
}
