using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ACE.Entity
{
    //[DbTable("vw_ace_character")]
    public class AceCharacter : AceObject, ICreatureStats
    {
        private readonly List<Friend> friends;

        public AceCharacter(uint id)
            : base(id)
        {
            friends = new List<Friend>();
            Friends = new ReadOnlyCollection<Friend>(friends);

            // Required default properties for character login
            // FIXME(ddevec): Should we have constants for (some of) these things?

            AceObjectDescriptionFlags = 0;
            SetProperty(PropertyBool.Stuck, true);
            SetProperty(PropertyBool.Attackable, true);

            WeenieHeaderFlags = 0;
            SetProperty(PropertyInt.ItemsCapacity, 102);
            SetProperty(PropertyInt.ContainersCapacity, 7);           
            SetProperty(PropertyInt.ItemUseable, (int)Usable.No);
            SetProperty(PropertyInt.ShowableOnRadar, (byte)RadarBehavior.ShowAlways);

            PhysicsState = 0;

            WeenieClassId = 1;
            SetProperty(PropertyInt.WeenieType, (int)Enum.WeenieType.Creature); // This might need to change
            SetProperty(PropertyDataId.Icon, 100667446);
            SetProperty(PropertyInt.ItemType, (int)Enum.ItemType.Creature);
            // SetIntProperty(PropertyInt.RadarBlipColor, (byte)RadarColor.White);
            SetProperty(PropertyBool.IsDeleted, false);
            SetProperty(PropertyInt.TotalLogins, 0);
            SetInt64Property(PropertyInt64.DeleteTime, 0);
            SetProperty(PropertyInt.Level, 1);
            SetInt64Property(PropertyInt64.AvailableExperience, 0);
            SetInt64Property(PropertyInt64.TotalExperience, 0);

            SetProperty(PropertyInt.EncumbranceVal, 0);

            SetProperty(PropertyBool.FirstEnterWorldDone, false);

            SetDoubleTimestamp(PropertyFloat.CreationTimestamp);
            SetProperty(PropertyInt.CreationTimestamp, (int)GetProperty(PropertyFloat.CreationTimestamp));
            SetProperty(PropertyString.DateOfBirth, $"{System.DateTime.UtcNow:dd MMMM yyyy}");

            SetProperty(PropertyInt.CreatureType, (int)Enum.CreatureType.Human);
            SetProperty(PropertyInt.ChannelsAllowed, (int)Channel.AllChans);
            SetProperty(PropertyInt.ChannelsActive, (int)Channel.AllBroadcast);

            SetProperty(PropertyInt.NumDeaths, 0);

            SetProperty(PropertyInt.ChessRank, 1400);
            SetProperty(PropertyInt.ChessTotalGames, 0);
            SetProperty(PropertyInt.ChessGamesLost, 0);
            SetProperty(PropertyInt.ChessGamesWon, 0);
            SetProperty(PropertyInt.FakeFishingSkill, 0);            

            SetProperty(PropertyFloat.GlobalXpMod, 0);
            SetProperty(PropertyInt.HealingBoostRating, 0);
            SetProperty(PropertyInt.WeaknessRating, 0);
            SetProperty(PropertyInt.NetherOverTime, 0);
            SetProperty(PropertyInt.NetherResistRating, 0);
            SetProperty(PropertyInt.DotResistRating, 0);
            SetProperty(PropertyInt.LifeResistRating, 0);
            SetProperty(PropertyInt.WeaponAuraDamage, 0);
            SetProperty(PropertyInt.WeaponAuraSpeed, 0);
            SetProperty(PropertyInt.PKDamageRating, 0);
            SetProperty(PropertyInt.PKDamageResistRating, 0);
            SetProperty(PropertyInt.Overpower, 0);
            SetProperty(PropertyInt.OverpowerResist, 0);
            SetProperty(PropertyInt.GearOverpower, 0);
            SetProperty(PropertyInt.GearOverpowerResist, 0);
            SetProperty(PropertyFloat.WeaponAuraOffense, 0);
            SetProperty(PropertyFloat.WeaponAuraDefense, 0);
            SetProperty(PropertyFloat.WeaponAuraElemental, 0);
            SetProperty(PropertyFloat.WeaponAuraManaConv, 0);
            SetInt64Property(PropertyInt64.AvailableLuminance, 0);
            SetInt64Property(PropertyInt64.MaximumLuminance, 0);
            SetProperty(PropertyInt.LumAugDamageRating, 0);
            SetProperty(PropertyInt.LumAugDamageReductionRating, 0);
            SetProperty(PropertyInt.LumAugCritDamageRating, 0);
            SetProperty(PropertyInt.LumAugCritReductionRating, 0);
            SetProperty(PropertyInt.LumAugSurgeChanceRating, 0);
            SetProperty(PropertyInt.LumAugItemManaUsage, 0);
            SetProperty(PropertyInt.LumAugItemManaGain, 0);
            SetProperty(PropertyInt.LumAugHealingRating, 0);
            SetProperty(PropertyInt.LumAugSkilledCraft, 0);
            SetProperty(PropertyInt.LumAugSkilledSpec, 0);
            SetProperty(PropertyInt.LumAugAllSkills, 0);
            SetProperty(PropertyInt.Enlightenment, 0);
            SetProperty(PropertyInt.GearDamage, 0);
            SetProperty(PropertyInt.GearDamageResist, 0);
            SetProperty(PropertyInt.GearCrit, 0);
            SetProperty(PropertyInt.GearCritResist, 0);
            SetProperty(PropertyInt.GearCritDamage, 0);
            SetProperty(PropertyInt.GearCritDamageResist, 0);
            SetProperty(PropertyInt.GearHealingBoost, 0);
            SetProperty(PropertyInt.GearNetherResist, 0);
            SetProperty(PropertyInt.GearLifeResist, 0);
            SetProperty(PropertyInt.GearMaxHealth, 0);
            SetProperty(PropertyInt.GearPKDamageRating, 0);
            SetProperty(PropertyInt.GearPKDamageResistRating, 0);

            SetProperty(PropertyBool.Account15Days, true);

            SetProperty(PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.NPK);
        }

        public ReadOnlyCollection<Friend> Friends { get; set; }

        public uint CharacterSlot { get; set; }

        public int CharacterOptions1Mapping
        {
            get { return GetProperty(PropertyInt.CharacterOptions1) ?? 0; }
            set { SetProperty(PropertyInt.CharacterOptions1, value); }
        }

        public int CharacterOptions2Mapping
        {
            get { return GetProperty(PropertyInt.CharacterOptions2) ?? 0; }
            set { SetProperty(PropertyInt.CharacterOptions2, value); }
        }

        public new int TotalLogins
        {
            get { return GetProperty(PropertyInt.TotalLogins) ?? 0; }
            set { SetProperty(PropertyInt.TotalLogins, value); }
        }
        
        public bool Deleted
        {
            get { return GetProperty(PropertyBool.IsDeleted) ?? false; }
            set { SetProperty(PropertyBool.IsDeleted, value); }
        }

        public new ulong DeleteTime
        {
            get { return GetProperty(PropertyInt64.DeleteTime) ?? 0; }
            set { SetInt64Property(PropertyInt64.DeleteTime, value); }
        }

        public bool GetCharacterOptions1(CharacterOptions1 option)
        {
            return (CharacterOptions1Mapping & (uint)option) != 0;
        }

        public void SetCharacterOptions1(CharacterOptions1 option, bool value)
        {
            if (value)
            {
                CharacterOptions1Mapping |= (int)option;
            }
            else
            {
                CharacterOptions1Mapping &= ~(int)option;
            }

            dirtyOptions = true;
        }

        public bool GetCharacterOptions2(CharacterOptions2 option)
        {
            return (CharacterOptions2Mapping & (uint)option) != 0;
        }

        public void SetCharacterOptions2(CharacterOptions2 option, bool value)
        {
            if (value)
            {
                CharacterOptions2Mapping |= (int)option;
            }
            else
            {
                CharacterOptions2Mapping &= ~(int)option;
            }

            dirtyOptions = true;
        }

        public new ulong AvailableExperience
        {
            get { return GetProperty(PropertyInt64.AvailableExperience) ?? 0; }
            set { SetInt64Property(PropertyInt64.AvailableExperience, value); }
        }

        public new ulong TotalExperience
        {
            get { return GetProperty(PropertyInt64.TotalExperience) ?? 0; }
            set { SetInt64Property(PropertyInt64.TotalExperience, value); }
        }

        public new int Age
        {
            get { return GetProperty(PropertyInt.Age) ?? 0; }
            set { SetProperty(PropertyInt.Age, value); }
        }

        public new bool IsDeleted
        {
            get { return GetProperty(PropertyBool.IsDeleted) ?? false; }
            set { SetProperty(PropertyBool.IsDeleted, value); }
        }

        public new int AvailableSkillCredits
        {
            get { return GetProperty(PropertyInt.AvailableSkillCredits) ?? 0; }
            set { SetProperty(PropertyInt.AvailableSkillCredits, value); }
        }

        public new int TotalSkillCredits
        {
            get { return GetProperty(PropertyInt.TotalSkillCredits) ?? 0; }
            set { SetProperty(PropertyInt.TotalSkillCredits, value); }
        }

        public new int NumDeaths
        {
            get { return GetProperty(PropertyInt.NumDeaths) ?? 0; }
            set { SetProperty(PropertyInt.NumDeaths, value); }
        }

        public new int DeathLevel
        {
            get { return GetProperty(PropertyInt.DeathLevel) ?? 0; }
            set { SetProperty(PropertyInt.DeathLevel, value); }
        }

        public new int VitaeCpPool
        {
            get { return GetProperty(PropertyInt.VitaeCpPool) ?? 0; }
            set { SetProperty(PropertyInt.VitaeCpPool, value); }
        }

        public new bool IsAdmin
        {
            get { return GetProperty(PropertyBool.IsAdmin) ?? false; }
            set { SetProperty(PropertyBool.IsAdmin, value); }
        }

        public new bool IsEnvoy
        {
            get { return GetProperty(PropertyBool.IsSentinel) ?? false; }
            set { SetProperty(PropertyBool.IsSentinel, value); }
        }

        public new bool IsArch
        {
            get { return GetProperty(PropertyBool.IsArch) ?? false; }
            set { SetProperty(PropertyBool.IsArch, value); }
        }

        public new bool IsPsr
        {
            get { return GetProperty(PropertyBool.IsPsr) ?? false; }
            set { SetProperty(PropertyBool.IsPsr, value); }
        }

        public uint EyesTexture
        {
            get { return GetProperty(PropertyDataId.EyesTexture) ?? 0; }
            set { SetProperty(PropertyDataId.EyesTexture, value); }
        }

        public uint DefaultEyesTexture
        {
            get { return GetProperty(PropertyDataId.DefaultEyesTexture) ?? 0; }
            set { SetProperty(PropertyDataId.DefaultEyesTexture, value); }
        }

        public uint NoseTexture
        {
            get { return GetProperty(PropertyDataId.NoseTexture) ?? 0; }
            set { SetProperty(PropertyDataId.NoseTexture, value); }
        }

        public uint DefaultNoseTexture
        {
            get { return GetProperty(PropertyDataId.DefaultNoseTexture) ?? 0; }
            set { SetProperty(PropertyDataId.DefaultNoseTexture, value); }
        }

        public uint MouthTexture
        {
            get { return GetProperty(PropertyDataId.MouthTexture) ?? 0; }
            set { SetProperty(PropertyDataId.MouthTexture, value); }
        }

        public uint DefaultMouthTexture
        {
            get { return GetProperty(PropertyDataId.DefaultMouthTexture) ?? 0; }
            set { SetProperty(PropertyDataId.DefaultMouthTexture, value); }
        }

        public uint HairTexture
        {
            get { return GetProperty(PropertyDataId.HairTexture) ?? 0; }
            set { SetProperty(PropertyDataId.HairTexture, value); }
        }

        public uint DefaultHairTexture
        {
            get { return GetProperty(PropertyDataId.DefaultHairTexture) ?? 0; }
            set { SetProperty(PropertyDataId.DefaultHairTexture, value); }
        }

        public uint HeadObject
        {
            get { return GetProperty(PropertyDataId.HeadObject) ?? 0; }
            set { SetProperty(PropertyDataId.HeadObject, value); }
        }

        public uint SkinPalette
        {
            get { return GetProperty(PropertyDataId.SkinPalette) ?? 0; }
            set { SetProperty(PropertyDataId.SkinPalette, value); }
        }

        public uint HairPalette
        {
            get { return GetProperty(PropertyDataId.HairPalette) ?? 0; }
            set { SetProperty(PropertyDataId.HairPalette, value); }
        }

        public uint EyesPalette
        {
            get { return GetProperty(PropertyDataId.EyesPalette) ?? 0; }
            set { SetProperty(PropertyDataId.EyesPalette, value); }
        }

        public uint? SetupTableId
        {
            get { return GetProperty(PropertyDataId.Setup); }
            set { SetProperty(PropertyDataId.Setup, value); }
        }

        public uint? MotionTableId
        {
            get { return GetProperty(PropertyDataId.MotionTable); }
            set { SetProperty(PropertyDataId.MotionTable, value); }
        }

        public ushort? PhysicsScript
        {
            get { return (ushort?)GetProperty(PropertyDataId.PhysicsScript); }
            set { SetProperty(PropertyDataId.PhysicsScript, value); }
        }

        public uint? PhysicsTableId
        {
            get { return GetProperty(PropertyDataId.PhysicsEffectTable); }
            set { SetProperty(PropertyDataId.PhysicsEffectTable, value); }
        }

        public uint? SoundTableId
        {
            get { return GetProperty(PropertyDataId.SoundTable); }
            set { SetProperty(PropertyDataId.SoundTable, value); }
        }

        public uint? CombatTableId
        {
            get { return GetProperty(PropertyDataId.CombatTable); }
            set { SetProperty(PropertyDataId.CombatTable, value); }
        }

        public new int Level
        {
            get { return GetProperty(PropertyInt.Level) ?? 1; }
            set { SetProperty(PropertyInt.Level, value); }
        }

        public uint? PaletteId
        {
            get { return GetProperty(PropertyDataId.PaletteBase); }
            set { SetProperty(PropertyDataId.PaletteBase, value); }
        }

        private bool dirtyOptions = true;

        private Dictionary<CharacterOption, bool> allOptions = new Dictionary<CharacterOption, bool>();

        public ReadOnlyDictionary<CharacterOption, bool> CharacterOptions
        {
            get
            {
                // normally, i would care about the thread safety of this.  but given that there's only one data
                // source for updating options (that player's client), it's really not a risk.

                if (dirtyOptions)
                {
                    var allOptions = new Dictionary<CharacterOption, bool>(System.Enum.GetValues(typeof(CharacterOption)).Length);

                    // Default all values to false.  Only true values will be loaded from the database.
                    foreach (CharacterOption characterOption in System.Enum.GetValues(typeof(CharacterOption)))
                    {
                        if (characterOption.GetCharacterOptions1Attribute() != null)
                        {
                            var option1 = (CharacterOptions1)System.Enum.Parse(typeof(CharacterOptions1), characterOption.ToString());
                            allOptions.Add(characterOption, GetCharacterOptions1(option1));
                        }
                        else
                        {
                            var option2 = (CharacterOptions2)System.Enum.Parse(typeof(CharacterOptions2), characterOption.ToString());
                            allOptions.Add(characterOption, GetCharacterOptions2(option2));
                        }
                    }

                    dirtyOptions = false;
                }

                return new ReadOnlyDictionary<CharacterOption, bool>(allOptions);
            }
        }

        public void SetCharacterOption(CharacterOption option, bool value)
        {
            if (option.GetCharacterOptions1Attribute() != null)
                SetCharacterOptions1((CharacterOptions1)System.Enum.Parse(typeof(CharacterOptions1), option.ToString()), value);
            else
                SetCharacterOptions2((CharacterOptions2)System.Enum.Parse(typeof(CharacterOptions2), option.ToString()), value);
        }

        public bool GetCharacterOption(CharacterOption option)
        {
            if (option.GetCharacterOptions1Attribute() != null)
                return GetCharacterOptions1((CharacterOptions1)System.Enum.Parse(typeof(CharacterOptions1), option.ToString()));
            return GetCharacterOptions2((CharacterOptions2)System.Enum.Parse(typeof(CharacterOptions2), option.ToString()));
        }

        /// <summary>
        /// Sets the skill to trained status for a character
        /// </summary>
        /// <param name="skill"></param>
        public bool TrainSkill(Skill skill, int creditsSpent)
        {
            CreatureSkillOld cs = GetSkillProperty(skill);
            if (cs != null && cs.Status != SkillStatus.Trained && cs.Status != SkillStatus.Specialized)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    var newSkill = new CreatureSkillOld(this, skill, SkillStatus.Trained, 0, 0);
                    SetProperty(skill, newSkill);
                    AvailableSkillCredits -= creditsSpent;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the skill to specialized status for a character
        /// </summary>
        /// <param name="skill"></param>
        public bool SpecializeSkill(Skill skill, int creditsSpent)
        {
            CreatureSkillOld cs = GetSkillProperty(skill);
            if (cs != null && cs.Status == SkillStatus.Trained)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    RefundXp(cs.ExperienceSpent);
                    var newSkill = new CreatureSkillOld(this, skill, SkillStatus.Specialized, 0, 0);
                    SetProperty(skill, newSkill);
                    AvailableSkillCredits -= creditsSpent;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the skill to untrained status for a character
        /// </summary>
        /// <param name="skill"></param>
        public bool UntrainSkill(Skill skill, int creditsSpent)
        {
            CreatureSkillOld cs = GetSkillProperty(skill);
            if (cs != null && cs.Status != SkillStatus.Trained && cs.Status != SkillStatus.Specialized) 
            {
                var newSkill = new CreatureSkillOld(this, skill, SkillStatus.Untrained, 0, 0);
                SetProperty(skill, newSkill);
                return true;
            }

            if (cs != null && cs.Status == SkillStatus.Trained)
            {
                RefundXp(cs.ExperienceSpent);
                var newSkill = new CreatureSkillOld(this, skill, SkillStatus.Untrained, 0, 0);
                SetProperty(skill, newSkill);
                AvailableSkillCredits += creditsSpent;
                return true;
            }

            return false;
        }

        /// <summary>
        /// gives avaiable xp and total xp of the amount specified
        /// </summary>
        public void GrantXp(ulong amount)
        {
            AvailableExperience  += amount;
            TotalExperience += amount;
        }

        /// <summary>
        /// spends the amount of xp specified, deducting it from avaiable experience
        /// </summary>
        public bool SpendXp(ulong amount)
        {
            if (AvailableExperience >= amount)
            {
                AvailableExperience -= amount;
                return true;
            }

            return false;
        }

        /// <summary>
        /// gives available xp of the amount specified without increasing total xp
        /// </summary>
        public void RefundXp(ulong amount)
        {
            AvailableExperience += amount;
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
    }
}
