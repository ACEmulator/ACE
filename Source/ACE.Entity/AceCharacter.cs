﻿using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ACE.Entity
{
    [DbTable("vw_ace_character")]
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
            SetBoolProperty(PropertyBool.Stuck, true);
            SetBoolProperty(PropertyBool.Attackable, true);

            WeenieHeaderFlags = 0;
            SetIntProperty(PropertyInt.ItemsCapacity, 102);
            SetIntProperty(PropertyInt.ContainersCapacity, 7);           
            SetIntProperty(PropertyInt.ItemUseable, (int)Usable.No);
            SetIntProperty(PropertyInt.ShowableOnRadar, (byte)RadarBehavior.ShowAlways);

            PhysicsState = 0;

            WeenieClassId = 1;
            SetIntProperty(PropertyInt.WeenieType, (int)Enum.WeenieType.Creature); // This might need to change
            SetDataIdProperty(PropertyDataId.Icon, 100667446);
            SetIntProperty(PropertyInt.ItemType, (int)Enum.ItemType.Creature);
            // SetIntProperty(PropertyInt.RadarBlipColor, (byte)RadarColor.White);
            SetBoolProperty(PropertyBool.IsDeleted, false);
            SetIntProperty(PropertyInt.TotalLogins, 0);
            SetInt64Property(PropertyInt64.DeleteTime, 0);
            SetIntProperty(PropertyInt.Level, 1);
            SetInt64Property(PropertyInt64.AvailableExperience, 0);
            SetInt64Property(PropertyInt64.TotalExperience, 0);

            SetIntProperty(PropertyInt.EncumbranceVal, 0);

            SetBoolProperty(PropertyBool.FirstEnterWorldDone, false);

            SetDoubleTimestamp(PropertyDouble.CreationTimestamp);
            SetIntProperty(PropertyInt.CreationTimestamp, (int)GetDoubleProperty(PropertyDouble.CreationTimestamp));
            SetStringProperty(PropertyString.DateOfBirth, $"{System.DateTime.UtcNow.ToString("dd MMMM yyyy")}");

            SetIntProperty(PropertyInt.CreatureType, (int)Enum.CreatureType.Human);
            SetIntProperty(PropertyInt.ChannelsAllowed, (int)Channel.AllChans);
            SetIntProperty(PropertyInt.ChannelsActive, (int)Channel.AllBroadcast);

            SetIntProperty(PropertyInt.NumDeaths, 0);

            SetIntProperty(PropertyInt.ChessRank, 1400);
            SetIntProperty(PropertyInt.ChessTotalGames, 0);
            SetIntProperty(PropertyInt.ChessGamesLost, 0);
            SetIntProperty(PropertyInt.ChessGamesWon, 0);
            SetIntProperty(PropertyInt.FakeFishingSkill, 0);            

            SetDoubleProperty(PropertyDouble.GlobalXpMod, 0);
            SetIntProperty(PropertyInt.HealingBoostRating, 0);
            SetIntProperty(PropertyInt.WeaknessRating, 0);
            SetIntProperty(PropertyInt.NetherOverTime, 0);
            SetIntProperty(PropertyInt.NetherResistRating, 0);
            SetIntProperty(PropertyInt.DotResistRating, 0);
            SetIntProperty(PropertyInt.LifeResistRating, 0);
            SetIntProperty(PropertyInt.WeaponAuraDamage, 0);
            SetIntProperty(PropertyInt.WeaponAuraSpeed, 0);
            SetIntProperty(PropertyInt.PKDamageRating, 0);
            SetIntProperty(PropertyInt.PKDamageResistRating, 0);
            SetIntProperty(PropertyInt.Overpower, 0);
            SetIntProperty(PropertyInt.OverpowerResist, 0);
            SetIntProperty(PropertyInt.GearOverpower, 0);
            SetIntProperty(PropertyInt.GearOverpowerResist, 0);
            SetDoubleProperty(PropertyDouble.WeaponAuraOffense, 0);
            SetDoubleProperty(PropertyDouble.WeaponAuraDefense, 0);
            SetDoubleProperty(PropertyDouble.WeaponAuraElemental, 0);
            SetDoubleProperty(PropertyDouble.WeaponAuraManaConv, 0);
            SetInt64Property(PropertyInt64.AvailableLuminance, 0);
            SetInt64Property(PropertyInt64.MaximumLuminance, 0);
            SetIntProperty(PropertyInt.LumAugDamageRating, 0);
            SetIntProperty(PropertyInt.LumAugDamageReductionRating, 0);
            SetIntProperty(PropertyInt.LumAugCritDamageRating, 0);
            SetIntProperty(PropertyInt.LumAugCritReductionRating, 0);
            SetIntProperty(PropertyInt.LumAugSurgeChanceRating, 0);
            SetIntProperty(PropertyInt.LumAugItemManaUsage, 0);
            SetIntProperty(PropertyInt.LumAugItemManaGain, 0);
            SetIntProperty(PropertyInt.LumAugHealingRating, 0);
            SetIntProperty(PropertyInt.LumAugSkilledCraft, 0);
            SetIntProperty(PropertyInt.LumAugSkilledSpec, 0);
            SetIntProperty(PropertyInt.LumAugAllSkills, 0);
            SetIntProperty(PropertyInt.Enlightenment, 0);
            SetIntProperty(PropertyInt.GearDamage, 0);
            SetIntProperty(PropertyInt.GearDamageResist, 0);
            SetIntProperty(PropertyInt.GearCrit, 0);
            SetIntProperty(PropertyInt.GearCritResist, 0);
            SetIntProperty(PropertyInt.GearCritDamage, 0);
            SetIntProperty(PropertyInt.GearCritDamageResist, 0);
            SetIntProperty(PropertyInt.GearHealingBoost, 0);
            SetIntProperty(PropertyInt.GearNetherResist, 0);
            SetIntProperty(PropertyInt.GearLifeResist, 0);
            SetIntProperty(PropertyInt.GearMaxHealth, 0);
            SetIntProperty(PropertyInt.GearPKDamageRating, 0);
            SetIntProperty(PropertyInt.GearPKDamageResistRating, 0);

            SetBoolProperty(PropertyBool.Account15Days, true);

            SetIntProperty(PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.NPK);
        }

        public ReadOnlyCollection<Friend> Friends { get; set; }

        public uint CharacterSlot { get; set; }

        public int CharacterOptions1Mapping
        {
            get { return GetIntProperty(PropertyInt.CharacterOptions1) ?? 0; }
            set { SetIntProperty(PropertyInt.CharacterOptions1, value); }
        }

        public int CharacterOptions2Mapping
        {
            get { return GetIntProperty(PropertyInt.CharacterOptions2) ?? 0; }
            set { SetIntProperty(PropertyInt.CharacterOptions2, value); }
        }

        public new int TotalLogins
        {
            get { return GetIntProperty(PropertyInt.TotalLogins) ?? 0; }
            set { SetIntProperty(PropertyInt.TotalLogins, value); }
        }
        
        public bool Deleted
        {
            get { return GetBoolProperty(PropertyBool.IsDeleted) ?? false; }
            set { SetBoolProperty(PropertyBool.IsDeleted, value); }
        }

        public new ulong DeleteTime
        {
            get { return GetInt64Property(PropertyInt64.DeleteTime) ?? 0; }
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
            get { return GetInt64Property(PropertyInt64.AvailableExperience) ?? 0; }
            set { SetInt64Property(PropertyInt64.AvailableExperience, value); }
        }

        public new ulong TotalExperience
        {
            get { return GetInt64Property(PropertyInt64.TotalExperience) ?? 0; }
            set { SetInt64Property(PropertyInt64.TotalExperience, value); }
        }

        public new int Age
        {
            get { return GetIntProperty(PropertyInt.Age) ?? 0; }
            set { SetIntProperty(PropertyInt.Age, value); }
        }

        public new bool IsDeleted
        {
            get { return GetBoolProperty(PropertyBool.IsDeleted) ?? false; }
            set { SetBoolProperty(PropertyBool.IsDeleted, value); }
        }

        public new int AvailableSkillCredits
        {
            get { return GetIntProperty(PropertyInt.AvailableSkillCredits) ?? 0; }
            set { SetIntProperty(PropertyInt.AvailableSkillCredits, value); }
        }

        public new int TotalSkillCredits
        {
            get { return GetIntProperty(PropertyInt.TotalSkillCredits) ?? 0; }
            set { SetIntProperty(PropertyInt.TotalSkillCredits, value); }
        }

        public new int NumDeaths
        {
            get { return GetIntProperty(PropertyInt.NumDeaths) ?? 0; }
            set { SetIntProperty(PropertyInt.NumDeaths, value); }
        }

        public new int DeathLevel
        {
            get { return GetIntProperty(PropertyInt.DeathLevel) ?? 0; }
            set { SetIntProperty(PropertyInt.DeathLevel, value); }
        }

        public new int VitaeCpPool
        {
            get { return GetIntProperty(PropertyInt.VitaeCpPool) ?? 0; }
            set { SetIntProperty(PropertyInt.VitaeCpPool, value); }
        }

        public new bool IsAdmin
        {
            get { return GetBoolProperty(PropertyBool.IsAdmin) ?? false; }
            set { SetBoolProperty(PropertyBool.IsAdmin, value); }
        }

        public new bool IsEnvoy
        {
            get { return GetBoolProperty(PropertyBool.IsSentinel) ?? false; }
            set { SetBoolProperty(PropertyBool.IsSentinel, value); }
        }

        public new bool IsArch
        {
            get { return GetBoolProperty(PropertyBool.IsArch) ?? false; }
            set { SetBoolProperty(PropertyBool.IsArch, value); }
        }

        public new bool IsPsr
        {
            get { return GetBoolProperty(PropertyBool.IsPsr) ?? false; }
            set { SetBoolProperty(PropertyBool.IsPsr, value); }
        }

        public uint EyesTexture
        {
            get { return GetDataIdProperty(PropertyDataId.EyesTexture) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.EyesTexture, value); }
        }

        public uint DefaultEyesTexture
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultEyesTexture) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.DefaultEyesTexture, value); }
        }

        public uint NoseTexture
        {
            get { return GetDataIdProperty(PropertyDataId.NoseTexture) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.NoseTexture, value); }
        }

        public uint DefaultNoseTexture
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultNoseTexture) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.DefaultNoseTexture, value); }
        }

        public uint MouthTexture
        {
            get { return GetDataIdProperty(PropertyDataId.MouthTexture) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.MouthTexture, value); }
        }

        public uint DefaultMouthTexture
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultMouthTexture) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.DefaultMouthTexture, value); }
        }

        public uint HairTexture
        {
            get { return GetDataIdProperty(PropertyDataId.HairTexture) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.HairTexture, value); }
        }

        public uint DefaultHairTexture
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultHairTexture) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.DefaultHairTexture, value); }
        }

        public uint HeadObject
        {
            get { return GetDataIdProperty(PropertyDataId.HeadObject) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.HeadObject, value); }
        }

        public uint SkinPalette
        {
            get { return GetDataIdProperty(PropertyDataId.SkinPalette) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.SkinPalette, value); }
        }

        public uint HairPalette
        {
            get { return GetDataIdProperty(PropertyDataId.HairPalette) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.HairPalette, value); }
        }

        public uint EyesPalette
        {
            get { return GetDataIdProperty(PropertyDataId.EyesPalette) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.EyesPalette, value); }
        }

        public uint? SetupTableId
        {
            get { return GetDataIdProperty(PropertyDataId.Setup); }
            set { SetDataIdProperty(PropertyDataId.Setup, value); }
        }

        public uint? MotionTableId
        {
            get { return GetDataIdProperty(PropertyDataId.MotionTable); }
            set { SetDataIdProperty(PropertyDataId.MotionTable, value); }
        }

        public ushort? PhysicsScript
        {
            get { return (ushort?)GetDataIdProperty(PropertyDataId.PhysicsScript); }
            set { SetDataIdProperty(PropertyDataId.PhysicsScript, value); }
        }

        public uint? PhysicsTableId
        {
            get { return GetDataIdProperty(PropertyDataId.PhysicsEffectTable); }
            set { SetDataIdProperty(PropertyDataId.PhysicsEffectTable, value); }
        }

        public uint? SoundTableId
        {
            get { return GetDataIdProperty(PropertyDataId.SoundTable); }
            set { SetDataIdProperty(PropertyDataId.SoundTable, value); }
        }

        public uint? CombatTableId
        {
            get { return GetDataIdProperty(PropertyDataId.CombatTable); }
            set { SetDataIdProperty(PropertyDataId.CombatTable, value); }
        }

        public new int Level
        {
            get { return GetIntProperty(PropertyInt.Level) ?? 1; }
            set { SetIntProperty(PropertyInt.Level, value); }
        }

        public uint? PaletteId
        {
            get { return GetDataIdProperty(PropertyDataId.PaletteBase); }
            set { SetDataIdProperty(PropertyDataId.PaletteBase, value); }
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

        /// <summary>
        /// Sets the skill to trained status for a character
        /// </summary>
        /// <param name="skill"></param>
        public bool TrainSkill(Skill skill, int creditsSpent)
        {
            CreatureSkill cs = GetSkillProperty(skill);
            if (cs != null && cs.Status != SkillStatus.Trained && cs.Status != SkillStatus.Specialized)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    var newSkill = new CreatureSkill(this, skill, SkillStatus.Trained, 0, 0);
                    SetSkillProperty(skill, newSkill);
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
            CreatureSkill cs = GetSkillProperty(skill);
            if (cs != null && cs.Status == SkillStatus.Trained)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    RefundXp(cs.ExperienceSpent);
                    var newSkill = new CreatureSkill(this, skill, SkillStatus.Specialized, 0, 0);
                    SetSkillProperty(skill, newSkill);
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
            CreatureSkill cs = GetSkillProperty(skill);
            if (cs != null && cs.Status != SkillStatus.Trained && cs.Status != SkillStatus.Specialized) 
            {
                var newSkill = new CreatureSkill(this, skill, SkillStatus.Untrained, 0, 0);
                SetSkillProperty(skill, newSkill);
                return true;
            }
            else if (cs != null && cs.Status == SkillStatus.Trained)
            {
                RefundXp(cs.ExperienceSpent);
                var newSkill = new CreatureSkill(this, skill, SkillStatus.Untrained, 0, 0);
                SetSkillProperty(skill, newSkill);
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
