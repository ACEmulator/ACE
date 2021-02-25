using System;
using System.Collections.Generic;
using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class HealKitWcids
    {
        private static ChanceTable<WeenieClassName> T1_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healingkitcrude,     0.75f ),
            ( WeenieClassName.healingkitplain,     0.25f ),
        };

        private static ChanceTable<WeenieClassName> T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healingkitcrude,     0.25f ),
            ( WeenieClassName.healingkitplain,     0.50f ),
            ( WeenieClassName.healingkitgood,      0.25f ),
        };

        private static ChanceTable<WeenieClassName> T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healingkitplain,     0.25f ),
            ( WeenieClassName.healingkitgood,      0.50f ),
            ( WeenieClassName.healingkitexcellent, 0.25f ),
        };

        private static ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healingkitgood,      0.25f ),
            ( WeenieClassName.healingkitexcellent, 0.50f ),
            ( WeenieClassName.healingkitpeerless,  0.25f ),
        };

        private static ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healingkitexcellent, 0.25f ),
            ( WeenieClassName.healingkitpeerless,  0.50f ),
            ( WeenieClassName.healingkittreated,   0.25f ),
        };

        private static ChanceTable<WeenieClassName> T6_T8_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healingkitpeerless,  0.25f ),
            ( WeenieClassName.healingkittreated,   0.75f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> healKitTiers = new List<ChanceTable<WeenieClassName>>()
        {
            T1_Chances,
            T2_Chances,
            T3_Chances,
            T4_Chances,
            T5_Chances,
            T6_T8_Chances,
            T6_T8_Chances,
            T6_T8_Chances,
        };

        public static WeenieClassName Roll(TreasureDeath profile)
        {
            // todo: verify t7 / t8 chances
            var table = healKitTiers[profile.Tier - 1];

            return table.Roll(profile.LootQualityMod);
        }
    }
}
