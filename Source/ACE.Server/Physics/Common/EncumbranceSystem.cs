namespace ACE.Server.Physics.Common
{
    public class EncumbranceSystem
    {
        public static int EncumbranceCapacity(int strength, int augments)
        {
            if (strength <= 0) return 0;

            var bonusBurden = 30 * augments;

            if (bonusBurden >= 0)
            {
                if (bonusBurden > 150)
                    bonusBurden = 150;

                return 150 * strength + strength * bonusBurden;
            }
            else
                return 150 * strength;
        }

        public static float Load(int capacity, int encumbrance)
        {
            if (capacity <= 0) return 3.0f;

            if (encumbrance >= 0)
                return encumbrance / capacity;
            else
                return 0.0f;
        }

        public static float LoadMod(float load)
        {
            if (load < 1.0f) return 1.0f;

            if (load < 2.0f)
                return 2.0f - load;
            else
                return 0.0f;
        }
    }
}
