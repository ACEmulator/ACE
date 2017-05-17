using System.Collections.Generic;
using ACE.Common;
using MySql.Data.MySqlClient;
using ACE.Entity.Enum.Properties;

namespace ACE.Entity
{
    using System;
    using System.Diagnostics.Eventing.Reader;

    [DbTable("ace_object")]
    [DbGetList("ace_object", 15, "typeId")]
    public class BaseAceObject
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public virtual uint AceObjectId { get; set; }

        [DbField("aceObjectDescriptionFlags", (int)MySqlDbType.UInt32)]
        public uint AceObjectDescriptionFlags { get; set; }

        [DbField("animationFrameId", (int)MySqlDbType.UInt32)]
        public uint AnimationFrameId { get; set; }

        [DbField("currentMotionState", (int)MySqlDbType.Text)]
        public string CurrentMotionState { get; set; }

        [DbField("iconId", (int)MySqlDbType.UInt32)]
        public uint IconId { get; set; }

        [DbField("iconOverlayId", (int)MySqlDbType.UInt32)]
        public uint IconOverlayId { get; set; }

        [DbField("iconUnderlayId", (int)MySqlDbType.UInt32)]
        public uint IconUnderlayId { get; set; }
        /// <summary>
        /// PhysicsData.CSetup
        /// </summary>
        [DbField("modelTableId", (int)MySqlDbType.UInt32)]
        public uint ModelTableId { get; set; }

        [DbField("motionTableId", (int)MySqlDbType.UInt32)]
        public uint MotionTableId { get; set; }

        [DbField("physicsDescriptionFlag", (int)MySqlDbType.UInt32)]
        public uint PhysicsDescriptionFlag { get; set; }

        [DbField("playScript", (int)MySqlDbType.UInt16)]
        public ushort PlayScript { get; set; }

        [DbField("physicsTableId", (int)MySqlDbType.UInt32)]
        public uint PhysicsTableId { get; set; }

        [DbField("soundTableId", (int)MySqlDbType.UInt32)]
        public uint SoundTableId { get; set; }

        [DbField("weenieHeaderFlags", (int)MySqlDbType.UInt32)]
        public uint WeenieHeaderFlags { get; set; }

        [DbField("spellId", (int)MySqlDbType.UInt16)]
        public ushort SpellId { get; set; }

        [DbField("defaultScript", (int)MySqlDbType.UInt32)]
        public uint DefaultScript { get; set; }

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
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.AmmoType).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.AmmoType);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
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
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.RadarBlipColor).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.RadarBlipColor);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
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
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.EncumbranceVal).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.EncumbranceVal);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
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
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CombatUse).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CombatUse);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
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
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ContainersCapacity).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ContainersCapacity);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public double? CooldownDuration
        {
            get
            {
                return AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.CooldownDuration)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.CooldownDuration).PropertyValue = (double)value;
                else
                {
                    var listItem = AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.CooldownDuration);
                    if (listItem != null)
                        AceObjectPropertiesDouble.Remove(listItem);
                }
            }
        }

        public string Name
        {
            get
            {
                return AceObjectPropertiesString.Find(x => x.StrPropertyId == (uint)PropertyString.Name)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesString.Find(x => x.StrPropertyId == (uint)PropertyString.Name).PropertyValue = value;
                else
                {
                    var listItem = AceObjectPropertiesString.Find(x => x.StrPropertyId == (uint)PropertyString.Name);
                    if (listItem != null)
                        AceObjectPropertiesString.Remove(listItem);
                }
            }
        }

        public uint? TypeId
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemType)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemType).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemType);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public uint? PaletteId
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PaletteTemplate)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PaletteTemplate).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PaletteTemplate);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        // TODO: Not sure if this enum is right.
        public uint? CooldownId
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.SharedCooldown)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.SharedCooldown).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.SharedCooldown);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public uint? UiEffects
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.UiEffects)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.UiEffects).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.UiEffects);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? HookType
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.HookType)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.HookType).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.HookType);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? HookItemTypes
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.HookItemType)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.HookItemType).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.HookItemType);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public byte? ItemsCapacity
        {
            get
            {
                return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemsCapacity)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemsCapacity).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemsCapacity);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? MaterialType
        {
            get
            {
                return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaterialType)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaterialType).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaterialType);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public ushort? MaxStackSize
        {
            get
            {
                return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaxStackSize)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaxStackSize).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaxStackSize);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }
        /// <summary>
        /// This is the Maximum an item can hold in the case of salvage 100
        /// everything else healing kits, lock picks etc it is the max number of uses.
        /// </summary>
        public ushort? MaxStructure
        {
            get
            {
                return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaxStructure)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaxStructure).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaxStructure);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? Radar
        {
            get
            {
                return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ShowableOnRadar)?.PropertyValue;
            }
            set
            {
                var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ShowableOnRadar);
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ShowableOnRadar).PropertyValue = (uint)value;
                else
                    if (listItem != null)
                    AceObjectPropertiesInt.Remove(listItem);
            }
        }

        public ushort? StackSize
        {
            get
            {
                return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.StackSize)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.StackSize).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.StackSize);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }
        /// <summary>
        /// This field represents the number of units or uses an item has in it or left.   Salvage in it
        /// healing kits, essences the number left.
        /// </summary>
        public ushort? Structure
        {
            get
            {
                return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Structure)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Structure).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Structure);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? TargetTypeId
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.TargetType)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.TargetType).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.TargetType);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public uint? ItemUseable
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemUseable)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemUseable).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemUseable);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public float? UseRadius
        {
            get
            {
                return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.UseRadius)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.UseRadius).PropertyValue = (double)value;
                else
                {
                    var listItem = AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.UseRadius);
                    if (listItem != null)
                        AceObjectPropertiesDouble.Remove(listItem);
                }
            }
        }

        /// <summary>
        /// Left the name as ValidLocations as it is more descriptive of what it does than just locations.
        /// This field maps to EquipMask Enum
        /// </summary>
        public uint? ValidLocations
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Locations)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Locations).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Locations);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public uint? Value
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Value)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Value).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Value);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public float Workmanship
        {
            get
            {
                if ((ItemWorkmanship != null) && (Structure != null) && (Structure != 0))
                {
                    return (float)Convert.ToDouble(ItemWorkmanship / (10000 * Structure));
                }
                return 0.00f;
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
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemWorkmanship)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemWorkmanship).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemWorkmanship);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public float? PhysicsScriptIntensity
        {
            get
            {
                return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.PhysicsScriptIntensity)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.PhysicsScriptIntensity).PropertyValue = (double)value;
                else
                {
                    var listItem = AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.PhysicsScriptIntensity);
                    if (listItem != null)
                        AceObjectPropertiesDouble.Remove(listItem);
                }
            }
        }

        public float? Elasticity
        {
            get
            {
                return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Elasticity)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Elasticity).PropertyValue = (double)value;
                else
                {
                    var listItem = AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Elasticity);
                    if (listItem != null)
                        AceObjectPropertiesDouble.Remove(listItem);
                }
            }
        }

        public float? Friction
        {
            get
            {
                return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Friction)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Friction).PropertyValue = (double)value;
                else
                {
                    var listItem = AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Friction);
                    if (listItem != null)
                        AceObjectPropertiesDouble.Remove(listItem);
                }
            }
        }

        public uint? CurrentWieldedLocation
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CurrentWieldedLocation)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CurrentWieldedLocation).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CurrentWieldedLocation);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public float? DefaultScale
        {
            get
            {
                return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.DefaultScale)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.DefaultScale).PropertyValue = (double)value;
                else
                {
                    var listItem = AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.DefaultScale);
                    if (listItem != null)
                        AceObjectPropertiesDouble.Remove(listItem);
                }
            }
        }

        /// <summary>
        /// TODO: convert to enum, probably a flags enum
        /// </summary>
        public uint? PhysicsState
        {
            get
            {
                return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PhysicsState)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PhysicsState).PropertyValue = (uint)value;
                else
                {
                    var listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PhysicsState);
                    if (listItem != null)
                        AceObjectPropertiesInt.Remove(listItem);
                }
            }
        }

        public float? Translucency
        {
            get
            {
                return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Translucency)?.PropertyValue;
            }
            set
            {
                if (value != null)
                    AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Translucency).PropertyValue = (double)value;
                else
                {
                    var listItem = AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.DefaultScale);
                    if (listItem != null)
                        AceObjectPropertiesDouble.Remove(listItem);
                }
            }
        }

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
