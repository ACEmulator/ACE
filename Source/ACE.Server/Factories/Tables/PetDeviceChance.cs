using System.Collections.Generic;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class PetDeviceChance
    {
        private static ChanceTable<int> T1_T3_PetLevelChances = new ChanceTable<int>()
        {
            ( 50, 1.0f )
        };

        private static ChanceTable<int> T4_PetLevelChances = new ChanceTable<int>()
        {
            ( 50, 0.75f ),
            ( 80, 0.25f )
        };

        private static ChanceTable<int> T5_PetLevelChances = new ChanceTable<int>()
        {
            ( 50,  0.15f ),
            ( 80,  0.65f ),
            ( 100, 0.20f ),
        };

        private static ChanceTable<int> T6_PetLevelChances = new ChanceTable<int>()
        {
            ( 80,  0.15f ),
            ( 100, 0.25f ),
            ( 125, 0.50f ),
            ( 150, 0.10f ),
        };

        private static ChanceTable<int> T7_PetLevelChances = new ChanceTable<int>()
        {
            ( 100, 0.15f ),
            ( 125, 0.25f ),
            ( 150, 0.50f ),
            ( 180, 0.10f ),
        };

        private static ChanceTable<int> T8_PetLevelChances = new ChanceTable<int>()
        {
            ( 100, 0.0125f ),
            ( 125, 0.025f ),
            ( 150, 0.05f ),
            ( 180, 0.50f ),
            ( 200, 0.4125f ),
        };

        private static readonly List<ChanceTable<int>> petLevelChances = new List<ChanceTable<int>>()
        {
            T1_T3_PetLevelChances,
            T1_T3_PetLevelChances,
            T1_T3_PetLevelChances,
            T4_PetLevelChances,
            T5_PetLevelChances,
            T6_PetLevelChances,
            T7_PetLevelChances,
            T8_PetLevelChances,
        };

        /// <summary>
        /// Rolls for a CombatPet level for a PetDevice
        /// </summary>
        public static int Roll(TreasureDeath profile)
        {
            var table = petLevelChances[profile.Tier - 1];

            return table.Roll(profile.LootQualityMod);
        }
    }
}
