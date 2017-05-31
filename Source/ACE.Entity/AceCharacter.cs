using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_character")]
    [DbGetList("vw_ace_character", 42, "guid")]
    public class AceCharacter : AceCreatureObject
    {
        [DbField("characterOptions1", (int)MySqlDbType.UInt16)]
        public uint CharacterOptions1Mapping { get; set; }

        [DbField("characterOptions2", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public uint CharacterOptions2Mapping { get; set; }
        
        public bool? GetCharacterOptions1(CharacterOptions1 option)
        {
            return (CharacterOptions1Mapping & (uint)option) != 0;
        }

        public void SetCharacterOptions1(CharacterOptions1 option, bool value)
        {
            if (value)
            {
                CharacterOptions1Mapping |= (uint)option;
            }
            else
            {
                CharacterOptions1Mapping &= ~(uint)option;
            }
        }

        public bool? GetCharacterOptions2(CharacterOptions2 option)
        {
            return (CharacterOptions2Mapping & (uint)option) != 0;
        }

        public void SetCharacterOptions2(CharacterOptions2 option, bool value)
        {
            if (value)
            {
                CharacterOptions2Mapping |= (uint)option;
            }
            else
            {
                CharacterOptions2Mapping &= ~(uint)option;
            }
        }

        public ulong AvailableExperience
        {
            get { return GetInt64Property(PropertyInt64.AvailableExperience) ?? 0; }
            set { SetInt64Property(PropertyInt64.AvailableExperience, value); }
        }

        public ulong TotalExperience
        {
            get { return GetInt64Property(PropertyInt64.TotalExperience) ?? 0; }
            set { SetInt64Property(PropertyInt64.TotalExperience, value); }
        }

        public uint Age
        {
            get { return GetIntProperty(PropertyInt.Age) ?? 0; }
            set { SetIntProperty(PropertyInt.Age, value); }
        }

        public string DateOfBirth
        {
            get { return GetStringProperty(PropertyString.DateOfBirth); }
            set { SetStringProperty(PropertyString.DateOfBirth, value); }
        }

        public uint AvailableSkillCredits
        {
            get { return GetIntProperty(PropertyInt.AvailableSkillCredits) ?? 0; }
            set { SetIntProperty(PropertyInt.AvailableSkillCredits, value); }
        }

        public uint TotalSkillCredits
        {
            get { return GetIntProperty(PropertyInt.TotalSkillCredits) ?? 0; }
            set { SetIntProperty(PropertyInt.TotalSkillCredits, value); }
        }

        public uint NumDeaths
        {
            get { return GetIntProperty(PropertyInt.NumDeaths) ?? 0; }
            set { SetIntProperty(PropertyInt.NumDeaths, value); }
        }

        public uint DeathLevel
        {
            get { return GetIntProperty(PropertyInt.DeathLevel) ?? 0; }
            set { SetIntProperty(PropertyInt.DeathLevel, value); }
        }

        public uint VitaeCpPool
        {
            get { return GetIntProperty(PropertyInt.VitaeCpPool) ?? 0; }
            set { SetIntProperty(PropertyInt.VitaeCpPool, value); }
        }

    }
}
