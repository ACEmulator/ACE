using System;
using System.Collections.Generic;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;

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
        // These dictionaries should ONLY be referenced by SetEphemeralValues, GetProperty, SetProperty and RemoveProperty functions.
        // They should NOT be accessed directly to get property values.
        private readonly Dictionary<PropertyBool, bool?> ephemeralPropertyBools = new Dictionary<PropertyBool, bool?>();
        private readonly Dictionary<PropertyDataId, uint?> ephemeralPropertyDataIds = new Dictionary<PropertyDataId, uint?>();
        private readonly Dictionary<PropertyFloat, double?> ephemeralPropertyFloats = new Dictionary<PropertyFloat, double?>();
        private readonly Dictionary<PropertyInstanceId, uint?> ephemeralPropertyInstanceIds = new Dictionary<PropertyInstanceId, uint?>();
        private readonly Dictionary<PropertyInt, int?> ephemeralPropertyInts = new Dictionary<PropertyInt, int?>();
        private readonly Dictionary<PropertyInt64, long?> ephemeralPropertyInt64s = new Dictionary<PropertyInt64, long?>();
        private readonly Dictionary<PropertyString, string> ephemeralPropertyStrings = new Dictionary<PropertyString, string>();

        // These dictionaries should ONLY be referenced by SetEphemeralValues, GetProperty, SetProperty and RemoveProperty functions.
        // They should NOT be accessed directly to get property values.
        private readonly Dictionary<PropertyBool, BiotaPropertiesBool> biotaPropertyBools = new Dictionary<PropertyBool, BiotaPropertiesBool>();
        private readonly Dictionary<PropertyDataId, BiotaPropertiesDID> biotaPropertyDataIds = new Dictionary<PropertyDataId, BiotaPropertiesDID>();
        private readonly Dictionary<PropertyFloat, BiotaPropertiesFloat> biotaPropertyFloats = new Dictionary<PropertyFloat, BiotaPropertiesFloat>();
        private readonly Dictionary<PropertyInstanceId, BiotaPropertiesIID> biotaPropertyInstanceIds = new Dictionary<PropertyInstanceId, BiotaPropertiesIID>();
        private readonly Dictionary<PropertyInt, BiotaPropertiesInt> biotaPropertyInts = new Dictionary<PropertyInt, BiotaPropertiesInt>();
        private readonly Dictionary<PropertyInt64, BiotaPropertiesInt64> biotaPropertyInt64s = new Dictionary<PropertyInt64, BiotaPropertiesInt64>();
        private readonly Dictionary<PropertyString, BiotaPropertiesString> biotaPropertyStrings = new Dictionary<PropertyString, BiotaPropertiesString>();

        #region GetProperty Functions
        public bool? GetProperty(PropertyBool property)
        {
            if (ephemeralPropertyBools.TryGetValue(property, out var value))
                return value;

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyBools.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public uint? GetProperty(PropertyDataId property)
        {
            if (ephemeralPropertyDataIds.TryGetValue(property, out var value))
                return value;

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyDataIds.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public double? GetProperty(PropertyFloat property)
        {
            if (ephemeralPropertyFloats.TryGetValue(property, out var value))
                return value;

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyFloats.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public uint? GetProperty(PropertyInstanceId property)
        {
            if (ephemeralPropertyInstanceIds.TryGetValue(property, out var value))
                return value;

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyInstanceIds.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public int? GetProperty(PropertyInt property)
        {
            if (ephemeralPropertyInts.TryGetValue(property, out var value))
                return value;

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyInts.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public long? GetProperty(PropertyInt64 property)
        {
            if (ephemeralPropertyInt64s.TryGetValue(property, out var value))
                return value;

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyInt64s.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public string GetProperty(PropertyString property)
        {
            if (ephemeralPropertyStrings.TryGetValue(property, out var value))
                return value;

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyStrings.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        #endregion

        #region SetProperty Functions
        public void SetProperty(PropertyBool property, bool value)
        {
            if (ephemeralPropertyBools.ContainsKey(property))
                ephemeralPropertyBools[property] = value;
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyBools, out var biotaChanged);
                if (biotaChanged)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyDataId property, uint value)
        {
            if (ephemeralPropertyDataIds.ContainsKey(property))
                ephemeralPropertyDataIds[property] = value;
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyDataIds, out var biotaChanged);
                if (biotaChanged)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyFloat property, double value)
        {
            if (ephemeralPropertyFloats.ContainsKey(property))
                ephemeralPropertyFloats[property] = value;
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyFloats, out var biotaChanged);
                if (biotaChanged)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyInstanceId property, uint value)
        {
            if (ephemeralPropertyInstanceIds.ContainsKey(property))
                ephemeralPropertyInstanceIds[property] = value;
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyInstanceIds, out var biotaChanged);
                if (biotaChanged)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyInt property, int value)
        {
            if (ephemeralPropertyInts.ContainsKey(property))
                ephemeralPropertyInts[property] = value;
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyInts, out var biotaChanged);
                if (biotaChanged)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyInt64 property, long value)
        {
            if (ephemeralPropertyInt64s.ContainsKey(property))
                ephemeralPropertyInt64s[property] = value;
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyInt64s, out var biotaChanged);
                if (biotaChanged)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyString property, string value)
        {
            if (ephemeralPropertyStrings.ContainsKey(property))
                ephemeralPropertyStrings[property] = value;
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyStrings, out var biotaChanged);
                if (biotaChanged)
                    ChangesDetected = true;
            }
        }
        #endregion

        #region RemoveProperty Functions
        public void RemoveProperty(PropertyBool property)
        {
            if (ephemeralPropertyBools.ContainsKey(property))
                ephemeralPropertyBools[property] = null;
            else if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyBools))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyDataId property)
        {
            if (ephemeralPropertyDataIds.ContainsKey(property))
                ephemeralPropertyDataIds[property] = null;
            else if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyDataIds))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyFloat property)
        {
            if (ephemeralPropertyFloats.ContainsKey(property))
                ephemeralPropertyFloats[property] = null;
            else if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyFloats))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyInstanceId property)
        {
            if (ephemeralPropertyInstanceIds.ContainsKey(property))
                ephemeralPropertyInstanceIds[property] = null;
            else if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyInstanceIds))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyInt property)
        {
            if (ephemeralPropertyInts.ContainsKey(property))
                ephemeralPropertyInts[property] = null;
            else if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyInts))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyInt64 property)
        {
            if (ephemeralPropertyInt64s.ContainsKey(property))
                ephemeralPropertyInt64s[property] = null;
            else if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyInt64s))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyString property)
        {
            if (ephemeralPropertyStrings.ContainsKey(property))
                ephemeralPropertyStrings[property] = null;
            else if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyStrings))
                ChangesDetected = true;
        }
        #endregion

        #region GetAllProperty Functions
        public Dictionary<PropertyBool, bool> GetAllPropertyBools()
        {
            var results = new Dictionary<PropertyBool, bool>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                foreach (var property in Biota.BiotaPropertiesBool)
                    results[(PropertyBool)property.Type] = property.Value;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            foreach (var property in ephemeralPropertyBools)
                if (property.Value.HasValue)
                    results[property.Key] = (bool)property.Value;

            return results;
        }

        public Dictionary<PropertyDataId, uint> GetAllPropertyDataId()
        {
            var results = new Dictionary<PropertyDataId, uint>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                foreach (var property in Biota.BiotaPropertiesDID)
                    results[(PropertyDataId)property.Type] = property.Value;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            foreach (var property in ephemeralPropertyDataIds)
                if (property.Value.HasValue)
                    results[property.Key] = (uint)property.Value;

            return results;
        }

        public Dictionary<PropertyFloat, double> GetAllPropertyFloat()
        {
            var results = new Dictionary<PropertyFloat, double>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                foreach (var property in Biota.BiotaPropertiesFloat)
                    results[(PropertyFloat)property.Type] = property.Value;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            foreach (var property in ephemeralPropertyFloats)
                if (property.Value.HasValue)
                    results[property.Key] = (double)property.Value;

            return results;
        }

        public Dictionary<PropertyInstanceId, uint> GetAllPropertyInstanceId()
        {
            var results = new Dictionary<PropertyInstanceId, uint>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                foreach (var property in Biota.BiotaPropertiesIID)
                    results[(PropertyInstanceId)property.Type] = property.Value;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            foreach (var property in ephemeralPropertyInstanceIds)
                if (property.Value.HasValue)
                    results[property.Key] = (uint)property.Value;

            return results;
        }

        public Dictionary<PropertyInt, int> GetAllPropertyInt()
        {
            var results = new Dictionary<PropertyInt, int>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                foreach (var property in Biota.BiotaPropertiesInt)
                    results[(PropertyInt)property.Type] = property.Value;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            foreach (var property in ephemeralPropertyInts)
                if (property.Value.HasValue)
                    results[property.Key] = (int)property.Value;

            return results;
        }

        public Dictionary<PropertyInt64, long> GetAllPropertyInt64()
        {
            var results = new Dictionary<PropertyInt64, long>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                foreach (var property in Biota.BiotaPropertiesInt64)
                    results[(PropertyInt64)property.Type] = property.Value;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            foreach (var property in ephemeralPropertyInt64s)
                if (property.Value.HasValue)
                    results[property.Key] = (long)property.Value;

            return results;
        }

        public Dictionary<PropertyString, string> GetAllPropertyString()
        {
            var results = new Dictionary<PropertyString, string>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                foreach (var property in Biota.BiotaPropertiesString)
                    results[(PropertyString)property.Type] = property.Value;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            foreach (var property in ephemeralPropertyStrings)
                if (property.Value != null)
                    results[property.Key] = property.Value;

            return results;
        }
        #endregion


        private readonly Dictionary<PositionType, Position> ephemeralPositions = new Dictionary<PositionType, Position>();

        /// <summary>
        /// Do not reference this directly.<para />
        /// This should only be referenced by GetPosition, SetPosition, RemovePosition and SaveBiotaToDatabase.
        /// </summary>
        private readonly Dictionary<PositionType, Position> positionCache = new Dictionary<PositionType, Position>();

        public Position GetPosition(PositionType positionType)
        {
            if (ephemeralPositions.TryGetValue(positionType, out var value))
                return value;

            bool success = positionCache.TryGetValue(positionType, out var ret);

            if (!success)
            {
                var result = Biota.GetPosition(positionType, BiotaDatabaseLock);

                if (result != null)
                    positionCache.TryAdd(positionType, result);

                return result;
            }

            return ret;
        }

        public Dictionary<PositionType, Position> GetPositions()
        {
            foreach (var position in Biota.GetPositions(BiotaDatabaseLock))
            {
                // We only add new positions. We don't overwrite existing cached positions
                if (!positionCache.ContainsKey(position.Key))
                    positionCache[position.Key] = position.Value;
            }

            var result = new Dictionary<PositionType, Position>(positionCache);

            // Add the ephemeral positions over the cached positions
            foreach (var kvp in ephemeralPositions)
                result[kvp.Key] = kvp.Value;

            return result;
        }

        /// <summary>
        /// !!! VERY IMPORTANT NOTE REGARDING SetPosition !!!<para />
        /// Position objects are reference types. Lets say you want to create a new object and give it the location of a player,
        /// If you do LandscapeItem.SetPosition(PositionType.Location, Player.Location), you've now set the Location position
        /// for both the player and the LandscapeItem to the same exact object. Modifying one will affect the other.<para />
        /// The proper way to would be: LandscapeItem.SetPosition(PositionType.Location, new Position(Player.Location))<para />
        /// Any time you want to set a position of a different PositionType, or, positions between WorldObjects, you should use the Position copy constructor.
        /// </summary>
        public void SetPosition(PositionType positionType, Position position)
        {
            if (ephemeralPositions.ContainsKey(positionType))
                ephemeralPositions[positionType] = position;
            else
            {
                if (position == null)
                    RemovePosition(positionType);
                else
                {
                    positionCache[positionType] = position;

                    Biota.SetPosition(positionType, position, BiotaDatabaseLock, out var biotaChanged);
                    if (biotaChanged)
                        ChangesDetected = true;
                }
            }
        }

        public void RemovePosition(PositionType positionType)
        {
            if (ephemeralPositions.ContainsKey(positionType))
                ephemeralPositions[positionType] = null;
            else
            {
                positionCache.Remove(positionType);

                if (Biota.TryRemovePosition(positionType, out _, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }


        // SetPropertiesForWorld, SetPropertiesForContainer, SetPropertiesForVendor
        #region Utility Functions
        internal void SetPropertiesForWorld(WorldObject objectToPlaceInRelationTo, double distanceInFront, bool rotate180 = false)
        {
            var newLocation = objectToPlaceInRelationTo.Location.InFrontOf(distanceInFront, rotate180);

            SetPropertiesForWorld(newLocation);
        }

        internal void SetPropertiesForWorld(Position location)
        {
            Location = new Position(location);

            // should be sent automatically
            //PositionFlags = PositionFlags.IsGrounded | PositionFlags.HasPlacementID | PositionFlags.OrientationHasNoX | PositionFlags.OrientationHasNoY;

            Placement = ACE.Entity.Enum.Placement.Resting; // This is needed to make items lay flat on the ground.
            PlacementPosition = null;

            ContainerId = null;
            WielderId = null;
            CurrentWieldedLocation = null;
        }

        internal void SetPropertiesForContainer()
        {
            Location = null;
            PositionFlags = PositionFlags.None;

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
            PositionFlags = PositionFlags.None;

            Placement = ACE.Entity.Enum.Placement.Resting; // This is needed to make items lay flat on the ground.
            PlacementPosition = null;

            ContainerId = null;
            WielderId = null;
            CurrentWieldedLocation = null;
        }
        #endregion


        // ========================================
        // ======== Physics Desc Properties =======
        // ========================================
        // used in CalculatedPhysicsDescriptionFlag()
        public Motion CurrentMotionState { get; set; }
        public MotionCommand CurrentMotionCommand { get; set; }

        public Placement? Placement // Sometimes known as AnimationFrame
        {
            get => (Placement?)GetProperty(PropertyInt.Placement);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Placement); else SetProperty(PropertyInt.Placement, (int)value.Value); }
        }

        public float Height => PhysicsObj != null ? PhysicsObj.GetHeight() : 0.0f;

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
            get => GetProperty(PropertyDataId.PhysicsScript);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.PhysicsScript); else SetProperty(PropertyDataId.PhysicsScript, value.Value); }
        }

        public float? DefaultScriptIntensity
        {
            get => (float?)GetProperty(PropertyFloat.PhysicsScriptIntensity);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.PhysicsScriptIntensity); else SetProperty(PropertyFloat.PhysicsScriptIntensity, value.Value); }
        }


        public bool GetPhysicsState(PhysicsState state)
        {
            if (PhysicsObj == null) return false;
            return (PhysicsObj.State & state) != 0;
        }

        public void SetPhysicsState(PhysicsState state, bool? value)
        {
            if (PhysicsObj != null)
            {
                if (value.HasValue && value.Value)
                    PhysicsObj.State |= state;
                else
                    PhysicsObj.State &= ~state;   // default to false for null, should get real physics default for this field
            }
        }

        public void SetPhysicsPropertyState(PropertyBool property, PhysicsState state, bool? value)
        {
            if (value.HasValue)
            {
                SetProperty(property, value.Value);
                SetPhysicsState(state, value);
            }
            else
            {
                RemoveProperty(property);
                SetPhysicsState(state, false);  // default to false for null, should get real physics default for this field
            }
        }

        // ========================================
        // ======= Physics State Properties =======
        // ========================================
        // used in CalculatedPhysicsState()
        public bool? Static
        {
            get => GetPhysicsState(PhysicsState.Static);
            set => SetPhysicsState(PhysicsState.Static, value);
        }

        public bool? Ethereal
        {
            get => GetProperty(PropertyBool.Ethereal);  // TODO: property or physics state?
            set => SetPhysicsPropertyState(PropertyBool.Ethereal, PhysicsState.Ethereal, value);
        }

        public bool? ReportCollisions
        {
            get => GetProperty(PropertyBool.ReportCollisions);
            set => SetPhysicsPropertyState(PropertyBool.ReportCollisions, PhysicsState.ReportCollisions, value);
        }

        public bool? IgnoreCollisions
        {
            get => GetProperty(PropertyBool.IgnoreCollisions);
            set => SetPhysicsPropertyState(PropertyBool.IgnoreCollisions, PhysicsState.IgnoreCollisions, value);
        }

        public bool? NoDraw
        {
            get => GetProperty(PropertyBool.NoDraw);
            set => SetPhysicsPropertyState(PropertyBool.NoDraw, PhysicsState.NoDraw, value);
        }

        public bool? Missile
        {
            get => GetPhysicsState(PhysicsState.Missile);
            set => SetPhysicsState(PhysicsState.Missile, value);
        }

        public bool? Pushable
        {
            get => GetPhysicsState(PhysicsState.Pushable);
            set => SetPhysicsState(PhysicsState.Missile, value);
        }

        public bool? AlignPath
        {
            get => GetPhysicsState(PhysicsState.AlignPath);
            set => SetPhysicsState(PhysicsState.AlignPath, value);
        }

        public bool? PathClipped
        {
            get => GetPhysicsState(PhysicsState.PathClipped);
            set => SetPhysicsState(PhysicsState.PathClipped, value);
        }

        public bool? GravityStatus
        {
            get => GetProperty(PropertyBool.GravityStatus);
            set => SetPhysicsPropertyState(PropertyBool.GravityStatus, PhysicsState.Gravity, value);
        }

        public bool? LightsStatus
        {
            get => GetProperty(PropertyBool.LightsStatus);
            set => SetPhysicsPropertyState(PropertyBool.LightsStatus, PhysicsState.LightingOn, value);
        }

        public bool? ParticleEmitter
        {
            get => GetPhysicsState(PhysicsState.ParticleEmitter);
            set => SetPhysicsState(PhysicsState.ParticleEmitter, value);
        }

        public bool? Hidden
        {
            get => GetPhysicsState(PhysicsState.Hidden);
            set => SetPhysicsState(PhysicsState.Hidden, value);
        }

        public bool? ScriptedCollision
        {
            get => GetProperty(PropertyBool.ScriptedCollision);
            set => SetPhysicsPropertyState(PropertyBool.ScriptedCollision, PhysicsState.ScriptedCollision, value);
        }

        public bool? Inelastic
        {
            get => GetProperty(PropertyBool.Inelastic);
            set => SetPhysicsPropertyState(PropertyBool.Inelastic, PhysicsState.Inelastic, value);
        }

        public bool? Cloaked
        {
            get => GetPhysicsState(PhysicsState.Cloaked);
            set => SetPhysicsState(PhysicsState.Cloaked, value);
        }

        public bool? ReportCollisionsAsEnvironment
        {
            get => GetProperty(PropertyBool.ReportCollisionsAsEnvironment);
            set => SetPhysicsPropertyState(PropertyBool.ReportCollisionsAsEnvironment, PhysicsState.ReportCollisionsAsEnvironment, value);
        }

        public bool? AllowEdgeSlide
        {
            get => GetProperty(PropertyBool.AllowEdgeSlide);
            set => SetPhysicsPropertyState(PropertyBool.AllowEdgeSlide, PhysicsState.EdgeSlide, value);
        }

        public bool? Sledding
        {
            get => GetPhysicsState(PhysicsState.Sledding);
            set => SetPhysicsState(PhysicsState.Sledding, value);
        }

        public bool? IsFrozen
        {
            get => GetProperty(PropertyBool.IsFrozen);
            set => SetPhysicsPropertyState(PropertyBool.IsFrozen, PhysicsState.Frozen, value);
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

        /// <summary>
        /// Persistent boolean value that tracks whether an equipped item is affecting (the item's spells are in effect and its mana is burning) or not.
        /// </summary>
        public bool? IsAffecting
        {
            get => GetProperty(PropertyBool.IsAffecting);
            set
            {
                if (!value.HasValue)
                {
                    if (GetProperty(PropertyBool.IsAffecting).HasValue)
                        RemoveProperty(PropertyBool.IsAffecting);
                }
                else
                {
                    var h = GetProperty(PropertyBool.IsAffecting);
                    if (!h.HasValue || h.HasValue && h.Value != value.Value)
                        SetProperty(PropertyBool.IsAffecting, value.Value);
                }

                if (!(value ?? false))
                {
                    ItemManaDepletionMessageTimestamp = null;
                    ItemManaConsumptionTimestamp = null;
                }
                else
                {
                    ItemManaDepletionMessageTimestamp = null;
                    ItemManaConsumptionTimestamp = DateTime.Now;
                }
            }
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

        public int? StackSize
        {
            get => GetProperty(PropertyInt.StackSize);
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

        /// <summary>
        /// The house owned by this player
        /// </summary>
        public uint? HouseId
        {
            get => GetProperty(PropertyDataId.HouseId);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.HouseId); else SetProperty(PropertyDataId.HouseId, value.Value); }
        }

        public uint? HouseInstance
        {
            get => GetProperty(PropertyInstanceId.House);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.House); else SetProperty(PropertyInstanceId.House, value.Value); }
        }

        public uint? HouseOwner
        {
            get => GetProperty(PropertyInstanceId.HouseOwner);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.HouseOwner); else SetProperty(PropertyInstanceId.HouseOwner, value.Value); }
        }

        /// <summary>
        /// The timestamp the player originally purchased house
        /// </summary>
        public int? HousePurchaseTimestamp
        {
            get => GetProperty(PropertyInt.HousePurchaseTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HousePurchaseTimestamp); else SetProperty(PropertyInt.HousePurchaseTimestamp, value.Value); }
        }

        public int HouseStatus
        {
            get => GetProperty(PropertyInt.HouseStatus) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.HouseStatus); else SetProperty(PropertyInt.HouseStatus, value); }
        }

        public HouseType? HouseType
        {
            get => (HouseType?)GetProperty(PropertyInt.HouseType);
            set { if (value.HasValue) RemoveProperty(PropertyInt.HouseType); else SetProperty(PropertyInt.HouseType, (int)value.Value); }
        }

        public int? HookItemType
        {
            get => GetProperty(PropertyInt.HookItemType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HookItemType); else SetProperty(PropertyInt.HookItemType, value.Value); }
        }

        public int? HookPlacement
        {
            get => GetProperty(PropertyInt.HookPlacement);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HookPlacement); else SetProperty(PropertyInt.HookPlacement, value.Value); }
        }

        public uint? Monarch
        {
            get => GetProperty(PropertyInstanceId.Monarch);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Monarch); else SetProperty(PropertyInstanceId.Monarch, value.Value); }
        }

        public uint? Patron
        {
            get => GetProperty(PropertyInstanceId.Patron);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Patron); else SetProperty(PropertyInstanceId.Patron, value.Value); }
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

        public MaterialType? MaterialType
        {
            get => (MaterialType?)GetProperty(PropertyInt.MaterialType);
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
        public bool IsOpen
        {
            get => GetProperty(PropertyBool.Open) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Open); else SetProperty(PropertyBool.Open, value); }
        }

        public bool IsLocked
        {
            get => GetProperty(PropertyBool.Locked) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Locked); else SetProperty(PropertyBool.Locked, value); }
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

        public bool SafeSpellComponents
        {
            get => GetProperty(PropertyBool.SafeSpellComponents) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.SafeSpellComponents); else SetProperty(PropertyBool.SafeSpellComponents, value); }
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

        public int? Level
        {
            get => GetProperty(PropertyInt.Level);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Level); else SetProperty(PropertyInt.Level, value.Value); }
        }

        public int? UseRequiresLevel
        {
            get => GetProperty(PropertyInt.UseRequiresLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.UseRequiresLevel); else SetProperty(PropertyInt.UseRequiresLevel, value.Value); }
        }

        public int? UseRequiresSkill
        {
            get => GetProperty(PropertyInt.UseRequiresSkill);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.UseRequiresSkill); else SetProperty(PropertyInt.UseRequiresSkill, value.Value); }
        }

        public int? UseRequiresSkillLevel
        {
            get => GetProperty(PropertyInt.UseRequiresSkillLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.UseRequiresSkillLevel); else SetProperty(PropertyInt.UseRequiresSkillLevel, value.Value); }
        }

        public int? UseRequiresSkillSpec
        {
            get => GetProperty(PropertyInt.UseRequiresSkillSpec);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.UseRequiresSkillSpec); else SetProperty(PropertyInt.UseRequiresSkillSpec, value.Value); }
        }

        public double? ArmorModVsSlash
        {
            get => GetProperty(PropertyFloat.ArmorModVsSlash);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsSlash); else SetProperty(PropertyFloat.ArmorModVsSlash, value.Value); }
        }
        public double? ArmorModVsPierce
        {
            get => GetProperty(PropertyFloat.ArmorModVsPierce);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsPierce); else SetProperty(PropertyFloat.ArmorModVsPierce, value.Value); }
        }
        public double? ArmorModVsBludgeon
        {
            get => GetProperty(PropertyFloat.ArmorModVsBludgeon);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsBludgeon); else SetProperty(PropertyFloat.ArmorModVsBludgeon, value.Value); }
        }
        public double? ArmorModVsCold
        {
            get => GetProperty(PropertyFloat.ArmorModVsCold);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsCold); else SetProperty(PropertyFloat.ArmorModVsCold, value.Value); }
        }
        public double? ArmorModVsFire
        {
            get => GetProperty(PropertyFloat.ArmorModVsFire);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsFire); else SetProperty(PropertyFloat.ArmorModVsFire, value.Value); }
        }
        public double? ArmorModVsAcid
        {
            get => GetProperty(PropertyFloat.ArmorModVsAcid);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsAcid); else SetProperty(PropertyFloat.ArmorModVsAcid, value.Value); }
        }
        public double? ArmorModVsElectric
        {
            get => GetProperty(PropertyFloat.ArmorModVsElectric);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsElectric); else SetProperty(PropertyFloat.ArmorModVsElectric, value.Value); }
        }

        public double? ArmorModVsNether
        {
            get => GetProperty(PropertyFloat.ArmorModVsNether);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsNether); else SetProperty(PropertyFloat.ArmorModVsNether, value.Value); }
        }

        public int? ArmorType
        {
            get => GetProperty(PropertyInt.ArmorType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ArmorType); else SetProperty(PropertyInt.ArmorType, value.Value); }
        }

        public int? ArmorLevel
        {
            get => GetProperty(PropertyInt.ArmorLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ArmorLevel); else SetProperty(PropertyInt.ArmorLevel, value.Value); }
        }

        public uint? CombatTableDID
        {
            get => GetProperty(PropertyDataId.CombatTable);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.CombatTable); else SetProperty(PropertyDataId.CombatTable, value.Value); }
        }

        public int? UseCreateContractId
        {
            get => GetProperty(PropertyInt.UseCreatesContractId);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.UseCreatesContractId); else SetProperty(PropertyInt.UseCreatesContractId, value.Value); }
        }

        /// <summary>
        /// Unix time this object was created
        /// </summary>
        public int? CreationTimestamp
        {
            get => GetProperty(PropertyInt.CreationTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CreationTimestamp); else SetProperty(PropertyInt.CreationTimestamp, value.Value); }
        }

        public bool UseBackpackSlot => (GetProperty(PropertyBool.RequiresBackpackSlot) ?? false) || WeenieType == WeenieType.Container;

        public int? PlacementPosition
        {
            get => GetProperty(PropertyInt.PlacementPosition);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PlacementPosition); else SetProperty(PropertyInt.PlacementPosition, value.Value); }
        }

        /* books */
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

        public string Inscription
        {
            get => GetProperty(PropertyString.Inscription);
            set { if (value == null) RemoveProperty(PropertyString.Inscription); else SetProperty(PropertyString.Inscription, value); }
        }
        public bool? IgnoreAuthor
        {
            get => GetProperty(PropertyBool.IgnoreAuthor);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.IgnoreAuthor); else SetProperty(PropertyBool.IgnoreAuthor, value.Value); }
        }


        public int? StackUnitValue
        {
            get => GetProperty(PropertyInt.StackUnitValue);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.StackUnitValue); else SetProperty(PropertyInt.StackUnitValue, value.Value); }
        }

        public int? StackUnitEncumbrance
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

        public double? ManaRate
        {
            get => GetProperty(PropertyFloat.ManaRate);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ManaRate); else SetProperty(PropertyFloat.ManaRate, value.Value); }
        }

        public int? ItemDifficulty
        {
            get => GetProperty(PropertyInt.ItemDifficulty);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemDifficulty); else SetProperty(PropertyInt.ItemDifficulty, value.Value); }
        }

        public int? AppraisalItemSkill
        {
            get => GetProperty(PropertyInt.AppraisalItemSkill);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AppraisalItemSkill); else SetProperty(PropertyInt.AppraisalItemSkill, value.Value); }
        }

        public int? ItemSkillLevelLimit
        {
            get => GetProperty(PropertyInt.ItemSkillLevelLimit);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemSkillLevelLimit); else SetProperty(PropertyInt.ItemSkillLevelLimit, value.Value); }
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

        public CreatureType? FriendType
        {
            get => (CreatureType?)GetProperty(PropertyInt.FriendType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.FriendType); else SetProperty(PropertyInt.FriendType, (int)value.Value); }
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

        public int ItemSpellcraft
        {
            get => GetProperty(PropertyInt.ItemSpellcraft) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.ItemSpellcraft); else SetProperty(PropertyInt.ItemSpellcraft, value); }
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
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.HeartbeatInterval); else SetProperty(PropertyFloat.HeartbeatInterval, value.Value); }
        }

        public double? HeartbeatTimestamp
        {
            get => GetProperty(PropertyFloat.HeartbeatTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.HeartbeatTimestamp); else SetProperty(PropertyFloat.HeartbeatTimestamp, value.Value); }
        }

        public int InitGeneratedObjects
        {
            get => GetProperty(PropertyInt.InitGeneratedObjects) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.InitGeneratedObjects); else SetProperty(PropertyInt.InitGeneratedObjects, value); }
        }

        public int InitCreate
        {
            get => InitGeneratedObjects;
            set => InitGeneratedObjects = value;
        }

        public int MaxGeneratedObjects
        {
            get => GetProperty(PropertyInt.MaxGeneratedObjects) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.MaxGeneratedObjects); else SetProperty(PropertyInt.MaxGeneratedObjects, value); }
        }

        public int MaxCreate
        {
            get => MaxGeneratedObjects;
            set => MaxGeneratedObjects = value;
        }

        public double RegenerationInterval
        {
            get => GetProperty(PropertyFloat.RegenerationInterval) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyFloat.RegenerationInterval); else SetProperty(PropertyFloat.RegenerationInterval, value); }
        }

        public bool GeneratorEnteredWorld
        {
            get => GetProperty(PropertyBool.GeneratorEnteredWorld) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.GeneratorEnteredWorld); else SetProperty(PropertyBool.GeneratorEnteredWorld, value); }
        }

        /// <summary>
        /// If TRUE, this is an admin-only visible object, only seen with /adminvision
        /// </summary>
        public bool Visibility
        {
            get => GetProperty(PropertyBool.Visibility) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Visibility); else SetProperty(PropertyBool.Visibility, value); }
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
        public Position Location
        {
            get => GetPosition(PositionType.Location);
            set => SetPosition(PositionType.Location, value);
        }

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

        public uint? LinkedPortalOneDID
        {
            get => GetProperty(PropertyDataId.LinkedPortalOne);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.LinkedPortalOne); else SetProperty(PropertyDataId.LinkedPortalOne, value.Value); }
        }

        public uint? LinkedPortalTwoDID
        {
            get => GetProperty(PropertyDataId.LinkedPortalTwo);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.LinkedPortalTwo); else SetProperty(PropertyDataId.LinkedPortalTwo, value.Value); }
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

        public int PkLevelModifier
        {
            get => GetProperty(PropertyInt.PkLevelModifier) ?? -1;
            set { if (value == -1) RemoveProperty(PropertyInt.PkLevelModifier); else SetProperty(PropertyInt.PkLevelModifier, value); }
        }

        public PlayerKillerStatus PlayerKillerStatus
        {
            get => (PlayerKillerStatus?)GetProperty(PropertyInt.PlayerKillerStatus) ?? PlayerKillerStatus.NPK;
            set => SetProperty(PropertyInt.PlayerKillerStatus, (int)value);
        }

        public CloakStatus? CloakStatus
        {
            get => (CloakStatus?)GetProperty(PropertyInt.CloakStatus);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CloakStatus); else SetProperty(PropertyInt.CloakStatus, (int)value.Value); }
        }

        public bool IgnorePortalRestrictions
        {
            get => GetProperty(PropertyBool.IgnorePortalRestrictions) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IgnorePortalRestrictions); else SetProperty(PropertyBool.IgnorePortalRestrictions, value); }
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

        /// <summary>
        /// Currently used by Generators and Players
        /// </summary>
        public bool FirstEnterWorldDone
        {
            get => GetProperty(PropertyBool.FirstEnterWorldDone) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.FirstEnterWorldDone); else SetProperty(PropertyBool.FirstEnterWorldDone, value); }
        }

        public uint? OwnerId
        {
            get => GetProperty(PropertyInstanceId.Owner);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Owner); else SetProperty(PropertyInstanceId.Owner, value.Value); }
        }

        public uint ActivationTarget
        {
            get => GetProperty(PropertyInstanceId.ActivationTarget) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInstanceId.ActivationTarget); else SetProperty(PropertyInstanceId.ActivationTarget, value); }
        }

        /// <summary>
        /// The number of seconds before this object can exist on an active landblock before it expires and should be destroyed.
        /// A value of -1 indicates that the item does not rot.<para />
        /// A value of 0, or less than 0 but not -1 indicates that the item has expired and should be destroyed.
        /// </summary>
        public double? TimeToRot
        {
            get => GetProperty(PropertyFloat.TimeToRot);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.TimeToRot); else SetProperty(PropertyFloat.TimeToRot, value.Value); }
        }

        public uint? AllowedActivator
        {
            get => GetProperty(PropertyInstanceId.AllowedActivator);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.AllowedActivator); else SetProperty(PropertyInstanceId.AllowedActivator, value.Value); }
        }

        // generator properties
        public uint? GeneratorId
        {
            get => GetProperty(PropertyInstanceId.Generator);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Generator); else SetProperty(PropertyInstanceId.Generator, value.Value); }
        }

        public bool CurrentlyPoweringUp
        {
            get => GetProperty(PropertyBool.CurrentlyPoweringUp) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.CurrentlyPoweringUp); else SetProperty(PropertyBool.CurrentlyPoweringUp, value); }
        }

        public bool GeneratorDisabled
        {
            get => GetProperty(PropertyBool.GeneratorDisabled) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.GeneratorDisabled); else SetProperty(PropertyBool.GeneratorDisabled, value); }
        }

        public bool GeneratorStatus
        {
            get => GetProperty(PropertyBool.GeneratorStatus) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.GeneratorStatus); else SetProperty(PropertyBool.GeneratorStatus, value); }
        }

        public bool GeneratorAutomaticDestruction
        {
            get => GetProperty(PropertyBool.GeneratorAutomaticDestruction) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.GeneratorAutomaticDestruction); else SetProperty(PropertyBool.GeneratorAutomaticDestruction, value); }
        }

        public string GeneratorEvent
        {
            get => GetProperty(PropertyString.GeneratorEvent);
            set { if (value == null) RemoveProperty(PropertyString.GeneratorEvent); else SetProperty(PropertyString.GeneratorEvent, value); }
        }

        public GeneratorTimeType GeneratorTimeType
        {
            get => (GeneratorTimeType)(GetProperty(PropertyInt.GeneratorTimeType) ?? 0);
            set { if (value == GeneratorTimeType.Undef) RemoveProperty(PropertyInt.GeneratorTimeType); else SetProperty(PropertyInt.GeneratorTimeType, (int)value); }
        }

        public GeneratorDestruct GeneratorDestructionType
        {
            get => (GeneratorDestruct)(GetProperty(PropertyInt.GeneratorDestructionType) ?? 0);
            set { if (value == GeneratorDestruct.Undef) RemoveProperty(PropertyInt.GeneratorDestructionType); else SetProperty(PropertyInt.GeneratorDestructionType, (int)value); }
        }

        public GeneratorDestruct GeneratorEndDestructionType
        {
            get => (GeneratorDestruct)(GetProperty(PropertyInt.GeneratorEndDestructionType) ?? 0);
            set { if (value == GeneratorDestruct.Undef) RemoveProperty(PropertyInt.GeneratorEndDestructionType); else SetProperty(PropertyInt.GeneratorEndDestructionType, (int)value); }
        }

        public GeneratorType GeneratorType
        {
            get => (GeneratorType)(GetProperty(PropertyInt.GeneratorType) ?? 0);
            set { if (value == GeneratorType.Undef) RemoveProperty(PropertyInt.GeneratorType); else SetProperty(PropertyInt.GeneratorType, (int)value); }
        }

        public int GeneratorStartTime
        {
            get => GetProperty(PropertyInt.GeneratorStartTime) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.GeneratorStartTime); else SetProperty(PropertyInt.GeneratorStartTime, value); }
        }

        public int GeneratorEndTime
        {
            get => GetProperty(PropertyInt.GeneratorEndTime) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.GeneratorEndTime); else SetProperty(PropertyInt.GeneratorEndTime, value); }
        }

        public double GeneratorInitialDelay
        {
            get => GetProperty(PropertyFloat.GeneratorInitialDelay) ?? 0d;
            set { if (value == 0d) RemoveProperty(PropertyFloat.GeneratorInitialDelay); else SetProperty(PropertyFloat.GeneratorInitialDelay, value); }
        }

        /* quest properties */

        public string Quest
        {
            get => GetProperty(PropertyString.Quest);
            set { if (value == null) RemoveProperty(PropertyString.Quest); else SetProperty(PropertyString.Quest, value); }
        }

        public string QuestRestriction
        {
            get => GetProperty(PropertyString.QuestRestriction);
            set { if (value == null) RemoveProperty(PropertyString.QuestRestriction); else SetProperty(PropertyString.QuestRestriction, value); }
        }

        /* pressure plates */

        /// <summary>
        /// Returns TRUE if this object can be activated (default)
        /// </summary>
        public bool Active
        {
            get => (GetProperty(PropertyInt.Active) ?? 1) == 1;
            set { if (value) RemoveProperty(PropertyInt.Active); else SetProperty(PropertyInt.Active, 0); }
        }

        /// <summary>
        /// The type of action to perform
        /// </summary>
        public ActivationResponse ActivationResponse
        {
            get => (ActivationResponse)(GetProperty(PropertyInt.ActivationResponse) ?? 2);
            set { if (value == ActivationResponse.Use) RemoveProperty(PropertyInt.ActivationResponse); else SetProperty(PropertyInt.ActivationResponse, (int)value); }
        }

        /// <summary>
        /// The MotionCommand to perform when ActivationResponse = Animate
        /// </summary>
        public MotionCommand ActivationAnimation
        {
            get => (MotionCommand)(GetProperty(PropertyDataId.ActivationAnimation) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyDataId.ActivationAnimation); else SetProperty(PropertyDataId.ActivationAnimation, (uint)value); }
        }

        /// <summary>
        /// The string that is sent to the player when ActivationResponse = Talk
        /// </summary>
        public string ActivationTalk
        {
            get => GetProperty(PropertyString.ActivationTalk);
            set { if (value == null) RemoveProperty(PropertyString.ActivationTalk); else SetProperty(PropertyString.ActivationTalk, value); }
        }

        /// <summary>
        /// The sound played when pressure plate is activated
        /// </summary>
        public uint UseSound
        {
            get => GetProperty(PropertyDataId.UseSound) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyDataId.UseSound); else SetProperty(PropertyDataId.UseSound, value); }
        }

        /* advocate */

        public MotionCommand UseTargetSuccessAnimation
        {
            get => (MotionCommand)(GetProperty(PropertyDataId.UseTargetSuccessAnimation) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyDataId.UseTargetSuccessAnimation); else SetProperty(PropertyDataId.UseTargetSuccessAnimation, (uint)value); }
        }

        public MotionCommand UseTargetFailureAnimation
        {
            get => (MotionCommand)(GetProperty(PropertyDataId.UseTargetFailureAnimation) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyDataId.UseTargetFailureAnimation); else SetProperty(PropertyDataId.UseTargetFailureAnimation, (uint)value); }
        }

        public MotionCommand UseUserAnimation
        {
            get => (MotionCommand)(GetProperty(PropertyDataId.UseUserAnimation) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyDataId.UseUserAnimation); else SetProperty(PropertyDataId.UseUserAnimation, (uint)value); }
        }

        public uint? UseCreateItem
        {
            get => GetProperty(PropertyDataId.UseCreateItem);
            set { if (value == null) RemoveProperty(PropertyDataId.UseCreateItem); else SetProperty(PropertyDataId.UseCreateItem, value.Value); }
        }

        public int? ResistLockpick
        {
            get => GetProperty(PropertyInt.ResistLockpick) ?? 0;
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ResistLockpick); else SetProperty(PropertyInt.ResistLockpick, value.Value); }
        }
    }
}
