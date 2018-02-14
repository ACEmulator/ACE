using System;
using System.Collections.Generic;
using System.Linq;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using ACE.Common;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    [DbTable("ace_object")]
    public class AceObject : ICreatureStats, ICloneable, IDirty
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

        [JsonIgnore]
        public bool IsDirty { get; set; }

        /// <summary>
        /// flag to indicate whether or not this instance came from the database
        /// or was created by the game engine.  use case: when calling "SaveObject"
        /// in the database, we need to know whether to insert or update.  There's
        /// really no other way to tell at present.
        /// </summary>
        [JsonIgnore]
        public bool HasEverBeenSavedToDatabase { get; set; }

        /// <summary>
        /// This is a mocked property that will set a flag in the database any time this object is altered.  this flag
        /// will allow us to detect objects that have changed post-installation and generate changesetss
        /// </summary>
        [JsonIgnore]
        [DbField("userModified", (int)MySqlDbType.Bit)]
        public virtual bool UserModified
        {
            get { return true; }
            set { } // method intentionally not implemented
        }

        /// <summary>
        /// Table field Primary Key
        /// </summary>
        [JsonProperty("aceObjectId")]
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true, ListGet = true, ListDelete = true)]
        public virtual uint AceObjectId { get; set; }

        /// <summary>
        /// Table Field Weenie Class
        /// </summary>
        [JsonProperty("weenieClassId")]
        [DbField("weenieClassId", (int)MySqlDbType.UInt32)]
        public uint WeenieClassId { get; set; }

        private uint _aceObjectDescriptionFlags;

        /// <summary>
        /// Table Field Flags
        /// </summary>
        [JsonIgnore]
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

        private uint _physicsDescriptionFlag;

        /// <summary>
        /// Table Field - Flags
        /// </summary>
        [JsonIgnore]
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

        private uint _weenieHeaderFlags;

        /// <summary>
        /// Table Field - Flags
        /// </summary>
        [JsonIgnore]
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

        private uint _weenieHeaderFlags2;

        /// <summary>
        /// Table Field - Flags
        /// </summary>
        [JsonIgnore]
        [DbField("weenieHeaderFlags2", (int)MySqlDbType.UInt32)]
        public uint WeenieHeaderFlags2
        {
            get { return _weenieHeaderFlags2; }
            set
            {
                _weenieHeaderFlags2 = value;
                IsDirty = true;
            }
        }

        private string _currentMotionState;

        [JsonProperty("currentMotionState")]
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

        [JsonIgnore]
        public CreatureAbility StrengthAbility
        {
            get { return GetAttributeProperty(Ability.Strength); }
            set { SetAttributeProperty(Ability.Strength, value); }
        }

        [JsonIgnore]
        public CreatureAbility EnduranceAbility
        {
            get { return GetAttributeProperty(Ability.Endurance); }
            set { SetAttributeProperty(Ability.Endurance, value); }
        }

        [JsonIgnore]
        public CreatureAbility CoordinationAbility
        {
            get { return GetAttributeProperty(Ability.Coordination); }
            set { SetAttributeProperty(Ability.Coordination, value); }
        }

        [JsonIgnore]
        public CreatureAbility QuicknessAbility
        {
            get { return GetAttributeProperty(Ability.Quickness); }
            set { SetAttributeProperty(Ability.Quickness, value); }
        }

        [JsonIgnore]
        public CreatureAbility FocusAbility
        {
            get { return GetAttributeProperty(Ability.Focus); }
            set { SetAttributeProperty(Ability.Focus, value); }
        }

        [JsonIgnore]
        public CreatureAbility SelfAbility
        {
            get { return GetAttributeProperty(Ability.Self); }
            set { SetAttributeProperty(Ability.Self, value); }
        }

        [JsonIgnore]
        public CreatureVital Health
        {
            get { return GetAttribute2ndProperty(Ability.Health); }
            set { SetAttribute2ndProperty(Ability.Health, value); }
        }

        [JsonIgnore]
        public CreatureVital Stamina
        {
            get { return GetAttribute2ndProperty(Ability.Stamina); }
            set { SetAttribute2ndProperty(Ability.Stamina, value); }
        }

        [JsonIgnore]
        public CreatureVital Mana
        {
            get { return GetAttribute2ndProperty(Ability.Mana); }
            set { SetAttribute2ndProperty(Ability.Mana, value); }
        }

        [JsonIgnore]
        public uint Strength { get { return StrengthAbility.MaxValue; } }

        [JsonIgnore]
        public uint Endurance { get { return EnduranceAbility.MaxValue; } }

        [JsonIgnore]
        public uint Coordination { get { return CoordinationAbility.MaxValue; } }

        [JsonIgnore]
        public uint Quickness { get { return QuicknessAbility.MaxValue; } }

        [JsonIgnore]
        public uint Focus { get { return FocusAbility.MaxValue; } }

        [JsonIgnore]
        public uint Self { get { return SelfAbility.MaxValue; } }

        [JsonIgnore]
        public uint? SetupDID
        {
            get { return GetProperty(PropertyDataId.Setup); }
            set { SetProperty(PropertyDataId.Setup, value); }
        }

        [JsonIgnore]
        public uint? MotionTableDID
        {
            get { return GetProperty(PropertyDataId.MotionTable); }
            set { SetProperty(PropertyDataId.MotionTable, value); }
        }

        [JsonIgnore]
        public uint? SoundTableDID
        {
            get { return GetProperty(PropertyDataId.SoundTable); }
            set { SetProperty(PropertyDataId.SoundTable, value); }
        }

        [JsonIgnore]
        public uint? PhysicsEffectTableDID
        {
            get { return GetProperty(PropertyDataId.PhysicsEffectTable); }
            set { SetProperty(PropertyDataId.PhysicsEffectTable, value); }
        }

        [JsonIgnore]
        public uint? CombatTableDID
        {
            get { return GetProperty(PropertyDataId.CombatTable); }
            set { SetProperty(PropertyDataId.CombatTable, value); }
        }

        [JsonIgnore]
        public int? PhysicsState
        {
            get { return GetProperty(PropertyInt.PhysicsState); }
            set { SetProperty(PropertyInt.PhysicsState, value); }
        }

        [JsonIgnore]
        public int? WeenieType
        {
            get { return GetProperty(PropertyInt.WeenieType); }
            set { SetProperty(PropertyInt.WeenieType, value); }
        }

        [JsonIgnore]
        public int? ItemType
        {
            get { return GetProperty(PropertyInt.ItemType); }
            set { SetProperty(PropertyInt.ItemType, value); }
        }

        [JsonIgnore]
        public uint? IconDID
        {
            get { return GetProperty(PropertyDataId.Icon); }
            set { SetProperty(PropertyDataId.Icon, value); }
        }

        [JsonIgnore]
        public string Name
        {
            get { return GetProperty(PropertyString.Name); }
            set { SetProperty(PropertyString.Name, value); }
        }

        [JsonIgnore]
        public string PluralName
        {
            get { return GetProperty(PropertyString.PluralName); }
            set { SetProperty(PropertyString.PluralName, value); }
        }
        [JsonIgnore]

        public byte? ItemsCapacity
        {
            get { return (byte?)GetProperty(PropertyInt.ItemsCapacity); }
            set { SetProperty(PropertyInt.ItemsCapacity, (int)value); }
        }

        [JsonIgnore]
        public byte? ContainersCapacity
        {
            get { return (byte?)GetProperty(PropertyInt.ContainersCapacity); }
            set { SetProperty(PropertyInt.ContainersCapacity, (int)value); }
        }

        [JsonIgnore]
        public int? AmmoType
        {
            get { return GetProperty(PropertyInt.AmmoType); }
            set { SetProperty(PropertyInt.AmmoType, (int)value); }
        }

        [JsonIgnore]
        public int? Value
        {
            get { return GetProperty(PropertyInt.Value); }
            set { SetProperty(PropertyInt.Value, value); }
        }

        [JsonIgnore]
        public int? UseCreateContractId
        {
            get { return GetProperty(PropertyInt.UseCreatesContractId); }
            set { SetProperty(PropertyInt.UseCreatesContractId, value); }
        }

        [JsonIgnore]
        public int? ItemUseable
        {
            get { return GetProperty(PropertyInt.ItemUseable); }
            set { SetProperty(PropertyInt.ItemUseable, (int)value); }
        }

        [JsonIgnore]
        public float? UseRadius
        {
            get { return (float?)GetProperty(PropertyDouble.UseRadius); }
            set { SetProperty(PropertyDouble.UseRadius, value); }
        }

        [JsonIgnore]
        public int? TargetType
        {
            get { return GetProperty(PropertyInt.TargetType); }
            set { SetProperty(PropertyInt.TargetType, value); }
        }

        [JsonIgnore]
        public int? UiEffects
        {
            get { return GetProperty(PropertyInt.UiEffects); }
            set { SetProperty(PropertyInt.UiEffects, value); }
        }

        [JsonIgnore]
        public byte? CombatUse
        {
            get { return (byte?)GetProperty(PropertyInt.CombatUse); }
            set { SetProperty(PropertyInt.CombatUse, value); }
        }

        [JsonIgnore]
        public int? DefaultCombatStyle
        {
            get { return GetProperty(PropertyInt.DefaultCombatStyle); }
            set { SetProperty(PropertyInt.DefaultCombatStyle, value); }
        }

        [JsonIgnore]
        public ushort? Structure
        {
            get { return (ushort?)GetProperty(PropertyInt.Structure); }
            set { SetProperty(PropertyInt.Structure, value); }
        }

        [JsonIgnore]
        public ushort? MaxStructure
        {
            get { return (ushort?)GetProperty(PropertyInt.MaxStructure); }
            set { SetProperty(PropertyInt.MaxStructure, value); }
        }

        [JsonIgnore]
        public ushort? StackSize
        {
            get { return (ushort?)GetProperty(PropertyInt.StackSize); }
            set { SetProperty(PropertyInt.StackSize, value); }
        }

        [JsonIgnore]
        public ushort? MaxStackSize
        {
            get { return (ushort?)GetProperty(PropertyInt.MaxStackSize); }
            set { SetProperty(PropertyInt.MaxStackSize, value); }
        }

        [JsonIgnore]
        public uint? ContainerIID
        {
            get { return GetProperty(PropertyInstanceId.Container); }
            set { SetProperty(PropertyInstanceId.Container, value); }
        }

        [JsonIgnore]
        public int? Placement
        {
            get { return GetProperty(PropertyInt.Placement); }
            set { SetProperty(PropertyInt.Placement, value); }
        }

        [JsonIgnore]
        public uint? WielderIID
        {
            get { return GetProperty(PropertyInstanceId.Wielder); }
            set { SetProperty(PropertyInstanceId.Wielder, value); }
        }

        [JsonIgnore]
        public uint? GeneratorIID
        {
            get { return GetProperty(PropertyInstanceId.Generator); }
            set { SetProperty(PropertyInstanceId.Generator, value); }
        }

        [JsonIgnore]
        public int? ValidLocations
        {
            get { return GetProperty(PropertyInt.ValidLocations); }
            set { SetProperty(PropertyInt.ValidLocations, value); }
        }

        [JsonIgnore]
        public int? CurrentWieldedLocation
        {
            get { return GetProperty(PropertyInt.CurrentWieldedLocation); }
            set { SetProperty(PropertyInt.CurrentWieldedLocation, value); }
        }

        [JsonIgnore]
        public int? ClothingPriority
        {
            get { return GetProperty(PropertyInt.ClothingPriority); }
            set { SetProperty(PropertyInt.ClothingPriority, value); }
        }

        [JsonIgnore]
        public byte? RadarBlipColor
        {
            get { return (byte?)GetProperty(PropertyInt.RadarBlipColor); }
            set { SetProperty(PropertyInt.RadarBlipColor, value); }
        }

        [JsonIgnore]
        public byte? ShowableOnRadar
        {
            get { return (byte?)GetProperty(PropertyInt.ShowableOnRadar); }
            set { SetProperty(PropertyInt.ShowableOnRadar, value); }
        }

        [JsonIgnore]
        public ushort? PhysicsScriptDID
        {
            get { return (ushort?)GetProperty(PropertyDataId.PhysicsScript); }
            set { SetProperty(PropertyDataId.PhysicsScript, value); }
        }

        [JsonIgnore]
        public int? ItemWorkmanship
        {
            get { return GetProperty(PropertyInt.ItemWorkmanship); }
            set { SetProperty(PropertyInt.ItemWorkmanship, value); }
        }

        [JsonIgnore]
        public ushort? EncumbranceVal
        {
            get { return (ushort?)GetProperty(PropertyInt.EncumbranceVal); }
            set { SetProperty(PropertyInt.EncumbranceVal, value); }
        }

        [JsonIgnore]
        public uint? SpellDID
        {
            get { return GetProperty(PropertyDataId.Spell); }
            set { SetProperty(PropertyDataId.Spell, value); }
        }

        [JsonIgnore]
        public ushort? HookType
        {
            get { return (ushort?)GetProperty(PropertyInt.HookType); }
            set { SetProperty(PropertyInt.HookType, value); }
        }

        [JsonIgnore]
        public ushort? HookItemType
        {
            get { return (ushort?)GetProperty(PropertyInt.HookItemType); }
            set { SetProperty(PropertyInt.HookItemType, value); }
        }

        [JsonIgnore]
        public uint? IconOverlayDID
        {
            get { return GetProperty(PropertyDataId.IconOverlay); }
            set { SetProperty(PropertyDataId.IconOverlay, value); }
        }

        [JsonIgnore]
        public uint? IconUnderlayDID
        {
            get { return GetProperty(PropertyDataId.IconUnderlay); }
            set { SetProperty(PropertyDataId.IconUnderlay, value); }
        }

        [JsonIgnore]
        public byte? MaterialType
        {
            get { return (byte?)GetProperty(PropertyInt.MaterialType); }
            set { SetProperty(PropertyInt.MaterialType, (byte?)value); }
        }

        [JsonIgnore]
        public int? SharedCooldown
        {
            get { return GetProperty(PropertyInt.SharedCooldown); }
            set { SetProperty(PropertyInt.SharedCooldown, value); }
        }

        [JsonIgnore]
        public double? CooldownDuration
        {
            get { return GetProperty(PropertyDouble.CooldownDuration); }
            set { SetProperty(PropertyDouble.CooldownDuration, value); }
        }

        // Wielder is Parent, No such thing as PropertyInstanceId.Parent
        [JsonIgnore]
        public uint? ParentIID
        {
            get { return GetProperty(PropertyInstanceId.Wielder); }
            set { SetProperty(PropertyInstanceId.Wielder, value); }
        }

        [JsonIgnore]
        public int? ParentLocation
        {
            get { return GetProperty(PropertyInt.ParentLocation); }
            set { SetProperty(PropertyInt.ParentLocation, value); }
        }

        [JsonIgnore]
        public float? DefaultScale
        {
            get { return (float?)GetProperty(PropertyDouble.DefaultScale); }
            set { SetProperty(PropertyDouble.DefaultScale, value); }
        }

        [JsonIgnore]
        public float? Friction
        {
            get { return (float?)GetProperty(PropertyDouble.Friction); }
            set { SetProperty(PropertyDouble.Friction, value); }
        }

        [JsonIgnore]
        public float? Elasticity
        {
            get { return (float?)GetProperty(PropertyDouble.Elasticity); }
            set { SetProperty(PropertyDouble.Elasticity, value); }
        }

        [JsonIgnore]
        public int? PlacementPosition
        {
            get { return GetProperty(PropertyInt.PlacementPosition); }
            set { SetProperty(PropertyInt.PlacementPosition, value); }
        }

        [JsonIgnore]
        public float? Translucency
        {
            get { return (float?)GetProperty(PropertyDouble.Translucency); }
            set { SetProperty(PropertyDouble.Translucency, value); }
        }
        
        [JsonIgnore]
        public float? PhysicsScriptIntensity
        {
            get { return (float?)GetProperty(PropertyDouble.PhysicsScriptIntensity); }
            set { SetProperty(PropertyDouble.PhysicsScriptIntensity, value); }
        }

        [JsonIgnore]
        public uint? PaletteBaseDID
        {
            get { return GetProperty(PropertyDataId.PaletteBase); }
            set { SetProperty(PropertyDataId.PaletteBase, value); }
        }

        [JsonIgnore]
        public uint? ClothingBaseDID
        {
            get { return GetProperty(PropertyDataId.ClothingBase); }
            set { SetProperty(PropertyDataId.ClothingBase, value); }
        }

        [JsonIgnore]
        public uint? AccountId
        {
            get { return GetProperty(PropertyInstanceId.Account); }
            set { SetProperty(PropertyInstanceId.Account, value); }
        }

        [JsonIgnore]
        public int? Heritage
        {
            get { return GetProperty(PropertyInt.HeritageGroup); }
            set { SetProperty(PropertyInt.HeritageGroup, value); }
        }

        [JsonIgnore]
        public int? Gender
        {
            get { return GetProperty(PropertyInt.Gender); }
            set { SetProperty(PropertyInt.Gender, value); }
        }

        [JsonIgnore]
        public uint? EyesTextureDID
        {
            get { return GetProperty(PropertyDataId.EyesTexture); }
            set { SetProperty(PropertyDataId.EyesTexture, value); }
        }

        [JsonIgnore]
        public uint? DefaultEyesTextureDID
        {
            get { return GetProperty(PropertyDataId.DefaultEyesTexture); }
            set { SetProperty(PropertyDataId.DefaultEyesTexture, value); }
        }

        [JsonIgnore]
        public uint? NoseTextureDID
        {
            get { return GetProperty(PropertyDataId.NoseTexture); }
            set { SetProperty(PropertyDataId.NoseTexture, value); }
        }

        [JsonIgnore]
        public uint? DefaultNoseTextureDID
        {
            get { return GetProperty(PropertyDataId.DefaultNoseTexture); }
            set { SetProperty(PropertyDataId.DefaultNoseTexture, value); }
        }

        [JsonIgnore]
        public uint? MouthTextureDID
        {
            get { return GetProperty(PropertyDataId.MouthTexture); }
            set { SetProperty(PropertyDataId.MouthTexture, value); }
        }

        [JsonIgnore]
        public uint? DefaultMouthTextureDID
        {
            get { return GetProperty(PropertyDataId.DefaultMouthTexture); }
            set { SetProperty(PropertyDataId.DefaultMouthTexture, value); }
        }

        [JsonIgnore]
        public uint? HairTextureDID
        {
            get { return GetProperty(PropertyDataId.HairTexture); }
            set { SetProperty(PropertyDataId.HairTexture, value); }
        }

        [JsonIgnore]
        public uint? DefaultHairTextureDID
        {
            get { return GetProperty(PropertyDataId.DefaultHairTexture); }
            set { SetProperty(PropertyDataId.DefaultHairTexture, value); }
        }

        [JsonIgnore]
        public uint? HeadObjectDID
        {
            get { return GetProperty(PropertyDataId.HeadObject); }
            set { SetProperty(PropertyDataId.HeadObject, value); }
        }

        [JsonIgnore]
        public uint? SkinPaletteDID
        {
            get { return GetProperty(PropertyDataId.SkinPalette); }
            set { SetProperty(PropertyDataId.SkinPalette, value); }
        }

        [JsonIgnore]
        public uint? HairPaletteDID
        {
            get { return GetProperty(PropertyDataId.HairPalette); }
            set { SetProperty(PropertyDataId.HairPalette, value); }
        }

        [JsonIgnore]
        public uint? EyesPaletteDID
        {
            get { return GetProperty(PropertyDataId.EyesPalette); }
            set { SetProperty(PropertyDataId.EyesPalette, value); }
        }

        [JsonIgnore]
        public int? Level
        {
            get { return GetProperty(PropertyInt.Level); }
            set { SetProperty(PropertyInt.Level, value); }
        }

        [JsonIgnore]
        public bool? GeneratorStatus
        {
            get { return GetProperty(PropertyBool.GeneratorStatus); }
            set { SetProperty(PropertyBool.GeneratorStatus, value); }
        }

        [JsonIgnore]
        public bool? GeneratorEnteredWorld
        {
            get { return GetProperty(PropertyBool.GeneratorEnteredWorld); }
            set { SetProperty(PropertyBool.GeneratorEnteredWorld, value); }
        }

        [JsonIgnore]
        public bool? GeneratorDisabled
        {
            get { return GetProperty(PropertyBool.GeneratorDisabled); }
            set { SetProperty(PropertyBool.GeneratorDisabled, value); }
        }

        [JsonIgnore]
        public bool? GeneratedTreasureItem
        {
            get { return GetProperty(PropertyBool.GeneratedTreasureItem); }
            set { SetProperty(PropertyBool.GeneratedTreasureItem, value); }
        }

        [JsonIgnore]
        public bool? GeneratorAutomaticDestruction
        {
            get { return GetProperty(PropertyBool.GeneratorAutomaticDestruction); }
            set { SetProperty(PropertyBool.GeneratorAutomaticDestruction, value); }
        }

        [JsonIgnore]
        public bool? CanGenerateRare
        {
            get { return GetProperty(PropertyBool.CanGenerateRare); }
            set { SetProperty(PropertyBool.CanGenerateRare, value); }
        }

        [JsonIgnore]
        public bool? CorpseGeneratedRare
        {
            get { return GetProperty(PropertyBool.CorpseGeneratedRare); }
            set { SetProperty(PropertyBool.CorpseGeneratedRare, value); }
        }

        [JsonIgnore]
        public bool? SuppressGenerateEffect
        {
            get { return GetProperty(PropertyBool.SuppressGenerateEffect); }
            set { SetProperty(PropertyBool.SuppressGenerateEffect, value); }
        }

        [JsonIgnore]
        public bool? ChestRegenOnClose
        {
            get { return GetProperty(PropertyBool.ChestRegenOnClose); }
            set { SetProperty(PropertyBool.ChestRegenOnClose, value); }
        }

        [JsonIgnore]
        public bool? ChestClearedWhenClosed
        {
            get { return GetProperty(PropertyBool.ChestClearedWhenClosed); }
            set { SetProperty(PropertyBool.ChestClearedWhenClosed, value); }
        }

        [JsonIgnore]
        public int? GeneratorTimeType
        {
            get { return GetProperty(PropertyInt.GeneratorTimeType); }
            set { SetProperty(PropertyInt.GeneratorTimeType, value); }
        }

        [JsonIgnore]
        public int? GeneratorProbability
        {
            get { return GetProperty(PropertyInt.GeneratorProbability); }
            set { SetProperty(PropertyInt.GeneratorProbability, value); }
        }

        [JsonIgnore]
        public int? MaxGeneratedObjects
        {
            get { return GetProperty(PropertyInt.MaxGeneratedObjects); }
            set { SetProperty(PropertyInt.MaxGeneratedObjects, value); }
        }

        [JsonIgnore]
        public int? GeneratorType
        {
            get { return GetProperty(PropertyInt.GeneratorType); }
            set { SetProperty(PropertyInt.GeneratorType, value); }
        }

        [JsonIgnore]
        public int? ActivationCreateClass
        {
            get { return GetProperty(PropertyInt.ActivationCreateClass); }
            set { SetProperty(PropertyInt.ActivationCreateClass, value); }
        }

        [JsonIgnore]
        public bool? Ethereal
        {
            get { return GetProperty(PropertyBool.Ethereal); }
            set { SetProperty(PropertyBool.Ethereal, value); }
        }

        [JsonIgnore]
        public bool? Open
        {
            get { return GetProperty(PropertyBool.Open); }
            set { SetProperty(PropertyBool.Open, value); }
        }

        [JsonIgnore]
        public bool? Locked
        {
            get { return GetProperty(PropertyBool.Locked); }
            set { SetProperty(PropertyBool.Locked, value); }
        }

        [JsonIgnore]
        public bool? DefaultLocked
        {
            get { return GetProperty(PropertyBool.DefaultLocked); }
            set { SetProperty(PropertyBool.DefaultLocked, value); }
        }

        [JsonIgnore]
        public bool? DefaultOpen
        {
            get { return GetProperty(PropertyBool.DefaultOpen); }
            set { SetProperty(PropertyBool.DefaultOpen, value); }
        }

        [JsonIgnore]
        public float? ResetInterval
        {
            get { return (float?)GetProperty(PropertyDouble.ResetInterval); }
            set { SetProperty(PropertyDouble.ResetInterval, value); }
        }

        [JsonIgnore]
        public double? ResetTimestamp
        {
            get { return GetProperty(PropertyDouble.ResetTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.ResetTimestamp); }
        }

        [JsonIgnore]
        public double? UseTimestamp
        {
            get { return GetProperty(PropertyDouble.UseTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.UseTimestamp); }
        }

        [JsonIgnore]
        public double? UseLockTimestamp
        {
            get { return GetProperty(PropertyDouble.UseLockTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.UseLockTimestamp); }
        }

        [JsonIgnore]
        public uint? LastUnlockerIID
        {
            get { return GetProperty(PropertyInstanceId.LastUnlocker); }
            set { SetProperty(PropertyInstanceId.LastUnlocker, value); }
        }

        [JsonIgnore]
        public string KeyCode
        {
            get { return GetProperty(PropertyString.KeyCode); }
            set { SetProperty(PropertyString.KeyCode, value); }
        }

        [JsonIgnore]
        public string LockCode
        {
            get { return GetProperty(PropertyString.LockCode); }
            set { SetProperty(PropertyString.LockCode, value); }
        }

        [JsonIgnore]
        public int? ResistLockpick
        {
            get { return GetProperty(PropertyInt.ResistLockpick); }
            set { SetProperty(PropertyInt.ResistLockpick, value); }
        }

        [JsonIgnore]
        public int? AppraisalLockpickSuccessPercent
        {
            get { return GetProperty(PropertyInt.AppraisalLockpickSuccessPercent); }
            set { SetProperty(PropertyInt.AppraisalLockpickSuccessPercent, value); }
        }

        [JsonIgnore]
        public int? MinLevel
        {
            get { return GetProperty(PropertyInt.MinLevel); }
            set { SetProperty(PropertyInt.MinLevel, value); }
        }

        [JsonIgnore]
        public int? MaxLevel
        {
            get { return GetProperty(PropertyInt.MaxLevel); }
            set { SetProperty(PropertyInt.MaxLevel, value); }
        }

        [JsonIgnore]
        public int? PortalBitmask
        {
            get { return GetProperty(PropertyInt.PortalBitmask); }
            set { SetProperty(PropertyInt.PortalBitmask, value); }
        }

        [JsonIgnore]
        public string AppraisalPortalDestination
        {
            get { return GetProperty(PropertyString.AppraisalPortalDestination); }
            set { SetProperty(PropertyString.AppraisalPortalDestination, value); }
        }

        [JsonIgnore]
        public string ShortDesc
        {
            get { return GetProperty(PropertyString.ShortDesc); }
            set { SetProperty(PropertyString.ShortDesc, value); }
        }

        [JsonIgnore]
        public string LongDesc
        {
            get { return GetProperty(PropertyString.LongDesc); }
            set { SetProperty(PropertyString.LongDesc, value); }
        }

        [JsonIgnore]
        public string Use
        {
            get { return GetProperty(PropertyString.Use); }
            set { SetProperty(PropertyString.Use, value); }
        }

        [JsonIgnore]
        public string UseMessage
        {
            get { return GetProperty(PropertyString.UseMessage); }
            set { SetProperty(PropertyString.UseMessage, value); }
        }

        [JsonIgnore]
        public bool? PortalShowDestination
        {
            get { return GetProperty(PropertyBool.PortalShowDestination); }
            set { SetProperty(PropertyBool.PortalShowDestination, value); }
        }

        [JsonIgnore]
        public string HeritageGroup
        {
            get { return GetProperty(PropertyString.HeritageGroup); }
            set { SetProperty(PropertyString.HeritageGroup, value); }
        }

        [JsonIgnore]
        public string Sex
        {
            get { return GetProperty(PropertyString.Sex); }
            set { SetProperty(PropertyString.Sex, value); }
        }

        [JsonIgnore]
        public string Title
        {
            get { return GetProperty(PropertyString.Title); }
            set { SetProperty(PropertyString.Title, value); }
        }

        [JsonIgnore]
        public string Template
        {
            get { return GetProperty(PropertyString.Template); }
            set { SetProperty(PropertyString.Template, value); }
        }

        [JsonIgnore]
        public string DisplayName
        {
            get { return GetProperty(PropertyString.DisplayName); }
            set { SetProperty(PropertyString.DisplayName, value); }
        }

        [JsonIgnore]
        public int? CharacterTitleId
        {
            get { return GetProperty(PropertyInt.CharacterTitleId); }
            set { SetProperty(PropertyInt.CharacterTitleId, value); }
        }

        [JsonIgnore]
        public int? NumCharacterTitles
        {
            get { return GetProperty(PropertyInt.NumCharacterTitles); }
            set { SetProperty(PropertyInt.NumCharacterTitles, value); }
        }

        [JsonIgnore]
        public double? CreationTimestamp
        {
            get { return GetProperty(PropertyDouble.CreationTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.CreationTimestamp); }
        }

        [JsonIgnore]
        public bool? Stuck
        {
            get { return GetProperty(PropertyBool.Stuck); }
            set { SetProperty(PropertyBool.Stuck, value); }
        }

        [JsonIgnore]
        public bool? IgnoreCollisions
        {
            get { return GetProperty(PropertyBool.IgnoreCollisions); }
            set { SetProperty(PropertyBool.IgnoreCollisions, value); }
        }

        [JsonIgnore]
        public bool? ReportCollisions
        {
            get { return GetProperty(PropertyBool.ReportCollisions); }
            set { SetProperty(PropertyBool.ReportCollisions, value); }
        }

        [JsonIgnore]
        public bool? GravityStatus
        {
            get { return GetProperty(PropertyBool.GravityStatus); }
            set { SetProperty(PropertyBool.GravityStatus, value); }
        }

        [JsonIgnore]
        public bool? LightsStatus
        {
            get { return GetProperty(PropertyBool.LightsStatus); }
            set { SetProperty(PropertyBool.LightsStatus, value); }
        }

        [JsonIgnore]
        public bool? ScriptedCollision
        {
            get { return GetProperty(PropertyBool.ScriptedCollision); }
            set { SetProperty(PropertyBool.ScriptedCollision, value); }
        }

        [JsonIgnore]
        public bool? Inelastic
        {
            get { return GetProperty(PropertyBool.Inelastic); }
            set { SetProperty(PropertyBool.Inelastic, value); }
        }

        [JsonIgnore]
        public bool? Visibility
        {
            get { return GetProperty(PropertyBool.Visibility); }
            set { SetProperty(PropertyBool.Visibility, value); }
        }

        [JsonIgnore]
        public bool? Attackable
        {
            get { return GetProperty(PropertyBool.Attackable); }
            set { SetProperty(PropertyBool.Attackable, value); }
        }

        [JsonIgnore]
        public bool? AdvocateState
        {
            get { return GetProperty(PropertyBool.AdvocateState); }
            set { SetProperty(PropertyBool.AdvocateState, value); }
        }

        [JsonIgnore]
        public bool? Inscribable
        {
            get { return GetProperty(PropertyBool.Inscribable); }
            set { SetProperty(PropertyBool.Inscribable, value); }
        }

        [JsonIgnore]
        public bool? UiHidden
        {
            get { return GetProperty(PropertyBool.UiHidden); }
            set { SetProperty(PropertyBool.UiHidden, value); }
        }

        [JsonIgnore]
        public bool? IgnoreHouseBarriers
        {
            get { return GetProperty(PropertyBool.IgnoreHouseBarriers); }
            set { SetProperty(PropertyBool.IgnoreHouseBarriers, value); }
        }

        [JsonIgnore]
        public bool? HiddenAdmin
        {
            get { return GetProperty(PropertyBool.HiddenAdmin); }
            set { SetProperty(PropertyBool.HiddenAdmin, value); }
        }

        [JsonIgnore]
        public bool? PkWounder
        {
            get { return GetProperty(PropertyBool.PkWounder); }
            set { SetProperty(PropertyBool.PkWounder, value); }
        }

        [JsonIgnore]
        public bool? PkKiller
        {
            get { return GetProperty(PropertyBool.PkKiller); }
            set { SetProperty(PropertyBool.PkKiller, value); }
        }

        [JsonIgnore]
        public bool? UnderLifestoneProtection
        {
            get { return GetProperty(PropertyBool.UnderLifestoneProtection); }
            set { SetProperty(PropertyBool.UnderLifestoneProtection, value); }
        }

        [JsonIgnore]
        public bool? DefaultOn
        {
            get { return GetProperty(PropertyBool.DefaultOn); }
            set { SetProperty(PropertyBool.DefaultOn, value); }
        }

        [JsonIgnore]
        public bool? IsFrozen
        {
            get { return GetProperty(PropertyBool.IsFrozen); }
            set { SetProperty(PropertyBool.IsFrozen, value); }
        }

        [JsonIgnore]
        public bool? ReportCollisionsAsEnvironment
        {
            get { return GetProperty(PropertyBool.ReportCollisionsAsEnvironment); }
            set { SetProperty(PropertyBool.ReportCollisionsAsEnvironment, value); }
        }

        [JsonIgnore]
        public bool? AllowEdgeSlide
        {
            get { return GetProperty(PropertyBool.AllowEdgeSlide); }
            set { SetProperty(PropertyBool.AllowEdgeSlide, value); }
        }

        [JsonIgnore]
        public bool? AdvocateQuest
        {
            get { return GetProperty(PropertyBool.AdvocateQuest); }
            set { SetProperty(PropertyBool.AdvocateQuest, value); }
        }

        [JsonIgnore]
        public bool? IsAdvocate
        {
            get { return GetProperty(PropertyBool.IsAdvocate); }
            set { SetProperty(PropertyBool.IsAdvocate, value); }
        }

        [JsonIgnore]
        public bool? IsSentinel
        {
            get { return GetProperty(PropertyBool.IsSentinel); }
            set { SetProperty(PropertyBool.IsSentinel, value); }
        }

        [JsonIgnore]
        public bool? NoDraw
        {
            get { return GetProperty(PropertyBool.NoDraw); }
            set { SetProperty(PropertyBool.NoDraw, value); }
        }

        [JsonIgnore]
        public bool? IgnorePortalRestrictions
        {
            get { return GetProperty(PropertyBool.IgnorePortalRestrictions); }
            set { SetProperty(PropertyBool.IgnorePortalRestrictions, value); }
        }

        [JsonIgnore]
        public bool? Retained
        {
            get { return GetProperty(PropertyBool.Retained); }
            set { SetProperty(PropertyBool.Retained, value); }
        }

        [JsonIgnore]
        public bool? Invincible
        {
            get { return GetProperty(PropertyBool.Invincible); }
            set { SetProperty(PropertyBool.Invincible, value); }
        }

        [JsonIgnore]
        public bool? IsGagged
        {
            get { return GetProperty(PropertyBool.IsGagged); }
            set { SetProperty(PropertyBool.IsGagged, value); }
        }

        [JsonIgnore]
        public bool? Afk
        {
            get { return GetProperty(PropertyBool.Afk); }
            set { SetProperty(PropertyBool.Afk, value); }
        }

        [JsonIgnore]
        public bool? IgnoreAuthor
        {
            get { return GetProperty(PropertyBool.IgnoreAuthor); }
            set { SetProperty(PropertyBool.IgnoreAuthor, value); }
        }

        [JsonIgnore]
        public bool? WieldOnUse
        {
            get { return GetProperty(PropertyBool.WieldOnUse); }
            set { SetProperty(PropertyBool.WieldOnUse, value); }
        }

        [JsonIgnore]
        public bool? AutowieldLeft
        {
            get { return GetProperty(PropertyBool.AutowieldLeft); }
            set { SetProperty(PropertyBool.AutowieldLeft, value); }
        }

        [JsonIgnore]
        public bool? VendorService
        {
            get { return GetProperty(PropertyBool.VendorService); }
            set { SetProperty(PropertyBool.VendorService, value); }
        }

        [JsonIgnore]
        public bool? RequiresBackpackSlot
        {
            get { return GetProperty(PropertyBool.RequiresBackpackSlot); }
            set { SetProperty(PropertyBool.RequiresBackpackSlot, value); }
        }

        [JsonIgnore]
        public bool UseBackpackSlot
        {
            get { return (GetProperty(PropertyBool.RequiresBackpackSlot) ?? false) ||
                          GetProperty(PropertyInt.WeenieType) == (uint)Enum.WeenieType.Container; }
        }

        [JsonIgnore]
        public int? ItemCurMana
        {
            get { return GetProperty(PropertyInt.ItemCurMana); }
            set { SetProperty(PropertyInt.ItemCurMana, value); }
        }

        [JsonIgnore]
        public int? ItemMaxMana
        {
            get { return GetProperty(PropertyInt.ItemMaxMana); }
            set { SetProperty(PropertyInt.ItemMaxMana, value); }
        }

        [JsonIgnore]
        public bool? NpcLooksLikeObject
        {
            get { return GetProperty(PropertyBool.NpcLooksLikeObject); }
            set { SetProperty(PropertyBool.NpcLooksLikeObject, value); }
        }

        [JsonIgnore]
        public uint? AllowedActivator
        {
            get { return GetProperty(PropertyInstanceId.AllowedActivator); }
            set { SetProperty(PropertyInstanceId.AllowedActivator, value); }
        }

        [JsonIgnore]
        public int? CreatureType
        {
            get { return GetProperty(PropertyInt.CreatureType); }
            set { SetProperty(PropertyInt.CreatureType, value); }
        }

        [JsonIgnore]
        public int? MerchandiseItemTypes
        {
            get { return GetProperty(PropertyInt.MerchandiseItemTypes); }
            set { SetProperty(PropertyInt.MerchandiseItemTypes, value); }
        }

        [JsonIgnore]
        public int? MerchandiseMinValue
        {
            get { return GetProperty(PropertyInt.MerchandiseMinValue); }
            set { SetProperty(PropertyInt.MerchandiseMinValue, value); }
        }

        [JsonIgnore]
        public int? MerchandiseMaxValue
        {
            get { return GetProperty(PropertyInt.MerchandiseMaxValue); }
            set { SetProperty(PropertyInt.MerchandiseMaxValue, value); }
        }

        [JsonIgnore]
        public string Inscription
        {
            get { return GetProperty(PropertyString.Inscription); }
            set { SetProperty(PropertyString.Inscription, value); }
        }

        #region Books

        [JsonIgnore]
        public string ScribeName
        {
            get { return GetProperty(PropertyString.ScribeName); }
            set { SetProperty(PropertyString.ScribeName, value); }
        }

        [JsonIgnore]
        public string ScribeAccount
        {
            get { return GetProperty(PropertyString.ScribeAccount); }
            set { SetProperty(PropertyString.ScribeAccount, value); }
        }

        [JsonIgnore]
        public uint? ScribeIID
        {
            get { return GetProperty(PropertyInstanceId.Scribe); }
            set { SetProperty(PropertyInstanceId.Scribe, value); }
        }

        [JsonIgnore]
        public int? AppraisalPages
        {
            get { return GetProperty(PropertyInt.AppraisalPages); }
            set { SetProperty(PropertyInt.AppraisalPages, value); }
        }

        [JsonIgnore]
        public int? AppraisalMaxPages
        {
            get { return GetProperty(PropertyInt.AppraisalMaxPages); }
            set { SetProperty(PropertyInt.AppraisalMaxPages, value); }
        }
        #endregion

        // TODO: This might be wrong place to store the data being stored here.
        [JsonIgnore]
        public int? AvailableCharacter
        {
            get { return GetProperty(PropertyInt.AvailableCharacter); }
            set { SetProperty(PropertyInt.AvailableCharacter, value); }
        }

        [JsonIgnore]
        public int? Boost
        {
            get { return GetProperty(PropertyInt.BoostValue); }
            set { SetProperty(PropertyInt.BoostValue, value); }
        }

        [JsonIgnore]
        public int? BoostEnum
        {
            get { return GetProperty(PropertyInt.BoosterEnum); }
            set { SetProperty(PropertyInt.BoosterEnum, value); }
        }

        [JsonIgnore]
        public double? HealkitMod
        {
            get { return GetProperty(PropertyDouble.HealkitMod); }
            set { SetProperty(PropertyDouble.HealkitMod, value); }
        }

        [JsonIgnore]
        public int? CoinValue
        {
            get { return GetProperty(PropertyInt.CoinValue); }
            set { SetProperty(PropertyInt.CoinValue, value); }
        }

        #region Positions

        [JsonIgnore]
        public Position Location
        {
            get { return GetPosition(PositionType.Location); }
            set { SetPosition(PositionType.Location, value); }
        }

        [JsonIgnore]
        public Position Destination
        {
            get { return GetPosition(PositionType.Destination); }
            set { SetPosition(PositionType.Destination, value); }
        }

        [JsonIgnore]
        public Position Instantiation
        {
            get { return GetPosition(PositionType.Instantiation); }
            set { SetPosition(PositionType.Instantiation, value); }
        }

        [JsonIgnore]
        public Position Sanctuary
        {
            get { return GetPosition(PositionType.Sanctuary); }
            set { SetPosition(PositionType.Sanctuary, value); }
        }

        [JsonIgnore]
        public Position Home
        {
            get { return GetPosition(PositionType.Home); }
            set { SetPosition(PositionType.Home, value); }
        }

        [JsonIgnore]
        public Position ActivationMove
        {
            get { return GetPosition(PositionType.ActivationMove); }
            set { SetPosition(PositionType.ActivationMove, value); }
        }

        [JsonIgnore]
        public Position Target
        {
            get { return GetPosition(PositionType.Target); }
            set { SetPosition(PositionType.Target, value); }
        }

        [JsonIgnore]
        public Position LinkedPortalOne
        {
            get { return GetPosition(PositionType.LinkedPortalOne); }
            set { SetPosition(PositionType.LinkedPortalOne, value); }
        }

        [JsonIgnore]
        public Position LastPortal
        {
            get { return GetPosition(PositionType.LastPortal); }
            set { SetPosition(PositionType.LastPortal, value); }
        }

        [JsonIgnore]
        public Position PortalStorm
        {
            get { return GetPosition(PositionType.PortalStorm); }
            set { SetPosition(PositionType.PortalStorm, value); }
        }

        [JsonIgnore]
        public Position CrashAndTurn
        {
            get { return GetPosition(PositionType.CrashAndTurn); }
            set { SetPosition(PositionType.CrashAndTurn, value); }
        }

        [JsonIgnore]
        public Position PortalSummonLoc
        {
            get { return GetPosition(PositionType.PortalSummonLoc); }
            set { SetPosition(PositionType.PortalSummonLoc, value); }
        }

        [JsonIgnore]
        public Position HouseBoot
        {
            get { return GetPosition(PositionType.HouseBoot); }
            set { SetPosition(PositionType.HouseBoot, value); }
        }

        [JsonIgnore]
        public Position LastOutsideDeath
        {
            get { return GetPosition(PositionType.LastOutsideDeath); }
            set { SetPosition(PositionType.LastOutsideDeath, value); }
        }

        [JsonIgnore]
        public Position LinkedLifestone
        {
            get { return GetPosition(PositionType.LinkedLifestone); }
            set { SetPosition(PositionType.LinkedLifestone, value); }
        }

        [JsonIgnore]
        public Position LinkedPortalTwo
        {
            get { return GetPosition(PositionType.LinkedPortalTwo); }
            set { SetPosition(PositionType.LinkedPortalTwo, value); }
        }

        [JsonIgnore]
        public Position Save1
        {
            get { return GetPosition(PositionType.Save1); }
            set { SetPosition(PositionType.Save1, value); }
        }

        [JsonIgnore]
        public Position Save2
        {
            get { return GetPosition(PositionType.Save2); }
            set { SetPosition(PositionType.Save2, value); }
        }

        [JsonIgnore]
        public Position Save3
        {
            get { return GetPosition(PositionType.Save3); }
            set { SetPosition(PositionType.Save3, value); }
        }

        [JsonIgnore]
        public Position Save4
        {
            get { return GetPosition(PositionType.Save4); }
            set { SetPosition(PositionType.Save4, value); }
        }

        [JsonIgnore]
        public Position Save5
        {
            get { return GetPosition(PositionType.Save5); }
            set { SetPosition(PositionType.Save5, value); }
        }

        [JsonIgnore]
        public Position Save6
        {
            get { return GetPosition(PositionType.Save6); }
            set { SetPosition(PositionType.Save6, value); }
        }

        [JsonIgnore]
        public Position Save7
        {
            get { return GetPosition(PositionType.Save7); }
            set { SetPosition(PositionType.Save7, value); }
        }

        [JsonIgnore]
        public Position Save8
        {
            get { return GetPosition(PositionType.Save8); }
            set { SetPosition(PositionType.Save8, value); }
        }

        [JsonIgnore]
        public Position Save9
        {
            get { return GetPosition(PositionType.Save9); }
            set { SetPosition(PositionType.Save9, value); }
        }

        [JsonIgnore]
        public Position RelativeDestination
        {
            get { return GetPosition(PositionType.RelativeDestination); }
            set { SetPosition(PositionType.RelativeDestination, value); }
        }

        [JsonIgnore]
        public Position TeleportedCharacter
        {
            get { return GetPosition(PositionType.TeleportedCharacter); }
            set { SetPosition(PositionType.TeleportedCharacter, value); }
        }
        #endregion

        [JsonIgnore]
        public double? BuyPrice
        {
            get { return GetProperty(PropertyDouble.BuyPrice); }
            set { SetProperty(PropertyDouble.BuyPrice, value); }
        }

        [JsonIgnore]
        public double? SellPrice
        {
            get { return GetProperty(PropertyDouble.SellPrice); }
            set { SetProperty(PropertyDouble.SellPrice, value); }
        }

        [JsonIgnore]
        public bool? DealMagicalItems
        {
            get { return GetProperty(PropertyBool.DealMagicalItems); }
            set { SetProperty(PropertyBool.DealMagicalItems, value); }
        }

        [JsonIgnore]
        public uint? AlternateCurrencyDID
        {
            get { return GetProperty(PropertyDataId.AlternateCurrency); }
            set { SetProperty(PropertyDataId.AlternateCurrency, value); }
        }

        [JsonIgnore]
        public double? HeartbeatInterval
        {
            get { return GetProperty(PropertyDouble.HeartbeatInterval); }
            set { SetProperty(PropertyDouble.HeartbeatInterval, value); }
        }

        [JsonIgnore]
        public int? InitGeneratedObjects
        {
            get { return GetProperty(PropertyInt.InitGeneratedObjects); }
            set { SetProperty(PropertyInt.InitGeneratedObjects, value); }
        }

        [JsonIgnore]
        public double? RegenerationInterval
        {
            get { return GetProperty(PropertyDouble.RegenerationInterval); }
            set { SetProperty(PropertyDouble.RegenerationInterval, value); }
        }

        [JsonIgnore]
        public int? PaletteTemplate
        {
            get { return GetProperty(PropertyInt.PaletteTemplate); }
            set { SetProperty(PropertyInt.PaletteTemplate, value); }
        }

        [JsonIgnore]
        public double? Shade
        {
            get { return GetProperty(PropertyDouble.Shade); }
            set { SetProperty(PropertyDouble.Shade, value); }
        }

        #region Chess
        [JsonIgnore]
        public int? ChessGamesLost
        {
            get { return GetProperty(PropertyInt.ChessGamesLost); }
            set { SetProperty(PropertyInt.ChessGamesLost, value); }
        }

        [JsonIgnore]
        public int? ChessGamesWon
        {
            get { return GetProperty(PropertyInt.ChessGamesWon); }
            set { SetProperty(PropertyInt.ChessGamesWon, value); }
        }

        [JsonIgnore]
        public int? ChessRank
        {
            get { return GetProperty(PropertyInt.ChessRank); }
            set { SetProperty(PropertyInt.ChessRank, value); }
        }

        [JsonIgnore]
        public int? ChessTotalGames
        {
            get { return GetProperty(PropertyInt.ChessTotalGames); }
            set { SetProperty(PropertyInt.ChessTotalGames, value); }
        }
        #endregion

        protected uint? GetProperty(PropertyDataId property)
        {
            return DataIdProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<uint> GetDataIdProperties(PropertyDataId property)
        {
            return DataIdProperties.Where(x => x.PropertyId == (uint)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetProperty(PropertyDataId didPropertyId, uint? value)
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

        protected bool? GetProperty(PropertyBool property)
        {
            return BoolProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<bool> GetBoolProperties(PropertyBool property)
        {
            return BoolProperties.Where(x => x.PropertyId == (uint)property).Where(b => b.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetProperty(PropertyBool propertyId, bool? value)
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

        protected uint? GetProperty(PropertyInstanceId property)
        {
            return InstanceIdProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<uint> GetInstanceIdProperties(PropertyInstanceId property)
        {
            return InstanceIdProperties.Where(x => x.PropertyId == (uint)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetProperty(PropertyInstanceId iidPropertyId, uint? value)
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
            bool success = AceObjectPropertiesAttributes.TryGetValue(ability, out var ret);

            if (!success || ret == null)
            {
                ret = new CreatureAbility(ability);
                AceObjectPropertiesAttributes.Add(ability, ret);
            }

            return ret;
        }

        private void SetProperty<K, V>(Dictionary<K, V> dict, K key, V value)
        {
            // FIXME: It seems like every set gets called twice.
            // It is allowing us to save a key value pair with a null value Og II
            if (dict.ContainsKey(key))
            {
                if (value != null)
                    dict[key] = value;
                else
                    dict.Remove(key);
            }
            else
            {
                if (value != null)
                    dict.Add(key, value);
            }
        }

        protected void SetAttributeProperty(Ability ability, CreatureAbility value)
        {
            SetProperty(AceObjectPropertiesAttributes, ability, value);
        }

        protected CreatureVital GetAttribute2ndProperty(Ability ability)
        {
            bool success = AceObjectPropertiesAttributes2nd.TryGetValue(ability, out var ret);

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

        public CreatureSkill GetSkillProperty(Skill skill)
        {
            bool success = AceObjectPropertiesSkills.TryGetValue(skill, out var ret);

            if (!success || ret == null)
            {
                ret = new CreatureSkill(this, skill, SkillStatus.Untrained, 0, 0);
                AceObjectPropertiesSkills.Add(skill, ret);
            }

            return ret;
        }

        protected void SetProperty(Skill skill, CreatureSkill value)
        {
            SetProperty(AceObjectPropertiesSkills, skill, value);
        }

        public List<CreatureSkill> GetSkills()
        {
            return AceObjectPropertiesSkills.Values.ToList();
        }

        public int? GetProperty(PropertyInt property)
        {
            return IntProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<int> GetIntProperties(PropertyInt property)
        {
            return IntProperties.Where(x => x.PropertyId == (uint)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetProperty(PropertyInt intPropertyId, int? value)
        {
            AceObjectPropertiesInt listItem = IntProperties.Find(x => x.PropertyId == (ushort)intPropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesInt { PropertyId = (uint)intPropertyId, PropertyValue = (int)value, AceObjectId = AceObjectId };
                    IntProperties.Add(listItem);
                }
                else
                {
                    listItem.PropertyValue = (int)value;
                }
            }
            else
            {
                if (listItem != null)
                    listItem.PropertyValue = null;
            }
        }

        public ulong? GetProperty(PropertyInt64 property)
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

        public double? GetProperty(PropertyDouble property)
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
            SetProperty(propertyId, timestamp);
        }

        protected void SetProperty(PropertyDouble propertyId, double? value)
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

        public string GetProperty(PropertyString property)
        {
            return StringProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<string> GetStringProperties(PropertyString property)
        {
            return StringProperties.Where(x => x.PropertyId == (uint)property).Select(x => x.PropertyValue).ToList();
        }

        protected void SetProperty(PropertyString propertyId, string value)
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

        [JsonProperty("palettes")]
        public List<PaletteOverride> PaletteOverrides { get; set; } = new List<PaletteOverride>();

        [JsonProperty("textures")]
        public List<TextureMapOverride> TextureOverrides { get; set; } = new List<TextureMapOverride>();

        [JsonProperty("animations")]
        public List<AnimationOverride> AnimationOverrides { get; set; } = new List<AnimationOverride>();

        [JsonProperty("uintProperties")]
        public List<AceObjectPropertiesInt> IntProperties { get; set; } = new List<AceObjectPropertiesInt>();

        [JsonProperty("uint64Properties")]
        public List<AceObjectPropertiesInt64> Int64Properties { get; set; } = new List<AceObjectPropertiesInt64>();

        [JsonProperty("doubleProperties")]
        public List<AceObjectPropertiesDouble> DoubleProperties { get; set; } = new List<AceObjectPropertiesDouble>();

        [JsonProperty("boolProperties")]
        public List<AceObjectPropertiesBool> BoolProperties { get; set; } = new List<AceObjectPropertiesBool>();

        [JsonProperty("didProperties")]
        public List<AceObjectPropertiesDataId> DataIdProperties { get; set; } = new List<AceObjectPropertiesDataId>();

        [JsonProperty("iidProperties")]
        public List<AceObjectPropertiesInstanceId> InstanceIdProperties { get; set; } = new List<AceObjectPropertiesInstanceId>();

        [JsonProperty("spells")]
        public List<AceObjectPropertiesSpell> SpellIdProperties { get; set; } = new List<AceObjectPropertiesSpell>();

        [JsonProperty("spellbars")]
        public List<AceObjectPropertiesSpellBarPositions> SpellsInSpellBars { get; set; } = new List<AceObjectPropertiesSpellBarPositions>();

        [JsonIgnore]
        public Dictionary<ObjectGuid, AceObject> Inventory = new Dictionary<ObjectGuid, AceObject>();

        [JsonProperty("inventoryWeenieIds")]
        public List<uint> InventoryWeenieIds { get { return Inventory.Values.Select(a => a.WeenieClassId).ToList(); } }
        
        [JsonIgnore]
        public Dictionary<ObjectGuid, AceObject> WieldedItems = new Dictionary<ObjectGuid, AceObject>();

        [JsonProperty("wieldedWeenieIds")]
        public List<uint> WieldedWeenieIds { get { return WieldedItems.Values.Select(a => a.WeenieClassId).ToList(); } }

        [JsonProperty("contracts")]
        public Dictionary<uint, AceContractTracker> TrackedContracts = new Dictionary<uint, AceContractTracker>();

        [JsonProperty("stringProperties")]
        public List<AceObjectPropertiesString> StringProperties { get; set; } = new List<AceObjectPropertiesString>();

        // uint references the page
        [JsonProperty("bookProperties")]
        public Dictionary<uint, AceObjectPropertiesBook> BookProperties { get; set; } = new Dictionary<uint, AceObjectPropertiesBook>();

        [JsonProperty("generatorProfiles")]
        public List<AceObjectGeneratorProfile> GeneratorProfiles { get; set; } = new List<AceObjectGeneratorProfile>();

        [JsonProperty("abilities")]
        public Dictionary<Ability, CreatureAbility> AceObjectPropertiesAttributes { get; set; } = new Dictionary<Ability, CreatureAbility>();

        // ReSharper disable once InconsistentNaming
        [JsonProperty("vitals")]
        public Dictionary<Ability, CreatureVital> AceObjectPropertiesAttributes2nd { get; set; } = new Dictionary<Ability, CreatureVital>();

        [JsonProperty("skills")]
        public Dictionary<Skill, CreatureSkill> AceObjectPropertiesSkills { get; set; } = new Dictionary<Skill, CreatureSkill>();

        [JsonProperty("positions")]
        public Dictionary<PositionType, Position> AceObjectPropertiesPositions { get; set; } = new Dictionary<PositionType, Position>();

        protected Position GetPosition(PositionType positionType)
        {
            bool success = AceObjectPropertiesPositions.TryGetValue(positionType, out var ret);

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

        public void SetTrackedContract(uint contractId, AceContractTracker value)
        {
            SetProperty(TrackedContracts, contractId, value);
        }

        public AceContractTracker GetTrackedContract(uint contractId)
        {
            bool success = TrackedContracts.TryGetValue(contractId, out var ret);
            return !success ? null : ret;
        }

        [JsonProperty("createlist")]
        public List<AceObjectInventory> CreateList { get; set; } = new List<AceObjectInventory>();

        public object Clone()
        {
            AceObject ret = new AceObject
            {
                AceObjectId = AceObjectId,
                WeenieClassId = WeenieClassId,
                AceObjectDescriptionFlags = AceObjectDescriptionFlags,
                PhysicsDescriptionFlag = PhysicsDescriptionFlag,
                WeenieHeaderFlags = WeenieHeaderFlags,
                HasEverBeenSavedToDatabase = HasEverBeenSavedToDatabase,
                PaletteOverrides = CloneList(PaletteOverrides),
                TextureOverrides = CloneList(TextureOverrides),
                AnimationOverrides = CloneList(AnimationOverrides),
                IntProperties = CloneList(IntProperties),
                Int64Properties = CloneList(Int64Properties),
                DoubleProperties = CloneList(DoubleProperties),
                BoolProperties = CloneList(BoolProperties),
                DataIdProperties = CloneList(DataIdProperties),
                InstanceIdProperties = CloneList(InstanceIdProperties),
                StringProperties = CloneList(StringProperties),
                GeneratorProfiles = CloneList(GeneratorProfiles),
                CreateList = CloneList(CreateList),
                AceObjectPropertiesAttributes = CloneDict(AceObjectPropertiesAttributes),
                AceObjectPropertiesAttributes2nd = CloneDict(AceObjectPropertiesAttributes2nd),
                AceObjectPropertiesSkills = CloneDict(AceObjectPropertiesSkills),
                AceObjectPropertiesPositions = CloneDict(AceObjectPropertiesPositions),
                SpellIdProperties = CloneList(SpellIdProperties),
                SpellsInSpellBars = CloneList(SpellsInSpellBars),
                BookProperties = CloneDict(BookProperties),
                Inventory = CloneDict(Inventory),
                WieldedItems = CloneDict(WieldedItems),
                TrackedContracts = CloneDict(TrackedContracts)
            };
            return ret;
        }

        /// <summary>
        /// This method takes a parameter to allow you to set a new guid and use this to make a new object that may or may not be persisted.
        /// </summary>
        public object Clone(uint guid)
        {
            AceObject ret = (AceObject)Clone();
            ret.AceObjectId = guid;
            // We are cloning a new AceObject with a new AceObjectID - need to set this to false. Og II
            ret.SetDirtyFlags();

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
            ret.SpellIdProperties.ForEach(c => c.AceObjectId = guid);
            ret.GeneratorProfiles.ForEach(c => c.AceObjectId = guid);
            ret.CreateList.ForEach(c => c.AceObjectId = guid);
            ret.SpellsInSpellBars.ForEach(c => c.AceObjectId = guid);
            // Cloning an object as new should not clone inventory I don't think intentionally left out. Og II

            // No need to change Dictionary guids per DDEVEC
            // AceObjectPropertiesAttributes AceObjectPropertiesAttributes2nd AceObjectPropertiesSkills AceObjectPropertiesPositions
            ret.BookProperties = CloneDict(BookProperties);
            return ret;
        }

       public void ClearDirtyFlags()
        {
            this.IsDirty = false;
            this.HasEverBeenSavedToDatabase = true;

            this.AceObjectPropertiesAttributes.Values.ToList().ForEach(x => x.ClearDirtyFlags());
            this.AceObjectPropertiesAttributes2nd.Values.ToList().ForEach(x => x.ClearDirtyFlags());
            this.AceObjectPropertiesSkills.Values.ToList().ForEach(x => x.ClearDirtyFlags());
            this.IntProperties.ForEach(x => x.ClearDirtyFlags());
            this.Int64Properties.ForEach(x => x.ClearDirtyFlags());
            this.DoubleProperties.ForEach(x => x.ClearDirtyFlags());
            this.BoolProperties.ForEach(x => x.ClearDirtyFlags());
            this.DataIdProperties.ForEach(x => x.ClearDirtyFlags());
            this.InstanceIdProperties.ForEach(x => x.ClearDirtyFlags());
            this.StringProperties.ForEach(x => x.ClearDirtyFlags());
            // this.Inventory.ToList().ForEach(x => x.Value.ClearDirtyFlags());
            // this.WieldedItems.ToList().ForEach(x => x.Value.ClearDirtyFlags());
        }

        public void SetDirtyFlags()
        {
            this.IsDirty = true;
            this.HasEverBeenSavedToDatabase = false;

            this.AceObjectPropertiesAttributes.Values.ToList().ForEach(x => x.SetDirtyFlags());
            this.AceObjectPropertiesAttributes2nd.Values.ToList().ForEach(x => x.SetDirtyFlags());
            this.AceObjectPropertiesSkills.Values.ToList().ForEach(x => x.SetDirtyFlags());
            this.IntProperties.ForEach(x => x.SetDirtyFlags());
            this.Int64Properties.ForEach(x => x.SetDirtyFlags());
            this.DoubleProperties.ForEach(x => x.SetDirtyFlags());
            this.BoolProperties.ForEach(x => x.SetDirtyFlags());
            this.DataIdProperties.ForEach(x => x.SetDirtyFlags());
            this.InstanceIdProperties.ForEach(x => x.SetDirtyFlags());
            this.StringProperties.ForEach(x => x.SetDirtyFlags());
            // this.Inventory.ToList().ForEach(x => x.Value.SetDirtyFlags());
            // this.WieldedItems.ToList().ForEach(x => x.Value.SetDirtyFlags());
        }

        private static List<T> CloneList<T>(IEnumerable<T> toClone) where T : ICloneable
        {
            return toClone.Select(x => (T)x.Clone()).ToList();
        }

        private static Dictionary<K, V> CloneDict<K, V>(Dictionary<K, V> toClone) where V : ICloneable
        {
            return toClone.ToDictionary(x => x.Key, x => (V)x.Value.Clone());
        }

        [JsonIgnore]
        public int? LinkSlot;

        [JsonIgnore]
        public bool? LinkSource;
    }
}
