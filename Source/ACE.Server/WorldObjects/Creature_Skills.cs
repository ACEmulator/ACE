using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public readonly Dictionary<Skill, CreatureSkill> Skills = new Dictionary<Skill, CreatureSkill>();

        /// <summary>
        /// Will return true if the skill was added, or false if the skill already exists.
        /// </summary>
        public bool AddSkill(Skill skill, SkillStatus skillStatus)
        {
            var result = Biota.BiotaPropertiesSkill.FirstOrDefault(x => x.Type == (uint)skill);

            if (result == null)
            {
                result = new BiotaPropertiesSkill { ObjectId = Biota.Id, Type = (ushort)skill, SAC = (uint)skillStatus };

                Biota.BiotaPropertiesSkill.Add(result);

                Skills[skill] = new CreatureSkill(this, skill);

                return true;
            }

            return false;
        }

        /// <summary>
        /// This will get a CreatureSkill wrapper around the BiotaPropertiesSkill record for this player.
        /// If the skill doesn't exist for this Biota, one will be creatd with a status of Untrained.
        /// </summary>
        public CreatureSkill GetCreatureSkill(Skill skill)
        {
            if (Skills.TryGetValue(skill, out var value))
                return value;

            AddSkill(skill, SkillStatus.Untrained);

            return Skills[skill];
        }
    }
}
