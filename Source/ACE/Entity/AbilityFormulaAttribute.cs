using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AbilityFormulaAttribute : Attribute
    {
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

        public Ability Abilities { get; set; }

        public byte Divisor { get; set; }

        public byte AbilityMultiplier { get; set; } = 1;
    }
}
