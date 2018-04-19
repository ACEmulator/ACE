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
            set => biotaPropertiesSkill.SAC = (uint)value;
        }

        /// <summary>
        /// Total experience for this skill,
        /// both spent and earned
        /// </summary>
        public uint ExperienceSpent
        {
            get => biotaPropertiesSkill.PP;
            set => biotaPropertiesSkill.PP = value;
        }

        public ushort Ranks
        {
            get => biotaPropertiesSkill.LevelFromPP;
            set => biotaPropertiesSkill.LevelFromPP = value;
        }

        public uint Base
        {
            get
            {
                var formula = Skill.GetFormula();

                uint total = 0;

                if (formula != null)
                {
                    if ((Status == SkillStatus.Untrained && Skill.GetUsability().UsableUntrained) || Status == SkillStatus.Trained || Status == SkillStatus.Specialized)
                        total = formula.CalcBase(creature.Strength.Base, creature.Endurance.Base, creature.Coordination.Base, creature.Quickness.Base, creature.Focus.Base, creature.Self.Base);
                }

                total += Ranks;

                // TODO: augs

                return total;
            }
        }

        public uint Current
        {
            get
            {
                var formula = Skill.GetFormula();

                uint total = 0;

                if (formula != null)
                {
                    if ((Status == SkillStatus.Untrained && Skill.GetUsability().UsableUntrained) || Status == SkillStatus.Trained || Status == SkillStatus.Specialized)
                        total = formula.CalcBase(creature.Strength.Current, creature.Endurance.Current, creature.Coordination.Current, creature.Quickness.Current, creature.Focus.Current, creature.Self.Current);
                }

                total += Ranks;

                var skillMod = creature.EnchantmentManager.GetSkillMod(Skill);
                total += (uint)skillMod;    // can be negative?

                // TODO: include augs + any other modifiers
                if (creature is Player)
                {
                    var player = creature as Player;

                    if (player.HasVitae)
                        total = (uint)Math.Round(total * player.Vitae);
                }
                return total;
            }
        }

        public double GetPercentSuccess(uint difficulty)
        {
            return GetPercentSuccess(Current, difficulty);
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
