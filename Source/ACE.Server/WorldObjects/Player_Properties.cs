
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        // ========================================
        // ========= Admin Tier Properties ========
        // ========================================

        public bool IsAdmin
        {
            get => GetProperty(PropertyBool.IsAdmin) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsAdmin); else SetProperty(PropertyBool.IsAdmin, value); }
        }

        public bool IsSentinel
        {
            get => GetProperty(PropertyBool.IsSentinel) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsSentinel); else SetProperty(PropertyBool.IsSentinel, value); }
        }

        public bool IsEnvoy
        {
            get => GetProperty(PropertyBool.IsSentinel) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsSentinel); else SetProperty(PropertyBool.IsSentinel, value); }
        }

        public bool IsArch
        {
            get => GetProperty(PropertyBool.IsArch) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsArch); else SetProperty(PropertyBool.IsArch, value); }
        }

        public bool IsPsr
        {
            get => GetProperty(PropertyBool.IsPsr) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsPsr); else SetProperty(PropertyBool.IsPsr, value); }
        }

        public bool IsAdvocate
        {
            get => GetProperty(PropertyBool.IsAdvocate) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsAdvocate); else SetProperty(PropertyBool.IsAdvocate, value); }
        }


        // ========================================
        // ========== Account Properties ==========
        // ========================================

        public bool Account15Days
        {
            get => GetProperty(PropertyBool.Account15Days) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Account15Days); else SetProperty(PropertyBool.Account15Days, value); }
        }


        // ========================================
        // ========= Advocate Properties ==========
        // ========================================

        public bool? AdvocateQuest
        {
            get => GetProperty(PropertyBool.AdvocateQuest);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.AdvocateQuest); else SetProperty(PropertyBool.AdvocateQuest, value.Value); }
        }

        public bool? AdvocateState
        {
            get => GetProperty(PropertyBool.AdvocateState);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.AdvocateState); else SetProperty(PropertyBool.AdvocateState, value.Value); }
        }

        public int? AdvocateLevel
        {
            get => GetProperty(PropertyInt.AdvocateLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AdvocateLevel); else SetProperty(PropertyInt.AdvocateLevel, value.Value); }
        }


        // ========================================
        // ========= Channel Properties ===========
        // ========================================

        public Channel? ChannelsActive
        {
            get => (Channel?)GetProperty(PropertyInt.ChannelsActive);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChannelsActive); else SetProperty(PropertyInt.ChannelsActive, (int)value.Value); }
        }

        public Channel? ChannelsAllowed
        {
            get => (Channel?)GetProperty(PropertyInt.ChannelsAllowed);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChannelsAllowed); else SetProperty(PropertyInt.ChannelsAllowed, (int)value.Value); }
        }


        // ========================================
        // ========== Client Properties ===========
        // ========================================

        public int? CharacterOptions1Mapping
        {
            get => GetProperty(PropertyInt.CharacterOptions1);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CharacterOptions1); else SetProperty(PropertyInt.CharacterOptions1, value.Value); }
        }

        public int? CharacterOptions2Mapping
        {
            get => GetProperty(PropertyInt.CharacterOptions2);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CharacterOptions2); else SetProperty(PropertyInt.CharacterOptions2, value.Value); }
        }


        // ========================================
        // ========== Player Properties ===========
        // ========================================

        public int? TotalLogins
        {
            get => GetProperty(PropertyInt.TotalLogins);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.TotalLogins); else SetProperty(PropertyInt.TotalLogins, value.Value); }
        }

        //public long? DeleteTime
        //{
        //    get => GetProperty(PropertyInt64.DeleteTime);
        //    set { if (!value.HasValue) RemoveProperty(PropertyInt64.DeleteTime); else SetProperty(PropertyInt64.DeleteTime, value.Value); }
        //}

        public int? Age
        {
            get => GetProperty(PropertyInt.Age);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Age); else SetProperty(PropertyInt.Age, value.Value); }
        }

        public long? AvailableExperience
        {
            get => GetProperty(PropertyInt64.AvailableExperience);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.AvailableExperience); else SetProperty(PropertyInt64.AvailableExperience, value.Value); }
        }

        public long? TotalExperience
        {
            get => GetProperty(PropertyInt64.TotalExperience);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.TotalExperience); else SetProperty(PropertyInt64.TotalExperience, value.Value); }
        }

        public long? AvailableLuminance
        {
            get => GetProperty(PropertyInt64.AvailableLuminance);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.AvailableLuminance); else SetProperty(PropertyInt64.AvailableLuminance, value.Value); }
        }

        public long? MaximumLuminance
        {
            get => GetProperty(PropertyInt64.MaximumLuminance);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.MaximumLuminance); else SetProperty(PropertyInt64.MaximumLuminance, value.Value); }
        }

        public int? AvailableSkillCredits
        {
            get => GetProperty(PropertyInt.AvailableSkillCredits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AvailableSkillCredits); else SetProperty(PropertyInt.AvailableSkillCredits, value.Value); }
        }

        public int? TotalSkillCredits
        {
            get => GetProperty(PropertyInt.TotalSkillCredits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.TotalSkillCredits); else SetProperty(PropertyInt.TotalSkillCredits, value.Value); }
        }

        public int? NumDeaths
        {
            get => GetProperty(PropertyInt.NumDeaths);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.NumDeaths); else SetProperty(PropertyInt.NumDeaths, value.Value); }
        }

        public int? DeathLevel
        {
            get => GetProperty(PropertyInt.DeathLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.DeathLevel); else SetProperty(PropertyInt.DeathLevel, value.Value); }
        }

        public int? VitaeCpPool
        {
            get => GetProperty(PropertyInt.VitaeCpPool);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.VitaeCpPool); else SetProperty(PropertyInt.VitaeCpPool, value.Value); }
        }

        public bool HasVitae
        {
            get
            {
                return EnchantmentManager.HasVitae;
            }
        }

        public float Vitae
        {
            get
            {
                if (!HasVitae)
                    return 1.0f;

                var vitae = EnchantmentManager.GetVitae();
                return vitae.StatModValue;
            }
        }

        public int? AllegianceCPPool
        {
            get => GetProperty(PropertyInt.AllegianceCpPool);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AllegianceCpPool); else SetProperty(PropertyInt.AllegianceCpPool, value.Value); }
        }

        // ========================================
        // ===== Player Properties - Titles========
        // ========================================

        public int? CharacterTitleId
        {
            get => GetProperty(PropertyInt.CharacterTitleId);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CharacterTitleId); else SetProperty(PropertyInt.CharacterTitleId, value.Value); }
        }

        public int? NumCharacterTitles
        {
            get => GetProperty(PropertyInt.NumCharacterTitles);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.NumCharacterTitles); else SetProperty(PropertyInt.NumCharacterTitles, value.Value); }
        }
    }
}
