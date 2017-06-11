using System.Collections.Generic;
using ACE.Common;
using MySql.Data.MySqlClient;
using ACE.Entity.Enum.Properties;
using System;

namespace ACE.Entity
{
    using Enum;
    using System.Linq;
    using System.Net.Mime;

    [DbTable("ace_object")]
    [DbList("ace_object", "landblock")]
    public class AceObject : ICreatureStats, ICloneable
    {
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
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
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
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.PlacementPosition)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.PlacementPosition, value); }
        }

        [DbField("currentMotionState", (int)MySqlDbType.Text)]
        public String CurrentMotionState { get; set; }

        public uint? IconId
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.Icon)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.Icon, value); }
        }

        public uint? IconOverlayId
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.IconOverlay)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.IconOverlay, value); }
        }

        public uint? IconUnderlayId
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.IconUnderlay)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.IconUnderlay, value); }
        }

        /// <summary>
        /// PhysicsData.CSetup
        /// </summary>
        public uint? ModelTableId
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.Setup)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.Setup, value); }
        }

        public uint? MotionTableId
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.MotionTable)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.MotionTable, value); }
        }

        public ushort? PhysicsScript
        {
            get { return (ushort?)DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.PhysicsScript)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.PhysicsScript, value); }
        }

        public uint? PhysicsTableId
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.PhysicsEffectTable)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.PhysicsEffectTable, value); }
        }

        public uint? SoundTableId
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.SoundTable)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.SoundTable, value); }
        }

        public ushort? SpellId
        {
            get { return (ushort?)DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.Spell)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.Spell, value); }
        }

        public uint? DefaultScript
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.UseUserAnimation)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.UseUserAnimation, value); }
        }

        public AceObjectPropertiesAttribute StrengthAbility
        {
            get { return GetAttributeProperty(Ability.Strength); }
            set { SetAttributeProperty(value); }
        }

        public AceObjectPropertiesAttribute EnduranceAbility
        {
            get { return GetAttributeProperty(Ability.Endurance); }
            set { SetAttributeProperty(value); }
        }

        public AceObjectPropertiesAttribute CoordinationAbility
        {
            get { return GetAttributeProperty(Ability.Coordination); }
            set { SetAttributeProperty(value); }
        }

        public AceObjectPropertiesAttribute QuicknessAbility
        {
            get { return GetAttributeProperty(Ability.Quickness); }
            set { SetAttributeProperty(value); }
        }

        public AceObjectPropertiesAttribute FocusAbility
        {
            get { return GetAttributeProperty(Ability.Focus); }
            set { SetAttributeProperty(value); }
        }

        public AceObjectPropertiesAttribute SelfAbility
        {
            get { return GetAttributeProperty(Ability.Self); }
            set { SetAttributeProperty(value); }
        }

        public AceObjectPropertiesAttribute2nd Health
        {
            get { return GetAttribute2ndProperty(Ability.Health); }
            set { SetAttribute2ndProperty(value); }
        }

        public AceObjectPropertiesAttribute2nd Stamina
        {
            get { return GetAttribute2ndProperty(Ability.Stamina); }
            set { SetAttribute2ndProperty(value); }
        }

        public AceObjectPropertiesAttribute2nd Mana
        {
            get { return GetAttribute2ndProperty(Ability.Mana); }
            set { SetAttribute2ndProperty(value); }
        }

        public uint Strength
        { get { return StrengthAbility.ActiveValue; } }

        public uint Endurance
        { get { return EnduranceAbility.ActiveValue; } }

        public uint Coordination
        { get { return CoordinationAbility.ActiveValue; } }

        public uint Quickness
        { get { return QuicknessAbility.ActiveValue; } }

        public uint Focus
        { get { return FocusAbility.ActiveValue; } }

        public uint Self
        { get { return SelfAbility.ActiveValue; } }

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

        public uint CombatTableId
        {
            get { return GetDataIdProperty(PropertyDataId.CombatTable) ?? 0; }
            set { SetDataIdProperty(PropertyDataId.CombatTable, value); }
        }

        public List<WeeniePaletteOverride> WeeniePaletteOverrides { get; set; } = new List<WeeniePaletteOverride>();

        public List<WeenieTextureMapOverride> WeenieTextureMapOverrides { get; set; } = new List<WeenieTextureMapOverride>();

        public List<WeenieAnimationOverride> WeenieAnimationOverrides { get; set; } = new List<WeenieAnimationOverride>();

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
        public uint ItemType
        {
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.ItemType).PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemType, value); }
        }

        // public uint? PaletteId
        // {
        //    get { return GetIntProperty(PropertyInt.PaletteTemplate).Value; }
        //    set { SetIntProperty(PropertyInt.PaletteTemplate, value); }
        // }

        public uint? PaletteId
        {
            get { return DataIdProperties.Find(x => x.PropertyId == (uint)PropertyDataId.PaletteBase)?.PropertyValue; }
            set { SetDataIdProperty(PropertyDataId.PaletteBase, value); }
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
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.PhysicsState).PropertyValue; }
            set { SetIntProperty(PropertyInt.PhysicsState, value); }
        }

        public float? Translucency
        {
            get { return (float?)GetDoubleProperty(PropertyDouble.Translucency); }
            set { SetDoubleProperty(PropertyDouble.Translucency, value); }
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

        public AceObjectPropertiesAttribute GetAttributeProperty(Ability ability)
        {
            var ret = AceObjectPropertiesAttributes.FirstOrDefault(x => x.AttributeId == (uint)ability);

            if (ret == null)
            {
                ret = new AceObjectPropertiesAttribute();
                ret.AceObjectId = AceObjectId;
                ret.AttributeId = (ushort)ability;
                ret.AttributeBase = 0;
                ret.AttributeRanks = 0;
                ret.AttributeXpSpent = 0;
                AceObjectPropertiesAttributes.Add(ret);
            }

            return ret;
        }

        public void SetAttributeProperty(AceObjectPropertiesAttribute attribute)
        {
            AceObjectPropertiesAttribute oldAttribute = GetAttributeProperty((Ability)attribute.AttributeId);
            if (attribute != null)
            {
                if (oldAttribute != null)
                {
                    oldAttribute.AttributeBase = attribute.AttributeBase;
                    oldAttribute.AttributeRanks = attribute.AttributeRanks;
                    oldAttribute.AttributeXpSpent = attribute.AttributeXpSpent;
                }
                else
                {
                    AceObjectPropertiesAttributes.Add(attribute);
                }
            }
            else
            {
                if (oldAttribute != null)
                {
                    AceObjectPropertiesAttributes.Remove(oldAttribute);
                }
            }
        }

        public AceObjectPropertiesAttribute2nd GetAttribute2ndProperty(Ability ability)
        {
            var ret = AceObjectPropertiesAttributes2nd.FirstOrDefault(x => x.Attribute2ndId == (uint)ability);

            if (ret == null)
            {
                ret = new AceObjectPropertiesAttribute2nd();
                ret.AceObjectId = AceObjectId;
                ret.Attribute2ndId = (ushort)ability;
                ret.Attribute2ndValue = 0;
                ret.Attribute2ndRanks = 0;
                ret.Attribute2ndXpSpend = 0;
                AceObjectPropertiesAttributes2nd.Add(ret);
            }

            return ret;
        }

        public void SetAttribute2ndProperty(AceObjectPropertiesAttribute2nd attribute)
        {
            AceObjectPropertiesAttribute2nd oldAttribute = GetAttribute2ndProperty((Ability)attribute.Attribute2ndId);
            if (attribute != null)
            {
                if (oldAttribute != null)
                {
                    oldAttribute.Attribute2ndValue = attribute.Attribute2ndValue;
                    oldAttribute.Attribute2ndRanks = attribute.Attribute2ndRanks;
                    oldAttribute.Attribute2ndXpSpend = attribute.Attribute2ndXpSpend;
                }
                else
                {
                    AceObjectPropertiesAttributes2nd.Add(attribute);
                }
            }
            else
            {
                if (oldAttribute != null)
                {
                    AceObjectPropertiesAttributes2nd.Remove(oldAttribute);
                }
            }
        }

        public AceObjectPropertiesSkill GetSkillProperty(Skill skill)
        {
            var ret = AceObjectPropertiesSkills.FirstOrDefault(x => x.SkillId == (uint)skill);

            if (ret == null)
            {
                ret = new AceObjectPropertiesSkill();
                ret.AceObjectId = AceObjectId;
                ret.SkillId = (ushort)skill;
                ret.SkillPoints = 0;
                ret.SkillStatus = (ushort)SkillStatus.Untrained;
                ret.SkillXpSpent = 0;
                AceObjectPropertiesSkills.Add(ret);
            }

            return ret;
        }

        public void SetSkillProperty(AceObjectPropertiesSkill skill)
        {
            AceObjectPropertiesSkill oldSkill = GetSkillProperty((Skill)skill.SkillId);
            if (skill != null)
            {
                if (oldSkill != null)
                {
                    oldSkill.SkillId = skill.SkillId;
                    oldSkill.SkillPoints = skill.SkillPoints;
                    oldSkill.SkillStatus = skill.SkillStatus;
                    oldSkill.SkillXpSpent = skill.SkillXpSpent;
                }
                else
                {
                    AceObjectPropertiesSkills.Add(skill);
                }
            }
            else
            {
                if (oldSkill != null)
                {
                    AceObjectPropertiesSkills.Remove(skill);
                }
            }
        }

        public List<AceObjectPropertiesSkill> GetSkills()
        {
            return AceObjectPropertiesSkills;
        }

        public void SetAceObjectPropertiesSkill(AceObjectPropertiesSkill skill)
        {
            AceObjectPropertiesSkill oldSkill = GetSkillProperty((Skill)skill.SkillId);
            if (skill != null)
            {
                if (oldSkill != null)
                {
                    oldSkill.SkillPoints = skill.SkillPoints;
                    oldSkill.SkillStatus = skill.SkillStatus;
                    oldSkill.SkillXpSpent = skill.SkillXpSpent;
                }
                else
                {
                    AceObjectPropertiesSkills.Add(skill);
                }
            }
            else
            {
                if (oldSkill != null)
                {
                    AceObjectPropertiesSkills.Remove(oldSkill);
                }
            }
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
                    listItem.PropertyValue = (uint)value;
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

        public List<AceObjectPropertiesString> StringProperties { get; set; } = new List<AceObjectPropertiesString>();

        public List<AceObjectPropertiesAttribute> AceObjectPropertiesAttributes { get; set; } = new List<AceObjectPropertiesAttribute>();

        // ReSharper disable once InconsistentNaming
        public List<AceObjectPropertiesAttribute2nd> AceObjectPropertiesAttributes2nd { get; set; } = new List<AceObjectPropertiesAttribute2nd>();

        public List<AceObjectPropertiesSkill> AceObjectPropertiesSkills { get; set; } = new List<AceObjectPropertiesSkill>();

        public Dictionary<PositionType, Position> Positions { get; set; } = new Dictionary<PositionType, Position>();

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

        protected Position GetPosition(PositionType positionType)
        {
            if (Positions.ContainsKey(positionType))
            {
                return Positions[positionType];
            }

            return null;
        }

        protected void SetPosition(PositionType positionType, Position position)
        {
            Positions[positionType] = position;
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
            ret.AceObjectPropertiesAttributes = CloneList(AceObjectPropertiesAttributes);
            ret.AceObjectPropertiesAttributes2nd = CloneList(AceObjectPropertiesAttributes2nd);
            ret.AceObjectPropertiesSkills = CloneList(AceObjectPropertiesSkills);
            var posList = CloneList(Positions.Values);
            foreach (var pos in posList)
            {
                ret.Positions[pos.PositionType] = pos;
            }

            return ret;
        }

        private static List<T> CloneList<T>(IEnumerable<T> toClone) where T : ICloneable
        {
            return toClone.Select(x => (T)x.Clone()).ToList();
        }
    }
}
