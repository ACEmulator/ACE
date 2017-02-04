using ACE.Network;
using System;
using System.Collections.Generic;

namespace ACE.Entity
{
    public class Character
    {
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
        }

        public Character(uint id, uint accountId)
            : this()
        {
            Id = id;
            AccountId = accountId;
        }

        public static Character CreateFromClientFragment(ClientPacketFragment fragment, uint accountId)
        {
            Character character = new Character();

            fragment.Payload.Skip(4);   /* Unknown constant (1) */

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
            
            uint numOfSkills = fragment.Payload.ReadUInt32();
            for (uint i = 0; i < numOfSkills; i++)
            {
                character.AddSkill((Skill)i, (SkillStatus)fragment.Payload.ReadUInt32());
            }

            character.Name = fragment.Payload.ReadString16L();
            character.StartArea = fragment.Payload.ReadUInt32();
            character.IsAdmin = Convert.ToBoolean(fragment.Payload.ReadUInt32());
            character.IsEnvoy = Convert.ToBoolean(fragment.Payload.ReadUInt32());
            character.TotalSkillPoints = fragment.Payload.ReadUInt32();

            return character;
        }

        public uint Id { get; set; }

        public uint AccountId { get; set; }

        public string Name { get; set; }

        public uint TemplateOption { get; set; }

        public uint StartArea { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsEnvoy { get; set; }

        public uint Slot { get; set; }

        public uint ClassId { get; set; }

        public uint TotalSkillPoints { get; set; }

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

        public Dictionary<Skill, CharacterSkill> Skills { get; private set; } = new Dictionary<Skill, CharacterSkill>();

        private void AddSkill(Skill skill, SkillStatus status)
        {
            Skills.Add(skill, new CharacterSkill(this, skill, status));
        }
    }
}
