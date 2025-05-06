using ACE.Common;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
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

        private static int GeneratePetDeviceRating(int tier)
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
