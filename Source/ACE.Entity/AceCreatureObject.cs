using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Entity
{
    [DbTable("vw_ace_creature_object")]
    [DbGetList("vw_ace_creature_object", 42, "weenieClassId")]
    public class AceCreatureObject : AceObject, ICreatureStats
    {
        protected Dictionary<Ability, CreatureAbility> abilities = new Dictionary<Ability, CreatureAbility>();

        protected Dictionary<Skill, CreatureSkill> skills = new Dictionary<Skill, CreatureSkill>();

        public AceCreatureObject()
        {
            abilities.Add(Ability.Strength, new CreatureAbility(this, Ability.Strength));
            abilities.Add(Ability.Strength, new CreatureAbility(this, Ability.Endurance));
            abilities.Add(Ability.Endurance, new CreatureAbility(this, Ability.Coordination));
            abilities.Add(Ability.Quickness, new CreatureAbility(this, Ability.Quickness));
            abilities.Add(Ability.Focus, new CreatureAbility(this, Ability.Focus));
            abilities.Add(Ability.Self, new CreatureAbility(this, Ability.Self));

            abilities.Add(Ability.Health, new CreatureAbility(this, Ability.Health));
            abilities.Add(Ability.Stamina, new CreatureAbility(this, Ability.Stamina));
            abilities.Add(Ability.Mana, new CreatureAbility(this, Ability.Mana));
        }

        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public override uint WeenieClassId { get; set; }

        [DbField("baseAceObjectId", (int)MySqlDbType.UInt32)]
        public override uint AceObjectId { get; set; }

        public CreatureAbility StrengthAbility
        {
            get { return abilities[Ability.Strength]; }
            set { abilities[Ability.Strength] = value; }
        }

        public CreatureAbility EnduranceAbility
        {
            get { return abilities[Ability.Endurance]; }
            set { abilities[Ability.Endurance] = value; }
        }

        public CreatureAbility CoordinationAbility
        {
            get { return abilities[Ability.Coordination]; }
            set { abilities[Ability.Coordination] = value; }
        }

        public CreatureAbility QuicknessAbility
        {
            get { return abilities[Ability.Quickness]; }
            set { abilities[Ability.Quickness] = value; }
        }

        public CreatureAbility FocusAbility
        {
            get { return abilities[Ability.Focus]; }
            set { abilities[Ability.Focus] = value; }
        }

        public CreatureAbility SelfAbility
        {
            get { return abilities[Ability.Self]; }
            set { abilities[Ability.Self] = value; }
        }

        public CreatureAbility Health
        {
            get { return abilities[Ability.Health]; }
            set { abilities[Ability.Health] = value; }
        }

        public CreatureAbility Stamina
        {
            get { return abilities[Ability.Stamina]; }
            set { abilities[Ability.Stamina] = value; }
        }

        public CreatureAbility Mana
        {
            get { return abilities[Ability.Mana]; }
            set { abilities[Ability.Mana] = value; }
        }

        public uint Strength
        { get { return StrengthAbility.UnbuffedValue; } }

        public uint Endurance
        { get { return EnduranceAbility.UnbuffedValue; } }

        public uint Coordination
        { get { return CoordinationAbility.UnbuffedValue; } }

        public uint Quickness
        { get { return QuicknessAbility.UnbuffedValue; } }

        public uint Focus
        { get { return FocusAbility.UnbuffedValue; } }

        public uint Self
        { get { return SelfAbility.UnbuffedValue; } }

        [DbField("luminance", (int)MySqlDbType.Byte)]
        public byte Luminance { get; set; }

        [DbField("lootTier", (int)MySqlDbType.Byte)]
        public byte LootTier { get; set; }

        public uint Level
        {
            get { return GetIntProperty(PropertyInt.Level) ?? 0; }
            set { SetIntProperty(PropertyInt.Level, value); }
        }

        public List<WeeniePaletteOverride> WeeniePaletteOverrides { get; set; } = new List<WeeniePaletteOverride>();

        public List<WeenieTextureMapOverride> WeenieTextureMapOverrides { get; set; } = new List<WeenieTextureMapOverride>();

        public List<WeenieAnimationOverride> WeenieAnimationOverrides { get; set; } = new List<WeenieAnimationOverride>();
        
        public CreatureAbility GetAbility(Ability ability)
        {
            if (abilities.ContainsKey(ability))
                return abilities[ability];

            return null;
        }

        public CreatureSkill GetSkill(Skill skill)
        {
            if (!skills.ContainsKey(skill))
            {
                skills.Add(skill, new CreatureSkill(this, skill, SkillStatus.Untrained, 0, 0));
            }

            return skills[skill];
        }

        public void LoadSkills(List<CreatureSkill> newSkills)
        {
            this.skills = new Dictionary<Skill, CreatureSkill>();
            newSkills.ForEach(s => this.skills.Add(s.Skill, s));
        }

        public List<CreatureSkill> GetSkills()
        {
            return this.skills.Select(kvp => kvp.Value).ToList();
        }
    }
}
