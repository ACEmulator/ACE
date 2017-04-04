using ACE.Entity.Enum;

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

        public CharacterSkill(Character character, Skill skill, SkillStatus status, uint ranks, uint xpSpent)
        {
            this.character = character;
            Skill = skill;
            Status = status;
            Ranks = ranks;
            ExperienceSpent = xpSpent;
        }
    }
}
