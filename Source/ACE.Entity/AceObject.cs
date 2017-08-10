using System.Collections.Generic;
using ACE.Common;
using MySql.Data.MySqlClient;
using ACE.Entity.Enum.Properties;
using System;
using ACE.Entity.Enum;
using System.Linq;

namespace ACE.Entity
{
    [DbTable("ace_object")]
    public class AceObject : ICreatureStats, ICloneable, IDirty
    ////public class AceObject : ICloneable, IDirty
    {
        public const uint WEENIE_MAX = 199999;

        public AceObject(uint id)
            : this()
        {
            AceObjectId = id;
        }

        public AceObject()
        {
        }

        public bool IsDirty { get; set; }

        /// <summary>
        /// flag to indicate whether or not this instance came from the database
        /// or was created by the game engine.  use case: when calling "SaveObject"
        /// in the database, we need to know whether to insert or update.  There's 
        /// really no other way to tell at present.
        /// </summary>
        public bool HasEverBeenSavedToDatabase { get; set; } = false;

        /// <summary>
        /// Table field Primary Key
        /// </summary>
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true, ListGet = true, ListDelete = true)]
        public virtual uint AceObjectId { get; set; }

        /// <summary>
        /// Table Field Weenie Class
        /// </summary>
        [DbField("weenieClassId", (int)MySqlDbType.UInt32)]
        public uint WeenieClassId { get; set; }

        private uint _aceObjectDescriptionFlags = 0;

        /// <summary>
        /// Table Field Flags
        /// </summary>
        [DbField("aceObjectDescriptionFlags", (int)MySqlDbType.UInt32)]
        public uint AceObjectDescriptionFlags
        {
            get { return _aceObjectDescriptionFlags; }
            set
            {
                _aceObjectDescriptionFlags = value;
                IsDirty = true;
            }
        }

        private uint _physicsDescriptionFlag = 0;

        /// <summary>
        /// Table Field - Flags
        /// </summary>
        [DbField("physicsDescriptionFlag", (int)MySqlDbType.UInt32)]
        public uint PhysicsDescriptionFlag
        {
            get { return _physicsDescriptionFlag; }
            set
            {
                _physicsDescriptionFlag = value;
                IsDirty = true;
            }
        }

        private uint _weenieHeaderFlags = 0;

        /// <summary>
        /// Table Field - Flags
        /// </summary>
        [DbField("weenieHeaderFlags", (int)MySqlDbType.UInt32)]
        public uint WeenieHeaderFlags
        {
            get { return _weenieHeaderFlags; }
            set
            {
                _weenieHeaderFlags = value;
                IsDirty = true;
            }
        }

        private uint _weenieHeaderFlags2 = 0;

        /// <summary>
        /// Table Field - Flags
        /// </summary>
        // [DbField("weenieHeaderFlags2", (int)MySqlDbType.UInt32)] // this field isn't stored in database
        public uint WeenieHeaderFlags2
        {
            get { return _weenieHeaderFlags2; }
            set
            {
                _weenieHeaderFlags2 = value;
                IsDirty = true;
            }
        }

        private string _currentMotionState = null;

        [DbField("currentMotionState", (int)MySqlDbType.Text)]
        public string CurrentMotionState
        {
            get { return _currentMotionState; }
            set
            {
                _currentMotionState = value;
                IsDirty = true;
            }
        }

        public CreatureAbility StrengthAbility
        {
            get { return GetAttributeProperty(Ability.Strength); }
            set { SetAttributeProperty(Ability.Strength, value); }
        }

        public CreatureAbility EnduranceAbility
        {
            get { return GetAttributeProperty(Ability.Endurance); }
            set { SetAttributeProperty(Ability.Endurance, value); }
        }

        public CreatureAbility CoordinationAbility
        {
            get { return GetAttributeProperty(Ability.Coordination); }
            set { SetAttributeProperty(Ability.Coordination, value); }
        }

        public CreatureAbility QuicknessAbility
        {
            get { return GetAttributeProperty(Ability.Quickness); }
            set { SetAttributeProperty(Ability.Quickness, value); }
        }

        public CreatureAbility FocusAbility
        {
            get { return GetAttributeProperty(Ability.Focus); }
            set { SetAttributeProperty(Ability.Focus, value); }
        }

        public CreatureAbility SelfAbility
        {
            get { return GetAttributeProperty(Ability.Self); }
            set { SetAttributeProperty(Ability.Self, value); }
        }

        public CreatureVital Health
        {
            get { return GetAttribute2ndProperty(Ability.Health); }
            set { SetAttribute2ndProperty(Ability.Health, value); }
        }

        public CreatureVital Stamina
        {
            get { return GetAttribute2ndProperty(Ability.Stamina); }
            set { SetAttribute2ndProperty(Ability.Stamina, value); }
        }

        public CreatureVital Mana
        {
            get { return GetAttribute2ndProperty(Ability.Mana); }
            set { SetAttribute2ndProperty(Ability.Mana, value); }
        }

        public uint Strength
        { get { return StrengthAbility.MaxValue; } }

        public uint Endurance
        { get { return EnduranceAbility.MaxValue; } }

        public uint Coordination
        { get { return CoordinationAbility.MaxValue; } }

        public uint Quickness
        { get { return QuicknessAbility.MaxValue; } }

        public uint Focus
        { get { return FocusAbility.MaxValue; } }

        public uint Self
        { get { return SelfAbility.MaxValue; } }

        public uint? SetupDID
        {
            get { return GetDataIdProperty(PropertyDataId.Setup); }
            set { SetDataIdProperty(PropertyDataId.Setup, value); }
        }

        public uint? MotionTableDID
        {
            get { return GetDataIdProperty(PropertyDataId.MotionTable); }
            set { SetDataIdProperty(PropertyDataId.MotionTable, value); }
        }

        public uint? SoundTableDID
        {
            get { return GetDataIdProperty(PropertyDataId.SoundTable); }
            set { SetDataIdProperty(PropertyDataId.SoundTable, value); }
        }

        public uint? PhysicsEffectTableDID
        {
            get { return GetDataIdProperty(PropertyDataId.PhysicsEffectTable); }
            set { SetDataIdProperty(PropertyDataId.PhysicsEffectTable, value); }
        }

        public uint? CombatTableDID
        {
            get { return GetDataIdProperty(PropertyDataId.CombatTable); }
            set { SetDataIdProperty(PropertyDataId.CombatTable, value); }
        }

        public uint? PhysicsState
        {
            get { return GetIntProperty(PropertyInt.PhysicsState); }
            set { SetIntProperty(PropertyInt.PhysicsState, value); }
        }

        public uint? WeenieType
        {
            get { return GetIntProperty(PropertyInt.WeenieType); }
            set { SetIntProperty(PropertyInt.WeenieType, value); }
        }

        public uint? ItemType
        {
            get { return GetIntProperty(PropertyInt.ItemType); }
            set { SetIntProperty(PropertyInt.ItemType, value); }
        }

        public uint? IconDID
        {
            get { return GetDataIdProperty(PropertyDataId.Icon); }
            set { SetDataIdProperty(PropertyDataId.Icon, value); }
        }

        public string Name
        {
            get { return GetStringProperty(PropertyString.Name); }
            set { SetStringProperty(PropertyString.Name, value); }
        }

        public string PluralName
        {
            get { return GetStringProperty(PropertyString.PluralName); }
            set { SetStringProperty(PropertyString.PluralName, value); }
        }

        public byte? ItemsCapacity
        {
            get { return (byte?)GetIntProperty(PropertyInt.ItemsCapacity); }
            set { SetIntProperty(PropertyInt.ItemsCapacity, (uint)value); }
        }

        public byte? ContainersCapacity
        {
            get { return (byte?)GetIntProperty(PropertyInt.ContainersCapacity); }
            set { SetIntProperty(PropertyInt.ContainersCapacity, (uint)value); }
        }

        public uint? AmmoType
        {
            get { return GetIntProperty(PropertyInt.AmmoType); }
            set { SetIntProperty(PropertyInt.AmmoType, (uint)value); }
        }

        public uint? Value
        {
            get { return GetIntProperty(PropertyInt.Value); }
            set { SetIntProperty(PropertyInt.Value, value); }
        }

        public uint? ItemUseable
        {
            get { return GetIntProperty(PropertyInt.ItemUseable); }
            set { SetIntProperty(PropertyInt.ItemUseable, (uint?)value); }
        }

        public float? UseRadius
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.UseRadius); }
            set { SetDoubleProperty(PropertyDouble.UseRadius, value); }
        }

        public uint? TargetType
        {
            get { return GetIntProperty(PropertyInt.TargetType); }
            set { SetIntProperty(PropertyInt.TargetType, value); }
        }

        public uint? UiEffects
        {
            get { return GetIntProperty(PropertyInt.UiEffects); }
            set { SetIntProperty(PropertyInt.UiEffects, value); }
        }

        public byte? CombatUse
        {
            get { return (byte?)GetIntProperty(PropertyInt.CombatUse); }
            set { SetIntProperty(PropertyInt.CombatUse, value); }
        }

        public uint? DefaultCombatStyle
        {
            get { return GetIntProperty(PropertyInt.DefaultCombatStyle); }
            set { SetIntProperty(PropertyInt.DefaultCombatStyle, value); }
        }

        public ushort? Structure
        {
            get { return (ushort?)GetIntProperty(PropertyInt.Structure); }
            set { SetIntProperty(PropertyInt.Structure, value); }
        }

        public ushort? MaxStructure
        {
            get { return (ushort?)GetIntProperty(PropertyInt.MaxStructure); }
            set { SetIntProperty(PropertyInt.MaxStructure, value); }
        }

        public ushort? StackSize
        {
            get { return (ushort?)GetIntProperty(PropertyInt.StackSize); }
            set { SetIntProperty(PropertyInt.StackSize, value); }
        }

        public ushort? MaxStackSize
        {
            get { return (ushort?)GetIntProperty(PropertyInt.MaxStackSize); }
            set { SetIntProperty(PropertyInt.MaxStackSize, value); }
        }

        public uint? ContainerIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Container); }
            set { SetInstanceIdProperty(PropertyInstanceId.Container, value); }
        }

        public uint? WielderIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Wielder); }
            set { SetInstanceIdProperty(PropertyInstanceId.Wielder, value); }
        }

        public uint? GeneratorIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Generator); }
            set { SetInstanceIdProperty(PropertyInstanceId.Generator, value); }
        }

        // LOCATIONS
        public uint? ValidLocations
        {
            get { return GetIntProperty(PropertyInt.ValidLocations); }
            set { SetIntProperty(PropertyInt.ValidLocations, value); }
        }

        public uint? CurrentWieldedLocation
        {
            get { return GetIntProperty(PropertyInt.CurrentWieldedLocation); }
            set { SetIntProperty(PropertyInt.CurrentWieldedLocation, value); }
        }

        public uint? ClothingPriority
        {
            get { return GetIntProperty(PropertyInt.ClothingPriority); }
            set { SetIntProperty(PropertyInt.ClothingPriority, value); }
        }

        public byte? RadarBlipColor
        {
            get { return (byte?)GetIntProperty(PropertyInt.RadarBlipColor); }
            set { SetIntProperty(PropertyInt.RadarBlipColor, value); }
        }

        public byte? ShowableOnRadar
        {
            get { return (byte?)GetIntProperty(PropertyInt.ShowableOnRadar); }
            set { SetIntProperty(PropertyInt.ShowableOnRadar, value); }
        }

        public ushort? PhysicsScriptDID
        {
            get { return (ushort?)GetDataIdProperty(PropertyDataId.PhysicsScript); }
            set { SetDataIdProperty(PropertyDataId.PhysicsScript, value); }
        }

        public uint? ItemWorkmanship
        {
            get { return GetIntProperty(PropertyInt.ItemWorkmanship); }
            set { SetIntProperty(PropertyInt.ItemWorkmanship, value); }
        }

        public ushort? EncumbranceVal
        {
            get { return (ushort?)GetIntProperty(PropertyInt.EncumbranceVal); }
            set { SetIntProperty(PropertyInt.EncumbranceVal, value); }
        }

        public uint? SpellDID
        {
            get { return GetDataIdProperty(PropertyDataId.Spell); }
            set { SetDataIdProperty(PropertyDataId.Spell, value); }
        }

        public ushort? HookType
        {
            get { return (ushort?)GetIntProperty(PropertyInt.HookType); }
            set { SetIntProperty(PropertyInt.HookType, value); }
        }

        public ushort? HookItemType
        {
            get { return (ushort?)GetIntProperty(PropertyInt.HookItemType); }
            set { SetIntProperty(PropertyInt.HookItemType, value); }
        }

        public uint? IconOverlayDID
        {
            get { return GetDataIdProperty(PropertyDataId.IconOverlay); }
            set { SetDataIdProperty(PropertyDataId.IconOverlay, value); }
        }

        public uint? IconUnderlayDID
        {
            get { return GetDataIdProperty(PropertyDataId.IconUnderlay); }
            set { SetDataIdProperty(PropertyDataId.IconUnderlay, value); }
        }

        public byte? MaterialType
        {
            get { return (byte?)GetIntProperty(PropertyInt.MaterialType); }
            set { SetIntProperty(PropertyInt.MaterialType, (byte?)value); }
        }

        public uint? SharedCooldown
        {
            get { return GetIntProperty(PropertyInt.SharedCooldown); }
            set { SetIntProperty(PropertyInt.SharedCooldown, value); }
        }

        public double? CooldownDuration
        {
            get { return GetDoubleProperty(PropertyDouble.CooldownDuration); }
            set { SetDoubleProperty(PropertyDouble.CooldownDuration, value); }
        }

        // Wielder is Parent, No such thing as PropertyInstanceId.Parent
        public uint? ParentIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Wielder); }
            set { SetInstanceIdProperty(PropertyInstanceId.Wielder, value); }
        }

        public uint? ParentLocation
        {
            get { return GetIntProperty(PropertyInt.ParentLocation); }
            set { SetIntProperty(PropertyInt.ParentLocation, value); }
        }

        public float? DefaultScale
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.DefaultScale); }
            set { SetDoubleProperty(PropertyDouble.DefaultScale, value); }
        }

        public float? Friction
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Friction); }
            set { SetDoubleProperty(PropertyDouble.Friction, value); }
        }

        public float? Elasticity
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Elasticity); }
            set { SetDoubleProperty(PropertyDouble.Elasticity, value); }
        }

        public uint? PlacementPosition
        {
            get { return GetIntProperty(PropertyInt.PlacementPosition); }
            set { SetIntProperty(PropertyInt.PlacementPosition, value); }
        }

        public float? Translucency
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Translucency); }
            set { SetDoubleProperty(PropertyDouble.Translucency, value); }
        }

        // public uint? DefaultScriptId
        // {
        //    get { return GetDataIdProperty(PropertyDataId.PhysicsScript); }
        //    set { SetDataIdProperty(PropertyDataId.PhysicsScript, value); }
        // }

        public float? PhysicsScriptIntensity
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.PhysicsScriptIntensity); }
            set { SetDoubleProperty(PropertyDouble.PhysicsScriptIntensity, value); }
        }

        public uint? PaletteBaseDID
        {
            get { return GetDataIdProperty(PropertyDataId.PaletteBase); }
            set { SetDataIdProperty(PropertyDataId.PaletteBase, value); }
        }

        public uint? ClothingBaseDID
        {
            get { return GetDataIdProperty(PropertyDataId.ClothingBase); }
            set { SetDataIdProperty(PropertyDataId.ClothingBase, value); }
        }

        public uint? CharacterOptions1
        {
            get { return GetIntProperty(PropertyInt.CharacterOptions1); }
            set { SetIntProperty(PropertyInt.CharacterOptions1, value); }
        }

        public uint? CharacterOptions2
        {
            get { return GetIntProperty(PropertyInt.CharacterOptions2); }
            set { SetIntProperty(PropertyInt.CharacterOptions2, value); }
        }

        public uint? TotalLogins
        {
            get { return GetIntProperty(PropertyInt.TotalLogins); }
            set { SetIntProperty(PropertyInt.TotalLogins, value); }
        }

        public uint? AccountIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Account); }
            set { SetInstanceIdProperty(PropertyInstanceId.Account, value); }
        }

        public bool? IsDeleted
        {
            get { return GetBoolProperty(PropertyBool.IsDeleted); }
            set { SetBoolProperty(PropertyBool.IsDeleted, value); }
        }

        public ulong? DeleteTime
        {
            get { return GetInt64Property(PropertyInt64.DeleteTime); }
            set { SetInt64Property(PropertyInt64.DeleteTime, value); }
        }

        public ulong? AvailableExperience
        {
            get { return GetInt64Property(PropertyInt64.AvailableExperience); }
            set { SetInt64Property(PropertyInt64.AvailableExperience, value); }
        }

        public ulong? TotalExperience
        {
            get { return GetInt64Property(PropertyInt64.TotalExperience); }
            set { SetInt64Property(PropertyInt64.TotalExperience, value); }
        }

        public uint? Age
        {
            get { return GetIntProperty(PropertyInt.Age); }
            set { SetIntProperty(PropertyInt.Age, value); }
        }

        public string DateOfBirth
        {
            get { return GetStringProperty(PropertyString.DateOfBirth); }
            set { SetStringProperty(PropertyString.DateOfBirth, value); }
        }

        public uint? AvailableSkillCredits
        {
            get { return GetIntProperty(PropertyInt.AvailableSkillCredits); }
            set { SetIntProperty(PropertyInt.AvailableSkillCredits, value); }
        }

        public uint? TotalSkillCredits
        {
            get { return GetIntProperty(PropertyInt.TotalSkillCredits); }
            set { SetIntProperty(PropertyInt.TotalSkillCredits, value); }
        }

        public uint? NumDeaths
        {
            get { return GetIntProperty(PropertyInt.NumDeaths); }
            set { SetIntProperty(PropertyInt.NumDeaths, value); }
        }

        public uint? DeathLevel
        {
            get { return GetIntProperty(PropertyInt.DeathLevel); }
            set { SetIntProperty(PropertyInt.DeathLevel, value); }
        }

        public uint? VitaeCpPool
        {
            get { return GetIntProperty(PropertyInt.VitaeCpPool); }
            set { SetIntProperty(PropertyInt.VitaeCpPool, value); }
        }

        public bool? IsAdmin
        {
            get { return GetBoolProperty(PropertyBool.IsAdmin); }
            set { SetBoolProperty(PropertyBool.IsAdmin, value); }
        }

        public bool? IsEnvoy
        {
            get { return GetBoolProperty(PropertyBool.IsSentinel); }
            set { SetBoolProperty(PropertyBool.IsSentinel, value); }
        }

        public bool? IsArch
        {
            get { return GetBoolProperty(PropertyBool.IsArch); }
            set { SetBoolProperty(PropertyBool.IsArch, value); }
        }

        public bool? IsPsr
        {
            get { return GetBoolProperty(PropertyBool.IsPsr); }
            set { SetBoolProperty(PropertyBool.IsPsr, value); }
        }

        public uint? Heritage
        {
            get { return GetIntProperty(PropertyInt.HeritageGroup); }
            set { SetIntProperty(PropertyInt.HeritageGroup, value); }
        }

        public uint? Gender
        {
            get { return GetIntProperty(PropertyInt.Gender); }
            set { SetIntProperty(PropertyInt.Gender, value); }
        }

        public uint? EyesTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.EyesTexture); }
            set { SetDataIdProperty(PropertyDataId.EyesTexture, value); }
        }

        public uint? DefaultEyesTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultEyesTexture); }
            set { SetDataIdProperty(PropertyDataId.DefaultEyesTexture, value); }
        }

        public uint? NoseTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.NoseTexture); }
            set { SetDataIdProperty(PropertyDataId.NoseTexture, value); }
        }

        public uint? DefaultNoseTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultNoseTexture); }
            set { SetDataIdProperty(PropertyDataId.DefaultNoseTexture, value); }
        }

        public uint? MouthTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.MouthTexture); }
            set { SetDataIdProperty(PropertyDataId.MouthTexture, value); }
        }

        public uint? DefaultMouthTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultMouthTexture); }
            set { SetDataIdProperty(PropertyDataId.DefaultMouthTexture, value); }
        }

        public uint? HairTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.HairTexture); }
            set { SetDataIdProperty(PropertyDataId.HairTexture, value); }
        }

        public uint? DefaultHairTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultHairTexture); }
            set { SetDataIdProperty(PropertyDataId.DefaultHairTexture, value); }
        }

        public uint? HeadObjectDID
        {
            get { return GetDataIdProperty(PropertyDataId.HeadObject); }
            set { SetDataIdProperty(PropertyDataId.HeadObject, value); }
        }

        public uint? SkinPaletteDID
        {
            get { return GetDataIdProperty(PropertyDataId.SkinPalette); }
            set { SetDataIdProperty(PropertyDataId.SkinPalette, value); }
        }

        public uint? HairPaletteDID
        {
            get { return GetDataIdProperty(PropertyDataId.HairPalette); }
            set { SetDataIdProperty(PropertyDataId.HairPalette, value); }
        }

        public uint? EyesPaletteDID
        {
            get { return GetDataIdProperty(PropertyDataId.EyesPalette); }
            set { SetDataIdProperty(PropertyDataId.EyesPalette, value); }
        }

        public uint? Level
        {
            get { return GetIntProperty(PropertyInt.Level); }
            set { SetIntProperty(PropertyInt.Level, value); }
        }

        public bool? GeneratorStatus
        {
            get { return GetBoolProperty(PropertyBool.GeneratorStatus); }
            set { SetBoolProperty(PropertyBool.GeneratorStatus, value); }
        }

        public bool? GeneratorEnteredWorld
        {
            get { return GetBoolProperty(PropertyBool.GeneratorEnteredWorld); }
            set { SetBoolProperty(PropertyBool.GeneratorEnteredWorld, value); }
        }

        public bool? GeneratorDisabled
        {
            get { return GetBoolProperty(PropertyBool.GeneratorDisabled); }
            set { SetBoolProperty(PropertyBool.GeneratorDisabled, value); }
        }

        public bool? GeneratedTreasureItem
        {
            get { return GetBoolProperty(PropertyBool.GeneratedTreasureItem); }
            set { SetBoolProperty(PropertyBool.GeneratedTreasureItem, value); }
        }

        public bool? GeneratorAutomaticDestruction
        {
            get { return GetBoolProperty(PropertyBool.GeneratorAutomaticDestruction); }
            set { SetBoolProperty(PropertyBool.GeneratorAutomaticDestruction, value); }
        }

        public bool? CanGenerateRare
        {
            get { return GetBoolProperty(PropertyBool.CanGenerateRare); }
            set { SetBoolProperty(PropertyBool.CanGenerateRare, value); }
        }

        public bool? CorpseGeneratedRare
        {
            get { return GetBoolProperty(PropertyBool.CorpseGeneratedRare); }
            set { SetBoolProperty(PropertyBool.CorpseGeneratedRare, value); }
        }

        public bool? SuppressGenerateEffect
        {
            get { return GetBoolProperty(PropertyBool.SuppressGenerateEffect); }
            set { SetBoolProperty(PropertyBool.SuppressGenerateEffect, value); }
        }

        public bool? ChestRegenOnClose
        {
            get { return GetBoolProperty(PropertyBool.ChestRegenOnClose); }
            set { SetBoolProperty(PropertyBool.ChestRegenOnClose, value); }
        }

        public bool? ChestClearedWhenClosed
        {
            get { return GetBoolProperty(PropertyBool.ChestClearedWhenClosed); }
            set { SetBoolProperty(PropertyBool.ChestClearedWhenClosed, value); }
        }

        public uint? GeneratorTimeType
        {
            get { return GetIntProperty(PropertyInt.GeneratorTimeType); }
            set { SetIntProperty(PropertyInt.GeneratorTimeType, value); }
        }

        public uint? GeneratorProbability
        {
            get { return GetIntProperty(PropertyInt.GeneratorProbability); }
            set { SetIntProperty(PropertyInt.GeneratorProbability, value); }
        }

        public uint? MaxGeneratedObjects
        {
            get { return GetIntProperty(PropertyInt.MaxGeneratedObjects); }
            set { SetIntProperty(PropertyInt.MaxGeneratedObjects, value); }
        }

        public uint? GeneratorType
        {
            get { return GetIntProperty(PropertyInt.GeneratorType); }
            set { SetIntProperty(PropertyInt.GeneratorType, value); }
        }

        public uint? ActivationCreateClass
        {
            get { return GetIntProperty(PropertyInt.ActivationCreateClass); }
            set { SetIntProperty(PropertyInt.ActivationCreateClass, value); }
        }

        public bool? Ethereal
        {
            get { return GetBoolProperty(PropertyBool.Ethereal); }
            set { SetBoolProperty(PropertyBool.Ethereal, value); }
        }

        public bool? Open
        {
            get { return GetBoolProperty(PropertyBool.Open); }
            set { SetBoolProperty(PropertyBool.Open, value); }
        }

        public bool? Locked
        {
            get { return GetBoolProperty(PropertyBool.Locked); }
            set { SetBoolProperty(PropertyBool.Locked, value); }
        }

        public bool? DefaultLocked
        {
            get { return GetBoolProperty(PropertyBool.DefaultLocked); }
            set { SetBoolProperty(PropertyBool.DefaultLocked, value); }
        }

        public bool? DefaultOpen
        {
            get { return GetBoolProperty(PropertyBool.DefaultOpen); }
            set { SetBoolProperty(PropertyBool.DefaultOpen, value); }
        }

        public float? ResetInterval
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.ResetInterval); }
            set { SetDoubleProperty(PropertyDouble.ResetInterval, value); }
        }

        public double? ResetTimestamp
        {
            get { return GetDoubleProperty(PropertyDouble.ResetTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.ResetTimestamp); }
        }

        public double? UseTimestamp
        {
            get { return GetDoubleProperty(PropertyDouble.UseTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.UseTimestamp); }
        }

        public double? UseLockTimestamp
        {
            get { return GetDoubleProperty(PropertyDouble.UseLockTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.UseLockTimestamp); }
        }

        public uint? LastUnlockerIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.LastUnlocker); }
            set { SetInstanceIdProperty(PropertyInstanceId.LastUnlocker, value); }
        }

        public string KeyCode
        {
            get { return GetStringProperty(PropertyString.KeyCode); }
            set { SetStringProperty(PropertyString.KeyCode, value); }
        }

        public string LockCode
        {
            get { return GetStringProperty(PropertyString.LockCode); }
            set { SetStringProperty(PropertyString.LockCode, value); }
        }

        public uint? ResistLockpick
        {
            get { return GetIntProperty(PropertyInt.ResistLockpick); }
            set { SetIntProperty(PropertyInt.ResistLockpick, value); }
        }

        public uint? AppraisalLockpickSuccessPercent
        {
            get { return GetIntProperty(PropertyInt.AppraisalLockpickSuccessPercent); }
            set { SetIntProperty(PropertyInt.AppraisalLockpickSuccessPercent, value); }
        }

        public uint? MinLevel
        {
            get { return GetIntProperty(PropertyInt.MinLevel); }
            set { SetIntProperty(PropertyInt.MinLevel, value); }
        }

        public uint? MaxLevel
        {
            get { return GetIntProperty(PropertyInt.MaxLevel); }
            set { SetIntProperty(PropertyInt.MaxLevel, value); }
        }

        public uint? PortalBitmask
        {
            get { return GetIntProperty(PropertyInt.PortalBitmask); }
            set { SetIntProperty(PropertyInt.PortalBitmask, value); }
        }

        public string AppraisalPortalDestination
        {
            get { return GetStringProperty(PropertyString.AppraisalPortalDestination); }
            set { SetStringProperty(PropertyString.AppraisalPortalDestination, value); }
        }

        public string ShortDesc
        {
            get { return GetStringProperty(PropertyString.ShortDesc); }
            set { SetStringProperty(PropertyString.ShortDesc, value); }
        }

        public string LongDesc
        {
            get { return GetStringProperty(PropertyString.LongDesc); }
            set { SetStringProperty(PropertyString.LongDesc, value); }
        }

        public string Use
        {
            get { return GetStringProperty(PropertyString.Use); }
            set { SetStringProperty(PropertyString.Use, value); }
        }

        public string UseMessage
        {
            get { return GetStringProperty(PropertyString.UseMessage); }
            set { SetStringProperty(PropertyString.UseMessage, value); }
        }

        public bool? PortalShowDestination
        {
            get { return GetBoolProperty(PropertyBool.PortalShowDestination); }
            set { SetBoolProperty(PropertyBool.PortalShowDestination, value); }
        }

        public string HeritageGroup
        {
            get { return GetStringProperty(PropertyString.HeritageGroup); }
            set { SetStringProperty(PropertyString.HeritageGroup, value); }
        }

        public string Sex
        {
            get { return GetStringProperty(PropertyString.Sex); }
            set { SetStringProperty(PropertyString.Sex, value); }
        }

        public string Title
        {
            get { return GetStringProperty(PropertyString.Title); }
            set { SetStringProperty(PropertyString.Title, value); }
        }

        public string Template
        {
            get { return GetStringProperty(PropertyString.Template); }
            set { SetStringProperty(PropertyString.Template, value); }
        }

        public string DisplayName
        {
            get { return GetStringProperty(PropertyString.DisplayName); }
            set { SetStringProperty(PropertyString.DisplayName, value); }
        }

        public uint? CharacterTitleId
        {
            get { return GetIntProperty(PropertyInt.CharacterTitleId); }
            set { SetIntProperty(PropertyInt.CharacterTitleId, value); }
        }

        public uint? NumCharacterTitles
        {
            get { return GetIntProperty(PropertyInt.NumCharacterTitles); }
            set { SetIntProperty(PropertyInt.NumCharacterTitles, value); }
        }

        public double? CreationTimestamp
        {
            get { return GetDoubleProperty(PropertyDouble.CreationTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.CreationTimestamp); }
        }

        public bool? Stuck
        {
            get { return GetBoolProperty(PropertyBool.Stuck); }
            set { SetBoolProperty(PropertyBool.Stuck, value); }
        }

        public bool? IgnoreCollisions
        {
            get { return GetBoolProperty(PropertyBool.IgnoreCollisions); }
            set { SetBoolProperty(PropertyBool.IgnoreCollisions, value); }
        }

        public bool? ReportCollisions
        {
            get { return GetBoolProperty(PropertyBool.ReportCollisions); }
            set { SetBoolProperty(PropertyBool.ReportCollisions, value); }
        }

        public bool? GravityStatus
        {
            get { return GetBoolProperty(PropertyBool.GravityStatus); }
            set { SetBoolProperty(PropertyBool.GravityStatus, value); }
        }

        public bool? LightsStatus
        {
            get { return GetBoolProperty(PropertyBool.LightsStatus); }
            set { SetBoolProperty(PropertyBool.LightsStatus, value); }
        }

        public bool? ScriptedCollision
        {
            get { return GetBoolProperty(PropertyBool.ScriptedCollision); }
            set { SetBoolProperty(PropertyBool.ScriptedCollision, value); }
        }

        public bool? Inelastic
        {
            get { return GetBoolProperty(PropertyBool.Inelastic); }
            set { SetBoolProperty(PropertyBool.Inelastic, value); }
        }

        public bool? Visibility
        {
            get { return GetBoolProperty(PropertyBool.Visibility); }
            set { SetBoolProperty(PropertyBool.Visibility, value); }
        }

        public bool? Attackable
        {
            get { return GetBoolProperty(PropertyBool.Attackable); }
            set { SetBoolProperty(PropertyBool.Attackable, value); }
        }

        public bool? AdvocateState
        {
            get { return GetBoolProperty(PropertyBool.AdvocateState); }
            set { SetBoolProperty(PropertyBool.AdvocateState, value); }
        }

        public bool? Inscribable
        {
            get { return GetBoolProperty(PropertyBool.Inscribable); }
            set { SetBoolProperty(PropertyBool.Inscribable, value); }
        }

        public bool? UiHidden
        {
            get { return GetBoolProperty(PropertyBool.UiHidden); }
            set { SetBoolProperty(PropertyBool.UiHidden, value); }
        }

        public bool? IgnoreHouseBarriers
        {
            get { return GetBoolProperty(PropertyBool.IgnoreHouseBarriers); }
            set { SetBoolProperty(PropertyBool.IgnoreHouseBarriers, value); }
        }

        public bool? HiddenAdmin
        {
            get { return GetBoolProperty(PropertyBool.HiddenAdmin); }
            set { SetBoolProperty(PropertyBool.HiddenAdmin, value); }
        }

        public bool? PkWounder
        {
            get { return GetBoolProperty(PropertyBool.PkWounder); }
            set { SetBoolProperty(PropertyBool.PkWounder, value); }
        }

        public bool? PkKiller
        {
            get { return GetBoolProperty(PropertyBool.PkKiller); }
            set { SetBoolProperty(PropertyBool.PkKiller, value); }
        }

        public bool? UnderLifestoneProtection
        {
            get { return GetBoolProperty(PropertyBool.UnderLifestoneProtection); }
            set { SetBoolProperty(PropertyBool.UnderLifestoneProtection, value); }
        }

        public bool? DefaultOn
        {
            get { return GetBoolProperty(PropertyBool.DefaultOn); }
            set { SetBoolProperty(PropertyBool.DefaultOn, value); }
        }

        public bool? IsFrozen
        {
            get { return GetBoolProperty(PropertyBool.IsFrozen); }
            set { SetBoolProperty(PropertyBool.IsFrozen, value); }
        }

        public bool? ReportCollisionsAsEnvironment
        {
            get { return GetBoolProperty(PropertyBool.ReportCollisionsAsEnvironment); }
            set { SetBoolProperty(PropertyBool.ReportCollisionsAsEnvironment, value); }
        }

        public bool? AllowEdgeSlide
        {
            get { return GetBoolProperty(PropertyBool.AllowEdgeSlide); }
            set { SetBoolProperty(PropertyBool.AllowEdgeSlide, value); }
        }

        public bool? AdvocateQuest
        {
            get { return GetBoolProperty(PropertyBool.AdvocateQuest); }
            set { SetBoolProperty(PropertyBool.AdvocateQuest, value); }
        }

        public bool? IsAdvocate
        {
            get { return GetBoolProperty(PropertyBool.IsAdvocate); }
            set { SetBoolProperty(PropertyBool.IsAdvocate, value); }
        }

        public bool? IsSentinel
        {
            get { return GetBoolProperty(PropertyBool.IsSentinel); }
            set { SetBoolProperty(PropertyBool.IsSentinel, value); }
        }

        public bool? NoDraw
        {
            get { return GetBoolProperty(PropertyBool.NoDraw); }
            set { SetBoolProperty(PropertyBool.NoDraw, value); }
        }

        public bool? IgnorePortalRestrictions
        {
            get { return GetBoolProperty(PropertyBool.IgnorePortalRestrictions); }
            set { SetBoolProperty(PropertyBool.IgnorePortalRestrictions, value); }
        }

        public bool? Retained
        {
            get { return GetBoolProperty(PropertyBool.Retained); }
            set { SetBoolProperty(PropertyBool.Retained, value); }
        }

        public bool? Invincible
        {
            get { return GetBoolProperty(PropertyBool.Invincible); }
            set { SetBoolProperty(PropertyBool.Invincible, value); }
        }

        public bool? IsGagged
        {
            get { return GetBoolProperty(PropertyBool.IsGagged); }
            set { SetBoolProperty(PropertyBool.IsGagged, value); }
        }

        public bool? Afk
        {
            get { return GetBoolProperty(PropertyBool.Afk); }
            set { SetBoolProperty(PropertyBool.Afk, value); }
        }

        public bool? IgnoreAuthor
        {
            get { return GetBoolProperty(PropertyBool.IgnoreAuthor); }
            set { SetBoolProperty(PropertyBool.IgnoreAuthor, value); }
        }

        public bool? WieldOnUse
        {
            get { return GetBoolProperty(PropertyBool.WieldOnUse); }
            set { SetBoolProperty(PropertyBool.WieldOnUse, value); }
        }

        public bool? AutowieldLeft
        {
            get { return GetBoolProperty(PropertyBool.AutowieldLeft); }
            set { SetBoolProperty(PropertyBool.AutowieldLeft, value); }
        }

        public bool? VendorService
        {
            get { return GetBoolProperty(PropertyBool.VendorService); }
            set { SetBoolProperty(PropertyBool.VendorService, value); }
        }

        public bool? RequiresBackpackSlot
        {
            get { return GetBoolProperty(PropertyBool.RequiresBackpackSlot); }
            set { SetBoolProperty(PropertyBool.RequiresBackpackSlot, value); }
        }

        public uint? ItemCurMana
        {
            get { return GetIntProperty(PropertyInt.ItemCurMana); }
            set { SetIntProperty(PropertyInt.ItemCurMana, value); }
        }

        public uint? ItemMaxMana
        {
            get { return GetIntProperty(PropertyInt.ItemMaxMana); }
            set { SetIntProperty(PropertyInt.ItemMaxMana, value); }
        }

        public bool? NpcLooksLikeObject
        {
            get { return GetBoolProperty(PropertyBool.NpcLooksLikeObject); }
            set { SetBoolProperty(PropertyBool.NpcLooksLikeObject, value); }
        }

        public uint? AllowedActivator
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.AllowedActivator); }
            set { SetInstanceIdProperty(PropertyInstanceId.AllowedActivator, value); }
        }

        public uint? CreatureType
        {
            get { return GetIntProperty(PropertyInt.CreatureType); }
            set { SetIntProperty(PropertyInt.CreatureType, value); }
        }

        public string Inscription
        {
            get { return GetStringProperty(PropertyString.Inscription); }
            set { SetStringProperty(PropertyString.Inscription, value); }
        }

        #region Books
        public string ScribeName
        {
            get { return GetStringProperty(PropertyString.ScribeName); }
            set { SetStringProperty(PropertyString.ScribeName, value); }
        }
        public string ScribeAccount
        {
            get { return GetStringProperty(PropertyString.ScribeAccount); }
            set { SetStringProperty(PropertyString.ScribeAccount, value); }
        }
        public uint? Scribe
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Scribe); }
            set { SetInstanceIdProperty(PropertyInstanceId.Scribe, value); }
        }
        #endregion

        #region Positions  
        public Position Location
        {
            get { return GetPosition(PositionType.Location); }
            set { SetPosition(PositionType.Location, value); }
        }

        public Position Destination
        {
            get { return GetPosition(PositionType.Destination); }
            set { SetPosition(PositionType.Destination, value); }
        }

        public Position Instantiation
        {
            get { return GetPosition(PositionType.Instantiation); }
            set { SetPosition(PositionType.Instantiation, value); }
        }

        public Position Sanctuary
        {
            get { return GetPosition(PositionType.Sanctuary); }
            set { SetPosition(PositionType.Sanctuary, value); }
        }

        public Position Home
        {
            get { return GetPosition(PositionType.Home); }
            set { SetPosition(PositionType.Home, value); }
        }

        public Position ActivationMove
        {
            get { return GetPosition(PositionType.ActivationMove); }
            set { SetPosition(PositionType.ActivationMove, value); }
        }

        public Position Target
        {
            get { return GetPosition(PositionType.Target); }
            set { SetPosition(PositionType.Target, value); }
        }

        public Position LinkedPortalOne
        {
            get { return GetPosition(PositionType.LinkedPortalOne); }
            set { SetPosition(PositionType.LinkedPortalOne, value); }
        }

        public Position LastPortal
        {
            get { return GetPosition(PositionType.LastPortal); }
            set { SetPosition(PositionType.LastPortal, value); }
        }

        public Position PortalStorm
        {
            get { return GetPosition(PositionType.PortalStorm); }
            set { SetPosition(PositionType.PortalStorm, value); }
        }

        public Position CrashAndTurn
        {
            get { return GetPosition(PositionType.CrashAndTurn); }
            set { SetPosition(PositionType.CrashAndTurn, value); }
        }

        public Position PortalSummonLoc
        {
            get { return GetPosition(PositionType.PortalSummonLoc); }
            set { SetPosition(PositionType.PortalSummonLoc, value); }
        }

        public Position HouseBoot
        {
            get { return GetPosition(PositionType.HouseBoot); }
            set { SetPosition(PositionType.HouseBoot, value); }
        }

        public Position LastOutsideDeath
        {
            get { return GetPosition(PositionType.LastOutsideDeath); }
            set { SetPosition(PositionType.LastOutsideDeath, value); }
        }

        public Position LinkedLifestone
        {
            get { return GetPosition(PositionType.LinkedLifestone); }
            set { SetPosition(PositionType.LinkedLifestone, value); }
        }

        public Position LinkedPortalTwo
        {
            get { return GetPosition(PositionType.LinkedPortalTwo); }
            set { SetPosition(PositionType.LinkedPortalTwo, value); }
        }

        public Position Save1
        {
            get { return GetPosition(PositionType.Save1); }
            set { SetPosition(PositionType.Save1, value); }
        }

        public Position Save2
        {
            get { return GetPosition(PositionType.Save2); }
            set { SetPosition(PositionType.Save2, value); }
        }

        public Position Save3
        {
            get { return GetPosition(PositionType.Save3); }
            set { SetPosition(PositionType.Save3, value); }
        }

        public Position Save4
        {
            get { return GetPosition(PositionType.Save4); }
            set { SetPosition(PositionType.Save4, value); }
        }

        public Position Save5
        {
            get { return GetPosition(PositionType.Save5); }
            set { SetPosition(PositionType.Save5, value); }
        }

        public Position Save6
        {
            get { return GetPosition(PositionType.Save6); }
            set { SetPosition(PositionType.Save6, value); }
        }

        public Position Save7
        {
            get { return GetPosition(PositionType.Save7); }
            set { SetPosition(PositionType.Save7, value); }
        }

        public Position Save8
        {
            get { return GetPosition(PositionType.Save8); }
            set { SetPosition(PositionType.Save8, value); }
        }

        public Position Save9
        {
            get { return GetPosition(PositionType.Save9); }
            set { SetPosition(PositionType.Save9, value); }
        }

        public Position RelativeDestination
        {
            get { return GetPosition(PositionType.RelativeDestination); }
            set { SetPosition(PositionType.RelativeDestination, value); }
        }

        public Position TeleportedCharacter
        {
            get { return GetPosition(PositionType.TeleportedCharacter); }
            set { SetPosition(PositionType.TeleportedCharacter, value); }
        }
        #endregion

        protected uint? GetDataIdProperty(PropertyDataId property)
        {
            return DataIdProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<uint> GetDataIdProperties(PropertyDataId property)
        {
            return DataIdProperties.Where(x => x.PropertyId == (uint)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetDataIdProperty(PropertyDataId didPropertyId, uint? value)
        {
            AceObjectPropertiesDataId listItem = DataIdProperties.Find(x => x.PropertyId == (short)didPropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesDataId { PropertyId = (uint)didPropertyId, PropertyValue = (uint)value, AceObjectId = AceObjectId };
                    DataIdProperties.Add(listItem);
                }
                else
                {
                    listItem.PropertyValue = (uint)value;
                }
            }
            else
            {
                if (listItem != null)
                    listItem.PropertyValue = null;
            }
        }

        protected bool? GetBoolProperty(PropertyBool property)
        {
            return BoolProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<bool> GetBoolProperties(PropertyBool property)
        {
            return BoolProperties.Where(x => x.PropertyId == (uint)property).Where(b => b.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetBoolProperty(PropertyBool propertyId, bool? value)
        {
            AceObjectPropertiesBool listItem = BoolProperties.Find(x => x.PropertyId == (short)propertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesBool { PropertyId = (uint)propertyId, PropertyValue = (bool)value, AceObjectId = AceObjectId };
                    BoolProperties.Add(listItem);
                }
                else
                {
                    listItem.PropertyValue = (bool)value;
                }
            }
            else
            {
                if (listItem != null)
                    listItem.PropertyValue = null;
            }
        }

        protected uint? GetInstanceIdProperty(PropertyInstanceId property)
        {
            return InstanceIdProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<uint> GetInstanceIdProperties(PropertyInstanceId property)
        {
            return InstanceIdProperties.Where(x => x.PropertyId == (uint)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetInstanceIdProperty(PropertyInstanceId iidPropertyId, uint? value)
        {
            AceObjectPropertiesInstanceId listItem = InstanceIdProperties.Find(x => x.PropertyId == (ushort)iidPropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesInstanceId { PropertyId = (uint)iidPropertyId, PropertyValue = (uint)value, AceObjectId = AceObjectId };
                    InstanceIdProperties.Add(listItem);
                }
                else
                {
                    listItem.PropertyValue = (uint)value;
                }
            }
            else
            {
                if (listItem != null)
                    listItem.PropertyValue = null;
            }
        }

        protected CreatureAbility GetAttributeProperty(Ability ability)
        {
            CreatureAbility ret;
            bool success = AceObjectPropertiesAttributes.TryGetValue(ability, out ret);

            if (!success || ret == null)
            {
                ret = new CreatureAbility(ability);
                AceObjectPropertiesAttributes.Add(ability, ret);
            }

            return ret;
        }

        private void SetProperty<K, V>(Dictionary<K, V> dict, K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        protected void SetAttributeProperty(Ability ability, CreatureAbility value)
        {
            SetProperty(AceObjectPropertiesAttributes, ability, value);
        }

        protected CreatureVital GetAttribute2ndProperty(Ability ability)
        {
            CreatureVital ret;
            bool success = AceObjectPropertiesAttributes2nd.TryGetValue(ability, out ret);

            if (!success || ret == null)
            {
                ret = new CreatureVital(this, ability, ability.GetRegenRate());
                AceObjectPropertiesAttributes2nd.Add(ability, ret);
            }

            return ret;
        }

        protected void SetAttribute2ndProperty(Ability ability, CreatureVital value)
        {
            SetProperty(AceObjectPropertiesAttributes2nd, ability, value);
        }

        protected CreatureSkill GetSkillProperty(Skill skill)
        {
            CreatureSkill ret;
            bool success = AceObjectPropertiesSkills.TryGetValue(skill, out ret);

            if (!success || ret == null)
            {
                ret = new CreatureSkill(this, skill, SkillStatus.Untrained, 0, 0);
                AceObjectPropertiesSkills.Add(skill, ret);
            }

            return ret;
        }

        protected void SetSkillProperty(Skill skill, CreatureSkill value)
        {
            SetProperty(AceObjectPropertiesSkills, skill, value);
        }

        public List<CreatureSkill> GetSkills()
        {
            return AceObjectPropertiesSkills.Values.ToList();
        }

        protected uint? GetIntProperty(PropertyInt property)
        {
            return IntProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<uint> GetIntProperties(PropertyInt property)
        {
            return IntProperties.Where(x => x.PropertyId == (uint)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetIntProperty(PropertyInt intPropertyId, uint? value)
        {
            AceObjectPropertiesInt listItem = IntProperties.Find(x => x.PropertyId == (ushort)intPropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesInt { PropertyId = (uint)intPropertyId, PropertyValue = (uint)value, AceObjectId = AceObjectId };
                    IntProperties.Add(listItem);
                }
                else
                {
                    listItem.PropertyValue = (uint)value;
                }
            }
            else
            {
                if (listItem != null)
                    listItem.PropertyValue = null;
            }
        }

        protected ulong? GetInt64Property(PropertyInt64 property)
        {
            return Int64Properties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<ulong> GetInt64Properties(PropertyInt64 property)
        {
            return Int64Properties.Where(x => x.PropertyId == (ushort)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetInt64Property(PropertyInt64 int64PropertyId, ulong? value)
        {
            AceObjectPropertiesInt64 listItem = Int64Properties.Find(x => x.PropertyId == (ushort)int64PropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesInt64 { PropertyId = (uint)int64PropertyId, PropertyValue = (ulong)value, AceObjectId = AceObjectId };
                    Int64Properties.Add(listItem);
                }
                else
                {
                    listItem.PropertyValue = (ulong)value;
                }
            }
            else
            {
                if (listItem != null)
                    listItem.PropertyValue = null;
            }
        }

        protected double? GetDoubleProperty(PropertyDouble property)
        {
            return DoubleProperties.FirstOrDefault(x => x.PropertyId == (ushort)property)?.PropertyValue;
        }

        protected List<double> GetDoubleProperties(PropertyDouble property)
        {
            return DoubleProperties.Where(x => x.PropertyId == (ushort)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        public void SetDoubleTimestamp(PropertyDouble propertyId)
        {
            TimeSpan span = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            double timestamp = span.TotalSeconds;
            SetDoubleProperty(propertyId, timestamp);
        }

        protected void SetDoubleProperty(PropertyDouble propertyId, double? value)
        {
            AceObjectPropertiesDouble listItem = DoubleProperties.Find(x => x.PropertyId == (short)propertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesDouble()
                    {
                        PropertyId = (ushort)propertyId,
                        PropertyValue = (double)value,
                        AceObjectId = AceObjectId
                    };

                    DoubleProperties.Add(listItem);
                }
                else
                {
                    listItem.PropertyValue = (double)value;
                }
            }
            else
            {
                if (listItem != null)
                    listItem.PropertyValue = null;
            }
        }

        protected string GetStringProperty(PropertyString property)
        {
            return StringProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<string> GetStringProperties(PropertyString property)
        {
            return StringProperties.Where(x => x.PropertyId == (uint)property).Select(x => x.PropertyValue).ToList();
        }

        protected void SetStringProperty(PropertyString propertyId, string value)
        {
            AceObjectPropertiesString listItem = StringProperties.Find(x => x.PropertyId == (ushort)propertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesString()
                    {
                        PropertyId = (ushort)propertyId,
                        PropertyValue = value,
                        AceObjectId = AceObjectId
                    };

                    StringProperties.Add(listItem);
                }
                else
                {
                    listItem.PropertyValue = value;
                }
            }
            else
            {
                if (listItem != null)
                    listItem.PropertyValue = null;
            }
        }

        public List<PaletteOverride> PaletteOverrides { get; set; } = new List<PaletteOverride>();

        public List<TextureMapOverride> TextureOverrides { get; set; } = new List<TextureMapOverride>();

        public List<AnimationOverride> AnimationOverrides { get; set; } = new List<AnimationOverride>();

        public List<AceObjectPropertiesInt> IntProperties { get; set; } = new List<AceObjectPropertiesInt>();

        public List<AceObjectPropertiesInt64> Int64Properties { get; set; } = new List<AceObjectPropertiesInt64>();

        public List<AceObjectPropertiesDouble> DoubleProperties { get; set; } = new List<AceObjectPropertiesDouble>();

        public List<AceObjectPropertiesBool> BoolProperties { get; set; } = new List<AceObjectPropertiesBool>();

        public List<AceObjectPropertiesDataId> DataIdProperties { get; set; } = new List<AceObjectPropertiesDataId>();

        public List<AceObjectPropertiesInstanceId> InstanceIdProperties { get; set; } = new List<AceObjectPropertiesInstanceId>();

        public List<AceObjectPropertiesSpell> SpellIdProperties { get; set; } = new List<AceObjectPropertiesSpell>();

        public List<AceObjectPropertiesSpellBarPositions> SpellsInSpellBars { get; set; } = new List<AceObjectPropertiesSpellBarPositions>();

        public List<AceObjectPropertiesString> StringProperties { get; set; } = new List<AceObjectPropertiesString>();

        // uint references the page
        public Dictionary<uint, AceObjectPropertiesBook> BookProperties { get; set; } = new Dictionary<uint, AceObjectPropertiesBook>();

        public List<AceObjectGeneratorLink> GeneratorLinks { get; set; } = new List<AceObjectGeneratorLink>();

        public Dictionary<Ability, CreatureAbility> AceObjectPropertiesAttributes { get; set; } = new Dictionary<Ability, CreatureAbility>();

        // ReSharper disable once InconsistentNaming
        public Dictionary<Ability, CreatureVital> AceObjectPropertiesAttributes2nd { get; set; } = new Dictionary<Ability, CreatureVital>();

        public Dictionary<Skill, CreatureSkill> AceObjectPropertiesSkills { get; set; } = new Dictionary<Skill, CreatureSkill>();

        public Dictionary<PositionType, Position> AceObjectPropertiesPositions { get; set; } = new Dictionary<PositionType, Position>();

        protected Position GetPosition(PositionType positionType)
        {
            Position ret;
            bool success = AceObjectPropertiesPositions.TryGetValue(positionType, out ret);

            if (!success)
            {
                return null;
            }
            return ret;
        }

        protected void SetPosition(PositionType positionType, Position value)
        {
            SetProperty(AceObjectPropertiesPositions, positionType, value);
        }

        public object Clone()
        {
            AceObject ret = new AceObject();
            ret.AceObjectId = AceObjectId;

            ret.WeenieClassId = WeenieClassId;
            ret.AceObjectDescriptionFlags = AceObjectDescriptionFlags;
            ret.PhysicsDescriptionFlag = PhysicsDescriptionFlag;
            ret.WeenieHeaderFlags = WeenieHeaderFlags;
            ret.HasEverBeenSavedToDatabase = HasEverBeenSavedToDatabase;

            // Then clone our properties
            ret.PaletteOverrides = CloneList(PaletteOverrides);
            ret.TextureOverrides = CloneList(TextureOverrides);
            ret.AnimationOverrides = CloneList(AnimationOverrides);
            ret.IntProperties = CloneList(IntProperties);
            ret.Int64Properties = CloneList(Int64Properties);
            ret.DoubleProperties = CloneList(DoubleProperties);
            ret.BoolProperties = CloneList(BoolProperties);
            ret.DataIdProperties = CloneList(DataIdProperties);
            ret.InstanceIdProperties = CloneList(InstanceIdProperties);
            ret.StringProperties = CloneList(StringProperties);
            ret.GeneratorLinks = CloneList(GeneratorLinks);
            ret.AceObjectPropertiesAttributes = CloneDict(AceObjectPropertiesAttributes);
            ret.AceObjectPropertiesAttributes2nd = CloneDict(AceObjectPropertiesAttributes2nd);
            ret.AceObjectPropertiesSkills = CloneDict(AceObjectPropertiesSkills);
            ret.AceObjectPropertiesPositions = CloneDict(AceObjectPropertiesPositions);
            ret.SpellIdProperties = CloneList(SpellIdProperties);
            ret.SpellsInSpellBars = CloneList(SpellsInSpellBars);
            ret.BookProperties = CloneDict(BookProperties);

            return ret;
        }

        /// <summary>
        /// This method takes a parameter to allow you to set a new guid and use this to make a new object that may or may not be persisted.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public object Clone(uint guid)
        {
            AceObject ret = (AceObject)Clone();
            ret.AceObjectId = guid;
            // We are cloning a new AceObject with a new AceObjectID - need to set this to false. Og II
            ret.HasEverBeenSavedToDatabase = false;
            ret.IsDirty = true;

            ret.PaletteOverrides.ForEach(c => c.AceObjectId = guid);
            ret.TextureOverrides.ForEach(c => c.AceObjectId = guid);
            ret.AnimationOverrides.ForEach(c => c.AceObjectId = guid);
            ret.IntProperties.ForEach(c => c.AceObjectId = guid);
            ret.Int64Properties.ForEach(c => c.AceObjectId = guid);
            ret.DoubleProperties.ForEach(c => c.AceObjectId = guid);
            ret.BoolProperties.ForEach(c => c.AceObjectId = guid);
            ret.DataIdProperties.ForEach(c => c.AceObjectId = guid);
            ret.InstanceIdProperties.ForEach(c => c.AceObjectId = guid);
            ret.StringProperties.ForEach(c => c.AceObjectId = guid);    
            ret.GeneratorLinks.ForEach(c => c.AceObjectId = guid);

            // No need to change Dictionary guids per DDEVEC
            // AceObjectPropertiesAttributes AceObjectPropertiesAttributes2nd AceObjectPropertiesSkills AceObjectPropertiesPositions

            ret.SpellIdProperties.ForEach(c => c.AceObjectId = guid);
            ret.SpellsInSpellBars.ForEach(c => c.AceObjectId = guid);
            ret.BookProperties = CloneDict(BookProperties);
            return ret;
        }

        public void ClearDirtyFlags()
        {
            IsDirty = false;
            HasEverBeenSavedToDatabase = true;

            AceObjectPropertiesAttributes.Values.ToList().ForEach(x => x.ClearDirtyFlags());
            AceObjectPropertiesAttributes2nd.Values.ToList().ForEach(x => x.ClearDirtyFlags());
            AceObjectPropertiesSkills.Values.ToList().ForEach(x => x.ClearDirtyFlags());
            IntProperties.ForEach(x => x.ClearDirtyFlags());
            Int64Properties.ForEach(x => x.ClearDirtyFlags());
            DoubleProperties.ForEach(x => x.ClearDirtyFlags());
            BoolProperties.ForEach(x => x.ClearDirtyFlags());
            DataIdProperties.ForEach(x => x.ClearDirtyFlags());
            InstanceIdProperties.ForEach(x => x.ClearDirtyFlags());
            StringProperties.ForEach(x => x.ClearDirtyFlags());
            BookProperties.Values.ToList().ForEach(x => x.ClearDirtyFlags());
        }

        private static List<T> CloneList<T>(IEnumerable<T> toClone) where T : ICloneable
        {
            return toClone.Select(x => (T)x.Clone()).ToList();
        }

        private static Dictionary<K, V> CloneDict<K, V>(Dictionary<K, V> toClone) where V : ICloneable
        {
            return toClone.ToDictionary(x => x.Key, x => (V)x.Value.Clone());
        }
    }
}
