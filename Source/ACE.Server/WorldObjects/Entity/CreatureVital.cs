using System;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects.Entity
{
    public class CreatureVital
    {
        private readonly Creature creature;
        private readonly Ability ability;

        public CreatureVital(Creature creature, Ability ability)
        {
            this.creature = creature;
            this.ability = ability;
        }

        public uint GetUnbuffedMaxValue()
        {
            var formula = ability.GetFormula();

            uint derivationTotal = 0;
            uint abilityTotal = 0;

            if (formula != null)
            {
                // restricted to endurance and self because those are the only 2 used by abilities

                Ability abilities = formula.Abilities;
                uint end = (uint)((abilities & Ability.Endurance) > 0 ? 1 : 0);
                uint wil = (uint)((abilities & Ability.Self) > 0 ? 1 : 0);

                derivationTotal += end * creature.Biota.GetAttribute(Ability.Endurance).InitLevel;
                derivationTotal += wil * creature.Biota.GetAttribute(Ability.Self).InitLevel;

                derivationTotal *= formula.AbilityMultiplier;
                abilityTotal = (uint)Math.Ceiling((double)derivationTotal / (double)formula.Divisor);
            }

            abilityTotal += creature.Biota.GetAttribute2nd(ability).LevelFromCP + creature.Biota.GetAttribute2nd(ability).InitLevel;

            return abilityTotal;
        }
    }
}
