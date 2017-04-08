using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CreatureAbility
    {
        // because health/stam/mana values are determined from stats, we need a reference to the WeenieCreatureData
        // so we can calculate.  this could be refactored into a better pattern, but it will do for now.
        private AceCreatureObject creature;

        public Ability Ability { get; private set; }

        public uint Base { get; set; }

        public uint Ranks { get; set; }

        /// <summary>
        /// only applies to Health/Stam/Mana
        /// </summary>
        public uint Current { get; set; }

        public uint UnbuffedValue
        {
            get
            {
                // TODO: buffs?  not sure where they will go

                var formula = this.Ability.GetFormula();

                uint derivationTotal = 0;
                uint abilityTotal = 0;

                if (formula != null)
                {
                    // restricted to endurance and self because those are the only 2 used by abilities

                    Ability abilities = formula.Abilities;
                    uint end = (uint)((abilities & Ability.Endurance) > 0 ? 1 : 0);
                    uint wil = (uint)((abilities & Ability.Self) > 0 ? 1 : 0);

                    derivationTotal += end * this.creature.Endurance;
                    derivationTotal += wil * this.creature.Self;

                    derivationTotal *= formula.AbilityMultiplier;

                    abilityTotal = derivationTotal / formula.Divisor;
                }

                abilityTotal += this.Ranks + this.Base;

                return abilityTotal;
            }
        }

        public uint MaxValue
        {
            get
            {
                // TODO: once buffs are implemented, make sure we have a max Value wich calculates the buffs in, 
                // as it's needed for the UpdateHealth GameMessage. For now this is just the unbuffed value.

                return UnbuffedValue;
            }
        }

        public uint ExperienceSpent { get; set; }

        public CreatureAbility(AceCreatureObject creature, Ability ability)
        {
            this.creature = creature;
            Ability = ability;
        }
    }
}
