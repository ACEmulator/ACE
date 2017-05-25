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

                var formula = Skill.GetFormula();

                uint abilityTotal = 0;
                uint skillTotal = 0;

                if (formula != null)
                {
                    if ((Status == SkillStatus.Untrained && Skill.GetUsability().UsableUntrained) ||
                        Status == SkillStatus.Trained ||
                        Status == SkillStatus.Specialized)
                    {
                        var abilities = formula.Abilities;
                        var str = (uint)((abilities & Ability.Strength) > 0 ? 1 : 0);
                        var end = (uint)((abilities & Ability.Endurance) > 0 ? 1 : 0);
                        var coo = (uint)((abilities & Ability.Coordination) > 0 ? 1 : 0);
                        var qui = (uint)((abilities & Ability.Quickness) > 0 ? 1 : 0);
                        var foc = (uint)((abilities & Ability.Focus) > 0 ? 1 : 0);
                        var wil = (uint)((abilities & Ability.Self) > 0 ? 1 : 0);

                        abilityTotal += str * character.StrengthAbility.UnbuffedValue;
                        abilityTotal += end * character.EnduranceAbility.UnbuffedValue;
                        abilityTotal += coo * character.CoordinationAbility.UnbuffedValue;
                        abilityTotal += qui * character.QuicknessAbility.UnbuffedValue;
                        abilityTotal += foc * character.FocusAbility.UnbuffedValue;
                        abilityTotal += wil * character.SelfAbility.UnbuffedValue;

                        abilityTotal *= formula.AbilityMultiplier;
                    }

                    skillTotal = abilityTotal / formula.Divisor;
                }

                skillTotal += Ranks;

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
