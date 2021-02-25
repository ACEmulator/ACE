using ACE.Common;

namespace ACE.Server.Factories.Entity
{
    public static class MissileMagicDefense
    {
        // WeaponMissileDefense / WeaponMagicDefense

        private static readonly ChanceTable<float> T1_T6_Defense = new ChanceTable<float>()
        {
            ( 1.005f, 0.10f ),
            ( 1.010f, 0.20f ),
            ( 1.015f, 0.40f ),
            ( 1.020f, 0.20f ),
            ( 1.025f, 0.10f ),
        };

        private static readonly ChanceTable<float> T7_T8_Defense = new ChanceTable<float>()
        {
            ( 1.005f, 0.05f ),
            ( 1.010f, 0.10f ),
            ( 1.015f, 0.15f ),
            ( 1.020f, 0.20f ),
            ( 1.025f, 0.20f ),
            ( 1.030f, 0.15f ),
            ( 1.035f, 0.10f ),
            ( 1.040f, 0.05f ),
        };

        public static float? Roll(int tier)
        {
            // preliminary roll: 10% chance
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            if (rng >= 0.1f) return null;

            if (tier < 7)
                return T1_T6_Defense.Roll();
            else
                return T7_T8_Defense.Roll();
        }
    }
}
