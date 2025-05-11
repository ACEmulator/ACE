using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureProfile_Mundane
    {
        // indexed by TreasureDeath.MundaneItemTypeSelectionChances

        private static ChanceTable<TreasureItemType> mundaneProfile1 = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Consumable,     1.0f ),
        };

        private static ChanceTable<TreasureItemType> mundaneProfile2 = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.HealKit,        1.0f ),
        };

        private static ChanceTable<TreasureItemType> mundaneProfile3 = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Lockpick,       1.0f ),
        };

        private static ChanceTable<TreasureItemType> mundaneProfile4 = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.SpellComponent, 1.0f ),
        };

        private static ChanceTable<TreasureItemType> mundaneProfile5 = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.ManaStone,      1.0f ),
        };

        private static ChanceTable<TreasureItemType> mundaneProfile6 = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Pyreal,         1.0f ),
        };

        /// <summary>
        /// The most common MundaneItem profile
        /// </summary>
        private static ChanceTable<TreasureItemType> mundaneProfile7 = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Pyreal,         0.17f ),
            ( TreasureItemType.Consumable,     0.17f ),
            ( TreasureItemType.HealKit,        0.16f ),
            ( TreasureItemType.Lockpick,       0.16f ),
            ( TreasureItemType.SpellComponent, 0.17f ),
            ( TreasureItemType.ManaStone,      0.17f ),
        };

        /// <summary>
        /// The second most common MundaneItem profile
        /// </summary>
        private static ChanceTable<TreasureItemType> mundaneProfile8 = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Pyreal,         0.34f ),
            ( TreasureItemType.SpellComponent, 0.33f ),
            ( TreasureItemType.ManaStone,      0.33f ),
        };

        /// <summary>
        /// TreasureDeath.MundaneItemTypeSelectionChances indexes into these profiles
        /// </summary>
        public static List<ChanceTable<TreasureItemType>> mundaneProfiles = new List<ChanceTable<TreasureItemType>>()
        {
            mundaneProfile1,
            mundaneProfile2,
            mundaneProfile3,
            mundaneProfile4,
            mundaneProfile5,
            mundaneProfile6,
            mundaneProfile7,
            mundaneProfile8,
        };

        /// <summary>
        /// Rolls for a TreasureItemType for a TreasureItemCategory.MundaneItem
        /// </summary>
        /// <param name="mundaneProfile">From TreasureDeath.MundaneItemTypeSelectionChances</param>
        public static TreasureItemType Roll(int mundaneProfile)
        {
            if (mundaneProfile < 1 || mundaneProfile > mundaneProfiles.Count)
                return TreasureItemType.Undef;

            return mundaneProfiles[mundaneProfile - 1].Roll();
        }
    }
}
