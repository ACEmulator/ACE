using System;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AbilityFormulaAttribute : Attribute
    {
        public Ability Abilities { get; set; }

        public byte Divisor { get; set; }

        public byte AbilityMultiplier { get; set; } = 1;

        public AbilityFormulaAttribute(Ability abilities)
            : this(abilities, 1)
        {
        }

        public AbilityFormulaAttribute(Ability abilities, byte divisor)
        {
            if (divisor == 0)
            {
                throw new ArgumentException("0 not a valid value for " + nameof(divisor));
            }

            Abilities = abilities;
            Divisor = divisor;
        }

        public uint CalcBase(ICreatureStats stats)
        {
            uint sum = 0;
            if (((uint)Abilities & (uint)Ability.Coordination) != 0)
            {
                sum += stats.Coordination;
            }
            if (((uint)Abilities & (uint)Ability.Endurance) != 0)
            {
                sum += stats.Endurance;
            }
            if (((uint)Abilities & (uint)Ability.Focus) != 0)
            {
                sum += stats.Focus;
            }
            if (((uint)Abilities & (uint)Ability.Quickness) != 0)
            {
                sum += stats.Quickness;
            }
            if (((uint)Abilities & (uint)Ability.Self) != 0)
            {
                sum += stats.Self;
            }
            if (((uint)Abilities & (uint)Ability.Strength) != 0)
            {
                sum += stats.Strength;
            }

            return (sum * AbilityMultiplier) / Divisor;
        }
    }
}
