using System;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class MovementSystem
    {
        public static float GetJumpHeight(float burden, int jumpSkill, float power, float scaling)
        {
            if (power < 0.0f) power = 0.0f;
            if (power > 1.0f) power = 1.0f;

            var result = EncumbranceSystem.GetBurdenMod(burden) * (jumpSkill / (jumpSkill + 1300) * 22.200001f + 0.050000001f) * power / scaling;

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
    }
}
