using ACE.Common;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateSummoningEssence(int tier, bool mutate = true)
        {
            // Adding a spread of Pet Device levels for each tier - Level 200 pets should only be dropping in T8 Loot - HQ 2/29/2020
            // The spread is from Optim's Data
            // T5-T8 20/35/30/15% split 
            // T8- 200,180,150,125
            // T7- 180,150,125,100
            // T6- 150,125,100,80
            // T5- 125,100,80,50
            // T4- 100,80,50
            // T3- 80,50
            // T2- 50
            // T1- 50

            // Tables are already 1-7, so removing them being Tier dependent

            int petLevel = 0;
            int chance = ThreadSafeRandom.Next(1, 100);
            if (chance > 80)
                petLevel = tier - 1;
            else if (chance > 45)
                petLevel = tier - 2;
            else if (chance > 15)
                petLevel = tier - 3;
            else
                petLevel = tier - 4;
            if (petLevel < 2)
                petLevel = 1;

            int summoningEssenceIndex = ThreadSafeRandom.Next(0, LootTables.SummoningEssencesMatrix.Length - 1);

            var id = (uint)LootTables.SummoningEssencesMatrix[summoningEssenceIndex][petLevel - 1];

            var petDevice = WorldObjectFactory.CreateNewWorldObject(id) as PetDevice;

            if (petDevice != null && mutate)
                MutatePetDevice(petDevice, tier);

            return petDevice;
        }

        private static void MutatePetDevice(WorldObject petDevice, int tier)
        {
            if (!(petDevice is PetDevice)) return;

            var ratingChance = 0.5f;

            // add rng ratings to pet device
            // linear or biased?
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearDamage = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearDamageResist = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritDamage = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritDamageResist = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCrit = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritResist = GeneratePetDeviceRating(tier);

            petDevice.ItemWorkmanship = WorkmanshipChance.Roll(tier);
        }

        public static int GeneratePetDeviceRating(int tier)
        {
            // thanks to morosity for this formula!
            var baseRating = ThreadSafeRandom.Next(1, 10);

            var chance = 0.4f + tier * 0.02f;
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            if (rng < chance)
                baseRating += ThreadSafeRandom.Next(1, 10);

            return baseRating;
        }
    }
}
