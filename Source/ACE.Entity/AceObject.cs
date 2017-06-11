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
    public class AceObject : ICreatureStats
    {
        protected Dictionary<Ability, CreatureAbility> abilities = new Dictionary<Ability, CreatureAbility>();

        protected Dictionary<Skill, CreatureSkill> skills = new Dictionary<Skill, CreatureSkill>();

        public AceObject(uint id)
            : this()
        {
            AceObjectId = id;
        }

        public AceObject()
        {
            abilities.Add(Ability.Strength, new CreatureAbility(this, Ability.Strength));
            abilities.Add(Ability.Endurance, new CreatureAbility(this, Ability.Endurance));
            abilities.Add(Ability.Coordination, new CreatureAbility(this, Ability.Coordination));
            abilities.Add(Ability.Quickness, new CreatureAbility(this, Ability.Quickness));
            abilities.Add(Ability.Focus, new CreatureAbility(this, Ability.Focus));
            abilities.Add(Ability.Self, new CreatureAbility(this, Ability.Self));

            abilities.Add(Ability.Health, new CreatureAbility(this, Ability.Health));
            abilities.Add(Ability.Stamina, new CreatureAbility(this, Ability.Stamina));
            abilities.Add(Ability.Mana, new CreatureAbility(this, Ability.Mana));
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
        public string CurrentMotionState { get; set; }

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

        public CreatureAbility StrengthAbility
        {
            get { return abilities[Ability.Strength]; }
            set { abilities[Ability.Strength] = value; }
        }

        public CreatureAbility EnduranceAbility
        {
            get { return abilities[Ability.Endurance]; }
            set { abilities[Ability.Endurance] = value; }
        }

        public CreatureAbility CoordinationAbility
        {
            get { return abilities[Ability.Coordination]; }
            set { abilities[Ability.Coordination] = value; }
        }

        public CreatureAbility QuicknessAbility
        {
            get { return abilities[Ability.Quickness]; }
            set { abilities[Ability.Quickness] = value; }
        }

        public CreatureAbility FocusAbility
        {
            get { return abilities[Ability.Focus]; }
            set { abilities[Ability.Focus] = value; }
        }

        public CreatureAbility SelfAbility
        {
            get { return abilities[Ability.Self]; }
            set { abilities[Ability.Self] = value; }
        }

        public CreatureAbility Health
        {
            get { return abilities[Ability.Health]; }
            set { abilities[Ability.Health] = value; }
        }

        public CreatureAbility Stamina
        {
            get { return abilities[Ability.Stamina]; }
            set { abilities[Ability.Stamina] = value; }
        }

        public CreatureAbility Mana
        {
            get { return abilities[Ability.Mana]; }
            set { abilities[Ability.Mana] = value; }
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
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.SharedCooldown)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.SharedCooldown, value); }
        }

        public uint? UiEffects
        {
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.UiEffects)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.UiEffects, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public ushort? HookType
        {
            get { return (ushort?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.HookType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.HookType, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public ushort? HookItemTypes
        {
            get { return (ushort?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.HookItemType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.HookItemType, value); }
        }

        public byte? ItemsCapacity
        {
            get { return (byte?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.ItemsCapacity)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemsCapacity, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? MaterialType
        {
            get { return (byte?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.MaterialType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.MaterialType, value); }
        }

        public ushort? MaxStackSize
        {
            get { return (ushort?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.MaxStackSize)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.MaxStackSize, value); }
        }
        /// <summary>
        /// This is the Maximum an item can hold in the case of salvage 100
        /// everything else healing kits, lock picks etc it is the max number of uses.
        /// </summary>
        public ushort? MaxStructure
        {
            get { return (ushort?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.MaxStructure)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.MaxStructure, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? Radar
        {
            get { return (byte?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.ShowableOnRadar)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ShowableOnRadar, value); }
        }

        public ushort? StackSize
        {
            get { return (ushort?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.StackSize)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.StackSize, value); }
        }
        /// <summary>
        /// This field represents the number of units or uses an item has in it or left.   Salvage in it
        /// healing kits, essences the number left.
        /// </summary>
        public ushort? Structure
        {
            get { return (ushort?)IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.Structure)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.Structure, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? TargetTypeId
        {
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.TargetType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.TargetType, value); }
        }

        public uint? ItemUseable
        {
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.ItemUseable)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemUseable, value); }
        }

        public float? UseRadius
        {
            get { return (float?)DoubleProperties.Find(x => x.PropertyId == (uint)PropertyDouble.UseRadius)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.UseRadius, value); }
        }

        /// <summary>
        /// Left the name as ValidLocations as it is more descriptive of what it does than just locations.
        /// This field maps to EquipMask Enum
        /// </summary>
        public uint? ValidLocations
        {
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.ValidLocations)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ValidLocations, value); }
        }

        public uint? Value
        {
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.Value)?.PropertyValue; }
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
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.ItemWorkmanship)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemWorkmanship, value); }
        }

        public float? PhysicsScriptIntensity
        {
            get { return (float?)DoubleProperties.Find(x => x.PropertyId == (uint)PropertyDouble.PhysicsScriptIntensity)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.PhysicsScriptIntensity, value); }
        }

        public float? Elasticity
        {
            get { return (float?)DoubleProperties.Find(x => x.PropertyId == (uint)PropertyDouble.Elasticity)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.Elasticity, value); }
        }

        public float? Friction
        {
            get { return (float?)DoubleProperties.Find(x => x.PropertyId == (uint)PropertyDouble.Friction)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.Friction, value); }
        }

        public uint? CurrentWieldedLocation
        {
            get { return IntProperties.Find(x => x.PropertyId == (uint)PropertyInt.CurrentWieldedLocation)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.CurrentWieldedLocation, value); }
        }

        public float? DefaultScale
        {
            get { return (float?)DoubleProperties.Find(x => x.PropertyId == (uint)PropertyDouble.DefaultScale)?.PropertyValue; }
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
            get { return (float?)DoubleProperties.Find(x => x.PropertyId == (uint)PropertyDouble.Translucency)?.PropertyValue; }
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

        // TODO: use this for fast loading - needs to be reconciled with creature ability
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

        public CreatureAbility GetAbility(Ability ability)
        {
            if (abilities.ContainsKey(ability))
                return abilities[ability];

            return null;
        }

        public CreatureSkill GetSkill(Skill skill)
        {
            if (!skills.ContainsKey(skill))
            {
                skills.Add(skill, new CreatureSkill(this, skill, SkillStatus.Untrained, 0, 0));
            }

            return skills[skill];
        }

        public void LoadSkills(List<CreatureSkill> newSkills)
        {
            this.skills = new Dictionary<Skill, CreatureSkill>();
            newSkills.ForEach(s => this.skills.Add(s.Skill, s));
        }

        public List<CreatureSkill> GetSkills()
        {
            return this.skills.Select(kvp => kvp.Value).ToList();
        }
    }
}
