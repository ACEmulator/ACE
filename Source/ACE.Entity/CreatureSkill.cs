using ACE.Entity.Enum;
using ACE.Entity;

namespace ACE.Entity
{
    public class CreatureSkill
    {
        // because skill values are determined from stats, we need a reference to the character
        // so we can calculate.  this could be refactored into a better pattern, but it will
        // do for now.
        private ICreatureStats character;

        public Skill Skill { get; private set; }

        public SkillStatus Status { get; private set; }

        public uint Ranks { get; set; } 
        
        public uint UnbuffedValue
        {
            get
            {
                // TODO: buffs?  not sure where they will go
                var formula = this.Skill.GetFormula();

                uint skillTotal = 0;

                if (formula != null)
                {
                    if ((Status == SkillStatus.Untrained && Skill.GetUsability().UsableUntrained) ||
                        Status == SkillStatus.Trained ||
                        Status == SkillStatus.Specialized)
                    {
                        skillTotal = formula.CalcBase(character);
                    }
                }

                skillTotal += this.Ranks;

                return skillTotal;
            }
        }

        public uint ExperienceSpent { get; set; }

        public CreatureSkill(ICreatureStats character, Skill skill, SkillStatus status, uint ranks, uint xpSpent)
        {
            this.character = character;
            Skill = skill;
            Status = status;
            Ranks = ranks;
            ExperienceSpent = xpSpent;
        }
    }
}
