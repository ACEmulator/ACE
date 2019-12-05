using System.Collections.Generic;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public readonly Dictionary<Skill, CreatureSkill> Skills = new Dictionary<Skill, CreatureSkill>();

        /// <summary>
        /// This will get a CreatureSkill wrapper around the BiotaPropertiesSkill record for this player.
        /// If the skill doesn't exist for this Biota, one will be created with a status of Untrained.
        /// </summary>
        /// <param name="add">If the skill doesn't exist for this biota, adds it</param>
        public CreatureSkill GetCreatureSkill(Skill skill, bool add = true)
        {
            if (Skills.TryGetValue(skill, out var value))
                return value;

            BiotaPropertiesSkill biotaPropertiesSkill = null;
            if (add)
            {
                biotaPropertiesSkill = Biota.GetOrAddSkill((ushort)skill, BiotaDatabaseLock, out var skillAdded);

                if (skillAdded)
                {
                    biotaPropertiesSkill.SAC = (uint)SkillAdvancementClass.Untrained;
                    ChangesDetected = true;
                }

                Skills[skill] = new CreatureSkill(this, biotaPropertiesSkill);
            }
            else
            {
                biotaPropertiesSkill = Biota.GetSkill((ushort)skill, BiotaDatabaseLock);

                if (biotaPropertiesSkill != null)
                    Skills[skill] = new CreatureSkill(this, biotaPropertiesSkill);
                else
                    return null;
            }

            return Skills[skill];
        }

        public CreatureSkill GetCreatureSkill(MagicSchool skill)
        {
            switch (skill)
            {
                case MagicSchool.CreatureEnchantment:
                    return GetCreatureSkill(Skill.CreatureEnchantment);
                case MagicSchool.ItemEnchantment:
                    return GetCreatureSkill(Skill.ItemEnchantment);
                case MagicSchool.LifeMagic:
                    return GetCreatureSkill(Skill.LifeMagic);
                case MagicSchool.VoidMagic:
                    return GetCreatureSkill(Skill.VoidMagic);
                case MagicSchool.WarMagic:
                    return GetCreatureSkill(Skill.WarMagic);
            }
            return null;
        }


        /// <summary>
        /// This is an IPlayer wrapper that is used by the AllegianceManager to handle passup. You shouldn't be using these anywhere else. Reference GetCreatureSkill() directly.
        /// </summary>
        public uint GetCurrentLoyalty()
        {
            return GetCreatureSkill(Skill.Loyalty).Current;
        }

        /// <summary>
        /// This is an IPlayer wrapper that is used by the AllegianceManager to handle passup. You shouldn't be using these anywhere else. Reference GetCreatureSkill() directly.
        /// </summary>
        public uint GetCurrentLeadership()
        {
            return GetCreatureSkill(Skill.Leadership).Current;
        }
    }
}
