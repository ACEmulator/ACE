using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum Ability : uint
    {
        None            = 0x000,
        Strength        = 0x001,
        Endurance       = 0x002,
        Coordination    = 0X004,
        Quickness       = 0x008,
        Focus           = 0x010,
        Self            = 0x020,

        [AbilityRegen(0.5)]
        [AbilityFormula(Endurance, 2)]
        [AbilityVital(Vital.Health)]
        Health          = 0x040,

        [AbilityRegen(1.0)]
        [AbilityFormula(Endurance)]
        [AbilityVital(Vital.Stamina)]
        Stamina         = 0x080,

        [AbilityRegen(0.7)]
        [AbilityFormula(Self)]
        [AbilityVital(Vital.Mana)]
        Mana            = 0x100,
        Full = Strength | Endurance | Quickness | Coordination | Focus | Self | Health | Stamina | Mana
    }

    public static class AbilityExtensions
    {
        public static AbilityFormulaAttribute GetFormula(this Ability ability)
        {
            return ability.GetAttributeOfType<AbilityFormulaAttribute>();
        }

        public static Vital GetVital(this Ability ability)
        {
            return ability.GetAttributeOfType<AbilityVitalAttribute>().Vital;
        }

        // FIXME(ddevec): This will eventually be a formula...
        public static double GetRegenRate(this Ability ability)
        {
            return ability.GetAttributeOfType<AbilityRegenAttribute>().Rate;
        }
    }
}
