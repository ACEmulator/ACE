using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureMundaneItemTypeChances_Orig
    {
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
        /// TreasureDeath.MundaneItemTreasureTypeSelectionChances indexes into these profiles
        /// </summary>
        public static Dictionary<int, ChanceTable<TreasureItemType_Orig>> mundaneItemTypeChancesGroups = new Dictionary<int, ChanceTable<TreasureItemType_Orig>>()
        {
            { 1, mundaneItemTypeChancesGroup1 },
            { 2, mundaneItemTypeChancesGroup2 },
            { 3, mundaneItemTypeChancesGroup3 },
            { 4, mundaneItemTypeChancesGroup4 },
            { 5, mundaneItemTypeChancesGroup5 },
            { 6, mundaneItemTypeChancesGroup6 },
            { 7, mundaneItemTypeChancesGroup7 },
            { 8, mundaneItemTypeChancesGroup8 },
        };
    }
}
