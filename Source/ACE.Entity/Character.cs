using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System.Linq;

namespace ACE.Entity
{
    public class Character : DbObject, ICreatureStats
    {
        protected Dictionary<Enum.Ability, CreatureAbility> abilities = new Dictionary<Enum.Ability, CreatureAbility>();

        private List<Friend> friends;

        public ReadOnlyCollection<Friend> Friends { get; set; }

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

        public CreatureAbility StrengthAbility { get; set; }

        public CreatureAbility EnduranceAbility { get; set; }

        public CreatureAbility CoordinationAbility { get; set; }

        public CreatureAbility QuicknessAbility { get; set; }

        public CreatureAbility FocusAbility { get; set; }

        public CreatureAbility SelfAbility { get; set; }

        public CreatureVital Health { get; set; }

        public CreatureVital Stamina { get; set; }

        public CreatureVital Mana { get; set; }

        public Appearance Appearance { get; set; } = new Appearance();

        public Position Location { get; set; }

        public Dictionary<Skill, CharacterSkill> Skills { get; private set; } = new Dictionary<Skill, CharacterSkill>();

        private Dictionary<CharacterOption, bool> characterOptions;
        public ReadOnlyDictionary<CharacterOption, bool> CharacterOptions { get; }

        private Dictionary<PositionType, Position> positions;

        public ReadOnlyDictionary<PositionType, Position> Positions { get; }

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

        public uint Age
        {
            get { return propertiesInt[PropertyInt.Age]; }
            set { propertiesInt[PropertyInt.Age] = value; }
        }

        public string DateOfBirth
        {
            get { return propertiesString[PropertyString.DateOfBirth]; }
            set { propertiesString[PropertyString.DateOfBirth] = value; }
        }

        public uint AvailableSkillCredits
        {
            get { return propertiesInt[PropertyInt.AvailableSkillCredits]; }
            set { propertiesInt[PropertyInt.AvailableSkillCredits] = value; }
        }

        public uint TotalSkillCredits
        {
            get { return propertiesInt[PropertyInt.TotalSkillCredits]; }
            set { propertiesInt[PropertyInt.TotalSkillCredits] = value; }
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

        public ReadOnlyDictionary<Enum.Ability, CreatureAbility> Abilities;

        public uint NumDeaths
        {
            get { return propertiesInt[PropertyInt.NumDeaths]; }
            set { propertiesInt[PropertyInt.NumDeaths] = value; }
        }

        public uint DeathLevel
        {
            get { return propertiesInt[PropertyInt.DeathLevel]; }
            set { propertiesInt[PropertyInt.DeathLevel] = value; }
        }

        public uint VitaeCpPool
        {
            get { return propertiesInt[PropertyInt.VitaeCpPool]; }
            set { propertiesInt[PropertyInt.VitaeCpPool] = value; }
        }

        private Character()
        {
            StrengthAbility = new CreatureAbility(Enum.Ability.Strength);
            EnduranceAbility = new CreatureAbility(Enum.Ability.Endurance);
            CoordinationAbility = new CreatureAbility(Enum.Ability.Coordination);
            QuicknessAbility = new CreatureAbility(Enum.Ability.Quickness);
            FocusAbility = new CreatureAbility(Enum.Ability.Focus);
            SelfAbility = new CreatureAbility(Enum.Ability.Self);

            // TODO: Real regen rates?
            Health = new CreatureVital(this, Enum.Ability.Health, .5);
            Stamina = new CreatureVital(this, Enum.Ability.Stamina, 1.0);
            Mana = new CreatureVital(this, Enum.Ability.Mana, .7);

            abilities.Add(Enum.Ability.Strength, StrengthAbility);
            abilities.Add(Enum.Ability.Endurance, EnduranceAbility);
            abilities.Add(Enum.Ability.Coordination, CoordinationAbility);
            abilities.Add(Enum.Ability.Quickness, QuicknessAbility);
            abilities.Add(Enum.Ability.Focus, FocusAbility);
            abilities.Add(Enum.Ability.Self, SelfAbility);

            abilities.Add(Enum.Ability.Health, Health);
            abilities.Add(Enum.Ability.Stamina, Stamina);
            abilities.Add(Enum.Ability.Mana, Mana);

            Abilities = new ReadOnlyDictionary<Enum.Ability, CreatureAbility>(abilities);

            // initialize properties collections to reasonable defaults
            InitializeProperties(typeof(Character));

            friends = new List<Friend>();
            Friends = new ReadOnlyCollection<Friend>(friends);

            InitializeCharacterOptions();
            CharacterOptions = new ReadOnlyDictionary<CharacterOption, bool>(characterOptions);

            // initialize the blank character positions
            InitializeCharacterPositions();
            Positions = new ReadOnlyDictionary<PositionType, Position>(positions);
        }

        public Character(uint id, uint accountId)
            : this()
        {
            Id = id;
            AccountId = accountId;
        }

        /// <summary>
        /// Sets the skill to trained status for a character
        /// </summary>
        /// <param name="skill"></param>
        public bool TrainSkill(Skill skill, uint creditsSpent)
        {
            if (Skills[skill].Status != SkillStatus.Trained && Skills[skill].Status != SkillStatus.Specialized)
                if (PropertiesInt[PropertyInt.AvailableSkillCredits] >= creditsSpent)
                {
                    Skills[skill] = new CharacterSkill(this, skill, SkillStatus.Trained, 0, 0);
                    AvailableSkillCredits = PropertiesInt[PropertyInt.AvailableSkillCredits] - creditsSpent;
                    return true;
                }
            return false;
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

        private void AddSkill(Skill skill, SkillStatus status, uint ranks, uint xpSpent)
        {
            Skills.Add(skill, new CharacterSkill(this, skill, status, ranks, xpSpent));
        }

        public void AddFriend(Friend friend)
        {
            friends.Add(friend);
        }

        public void RemoveFriend(uint lowId)
        {
            Friend friend = friends.SingleOrDefault(f => f.Id.Low == lowId);

            if (friend != null)
                friends.Remove(friend);
        }

        public void RemoveAllFriends()
        {
            friends.Clear();
        }

        public void SetCharacterOption(CharacterOption option, bool value)
        {
            if (characterOptions.ContainsKey(option))
                characterOptions[option] = value;
        }

        private void InitializeCharacterOptions()
        {
            characterOptions = new Dictionary<CharacterOption, bool>(System.Enum.GetValues(typeof(CharacterOption)).Length);

            // Default all values to false.  Only true values will be loaded from the database.
            foreach (CharacterOption characterOption in System.Enum.GetValues(typeof(CharacterOption)))
            {
                characterOptions.Add(characterOption, false);
            }
        }

        public void SetCharacterPosition(Position setPosition)
        {
            if (setPosition.PositionType == PositionType.Undef)
                return;

            setPosition.CharacterId = Id;
            
            if (positions.ContainsKey(setPosition.PositionType))
            {
                positions[setPosition.PositionType] = setPosition;
            } else
                positions.Add(setPosition.PositionType, setPosition);
        }

        private void InitializeCharacterPositions()
        {
            positions = new Dictionary<PositionType, Position>(System.Enum.GetValues(typeof(PositionType)).Length);
        }

        /// <summary>
        /// This should be moved back to the ACE project when time permits.
        /// </summary>
        public static Character CreateFromClientFragment(BinaryReader reader, uint accountId)
        {
            Character character = new Character();

            reader.Skip(4);   /* Unknown constant (1) */

            character.Heritage = reader.ReadUInt32();
            character.Gender = reader.ReadUInt32();
            character.Appearance = Appearance.FromNetowrk(reader);
            character.TemplateOption = reader.ReadUInt32();
            character.StrengthAbility.Base = reader.ReadUInt32();
            character.EnduranceAbility.Base = reader.ReadUInt32();
            character.CoordinationAbility.Base = reader.ReadUInt32();
            character.QuicknessAbility.Base = reader.ReadUInt32();
            character.FocusAbility.Base = reader.ReadUInt32();
            character.SelfAbility.Base = reader.ReadUInt32();
            character.Slot = reader.ReadUInt32();
            character.ClassId = reader.ReadUInt32();

            // characters start with max vitals
            character.Health.Current = character.Health.UnbuffedValue;
            character.Stamina.Current = character.Stamina.UnbuffedValue;
            character.Mana.Current = character.Mana.UnbuffedValue;

            character.TotalSkillCredits = 52;
            character.AvailableSkillCredits = 52;

            uint numOfSkills = reader.ReadUInt32();
            Skill skill;
            SkillStatus skillStatus;
            SkillCostAttribute skillCost;
            for (uint i = 0; i < numOfSkills; i++)
            {
                skill = (Skill)i;
                skillCost = skill.GetCost();
                skillStatus = (SkillStatus)reader.ReadUInt32();
                if (skillStatus == SkillStatus.Specialized)
                    if (skillCost.TrainsFree)
                        character.AvailableSkillCredits = character.AvailableSkillCredits - skillCost.SpecializationCost;
                    else
                        character.AvailableSkillCredits = character.AvailableSkillCredits - skillCost.SpecializationCost - skillCost.TrainingCost;
                if (skillStatus == SkillStatus.Trained)
                    if (!skillCost.TrainsFree)
                        character.AvailableSkillCredits = character.AvailableSkillCredits - skillCost.TrainingCost;
                character.AddSkill(skill, skillStatus, 0, 0);
            }

            character.Name = reader.ReadString16L();
            character.StartArea = reader.ReadUInt32();
            character.IsAdmin = Convert.ToBoolean(reader.ReadUInt32());
            character.IsEnvoy = Convert.ToBoolean(reader.ReadUInt32());
            // character.TotalSkillPoints = reader.ReadUInt32();  // garbage ? 

            return character;
        }
    }
}
