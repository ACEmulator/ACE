using System;
using System.Collections.Generic;
using System.Threading;

using ACE.Database;
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
    // todo: After we have all the properties moved here, we should set them all to private. Only what's needed should be protected/public.
    // todo: In addition, unused setters should be commented (not removed).
    //
    // todo: Also, properties only used by certain WorldObjectTypes should be moved to that worldobject type class and not remain here in the base.
    // todo: For example, ChessGamesWon would only be used by a player. That property doesn't need to be in WorldObject and thus accessable to all WorldObject classes.
    //
    // todo: When we're confident a set of functions, or a group of properties are "final", we can wrap them in a region
    partial class WorldObject
    {
        public Dictionary<PropertyBool, bool?> EphemeralPropertyBools = new Dictionary<PropertyBool, bool?>();
        public Dictionary<PropertyDataId, uint?> EphemeralPropertyDataIds = new Dictionary<PropertyDataId, uint?>();
        public Dictionary<PropertyFloat, double?> EphemeralPropertyFloats = new Dictionary<PropertyFloat, double?>();
        public Dictionary<PropertyInstanceId, uint?> EphemeralPropertyInstanceIds = new Dictionary<PropertyInstanceId, uint?>();
        public Dictionary<PropertyInt, int?> EphemeralPropertyInts = new Dictionary<PropertyInt, int?>();
        public Dictionary<PropertyInt64, long?> EphemeralPropertyInt64s = new Dictionary<PropertyInt64, long?>();
        public Dictionary<PropertyString, string> EphemeralPropertyStrings = new Dictionary<PropertyString, string>();

        #region Property Locks
        private readonly ReaderWriterLockSlim biotaPropertiesBoolLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim biotaPropertiesDIDLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim biotaPropertiesEnchantmentLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim biotaPropertiesFloatLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim biotaPropertiesIIDLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim biotaPropertiesIntLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim biotaPropertiesInt64Lock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim biotaPropertiesPositionLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim biotaPropertiesStringLock = new ReaderWriterLockSlim();
        #endregion

        #region GetProperty Functions
        public bool? GetProperty(PropertyBool property) { if (EphemeralPropertyBools.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property, biotaPropertiesBoolLock); }
        public uint? GetProperty(PropertyDataId property) { if (EphemeralPropertyDataIds.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property, biotaPropertiesDIDLock); }
        public double? GetProperty(PropertyFloat property) { if (EphemeralPropertyFloats.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property, biotaPropertiesFloatLock); }
        public uint? GetProperty(PropertyInstanceId property) { if (EphemeralPropertyInstanceIds.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property, biotaPropertiesIIDLock); }
        public int? GetProperty(PropertyInt property) { if (EphemeralPropertyInts.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property, biotaPropertiesIntLock); }
        public long? GetProperty(PropertyInt64 property) { if (EphemeralPropertyInt64s.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property, biotaPropertiesInt64Lock); }
        public string GetProperty(PropertyString property) { if (EphemeralPropertyStrings.TryGetValue(property, out var value)) return value; return Biota.GetProperty(property, biotaPropertiesStringLock); }
        #endregion

        #region SetProperty Functions
        public void SetProperty(PropertyBool property, bool value)
        {
            if (EphemeralPropertyBools.ContainsKey(property))
                EphemeralPropertyBools[property] = value;
            else
            {
                Biota.SetProperty(property, value, biotaPropertiesBoolLock);
                ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyDataId property, uint value)
        {
            if (EphemeralPropertyDataIds.ContainsKey(property))
                EphemeralPropertyDataIds[property] = value;
            else
            {
                Biota.SetProperty(property, value, biotaPropertiesDIDLock);
                ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyFloat property, double value)
        {
            if (EphemeralPropertyFloats.ContainsKey(property))
                EphemeralPropertyFloats[property] = value;
            else
            {
                Biota.SetProperty(property, value, biotaPropertiesFloatLock);
                ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyInstanceId property, uint value)
        {
            if (EphemeralPropertyInstanceIds.ContainsKey(property))
                EphemeralPropertyInstanceIds[property] = value;
            else
            {
                Biota.SetProperty(property, value, biotaPropertiesIIDLock);
                ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyInt property, int value)
        {
            if (EphemeralPropertyInts.ContainsKey(property))
                EphemeralPropertyInts[property] = value;
            else
            {
                Biota.SetProperty(property, value, biotaPropertiesIntLock);
                ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyInt64 property, long value)
        {
            if (EphemeralPropertyInt64s.ContainsKey(property))
                EphemeralPropertyInt64s[property] = value;
            else
            {
                Biota.SetProperty(property, value, biotaPropertiesInt64Lock);
                ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyString property, string value)
        {
            if (EphemeralPropertyStrings.ContainsKey(property))
                EphemeralPropertyStrings[property] = value;
            else
            {
                Biota.SetProperty(property, value, biotaPropertiesStringLock);
                ChangesDetected = true;
            }
        }
        #endregion

        #region RemoveProperty Functions
        public void RemoveProperty(PropertyBool property)
        {
            if (EphemeralPropertyBools.ContainsKey(property))
                EphemeralPropertyBools[property] = null;
            else if (Biota.TryRemoveProperty(property, out var entity, biotaPropertiesBoolLock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }
        public void RemoveProperty(PropertyDataId property)
        {
            if (EphemeralPropertyDataIds.ContainsKey(property))
                EphemeralPropertyDataIds[property] = null;
            else if (Biota.TryRemoveProperty(property, out var entity, biotaPropertiesDIDLock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }
        public void RemoveProperty(PropertyFloat property)
        {
            if (EphemeralPropertyFloats.ContainsKey(property))
                EphemeralPropertyFloats[property] = null;
            else if (Biota.TryRemoveProperty(property, out var entity, biotaPropertiesFloatLock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }
        public void RemoveProperty(PropertyInstanceId property)
        {
            if (EphemeralPropertyInstanceIds.ContainsKey(property))
                EphemeralPropertyInstanceIds[property] = null;
            else if (Biota.TryRemoveProperty(property, out var entity, biotaPropertiesIIDLock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }
        public void RemoveProperty(PropertyInt property)
        {
            if (EphemeralPropertyInts.ContainsKey(property))
                EphemeralPropertyInts[property] = null;
            else if (Biota.TryRemoveProperty(property, out var entity, biotaPropertiesIntLock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }
        public void RemoveProperty(PropertyInt64 property)
        {
            if (EphemeralPropertyInt64s.ContainsKey(property))
                EphemeralPropertyInt64s[property] = null;
            else if (Biota.TryRemoveProperty(property, out var entity, biotaPropertiesInt64Lock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }
        public void RemoveProperty(PropertyString property)
        {
            if (EphemeralPropertyStrings.ContainsKey(property))
                EphemeralPropertyStrings[property] = null;
            else if (Biota.TryRemoveProperty(property, out var entity, biotaPropertiesStringLock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }
        #endregion

        #region GetAllProperty Functions
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

        public Dictionary<PropertyInstanceId, uint> GetAllPropertyInstanceId()
        {
            var results = new Dictionary<PropertyInstanceId, uint>();

            foreach (var property in Biota.BiotaPropertiesIID)
                results[(PropertyInstanceId)property.Type] = property.Value;

            foreach (var property in EphemeralPropertyInstanceIds)
                if (property.Value.HasValue)
                    results[property.Key] = (uint)property.Value;

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
        #endregion


        //public Dictionary<PositionType, Position> Positions { get; set; } = new Dictionary<PositionType, Position>();

        // this is just temp so code compiles, remove it later
        // maybe this is a temp.. I haven't reviewed all of our position code yet. I don't know if we create a wrapper around the biota position table, or cache into a dictionary here
        // maybe we also need ephemeral positions..
        // What i want to avoid is duplicating data. If the biota is the authority, we can create a wrapper class like WorldObjectPosition that takes in the biota position record as a ctor (similar to CreatureAttribute, CreatureSkill, etc..)
        public Dictionary<PositionType, Position> Positions = new Dictionary<PositionType, Position>();

        public Position GetPosition(PositionType positionType) // { return Biota.GetPosition(positionType); }
        {
            bool success = Positions.TryGetValue(positionType, out var ret);

            if (!success)
            {
                var result = Biota.GetPosition(positionType, biotaPropertiesPositionLock);

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
                RemovePosition(positionType);
            else
            {
                if (!Positions.ContainsKey(positionType))
                    Positions.TryAdd(positionType, position);
                else
                    Positions[positionType] = position;

                Biota.SetPosition(positionType, position, biotaPropertiesPositionLock);
                ChangesDetected = true;
            }
        }

        public void RemovePosition(PositionType positionType)
        {
            Positions.Remove(positionType);

            if (Biota.TryRemovePosition(positionType, out var entity, biotaPropertiesPositionLock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }


        public void RemoveEnchantment(int spellId)
        {
            if (Biota.TryRemoveEnchantment(spellId, out var entity, biotaPropertiesEnchantmentLock) && ExistsInDatabase && entity.Id != 0)
                DatabaseManager.Shard.RemoveEntity(entity, null);
        }


        // SetPropertiesForWorld, SetPropertiesForContainer, SetPropertiesForVendor
        #region Utility Functions
        internal void SetPropertiesForWorld(WorldObject objectToPlaceInRelationTo)
        {
            Location = objectToPlaceInRelationTo.Location.InFrontOf(1.1f);
            PositionFlag = UpdatePositionFlag.Contact | UpdatePositionFlag.Placement | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.ZeroQx;

            Placement = ACE.Entity.Enum.Placement.Resting; // This is needed to make items lay flat on the ground.
            PlacementPosition = null;

            ContainerId = null;
            WielderId = null;
            CurrentWieldedLocation = null;
        }

        internal void SetPropertiesForContainer()
        {
            Location = null;
            PositionFlag = UpdatePositionFlag.None;

            Placement = ACE.Entity.Enum.Placement.Resting;
            if (PlacementPosition == null)
                PlacementPosition = 0;

            ParentLocation = null;
            WielderId = null;
            CurrentWieldedLocation = null;
        }

        internal void SetPropertiesForVendor()
        {
            Location = null;
            PositionFlag = UpdatePositionFlag.None;

            Placement = ACE.Entity.Enum.Placement.Resting; // This is needed to make items lay flat on the ground.
            PlacementPosition = null;

            ContainerId = null;
            WielderId = null;
            CurrentWieldedLocation = null;
        }
        #endregion


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
        public UniversalMotion CurrentMotionState { get; set; }

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

        public float Height => PhysicsObj != null ? PhysicsObj.GetHeight() : 0.0f;

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
        public virtual string Name
        {
            get => GetProperty(PropertyString.Name);
            set => SetProperty(PropertyString.Name, value);
        }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public uint WeenieClassId => Biota.WeenieClassId;

        public string WeenieClassName => DatabaseManager.World.GetCachedWeenie(WeenieClassId).ClassName;

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
            set { if (value == null) RemoveProperty(PropertyString.PluralName); else SetProperty(PropertyString.PluralName, value); }
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
            //get => (StackUnitValue * (StackSize ?? 1));
            get => GetProperty(PropertyInt.Value);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Value); else SetProperty(PropertyInt.Value, value.Value); }
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

        public uint? ContainerId
        {
            get => GetProperty(PropertyInstanceId.Container);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Container); else SetProperty(PropertyInstanceId.Container, value.Value); }
        }

        public uint? WielderId
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

        public Spell? Spell
        {
            get => (Spell?)GetProperty(PropertyDataId.Spell);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.Spell); else SetProperty(PropertyDataId.Spell, (uint)value.Value); }
        }

        /// <summary>
        /// Housing links to another packet, that needs sent.. The HouseRestrictions ACL Control list that contains all the housing data
        /// </summary>
        public uint? HouseOwner
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

        public uint? Monarch
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

        public uint? PetOwner
        {
            get => GetProperty(PropertyInstanceId.PetOwner);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.PetOwner); else SetProperty(PropertyInstanceId.PetOwner, value.Value); }
        }


        // ========================================
        // ======== Description Properties ========
        // ========================================
        // used in CalculatedDescriptionFlag()
        public bool? IsOpen
        {
            get => GetProperty(PropertyBool.Open);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Open); else SetProperty(PropertyBool.Open, value.Value); }
        }

        public bool? IsLocked
        {
            get => GetProperty(PropertyBool.Locked);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Locked); else SetProperty(PropertyBool.Locked, value.Value); }
        }

        public bool? Inscribable
        {
            get => GetProperty(PropertyBool.Inscribable);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Inscribable); else SetProperty(PropertyBool.Inscribable, value.Value); }
        }

        public bool? Stuck
        {
            get => GetProperty(PropertyBool.Stuck);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Stuck); else SetProperty(PropertyBool.Stuck, value.Value); }
        }

        public bool? Attackable
        {
            get => GetProperty(PropertyBool.Attackable);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Attackable); else SetProperty(PropertyBool.Attackable, value.Value); }
        }

        public bool? HiddenAdmin
        {
            get => GetProperty(PropertyBool.HiddenAdmin);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.HiddenAdmin); else SetProperty(PropertyBool.HiddenAdmin, value.Value); }
        }

        public bool? UiHidden
        {
            get => GetProperty(PropertyBool.UiHidden);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.UiHidden); else SetProperty(PropertyBool.UiHidden, value.Value); }
        }

        public bool? IgnoreHouseBarriers
        {
            get => GetProperty(PropertyBool.IgnoreHouseBarriers);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.IgnoreHouseBarriers); else SetProperty(PropertyBool.IgnoreHouseBarriers, value.Value); }
        }

        public bool? RequiresBackpackSlot
        {
            get => GetProperty(PropertyBool.RequiresBackpackSlot);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.RequiresBackpackSlot); else SetProperty(PropertyBool.RequiresBackpackSlot, value.Value); }
        }

        public bool? Retained
        {
            get => GetProperty(PropertyBool.Retained);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Retained); else SetProperty(PropertyBool.Retained, value.Value); }
        }

        public bool? WieldOnUse
        {
            get => GetProperty(PropertyBool.WieldOnUse);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.WieldOnUse); else SetProperty(PropertyBool.WieldOnUse, value.Value); }
        }

        public bool? AutowieldLeft
        {
            get => GetProperty(PropertyBool.AutowieldLeft);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.AutowieldLeft); else SetProperty(PropertyBool.AutowieldLeft, value.Value); }
        }


        // ========================================
        // ======== Appearance Properties =========
        // ========================================
        // Used in RandomizeFace()
        public int? Heritage
        {
            get => GetProperty(PropertyInt.HeritageGroup);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HeritageGroup); else SetProperty(PropertyInt.HeritageGroup, value.Value); }
        }

        public int? Gender
        {
            get => GetProperty(PropertyInt.Gender);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Gender); else SetProperty(PropertyInt.Gender, value.Value); }
        }

        public string HeritageGroup
        {
            get => GetProperty(PropertyString.HeritageGroup);
            set { if (value == null) RemoveProperty(PropertyString.HeritageGroup); else SetProperty(PropertyString.HeritageGroup, value); }
        }

        public string Sex
        {
            get => GetProperty(PropertyString.Sex);
            set { if (value == null) RemoveProperty(PropertyString.Sex); else SetProperty(PropertyString.Sex, value); }
        }

        public uint? HeadObjectDID
        {
            get => GetProperty(PropertyDataId.HeadObject);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.HeadObject); else SetProperty(PropertyDataId.HeadObject, value.Value); }
        }

        public uint? HairTextureDID
        {
            get => GetProperty(PropertyDataId.HairTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.HairTexture); else SetProperty(PropertyDataId.HairTexture, value.Value); }
        }

        public uint? DefaultHairTextureDID
        {
            get => GetProperty(PropertyDataId.DefaultHairTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DefaultHairTexture); else SetProperty(PropertyDataId.DefaultHairTexture, value.Value); }
        }

        public uint? HairPaletteDID
        {
            get => GetProperty(PropertyDataId.HairPalette);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.HairPalette); else SetProperty(PropertyDataId.HairPalette, value.Value); }
        }

        public uint? SkinPaletteDID
        {
            get => GetProperty(PropertyDataId.SkinPalette);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.SkinPalette); else SetProperty(PropertyDataId.SkinPalette, value.Value); }
        }

        public uint? EyesPaletteDID
        {
            get => GetProperty(PropertyDataId.EyesPalette);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.EyesPalette); else SetProperty(PropertyDataId.EyesPalette, value.Value); }
        }

        public uint? EyesTextureDID
        {
            get => GetProperty(PropertyDataId.EyesTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.EyesTexture); else SetProperty(PropertyDataId.EyesTexture, value.Value); }
        }

        public uint? DefaultEyesTextureDID
        {
            get => GetProperty(PropertyDataId.DefaultEyesTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DefaultEyesTexture); else SetProperty(PropertyDataId.DefaultEyesTexture, value.Value); }
        }

        public uint? NoseTextureDID
        {
            get => GetProperty(PropertyDataId.NoseTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.NoseTexture); else SetProperty(PropertyDataId.NoseTexture, value.Value); }
        }

        public uint? DefaultNoseTextureDID
        {
            get => GetProperty(PropertyDataId.DefaultNoseTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DefaultNoseTexture); else SetProperty(PropertyDataId.DefaultNoseTexture, value.Value); }
        }

        public uint? MouthTextureDID
        {
            get => GetProperty(PropertyDataId.MouthTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.MouthTexture); else SetProperty(PropertyDataId.MouthTexture, value.Value); }
        }

        public uint? DefaultMouthTextureDID
        {
            get => GetProperty(PropertyDataId.DefaultMouthTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DefaultMouthTexture); else SetProperty(PropertyDataId.DefaultMouthTexture, value.Value); }
        }

        public uint? PaletteBaseDID
        {
            get => GetProperty(PropertyDataId.PaletteBase);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.PaletteBase); else SetProperty(PropertyDataId.PaletteBase, value.Value); }
        }


        // ========================================
        // =========== Other Properties ===========
        // ========================================

        public uint? EyesTexture
        {
            get => GetProperty(PropertyDataId.EyesTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.EyesTexture); else SetProperty(PropertyDataId.EyesTexture, value.Value); }
        }

        public uint? DefaultEyesTexture
        {
            get => GetProperty(PropertyDataId.DefaultEyesTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DefaultEyesTexture); else SetProperty(PropertyDataId.DefaultEyesTexture, value.Value); }
        }

        public uint? NoseTexture
        {
            get => GetProperty(PropertyDataId.NoseTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.NoseTexture); else SetProperty(PropertyDataId.NoseTexture, value.Value); }
        }

        public uint? DefaultNoseTexture
        {
            get => GetProperty(PropertyDataId.DefaultNoseTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DefaultNoseTexture); else SetProperty(PropertyDataId.DefaultNoseTexture, value.Value); }
        }

        public uint? MouthTexture
        {
            get => GetProperty(PropertyDataId.MouthTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.MouthTexture); else SetProperty(PropertyDataId.MouthTexture, value.Value); }
        }

        public uint? DefaultMouthTexture
        {
            get => GetProperty(PropertyDataId.DefaultMouthTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DefaultMouthTexture); else SetProperty(PropertyDataId.DefaultMouthTexture, value.Value); }
        }

        public uint? HairTexture
        {
            get => GetProperty(PropertyDataId.HairTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.HairTexture); else SetProperty(PropertyDataId.HairTexture, value.Value); }
        }

        public uint? DefaultHairTexture
        {
            get => GetProperty(PropertyDataId.DefaultHairTexture);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DefaultHairTexture); else SetProperty(PropertyDataId.DefaultHairTexture, value.Value); }
        }

        public uint? HeadObject
        {
            get => GetProperty(PropertyDataId.HeadObject);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.HeadObject); else SetProperty(PropertyDataId.HeadObject, value.Value); }
        }

        public uint? SkinPalette
        {
            get => GetProperty(PropertyDataId.SkinPalette);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.SkinPalette); else SetProperty(PropertyDataId.SkinPalette, value.Value); }
        }

        public uint? HairPalette
        {
            get => GetProperty(PropertyDataId.HairPalette);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.HairPalette); else SetProperty(PropertyDataId.HairPalette, value.Value); }
        }

        public uint? EyesPalette
        {
            get => GetProperty(PropertyDataId.EyesPalette);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.EyesPalette); else SetProperty(PropertyDataId.EyesPalette, value.Value); }
        }

        public int? Level
        {
            get => GetProperty(PropertyInt.Level);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Level); else SetProperty(PropertyInt.Level, value.Value); }
        }

        public uint? PaletteId
        {
            get => GetProperty(PropertyDataId.PaletteBase);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.PaletteBase); else SetProperty(PropertyDataId.PaletteBase, value.Value); }
        }


        //public uint? SetupDID
        //{
        //    get { return GetProperty(PropertyDataId.Setup); }
        //    set { SetProperty(PropertyDataId.Setup, value); }
        //}


        //public uint? MotionTableDID
        //{
        //    get { return GetProperty(PropertyDataId.MotionTable); }
        //    set { SetProperty(PropertyDataId.MotionTable, value); }
        //}


        //public uint? SoundTableDID
        //{
        //    get { return GetProperty(PropertyDataId.SoundTable); }
        //    set { SetProperty(PropertyDataId.SoundTable, value); }
        //}


        //public uint? PhysicsEffectTableDID
        //{
        //    get { return GetProperty(PropertyDataId.PhysicsEffectTable); }
        //    set { SetProperty(PropertyDataId.PhysicsEffectTable, value); }
        //}


        //public uint? CombatTableDID
        //{
        //    get { return GetProperty(PropertyDataId.CombatTable); }
        //    set { SetProperty(PropertyDataId.CombatTable, value); }
        //}


        //public int? PhysicsState
        //{
        //    get { return GetProperty(PropertyInt.PhysicsState); }
        //    set { SetProperty(PropertyInt.PhysicsState, value); }
        //}


        //public uint? IconDID
        //{
        //    get { return GetProperty(PropertyDataId.Icon); }
        //    set { SetProperty(PropertyDataId.Icon, value); }
        //}

        //public string PluralName
        //{
        //    get { return GetProperty(PropertyString.PluralName); }
        //    set { SetProperty(PropertyString.PluralName, value); }
        //}


        //public byte? ItemsCapacity
        //{
        //    get { return (byte?)GetProperty(PropertyInt.ItemsCapacity); }
        //    set { SetProperty(PropertyInt.ItemsCapacity, (int)value); }
        //}


        //public byte? ContainersCapacity
        //{
        //    get { return (byte?)GetProperty(PropertyInt.ContainersCapacity); }
        //    set { SetProperty(PropertyInt.ContainersCapacity, (int)value); }
        //}

        public int? UseCreateContractId
        {
            get => GetProperty(PropertyInt.UseCreatesContractId);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.UseCreatesContractId); else SetProperty(PropertyInt.UseCreatesContractId, value.Value); }
        }


        //public int? ItemUseable
        //{
        //    get { return GetProperty(PropertyInt.ItemUseable); }
        //    set { SetProperty(PropertyInt.ItemUseable, (int)value); }
        //}      

        //public uint? ContainerIID
        //{
        //    get { return GetProperty(PropertyInstanceId.Container); }
        //    set { SetProperty(PropertyInstanceId.Container, value); }
        //}

        //public uint? WielderIID
        //{
        //    get { return GetProperty(PropertyInstanceId.Wielder); }
        //    set { SetProperty(PropertyInstanceId.Wielder, value); }
        //}


        //public uint? GeneratorIID
        //{
        //    get { return GetProperty(PropertyInstanceId.Generator); }
        //    set { SetProperty(PropertyInstanceId.Generator, value); }
        //}

        //public int? ClothingPriority
        //{
        //    get { return GetProperty(PropertyInt.ClothingPriority); }
        //    set { SetProperty(PropertyInt.ClothingPriority, value); }
        //}


        //public byte? RadarBlipColor
        //{
        //    get { return (byte?)GetProperty(PropertyInt.RadarBlipColor); }
        //    set { SetProperty(PropertyInt.RadarBlipColor, value); }
        //}


        //public byte? ShowableOnRadar
        //{
        //    get { return (byte?)GetProperty(PropertyInt.ShowableOnRadar); }
        //    set { SetProperty(PropertyInt.ShowableOnRadar, value); }
        //}


        //public ushort? PhysicsScriptDID
        //{
        //    get { return (ushort?)GetProperty(PropertyDataId.PhysicsScript); }
        //    set { SetProperty(PropertyDataId.PhysicsScript, value); }
        //}


        //public uint? IconOverlayDID
        //{
        //    get { return GetProperty(PropertyDataId.IconOverlay); }
        //    set { SetProperty(PropertyDataId.IconOverlay, value); }
        //}


        //public uint? IconUnderlayDID
        //{
        //    get { return GetProperty(PropertyDataId.IconUnderlay); }
        //    set { SetProperty(PropertyDataId.IconUnderlay, value); }
        //}


        //public int? SharedCooldown
        //{
        //    get { return GetProperty(PropertyInt.SharedCooldown); }
        //    set { SetProperty(PropertyInt.SharedCooldown, value); }
        //}



        //// Wielder is Parent, No such thing as PropertyInstanceId.Parent

        //public uint? ParentIID
        //{
        //    get { return GetProperty(PropertyInstanceId.Wielder); }
        //    set { SetProperty(PropertyInstanceId.Wielder, value); }
        //}


        //public float? DefaultScale
        //{
        //    get { return (float?)GetProperty(PropertyFloat.DefaultScale); }
        //    set { SetProperty(PropertyFloat.DefaultScale, value); }
        //}





        //public float? PhysicsScriptIntensity
        //{
        //    get { return (float?)GetProperty(PropertyFloat.PhysicsScriptIntensity); }
        //    set { SetProperty(PropertyFloat.PhysicsScriptIntensity, value); }
        //}


        //public uint? PaletteBaseDID
        //{
        //    get { return GetProperty(PropertyDataId.PaletteBase); }
        //    set { SetProperty(PropertyDataId.PaletteBase, value); }
        //}


        //public uint? ClothingBaseDID
        //{
        //    get { return GetProperty(PropertyDataId.ClothingBase); }
        //    set { SetProperty(PropertyDataId.ClothingBase, value); }
        //}


        ////public uint? AccountId
        ////{
        ////    get { return GetProperty(PropertyInstanceId.Account); }
        ////    set { SetProperty(PropertyInstanceId.Account, value); }
        ////}





        //public bool? GeneratorStatus
        //{
        //    get { return GetProperty(PropertyBool.GeneratorStatus); }
        //    set { SetProperty(PropertyBool.GeneratorStatus, value); }
        //}


        //public bool? GeneratorEnteredWorld
        //{
        //    get { return GetProperty(PropertyBool.GeneratorEnteredWorld); }
        //    set { SetProperty(PropertyBool.GeneratorEnteredWorld, value); }
        //}


        //public bool? GeneratorDisabled
        //{
        //    get { return GetProperty(PropertyBool.GeneratorDisabled); }
        //    set { SetProperty(PropertyBool.GeneratorDisabled, value); }
        //}


        //public bool? GeneratedTreasureItem
        //{
        //    get { return GetProperty(PropertyBool.GeneratedTreasureItem); }
        //    set { SetProperty(PropertyBool.GeneratedTreasureItem, value); }
        //}


        //public bool? GeneratorAutomaticDestruction
        //{
        //    get { return GetProperty(PropertyBool.GeneratorAutomaticDestruction); }
        //    set { SetProperty(PropertyBool.GeneratorAutomaticDestruction, value); }
        //}


        //public bool? CanGenerateRare
        //{
        //    get { return GetProperty(PropertyBool.CanGenerateRare); }
        //    set { SetProperty(PropertyBool.CanGenerateRare, value); }
        //}


        //public bool? CorpseGeneratedRare
        //{
        //    get { return GetProperty(PropertyBool.CorpseGeneratedRare); }
        //    set { SetProperty(PropertyBool.CorpseGeneratedRare, value); }
        //}




        //public bool? ChestRegenOnClose
        //{
        //    get { return GetProperty(PropertyBool.ChestRegenOnClose); }
        //    set { SetProperty(PropertyBool.ChestRegenOnClose, value); }
        //}


        //public bool? ChestClearedWhenClosed
        //{
        //    get { return GetProperty(PropertyBool.ChestClearedWhenClosed); }
        //    set { SetProperty(PropertyBool.ChestClearedWhenClosed, value); }
        //}


        //public int? GeneratorTimeType
        //{
        //    get { return GetProperty(PropertyInt.GeneratorTimeType); }
        //    set { SetProperty(PropertyInt.GeneratorTimeType, value); }
        //}


        ////public int? GeneratorProbability
        ////{
        ////    get { return GetProperty(PropertyInt.GeneratorProbability); }
        ////    set { SetProperty(PropertyInt.GeneratorProbability, value); }
        ////}





        //public int? GeneratorType
        //{
        //    get { return GetProperty(PropertyInt.GeneratorType); }
        //    set { SetProperty(PropertyInt.GeneratorType, value); }
        //}


        //public int? ActivationCreateClass
        //{
        //    get { return GetProperty(PropertyInt.ActivationCreateClass); }
        //    set { SetProperty(PropertyInt.ActivationCreateClass, value); }
        //}





        //public bool? Open
        //{
        //    get { return GetProperty(PropertyBool.Open); }
        //    set { SetProperty(PropertyBool.Open, value); }
        //}


        //public bool? Locked
        //{
        //    get { return GetProperty(PropertyBool.Locked); }
        //    set { SetProperty(PropertyBool.Locked, value); }
        //}


        //public bool? DefaultLocked
        //{
        //    get { return GetProperty(PropertyBool.DefaultLocked); }
        //    set { SetProperty(PropertyBool.DefaultLocked, value); }
        //}


        //public bool? DefaultOpen
        //{
        //    get { return GetProperty(PropertyBool.DefaultOpen); }
        //    set { SetProperty(PropertyBool.DefaultOpen, value); }
        //}


        //public float? ResetInterval
        //{
        //    get { return (float?)GetProperty(PropertyFloat.ResetInterval); }
        //    set { SetProperty(PropertyFloat.ResetInterval, value); }
        //}


        //public double? ResetTimestamp
        //{
        //    get { return GetProperty(PropertyFloat.ResetTimestamp); }
        //    set { SetDoubleTimestamp(PropertyFloat.ResetTimestamp); }
        //}


        //public double? UseTimestamp
        //{
        //    get { return GetProperty(PropertyFloat.UseTimestamp); }
        //    set { SetDoubleTimestamp(PropertyFloat.UseTimestamp); }
        //}


        //public double? UseLockTimestamp
        //{
        //    get { return GetProperty(PropertyFloat.UseLockTimestamp); }
        //    set { SetDoubleTimestamp(PropertyFloat.UseLockTimestamp); }
        //}


        //public uint? LastUnlockerIID
        //{
        //    get { return GetProperty(PropertyInstanceId.LastUnlocker); }
        //    set { SetProperty(PropertyInstanceId.LastUnlocker, value); }
        //}


        //public string KeyCode
        //{
        //    get { return GetProperty(PropertyString.KeyCode); }
        //    set { SetProperty(PropertyString.KeyCode, value); }
        //}


        //public string LockCode
        //{
        //    get { return GetProperty(PropertyString.LockCode); }
        //    set { SetProperty(PropertyString.LockCode, value); }
        //}


        //public int? ResistLockpick
        //{
        //    get { return GetProperty(PropertyInt.ResistLockpick); }
        //    set { SetProperty(PropertyInt.ResistLockpick, value); }
        //}


        //public int? AppraisalLockpickSuccessPercent
        //{
        //    get { return GetProperty(PropertyInt.AppraisalLockpickSuccessPercent); }
        //    set { SetProperty(PropertyInt.AppraisalLockpickSuccessPercent, value); }
        //}


        //public int? MinLevel
        //{
        //    get { return GetProperty(PropertyInt.MinLevel); }
        //    set { SetProperty(PropertyInt.MinLevel, value); }
        //}


        //public int? MaxLevel
        //{
        //    get { return GetProperty(PropertyInt.MaxLevel); }
        //    set { SetProperty(PropertyInt.MaxLevel, value); }
        //}


        //public int? PortalBitmask
        //{
        //    get { return GetProperty(PropertyInt.PortalBitmask); }
        //    set { SetProperty(PropertyInt.PortalBitmask, value); }
        //}


        //public string AppraisalPortalDestination
        //{
        //    get { return GetProperty(PropertyString.AppraisalPortalDestination); }
        //    set { SetProperty(PropertyString.AppraisalPortalDestination, value); }
        //}


        //public string ShortDesc
        //{
        //    get { return GetProperty(PropertyString.ShortDesc); }
        //    set { SetProperty(PropertyString.ShortDesc, value); }
        //}





        //public string UseMessage
        //{
        //    get { return GetProperty(PropertyString.UseMessage); }
        //    set { SetProperty(PropertyString.UseMessage, value); }
        //}


        //public bool? PortalShowDestination
        //{
        //    get { return GetProperty(PropertyBool.PortalShowDestination); }
        //    set { SetProperty(PropertyBool.PortalShowDestination, value); }
        //}





        //public string Title
        //{
        //    get { return GetProperty(PropertyString.Title); }
        //    set { SetProperty(PropertyString.Title, value); }
        //}


        //public string Template
        //{
        //    get { return GetProperty(PropertyString.Template); }
        //    set { SetProperty(PropertyString.Template, value); }
        //}


        //public string DisplayName
        //{
        //    get { return GetProperty(PropertyString.DisplayName); }
        //    set { SetProperty(PropertyString.DisplayName, value); }
        //}


        //public int? CharacterTitleId
        //{
        //    get { return GetProperty(PropertyInt.CharacterTitleId); }
        //    set { SetProperty(PropertyInt.CharacterTitleId, value); }
        //}


        //public int? NumCharacterTitles
        //{
        //    get { return GetProperty(PropertyInt.NumCharacterTitles); }
        //    set { SetProperty(PropertyInt.NumCharacterTitles, value); }
        //}


        public double? CreationTimestamp
        {
            get => GetProperty(PropertyFloat.CreationTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.CreationTimestamp); else SetProperty(PropertyFloat.CreationTimestamp, value.Value); }
        }





        //public bool? AdvocateState
        //{
        //    get { return GetProperty(PropertyBool.AdvocateState); }
        //    set { SetProperty(PropertyBool.AdvocateState, value); }
        //}





        //public bool? PkWounder
        //{
        //    get { return GetProperty(PropertyBool.PkWounder); }
        //    set { SetProperty(PropertyBool.PkWounder, value); }
        //}


        //public bool? PkKiller
        //{
        //    get { return GetProperty(PropertyBool.PkKiller); }
        //    set { SetProperty(PropertyBool.PkKiller, value); }
        //}


        //public bool? UnderLifestoneProtection
        //{
        //    get { return GetProperty(PropertyBool.UnderLifestoneProtection); }
        //    set { SetProperty(PropertyBool.UnderLifestoneProtection, value); }
        //}


        //public bool? DefaultOn
        //{
        //    get { return GetProperty(PropertyBool.DefaultOn); }
        //    set { SetProperty(PropertyBool.DefaultOn, value); }
        //}





        //public bool? AdvocateQuest
        //{
        //    get { return GetProperty(PropertyBool.AdvocateQuest); }
        //    set { SetProperty(PropertyBool.AdvocateQuest, value); }
        //}


        //public bool? IsAdvocate
        //{
        //    get { return GetProperty(PropertyBool.IsAdvocate); }
        //    set { SetProperty(PropertyBool.IsAdvocate, value); }
        //}


        //public bool? IsSentinel
        //{
        //    get { return GetProperty(PropertyBool.IsSentinel); }
        //    set { SetProperty(PropertyBool.IsSentinel, value); }
        //}





        //public bool? IgnorePortalRestrictions
        //{
        //    get { return GetProperty(PropertyBool.IgnorePortalRestrictions); }
        //    set { SetProperty(PropertyBool.IgnorePortalRestrictions, value); }
        //}





        //public bool? Invincible
        //{
        //    get { return GetProperty(PropertyBool.Invincible); }
        //    set { SetProperty(PropertyBool.Invincible, value); }
        //}


        //public bool? IsGagged
        //{
        //    get { return GetProperty(PropertyBool.IsGagged); }
        //    set { SetProperty(PropertyBool.IsGagged, value); }
        //}


        //public bool? Afk
        //{
        //    get { return GetProperty(PropertyBool.Afk); }
        //    set { SetProperty(PropertyBool.Afk, value); }
        //}


        public bool? IgnoreAuthor
        {
            get => GetProperty(PropertyBool.IgnoreAuthor);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.IgnoreAuthor); else SetProperty(PropertyBool.IgnoreAuthor, value.Value); }
        }





        //public bool? VendorService
        //{
        //    get { return GetProperty(PropertyBool.VendorService); }
        //    set { SetProperty(PropertyBool.VendorService, value); }
        //}





        public bool UseBackpackSlot => (GetProperty(PropertyBool.RequiresBackpackSlot) ?? false) || WeenieType == WeenieType.Container;

        public int? PlacementPosition
        {
            get => GetProperty(PropertyInt.PlacementPosition);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PlacementPosition); else SetProperty(PropertyInt.PlacementPosition, value.Value); }
        }

        //public uint? AllowedActivator
        //{
        //    get { return GetProperty(PropertyInstanceId.AllowedActivator); }
        //    set { SetProperty(PropertyInstanceId.AllowedActivator, value); }
        //}




        // todo is this a book only property? If so, it should go with the Books properties
        public string Inscription
        {
            get => GetProperty(PropertyString.Inscription);
            set { if (value == null) RemoveProperty(PropertyString.Inscription); else SetProperty(PropertyString.Inscription, value); }
        }

        // todo should these be moved to Book_Properties.cs?
        #region Books
        public string ScribeName
        {
            get => GetProperty(PropertyString.ScribeName);
            set { if (value == null) RemoveProperty(PropertyString.ScribeName); else SetProperty(PropertyString.ScribeName, value); }
        }


        public string ScribeAccount
        {
            get => GetProperty(PropertyString.ScribeAccount);
            set { if (value == null) RemoveProperty(PropertyString.ScribeAccount); else SetProperty(PropertyString.ScribeAccount, value); }
        }


        public uint? ScribeIID
        {
            get => GetProperty(PropertyInstanceId.Scribe);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Scribe); else SetProperty(PropertyInstanceId.Scribe, value.Value); }
        }


        public int? AppraisalPages
        {
            get => GetProperty(PropertyInt.AppraisalPages);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AppraisalPages); else SetProperty(PropertyInt.AppraisalPages, value.Value); }
        }


        public int? AppraisalMaxPages
        {
            get => GetProperty(PropertyInt.AppraisalMaxPages);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AppraisalMaxPages); else SetProperty(PropertyInt.AppraisalMaxPages, value.Value); }
        }
        #endregion


        //public int? AvailableCharacter
        //{
        //    get { return GetProperty(PropertyInt.AvailableCharacter); }
        //    set { SetProperty(PropertyInt.AvailableCharacter, value); }
        //}

        public virtual int? StackUnitValue
        {
            get => GetProperty(PropertyInt.StackUnitValue);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.StackUnitValue); else SetProperty(PropertyInt.StackUnitValue, value.Value); }
        }

        public virtual int? StackUnitEncumbrance
        {
            get => GetProperty(PropertyInt.StackUnitEncumbrance);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.StackUnitEncumbrance); else SetProperty(PropertyInt.StackUnitEncumbrance, value.Value); }
        }

        public virtual int? EncumbranceVal
        {
            get => GetProperty(PropertyInt.EncumbranceVal);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.EncumbranceVal); else SetProperty(PropertyInt.EncumbranceVal, value.Value); }
        }

        public uint? PaletteBaseId
        {
            get => GetProperty(PropertyDataId.PaletteBase);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.PaletteBase); else SetProperty(PropertyDataId.PaletteBase, value.Value); }
        }

        public ParentLocation? ParentLocation
        {
            get => (ParentLocation?)GetProperty(PropertyInt.ParentLocation);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ParentLocation); else SetProperty(PropertyInt.ParentLocation, (int)value.Value); }
        }

        public CombatStyle? DefaultCombatStyle
        {
            get => (CombatStyle?)GetProperty(PropertyInt.DefaultCombatStyle);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.DefaultCombatStyle); else SetProperty(PropertyInt.DefaultCombatStyle, (int)value.Value); }
        }

        public uint? GeneratorId
        {
            get => GetProperty(PropertyInstanceId.Generator);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Generator); else SetProperty(PropertyInstanceId.Generator, value.Value); }
        }

        public uint? ClothingBase
        {
            get => GetProperty(PropertyDataId.ClothingBase);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.ClothingBase); else SetProperty(PropertyDataId.ClothingBase, value.Value); }
        }

        public int? ItemCurMana
        {
            get => GetProperty(PropertyInt.ItemCurMana);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemCurMana); else SetProperty(PropertyInt.ItemCurMana, value.Value); }
        }

        public int? ItemMaxMana
        {
            get => GetProperty(PropertyInt.ItemMaxMana);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemMaxMana); else SetProperty(PropertyInt.ItemMaxMana, value.Value); }
        }

        public bool? NpcLooksLikeObject
        {
            get => GetProperty(PropertyBool.NpcLooksLikeObject);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.NpcLooksLikeObject); else SetProperty(PropertyBool.NpcLooksLikeObject, value.Value); }
        }

        public bool? SuppressGenerateEffect
        {
            get => GetProperty(PropertyBool.SuppressGenerateEffect);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.SuppressGenerateEffect); else SetProperty(PropertyBool.SuppressGenerateEffect, value.Value); }
        }

        public CreatureType? CreatureType
        {
            get => (CreatureType?)GetProperty(PropertyInt.CreatureType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CreatureType); else SetProperty(PropertyInt.CreatureType, (int)value.Value); }
        }

        public string LongDesc
        {
            get => GetProperty(PropertyString.LongDesc);
            set { if (value == null) RemoveProperty(PropertyString.LongDesc); else SetProperty(PropertyString.LongDesc, value); }
        }

        public string Use
        {
            get => GetProperty(PropertyString.Use);
            set { if (value == null) RemoveProperty(PropertyString.Use); else SetProperty(PropertyString.Use, value); }
        }

        public int? Boost
        {
            get => GetProperty(PropertyInt.BoostValue);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.BoostValue); else SetProperty(PropertyInt.BoostValue, value.Value); }
        }

        public uint? SpellDID
        {
            get => GetProperty(PropertyDataId.Spell);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.Spell); else SetProperty(PropertyDataId.Spell, value.Value); }
        }

        public int? BoostEnum
        {
            get => GetProperty(PropertyInt.BoosterEnum);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.BoosterEnum); else SetProperty(PropertyInt.BoosterEnum, value.Value); }
        }

        public double? HealkitMod
        {
            get => GetProperty(PropertyFloat.HealkitMod);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.HealkitMod); else SetProperty(PropertyFloat.HealkitMod, value.Value); }
        }

        public virtual int? CoinValue
        {
            get => GetProperty(PropertyInt.CoinValue);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CoinValue); else SetProperty(PropertyInt.CoinValue, value.Value); }
        }

        public int? ChessGamesLost
        {
            get => GetProperty(PropertyInt.ChessGamesLost);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChessGamesLost); else SetProperty(PropertyInt.ChessGamesLost, value.Value); }
        }

        public int? ChessGamesWon
        {
            get => GetProperty(PropertyInt.ChessGamesWon);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChessGamesWon); else SetProperty(PropertyInt.ChessGamesWon, value.Value); }
        }

        public int? ChessRank
        {
            get => GetProperty(PropertyInt.ChessRank);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChessRank); else SetProperty(PropertyInt.ChessRank, value.Value); }
        }

        public int? ChessTotalGames
        {
            get => GetProperty(PropertyInt.ChessTotalGames);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChessTotalGames); else SetProperty(PropertyInt.ChessTotalGames, value.Value); }
        }

        public int? MerchandiseItemTypes
        {
            get => GetProperty(PropertyInt.MerchandiseItemTypes);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MerchandiseItemTypes); else SetProperty(PropertyInt.MerchandiseItemTypes, value.Value); }
        }

        public int? MerchandiseMinValue
        {
            get => GetProperty(PropertyInt.MerchandiseMinValue);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MerchandiseMinValue); else SetProperty(PropertyInt.MerchandiseMinValue, value.Value); }
        }

        public int? MerchandiseMaxValue
        {
            get => GetProperty(PropertyInt.MerchandiseMaxValue);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MerchandiseMaxValue); else SetProperty(PropertyInt.MerchandiseMaxValue, value.Value); }
        }

        public double? BuyPrice
        {
            get => GetProperty(PropertyFloat.BuyPrice);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.BuyPrice); else SetProperty(PropertyFloat.BuyPrice, value.Value); }
        }

        public double? SellPrice
        {
            get => GetProperty(PropertyFloat.SellPrice);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.SellPrice); else SetProperty(PropertyFloat.SellPrice, value.Value); }
        }

        public bool? DealMagicalItems
        {
            get => GetProperty(PropertyBool.DealMagicalItems);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.DealMagicalItems); else SetProperty(PropertyBool.DealMagicalItems, value.Value); }
        }

        public uint? AlternateCurrencyDID
        {
            get => GetProperty(PropertyDataId.AlternateCurrency);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.AlternateCurrency); else SetProperty(PropertyDataId.AlternateCurrency, value.Value); }
        }

        public double? HeartbeatInterval
        {
            get => GetProperty(PropertyFloat.HeartbeatInterval);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.SellPrice); else SetProperty(PropertyFloat.HeartbeatInterval, value.Value); }
        }

        public int? InitGeneratedObjects
        {
            get => GetProperty(PropertyInt.InitGeneratedObjects);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.InitGeneratedObjects); else SetProperty(PropertyInt.InitGeneratedObjects, value.Value); }
        }

        public int? MaxGeneratedObjects
        {
            get => GetProperty(PropertyInt.MaxGeneratedObjects);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MaxGeneratedObjects); else SetProperty(PropertyInt.MaxGeneratedObjects, value.Value); }
        }

        public double? RegenerationInterval
        {
            get => GetProperty(PropertyFloat.RegenerationInterval);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.RegenerationInterval); else SetProperty(PropertyFloat.RegenerationInterval, value.Value); }
        }

        public bool? GeneratorEnteredWorld
        {
            get => GetProperty(PropertyBool.GeneratorEnteredWorld);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.GeneratorEnteredWorld); else SetProperty(PropertyBool.GeneratorEnteredWorld, value.Value); }
        }

        public bool? Visibility
        {
            get => GetProperty(PropertyBool.Visibility);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Visibility); else SetProperty(PropertyBool.Visibility, value.Value); }
        }

        public int? PaletteTemplate
        {
            get => GetProperty(PropertyInt.PaletteTemplate);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PaletteTemplate); else SetProperty(PropertyInt.PaletteTemplate, value.Value); }
        }

        public double? Shade
        {
            get => GetProperty(PropertyFloat.Shade);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.Shade); else SetProperty(PropertyFloat.Shade, value.Value); }
        }



        // ========================================
        //= ======== Position Properties ==========
        // ========================================
        //public Position Location
        //{
        //    get { return GetPosition(PositionType.Location); }
        //    set { SetPosition(PositionType.Location, value); }
        //}


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


        public uint? CurrentCombatTarget
        {
            get => GetProperty(PropertyInstanceId.CurrentCombatTarget);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.CurrentCombatTarget); else SetProperty(PropertyInstanceId.CurrentCombatTarget, value.Value); }
        }

        public uint? CurrentEnemy
        {
            get => GetProperty(PropertyInstanceId.CurrentEnemy);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.CurrentEnemy); else SetProperty(PropertyInstanceId.CurrentEnemy, value.Value); }
        }

        public uint? CurrentAttacker
        {
            get => GetProperty(PropertyInstanceId.CurrentAttacker);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.CurrentAttacker); else SetProperty(PropertyInstanceId.CurrentAttacker, value.Value); }
        }

        public uint? CurrentDamager
        {
            get => GetProperty(PropertyInstanceId.CurrentDamager);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.CurrentDamager); else SetProperty(PropertyInstanceId.CurrentDamager, value.Value); }
        }

        public uint? CurrentFollowTarget
        {
            get => GetProperty(PropertyInstanceId.CurrentFollowTarget);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.CurrentFollowTarget); else SetProperty(PropertyInstanceId.CurrentFollowTarget, value.Value); }
        }

        public uint? CurrentAppraisalTarget
        {
            get => GetProperty(PropertyInstanceId.CurrentAppraisalTarget);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.CurrentAppraisalTarget); else SetProperty(PropertyInstanceId.CurrentAppraisalTarget, value.Value); }
        }

        public uint? CurrentFellowshipAppraisalTarget
        {
            get => GetProperty(PropertyInstanceId.CurrentFellowshipAppraisalTarget);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.CurrentFellowshipAppraisalTarget); else SetProperty(PropertyInstanceId.CurrentFellowshipAppraisalTarget, value.Value); }
        }

        public uint? CombatTarget
        {
            get => GetProperty(PropertyInstanceId.CombatTarget);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.CombatTarget); else SetProperty(PropertyInstanceId.CombatTarget, value.Value); }
        }

        public uint? HealthQueryTarget
        {
            get => GetProperty(PropertyInstanceId.HealthQueryTarget);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.HealthQueryTarget); else SetProperty(PropertyInstanceId.HealthQueryTarget, value.Value); }
        }

        public uint? ManaQueryTarget
        {
            get => GetProperty(PropertyInstanceId.ManaQueryTarget);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.ManaQueryTarget); else SetProperty(PropertyInstanceId.ManaQueryTarget, value.Value); }
        }

        public uint? RequestedAppraisalTarget
        {
            get => GetProperty(PropertyInstanceId.RequestedAppraisalTarget);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.RequestedAppraisalTarget); else SetProperty(PropertyInstanceId.RequestedAppraisalTarget, value.Value); }
        }

        public int? PkLevelModifier
        {
            get => GetProperty(PropertyInt.PkLevelModifier);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PkLevelModifier); else SetProperty(PropertyInt.PkLevelModifier, value.Value); }
        }

        public PlayerKillerStatus? PlayerKillerStatus
        {
            get => (PlayerKillerStatus?)GetProperty(PropertyInt.PlayerKillerStatus);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PlayerKillerStatus); else SetProperty(PropertyInt.PlayerKillerStatus, (int)value.Value); }
        }

        public CloakStatus? CloakStatus
        {
            get => (CloakStatus?)GetProperty(PropertyInt.CloakStatus);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CloakStatus); else SetProperty(PropertyInt.CloakStatus, (int)value.Value); }
        }

        public bool? IgnorePortalRestrictions
        {
            get => GetProperty(PropertyBool.IgnorePortalRestrictions);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.IgnorePortalRestrictions); else SetProperty(PropertyBool.IgnorePortalRestrictions, value.Value); }
        }

        public bool? Invincible
        {
            get => GetProperty(PropertyBool.Invincible);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.Invincible); else SetProperty(PropertyBool.Invincible, value.Value); }
        }

        public int? XpOverride
        {
            get => GetProperty(PropertyInt.XpOverride);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.XpOverride); else SetProperty(PropertyInt.XpOverride, value.Value); }
        }

        public bool? FirstEnterWorldDone
        {
            get => GetProperty(PropertyBool.FirstEnterWorldDone);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.FirstEnterWorldDone); else SetProperty(PropertyBool.FirstEnterWorldDone, value.Value); }
        }
    }
}
