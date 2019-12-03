using System;

using ACE.Common.Extensions;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Server.Entity;

namespace ACE.Server.WorldObjects.Entity
{
    public class CreatureSkill
    {
        private readonly Creature creature;

        // The underlying database record
        public readonly BiotaPropertiesSkill BiotaPropertiesSkill;

        public readonly Skill Skill;

        public CreatureSkill(Creature creature, BiotaPropertiesSkill biotaPropertiesSkill)
        {
            this.creature = creature;
            this.BiotaPropertiesSkill = biotaPropertiesSkill;

            Skill = (Skill)biotaPropertiesSkill.Type;
        }

        /// <summary>
        /// A bonus from character creation: +5 for trained, +10 for specialized
        /// </summary>
        public uint InitLevel
        {
            get => BiotaPropertiesSkill.InitLevel;
            set => BiotaPropertiesSkill.InitLevel = value;
        }

        public SkillAdvancementClass AdvancementClass
        {
            get => (SkillAdvancementClass)BiotaPropertiesSkill.SAC;
            set
            {
                if (BiotaPropertiesSkill.SAC != (uint)value)
                    creature.ChangesDetected = true;

                BiotaPropertiesSkill.SAC = (uint)value;
            }
        }

        public bool IsUsable
        {
            get
            {
                if (AdvancementClass == SkillAdvancementClass.Trained || AdvancementClass == SkillAdvancementClass.Specialized)
                    return true;

                if (AdvancementClass == SkillAdvancementClass.Untrained)
                {
                    DatManager.PortalDat.SkillTable.SkillBaseHash.TryGetValue((uint)Skill, out var skillTableRecord);

                    if (skillTableRecord?.MinLevel == 1)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// The amount of experience put into this skill,
        /// from raising directly and earned through use
        /// </summary>
        public uint ExperienceSpent
        {
            get => BiotaPropertiesSkill.PP;
            set
            {
                if (BiotaPropertiesSkill.PP != value)
                    creature.ChangesDetected = true;

                BiotaPropertiesSkill.PP = value;
            }
        }

        /// <summary>
        /// Returns the amount of skill experience remaining
        /// until max rank is reached
        /// </summary>
        public uint ExperienceLeft
        {
            get
            {
                var skillXPTable = Player.GetSkillXPTable(AdvancementClass);
                if (skillXPTable == null)
                    return 0;

                // a player can actually have negative experience remaining,
                // if they had a Trained skill maxed, and then specialized it in skill temple afterwards.

                // (confirmed this is how it was in retail)

                var remainingXP = (long)skillXPTable[skillXPTable.Count - 1] - ExperienceSpent;

                return (uint)Math.Max(0, remainingXP);
            }
        }

        /// <summary>
        /// The number of levels a skill has been raised,
        /// derived from ExperienceSpent
        /// </summary>
        public ushort Ranks
        {
            get => BiotaPropertiesSkill.LevelFromPP;
            set
            {
                if (BiotaPropertiesSkill.LevelFromPP != value)
                    creature.ChangesDetected = true;

                BiotaPropertiesSkill.LevelFromPP = value;
            }
        }

        /// <summary>
        /// Returns TRUE if this skill has been raised the maximum # of times
        /// </summary>
        public bool IsMaxRank
        {
            get
            {
                var skillXPTable = Player.GetSkillXPTable(AdvancementClass);
                if (skillXPTable == null)
                    return false;

                return Ranks >= (skillXPTable.Count - 1);
            }
        }

        public uint Base
        {
            get
            {
                uint total = 0;

                if (IsUsable)
                    total = AttributeFormula.GetFormula(creature, Skill, false);

                total += InitLevel + Ranks;

                if (creature is Player player)
                    total += GetAugBonus(player, false);

                return total;
            }
        }

        public uint Current
        {
            get
            {
                uint total = 0;

                if (IsUsable)
                    total = AttributeFormula.GetFormula(creature, Skill);

                total += InitLevel + Ranks;

                if (creature is Player player)
                {
                    var vitae = player.Vitae;

                    if (vitae != 1.0f)
                        total = (uint)(total * vitae).Round();

                    // everything beyond this point does not get scaled by vitae
                    total += GetAugBonus(player, true);
                }

                var skillMod = creature.EnchantmentManager.GetSkillMod(Skill);

                total = (uint)Math.Max(0, total + skillMod);    // skill level cannot be debuffed below 0

                return total;
            }
        }

        public uint GetAugBonus(Player player, bool current)
        {
            // TODO: verify which of these are base, and which are current
            uint total = 0;

            if (current && player.AugmentationJackOfAllTrades != 0)
                total += (uint)(player.AugmentationJackOfAllTrades * 5);

            if (player.LumAugAllSkills != 0)
                total += (uint)player.LumAugAllSkills;

            if (player.AugmentationSkilledMelee > 0 && Player.MeleeSkills.Contains(Skill))
                total += (uint)(player.AugmentationSkilledMelee * 10);
            else if (player.AugmentationSkilledMissile > 0 && Player.MissileSkills.Contains(Skill))
                total += (uint)(player.AugmentationSkilledMissile * 10);
            else if (player.AugmentationSkilledMagic > 0 && Player.MagicSkills.Contains(Skill))
                total += (uint)(player.AugmentationSkilledMagic * 10);

            switch (Skill)
            {
                case Skill.ArmorTinkering:
                case Skill.ItemTinkering:
                case Skill.MagicItemTinkering:
                case Skill.WeaponTinkering:
                case Skill.Salvaging:

                    if (player.LumAugSkilledCraft != 0)
                        total += (uint)player.LumAugSkilledCraft;
                    break;
            }

            if (AdvancementClass == SkillAdvancementClass.Specialized && player.LumAugSkilledSpec != 0)
                total += (uint)player.LumAugSkilledSpec * 2;

            if (player.Enlightenment != 0)
                total += (uint)player.Enlightenment;

            return total;
        }
    }
}
