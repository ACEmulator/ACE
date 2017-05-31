using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ACE.Entity
{
    [DbTable("vw_ace_creature_object")]
    [DbGetList("vw_ace_creature_object", 42, "weenieClassId")]
    public class AceCreatureObject : AceObject, ICreatureStats
    {
        public AceCreatureObject()
        {
            StrengthAbility = new CreatureAbility(this, Enum.Ability.Strength);
            EnduranceAbility = new CreatureAbility(this, Enum.Ability.Endurance);
            CoordinationAbility = new CreatureAbility(this, Enum.Ability.Coordination);
            QuicknessAbility = new CreatureAbility(this, Enum.Ability.Quickness);
            FocusAbility = new CreatureAbility(this, Enum.Ability.Focus);
            SelfAbility = new CreatureAbility(this, Enum.Ability.Self);

            Health = new CreatureAbility(this, Enum.Ability.Health);
            Stamina = new CreatureAbility(this, Enum.Ability.Stamina);
            Mana = new CreatureAbility(this, Enum.Ability.Mana);
        }

        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public override uint WeenieClassId { get; set; }

        [DbField("baseAceObjectId", (int)MySqlDbType.UInt32)]
        public override uint AceObjectId { get; set; }

        protected Dictionary<Ability, CreatureAbility> abilities = new Dictionary<Ability, CreatureAbility>();

        protected Dictionary<Skill, CreatureSkill> skills = new Dictionary<Skill, CreatureSkill>();

        public CreatureAbility StrengthAbility { get; set; }

        public CreatureAbility EnduranceAbility { get; set; }

        public CreatureAbility CoordinationAbility { get; set; }

        public CreatureAbility QuicknessAbility { get; set; }

        public CreatureAbility FocusAbility { get; set; }

        public CreatureAbility SelfAbility { get; set; }

        public CreatureAbility Health { get; set; }

        public CreatureAbility Stamina { get; set; }

        public CreatureAbility Mana { get; set; }

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
    }
}
