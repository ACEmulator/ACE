using System.Collections.Generic;
using ACE.Common;
using MySql.Data.MySqlClient;
using ACE.Entity.Enum.Properties;
using System;

namespace ACE.Entity
{
    [DbTable("vw_base_ace_object")]
    [DbGetList("vw_base_ace_object", 15, "itemType")]
    public class BaseAceObject
    {
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
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PlacementPosition)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.PlacementPosition, value); }
        }

        [DbField("currentMotionState", (int)MySqlDbType.Text)]
        public string CurrentMotionState { get; set; }

        public uint? IconId
        {
            get { return AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.Icon)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.Icon, value); }
        }

        public uint? IconOverlayId
        {
            get { return AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.IconOverlay)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.IconOverlay, value); }
        }

        public uint? IconUnderlayId
        {
            get { return AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.IconUnderlay)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.IconUnderlay, value); }
        }

        /// <summary>
        /// PhysicsData.CSetup
        /// </summary>
        public uint? ModelTableId
        {
            get { return AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.Setup)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.Setup, value); }
        }

        public uint? MotionTableId
        {
            get { return AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.MotionTable)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.MotionTable, value); }
        }

        public ushort? PhysicsScript
        {
            get { return (ushort?)AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.PhysicsScript)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.PhysicsScript, value); }
        }

        public uint? PhysicsTableId
        {
            get { return AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.PhysicsEffectTable)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.PhysicsEffectTable, value); }
        }


        public uint? SoundTableId
        {
            get { return AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.SoundTable)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.SoundTable, value); }
        }

        public ushort? SpellId
        {
            get { return (ushort?)AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.Spell)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.Spell, value); }
        }

        public uint? DefaultScript
        {
            get { return AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)PropertiesDid.UseUserAnimation)?.PropertyValue; }
            set { SetDidProperty(PropertiesDid.UseUserAnimation, value); }
        }

        public void SetDidProperty(PropertiesDid didPropertyId, uint? value)
        {
            AceObjectPropertiesDid listItem = AceObjectPropertiesDid.Find(x => x.DidPropertyId == (uint)didPropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesDid { DidPropertyId = (uint)didPropertyId, PropertyValue = (uint)value };
                    AceObjectPropertiesDid.Add(listItem);
                }
                else
                    listItem.PropertyValue = (uint)value;
            }
            else
            {
                if (listItem != null)
                    AceObjectPropertiesDid.Remove(listItem);
            }
        }

        public void SetIidProperty(PropertiesDid iidPropertyId, uint? value)
        {
            AceObjectPropertiesIid listItem = AceObjectPropertiesIid.Find(x => x.IidPropertyId == (uint)iidPropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesIid { IidPropertyId = (uint)iidPropertyId, PropertyValue = (uint)value };
                    AceObjectPropertiesIid.Add(listItem);
                }
                else
                    listItem.PropertyValue = (uint)value;
            }
            else
            {
                if (listItem != null)
                    AceObjectPropertiesIid.Remove(listItem);
            }
        }

        public void SetIntProperty(PropertyInt intPropertyId, uint? value)
        {
            AceObjectPropertiesInt listItem = AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)intPropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesInt { IntPropertyId = (uint)intPropertyId, PropertyValue = (uint)value };
                    AceObjectPropertiesInt.Add(listItem);
                }
                else
                    listItem.PropertyValue = (uint)value;
            }
            else
            {
                if (listItem != null)
                    AceObjectPropertiesInt.Remove(listItem);
            }
        }

        public void SetDoubleProperty(PropertyDouble dblPropertyId, double? value)
        {
            AceObjectPropertiesDouble listItem = AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)dblPropertyId);
            if (value != null)
            {
                if (listItem == null)
                {
                    listItem = new AceObjectPropertiesDouble()
                    {
                        DblPropertyId = (uint)dblPropertyId,
                        PropertyValue = (double)value
                    };
                    AceObjectPropertiesDouble.Add(listItem);
                }
                else listItem.PropertyValue = (double)value;
            }
            else
            {
                if (listItem != null)
                    AceObjectPropertiesDouble.Remove(listItem);
            }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? AmmoType
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.AmmoType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.AmmoType, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? BlipColor
        {
            get { return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.RadarBlipColor)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.RadarBlipColor, value); }
        }

        public ushort? Burden
        {
            get { return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.EncumbranceVal)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.EncumbranceVal, value); }
        }
        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? CombatUse
        {
            get { return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CombatUse)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.CombatUse, value); }
        }

        public byte? ContainersCapacity
        {
            get { return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ContainersCapacity)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ContainersCapacity, value); }
        }

        public double? CooldownDuration
        {
            get { return AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.CooldownDuration)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.CooldownDuration, value); }
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

        [DbField("itemType", (int)MySqlDbType.UInt32)]
        public uint ItemType
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemType).PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemType, value); }
        }

        public uint? PaletteId
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PaletteTemplate)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.PaletteTemplate, value); }
        }

        // TODO: Not sure if this enum is right.
        public uint? CooldownId
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.SharedCooldown)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.SharedCooldown, value); }
        }

        public uint? UiEffects
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.UiEffects)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.UiEffects, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public ushort? HookType
        {
            get { return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.HookType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.HookType, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public ushort? HookItemTypes
        {
            get { return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.HookItemType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.HookItemType, value); }
        }

        public byte? ItemsCapacity
        {
            get { return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemsCapacity)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemsCapacity, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? MaterialType
        {
            get { return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaterialType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.MaterialType, value); }
        }

        public ushort? MaxStackSize
        {
            get { return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaxStackSize)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.MaxStackSize, value); }
        }
        /// <summary>
        /// This is the Maximum an item can hold in the case of salvage 100
        /// everything else healing kits, lock picks etc it is the max number of uses.
        /// </summary>
        public ushort? MaxStructure
        {
            get { return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.MaxStructure)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.MaxStructure, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public byte? Radar
        {
            get { return (byte?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ShowableOnRadar)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ShowableOnRadar, value); }
        }

        public ushort? StackSize
        {
            get { return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.StackSize)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.StackSize, value); }
        }
        /// <summary>
        /// This field represents the number of units or uses an item has in it or left.   Salvage in it
        /// healing kits, essences the number left.
        /// </summary>
        public ushort? Structure
        {
            get { return (ushort?)AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Structure)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.Structure, value); }
        }

        /// <summary>
        /// TODO: convert to enum
        /// </summary>
        public uint? TargetTypeId
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.TargetType)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.TargetType, value); }
        }

        public uint? ItemUseable
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemUseable)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemUseable, value); }
        }

        public float? UseRadius
        {
            get { return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.UseRadius)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.UseRadius, value); }
        }

        /// <summary>
        /// Left the name as ValidLocations as it is more descriptive of what it does than just locations.
        /// This field maps to EquipMask Enum
        /// </summary>
        public uint? ValidLocations
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ValidLocations)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ValidLocations, value); }
        }

        public uint? Value
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.Value)?.PropertyValue; }
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
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.ItemWorkmanship)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.ItemWorkmanship, value); }
        }

        public float? PhysicsScriptIntensity
        {
            get { return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.PhysicsScriptIntensity)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.PhysicsScriptIntensity, value); }
        }

        public float? Elasticity
        {
            get { return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Elasticity)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.Elasticity, value); }
        }

        public float? Friction
        {
            get { return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Friction)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.Friction, value); }
        }

        public uint? CurrentWieldedLocation
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.CurrentWieldedLocation)?.PropertyValue; }
            set { SetIntProperty(PropertyInt.CurrentWieldedLocation, value); }
        }

        public float? DefaultScale
        {
            get { return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.DefaultScale)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.DefaultScale, value); }
        }

        /// <summary>
        /// TODO: convert to enum, probably a flags enum
        /// </summary>
        public uint PhysicsState
        {
            get { return AceObjectPropertiesInt.Find(x => x.IntPropertyId == (uint)PropertyInt.PhysicsState).PropertyValue; }
            set { SetIntProperty(PropertyInt.PhysicsState, value); }
        }

        public float? Translucency
        {
            get { return (float?)AceObjectPropertiesDouble.Find(x => x.DblPropertyId == (uint)PropertyDouble.Translucency)?.PropertyValue; }
            set { SetDoubleProperty(PropertyDouble.Translucency, value); }
        }

        public List<PaletteOverride> PaletteOverrides { get; set; } = new List<PaletteOverride>();

        public List<TextureMapOverride> TextureOverrides { get; set; } = new List<TextureMapOverride>();

        public List<AnimationOverride> AnimationOverrides { get; set; } = new List<AnimationOverride>();

        public List<AceObjectPropertiesInt> AceObjectPropertiesInt { get; set; } = new List<AceObjectPropertiesInt>();

        public List<AceObjectPropertiesBigInt> AceObjectPropertiesBigInt { get; set; } = new List<AceObjectPropertiesBigInt>();

        public List<AceObjectPropertiesDouble> AceObjectPropertiesDouble { get; set; } = new List<AceObjectPropertiesDouble>();

        public List<AceObjectPropertiesBool> AceObjectPropertiesBool { get; set; } = new List<AceObjectPropertiesBool>();

        public List<AceObjectPropertiesDid> AceObjectPropertiesDid { get; set; } = new List<AceObjectPropertiesDid>();

        public List<AceObjectPropertiesIid> AceObjectPropertiesIid { get; set; } = new List<AceObjectPropertiesIid>();

        public List<AceObjectPropertiesString> AceObjectPropertiesString { get; set; } = new List<AceObjectPropertiesString>();
    }
}
