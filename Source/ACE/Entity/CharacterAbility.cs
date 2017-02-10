using ACE.Entity.Enum;
using System;
using System.Collections.Generic;

namespace ACE.Entity
{
    public class CharacterAbility
    {
        // because health/stam/mana values are determined from stats, we need a reference to the character
        // so we can calculate.  this could be refactored into a better pattern, but it will
        // do for now.
        private Character character;

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

                    derivationTotal += end * this.character.Endurance.UnbuffedValue;
                    derivationTotal += wil * this.character.Self.UnbuffedValue;

                    derivationTotal *= formula.AbilityMultiplier;

                    abilityTotal = derivationTotal / formula.Divisor;
                }

                abilityTotal += this.Ranks + this.Base;

                return abilityTotal;
            }
        }

        public uint ExperienceSpent { get; set; }

        public CharacterAbility(Character character, Ability ability)
        {
            this.character = character;
            Ability = ability;
        }

        /// <summary>
        /// spends the xp on this ability.
        /// </summary>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        public uint SpendXp(uint amount)
        {
            uint result = 0;
            bool addToCurrentValue = false;
            List<Tuple<uint, uint, uint>> chart;

            switch (this.Ability)
            {
                case Ability.Health:
                case Ability.Stamina:
                case Ability.Mana:
                    chart = AbilityExtensions.SecondaryAbilityChart;
                    addToCurrentValue = true;
                    break;
                default:
                    chart = AbilityExtensions.PrimaryAbilityChart;
                    break;
            }

            uint rankUps = 0u;
            uint currentXp = chart[Convert.ToInt32(this.Ranks)].Item2;
            uint rank1 = chart[Convert.ToInt32(this.Ranks) + 1].Item3;
            uint rank10 = chart[Convert.ToInt32(this.Ranks) + 10].Item2 - chart[Convert.ToInt32(this.Ranks)].Item2;

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
                rankUps = 10u;

            if (rankUps > 0)
            {
                this.Current += addToCurrentValue ? rankUps : 0u;
                this.Ranks += rankUps;
                this.ExperienceSpent += amount;
                this.character.SpendXp(amount);
                result = this.ExperienceSpent;
            }

            return result;
        }
    }
}