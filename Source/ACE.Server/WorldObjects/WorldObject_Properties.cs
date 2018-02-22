using System;
using System.Collections.Generic;

using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.Enum;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public Dictionary<PropertyBool, bool?> EphemeralPropertyBools = new Dictionary<PropertyBool, bool?>();
        public Dictionary<PropertyDataId, uint?> EphemeralPropertyDataIds = new Dictionary<PropertyDataId, uint?>();
        public Dictionary<PropertyFloat, double?> EphemeralPropertyFloats = new Dictionary<PropertyFloat, double?>();
        public Dictionary<PropertyInstanceId, int?> EphemeralPropertyInstanceIds = new Dictionary<PropertyInstanceId, int?>();
        public Dictionary<PropertyInt, int?> EphemeralPropertyInts = new Dictionary<PropertyInt, int?>();
        public Dictionary<PropertyInt64, long?> EphemeralPropertyInt64s = new Dictionary<PropertyInt64, long?>();
        public Dictionary<PropertyString, string> EphemeralPropertyStrings = new Dictionary<PropertyString, string>();

        public bool? GetProperty(PropertyBool property) { if (EphemeralPropertyBools.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public uint? GetProperty(PropertyDataId property) { if (EphemeralPropertyDataIds.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public double? GetProperty(PropertyFloat property) { if (EphemeralPropertyFloats.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public int? GetProperty(PropertyInstanceId property) { if (EphemeralPropertyInstanceIds.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public int? GetProperty(PropertyInt property) { if (EphemeralPropertyInts.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public long? GetProperty(PropertyInt64 property) { if (EphemeralPropertyInt64s.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }
        public string GetProperty(PropertyString property) { if (EphemeralPropertyStrings.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property); }

        public void SetProperty(PropertyBool property, bool value) { if (EphemeralPropertyBools.ContainsKey(property)) EphemeralPropertyBools[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyDataId property, uint value) { if (EphemeralPropertyDataIds.ContainsKey(property)) EphemeralPropertyDataIds[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyFloat property, double value) { if (EphemeralPropertyFloats.ContainsKey(property)) EphemeralPropertyFloats[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInstanceId property, int value) { if (EphemeralPropertyInstanceIds.ContainsKey(property)) EphemeralPropertyInstanceIds[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInt property, int value) { if (EphemeralPropertyInts.ContainsKey(property)) EphemeralPropertyInts[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInt64 property, long value) { if (EphemeralPropertyInt64s.ContainsKey(property)) EphemeralPropertyInt64s[property] = value; else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyString property, string value) { if (EphemeralPropertyStrings.ContainsKey(property)) EphemeralPropertyStrings[property] = value; else Biota.SetProperty(property, value); }

        public void RemoveProperty(PropertyBool property) { if (EphemeralPropertyBools.ContainsKey(property)) EphemeralPropertyBools[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyDataId property) { if (EphemeralPropertyDataIds.ContainsKey(property)) EphemeralPropertyDataIds[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyFloat property) { if (EphemeralPropertyFloats.ContainsKey(property)) EphemeralPropertyFloats[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInstanceId property) { if (EphemeralPropertyInstanceIds.ContainsKey(property)) EphemeralPropertyInstanceIds[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInt property) { if (EphemeralPropertyInts.ContainsKey(property)) EphemeralPropertyInts[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInt64 property) { if (EphemeralPropertyInt64s.ContainsKey(property)) EphemeralPropertyInt64s[property] = null; else Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyString property) { if (EphemeralPropertyStrings.ContainsKey(property)) EphemeralPropertyStrings[property] = null; else Biota.RemoveProperty(property); }

        public Dictionary<PropertyBool, bool> GetAllPropertyBools()
        {
            var results = new Dictionary<PropertyBool, bool>();

            foreach (var property in Biota.BiotaPropertiesBool)
                results[(PropertyBool)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyBools)
                if (property.Value.HasValue)
                    results[property.Key] = (bool)property.Value;

            return results;
        }

        public Dictionary<PropertyDataId, uint> GetAllPropertyDataId()
        {
            var results = new Dictionary<PropertyDataId, uint>();

            foreach (var property in Biota.BiotaPropertiesDID)
                results[(PropertyDataId)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyDataIds)
                if (property.Value.HasValue)
                    results[property.Key] = (uint)property.Value;

            return results;
        }

        public Dictionary<PropertyFloat, double> GetAllPropertyFloat()
        {
            var results = new Dictionary<PropertyFloat, double>();

            foreach (var property in Biota.BiotaPropertiesFloat)
                results[(PropertyFloat)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyFloats)
                if (property.Value.HasValue)
                    results[property.Key] = (double)property.Value;

            return results;
        }

        public Dictionary<PropertyInstanceId, int> GetAllPropertyInstanceId()
        {
            var results = new Dictionary<PropertyInstanceId, int>();

            foreach (var property in Biota.BiotaPropertiesIID)
                results[(PropertyInstanceId)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyInstanceIds)
                if (property.Value.HasValue)
                    results[property.Key] = (int)property.Value;

            return results;
        }

        public Dictionary<PropertyInt, int> GetAllPropertyInt()
        {
            var results = new Dictionary<PropertyInt, int>();

            foreach (var property in Biota.BiotaPropertiesInt)
                results[(PropertyInt)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyInts)
                if (property.Value.HasValue)
                    results[property.Key] = (int)property.Value;

            return results;
        }

        public Dictionary<PropertyInt64, long> GetAllPropertyInt64()
        {
            var results = new Dictionary<PropertyInt64, long>();

            foreach (var property in Biota.BiotaPropertiesInt64)
                results[(PropertyInt64)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyInt64s)
                if (property.Value.HasValue)
                    results[property.Key] = (long)property.Value;

            return results;
        }

        public Dictionary<PropertyString, string> GetAllPropertyString()
        {
            var results = new Dictionary<PropertyString, string>();

            foreach (var property in Biota.BiotaPropertiesString)
                results[(PropertyString)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyStrings)
                if (property.Value != null)
                    results[property.Key] = property.Value;

            return results;
        }


        //public Dictionary<PositionType, Position> Positions { get; set; } = new Dictionary<PositionType, Position>();

        public Position GetPosition(PositionType positionType) // { return Biota.GetPosition(positionType); }
        {
            bool success = Positions.TryGetValue(positionType, out var ret);

            if (!success)
            {
                var result = Biota.GetPosition(positionType);

                if (result != null)
                    Positions.TryAdd(positionType, result);

                return result;
            }

            //if (!success)
            //{
            //    return null;
            //}
            return ret;
        }

        public void SetPosition(PositionType positionType, Position position) // { Biota.SetPosition(positionType, position); }
        {
            if (position == null)
            {
                Positions.Remove(positionType);
                Biota.RemovePosition(positionType);
            }
            else
            {
                if (!Positions.ContainsKey(positionType))
                    Positions.TryAdd(positionType, position);
                else
                    Positions[positionType] = position;
                Biota.SetPosition(positionType, position);
            }
        }

        // todo: We also need to manually remove the property from the shard db.
        // todo: Using these fn's will only remove the property for this session, but the property will return next session since the record isn't removed from the db.
        // todo: fix this in BiotaExcentions. Add code there that removes the entry from teh shard
        public void RemovePosition(PositionType positionType) { Biota.RemovePosition(positionType); }


        // ========================================
        // =========== Model Properties ===========
        // ========================================
        // used in SerializeModelData()
        [Obsolete]
        private readonly List<ModelPalette> modelPalettes = new List<ModelPalette>();
        [Obsolete]
        private readonly List<ModelTexture> modelTextures = new List<ModelTexture>();
        [Obsolete]
        private readonly List<Model> models = new List<Model>();


        // ========================================
        // ======== Physics Desc Properties =======
        // ========================================
        // used in CalculatedPhysicsDescriptionFlag()
        public MotionState CurrentMotionState { get; set; }

        public Placement? Placement // Sometimes known as AnimationFrame
        {
            get => (Placement?)GetProperty(PropertyInt.Placement);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Placement); else SetProperty(PropertyInt.Placement, (int)value.Value); }
        }

        public virtual Position Location
        {
            get => GetPosition(PositionType.Location);
            set => SetPosition(PositionType.Location, value);
            //set
            //{
            //    /*
            //    log.Debug($"{Name} moved to {Position}");
            //    Position = value;
            //    */
            //    if (GetPosition(PositionType.Location) != null)
            //        LastUpdatedTicks = WorldManager.PortalYearTicks;
            //    SetPosition(PositionType.Location, value);
            //}
        }

        //public double LastUpdatedTicks { get; set; }

        /// <summary>
        /// mtable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint MotionTableId
        {
            get => GetProperty(PropertyDataId.MotionTable) ?? 0;
            set => SetProperty(PropertyDataId.MotionTable, value);
        }

        /// <summary>
        /// stable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint SoundTableId
        {
            get => GetProperty(PropertyDataId.SoundTable) ?? 0;
            set => SetProperty(PropertyDataId.SoundTable, value);
        }

        /// <summary>
        /// phstable_id in aclogviewer This is the physics table for the object.   Looked up from dat file.
        /// </summary>
        public uint PhysicsTableId
        {
            get => GetProperty(PropertyDataId.PhysicsEffectTable) ?? 0;
            set => SetProperty(PropertyDataId.PhysicsEffectTable, value);
        }

        /// <summary>
        /// setup_id in aclogviewer - used to get the correct model out of the dat file
        /// </summary>
        public uint SetupTableId
        {
            get => GetProperty(PropertyDataId.Setup) ?? 0;
            set => SetProperty(PropertyDataId.Setup, value);
        }

        // PhysicsDescriptionFlag.Parent is pulled from WielderId

        public List<HeldItem> Children { get; } = new List<HeldItem>();

        public float? ObjScale
        {
            get => (float?)GetProperty(PropertyFloat.DefaultScale);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.DefaultScale); else SetProperty(PropertyFloat.DefaultScale, value.Value); }
        }

        public float? Friction
        {
            get => (float?)GetProperty(PropertyFloat.Friction);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.Friction); else SetProperty(PropertyFloat.Friction, value.Value); }
        }

        public float? Elasticity
        {
            get => (float?)GetProperty(PropertyFloat.Elasticity);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.Elasticity); else SetProperty(PropertyFloat.Elasticity, value.Value); }
        }

        public float? Translucency
        {
            get => (float?)GetProperty(PropertyFloat.Translucency);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.Translucency); else SetProperty(PropertyFloat.Translucency, value.Value); }
        }

        public AceVector3 Velocity = null;

        public AceVector3 Acceleration { get; set; }

        public AceVector3 Omega = null;

        public SetupModel CSetup => DatManager.PortalDat.ReadFromDat<SetupModel>(SetupTableId);

        public uint? DefaultScriptId
        {
            // Is this CSetup.DefaultScript?
            get => Script;
            set { Script = (ushort?)value; }
        }

        public float? DefaultScriptIntensity
        {
            get => (float?)GetProperty(PropertyFloat.PhysicsScriptIntensity);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.PhysicsScriptIntensity); else SetProperty(PropertyFloat.PhysicsScriptIntensity, value.Value); }
        }


        // ========================================
        // ======= Physics State Properties =======
        // ========================================
        // used in CalculatedPhysicsState()
        public bool? Static { get; set; }

        public bool? Ethereal
        {
            get => GetProperty(PropertyBool.Ethereal);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Ethereal); else SetProperty(PropertyBool.Ethereal, value.Value); }
        }

        public bool? ReportCollisions
        {
            get => GetProperty(PropertyBool.ReportCollisions);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.ReportCollisions); else SetProperty(PropertyBool.ReportCollisions, value.Value); }
        }

        public bool? IgnoreCollisions
        {
            get => GetProperty(PropertyBool.IgnoreCollisions);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.IgnoreCollisions); else SetProperty(PropertyBool.IgnoreCollisions, value.Value); }
        }

        public bool? NoDraw
        {
            get => GetProperty(PropertyBool.NoDraw);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.NoDraw); else SetProperty(PropertyBool.NoDraw, value.Value); }
        }

        public bool? Missile { get; set; }

        public bool? Pushable { get; set; }

        public bool? AlignPath { get; set; }

        public bool? PathClipped { get; set; }

        public bool? GravityStatus
        {
            get => GetProperty(PropertyBool.GravityStatus);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.GravityStatus); else SetProperty(PropertyBool.GravityStatus, value.Value); }
        }

        public bool? LightsStatus
        {
            get => GetProperty(PropertyBool.LightsStatus);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.LightsStatus); else SetProperty(PropertyBool.LightsStatus, value.Value); }
        }

        public bool? ParticleEmitter { get; set; }

        public bool? Hidden { get; set; }

        public bool? ScriptedCollision
        {
            get => GetProperty(PropertyBool.ScriptedCollision);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.ScriptedCollision); else SetProperty(PropertyBool.ScriptedCollision, value.Value); }
        }

        public bool? Inelastic
        {
            get => GetProperty(PropertyBool.Inelastic);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Inelastic); else SetProperty(PropertyBool.Inelastic, value.Value); }
        }

        public bool? Cloaked { get; set; }

        public bool? ReportCollisionsAsEnvironment
        {
            get => GetProperty(PropertyBool.ReportCollisionsAsEnvironment);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.ReportCollisionsAsEnvironment); else SetProperty(PropertyBool.ReportCollisionsAsEnvironment, value.Value); }
        }

        public bool? AllowEdgeSlide
        {
            get => GetProperty(PropertyBool.AllowEdgeSlide);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.AllowEdgeSlide); else SetProperty(PropertyBool.AllowEdgeSlide, value.Value); }
        }

        public bool? Sledding { get; set; }

        public bool? IsFrozen
        {
            get => GetProperty(PropertyBool.IsFrozen);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.IsFrozen); else SetProperty(PropertyBool.IsFrozen, value.Value); }
        }


        // ========================================
        // ========== Generic Properties ==========
        // ========================================
        // used in SerializeCreateObject()
        public string Name
        {
            get => GetProperty(PropertyString.Name);
            set => SetProperty(PropertyString.Name, value);
        }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public uint WeenieClassId => Biota.WeenieClassId;

        public WeenieType WeenieType => (WeenieType)Biota.WeenieType;

        public uint IconId
        {
            get => GetProperty(PropertyDataId.Icon) ?? 0;
            set => SetProperty(PropertyDataId.Icon, value);
        }

        public ItemType ItemType
        {
            get => (ItemType)(GetProperty(PropertyInt.ItemType) ?? 0);
            set => SetProperty(PropertyInt.ItemType, (int)value);
        }


        // ========================================
        // ======= Weenie Header Properties =======
        // ========================================
        // used in CalculatedWeenieHeaderFlag()
        public string NamePlural
        {
            get => GetProperty(PropertyString.PluralName);
            set => SetProperty(PropertyString.PluralName, value);
        }

        public byte? ItemCapacity
        {
            get => (byte?)GetProperty(PropertyInt.ItemsCapacity);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemsCapacity); else SetProperty(PropertyInt.ItemsCapacity, value.Value); }
        }

        public byte? ContainerCapacity
        {
            get => (byte?)GetProperty(PropertyInt.ContainersCapacity);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ContainersCapacity); else SetProperty(PropertyInt.ContainersCapacity, value.Value); }
        }

        public AmmoType? AmmoType
        {
            get => (AmmoType?)GetProperty(PropertyInt.AmmoType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AmmoType); else SetProperty(PropertyInt.AmmoType, (int)value.Value); }
        }

        public virtual int? Value
        {
            // todo this value has different get/set.. get is calculated while set goes to db, that's wrong.. should be 1:1 or 1:
            get => (StackUnitValue * (StackSize ?? 1));
            set => AceObject.Value = value;
        }

        public Usable? Usable
        {
            get => (Usable?)GetProperty(PropertyInt.ItemUseable);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemUseable); else SetProperty(PropertyInt.ItemUseable, (int)value.Value); }
        }

        public float? UseRadius
        {
            get => (float?)GetProperty(PropertyFloat.UseRadius);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.UseRadius); else SetProperty(PropertyFloat.UseRadius, value.Value); }
        }

        public int? TargetType
        {
            get => GetProperty(PropertyInt.TargetType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.TargetType); else SetProperty(PropertyInt.TargetType, value.Value); }
        }

        public UiEffects? UiEffects
        {
            get => (UiEffects?)GetProperty(PropertyInt.UiEffects);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.UiEffects); else SetProperty(PropertyInt.UiEffects, (int)value.Value); }
        }

        public CombatUse? CombatUse
        {
            get => (CombatUse?)GetProperty(PropertyInt.CombatUse);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CombatUse); else SetProperty(PropertyInt.CombatUse, (int)value.Value); }
        }

        /// <summary>
        /// This is used to indicate the number of uses remaining.  Example 32 uses left out of 50 (MaxStructure)
        /// </summary>
        public ushort? Structure
        {
            get => (ushort?)GetProperty(PropertyInt.Structure);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Structure); else SetProperty(PropertyInt.Structure, value.Value); }
        }

        /// <summary>
        /// Use Limit - example 50 use healing kit
        /// </summary>
        public ushort? MaxStructure
        {
            get => (ushort?)GetProperty(PropertyInt.MaxStructure);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MaxStructure); else SetProperty(PropertyInt.MaxStructure, value.Value); }
        }

        public virtual ushort? StackSize
        {
            get => (ushort?)GetProperty(PropertyInt.StackSize);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.StackSize); else SetProperty(PropertyInt.StackSize, value.Value); }
        }

        public ushort? MaxStackSize
        {
            get => (ushort?)GetProperty(PropertyInt.MaxStackSize);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MaxStackSize); else SetProperty(PropertyInt.MaxStackSize, value.Value); }
        }

        public int? ContainerId
        {
            get => GetProperty(PropertyInstanceId.Container);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Container); else SetProperty(PropertyInstanceId.Container, value.Value); }
        }

        public int? WielderId
        {
            get => GetProperty(PropertyInstanceId.Wielder);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Wielder); else SetProperty(PropertyInstanceId.Wielder, value.Value); }
        }

        public EquipMask? ValidLocations
        {
            get => (EquipMask?)GetProperty(PropertyInt.ValidLocations);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ValidLocations); else SetProperty(PropertyInt.ValidLocations, (int)value.Value); }
        }

        public EquipMask? CurrentWieldedLocation
        {
            get => (EquipMask?)GetProperty(PropertyInt.CurrentWieldedLocation);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CurrentWieldedLocation); else SetProperty(PropertyInt.CurrentWieldedLocation, (int)value.Value); }
        }

        public CoverageMask? Priority
        {
            get => (CoverageMask?)GetProperty(PropertyInt.ClothingPriority);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ClothingPriority); else SetProperty(PropertyInt.ClothingPriority, (int)value.Value); }
        }

        public RadarColor? RadarColor
        {
            get => (RadarColor?)GetProperty(PropertyInt.RadarBlipColor);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.RadarBlipColor); else SetProperty(PropertyInt.RadarBlipColor, (int)value.Value); }
        }

        public RadarBehavior? RadarBehavior
        {
            get => (RadarBehavior?)GetProperty(PropertyInt.ShowableOnRadar);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ShowableOnRadar); else SetProperty(PropertyInt.ShowableOnRadar, (int)value.Value); }
        }

        public ushort? Script
        {
            get => (ushort?)GetProperty(PropertyDataId.PhysicsScript);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.PhysicsScript); else SetProperty(PropertyDataId.PhysicsScript, value.Value); }
        }

        private int? ItemWorkmanship
        {
            get => GetProperty(PropertyInt.ItemWorkmanship);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemWorkmanship); else SetProperty(PropertyInt.ItemWorkmanship, value.Value); }
        }

        public float? Workmanship
        {
            get
            {
                if ((ItemWorkmanship != null) && (Structure != null) && (Structure != 0))
                    return (float)Convert.ToDouble(ItemWorkmanship / (10000 * Structure));

                return (ItemWorkmanship);
            }
            set
            {
                if ((Structure != null) && (Structure != 0))
                    ItemWorkmanship = Convert.ToInt32(value * 10000 * Structure);
                else
                    ItemWorkmanship = Convert.ToInt32(value);
            }
        }

        public virtual ushort? Burden
        {
            // todo this value has different get/set.. get is calculated while set goes to db, that's wrong.. should be 1:1 or 1:
            get => (ushort)(StackUnitBurden * (StackSize ?? 1));
            set => AceObject.EncumbranceVal = value;
        }

        public Spell? Spell
        {
            get => (Spell?)GetProperty(PropertyDataId.Spell);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.Spell); else SetProperty(PropertyDataId.Spell, (uint)value.Value); }
        }

        /// <summary>
        /// Housing links to another packet, that needs sent.. The HouseRestrictions ACL Control list that contains all the housing data
        /// </summary>
        public int? HouseOwner
        {
            get => GetProperty(PropertyInstanceId.HouseOwner);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.HouseOwner); else SetProperty(PropertyInstanceId.HouseOwner, value.Value); }
        }

        public uint? HouseRestrictions { get; set; }

        public ushort? HookItemType
        {
            get => (ushort?)GetProperty(PropertyInt.HookItemType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HookItemType); else SetProperty(PropertyInt.HookItemType, value.Value); }
        }

        public int? Monarch
        {
            get => GetProperty(PropertyInstanceId.Monarch);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Monarch); else SetProperty(PropertyInstanceId.Monarch, value.Value); }
        }

        public ushort? HookType
        {
            get => (ushort?)GetProperty(PropertyInt.HookType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HookType); else SetProperty(PropertyInt.HookType, value.Value); }
        }

        public uint? IconOverlayId
        {
            get => GetProperty(PropertyDataId.IconOverlay);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.IconOverlay); else SetProperty(PropertyDataId.IconOverlay, value.Value); }
        }

        public Material? MaterialType
        {
            get => (Material?)GetProperty(PropertyInt.MaterialType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MaterialType); else SetProperty(PropertyInt.MaterialType, (int)value.Value); }
        }


        // ========================================
        // ====== Weenie Header 2 Properties ======
        // ========================================
        // used in CalculatedWeenieHeaderFlag2()
        public uint? IconUnderlayId
        {
            get => GetProperty(PropertyDataId.IconUnderlay);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.IconUnderlay); else SetProperty(PropertyDataId.IconUnderlay, value.Value); }
        }

        public int? CooldownId
        {
            get => GetProperty(PropertyInt.SharedCooldown);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.SharedCooldown); else SetProperty(PropertyInt.SharedCooldown, value.Value); }
        }

        public double? CooldownDuration
        {
            get => GetProperty(PropertyFloat.CooldownDuration);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.CooldownDuration); else SetProperty(PropertyFloat.CooldownDuration, value.Value); }
        }

        public int? PetOwner
        {
            get => GetProperty(PropertyInstanceId.PetOwner);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.PetOwner); else SetProperty(PropertyInstanceId.PetOwner, value.Value); }
        }
    }
}
