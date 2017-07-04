using System;
using ACE.Entity.Enum;
using ACE.Entity;

namespace ACE.Entity
{
    public class CreatureSkill : ICloneable
    {
        private AceObjectPropertiesSkill _backer;

        // because skill values are determined from stats, we need a reference to the character
        // so we can calculate.  this could be refactored into a better pattern, but it will
        // do for now.
        private ICreatureStats character;

        public Skill Skill
        {
            get { return (Skill)_backer.SkillId; }
            protected set
            {
                _backer.SkillId = (ushort)value;
            }
        }

        public SkillStatus Status
        {
            get { return (SkillStatus)_backer.SkillStatus; }
            protected set
            {
                _backer.SkillStatus = (ushort)value;
                _backer.IsDirty = true;
            }
        }

        public uint Ranks
        {
            get { return _backer.SkillPoints; }
            set
            {
                _backer.SkillPoints = (ushort)value;
                _backer.IsDirty = true;
            }
        } 
        
        public uint UnbuffedValue
        {
            get
            {
                // TODO: buffs? Augs? not sure where they will go
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

        public uint ExperienceSpent
        {
            get { return _backer.SkillXpSpent; }
            set
            {
                _backer.SkillXpSpent = value;
                _backer.IsDirty = true;
            }
        }
        public uint ActiveValue
        {
            // FIXME(ddevec) -- buffs?:
            get { return UnbuffedValue; }
        }
        
        public CreatureSkill(ICreatureStats character, Skill skill, SkillStatus status, uint ranks, uint xpSpent)
        {
            this.character = character;
            _backer = new AceObjectPropertiesSkill();
            _backer.AceObjectId = character.AceObjectId;
            Skill = skill;
            Status = status;
            Ranks = ranks;
            ExperienceSpent = xpSpent;
        }

        public CreatureSkill(ICreatureStats character, AceObjectPropertiesSkill skill)
        {
            this.character = character;
            _backer = skill;
        }

        public AceObjectPropertiesSkill GetAceObjectSkill()
        {
            return _backer;
        }

        public void ClearDirtyFlags()
        {
            _backer.IsDirty = false;
            _backer.HasEverBeenSavedToDatabase = true;
        }

        public object Clone()
        {
            return new CreatureSkill(this.character, (AceObjectPropertiesSkill)_backer.Clone());
        }
    }
}
