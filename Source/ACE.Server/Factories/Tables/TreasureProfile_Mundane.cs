using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureProfile_Mundane
    {
        // indexed by TreasureDeath.MundaneItemTypeSelectionChances

        private static ChanceTable<TreasureItemType_Orig> mundaneProfile1 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Consumable,     1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> mundaneProfile2 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.HealKit,        1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> mundaneProfile3 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Lockpick,       1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> mundaneProfile4 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SpellComponent, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> mundaneProfile5 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.ManaStone,      1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> mundaneProfile6 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         1.0f ),
        };

        /// <summary>
        /// The most common MundaneItem profile
        /// </summary>
        private static ChanceTable<TreasureItemType_Orig> mundaneProfile7 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.17f ),
            ( TreasureItemType_Orig.Consumable,     0.17f ),
            ( TreasureItemType_Orig.HealKit,        0.16f ),
            ( TreasureItemType_Orig.Lockpick,       0.16f ),
            ( TreasureItemType_Orig.SpellComponent, 0.17f ),
            ( TreasureItemType_Orig.ManaStone,      0.17f ),
        };

        /// <summary>
        /// The second most common MundaneItem profile
        /// </summary>
        private static ChanceTable<TreasureItemType_Orig> mundaneProfile8 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Pyreal,         0.34f ),
            ( TreasureItemType_Orig.SpellComponent, 0.33f ),
            ( TreasureItemType_Orig.ManaStone,      0.33f ),
        };

        /// <summary>
        /// TreasureDeath.MundaneItemTypeSelectionChances indexes into these profiles
        /// </summary>
        public static List<ChanceTable<TreasureItemType_Orig>> mundaneProfiles = new List<ChanceTable<TreasureItemType_Orig>>()
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
        public static TreasureItemType_Orig Roll(int mundaneProfile)
        {
            if (mundaneProfile < 1 || mundaneProfile > mundaneProfiles.Count)
                return TreasureItemType_Orig.Undef;

            return mundaneProfiles[mundaneProfile - 1].Roll();
        }
    }
}
