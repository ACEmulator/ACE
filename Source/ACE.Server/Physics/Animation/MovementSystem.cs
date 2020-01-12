using System;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class MovementSystem
    {
        public static float GetJumpHeight(float burden, uint jumpSkill, float power, float scaling)
        {
            power = Math.Clamp(power, 0.0f, 1.0f);

            var result = EncumbranceSystem.GetBurdenMod(burden) * (jumpSkill / (jumpSkill + 1300.0f) * 22.2f + 0.05f) * power / scaling;

            if (result < 0.35f)
                result = 0.35f;

            return result;
        }

        public static double GetRunRate(float burden, int runSkill, float scaling)
        {
            var loadMod = EncumbranceSystem.GetBurdenMod(burden);

            if (runSkill >= 800.0f)     // max run speed?
                return 18.0f / 4.0f;
            else
                return ((loadMod * ((float)runSkill / (runSkill + 200) * 11) + 4) / scaling) / 4.0f;
        }

        public static int JumpStaminaCost(float power, float burden, bool pk)
        {
            if (pk)
                return (int)((power + 1.0f) * 100.0f);
            else
                return (int)Math.Ceiling((burden + 0.5f) * power * 8.0f + 2.0f);
        }

        public static float GetJumpPower(uint stamina, float burden, bool pk)
        {
            if (pk)
                return stamina / 100.0f - 1.0f;
            else
                return (stamina - 2.0f) / (burden * 8.0f + 4.0f);
        }
    }
}
