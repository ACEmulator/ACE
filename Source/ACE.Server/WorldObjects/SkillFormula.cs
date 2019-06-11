using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    public class SkillFormula
    {
        public static readonly float StrengthMod = 0.011f;
        public static readonly float CoordinationMod = 0.008f;

        public static readonly float ArmorMod = 200.0f / 3.0f;

        public static float GetAttributeMod(PropertyAttribute attribute, int current)
        {
            var attributeMod = 0.0f;
            switch (attribute)
            {
                case PropertyAttribute.Strength:
                    attributeMod = StrengthMod;
                    break;
                case PropertyAttribute.Coordination:
                    attributeMod = CoordinationMod;
                    break;
            }
            var attributeBonus = Math.Max(1.0f + (current - 55) * attributeMod, 1.0f);
            return attributeBonus;
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
