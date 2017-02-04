using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CharacterAbility
    {
        // because health/stam/mana values are determined from stats, we need a reference to the character
        // so we can calculate.  this could be refactored into a better pattern, but it will
        // do for now.
        private Character _character;

        public Ability Ability { get; private set; }

        public uint Base { get; set; }

        public uint Ranks { get; set; }

        public uint Value
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
                    
                    derivationTotal += end * this._character.Endurance.Value;
                    derivationTotal += wil * this._character.Self.Value;

                    derivationTotal *= formula.AbilityMultiplier;

                    abilityTotal = derivationTotal / formula.Divisor;
                }

                abilityTotal += this.Ranks + this.Base;

                return abilityTotal;
            }
        }

        public uint ExperienceSpent
        {
            get
            {
                // TODO: Implement xp chart if property is necessary.
                return 0;
            }
        }

        public CharacterAbility(Character character, Ability ability)
        {
            _character = character;
            Ability = ability;
        }
    }
}
