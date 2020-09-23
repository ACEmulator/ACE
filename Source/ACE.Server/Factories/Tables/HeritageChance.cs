using System.Collections.Generic;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class HeritageChance
    {
        // this maps to TreasureDeath.UnknownChances

        // this is used as a pre-roll into some of the WeaponWcid tables

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup1 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    1.00f ),
            ( HeritageGroup.Gharundim,  0.00f ),
            ( HeritageGroup.Sho,        0.00f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup2 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.00f ),
            ( HeritageGroup.Gharundim,  1.00f ),
            ( HeritageGroup.Sho,        0.00f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup3 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.00f ),
            ( HeritageGroup.Gharundim,  0.00f ),
            ( HeritageGroup.Sho,        1.00f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup4 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.75f ),
            ( HeritageGroup.Gharundim,  0.25f ),
            ( HeritageGroup.Sho,        0.00f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup5 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.75f ),
            ( HeritageGroup.Gharundim,  0.00f ),
            ( HeritageGroup.Sho,        0.25f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup6 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.25f ),
            ( HeritageGroup.Gharundim,  0.75f ),
            ( HeritageGroup.Sho,        0.00f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup7 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.00f ),
            ( HeritageGroup.Gharundim,  0.75f ),
            ( HeritageGroup.Sho,        0.25f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup8 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.25f ),
            ( HeritageGroup.Gharundim,  0.00f ),
            ( HeritageGroup.Sho,        0.75f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup9 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.00f ),
            ( HeritageGroup.Gharundim,  0.25f ),
            ( HeritageGroup.Sho,        0.75f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup10 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.50f ),
            ( HeritageGroup.Gharundim,  0.50f ),
            ( HeritageGroup.Sho,        0.00f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup11 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.50f ),
            ( HeritageGroup.Gharundim,  0.00f ),
            ( HeritageGroup.Sho,        0.50f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup12 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.00f ),
            ( HeritageGroup.Gharundim,  0.50f ),
            ( HeritageGroup.Sho,        0.50f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup13 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.80f ),
            ( HeritageGroup.Gharundim,  0.10f ),
            ( HeritageGroup.Sho,        0.10f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup14 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.10f ),
            ( HeritageGroup.Gharundim,  0.80f ),
            ( HeritageGroup.Sho,        0.10f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup15 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.10f ),
            ( HeritageGroup.Gharundim,  0.10f ),
            ( HeritageGroup.Sho,        0.80f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup16 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.50f ),
            ( HeritageGroup.Gharundim,  0.25f ),
            ( HeritageGroup.Sho,        0.25f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup17 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.25f ),
            ( HeritageGroup.Gharundim,  0.50f ),
            ( HeritageGroup.Sho,        0.25f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup18 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.25f ),
            ( HeritageGroup.Gharundim,  0.25f ),
            ( HeritageGroup.Sho,        0.50f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup19 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.34f ),
            ( HeritageGroup.Gharundim,  0.33f ),
            ( HeritageGroup.Sho,        0.33f ),
            ( HeritageGroup.Viamontian, 0.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup20 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.00f ),
            ( HeritageGroup.Gharundim,  0.00f ),
            ( HeritageGroup.Sho,        0.00f ),
            ( HeritageGroup.Viamontian, 1.00f ),
        };

        private static readonly ChanceTable<HeritageGroup> heritageChanceGroup21 = new ChanceTable<HeritageGroup>()
        {
            ( HeritageGroup.Aluvian,    0.25f ),
            ( HeritageGroup.Gharundim,  0.25f ),
            ( HeritageGroup.Sho,        0.25f ),
            ( HeritageGroup.Viamontian, 0.25f ),
        };

        /// <summary>
        /// Key is TreasureDeath.UnknownChances (todo: rename)
        /// </summary>
        private static readonly Dictionary<int, ChanceTable<HeritageGroup>> heritageDistGroups = new Dictionary<int, ChanceTable<HeritageGroup>>()
        {
            { 1, heritageChanceGroup1 },
            { 2, heritageChanceGroup2 },
            { 3, heritageChanceGroup3 },
            { 4, heritageChanceGroup4 },
            { 5, heritageChanceGroup5 },
            { 6, heritageChanceGroup6 },
            { 7, heritageChanceGroup7 },
            { 8, heritageChanceGroup8 },
            { 9, heritageChanceGroup9 },
            { 10, heritageChanceGroup10 },
            { 11, heritageChanceGroup11 },
            { 12, heritageChanceGroup12 },
            { 13, heritageChanceGroup13 },
            { 14, heritageChanceGroup14 },
            { 15, heritageChanceGroup15 },
            { 16, heritageChanceGroup16 },
            { 17, heritageChanceGroup17 },
            { 18, heritageChanceGroup18 },
            { 19, heritageChanceGroup19 },
            { 20, heritageChanceGroup20 },
            { 21, heritageChanceGroup21 },
        };

        public static HeritageGroup Roll(int heritageDistGroup)
        {
            if (!heritageDistGroups.TryGetValue(heritageDistGroup, out var table))
            {
                // fallback method - fix the treasure_death.heritage_chances data for new rows
                return (HeritageGroup)ThreadSafeRandom.Next(1, 3);
            }

            var heritageGroup = table.Roll();

            // until additional heritage tables are in,
            // return an even chance of aluvian / gharu'ndim / sho for viamontian+
            if (heritageGroup >= HeritageGroup.Viamontian)
                heritageGroup = (HeritageGroup)ThreadSafeRandom.Next(1, 3);

            return heritageGroup;
        }
    }
}
