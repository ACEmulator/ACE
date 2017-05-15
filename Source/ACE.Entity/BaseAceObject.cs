using System.Collections.Generic;
using ACE.Common;
using MySql.Data.MySqlClient;
using ACE.Entity.Enum.Properties;

namespace ACE.Entity
{
    [DbTable("base_ace_object")]
    [DbGetList("base_ace_object", 15, "typeId")]
    public class BaseAceObject
    {
        [DbField("baseAceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public virtual uint AceObjectId { get; set; }
        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? AmmoType
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.AmmoType)?.PropertyValue;
            }
            set
            {
                var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.AmmoType);
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.AmmoType).PropertyValue = (uint)value;
                else
                    if (listItem != null)
                    AceObjectPropertiesInt.Remove(listItem);
            }
        }
        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? BlipColor
        {
            get
            {
                return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.RadarBlipColor)?.PropertyValue;
            }
            set
            {
                var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.RadarBlipColor);
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.RadarBlipColor).PropertyValue = (byte)value;
                else
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
            }
        }

        public uint? Burden
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.EncumbranceVal)?.PropertyValue;
            }
            set
            {
                var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.EncumbranceVal);
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.EncumbranceVal).PropertyValue = (uint)value;
                else
                    if (listItem != null)
                    AceObjectPropertiesInt.Remove(listItem);
            }
        }
        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? CombatUse
        {
            get
            {
                return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CombatUse)?.PropertyValue;
            }
            set
            {
                var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CombatUse);
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CombatUse).PropertyValue = (byte)value;
                else
                    if (listItem != null)
                    AceObjectPropertiesInt.Remove(listItem);
            }
        }

        public byte? ContainersCapacity
        {
            get
            {
                return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ContainersCapacity)?.PropertyValue;
            }
            set
            {
                var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ContainersCapacity);
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ContainersCapacity).PropertyValue = (byte)value;
                else
                    if (listItem != null)
                    AceObjectPropertiesInt.Remove(listItem);
            }
        }

        public double? CooldownDuration
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyDouble.CooldownDuration)?.PropertyValue;
            }
            set
            {
                var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyDouble.CooldownDuration);
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyDouble.CooldownDuration).PropertyValue = (byte)value;
                else
                    if (listItem != null)
                    AceObjectPropertiesInt.Remove(listItem);
            }
        }

        [DbField("name", (int)MySqlDbType.Text)]
        public string Name { get; set; }

        [DbField("typeId", (int)MySqlDbType.UInt32)]
        public uint TypeId { get; set; }

        [DbField("paletteId", (int)MySqlDbType.UInt32)]
        public uint PaletteId { get; set; }

        [DbField("bitField", (int)MySqlDbType.UInt32)]
        public uint WdescBitField { get; set; }

        [DbField("cooldownId", (int)MySqlDbType.UInt32)]
        public uint CooldownId { get; set; }

        [DbField("effects", (int)MySqlDbType.UInt32)]
        public uint Effects { get; set; }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        [DbField("hookTypeId", (int)MySqlDbType.UInt32)]
        public uint HookTypeId { get; set; }

        [DbField("iconId", (int)MySqlDbType.UInt32)]
        public uint IconId { get; set; }

        [DbField("iconOverlayId", (int)MySqlDbType.UInt32)]
        public uint IconOverlayId { get; set; }

        [DbField("iconUnderlayId", (int)MySqlDbType.UInt32)]
        public uint IconUnderlayId { get; set; }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        [DbField("hookItemTypes", (int)MySqlDbType.UInt32)]
        public uint HookItemTypes { get; set; }

        [DbField("itemsCapacity", (int)MySqlDbType.UByte)]
        public byte ItemsCapacity { get; set; }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        [DbField("materialType", (int)MySqlDbType.UByte)]
        public byte MaterialType { get; set; }

        [DbField("maxStackSize", (int)MySqlDbType.UInt16)]
        public ushort MaxStackSize { get; set; }

        [DbField("maxStructure", (int)MySqlDbType.UInt16)]
        public ushort MaxStructure { get; set; }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        [DbField("radar", (int)MySqlDbType.UByte)]
        public byte Radar { get; set; }

        [DbField("pscript", (int)MySqlDbType.UInt16)]
        public ushort PScript { get; set; }

        [DbField("spellId", (int)MySqlDbType.UInt16)]
        public ushort SpellId { get; set; }

        [DbField("stackSize", (int)MySqlDbType.UInt16)]
        public ushort StackSize { get; set; }

        [DbField("structure", (int)MySqlDbType.UInt16)]
        public ushort Structure { get; set; }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        [DbField("targetTypeId", (int)MySqlDbType.UInt32)]
        public uint TargetTypeId { get; set; }

        [DbField("usability", (int)MySqlDbType.UInt32)]
        public uint Usability { get; set; }

        [DbField("useRadius", (int)MySqlDbType.Float)]
        public float UseRadius { get; set; }

        /// <summary>
        /// TODO: Investigate if this is an enum - i expect it is a flags enum of some sort.
        /// </summary>
        [DbField("validLocations", (int)MySqlDbType.UInt32)]
        public uint ValidLocations { get; set; }

        [DbField("value", (int)MySqlDbType.UInt32)]
        public uint Value { get; set; }

        [DbField("header", (int)MySqlDbType.UInt32)]
        public uint WeenieFlags { get; set; }

        [DbField("workmanship", (int)MySqlDbType.Float)]
        public float Workmanship { get; set; }

        [DbField("animationFrameId", (int)MySqlDbType.UInt32)]
        public uint AnimationFrameId { get; set; }

        [DbField("defaultScript", (int)MySqlDbType.UInt32)]
        public uint DefaultScript { get; set; }

        [DbField("defaultScriptIntensity", (int)MySqlDbType.Float)]
        public float DefaultScriptIntensity { get; set; }

        [DbField("elasticity", (int)MySqlDbType.Float)]
        public float Elasticity { get; set; }

        [DbField("friction", (int)MySqlDbType.Float)]
        public float Friction { get; set; }

        [DbField("locationId", (int)MySqlDbType.UInt32)]
        public uint LocationId { get; set; }
        /// <summary>
        /// PhysicsData.CSetup
        /// </summary>
        [DbField("modelTableId", (int)MySqlDbType.UInt32)]
        public uint ModelTableId { get; set; }

        [DbField("motionTableId", (int)MySqlDbType.UInt32)]
        public uint MotionTableId { get; set; }

        [DbField("objectScale", (int)MySqlDbType.Float)]
        public float ObjectScale { get; set; }

        [DbField("physicsBitField", (int)MySqlDbType.UInt32)]
        public uint PhysicsBitField { get; set; }

        /// <summary>
        /// TODO: convert to enum, probably a flags enum
        /// </summary>
        [DbField("physicsState", (int)MySqlDbType.UInt32)]
        public uint PhysicsState { get; set; }

        [DbField("physicsTableId", (int)MySqlDbType.UInt32)]
        public uint PhysicsTableId { get; set; }

        [DbField("soundTableId", (int)MySqlDbType.UInt32)]
        public uint SoundTableId { get; set; }

        [DbField("translucency", (int)MySqlDbType.Float)]
        public float Translucency { get; set; }

        [DbField("currentMotionState", (int)MySqlDbType.Text)]
        public string CurrentMotionState { get; set; }

        public List<PaletteOverride> PaletteOverrides { get; set; } = new List<PaletteOverride>();

        public List<TextureMapOverride> TextureOverrides { get; set; } = new List<TextureMapOverride>();

        public List<AnimationOverride> AnimationOverrides { get; set; } = new List<AnimationOverride>();

        public List<AceObjectPropertiesInt> AceObjectPropertiesInt { get; set; } = new List<AceObjectPropertiesInt>();

        public List<AceObjectPropertiesBigInt> AceObjectPropertiesBigInt { get; set; } = new List<AceObjectPropertiesBigInt>();

        public List<AceObjectPropertiesDouble> AceObjectPropertiesDouble { get; set; } = new List<AceObjectPropertiesDouble>();

        public List<AceObjectPropertiesBool> AceObjectPropertiesBool { get; set; } = new List<AceObjectPropertiesBool>();

        public List<AceObjectPropertiesString> AceObjectPropertiesString { get; set; } = new List<AceObjectPropertiesString>();
    }
}
