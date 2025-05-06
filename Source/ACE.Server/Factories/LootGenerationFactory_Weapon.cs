using ACE.Common;
using ACE.Database.Models.World;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        /// <summary>
        /// This is only called by /testlootgen command
        /// The actual lootgen system doesn't use this.
        /// </summary>
        private static WorldObject CreateWeapon(TreasureDeath profile, bool isMagical)
        {
            var weaponType = WeaponTypeChance.Roll(profile.Tier);

            if (weaponType.IsMeleeWeapon())
                return CreateMeleeWeapon(profile, isMagical);
            else if (weaponType.IsMissileWeapon())
                return CreateMissileWeapon(profile, isMagical);
            else
                return CreateCaster(profile, isMagical);
        }

        private static float RollWeaponSpeedMod(TreasureDeath treasureDeath)
        {
            var qualityLevel = QualityChance.Roll(treasureDeath);

            if (qualityLevel == 0)
                return 1.0f;    // no bonus

            var rng = (float)ThreadSafeRandom.Next(-0.025f, 0.025f);

            // min/max range: 67.5% - 100%
            var weaponSpeedMod = 1.0f - (qualityLevel * 0.025f + rng);

            //Console.WriteLine($"WeaponSpeedMod: {weaponSpeedMod}");

            return weaponSpeedMod;
        }
    }
}
