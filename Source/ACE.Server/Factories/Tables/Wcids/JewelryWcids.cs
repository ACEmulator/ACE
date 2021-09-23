using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class JewelryWcids
    {
        // original headwear:
        // - crown

        // added 09-2005 - under cover of night
        // - coronet
        // - circlet
        // - diadem
        // - signet crown
        // - teardrop crown

        // trinkets: drop rate 15% consistently per tier, 2.5% for each of the 6 trinkets
        // scaling the pre-t7 tables to 85% / 15%

        private static ChanceTable<WeenieClassName> T1_T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.amulet,        0.085f ),
            ( WeenieClassName.bracelet,      0.255f ),
            ( WeenieClassName.braceletheavy, 0.085f ),
            ( WeenieClassName.necklace,      0.17f ),
            ( WeenieClassName.ring,          0.2295f ),
            ( WeenieClassName.ringjeweled,   0.0255f ),

            ( WeenieClassName.ace41483_compass,          0.025f ),
            ( WeenieClassName.ace41484_goggles,          0.025f ),
            ( WeenieClassName.ace41487_mechanicalscarab, 0.025f ),
            ( WeenieClassName.ace41486_puzzlebox,        0.025f ),
            ( WeenieClassName.ace41485_pocketwatch,      0.025f ),
            ( WeenieClassName.ace41488_top,              0.025f ),
        };

        private static ChanceTable<WeenieClassName> T3_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.amulet,        0.085f ),
            ( WeenieClassName.bracelet,      0.1275f ),
            ( WeenieClassName.braceletheavy, 0.1275f ),
            ( WeenieClassName.gorget,        0.085f ),
            ( WeenieClassName.necklace,      0.1275f ),
            ( WeenieClassName.necklaceheavy, 0.0425f ),
            ( WeenieClassName.ring,          0.1275f ),
            ( WeenieClassName.ringjeweled,   0.1275f ),

            ( WeenieClassName.ace41483_compass,          0.025f ),
            ( WeenieClassName.ace41484_goggles,          0.025f ),
            ( WeenieClassName.ace41487_mechanicalscarab, 0.025f ),
            ( WeenieClassName.ace41486_puzzlebox,        0.025f ),
            ( WeenieClassName.ace41485_pocketwatch,      0.025f ),
            ( WeenieClassName.ace41488_top,              0.025f ),
        };

        private static ChanceTable<WeenieClassName> T5_T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.amulet,        0.0425f ),
            ( WeenieClassName.bracelet,      0.0425f ),
            ( WeenieClassName.braceletheavy, 0.17f ),

            //( WeenieClassName.crown,         0.085f ),
            ( WeenieClassName.crown,                  0.015f ),     // todo: these possibly dropped in all tiers post-ucon
            ( WeenieClassName.ace31864_teardropcrown, 0.014f ),
            ( WeenieClassName.ace31865_circlet,       0.014f ),
            ( WeenieClassName.ace31866_coronet,       0.014f ),
            ( WeenieClassName.ace31867_diadem,        0.014f ),
            ( WeenieClassName.ace31868_signetcrown,   0.014f ),

            ( WeenieClassName.gorget,        0.085f ),
            ( WeenieClassName.necklace,      0.0425f ),
            ( WeenieClassName.necklaceheavy, 0.1275f ),
            ( WeenieClassName.ring,          0.051f ),
            ( WeenieClassName.ringjeweled,   0.204f ),

            ( WeenieClassName.ace41483_compass,          0.025f ),
            ( WeenieClassName.ace41484_goggles,          0.025f ),
            ( WeenieClassName.ace41487_mechanicalscarab, 0.025f ),
            ( WeenieClassName.ace41486_puzzlebox,        0.025f ),
            ( WeenieClassName.ace41485_pocketwatch,      0.025f ),
            ( WeenieClassName.ace41488_top,              0.025f ),
        };

        private static List<ChanceTable<WeenieClassName>> tierChances = new List<ChanceTable<WeenieClassName>>()
        {
            T1_T2_Chances,
            T1_T2_Chances,
            T3_T4_Chances,
            T3_T4_Chances,
            T5_T6_Chances,
            T5_T6_Chances,
        };

        public static WeenieClassName Roll(int tier)
        {
            // todo: add unique profiles for t7 / t8?
            tier = Math.Clamp(tier, 1, 6);

            return tierChances[tier - 1].Roll();
        }

        private static readonly HashSet<WeenieClassName> _combined = new HashSet<WeenieClassName>();

        static JewelryWcids()
        {
            foreach (var tierChance in tierChances)
            {
                foreach (var entry in tierChance)
                    _combined.Add(entry.result);
            }
        }

        public static bool Contains(WeenieClassName wcid)
        {
            return _combined.Contains(wcid);
        }
    }
}
