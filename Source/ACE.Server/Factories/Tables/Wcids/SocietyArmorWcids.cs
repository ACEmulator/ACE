using System.Collections.Generic;

using ACE.Database.Models.World;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class SocietyArmorWcids
    {
        // Rank 1 - Initiate - Gauntlets, Sollerets
        // Rank 2 - Adept    - Greaves, Tassets
        // Rank 3 - Knight   - Pauldrons, Vambraces
        // Rank 4 - Lord     - Breastplate, Girth
        // Rank 5 - Master   - Helm

        private static readonly List<WeenieClassName> CelestialHandWcids = new List<WeenieClassName>()
        {
            WeenieClassName.ace38463_celestialhandbreastplate,
            WeenieClassName.ace38464_celestialhandgauntlets,
            WeenieClassName.ace38465_celestialhandgirth,

            WeenieClassName.ace38466_celestialhandgreaves,
            WeenieClassName.ace38467_celestialhandhelm,
            WeenieClassName.ace38468_celestialhandpauldrons,
            WeenieClassName.ace38469_celestialhandtassets,
            WeenieClassName.ace38470_celestialhandvambraces,
            WeenieClassName.ace38471_celestialhandsollerets,
        };

        private static readonly List<WeenieClassName> EldrytchWebWcids = new List<WeenieClassName>()
        {
            WeenieClassName.ace38472_eldrytchwebbreastplate,
            WeenieClassName.ace38473_eldrytchwebgauntlets,
            WeenieClassName.ace38474_eldrytchwebgirth,

            WeenieClassName.ace38475_eldrytchwebgreaves,
            WeenieClassName.ace38476_eldrytchwebhelm,
            WeenieClassName.ace38477_eldrytchwebpauldrons,
            WeenieClassName.ace38478_eldrytchwebtassets,
            WeenieClassName.ace38479_eldrytchwebvambraces,
            WeenieClassName.ace38480_eldrytchwebsollerets,
        };

        private static readonly List<WeenieClassName> RadiantBloodWcids = new List<WeenieClassName>()
        {
            WeenieClassName.ace38481_radiantbloodbreastplate,
            WeenieClassName.ace38482_radiantbloodgauntlets,
            WeenieClassName.ace38483_radiantbloodgirth,

            WeenieClassName.ace38484_radiantbloodgreaves,
            WeenieClassName.ace38485_radiantbloodhelm,
            WeenieClassName.ace38486_radiantbloodpauldrons,
            WeenieClassName.ace38487_radiantbloodtassets,
            WeenieClassName.ace38488_radiantbloodvambraces,
            WeenieClassName.ace38489_radiantbloodsollerets,
        };

        private static readonly List<List<WeenieClassName>> societyArmorTables = new List<List<WeenieClassName>>()
        {
            CelestialHandWcids,
            EldrytchWebWcids,
            RadiantBloodWcids,
        };

        public static WeenieClassName Roll(TreasureDeath profile, TreasureItemType_Orig treasureItemType)
        {
            // get society from extended heritage chances
            var society = GetSociety(profile);

            if (society == SocietyType.Undef)
                return WeenieClassName.undef;

            var table = societyArmorTables[(int)society - 1];

            var societyArmorType = treasureItemType.GetSocietyArmorType();

            if (societyArmorType == SocietyArmorType.Undef)
                return WeenieClassName.undef;

            return table[(int)societyArmorType - 1];
        }

        public static SocietyType GetSociety(TreasureDeath profile)
        {
            var heritage = HeritageChance.Roll(profile.UnknownChances);

            return heritage.ToSociety();
        }

        private static readonly HashSet<WeenieClassName> _combined = new HashSet<WeenieClassName>();

        static SocietyArmorWcids()
        {
            foreach (var table in societyArmorTables)
            {
                foreach (var wcid in table)
                    _combined.Add(wcid);
            }
        }

        public static bool Contains(WeenieClassName wcid)
        {
            return _combined.Contains(wcid);
        }
    }
}
