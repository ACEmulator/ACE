using System.Collections.Generic;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class CoalescedManaWcids
    {
        private static ChanceTable<WeenieClassName> T1_T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.ace42518_coalescedmana, 1.0f ),
        };

        private static ChanceTable<WeenieClassName> T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.ace42518_coalescedmana, 0.75f ),
            ( WeenieClassName.ace42517_coalescedmana, 0.25f ),
        };

        private static ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.ace42518_coalescedmana, 0.25f ),
            ( WeenieClassName.ace42517_coalescedmana, 0.50f ),
            ( WeenieClassName.ace42516_coalescedmana, 0.25f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> tierChances = new List<ChanceTable<WeenieClassName>>()
        {
            T1_T2_Chances,
            T1_T2_Chances,
            T3_Chances,
            T4_Chances,
        };

        public static WeenieClassName Roll(TreasureDeath profile)
        {
            if (profile.Tier > 4)
                return WeenieClassName.undef;

            var table = tierChances[profile.Tier - 1];

            return table.Roll(profile.LootQualityMod);
        }
    }
}
