using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class HeritageChance
    {
        // this maps to TreasureDeath.UnknownChances

        // this is used as a pre-roll into some of the ClothingWcid, ArmorWcid, and WeaponWcid tables

        private static ChanceTable<TreasureHeritageGroup> heritageProfile1 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    1.00f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile2 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Gharundim,  1.00f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile3 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Sho,        1.00f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile4 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.75f ),
            ( TreasureHeritageGroup.Gharundim,  0.25f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile5 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.75f ),
            ( TreasureHeritageGroup.Sho,        0.25f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile6 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.25f ),
            ( TreasureHeritageGroup.Gharundim,  0.75f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile7 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Gharundim,  0.75f ),
            ( TreasureHeritageGroup.Sho,        0.25f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile8 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.25f ),
            ( TreasureHeritageGroup.Sho,        0.75f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile9 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Gharundim,  0.25f ),
            ( TreasureHeritageGroup.Sho,        0.75f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile10 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.50f ),
            ( TreasureHeritageGroup.Gharundim,  0.50f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile11 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.50f ),
            ( TreasureHeritageGroup.Sho,        0.50f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile12 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Gharundim,  0.50f ),
            ( TreasureHeritageGroup.Sho,        0.50f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile13 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.80f ),
            ( TreasureHeritageGroup.Gharundim,  0.10f ),
            ( TreasureHeritageGroup.Sho,        0.10f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile14 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.10f ),
            ( TreasureHeritageGroup.Gharundim,  0.80f ),
            ( TreasureHeritageGroup.Sho,        0.10f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile15 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.10f ),
            ( TreasureHeritageGroup.Gharundim,  0.10f ),
            ( TreasureHeritageGroup.Sho,        0.80f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile16 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.50f ),
            ( TreasureHeritageGroup.Gharundim,  0.25f ),
            ( TreasureHeritageGroup.Sho,        0.25f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile17 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.25f ),
            ( TreasureHeritageGroup.Gharundim,  0.50f ),
            ( TreasureHeritageGroup.Sho,        0.25f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile18 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.25f ),
            ( TreasureHeritageGroup.Gharundim,  0.25f ),
            ( TreasureHeritageGroup.Sho,        0.50f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile19 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.34f ),
            ( TreasureHeritageGroup.Gharundim,  0.33f ),
            ( TreasureHeritageGroup.Sho,        0.33f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile20 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Viamontian, 1.00f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile21 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.Aluvian,    0.25f ),
            ( TreasureHeritageGroup.Gharundim,  0.25f ),
            ( TreasureHeritageGroup.Sho,        0.25f ),
            ( TreasureHeritageGroup.Viamontian, 0.25f ),
        };

        // added

        private static ChanceTable<TreasureHeritageGroup> heritageProfile22 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.CelestialHand, 1.0f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile23 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.EldrytchWeb,   1.0f ),
        };

        private static ChanceTable<TreasureHeritageGroup> heritageProfile24 = new ChanceTable<TreasureHeritageGroup>()
        {
            ( TreasureHeritageGroup.RadiantBlood,  1.0f ),
        };

        /// <summary>
        /// Key is TreasureDeath.UnknownChances (todo: rename)
        /// </summary>
        private static readonly List<ChanceTable<TreasureHeritageGroup>> heritageProfiles = new List<ChanceTable<TreasureHeritageGroup>>()
        {
            heritageProfile1,
            heritageProfile2,
            heritageProfile3,
            heritageProfile4,
            heritageProfile5,
            heritageProfile6,
            heritageProfile7,
            heritageProfile8,
            heritageProfile9,
            heritageProfile10,
            heritageProfile11,
            heritageProfile12,
            heritageProfile13,
            heritageProfile14,
            heritageProfile15,
            heritageProfile16,
            heritageProfile17,
            heritageProfile18,
            heritageProfile19,
            heritageProfile20,
            heritageProfile21,
            heritageProfile22,
            heritageProfile23,
            heritageProfile24,
        };

        public static TreasureHeritageGroup Roll(int heritageProfile, bool addViamontian = false)
        {
            if (heritageProfile < 1 || heritageProfile > heritageProfiles.Count)
            {
                // fallback method - fix the treasure_death.heritage_chances data for new rows
                return (TreasureHeritageGroup)ThreadSafeRandom.Next(1, 3);
            }

            // convert profile 19 to 21 at runtime?
            if (addViamontian && heritageProfile == 19)
                heritageProfile = 21;

            var heritageGroup = heritageProfiles[heritageProfile - 1].Roll();

            return heritageGroup;
        }
    }
}
