using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.WorldObjects
{
    public class SkillCheck
    {
        public static double GetSkillChance(int skill, int difficulty)
        {
            var chance = 1.0 - (1.0 / (1.0 + Math.Exp(0.03 * (skill - difficulty))));

            return Math.Min(1.0, Math.Max(0.0, chance));
        }
    }
}
