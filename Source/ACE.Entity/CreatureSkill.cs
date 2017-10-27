using System;
using ACE.Entity.Enum;
using ACE.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ACE.Entity
{
    public class CreatureSkill : ICloneable
    {
        private AceObjectPropertiesSkill _backer;
        private Random _random = new Random();

        // because skill values are determined from stats, we need a reference to the character
        // so we can calculate.  this could be refactored into a better pattern, but it will
        // do for now.
        private ICreatureStats character;
        
        [JsonProperty("skillId")]
        public Skill Skill
        {
            get { return (Skill)_backer.SkillId; }
            protected set
            {
                _backer.SkillId = (ushort)value;
            }
        }

        [JsonProperty("skillStatus")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SkillStatus Status
        {
            get { return (SkillStatus)_backer.SkillStatus; }
            protected set
            {
                _backer.SkillStatus = (ushort)value;
                _backer.IsDirty = true;
            }
        }

        [JsonProperty("ranks")]
        public uint Ranks
        {
            get { return _backer.SkillPoints; }
            set
            {
                _backer.SkillPoints = (ushort)value;
                _backer.IsDirty = true;
            }
        }

        [JsonProperty("baseValue")]
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

        [JsonProperty("experienceSpent")]
        public uint ExperienceSpent
        {
            get { return _backer.SkillXpSpent; }
            set
            {
                _backer.SkillXpSpent = value;
                _backer.IsDirty = true;
            }
        }

        [JsonIgnore]
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

        public void SetDirtyFlags()
        {
            _backer.IsDirty = true;
            _backer.HasEverBeenSavedToDatabase = false;
        }

        public object Clone()
        {
            return new CreatureSkill(this.character, (AceObjectPropertiesSkill)_backer.Clone());
        }

        public double GetPercentSuccess(uint difficulty)
        {
            return GetPercentSuccess(ActiveValue, difficulty);
        }

        public static double GetPercentSuccess(uint skillLevel, uint difficulty)
        {
            float delta = (float)(skillLevel - difficulty);
            var scalar = 1d + Math.Pow(Math.E, 0.03 * delta);
            var percentSuccess = 1d - (1d / scalar);
            return percentSuccess;
        }
    }
}
