using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    public class SkillFormula
    {
        // everything else: melee weapons (including finesse), thrown weapons, atlatls
        public static readonly float DefaultMod = 0.011f;

        // bows and crossbows
        public static readonly float BowMod = 0.008f;

        public static readonly float ArmorMod = 200.0f / 3.0f;

        public static float GetAttributeMod(int currentSkill, bool isBow = false)
        {
            var factor = isBow ? BowMod : DefaultMod;

            return Math.Max(1.0f + (currentSkill - 55) * factor, 1.0f);
        }

        /// <summary>
        /// Converts AL from an additive linear value
        /// to a scaled damage multiplier
        /// </summary>
        public static float CalcArmorMod(float armorLevel)
        {
            if (armorLevel > 0)
                return ArmorMod / (armorLevel + ArmorMod);
            else if (armorLevel < 0)
                return 1.0f - armorLevel / ArmorMod;
            else
                return 1.0f;
        }
    }
}
