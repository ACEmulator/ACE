using System;
using System.Collections.Generic;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum Ability : uint
    {
        None = 0,
        Strength = 1,
        Coordination = 2,
        Endurance = 4,
        Quickness = 8,
        Focus = 16,
        Self = 32,

        [AbilityFormula(Endurance, 2)]
        Health = 64,

        [AbilityFormula(Endurance)]
        Stamina = 128,

        [AbilityFormula(Self)]
        Mana = 256
    }

    public static class AbilityExtensions
    {
        public static AbilityFormulaAttribute GetFormula(this Ability ability)
        {
            return Enum.EnumHelper.GetAttributeOfType<AbilityFormulaAttribute>(ability);
        }
    }
}
