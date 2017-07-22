using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum Ability : uint
    {
        None            = 0,
        Strength        = 1,
        Endurance       = 2,
        Coordination    = 4,        
        Quickness       = 8,
        Focus           = 16,
        Self            = 32,

        [AbilityRegen(0.5)]
        [AbilityFormula(Endurance, 2)]
        Health          = 64,

        [AbilityRegen(1.0)]
        [AbilityFormula(Endurance)]
        Stamina         = 128,

        [AbilityRegen(0.7)]
        [AbilityFormula(Self)]
        Mana            = 256
    }

    public static class AbilityExtensions
    {
        public static AbilityFormulaAttribute GetFormula(this Ability ability)
        {
            return Enum.EnumHelper.GetAttributeOfType<AbilityFormulaAttribute>(ability);
        }

        // FIXME(ddevec): This will eventually be a formula...
        public static double GetRegenRate(this Ability ability)
        {
            return Enum.EnumHelper.GetAttributeOfType<AbilityRegenAttribute>(ability).Rate;
        }
    }
}
