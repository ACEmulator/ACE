using System.Collections.Generic;
using ACE.Common;
using MySql.Data.MySqlClient;
using ACE.Entity.Enum.Properties;
using System;
using ACE.Entity.Enum;
using System.Linq;
using Newtonsoft.Json;

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
        public bool HasEverBeenSavedToDatabase { get; set; } = false;

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

        private uint _aceObjectDescriptionFlags = 0;

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

        private uint _physicsDescriptionFlag = 0;

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

        private uint _weenieHeaderFlags = 0;

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

        private uint _weenieHeaderFlags2 = 0;

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

        private string _currentMotionState = null;

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
        public uint Strength
        { get { return StrengthAbility.MaxValue; } }

        [JsonIgnore]
        public uint Endurance
        { get { return EnduranceAbility.MaxValue; } }

        [JsonIgnore]
        public uint Coordination
        { get { return CoordinationAbility.MaxValue; } }

        [JsonIgnore]
        public uint Quickness
        { get { return QuicknessAbility.MaxValue; } }

        [JsonIgnore]
        public uint Focus
        { get { return FocusAbility.MaxValue; } }

        [JsonIgnore]
        public uint Self
        { get { return SelfAbility.MaxValue; } }

        [JsonIgnore]
        public uint? SetupDID
        {
            get { return GetDataIdProperty(PropertyDataId.Setup); }
            set { SetDataIdProperty(PropertyDataId.Setup, value); }
        }

        [JsonIgnore]
        public uint? MotionTableDID
        {
            get { return GetDataIdProperty(PropertyDataId.MotionTable); }
            set { SetDataIdProperty(PropertyDataId.MotionTable, value); }
        }

        [JsonIgnore]
        public uint? SoundTableDID
        {
            get { return GetDataIdProperty(PropertyDataId.SoundTable); }
            set { SetDataIdProperty(PropertyDataId.SoundTable, value); }
        }

        [JsonIgnore]
        public uint? PhysicsEffectTableDID
        {
            get { return GetDataIdProperty(PropertyDataId.PhysicsEffectTable); }
            set { SetDataIdProperty(PropertyDataId.PhysicsEffectTable, value); }
        }

        [JsonIgnore]
        public uint? CombatTableDID
        {
            get { return GetDataIdProperty(PropertyDataId.CombatTable); }
            set { SetDataIdProperty(PropertyDataId.CombatTable, value); }
        }

        [JsonIgnore]
        public int? PhysicsState
        {
            get { return GetIntProperty(PropertyInt.PhysicsState); }
            set { SetIntProperty(PropertyInt.PhysicsState, value); }
        }

        [JsonIgnore]
        public int? WeenieType
        {
            get { return GetIntProperty(PropertyInt.WeenieType); }
            set { SetIntProperty(PropertyInt.WeenieType, value); }
        }

        [JsonIgnore]
        public int? ItemType
        {
            get { return GetIntProperty(PropertyInt.ItemType); }
            set { SetIntProperty(PropertyInt.ItemType, value); }
        }

        [JsonIgnore]
        public uint? IconDID
        {
            get { return GetDataIdProperty(PropertyDataId.Icon); }
            set { SetDataIdProperty(PropertyDataId.Icon, value); }
        }

        [JsonIgnore]
        public string Name
        {
            get { return GetStringProperty(PropertyString.Name); }
            set { SetStringProperty(PropertyString.Name, value); }
        }

        [JsonIgnore]
        public string PluralName
        {
            get { return GetStringProperty(PropertyString.PluralName); }
            set { SetStringProperty(PropertyString.PluralName, value); }
        }
        [JsonIgnore]

        public byte? ItemsCapacity
        {
            get { return (byte?)GetIntProperty(PropertyInt.ItemsCapacity); }
            set { SetIntProperty(PropertyInt.ItemsCapacity, (int)value); }
        }

        [JsonIgnore]
        public byte? ContainersCapacity
        {
            get { return (byte?)GetIntProperty(PropertyInt.ContainersCapacity); }
            set { SetIntProperty(PropertyInt.ContainersCapacity, (int)value); }
        }

        [JsonIgnore]
        public int? AmmoType
        {
            get { return GetIntProperty(PropertyInt.AmmoType); }
            set { SetIntProperty(PropertyInt.AmmoType, (int)value); }
        }

        [JsonIgnore]
        public int? Value
        {
            get { return GetIntProperty(PropertyInt.Value); }
            set { SetIntProperty(PropertyInt.Value, value); }
        }

        [JsonIgnore]
        public int? UseCreateContractId
        {
            get { return GetIntProperty(PropertyInt.UseCreatesContractId); }
            set { SetIntProperty(PropertyInt.UseCreatesContractId, value); }
        }

        [JsonIgnore]
        public int? ItemUseable
        {
            get { return GetIntProperty(PropertyInt.ItemUseable); }
            set { SetIntProperty(PropertyInt.ItemUseable, (int)value); }
        }

        [JsonIgnore]
        public float? UseRadius
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.UseRadius); }
            set { SetDoubleProperty(PropertyDouble.UseRadius, value); }
        }

        [JsonIgnore]
        public int? TargetType
        {
            get { return GetIntProperty(PropertyInt.TargetType); }
            set { SetIntProperty(PropertyInt.TargetType, value); }
        }

        [JsonIgnore]
        public int? UiEffects
        {
            get { return GetIntProperty(PropertyInt.UiEffects); }
            set { SetIntProperty(PropertyInt.UiEffects, value); }
        }

        [JsonIgnore]
        public byte? CombatUse
        {
            get { return (byte?)GetIntProperty(PropertyInt.CombatUse); }
            set { SetIntProperty(PropertyInt.CombatUse, value); }
        }

        [JsonIgnore]
        public int? DefaultCombatStyle
        {
            get { return GetIntProperty(PropertyInt.DefaultCombatStyle); }
            set { SetIntProperty(PropertyInt.DefaultCombatStyle, value); }
        }

        [JsonIgnore]
        public ushort? Structure
        {
            get { return (ushort?)GetIntProperty(PropertyInt.Structure); }
            set { SetIntProperty(PropertyInt.Structure, value); }
        }

        [JsonIgnore]
        public ushort? MaxStructure
        {
            get { return (ushort?)GetIntProperty(PropertyInt.MaxStructure); }
            set { SetIntProperty(PropertyInt.MaxStructure, value); }
        }

        [JsonIgnore]
        public ushort? StackSize
        {
            get { return (ushort?)GetIntProperty(PropertyInt.StackSize); }
            set { SetIntProperty(PropertyInt.StackSize, value); }
        }

        [JsonIgnore]
        public ushort? MaxStackSize
        {
            get { return (ushort?)GetIntProperty(PropertyInt.MaxStackSize); }
            set { SetIntProperty(PropertyInt.MaxStackSize, value); }
        }

        [JsonIgnore]
        public uint? ContainerIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Container); }
            set { SetInstanceIdProperty(PropertyInstanceId.Container, value); }
        }

        [JsonIgnore]
        public int? Placement
        {
            get { return GetIntProperty(PropertyInt.Placement); }
            set { SetIntProperty(PropertyInt.Placement, value); }
        }

        [JsonIgnore]
        public uint? WielderIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Wielder); }
            set { SetInstanceIdProperty(PropertyInstanceId.Wielder, value); }
        }

        [JsonIgnore]
        public uint? GeneratorIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Generator); }
            set { SetInstanceIdProperty(PropertyInstanceId.Generator, value); }
        }

        [JsonIgnore]
        public int? ValidLocations
        {
            get { return GetIntProperty(PropertyInt.ValidLocations); }
            set { SetIntProperty(PropertyInt.ValidLocations, value); }
        }

        [JsonIgnore]
        public int? CurrentWieldedLocation
        {
            get { return GetIntProperty(PropertyInt.CurrentWieldedLocation); }
            set { SetIntProperty(PropertyInt.CurrentWieldedLocation, value); }
        }

        [JsonIgnore]
        public int? ClothingPriority
        {
            get { return GetIntProperty(PropertyInt.ClothingPriority); }
            set { SetIntProperty(PropertyInt.ClothingPriority, value); }
        }

        [JsonIgnore]
        public byte? RadarBlipColor
        {
            get { return (byte?)GetIntProperty(PropertyInt.RadarBlipColor); }
            set { SetIntProperty(PropertyInt.RadarBlipColor, value); }
        }

        [JsonIgnore]
        public byte? ShowableOnRadar
        {
            get { return (byte?)GetIntProperty(PropertyInt.ShowableOnRadar); }
            set { SetIntProperty(PropertyInt.ShowableOnRadar, value); }
        }

        [JsonIgnore]
        public ushort? PhysicsScriptDID
        {
            get { return (ushort?)GetDataIdProperty(PropertyDataId.PhysicsScript); }
            set { SetDataIdProperty(PropertyDataId.PhysicsScript, value); }
        }

        [JsonIgnore]
        public int? ItemWorkmanship
        {
            get { return GetIntProperty(PropertyInt.ItemWorkmanship); }
            set { SetIntProperty(PropertyInt.ItemWorkmanship, value); }
        }

        [JsonIgnore]
        public ushort? EncumbranceVal
        {
            get { return (ushort?)GetIntProperty(PropertyInt.EncumbranceVal); }
            set { SetIntProperty(PropertyInt.EncumbranceVal, value); }
        }

        [JsonIgnore]
        public uint? SpellDID
        {
            get { return GetDataIdProperty(PropertyDataId.Spell); }
            set { SetDataIdProperty(PropertyDataId.Spell, value); }
        }

        [JsonIgnore]
        public ushort? HookType
        {
            get { return (ushort?)GetIntProperty(PropertyInt.HookType); }
            set { SetIntProperty(PropertyInt.HookType, value); }
        }

        [JsonIgnore]
        public ushort? HookItemType
        {
            get { return (ushort?)GetIntProperty(PropertyInt.HookItemType); }
            set { SetIntProperty(PropertyInt.HookItemType, value); }
        }

        [JsonIgnore]
        public uint? IconOverlayDID
        {
            get { return GetDataIdProperty(PropertyDataId.IconOverlay); }
            set { SetDataIdProperty(PropertyDataId.IconOverlay, value); }
        }

        [JsonIgnore]
        public uint? IconUnderlayDID
        {
            get { return GetDataIdProperty(PropertyDataId.IconUnderlay); }
            set { SetDataIdProperty(PropertyDataId.IconUnderlay, value); }
        }

        [JsonIgnore]
        public byte? MaterialType
        {
            get { return (byte?)GetIntProperty(PropertyInt.MaterialType); }
            set { SetIntProperty(PropertyInt.MaterialType, (byte?)value); }
        }

        [JsonIgnore]
        public int? SharedCooldown
        {
            get { return GetIntProperty(PropertyInt.SharedCooldown); }
            set { SetIntProperty(PropertyInt.SharedCooldown, value); }
        }

        [JsonIgnore]
        public double? CooldownDuration
        {
            get { return GetDoubleProperty(PropertyDouble.CooldownDuration); }
            set { SetDoubleProperty(PropertyDouble.CooldownDuration, value); }
        }

        // Wielder is Parent, No such thing as PropertyInstanceId.Parent
        [JsonIgnore]
        public uint? ParentIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Wielder); }
            set { SetInstanceIdProperty(PropertyInstanceId.Wielder, value); }
        }

        [JsonIgnore]
        public int? ParentLocation
        {
            get { return GetIntProperty(PropertyInt.ParentLocation); }
            set { SetIntProperty(PropertyInt.ParentLocation, value); }
        }

        [JsonIgnore]
        public float? DefaultScale
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.DefaultScale); }
            set { SetDoubleProperty(PropertyDouble.DefaultScale, value); }
        }

        [JsonIgnore]
        public float? Friction
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Friction); }
            set { SetDoubleProperty(PropertyDouble.Friction, value); }
        }

        [JsonIgnore]
        public float? Elasticity
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Elasticity); }
            set { SetDoubleProperty(PropertyDouble.Elasticity, value); }
        }

        [JsonIgnore]
        public int? PlacementPosition
        {
            get { return GetIntProperty(PropertyInt.PlacementPosition); }
            set { SetIntProperty(PropertyInt.PlacementPosition, value); }
        }

        [JsonIgnore]
        public float? Translucency
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Translucency); }
            set { SetDoubleProperty(PropertyDouble.Translucency, value); }
        }
        
        [JsonIgnore]
        public float? PhysicsScriptIntensity
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.PhysicsScriptIntensity); }
            set { SetDoubleProperty(PropertyDouble.PhysicsScriptIntensity, value); }
        }

        [JsonIgnore]
        public uint? PaletteBaseDID
        {
            get { return GetDataIdProperty(PropertyDataId.PaletteBase); }
            set { SetDataIdProperty(PropertyDataId.PaletteBase, value); }
        }

        [JsonIgnore]
        public uint? ClothingBaseDID
        {
            get { return GetDataIdProperty(PropertyDataId.ClothingBase); }
            set { SetDataIdProperty(PropertyDataId.ClothingBase, value); }
        }

        [JsonIgnore]
        public int? CharacterOptions1
        {
            get { return GetIntProperty(PropertyInt.CharacterOptions1); }
            set { SetIntProperty(PropertyInt.CharacterOptions1, value); }
        }

        [JsonIgnore]
        public int? CharacterOptions2
        {
            get { return GetIntProperty(PropertyInt.CharacterOptions2); }
            set { SetIntProperty(PropertyInt.CharacterOptions2, value); }
        }

        [JsonIgnore]
        public int? TotalLogins
        {
            get { return GetIntProperty(PropertyInt.TotalLogins); }
            set { SetIntProperty(PropertyInt.TotalLogins, value); }
        }

        [JsonIgnore]
        public uint? SubscriptionId
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Subscription); }
            set { SetInstanceIdProperty(PropertyInstanceId.Subscription, value); }
        }

        [JsonIgnore]
        public bool? IsDeleted
        {
            get { return GetBoolProperty(PropertyBool.IsDeleted); }
            set { SetBoolProperty(PropertyBool.IsDeleted, value); }
        }

        [JsonIgnore]
        public ulong? DeleteTime
        {
            get { return GetInt64Property(PropertyInt64.DeleteTime); }
            set { SetInt64Property(PropertyInt64.DeleteTime, value); }
        }

        [JsonIgnore]
        public ulong? AvailableExperience
        {
            get { return GetInt64Property(PropertyInt64.AvailableExperience); }
            set { SetInt64Property(PropertyInt64.AvailableExperience, value); }
        }

        [JsonIgnore]
        public ulong? TotalExperience
        {
            get { return GetInt64Property(PropertyInt64.TotalExperience); }
            set { SetInt64Property(PropertyInt64.TotalExperience, value); }
        }

        [JsonIgnore]
        public int? Age
        {
            get { return GetIntProperty(PropertyInt.Age); }
            set { SetIntProperty(PropertyInt.Age, value); }
        }

        [JsonIgnore]
        public string DateOfBirth
        {
            get { return GetStringProperty(PropertyString.DateOfBirth); }
            set { SetStringProperty(PropertyString.DateOfBirth, value); }
        }

        [JsonIgnore]
        public int? AvailableSkillCredits
        {
            get { return GetIntProperty(PropertyInt.AvailableSkillCredits); }
            set { SetIntProperty(PropertyInt.AvailableSkillCredits, value); }
        }

        [JsonIgnore]
        public int? TotalSkillCredits
        {
            get { return GetIntProperty(PropertyInt.TotalSkillCredits); }
            set { SetIntProperty(PropertyInt.TotalSkillCredits, value); }
        }

        [JsonIgnore]
        public int? NumDeaths
        {
            get { return GetIntProperty(PropertyInt.NumDeaths); }
            set { SetIntProperty(PropertyInt.NumDeaths, value); }
        }

        [JsonIgnore]
        public int? DeathLevel
        {
            get { return GetIntProperty(PropertyInt.DeathLevel); }
            set { SetIntProperty(PropertyInt.DeathLevel, value); }
        }

        [JsonIgnore]
        public int? VitaeCpPool
        {
            get { return GetIntProperty(PropertyInt.VitaeCpPool); }
            set { SetIntProperty(PropertyInt.VitaeCpPool, value); }
        }
        
        [JsonIgnore]
        public bool? IsAdmin
        {
            get { return GetBoolProperty(PropertyBool.IsAdmin); }
            set { SetBoolProperty(PropertyBool.IsAdmin, value); }
        }

        [JsonIgnore]
        public bool? IsEnvoy
        {
            get { return GetBoolProperty(PropertyBool.IsSentinel); }
            set { SetBoolProperty(PropertyBool.IsSentinel, value); }
        }

        [JsonIgnore]
        public bool? IsArch
        {
            get { return GetBoolProperty(PropertyBool.IsArch); }
            set { SetBoolProperty(PropertyBool.IsArch, value); }
        }

        [JsonIgnore]
        public bool? IsPsr
        {
            get { return GetBoolProperty(PropertyBool.IsPsr); }
            set { SetBoolProperty(PropertyBool.IsPsr, value); }
        }

        [JsonIgnore]
        public int? Heritage
        {
            get { return GetIntProperty(PropertyInt.HeritageGroup); }
            set { SetIntProperty(PropertyInt.HeritageGroup, value); }
        }

        [JsonIgnore]
        public int? Gender
        {
            get { return GetIntProperty(PropertyInt.Gender); }
            set { SetIntProperty(PropertyInt.Gender, value); }
        }

        [JsonIgnore]
        public uint? EyesTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.EyesTexture); }
            set { SetDataIdProperty(PropertyDataId.EyesTexture, value); }
        }

        [JsonIgnore]
        public uint? DefaultEyesTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultEyesTexture); }
            set { SetDataIdProperty(PropertyDataId.DefaultEyesTexture, value); }
        }

        [JsonIgnore]
        public uint? NoseTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.NoseTexture); }
            set { SetDataIdProperty(PropertyDataId.NoseTexture, value); }
        }

        [JsonIgnore]
        public uint? DefaultNoseTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultNoseTexture); }
            set { SetDataIdProperty(PropertyDataId.DefaultNoseTexture, value); }
        }

        [JsonIgnore]
        public uint? MouthTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.MouthTexture); }
            set { SetDataIdProperty(PropertyDataId.MouthTexture, value); }
        }

        [JsonIgnore]
        public uint? DefaultMouthTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultMouthTexture); }
            set { SetDataIdProperty(PropertyDataId.DefaultMouthTexture, value); }
        }

        [JsonIgnore]
        public uint? HairTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.HairTexture); }
            set { SetDataIdProperty(PropertyDataId.HairTexture, value); }
        }

        [JsonIgnore]
        public uint? DefaultHairTextureDID
        {
            get { return GetDataIdProperty(PropertyDataId.DefaultHairTexture); }
            set { SetDataIdProperty(PropertyDataId.DefaultHairTexture, value); }
        }

        [JsonIgnore]
        public uint? HeadObjectDID
        {
            get { return GetDataIdProperty(PropertyDataId.HeadObject); }
            set { SetDataIdProperty(PropertyDataId.HeadObject, value); }
        }

        [JsonIgnore]
        public uint? SkinPaletteDID
        {
            get { return GetDataIdProperty(PropertyDataId.SkinPalette); }
            set { SetDataIdProperty(PropertyDataId.SkinPalette, value); }
        }

        [JsonIgnore]
        public uint? HairPaletteDID
        {
            get { return GetDataIdProperty(PropertyDataId.HairPalette); }
            set { SetDataIdProperty(PropertyDataId.HairPalette, value); }
        }

        [JsonIgnore]
        public uint? EyesPaletteDID
        {
            get { return GetDataIdProperty(PropertyDataId.EyesPalette); }
            set { SetDataIdProperty(PropertyDataId.EyesPalette, value); }
        }

        [JsonIgnore]
        public int? Level
        {
            get { return GetIntProperty(PropertyInt.Level); }
            set { SetIntProperty(PropertyInt.Level, value); }
        }

        [JsonIgnore]
        public bool? GeneratorStatus
        {
            get { return GetBoolProperty(PropertyBool.GeneratorStatus); }
            set { SetBoolProperty(PropertyBool.GeneratorStatus, value); }
        }

        [JsonIgnore]
        public bool? GeneratorEnteredWorld
        {
            get { return GetBoolProperty(PropertyBool.GeneratorEnteredWorld); }
            set { SetBoolProperty(PropertyBool.GeneratorEnteredWorld, value); }
        }

        [JsonIgnore]
        public bool? GeneratorDisabled
        {
            get { return GetBoolProperty(PropertyBool.GeneratorDisabled); }
            set { SetBoolProperty(PropertyBool.GeneratorDisabled, value); }
        }

        [JsonIgnore]
        public bool? GeneratedTreasureItem
        {
            get { return GetBoolProperty(PropertyBool.GeneratedTreasureItem); }
            set { SetBoolProperty(PropertyBool.GeneratedTreasureItem, value); }
        }

        [JsonIgnore]
        public bool? GeneratorAutomaticDestruction
        {
            get { return GetBoolProperty(PropertyBool.GeneratorAutomaticDestruction); }
            set { SetBoolProperty(PropertyBool.GeneratorAutomaticDestruction, value); }
        }

        [JsonIgnore]
        public bool? CanGenerateRare
        {
            get { return GetBoolProperty(PropertyBool.CanGenerateRare); }
            set { SetBoolProperty(PropertyBool.CanGenerateRare, value); }
        }

        [JsonIgnore]
        public bool? CorpseGeneratedRare
        {
            get { return GetBoolProperty(PropertyBool.CorpseGeneratedRare); }
            set { SetBoolProperty(PropertyBool.CorpseGeneratedRare, value); }
        }

        [JsonIgnore]
        public bool? SuppressGenerateEffect
        {
            get { return GetBoolProperty(PropertyBool.SuppressGenerateEffect); }
            set { SetBoolProperty(PropertyBool.SuppressGenerateEffect, value); }
        }

        [JsonIgnore]
        public bool? ChestRegenOnClose
        {
            get { return GetBoolProperty(PropertyBool.ChestRegenOnClose); }
            set { SetBoolProperty(PropertyBool.ChestRegenOnClose, value); }
        }

        [JsonIgnore]
        public bool? ChestClearedWhenClosed
        {
            get { return GetBoolProperty(PropertyBool.ChestClearedWhenClosed); }
            set { SetBoolProperty(PropertyBool.ChestClearedWhenClosed, value); }
        }

        [JsonIgnore]
        public int? GeneratorTimeType
        {
            get { return GetIntProperty(PropertyInt.GeneratorTimeType); }
            set { SetIntProperty(PropertyInt.GeneratorTimeType, value); }
        }

        [JsonIgnore]
        public int? GeneratorProbability
        {
            get { return GetIntProperty(PropertyInt.GeneratorProbability); }
            set { SetIntProperty(PropertyInt.GeneratorProbability, value); }
        }

        [JsonIgnore]
        public int? MaxGeneratedObjects
        {
            get { return GetIntProperty(PropertyInt.MaxGeneratedObjects); }
            set { SetIntProperty(PropertyInt.MaxGeneratedObjects, value); }
        }

        [JsonIgnore]
        public int? GeneratorType
        {
            get { return GetIntProperty(PropertyInt.GeneratorType); }
            set { SetIntProperty(PropertyInt.GeneratorType, value); }
        }

        [JsonIgnore]
        public int? ActivationCreateClass
        {
            get { return GetIntProperty(PropertyInt.ActivationCreateClass); }
            set { SetIntProperty(PropertyInt.ActivationCreateClass, value); }
        }

        [JsonIgnore]
        public bool? Ethereal
        {
            get { return GetBoolProperty(PropertyBool.Ethereal); }
            set { SetBoolProperty(PropertyBool.Ethereal, value); }
        }

        [JsonIgnore]
        public bool? Open
        {
            get { return GetBoolProperty(PropertyBool.Open); }
            set { SetBoolProperty(PropertyBool.Open, value); }
        }

        [JsonIgnore]
        public bool? Locked
        {
            get { return GetBoolProperty(PropertyBool.Locked); }
            set { SetBoolProperty(PropertyBool.Locked, value); }
        }

        [JsonIgnore]
        public bool? DefaultLocked
        {
            get { return GetBoolProperty(PropertyBool.DefaultLocked); }
            set { SetBoolProperty(PropertyBool.DefaultLocked, value); }
        }

        [JsonIgnore]
        public bool? DefaultOpen
        {
            get { return GetBoolProperty(PropertyBool.DefaultOpen); }
            set { SetBoolProperty(PropertyBool.DefaultOpen, value); }
        }

        [JsonIgnore]
        public float? ResetInterval
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.ResetInterval); }
            set { SetDoubleProperty(PropertyDouble.ResetInterval, value); }
        }

        [JsonIgnore]
        public double? ResetTimestamp
        {
            get { return GetDoubleProperty(PropertyDouble.ResetTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.ResetTimestamp); }
        }

        [JsonIgnore]
        public double? UseTimestamp
        {
            get { return GetDoubleProperty(PropertyDouble.UseTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.UseTimestamp); }
        }

        [JsonIgnore]
        public double? UseLockTimestamp
        {
            get { return GetDoubleProperty(PropertyDouble.UseLockTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.UseLockTimestamp); }
        }

        [JsonIgnore]
        public uint? LastUnlockerIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.LastUnlocker); }
            set { SetInstanceIdProperty(PropertyInstanceId.LastUnlocker, value); }
        }

        [JsonIgnore]
        public string KeyCode
        {
            get { return GetStringProperty(PropertyString.KeyCode); }
            set { SetStringProperty(PropertyString.KeyCode, value); }
        }

        [JsonIgnore]
        public string LockCode
        {
            get { return GetStringProperty(PropertyString.LockCode); }
            set { SetStringProperty(PropertyString.LockCode, value); }
        }

        [JsonIgnore]
        public int? ResistLockpick
        {
            get { return GetIntProperty(PropertyInt.ResistLockpick); }
            set { SetIntProperty(PropertyInt.ResistLockpick, value); }
        }

        [JsonIgnore]
        public int? AppraisalLockpickSuccessPercent
        {
            get { return GetIntProperty(PropertyInt.AppraisalLockpickSuccessPercent); }
            set { SetIntProperty(PropertyInt.AppraisalLockpickSuccessPercent, value); }
        }

        [JsonIgnore]
        public int? MinLevel
        {
            get { return GetIntProperty(PropertyInt.MinLevel); }
            set { SetIntProperty(PropertyInt.MinLevel, value); }
        }

        [JsonIgnore]
        public int? MaxLevel
        {
            get { return GetIntProperty(PropertyInt.MaxLevel); }
            set { SetIntProperty(PropertyInt.MaxLevel, value); }
        }

        [JsonIgnore]
        public int? PortalBitmask
        {
            get { return GetIntProperty(PropertyInt.PortalBitmask); }
            set { SetIntProperty(PropertyInt.PortalBitmask, value); }
        }

        [JsonIgnore]
        public string AppraisalPortalDestination
        {
            get { return GetStringProperty(PropertyString.AppraisalPortalDestination); }
            set { SetStringProperty(PropertyString.AppraisalPortalDestination, value); }
        }

        [JsonIgnore]
        public string ShortDesc
        {
            get { return GetStringProperty(PropertyString.ShortDesc); }
            set { SetStringProperty(PropertyString.ShortDesc, value); }
        }

        [JsonIgnore]
        public string LongDesc
        {
            get { return GetStringProperty(PropertyString.LongDesc); }
            set { SetStringProperty(PropertyString.LongDesc, value); }
        }

        [JsonIgnore]
        public string Use
        {
            get { return GetStringProperty(PropertyString.Use); }
            set { SetStringProperty(PropertyString.Use, value); }
        }

        [JsonIgnore]
        public string UseMessage
        {
            get { return GetStringProperty(PropertyString.UseMessage); }
            set { SetStringProperty(PropertyString.UseMessage, value); }
        }

        [JsonIgnore]
        public bool? PortalShowDestination
        {
            get { return GetBoolProperty(PropertyBool.PortalShowDestination); }
            set { SetBoolProperty(PropertyBool.PortalShowDestination, value); }
        }

        [JsonIgnore]
        public string HeritageGroup
        {
            get { return GetStringProperty(PropertyString.HeritageGroup); }
            set { SetStringProperty(PropertyString.HeritageGroup, value); }
        }

        [JsonIgnore]
        public string Sex
        {
            get { return GetStringProperty(PropertyString.Sex); }
            set { SetStringProperty(PropertyString.Sex, value); }
        }

        [JsonIgnore]
        public string Title
        {
            get { return GetStringProperty(PropertyString.Title); }
            set { SetStringProperty(PropertyString.Title, value); }
        }

        [JsonIgnore]
        public string Template
        {
            get { return GetStringProperty(PropertyString.Template); }
            set { SetStringProperty(PropertyString.Template, value); }
        }

        [JsonIgnore]
        public string DisplayName
        {
            get { return GetStringProperty(PropertyString.DisplayName); }
            set { SetStringProperty(PropertyString.DisplayName, value); }
        }

        [JsonIgnore]
        public int? CharacterTitleId
        {
            get { return GetIntProperty(PropertyInt.CharacterTitleId); }
            set { SetIntProperty(PropertyInt.CharacterTitleId, value); }
        }

        [JsonIgnore]
        public int? NumCharacterTitles
        {
            get { return GetIntProperty(PropertyInt.NumCharacterTitles); }
            set { SetIntProperty(PropertyInt.NumCharacterTitles, value); }
        }

        [JsonIgnore]
        public double? CreationTimestamp
        {
            get { return GetDoubleProperty(PropertyDouble.CreationTimestamp); }
            set { SetDoubleTimestamp(PropertyDouble.CreationTimestamp); }
        }

        [JsonIgnore]
        public bool? Stuck
        {
            get { return GetBoolProperty(PropertyBool.Stuck); }
            set { SetBoolProperty(PropertyBool.Stuck, value); }
        }

        [JsonIgnore]
        public bool? IgnoreCollisions
        {
            get { return GetBoolProperty(PropertyBool.IgnoreCollisions); }
            set { SetBoolProperty(PropertyBool.IgnoreCollisions, value); }
        }

        [JsonIgnore]
        public bool? ReportCollisions
        {
            get { return GetBoolProperty(PropertyBool.ReportCollisions); }
            set { SetBoolProperty(PropertyBool.ReportCollisions, value); }
        }

        [JsonIgnore]
        public bool? GravityStatus
        {
            get { return GetBoolProperty(PropertyBool.GravityStatus); }
            set { SetBoolProperty(PropertyBool.GravityStatus, value); }
        }

        [JsonIgnore]
        public bool? LightsStatus
        {
            get { return GetBoolProperty(PropertyBool.LightsStatus); }
            set { SetBoolProperty(PropertyBool.LightsStatus, value); }
        }

        [JsonIgnore]
        public bool? ScriptedCollision
        {
            get { return GetBoolProperty(PropertyBool.ScriptedCollision); }
            set { SetBoolProperty(PropertyBool.ScriptedCollision, value); }
        }

        [JsonIgnore]
        public bool? Inelastic
        {
            get { return GetBoolProperty(PropertyBool.Inelastic); }
            set { SetBoolProperty(PropertyBool.Inelastic, value); }
        }

        [JsonIgnore]
        public bool? Visibility
        {
            get { return GetBoolProperty(PropertyBool.Visibility); }
            set { SetBoolProperty(PropertyBool.Visibility, value); }
        }

        [JsonIgnore]
        public bool? Attackable
        {
            get { return GetBoolProperty(PropertyBool.Attackable); }
            set { SetBoolProperty(PropertyBool.Attackable, value); }
        }

        [JsonIgnore]
        public bool? AdvocateState
        {
            get { return GetBoolProperty(PropertyBool.AdvocateState); }
            set { SetBoolProperty(PropertyBool.AdvocateState, value); }
        }

        [JsonIgnore]
        public bool? Inscribable
        {
            get { return GetBoolProperty(PropertyBool.Inscribable); }
            set { SetBoolProperty(PropertyBool.Inscribable, value); }
        }

        [JsonIgnore]
        public bool? UiHidden
        {
            get { return GetBoolProperty(PropertyBool.UiHidden); }
            set { SetBoolProperty(PropertyBool.UiHidden, value); }
        }

        [JsonIgnore]
        public bool? IgnoreHouseBarriers
        {
            get { return GetBoolProperty(PropertyBool.IgnoreHouseBarriers); }
            set { SetBoolProperty(PropertyBool.IgnoreHouseBarriers, value); }
        }

        [JsonIgnore]
        public bool? HiddenAdmin
        {
            get { return GetBoolProperty(PropertyBool.HiddenAdmin); }
            set { SetBoolProperty(PropertyBool.HiddenAdmin, value); }
        }

        [JsonIgnore]
        public bool? PkWounder
        {
            get { return GetBoolProperty(PropertyBool.PkWounder); }
            set { SetBoolProperty(PropertyBool.PkWounder, value); }
        }

        [JsonIgnore]
        public bool? PkKiller
        {
            get { return GetBoolProperty(PropertyBool.PkKiller); }
            set { SetBoolProperty(PropertyBool.PkKiller, value); }
        }

        [JsonIgnore]
        public bool? UnderLifestoneProtection
        {
            get { return GetBoolProperty(PropertyBool.UnderLifestoneProtection); }
            set { SetBoolProperty(PropertyBool.UnderLifestoneProtection, value); }
        }

        [JsonIgnore]
        public bool? DefaultOn
        {
            get { return GetBoolProperty(PropertyBool.DefaultOn); }
            set { SetBoolProperty(PropertyBool.DefaultOn, value); }
        }

        [JsonIgnore]
        public bool? IsFrozen
        {
            get { return GetBoolProperty(PropertyBool.IsFrozen); }
            set { SetBoolProperty(PropertyBool.IsFrozen, value); }
        }

        [JsonIgnore]
        public bool? ReportCollisionsAsEnvironment
        {
            get { return GetBoolProperty(PropertyBool.ReportCollisionsAsEnvironment); }
            set { SetBoolProperty(PropertyBool.ReportCollisionsAsEnvironment, value); }
        }

        [JsonIgnore]
        public bool? AllowEdgeSlide
        {
            get { return GetBoolProperty(PropertyBool.AllowEdgeSlide); }
            set { SetBoolProperty(PropertyBool.AllowEdgeSlide, value); }
        }

        [JsonIgnore]
        public bool? AdvocateQuest
        {
            get { return GetBoolProperty(PropertyBool.AdvocateQuest); }
            set { SetBoolProperty(PropertyBool.AdvocateQuest, value); }
        }

        [JsonIgnore]
        public bool? IsAdvocate
        {
            get { return GetBoolProperty(PropertyBool.IsAdvocate); }
            set { SetBoolProperty(PropertyBool.IsAdvocate, value); }
        }

        [JsonIgnore]
        public bool? IsSentinel
        {
            get { return GetBoolProperty(PropertyBool.IsSentinel); }
            set { SetBoolProperty(PropertyBool.IsSentinel, value); }
        }

        [JsonIgnore]
        public bool? NoDraw
        {
            get { return GetBoolProperty(PropertyBool.NoDraw); }
            set { SetBoolProperty(PropertyBool.NoDraw, value); }
        }

        [JsonIgnore]
        public bool? IgnorePortalRestrictions
        {
            get { return GetBoolProperty(PropertyBool.IgnorePortalRestrictions); }
            set { SetBoolProperty(PropertyBool.IgnorePortalRestrictions, value); }
        }

        [JsonIgnore]
        public bool? Retained
        {
            get { return GetBoolProperty(PropertyBool.Retained); }
            set { SetBoolProperty(PropertyBool.Retained, value); }
        }

        [JsonIgnore]
        public bool? Invincible
        {
            get { return GetBoolProperty(PropertyBool.Invincible); }
            set { SetBoolProperty(PropertyBool.Invincible, value); }
        }

        [JsonIgnore]
        public bool? IsGagged
        {
            get { return GetBoolProperty(PropertyBool.IsGagged); }
            set { SetBoolProperty(PropertyBool.IsGagged, value); }
        }

        [JsonIgnore]
        public bool? Afk
        {
            get { return GetBoolProperty(PropertyBool.Afk); }
            set { SetBoolProperty(PropertyBool.Afk, value); }
        }

        [JsonIgnore]
        public bool? IgnoreAuthor
        {
            get { return GetBoolProperty(PropertyBool.IgnoreAuthor); }
            set { SetBoolProperty(PropertyBool.IgnoreAuthor, value); }
        }

        [JsonIgnore]
        public bool? WieldOnUse
        {
            get { return GetBoolProperty(PropertyBool.WieldOnUse); }
            set { SetBoolProperty(PropertyBool.WieldOnUse, value); }
        }

        [JsonIgnore]
        public bool? AutowieldLeft
        {
            get { return GetBoolProperty(PropertyBool.AutowieldLeft); }
            set { SetBoolProperty(PropertyBool.AutowieldLeft, value); }
        }

        [JsonIgnore]
        public bool? VendorService
        {
            get { return GetBoolProperty(PropertyBool.VendorService); }
            set { SetBoolProperty(PropertyBool.VendorService, value); }
        }

        [JsonIgnore]
        public bool? RequiresBackpackSlot
        {
            get { return GetBoolProperty(PropertyBool.RequiresBackpackSlot); }
            set { SetBoolProperty(PropertyBool.RequiresBackpackSlot, value); }
        }

        [JsonIgnore]
        public bool UseBackpackSlot
        {
            get { return (GetBoolProperty(PropertyBool.RequiresBackpackSlot) ?? false) ||
                          GetIntProperty(PropertyInt.WeenieType) == (uint)Enum.WeenieType.Container; }
        }

        [JsonIgnore]
        public int? ItemCurMana
        {
            get { return GetIntProperty(PropertyInt.ItemCurMana); }
            set { SetIntProperty(PropertyInt.ItemCurMana, value); }
        }

        [JsonIgnore]
        public int? ItemMaxMana
        {
            get { return GetIntProperty(PropertyInt.ItemMaxMana); }
            set { SetIntProperty(PropertyInt.ItemMaxMana, value); }
        }

        [JsonIgnore]
        public bool? NpcLooksLikeObject
        {
            get { return GetBoolProperty(PropertyBool.NpcLooksLikeObject); }
            set { SetBoolProperty(PropertyBool.NpcLooksLikeObject, value); }
        }

        [JsonIgnore]
        public uint? AllowedActivator
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.AllowedActivator); }
            set { SetInstanceIdProperty(PropertyInstanceId.AllowedActivator, value); }
        }

        [JsonIgnore]
        public int? CreatureType
        {
            get { return GetIntProperty(PropertyInt.CreatureType); }
            set { SetIntProperty(PropertyInt.CreatureType, value); }
        }

        [JsonIgnore]
        public int? MerchandiseItemTypes
        {
            get { return GetIntProperty(PropertyInt.MerchandiseItemTypes); }
            set { SetIntProperty(PropertyInt.MerchandiseItemTypes, value); }
        }

        [JsonIgnore]
        public int? MerchandiseMinValue
        {
            get { return GetIntProperty(PropertyInt.MerchandiseMinValue); }
            set { SetIntProperty(PropertyInt.MerchandiseMinValue, value); }
        }

        [JsonIgnore]
        public int? MerchandiseMaxValue
        {
            get { return GetIntProperty(PropertyInt.MerchandiseMaxValue); }
            set { SetIntProperty(PropertyInt.MerchandiseMaxValue, value); }
        }

        [JsonIgnore]
        public string Inscription
        {
            get { return GetStringProperty(PropertyString.Inscription); }
            set { SetStringProperty(PropertyString.Inscription, value); }
        }

        #region Books

        [JsonIgnore]
        public string ScribeName
        {
            get { return GetStringProperty(PropertyString.ScribeName); }
            set { SetStringProperty(PropertyString.ScribeName, value); }
        }

        [JsonIgnore]
        public string ScribeAccount
        {
            get { return GetStringProperty(PropertyString.ScribeAccount); }
            set { SetStringProperty(PropertyString.ScribeAccount, value); }
        }

        [JsonIgnore]
        public uint? ScribeIID
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Scribe); }
            set { SetInstanceIdProperty(PropertyInstanceId.Scribe, value); }
        }

        [JsonIgnore]
        public int? AppraisalPages
        {
            get { return GetIntProperty(PropertyInt.AppraisalPages); }
            set { SetIntProperty(PropertyInt.AppraisalPages, value); }
        }

        [JsonIgnore]
        public int? AppraisalMaxPages
        {
            get { return GetIntProperty(PropertyInt.AppraisalMaxPages); }
            set { SetIntProperty(PropertyInt.AppraisalMaxPages, value); }
        }
        #endregion

        // TODO: This might be wrong place to store the data being stored here.
        [JsonIgnore]
        public int? AvailableCharacter
        {
            get { return GetIntProperty(PropertyInt.AvailableCharacter); }
            set { SetIntProperty(PropertyInt.AvailableCharacter, value); }
        }

        [JsonIgnore]
        public int? Boost
        {
            get { return GetIntProperty(PropertyInt.BoostValue); }
            set { SetIntProperty(PropertyInt.BoostValue, value); }
        }

        [JsonIgnore]
        public int? BoostEnum
        {
            get { return GetIntProperty(PropertyInt.BoosterEnum); }
            set { SetIntProperty(PropertyInt.BoosterEnum, value); }
        }

        [JsonIgnore]
        public double? HealkitMod
        {
            get { return GetDoubleProperty(PropertyDouble.HealkitMod); }
            set { SetDoubleProperty(PropertyDouble.HealkitMod, value); }
        }

        [JsonIgnore]
        public int? CoinValue
        {
            get { return GetIntProperty(PropertyInt.CoinValue); }
            set { SetIntProperty(PropertyInt.CoinValue, value); }
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
            get { return GetDoubleProperty(PropertyDouble.BuyPrice); }
            set { SetDoubleProperty(PropertyDouble.BuyPrice, value); }
        }

        [JsonIgnore]
        public double? SellPrice
        {
            get { return GetDoubleProperty(PropertyDouble.SellPrice); }
            set { SetDoubleProperty(PropertyDouble.SellPrice, value); }
        }

        [JsonIgnore]
        public bool? DealMagicalItems
        {
            get { return GetBoolProperty(PropertyBool.DealMagicalItems); }
            set { SetBoolProperty(PropertyBool.DealMagicalItems, value); }
        }

        [JsonIgnore]
        public uint? AlternateCurrencyDID
        {
            get { return GetDataIdProperty(PropertyDataId.AlternateCurrency); }
            set { SetDataIdProperty(PropertyDataId.AlternateCurrency, value); }
        }

        [JsonIgnore]
        public double? HeartbeatInterval
        {
            get { return GetDoubleProperty(PropertyDouble.HeartbeatInterval); }
            set { SetDoubleProperty(PropertyDouble.HeartbeatInterval, value); }
        }

        [JsonIgnore]
        public int? InitGeneratedObjects
        {
            get { return GetIntProperty(PropertyInt.InitGeneratedObjects); }
            set { SetIntProperty(PropertyInt.InitGeneratedObjects, value); }
        }

        [JsonIgnore]
        public double? RegenerationInterval
        {
            get { return GetDoubleProperty(PropertyDouble.RegenerationInterval); }
            set { SetDoubleProperty(PropertyDouble.RegenerationInterval, value); }
        }

        [JsonIgnore]
        public int? PaletteTemplate
        {
            get { return GetIntProperty(PropertyInt.PaletteTemplate); }
            set { SetIntProperty(PropertyInt.PaletteTemplate, value); }
        }

        [JsonIgnore]
        public double? Shade
        {
            get { return GetDoubleProperty(PropertyDouble.Shade); }
            set { SetDoubleProperty(PropertyDouble.Shade, value); }
        }

        #region Chess
        [JsonIgnore]
        public int? ChessGamesLost
        {
            get { return GetIntProperty(PropertyInt.ChessGamesLost); }
            set { SetIntProperty(PropertyInt.ChessGamesLost, value); }
        }

        [JsonIgnore]
        public int? ChessGamesWon
        {
            get { return GetIntProperty(PropertyInt.ChessGamesWon); }
            set { SetIntProperty(PropertyInt.ChessGamesWon, value); }
        }

        [JsonIgnore]
        public int? ChessRank
        {
            get { return GetIntProperty(PropertyInt.ChessRank); }
            set { SetIntProperty(PropertyInt.ChessRank, value); }
        }

        [JsonIgnore]
        public int? ChessTotalGames
        {
            get { return GetIntProperty(PropertyInt.ChessTotalGames); }
            set { SetIntProperty(PropertyInt.ChessTotalGames, value); }
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

        public CreatureSkill GetSkillProperty(Skill skill)
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

        public int? GetIntProperty(PropertyInt property)
        {
            return IntProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        protected List<int> GetIntProperties(PropertyInt property)
        {
            return IntProperties.Where(x => x.PropertyId == (uint)property).Where(x => x.PropertyValue != null).Select(x => x.PropertyValue.Value).ToList();
        }

        protected void SetIntProperty(PropertyInt intPropertyId, int? value)
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

        public ulong? GetInt64Property(PropertyInt64 property)
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

        public double? GetDoubleProperty(PropertyDouble property)
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

        public string GetStringProperty(PropertyString property)
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
        public List<uint> InventoryWeenieIds
        {
            get { return Inventory.Values.Select(a => a.WeenieClassId).ToList(); }
        }
        
        [JsonIgnore]
        public Dictionary<ObjectGuid, AceObject> WieldedItems = new Dictionary<ObjectGuid, AceObject>();

        [JsonProperty("wieldedWeenieIds")]
        public List<uint> WieldedWeenieIds
        {
            get { return WieldedItems.Values.Select(a => a.WeenieClassId).ToList(); }
        }

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

        public void SetTrackedContract(uint contractId, AceContractTracker value)
        {
            SetProperty(TrackedContracts, contractId, value);
        }

        public AceContractTracker GetTrackedContract(uint contractId)
        {
            AceContractTracker ret;
            bool success = TrackedContracts.TryGetValue(contractId, out ret);
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
