using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ACE.Entity
{
    public class Character : DbObject
    {
        protected Dictionary<Enum.Ability, CharacterAbility> abilities = new Dictionary<Enum.Ability, CharacterAbility>();

        public uint AccountId { get; set; }

        public uint TemplateOption { get; set; }

        public uint StartArea { get; set; }

        public bool IsAdmin
        {
            get { return propertiesBool[PropertyBool.IsAdmin]; }
            set { propertiesBool[PropertyBool.IsAdmin] = value; }
        }

        public bool IsArch
        {
            get { return propertiesBool[PropertyBool.IsArch]; }
            set { propertiesBool[PropertyBool.IsArch] = value; }
        }

        public bool IsPsr
        {
            get { return propertiesBool[PropertyBool.IsPsr]; }
            set { propertiesBool[PropertyBool.IsPsr] = value; }
        }

        public uint Heritage
        {
            get { return propertiesInt[PropertyInt.HeritageGroup]; }
            set { propertiesInt[PropertyInt.HeritageGroup] = value; }
        }

        public uint Gender
        {
            get { return propertiesInt[PropertyInt.Gender]; }
            set { propertiesInt[PropertyInt.Gender] = value; }
        }

        /// <summary>
        /// pulls from the IsSentinal bool property
        /// </summary>
        public bool IsEnvoy
        {
            get { return propertiesBool[PropertyBool.IsSentinel]; }
            set { propertiesBool[PropertyBool.IsSentinel] = value; }
        }

        public uint Slot { get; set; }

        public uint ClassId { get; set; }

        public uint TotalSkillPoints { get; set; }

        public uint TotalLogins { get; set; } = 1u;  // total amount of times the player has logged into this character

        public CharacterAbility Strength { get; set; }

        public CharacterAbility Endurance { get; set; }

        public CharacterAbility Coordination { get; set; }

        public CharacterAbility Quickness { get; set; }

        public CharacterAbility Focus { get; set; }

        public CharacterAbility Self { get; set; }

        public CharacterAbility Health { get; set; }

        public CharacterAbility Stamina { get; set; }

        public CharacterAbility Mana { get; set; }

        public Appearance Appearance { get; set; } = new Appearance();

        public Position Position { get; set; }

        public Dictionary<Skill, CharacterSkill> Skills { get; private set; } = new Dictionary<Skill, CharacterSkill>();

        public ulong AvailableExperience
        {
            get { return propertiesInt64[PropertyInt64.AvailableExperience]; }
        }

        public ulong TotalExperience
        {
            get { return propertiesInt64[PropertyInt64.TotalExperience]; }
        }

        public uint Level
        {
            get { return propertiesInt[PropertyInt.Level]; }
            set { propertiesInt[PropertyInt.Level] = value; }
        }

        public ReadOnlyDictionary<Enum.Ability, CharacterAbility> Abilities;

        private Character()
        {
            Strength = new CharacterAbility(this, Enum.Ability.Strength);
            Endurance = new CharacterAbility(this, Enum.Ability.Endurance);
            Coordination = new CharacterAbility(this, Enum.Ability.Coordination);
            Quickness = new CharacterAbility(this, Enum.Ability.Quickness);
            Focus = new CharacterAbility(this, Enum.Ability.Focus);
            Self = new CharacterAbility(this, Enum.Ability.Self);

            Health = new CharacterAbility(this, Enum.Ability.Health);
            Stamina = new CharacterAbility(this, Enum.Ability.Stamina);
            Mana = new CharacterAbility(this, Enum.Ability.Mana);

            abilities.Add(Enum.Ability.Strength, Strength);
            abilities.Add(Enum.Ability.Endurance, Endurance);
            abilities.Add(Enum.Ability.Coordination, Coordination);
            abilities.Add(Enum.Ability.Quickness, Quickness);
            abilities.Add(Enum.Ability.Focus, Focus);
            abilities.Add(Enum.Ability.Self, Self);

            abilities.Add(Enum.Ability.Health, Health);
            abilities.Add(Enum.Ability.Stamina, Stamina);
            abilities.Add(Enum.Ability.Mana, Mana);

            Abilities = new ReadOnlyDictionary<Enum.Ability, CharacterAbility>(abilities);

            // initialize properties collections to reasonable defaults
            InitializeProperties(typeof(Character));
        }

        public Character(uint id, uint accountId)
            : this()
        {
            Id = id;
            AccountId = accountId;
        }
        
        /// <summary>
        /// gives avaiable xp and total xp of the amount specified
        /// </summary>
        public void GrantXp(ulong amount)
        {
            propertiesInt64[PropertyInt64.AvailableExperience] += amount;
            propertiesInt64[PropertyInt64.TotalExperience] += amount;
        }

        /// <summary>
        /// spends the amount of xp specified, deducting it from avaiable experience
        /// </summary>
        public void SpendXp(ulong amount)
        {
            if (propertiesInt64[PropertyInt64.AvailableExperience] >= amount)
            {
                propertiesInt64[PropertyInt64.AvailableExperience] -= amount;
            }
        }

        /// <summary>
        /// gives available xp of the amount specified without increasing total xp
        /// </summary>
        public void RefundXp(ulong amount)
        {
            propertiesInt64[PropertyInt64.AvailableExperience] += amount;
        }

        private void AddSkill(Skill skill, SkillStatus status, uint ranks)
        {
            Skills.Add(skill, new CharacterSkill(this, skill, status, ranks));
        }

        public static Character CreateFromClientFragment(ClientPacketFragment fragment, uint accountId)
        {
            Character character = new Character();

            fragment.Payload.Skip(4);   /* Unknown constant (1) */

            character.Heritage = fragment.Payload.ReadUInt32();
            character.Gender = fragment.Payload.ReadUInt32();
            character.Appearance = Appearance.FromFragment(fragment);
            character.TemplateOption = fragment.Payload.ReadUInt32();
            character.Strength.Base = fragment.Payload.ReadUInt32();
            character.Endurance.Base = fragment.Payload.ReadUInt32();
            character.Coordination.Base = fragment.Payload.ReadUInt32();
            character.Quickness.Base = fragment.Payload.ReadUInt32();
            character.Focus.Base = fragment.Payload.ReadUInt32();
            character.Self.Base = fragment.Payload.ReadUInt32();
            character.Slot = fragment.Payload.ReadUInt32();
            character.ClassId = fragment.Payload.ReadUInt32();

            // characters start with max vitals
            character.Health.Current = character.Health.UnbuffedValue;
            character.Stamina.Current = character.Stamina.UnbuffedValue;
            character.Mana.Current = character.Mana.UnbuffedValue;

            uint numOfSkills = fragment.Payload.ReadUInt32();
            for (uint i = 0; i < numOfSkills; i++)
            {
                character.AddSkill((Skill)i, (SkillStatus)fragment.Payload.ReadUInt32(), 0);
            }

            character.Name = fragment.Payload.ReadString16L();
            character.StartArea = fragment.Payload.ReadUInt32();
            character.IsAdmin = Convert.ToBoolean(fragment.Payload.ReadUInt32());
            character.IsEnvoy = Convert.ToBoolean(fragment.Payload.ReadUInt32());
            character.TotalSkillPoints = fragment.Payload.ReadUInt32();

            return character;
        }
    }
}
