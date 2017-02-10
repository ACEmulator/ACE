using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Entity
{
    public class CharacterSkill
    {
        // because skill values are determined from stats, we need a reference to the character
        // so we can calculate.  this could be refactored into a better pattern, but it will
        // do for now.
        private Character character;

        public Skill Skill { get; private set; }

        public SkillStatus Status { get; private set; }

        public uint Ranks { get; set; } 
        
        public uint UnbuffedValue
        {
            get
            {
                // TODO: buffs?  not sure where they will go

                var formula = this.Skill.GetFormula();

                uint abilityTotal = 0;
                uint skillTotal = 0;

                if (formula != null)
                {
                    if ((Status == SkillStatus.Untrained && Skill.GetUsability().UsableUntrained) ||
                        Status == SkillStatus.Trained ||
                        Status == SkillStatus.Specialized)
                    {
                        Ability abilities = formula.Abilities;
                        uint str = (uint)((abilities & Ability.Strength) > 0 ? 1 : 0);
                        uint end = (uint)((abilities & Ability.Endurance) > 0 ? 1 : 0);
                        uint coo = (uint)((abilities & Ability.Coordination) > 0 ? 1 : 0);
                        uint qui = (uint)((abilities & Ability.Quickness) > 0 ? 1 : 0);
                        uint foc = (uint)((abilities & Ability.Focus) > 0 ? 1 : 0);
                        uint wil = (uint)((abilities & Ability.Self) > 0 ? 1 : 0);

                        abilityTotal += str * this.character.Strength.UnbuffedValue;
                        abilityTotal += end * this.character.Endurance.UnbuffedValue;
                        abilityTotal += coo * this.character.Coordination.UnbuffedValue;
                        abilityTotal += qui * this.character.Quickness.UnbuffedValue;
                        abilityTotal += foc * this.character.Focus.UnbuffedValue;
                        abilityTotal += wil * this.character.Self.UnbuffedValue;

                        abilityTotal *= formula.AbilityMultiplier;
                    }

                    skillTotal = abilityTotal / formula.Divisor;
                }

                skillTotal += this.Ranks;

                return skillTotal;
            }
        }

        public uint ExperienceSpent { get; set; }

        public CharacterSkill(Character character, Skill skill, SkillStatus status, uint ranks)
        {
            this.character = character;
            Skill = skill;
            Status = status;
            Ranks = ranks;
        }

        /// <summary>
        /// spends the xp on this skill.
        /// </summary>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        public uint SpendXp(uint amount)
        {
            uint result = 0u;
            List<Tuple<uint, uint, uint>> chart;

            if (Status == SkillStatus.Trained)
                chart = SkillExtensions.TrainedChart;
            else if (Status == SkillStatus.Specialized)
                chart = SkillExtensions.SpecializedChart;
            else
                return result;

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
                this.Ranks += rankUps;
                this.ExperienceSpent += amount;
                this.character.SpendXp(amount);
                result = this.ExperienceSpent;
            }

            return result;
        }
    }
}
