using System;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects.Entity
{
    public class CreatureSkill
    {
        private readonly Creature creature;
        public readonly Skill Skill;

        // This is the underlying database record
        private readonly BiotaPropertiesSkill biotaPropertiesSkill;

        public CreatureSkill(Creature creature, Skill skill)
        {
            this.creature = creature;
            Skill = skill;

            biotaPropertiesSkill = creature.Biota.GetProperty(skill);
        }

        public SkillStatus Status
        {
            get => (SkillStatus)biotaPropertiesSkill.SAC;
            protected set => biotaPropertiesSkill.SAC = (uint)value;
        }

        public ushort Ranks
        {
            get => biotaPropertiesSkill.LevelFromPP;
            set => biotaPropertiesSkill.LevelFromPP = value;
        }

        public uint UnbuffedValue
        {
            get
            {
                // TODO: buffs? Augs? not sure where they will go
                var formula = Skill.GetFormula();

                uint skillTotal = 0;

                if (formula != null)
                {
                    if ((Status == SkillStatus.Untrained && Skill.GetUsability().UsableUntrained) ||
                        Status == SkillStatus.Trained ||
                        Status == SkillStatus.Specialized)
                    {
                        throw new NotImplementedException(); // todo fix for new EF model
                        //skillTotal = formula.CalcBase(creature);
                    }
                }

                skillTotal += Ranks;

                return skillTotal;
            }
        }

        public uint ExperienceSpent
        {
            get => biotaPropertiesSkill.PP;
            set => biotaPropertiesSkill.PP = value;
        }

        public uint ActiveValue
        {
            get
            {
                // FIXME(ddevec) -- buffs?:
                return UnbuffedValue;
            }
        }

        public double GetPercentSuccess(uint difficulty)
        {
            return GetPercentSuccess(ActiveValue, difficulty);
        }

        public static double GetPercentSuccess(uint skillLevel, uint difficulty)
        {
            float delta = skillLevel - difficulty;
            var scalar = 1d + Math.Pow(Math.E, 0.03 * delta);
            var percentSuccess = 1d - (1d / scalar);
            return percentSuccess;
        }
    }
}
