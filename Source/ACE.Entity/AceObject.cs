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
    public class AceObject : ICreatureStats, ICloneable
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

        /// <summary>
        /// Table field Primary Key
        /// </summary>
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true, ListGet = true, ListDelete = true)]
        public virtual uint AceObjectId { get; set; }

        /// <summary>
        /// Table Field Weenie Class
        /// </summary>
        [DbField("weenieClassId", (int)MySqlDbType.UInt32)]
        public virtual uint WeenieClassId { get; set; }

        /// <summary>
        /// Table Field Flags
        /// </summary>
        [DbField("aceObjectDescriptionFlags", (int)MySqlDbType.UInt32)]
        public uint AceObjectDescriptionFlags { get; set; }

        /// <summary>
        /// Table Field - Flags
        /// </summary>
        [DbField("physicsDescriptionFlag", (int)MySqlDbType.UInt32)]
        public uint PhysicsDescriptionFlag { get; set; }

        /// <summary>
        /// Table Field - Flags
        /// </summary>
        [DbField("weenieHeaderFlags", (int)MySqlDbType.UInt32)]
        public uint WeenieHeaderFlags { get; set; }

        public uint? AnimationFrameId
        {
            get { return GetIntProperty(PropertyInt.PlacementPosition); }
            set { SetIntProperty(PropertyInt.PlacementPosition, value); }
        }

        public uint? Priority
        {
            get { return GetIntProperty(PropertyInt.ClothingPriority); }
            set { SetIntProperty(PropertyInt.ClothingPriority, value); }
        }

        [DbField("currentMotionState", (int)MySqlDbType.Text)]
        public string CurrentMotionState { get; set; }

        public uint? IconId
        {
            get { return GetDataIdProperty(PropertyDataId.Icon); }
            set { SetDataIdProperty(PropertyDataId.Icon, value); }
        }

        public uint? IconOverlayId
        {
            get { return GetDataIdProperty(PropertyDataId.IconOverlay); }
            set { SetDataIdProperty(PropertyDataId.IconOverlay, value); }
        }

        public uint? IconUnderlayId
        {
            get { return GetDataIdProperty(PropertyDataId.IconUnderlay); }
            set { SetDataIdProperty(PropertyDataId.IconUnderlay, value); }
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

        public ushort? SpellId
        {
            get { return (ushort?)GetDataIdProperty(PropertyDataId.Spell); }
            set { SetDataIdProperty(PropertyDataId.Spell, value); }
        }

        public uint? DefaultScript
        {
            get { return GetDataIdProperty(PropertyDataId.UseUserAnimation); }
            set { SetDataIdProperty(PropertyDataId.UseUserAnimation, value); }
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

        public byte LuminanceAward
        {
            get { return (byte?)GetIntProperty(PropertyInt.LuminanceAward) ?? 0x0; }
            set { SetIntProperty(PropertyInt.LuminanceAward, value); }
        }

        public byte LootTier
        {
            get { return (byte?)GetIntProperty(PropertyInt.LootTier) ?? 0; }
            set { SetIntProperty(PropertyInt.LootTier, value); }
        }

        public uint Level
        {
            get { return GetIntProperty(PropertyInt.Level) ?? 0; }
            set { SetIntProperty(PropertyInt.Level, value); }
        }

        public uint GeneratorTimeType
        {
            get { return GetIntProperty(PropertyInt.GeneratorTimeType) ?? 0; }
            set { SetIntProperty(PropertyInt.GeneratorTimeType, value); }
        }

        public uint GeneratorProbability
        {
            get { return GetIntProperty(PropertyInt.GeneratorProbability) ?? 0; }
            set { SetIntProperty(PropertyInt.GeneratorProbability, value); }
        }

        public uint MaxGeneratedObjects
        {
            get { return GetIntProperty(PropertyInt.MaxGeneratedObjects) ?? 0; }
            set { SetIntProperty(PropertyInt.MaxGeneratedObjects, value); }
        }

        public uint GeneratorType
        {
            get { return GetIntProperty(PropertyInt.GeneratorType) ?? 0; }
            set { SetIntProperty(PropertyInt.GeneratorType, value); }
        }

        public uint ActivationCreateClass
        {
            get { return GetIntProperty(PropertyInt.ActivationCreateClass) ?? 0; }
            set { SetIntProperty(PropertyInt.ActivationCreateClass, value); }
        }

        public uint CombatTableId
        {
            get { return GetDataIdProperty(PropertyDataId.CombatTable) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.CombatTable, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? AmmoType
        {
            get { return GetIntProperty(PropertyInt.AmmoType); }
            set { SetIntProperty(PropertyInt.AmmoType, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? BlipColor
        {
            get { return (byte?)GetIntProperty(PropertyInt.RadarBlipColor); }
            set { SetIntProperty(PropertyInt.RadarBlipColor, value); }
        }

        public ushort? Burden
        {
            get { return (ushort?)GetIntProperty(PropertyInt.EncumbranceVal); }
            set { SetIntProperty(PropertyInt.EncumbranceVal, value); }
        }
        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? CombatUse
        {
            get { return (byte?)GetIntProperty(PropertyInt.CombatUse); }
            set { SetIntProperty(PropertyInt.CombatUse, value); }
        }

        public virtual byte? ContainersCapacity
        {
            get { return (byte?)GetIntProperty(PropertyInt.ContainersCapacity); }
            set { SetIntProperty(PropertyInt.ContainersCapacity, value); }
        }

        public double? CooldownDuration
        {
            get { return GetDoubleProperty(PropertyDouble.CooldownDuration); }
            set { SetDoubleProperty(PropertyDouble.CooldownDuration, value); }
        }

        public string Name
        {
            get { return GetStringProperty(PropertyString.Name); }
            set { SetStringProperty(PropertyString.Name, value); }
        }

        /// <summary>
        /// will throw if not null!
        /// </summary>
        public uint Type
        {
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.ItemType).PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemType, value); }
        }

        public uint? PaletteId
        {
            get { return GetDataIdProperty(PropertyDataId.PaletteBase); }
            set { SetDataIdProperty(PropertyDataId.PaletteBase, value); }
        }

        public uint? ClothingBase
        {
            get { return GetDataIdProperty(PropertyDataId.Clothingbase); }
            set { SetDataIdProperty(PropertyDataId.Clothingbase, value); }
        }

        // TODO: Not sure if this enum is right.
        public uint? CooldownId
        {
            get { return GetIntProperty(PropertyInt.SharedCooldown); }
            set { SetIntProperty(PropertyInt.SharedCooldown, value); }
        }

        public uint? UiEffects
        {
            get { return GetIntProperty(PropertyInt.UiEffects); }
            set { SetIntProperty(PropertyInt.UiEffects, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public ushort? HookType
        {
            get { return (ushort?)GetIntProperty(PropertyInt.HookType); }
            set { SetIntProperty(PropertyInt.HookType, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public ushort? HookItemTypes
        {
            get { return (ushort?)GetIntProperty(PropertyInt.HookItemType); }
            set { SetIntProperty(PropertyInt.HookItemType, value); }
        }

        public byte? ItemsCapacity
        {
            get { return (byte?)GetIntProperty(PropertyInt.ItemsCapacity); }
            set { SetIntProperty(PropertyInt.ItemsCapacity, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? MaterialType
        {
            get { return (byte?)GetIntProperty(PropertyInt.MaterialType); }
            set { SetIntProperty(PropertyInt.MaterialType, value); }
        }

        public ushort? MaxStackSize
        {
            get { return (ushort?)GetIntProperty(PropertyInt.MaxStackSize); }
            set { SetIntProperty(PropertyInt.MaxStackSize, value); }
        }

        public uint? WielderId
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Wielder); }
            set { SetInstanceIdProperty(PropertyInstanceId.Wielder, value); }
        }

        public uint? ContainerId
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Container); }
            set { SetInstanceIdProperty(PropertyInstanceId.Container, value); }
        }

        /// <summary>
        /// This is the Maximum an item can hold in the case of salvage 100
        /// everything else healing kits, lock picks etc it is the max number of uses.
        /// </summary>
        public ushort? MaxStructure
        {
            get { return (ushort?)GetIntProperty(PropertyInt.MaxStructure); }
            set { SetIntProperty(PropertyInt.MaxStructure, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? Radar
        {
            get { return (byte?)GetIntProperty(PropertyInt.ShowableOnRadar); }
            set { SetIntProperty(PropertyInt.ShowableOnRadar, value); }
        }

        public ushort? StackSize
        {
            get { return (ushort?)GetIntProperty(PropertyInt.StackSize); }
            set { SetIntProperty(PropertyInt.StackSize, value); }
        }

        public uint? DefaultCombatStyle
        {
            get { return GetIntProperty(PropertyInt.DefaultCombatStyle); }
            set { SetIntProperty(PropertyInt.DefaultCombatStyle, value); }
        }
        /// <summary>
        /// This field represents the number of units or uses an item has in it or left.   Salvage in it
        /// healing kits, essences the number left.
        /// </summary>
        public ushort? Structure
        {
            get { return (ushort?)GetIntProperty(PropertyInt.Structure); }
            set { SetIntProperty(PropertyInt.Structure, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? TargetTypeId
        {
            get { return GetIntProperty(PropertyInt.TargetType); }
            set { SetIntProperty(PropertyInt.TargetType, value); }
        }

        public uint? ItemUseable
        {
            get { return GetIntProperty(PropertyInt.ItemUseable); }
            set { SetIntProperty(PropertyInt.ItemUseable, value); }
        }

        public float? UseRadius
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.UseRadius); }
            set { SetDoubleProperty(PropertyDouble.UseRadius, value); }
        }

        /// <summary>
        /// Left the name as ValidLocations as it is more descriptive of what it does than just locations.
        /// This field maps to EquipMask Enum
        /// </summary>
        public uint? ValidLocations
        {
            get { return GetIntProperty(PropertyInt.ValidLocations); }
            set { SetIntProperty(PropertyInt.ValidLocations, value); }
        }

        public uint? Value
        {
            get { return GetIntProperty(PropertyInt.Value); }
            set { SetIntProperty(PropertyInt.Value, value); }
        }

        public float Workmanship
        {
            get
            {
                if ((ItemWorkmanship != null) && (Structure != null) && (Structure != 0))
                {
                    return (float)Convert.ToDouble(ItemWorkmanship / (10000 * Structure));
                }
                return (ItemWorkmanship ?? 0.0f);
            }
            set
            {
                if ((Structure != null) && (Structure != 0))
                {
                    ItemWorkmanship = (uint)Convert.ToInt32(value * 10000 * Structure);
                }
                else
                {
                    ItemWorkmanship = (uint)Convert.ToInt32(value);
                }
            }
        }

        private uint? ItemWorkmanship
        {
            get { return GetIntProperty(PropertyInt.ItemWorkmanship); }
            set { SetIntProperty(PropertyInt.ItemWorkmanship, value); }
        }

        public float? PhysicsScriptIntensity
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.PhysicsScriptIntensity); }
            set { SetDoubleProperty(PropertyDouble.PhysicsScriptIntensity, value); }
        }

        public float? Elasticity
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Elasticity); }
            set { SetDoubleProperty(PropertyDouble.Elasticity, value); }
        }

        public float? Friction
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Friction); }
            set { SetDoubleProperty(PropertyDouble.Friction, value); }
        }

        public uint? CurrentWieldedLocation
        {
            get { return GetIntProperty(PropertyInt.CurrentWieldedLocation); }
            set { SetIntProperty(PropertyInt.CurrentWieldedLocation, value); }
        }

        public uint? Parent
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

        /// <summary>
        /// TODO: convert to enum, probably a flags enum
        /// </summary>
        public uint PhysicsState
        {
            get { return GetIntProperty(PropertyInt.PhysicsState) ?? default(int); }
            set { SetIntProperty(PropertyInt.PhysicsState, value); }
        }

        public float? Translucency
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Translucency); }
            set { SetDoubleProperty(PropertyDouble.Translucency, value); }
        }

        public uint? Generator
        {
            get { return GetInstanceIdProperty(PropertyInstanceId.Generator); }
            set { SetInstanceIdProperty(PropertyInstanceId.Generator, value); }
        }

        public uint? GetDataIdProperty(PropertyDataId property)
        {
            return DataIdProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        public List<uint> GetDataIdProperties(PropertyDataId property)
        {
            return DataIdProperties.Where(x => x.PropertyId == (uint)property).Select(x => x.PropertyValue).ToList();
        }

        public void SetDataIdProperty(PropertyDataId didPropertyId, uint? value)
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
                {
                    DataIdProperties.Remove(listItem);
                }
            }
        }

        public bool? GetBoolProperty(PropertyBool property)
        {
            return BoolProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        public List<bool> GetBoolProperties(PropertyBool property)
        {
            return BoolProperties.Where(x => x.PropertyId == (uint)property).Select(x => x.PropertyValue).ToList();
        }

        public void SetBoolProperty(PropertyBool propertyId, bool? value)
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
                {
                    BoolProperties.Remove(listItem);
                }
            }
        }

        public uint? GetInstanceIdProperty(PropertyInstanceId property)
        {
            return InstanceIdProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        public List<uint> GetInstanceIdProperties(PropertyInstanceId property)
        {
            return InstanceIdProperties.Where(x => x.PropertyId == (uint)property).Select(x => x.PropertyValue).ToList();
        }

        public void SetInstanceIdProperty(PropertyInstanceId iidPropertyId, uint? value)
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
                {
                    InstanceIdProperties.Remove(listItem);
                }
            }
        }

        public CreatureAbility GetAttributeProperty(Ability ability)
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

        private void SetProperty<K, V>(Dictionary<K, V> dict, K key, V value) {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        public void SetAttributeProperty(Ability ability, CreatureAbility value)
        {
            SetProperty(AceObjectPropertiesAttributes, ability, value);
        }

        public CreatureVital GetAttribute2ndProperty(Ability ability)
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

        public void SetAttribute2ndProperty(Ability ability, CreatureVital value)
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

        public void SetSkillProperty(Skill skill, CreatureSkill value)
        {
            SetProperty(AceObjectPropertiesSkills, skill, value);
        }

        public List<CreatureSkill> GetSkills()
        {
            return AceObjectPropertiesSkills.Values.ToList();
        }

        public uint? GetIntProperty(PropertyInt property)
        {
            return IntProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        public List<uint> GetIntProperties(PropertyInt property)
        {
            return IntProperties.Where(x => x.PropertyId == (uint)property).Select(x => x.PropertyValue).ToList();
        }

        public void SetIntProperty(PropertyInt intPropertyId, uint? value)
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
                {
                    IntProperties.Remove(listItem);
                }
            }
        }

        public ulong? GetInt64Property(PropertyInt64 property)
        {
            return Int64Properties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        public List<ulong> GetInt64Properties(PropertyInt64 property)
        {
            return Int64Properties.Where(x => x.PropertyId == (ushort)property).Select(x => x.PropertyValue).ToList();
        }

        public void SetInt64Property(PropertyInt64 int64PropertyId, ulong? value)
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
                    Int64Properties.Remove(listItem);
            }
        }

        public double? GetDoubleProperty(PropertyDouble property)
        {
            return DoubleProperties.FirstOrDefault(x => x.PropertyId == (ushort)property)?.PropertyValue;
        }

        public List<double> GetDoubleProperties(PropertyDouble property)
        {
            return DoubleProperties.Where(x => x.PropertyId == (ushort)property).Select(x => x.PropertyValue).ToList();
        }

        public void SetDoubleTimestamp(PropertyDouble propertyId)
        {
            TimeSpan span = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            double timestamp = span.TotalSeconds;
            SetDoubleProperty(propertyId, timestamp);
        }

        public void SetDoubleProperty(PropertyDouble propertyId, double? value)
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
                    DoubleProperties.Remove(listItem);
            }
        }

        public string GetStringProperty(PropertyString property)
        {
            return StringProperties.FirstOrDefault(x => x.PropertyId == (uint)property)?.PropertyValue;
        }

        public List<string> GetStringProperties(PropertyString property)
        {
            return StringProperties.Where(x => x.PropertyId == (uint)property).Select(x => x.PropertyValue).ToList();
        }

        public void SetStringProperty(PropertyString propertyId, string value)
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
                {
                    StringProperties.Remove(listItem);
                }
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

        public List<AceObjectPropertiesString> StringProperties { get; set; } = new List<AceObjectPropertiesString>();

        public Dictionary<Ability, CreatureAbility> AceObjectPropertiesAttributes { get; set; } = new Dictionary<Ability, CreatureAbility>();

        // ReSharper disable once InconsistentNaming
        public Dictionary<Ability, CreatureVital> AceObjectPropertiesAttributes2nd { get; set; } = new Dictionary<Ability, CreatureVital>();

        public Dictionary<Skill, CreatureSkill> AceObjectPropertiesSkills { get; set; } = new Dictionary<Skill, CreatureSkill>();

        public Dictionary<PositionType, Position> AceObjectPropertiesPositions { get; set; } = new Dictionary<PositionType, Position>();

        public Position Destination
        {
            get { return GetPosition(PositionType.Destination); }
            set { SetPosition(PositionType.Destination, value); }
        }

        public Position Location
        {
            get { return GetPosition(PositionType.Location); }
            set { SetPosition(PositionType.Location, value); }
        }

        public Position Instantiation
        {
            get { return GetPosition(PositionType.Instantiation); }
            set { SetPosition(PositionType.Instantiation, value); }
        }

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
            ret.AceObjectPropertiesAttributes = CloneDict(AceObjectPropertiesAttributes);
            ret.AceObjectPropertiesAttributes2nd = CloneDict(AceObjectPropertiesAttributes2nd);
            ret.AceObjectPropertiesSkills = CloneDict(AceObjectPropertiesSkills);
            ret.AceObjectPropertiesPositions = CloneDict(AceObjectPropertiesPositions);
            ret.SpellIdProperties = CloneList(SpellIdProperties);

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
            return ret;
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
