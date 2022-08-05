using System;
using System.Collections.Generic;
using System.Numerics;

using ACE.Common;
using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.Structure;
using ACE.Server.Physics.Extensions;

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
        private Dictionary<PropertyBool, bool?> ephemeralPropertyBools { get; set; }
        private Dictionary<PropertyDataId, uint?> ephemeralPropertyDataIds { get; set; }
        private Dictionary<PropertyFloat, double?> ephemeralPropertyFloats { get; set; }
        private Dictionary<PropertyInstanceId, uint?> ephemeralPropertyInstanceIds { get; set; }
        protected Dictionary<PropertyInt, int?> ephemeralPropertyInts { get; set; }
        private Dictionary<PropertyInt64, long?> ephemeralPropertyInt64s { get; set; }
        private Dictionary<PropertyString, string> ephemeralPropertyStrings { get; set; }

        #region GetProperty Functions
        public bool? GetProperty(PropertyBool property)
        {
            if (ephemeralPropertyBools != null && ephemeralPropertyBools.TryGetValue(property, out var value))
                return value;

            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public uint? GetProperty(PropertyDataId property)
        {
            if (ephemeralPropertyDataIds != null && ephemeralPropertyDataIds.TryGetValue(property, out var value))
                return value;

            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public double? GetProperty(PropertyFloat property)
        {
            if (ephemeralPropertyFloats != null && ephemeralPropertyFloats.TryGetValue(property, out var value))
                return value;

            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public uint? GetProperty(PropertyInstanceId property)
        {
            if (ephemeralPropertyInstanceIds != null && ephemeralPropertyInstanceIds.TryGetValue(property, out var value))
                return value;

            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public int? GetProperty(PropertyInt property)
        {
            if (ephemeralPropertyInts != null && ephemeralPropertyInts.TryGetValue(property, out var value))
                return value;

            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public long? GetProperty(PropertyInt64 property)
        {
            if (ephemeralPropertyInt64s != null && ephemeralPropertyInt64s.TryGetValue(property, out var value))
                return value;

            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public string GetProperty(PropertyString property)
        {
            if (ephemeralPropertyStrings != null && ephemeralPropertyStrings.TryGetValue(property, out var value))
                return value;

            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        #endregion

        #region SetProperty Functions
        public void SetProperty(PropertyBool property, bool value)
        {
            if (EphemeralProperties.PropertiesBool.Contains(property))
            {
                if (ephemeralPropertyBools == null)
                    ephemeralPropertyBools = new Dictionary<PropertyBool, bool?>();
                ephemeralPropertyBools[property] = value;
            }
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);

                if (changed)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyDataId property, uint value)
        {
            if (EphemeralProperties.PropertiesDataId.Contains(property))
            {
                if (ephemeralPropertyDataIds == null)
                    ephemeralPropertyDataIds = new Dictionary<PropertyDataId, uint?>();
                ephemeralPropertyDataIds[property] = value;
            }
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);

                if (changed)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyFloat property, double value)
        {
            if (EphemeralProperties.PropertiesDouble.Contains(property))
            {
                if (ephemeralPropertyFloats == null)
                    ephemeralPropertyFloats = new Dictionary<PropertyFloat, double?>();
                ephemeralPropertyFloats[property] = value;
            }
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);

                if (changed)
                    ChangesDetected = true;
            }
        }
        public void IncProperty(PropertyFloat property, double value)
        {
            var prop = GetProperty(property) ?? 0;
            SetProperty(property, prop + value);
        }
        public void SetProperty(PropertyInstanceId property, uint value)
        {
            if (EphemeralProperties.PropertiesInstanceId.Contains(property))
            {
                if (ephemeralPropertyInstanceIds == null)
                    ephemeralPropertyInstanceIds = new Dictionary<PropertyInstanceId, uint?>();
                ephemeralPropertyInstanceIds[property] = value;
            }
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);

                if (changed)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyInt property, int value)
        {
            // PropertyInt.EncumbranceVal and PropertyInt.Value are sometimes treated as ephemeral, so this extra check is needed
            if (EphemeralProperties.PropertiesInt.Contains(property) || (ephemeralPropertyInts != null && ephemeralPropertyInts.ContainsKey(property)))
            {
                if (ephemeralPropertyInts == null)
                    ephemeralPropertyInts = new Dictionary<PropertyInt, int?>();
                ephemeralPropertyInts[property] = value;
            }
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);

                if (changed)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyInt64 property, long value)
        {
            if (EphemeralProperties.PropertiesInt64.Contains(property))
            {
                if (ephemeralPropertyInt64s == null)
                    ephemeralPropertyInt64s = new Dictionary<PropertyInt64, long?>();
                ephemeralPropertyInt64s[property] = value;
            }
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);

                if (changed)
                    ChangesDetected = true;
            }
        }
        public void SetProperty(PropertyString property, string value)
        {
            if (EphemeralProperties.PropertiesString.Contains(property))
            {
                if (ephemeralPropertyStrings == null)
                    ephemeralPropertyStrings = new Dictionary<PropertyString, string>();
                ephemeralPropertyStrings[property] = value;
            }
            else
            {
                Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);

                if (changed)
                    ChangesDetected = true;
            }
        }
        #endregion

        #region RemoveProperty Functions
        public void RemoveProperty(PropertyBool property)
        {
            if (EphemeralProperties.PropertiesBool.Contains(property))
            {
                if (ephemeralPropertyBools != null)
                    ephemeralPropertyBools[property] = null;
            }
            else
            {
                if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }
        public void RemoveProperty(PropertyDataId property)
        {
            if (EphemeralProperties.PropertiesDataId.Contains(property))
            {
                if (ephemeralPropertyDataIds != null)
                    ephemeralPropertyDataIds[property] = null;
            }
            else
            {
                if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }
        public void RemoveProperty(PropertyFloat property)
        {
            if (EphemeralProperties.PropertiesDouble.Contains(property))
            {
                if (ephemeralPropertyFloats != null)
                    ephemeralPropertyFloats[property] = null;
            }
            else
            {
                if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }
        public void RemoveProperty(PropertyInstanceId property)
        {
            if (EphemeralProperties.PropertiesInstanceId.Contains(property))
            {
                if (ephemeralPropertyInstanceIds != null)
                    ephemeralPropertyInstanceIds[property] = null;
            }
            else
            {
                if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }
        public void RemoveProperty(PropertyInt property)
        {
            if (EphemeralProperties.PropertiesInt.Contains(property))
            {
                if (ephemeralPropertyInts != null)
                    ephemeralPropertyInts[property] = null;
            }
            else
            {
                if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }
        public void RemoveProperty(PropertyInt64 property)
        {
            if (EphemeralProperties.PropertiesInt64.Contains(property))
            {
                if (ephemeralPropertyInt64s != null)
                    ephemeralPropertyInt64s[property] = null;
            }
            else
            {
                if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }
        public void RemoveProperty(PropertyString property)
        {
            if (EphemeralProperties.PropertiesString.Contains(property))
            {
                if (ephemeralPropertyStrings != null)
                    ephemeralPropertyStrings[property] = null;
            }
            else
            {
                if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }
        #endregion

        #region GetAllProperty Functions
        public Dictionary<PropertyBool, bool> GetAllPropertyBools()
        {
            var results = new Dictionary<PropertyBool, bool>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (Biota.PropertiesBool != null)
                {
                    foreach (var kvp in Biota.PropertiesBool)
                        results[kvp.Key] = kvp.Value;
                }
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            if (ephemeralPropertyBools != null)
            {
                foreach (var property in ephemeralPropertyBools)
                {
                    if (property.Value.HasValue)
                        results[property.Key] = property.Value.Value;
                    else
                        results.Remove(property.Key);
                }
            }

            return results;
        }

        public Dictionary<PropertyDataId, uint> GetAllPropertyDataId()
        {
            var results = new Dictionary<PropertyDataId, uint>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (Biota.PropertiesDID != null)
                {
                    foreach (var kvp in Biota.PropertiesDID)
                        results[kvp.Key] = kvp.Value;
                }
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            if (ephemeralPropertyDataIds != null)
            {
                foreach (var property in ephemeralPropertyDataIds)
                {
                    if (property.Value.HasValue)
                        results[property.Key] = property.Value.Value;
                    else
                        results.Remove(property.Key);
                }
            }

            return results;
        }

        public Dictionary<PropertyFloat, double> GetAllPropertyFloat()
        {
            var results = new Dictionary<PropertyFloat, double>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (Biota.PropertiesFloat != null)
                {
                    foreach (var kvp in Biota.PropertiesFloat)
                        results[kvp.Key] = kvp.Value;
                }
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            if (ephemeralPropertyFloats != null)
            {
                foreach (var property in ephemeralPropertyFloats)
                {
                    if (property.Value.HasValue)
                        results[property.Key] = property.Value.Value;
                    else
                        results.Remove(property.Key);
                }
            }

            return results;
        }

        public Dictionary<PropertyInstanceId, uint> GetAllPropertyInstanceId()
        {
            var results = new Dictionary<PropertyInstanceId, uint>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (Biota.PropertiesIID != null)
                {
                    foreach (var kvp in Biota.PropertiesIID)
                        results[kvp.Key] = kvp.Value;
                }
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            if (ephemeralPropertyInstanceIds != null)
            {
                foreach (var property in ephemeralPropertyInstanceIds)
                {
                    if (property.Value.HasValue)
                        results[property.Key] = property.Value.Value;
                    else
                        results.Remove(property.Key);
                }
            }

            return results;
        }

        public Dictionary<PropertyInt, int> GetAllPropertyInt()
        {
            var results = new Dictionary<PropertyInt, int>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (Biota.PropertiesInt != null)
                {
                    foreach (var kvp in Biota.PropertiesInt)
                        results[kvp.Key] = kvp.Value;
                }
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            if (ephemeralPropertyInts != null)
            {
                foreach (var property in ephemeralPropertyInts)
                {
                    if (property.Value.HasValue)
                        results[property.Key] = property.Value.Value;
                    else
                        results.Remove(property.Key);
                }
            }

            return results;
        }

        public Dictionary<PropertyInt64, long> GetAllPropertyInt64()
        {
            var results = new Dictionary<PropertyInt64, long>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (Biota.PropertiesInt64 != null)
                {
                    foreach (var kvp in Biota.PropertiesInt64)
                        results[kvp.Key] = kvp.Value;
                }
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            if (ephemeralPropertyInt64s != null)
            {
                foreach (var property in ephemeralPropertyInt64s)
                {
                    if (property.Value.HasValue)
                        results[property.Key] = property.Value.Value;
                    else
                        results.Remove(property.Key);
                }
            }

            return results;
        }

        public Dictionary<PropertyString, string> GetAllPropertyString()
        {
            var results = new Dictionary<PropertyString, string>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (Biota.PropertiesString != null)
                {
                    foreach (var kvp in Biota.PropertiesString)
                        results[kvp.Key] = kvp.Value;
                }
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            if (ephemeralPropertyStrings != null)
            {
                foreach (var property in ephemeralPropertyStrings)
                {
                    if (property.Value != null)
                        results[property.Key] = property.Value;
                    else
                        results.Remove(property.Key);
                }
            }

            return results;
        }
        #endregion


        private readonly Dictionary<PositionType, Position> ephemeralPositions = new Dictionary<PositionType, Position>();

        /// <summary>
        /// Do not reference this directly.<para />
        /// This should only be referenced by GetPosition, SetPosition, RemovePosition and SaveBiotaToDatabase.
        /// </summary>
        private readonly Dictionary<PositionType, Position> positionCache = new Dictionary<PositionType, Position>();

        #region GetPosition, SetPosition, RemovePosition, GetAllPositions Functions
        public Position GetPosition(PositionType positionType)
        {
            if (ephemeralPositions.TryGetValue(positionType, out var ephemeralPosition))
                return ephemeralPosition;

            if (positionCache.TryGetValue(positionType, out var cachedPosition))
                return cachedPosition;

            var position = Biota.GetPosition(positionType, BiotaDatabaseLock);

            if (position != null && !position.Rotation.IsRotationValid())
            {
                position.AttemptToFixRotation(this, positionType);
            }

            positionCache[positionType] = position;

            return position;
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
            //if (position != null && !position.Rotation.IsRotationValid())
                //position.AttemptToFixRotation(this, positionType);

            if (EphemeralProperties.PositionTypes.Contains(positionType))
                ephemeralPositions[positionType] = position;
            else
            {
                if (position == null)
                    RemovePosition(positionType);
                else
                {
                    positionCache[positionType] = position;

                    Biota.SetPosition(positionType, position, BiotaDatabaseLock);
                    ChangesDetected = true;
                }
            }
        }

        public void RemovePosition(PositionType positionType)
        {
            if (EphemeralProperties.PositionTypes.Contains(positionType))
                ephemeralPositions[positionType] = null;
            else
            {
                positionCache.Remove(positionType);

                if (Biota.TryRemoveProperty(positionType, BiotaDatabaseLock))
                    ChangesDetected = true;
            }
        }

        public Dictionary<PositionType, Position> GetAllPositions()
        {
            var results = new Dictionary<PositionType, Position>();

            BiotaDatabaseLock.EnterReadLock();
            try
            {
                foreach (var kvp in Biota.PropertiesPosition)
                    results[kvp.Key] = new Position(kvp.Value.ObjCellId, kvp.Value.PositionX, kvp.Value.PositionY, kvp.Value.PositionZ, kvp.Value.RotationX, kvp.Value.RotationY, kvp.Value.RotationZ, kvp.Value.RotationW);
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }

            foreach (var property in ephemeralPositions)
            {
                if (property.Value != null)
                    results[property.Key] = property.Value;
                else
                    results.Remove(property.Key);
            }

            return results;
        }
        #endregion


        // ========================================
        // ======== Physics Desc Properties =======
        // ========================================
        // used in CalculatedPhysicsDescriptionFlag()
        public Motion CurrentMotionState { get; set; }

        public MoveToState CurrentMoveToState { get; set; } = new MoveToState();
        public MovementData CurrentMovementData { get; set; } = new MovementData();


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

        public Vector3 Velocity => PhysicsObj?.Velocity ?? Vector3.Zero;

        public Vector3 Acceleration => PhysicsObj?.Acceleration ?? Vector3.Zero;

        public Vector3 Omega => PhysicsObj?.Omega ?? Vector3.Zero;

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

        public string NameWithMaterial => GetNameWithMaterial();

        public string GetNameWithMaterial(int? stackSize = null)
        {
            var name = stackSize != null && stackSize != 1 ? GetPluralName() : Name;

            if (MaterialType == null)
                return name;

            var material = RecipeManager.GetMaterialName(MaterialType ?? 0);

            if (name.Contains(material))
                name = name.Replace(material, "");

            return $"{material} {name}";
        }

        public string DisplayName
        {
            get => GetProperty(PropertyString.DisplayName);
            set { if (value == null) RemoveProperty(PropertyString.DisplayName); else SetProperty(PropertyString.DisplayName, value); }
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
        public string PluralName
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

        public int? Value
        {
            get => GetProperty(PropertyInt.Value);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Value); else SetProperty(PropertyInt.Value, value.Value); }
        }

        /// <summary>
        /// Flag indicates if an equipped item w/ built-in spells is currently activated, and mana is burning on item
        /// </summary>
        public bool IsAffecting
        {
            get => GetProperty(PropertyBool.IsAffecting) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsAffecting); else SetProperty(PropertyBool.IsAffecting, value); }
        }

        public Usable? ItemUseable
        {
            get => (Usable?)GetProperty(PropertyInt.ItemUseable);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemUseable); else SetProperty(PropertyInt.ItemUseable, (int)value.Value); }
        }

        public float? UseRadius
        {
            get => (float?)GetProperty(PropertyFloat.UseRadius);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.UseRadius); else SetProperty(PropertyFloat.UseRadius, value.Value); }
        }

        public ItemType? TargetType
        {
            get => (ItemType?)GetProperty(PropertyInt.TargetType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.TargetType); else SetProperty(PropertyInt.TargetType, (int)value.Value); }
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

        public int? Damage
        {
            get => GetProperty(PropertyInt.Damage);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Damage); else SetProperty(PropertyInt.Damage, value.Value); }
        }

        public double? DamageMod
        {
            get => GetProperty(PropertyFloat.DamageMod);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.DamageMod); else SetProperty(PropertyFloat.DamageMod, value.Value); }
        }

        public double? DamageVariance
        {
            get => GetProperty(PropertyFloat.DamageVariance);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.DamageVariance); else SetProperty(PropertyFloat.DamageVariance, value.Value); }
        }

        public int? WeaponTime
        {
            get => GetProperty(PropertyInt.WeaponTime);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WeaponTime); else SetProperty(PropertyInt.WeaponTime, value.Value); }
        }

        public double? WeaponDefense
        {
            get => GetProperty(PropertyFloat.WeaponDefense);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.WeaponDefense); else SetProperty(PropertyFloat.WeaponDefense, value.Value); }
        }

        public double? WeaponMissileDefense
        {
            get => GetProperty(PropertyFloat.WeaponMissileDefense);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.WeaponMissileDefense); else SetProperty(PropertyFloat.WeaponMissileDefense, value.Value); }
        }

        public double? WeaponMagicDefense
        {
            get => GetProperty(PropertyFloat.WeaponMagicDefense);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.WeaponMagicDefense); else SetProperty(PropertyFloat.WeaponMagicDefense, value.Value); }
        }

        public double? WeaponOffense
        {
            get => GetProperty(PropertyFloat.WeaponOffense);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.WeaponOffense); else SetProperty(PropertyFloat.WeaponOffense, value.Value); }
        }

        public double? ManaConversionMod
        {
            get => GetProperty(PropertyFloat.ManaConversionMod);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ManaConversionMod); else SetProperty(PropertyFloat.ManaConversionMod, value.Value); }
        }

        // for missile launchers, additive
        public int? ElementalDamageBonus
        {
            get => GetProperty(PropertyInt.ElementalDamageBonus);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ElementalDamageBonus); else SetProperty(PropertyInt.ElementalDamageBonus, value.Value); }
        }

        // for casters, multiplicative
        public double? ElementalDamageMod
        {
            get => GetProperty(PropertyFloat.ElementalDamageMod);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ElementalDamageMod); else SetProperty(PropertyFloat.ElementalDamageMod, value.Value); }
        }

        public WieldRequirement WieldRequirements
        {
            get => (WieldRequirement)(GetProperty(PropertyInt.WieldRequirements) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.WieldRequirements); else SetProperty(PropertyInt.WieldRequirements, (int)value); }
        }

        /// <summary>
        /// can also be used for attributes
        /// </summary>
        public int? WieldSkillType
        {
            get => GetProperty(PropertyInt.WieldSkillType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WieldSkillType); else SetProperty(PropertyInt.WieldSkillType, value.Value); }
        }

        public int? WieldDifficulty
        {
            get => GetProperty(PropertyInt.WieldDifficulty);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WieldDifficulty); else SetProperty(PropertyInt.WieldDifficulty, (int)value); }
        }

        public WieldRequirement WieldRequirements2
        {
            get => (WieldRequirement)(GetProperty(PropertyInt.WieldRequirements2) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.WieldRequirements2); else SetProperty(PropertyInt.WieldRequirements2, (int)value); }
        }

        /// <summary>
        /// can also be used for attributes
        /// </summary>
        public int? WieldSkillType2
        {
            get => GetProperty(PropertyInt.WieldSkillType2);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WieldSkillType2); else SetProperty(PropertyInt.WieldSkillType2, value.Value); }
        }

        public int? WieldDifficulty2
        {
            get => GetProperty(PropertyInt.WieldDifficulty2);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WieldDifficulty2); else SetProperty(PropertyInt.WieldDifficulty2, (int)value); }
        }

        public WieldRequirement WieldRequirements3
        {
            get => (WieldRequirement)(GetProperty(PropertyInt.WieldRequirements3) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.WieldRequirements3); else SetProperty(PropertyInt.WieldRequirements3, (int)value); }
        }

        /// <summary>
        /// can also be used for attributes
        /// </summary>
        public int? WieldSkillType3
        {
            get => GetProperty(PropertyInt.WieldSkillType3);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WieldSkillType3); else SetProperty(PropertyInt.WieldSkillType3, value.Value); }
        }

        public int? WieldDifficulty3
        {
            get => GetProperty(PropertyInt.WieldDifficulty3);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WieldDifficulty3); else SetProperty(PropertyInt.WieldDifficulty3, (int)value); }
        }

        public WieldRequirement WieldRequirements4
        {
            get => (WieldRequirement)(GetProperty(PropertyInt.WieldRequirements4) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.WieldRequirements4); else SetProperty(PropertyInt.WieldRequirements4, (int)value); }
        }

        /// <summary>
        /// can also be used for attributes
        /// </summary>
        public int? WieldSkillType4
        {
            get => GetProperty(PropertyInt.WieldSkillType4);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WieldSkillType4); else SetProperty(PropertyInt.WieldSkillType4, value.Value); }
        }

        public int? WieldDifficulty4
        {
            get => GetProperty(PropertyInt.WieldDifficulty4);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.WieldDifficulty4); else SetProperty(PropertyInt.WieldDifficulty4, (int)value); }
        }

        public int? ItemAllegianceRankLimit
        {
            get => GetProperty(PropertyInt.ItemAllegianceRankLimit);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemAllegianceRankLimit); else SetProperty(PropertyInt.ItemAllegianceRankLimit, value.Value); }
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

        /// <summary>
        /// Instead of setting this directly, consider using SetStackSize() instead which also sets EncumbranceVal and Value
        /// </summary>
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

        /// <summary>
        /// If this property is not defined, defaults to true
        /// </summary>
        public bool IsSellable
        {
            get => GetProperty(PropertyBool.IsSellable) ?? true;
            set { if (value) RemoveProperty(PropertyBool.IsSellable); else SetProperty(PropertyBool.IsSellable, value); }
        }

        public WorldObject Container;

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

        public CoverageMask? ClothingPriority
        {
            get => (CoverageMask?)GetProperty(PropertyInt.ClothingPriority);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ClothingPriority); else SetProperty(PropertyInt.ClothingPriority, (int)value.Value); }
        }

        /// <summary>
        /// Returns the VisualClothingPriority, if set, else returns ClothingPriority
        /// </summary>
        public CoverageMask? VisualClothingPriority
        {
            get { if ((CoverageMask?)GetProperty(PropertyInt.VisualClothingPriority) != null) { return (CoverageMask?)GetProperty(PropertyInt.VisualClothingPriority); } else { return (CoverageMask?)GetProperty(PropertyInt.ClothingPriority); }; }
            set { if (!value.HasValue) RemoveProperty(PropertyInt.VisualClothingPriority); else SetProperty(PropertyInt.VisualClothingPriority, (int)value.Value); }
        }
        /// <summary>
        /// Function to genreate and set the VisualClothingPriority of the armor piece
        /// </summary>
        public void setVisualClothingPriority()
        {
            if (ClothingBase.HasValue && (CurrentWieldedLocation & (EquipMask.Armor | EquipMask.Extremity)) != 0)
            {
                ClothingTable item = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)ClothingBase);
                VisualClothingPriority = item.GetVisualPriority();
            }
        }

        public bool? TopLayerPriority
        {
            get => (bool?)GetProperty(PropertyBool.TopLayerPriority);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.TopLayerPriority); else SetProperty(PropertyBool.TopLayerPriority, (bool)value.Value); }
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

        public int? ItemWorkmanship
        {
            get => GetProperty(PropertyInt.ItemWorkmanship);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemWorkmanship); else SetProperty(PropertyInt.ItemWorkmanship, value.Value); }
        }

        public int? NumItemsInMaterial
        {
            get => GetProperty(PropertyInt.NumItemsInMaterial);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.NumItemsInMaterial); else SetProperty(PropertyInt.NumItemsInMaterial, value.Value); }
        }

        public AppraisalLongDescDecorations? AppraisalLongDescDecoration
        {
            get => (AppraisalLongDescDecorations?)GetProperty(PropertyInt.AppraisalLongDescDecoration);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AppraisalLongDescDecoration); else SetProperty(PropertyInt.AppraisalLongDescDecoration, (int)value); }
        }

        public float? Workmanship
        {
            get
            {
                if (ItemWorkmanship == null) return null;

                var numItemsInMaterial = GetProperty(PropertyInt.NumItemsInMaterial) ?? 1;

                var workmanship = (float)ItemWorkmanship / numItemsInMaterial;

                // try to recover from previous botched formula...

                // TODO: remove this code after awhile
                if (workmanship < 1.0f || workmanship > 10.0f)
                {
                    var prevWorkmanship = workmanship;

                    var structure = Structure ?? 1;

                    workmanship = (float)ItemWorkmanship.Value / 10000 / structure;

                    ItemWorkmanship = (int)Math.Round(workmanship * numItemsInMaterial);

                    workmanship = Math.Clamp(workmanship, 1.0f, 10.0f);

                    //log.Warn($"{Name}.Workmanship: adjusted from {prevWorkmanship} to {workmanship}");
                }

                return workmanship;
            }
            set
            {
                if (value != null)
                {
                    var numItemsInMaterial = GetProperty(PropertyInt.NumItemsInMaterial) ?? 1;

                    ItemWorkmanship = (int)Math.Round(value.Value * numItemsInMaterial);
                }
                else
                    ItemWorkmanship = null;
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

        public string HouseOwnerName
        {
            get => GetProperty(PropertyString.HouseOwnerName);
            set { if (value == null) RemoveProperty(PropertyString.HouseOwnerName); else SetProperty(PropertyString.HouseOwnerName, value); }
        }

        public HouseStatus HouseStatus
        {
            get => (HouseStatus?)GetProperty(PropertyInt.HouseStatus) ?? HouseStatus.Active;
            set { if (value == HouseStatus.Active) RemoveProperty(PropertyInt.HouseStatus); else SetProperty(PropertyInt.HouseStatus, (int)value); }
        }

        public HouseType HouseType
        {
            get => (HouseType?)GetProperty(PropertyInt.HouseType) ?? HouseType.Undef;
            set { if (value == HouseType.Undef) RemoveProperty(PropertyInt.HouseType); else SetProperty(PropertyInt.HouseType, (int)value); }
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

        public uint? MonarchId
        {
            get => GetProperty(PropertyInstanceId.Monarch);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Monarch); else SetProperty(PropertyInstanceId.Monarch, value.Value); }
        }

        public uint? PatronId
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

        public uint? IconOverlaySecondary
        {
            get => GetProperty(PropertyDataId.IconOverlaySecondary);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.IconOverlaySecondary); else SetProperty(PropertyDataId.IconOverlaySecondary, value.Value); }
        }

        public MaterialType? MaterialType
        {
            get => (MaterialType?)GetProperty(PropertyInt.MaterialType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MaterialType); else SetProperty(PropertyInt.MaterialType, (int)value.Value); }
        }

        public MaterialType? GemType
        {
            get => (MaterialType?)GetProperty(PropertyInt.GemType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GemType); else SetProperty(PropertyInt.GemType, (int)value.Value); }
        }

        public int? GemCount
        {
            get => GetProperty(PropertyInt.GemCount);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GemCount); else SetProperty(PropertyInt.GemCount, value.Value); }
        }

        public AttunedStatus? Attuned
        {
            get => (AttunedStatus?)GetProperty(PropertyInt.Attuned);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Attuned); else SetProperty(PropertyInt.Attuned, (int)value.Value); }
        }

        public BondedStatus? Bonded
        {
            get => (BondedStatus?)GetProperty(PropertyInt.Bonded);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Bonded); else SetProperty(PropertyInt.Bonded, (int)value.Value); }
        }

        public bool IsOpen
        {
            get => GetProperty(PropertyBool.Open) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Open); else SetProperty(PropertyBool.Open, value); }
        }


        // ========================================
        // ====== Weenie Header 2 Properties ======
        // ========================================
        // used in CalculateWeenieHeaderFlag2()
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

        public bool IsLocked
        {
            get => GetProperty(PropertyBool.Locked) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Locked); else SetProperty(PropertyBool.Locked, value); }
        }

        public bool Inscribable
        {
            get => GetProperty(PropertyBool.Inscribable) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Inscribable); else SetProperty(PropertyBool.Inscribable, value); }
        }

        public bool Stuck
        {
            get => GetProperty(PropertyBool.Stuck) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Stuck); else SetProperty(PropertyBool.Stuck, value); }
        }

        /// <summary>
        /// If this property is not defined, defaults to true
        /// </summary>
        public bool Attackable
        {
            get => GetProperty(PropertyBool.Attackable) ?? true;
            set { if (value) RemoveProperty(PropertyBool.Attackable); else SetProperty(PropertyBool.Attackable, value); }
        }

        public bool HiddenAdmin
        {
            get => GetProperty(PropertyBool.HiddenAdmin) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.HiddenAdmin); else SetProperty(PropertyBool.HiddenAdmin, value); }
        }

        public bool UiHidden
        {
            get => GetProperty(PropertyBool.UiHidden) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.UiHidden); else SetProperty(PropertyBool.UiHidden, value); }
        }

        public bool IgnoreHouseBarriers
        {
            get => GetProperty(PropertyBool.IgnoreHouseBarriers) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IgnoreHouseBarriers); else SetProperty(PropertyBool.IgnoreHouseBarriers, value); }
        }

        public bool RequiresPackSlot
        {
            get => GetProperty(PropertyBool.RequiresBackpackSlot) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.RequiresBackpackSlot); else SetProperty(PropertyBool.RequiresBackpackSlot, value); }
        }

        public bool Retained
        {
            get => GetProperty(PropertyBool.Retained) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Retained); else SetProperty(PropertyBool.Retained, value); }
        }

        public bool WieldOnUse
        {
            get => GetProperty(PropertyBool.WieldOnUse) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.WieldOnUse); else SetProperty(PropertyBool.WieldOnUse, value); }
        }

        public bool WieldLeft
        {
            get => GetProperty(PropertyBool.AutowieldLeft) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AutowieldLeft); else SetProperty(PropertyBool.AutowieldLeft, value); }
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

        public HeritageGroup HeritageGroup
        {
            get => (HeritageGroup)(GetProperty(PropertyInt.HeritageGroup) ?? 0);
            set { if (value == HeritageGroup.Invalid) RemoveProperty(PropertyInt.HeritageGroup); else SetProperty(PropertyInt.HeritageGroup, (int)value); }
        }

        public string HeritageGroupName
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

        public int? HairStyle
        {
            get => GetProperty(PropertyInt.Hairstyle);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Hairstyle); else SetProperty(PropertyInt.Hairstyle, value.Value); }
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

        public double? AbsorbMagicDamage
        {
            get => GetProperty(PropertyFloat.AbsorbMagicDamage);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.AbsorbMagicDamage); else SetProperty(PropertyFloat.AbsorbMagicDamage, value.Value); }
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

        public double? ReleasedTimestamp
        {
            get => GetProperty(PropertyFloat.ReleasedTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ReleasedTimestamp); else SetProperty(PropertyFloat.ReleasedTimestamp, value.Value); }
        }

        public double? CheckpointTimestamp
        {
            get => GetProperty(PropertyFloat.CheckpointTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.CheckpointTimestamp); else SetProperty(PropertyFloat.CheckpointTimestamp, value.Value); }
        }

        public bool UseBackpackSlot => WeenieType == WeenieType.Container || RequiresPackSlot;

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

        public int? EncumbranceVal
        {
            get => GetProperty(PropertyInt.EncumbranceVal);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.EncumbranceVal); else SetProperty(PropertyInt.EncumbranceVal, value.Value); }
        }

        public double? BulkMod
        {
            get => GetProperty(PropertyFloat.BulkMod);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.BulkMod); else SetProperty(PropertyFloat.BulkMod, value.Value); }
        }

        public double? SizeMod
        {
            get => GetProperty(PropertyFloat.SizeMod);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.SizeMod); else SetProperty(PropertyFloat.SizeMod, value.Value); }
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

        public int? ItemManaCost
        {
            get => GetProperty(PropertyInt.ItemManaCost);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemManaCost); else SetProperty(PropertyInt.ItemManaCost, value.Value); }
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

        public Skill? ItemSkillLimit
        {
            get => (Skill?)GetProperty(PropertyDataId.ItemSkillLimit);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.ItemSkillLimit); else SetProperty(PropertyDataId.ItemSkillLimit, (uint)value); }
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

        public CreatureType? FoeType
        {
            get => (CreatureType?)GetProperty(PropertyInt.FoeType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.FoeType); else SetProperty(PropertyInt.FoeType, (int)value.Value); }
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

        public int BoostValue
        {
            get => GetProperty(PropertyInt.BoostValue) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.BoostValue); else SetProperty(PropertyInt.BoostValue, value); }
        }

        public PropertyAttribute2nd BoosterEnum
        {
            get => (PropertyAttribute2nd)(GetProperty(PropertyInt.BoosterEnum) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.BoosterEnum); else SetProperty(PropertyInt.BoosterEnum, (int)value); }
        }

        public bool UnlimitedUse
        {
            get => GetProperty(PropertyBool.UnlimitedUse) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.UnlimitedUse); else SetProperty(PropertyBool.UnlimitedUse, value); }
        }

        public uint? SpellDID
        {
            get => GetProperty(PropertyDataId.Spell);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.Spell); else SetProperty(PropertyDataId.Spell, value.Value); }
        }

        public int? ItemSpellcraft
        {
            get => GetProperty(PropertyInt.ItemSpellcraft);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemSpellcraft); else SetProperty(PropertyInt.ItemSpellcraft, value.Value); }
        }

        public double? HealkitMod
        {
            get => GetProperty(PropertyFloat.HealkitMod);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.HealkitMod); else SetProperty(PropertyFloat.HealkitMod, value.Value); }
        }

        public int? CoinValue
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

        public double RegenerationTimestamp
        {
            get => GetProperty(PropertyFloat.RegenerationTimestamp) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyFloat.RegenerationTimestamp); else SetProperty(PropertyFloat.RegenerationTimestamp, value); }
        }

        public double GeneratorUpdateTimestamp
        {
            get => GetProperty(PropertyFloat.GeneratorUpdateTimestamp) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyFloat.GeneratorUpdateTimestamp); else SetProperty(PropertyFloat.GeneratorUpdateTimestamp, value); }
        }

        public bool GeneratorEnteredWorld
        {
            get => GetProperty(PropertyBool.GeneratorEnteredWorld) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.GeneratorEnteredWorld); else SetProperty(PropertyBool.GeneratorEnteredWorld, value); }
        }

        public bool GeneratedTreasureItem
        {
            get => GetProperty(PropertyBool.GeneratedTreasureItem) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.GeneratedTreasureItem); else SetProperty(PropertyBool.GeneratedTreasureItem, value); }
        }

        public int? TsysMutationData
        {
            get => GetProperty(PropertyInt.TsysMutationData);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.TsysMutationData); else SetProperty(PropertyInt.TsysMutationData, value.Value); }
        }

        // helpers
        public byte? MaterialCode => (byte?)TsysMutationData;

        public byte? GemCode => (byte?)(TsysMutationData >> 8);

        public byte? ColorCode => (byte?)(TsysMutationData >> 16);

        public byte? SpellSelectionCode => (byte?)(TsysMutationData >> 24);


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

        public double? Shade2
        {
            get => GetProperty(PropertyFloat.Shade2);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.Shade2); else SetProperty(PropertyFloat.Shade2, value.Value); }
        }

        public double? Shade3
        {
            get => GetProperty(PropertyFloat.Shade3);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.Shade3); else SetProperty(PropertyFloat.Shade3, value.Value); }
        }

        public double? Shade4
        {
            get => GetProperty(PropertyFloat.Shade4);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.Shade4); else SetProperty(PropertyFloat.Shade4, value.Value); }
        }

        public int NumTimesTinkered
        {
            get => GetProperty(PropertyInt.NumTimesTinkered) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.NumTimesTinkered); else SetProperty(PropertyInt.NumTimesTinkered, value); }
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

        public uint? LastPortalDID
        {
            get => GetProperty(PropertyDataId.LastPortal);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.LastPortal); else SetProperty(PropertyDataId.LastPortal, value.Value); }
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

        public PKLevel PkLevel
        {
            get => (PKLevel)PkLevelModifier;
            set => PkLevelModifier = (int)value;
        }

        public int PkLevelModifier
        {
            get => GetProperty(PropertyInt.PkLevelModifier) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.PkLevelModifier); else SetProperty(PropertyInt.PkLevelModifier, value); }
        }

        public PlayerKillerStatus PlayerKillerStatus
        {
            get => (PlayerKillerStatus?)GetProperty(PropertyInt.PlayerKillerStatus) ?? PlayerKillerStatus.NPK;
            set { SetProperty(PropertyInt.PlayerKillerStatus, (int)value); }
        }

        public CloakStatus CloakStatus
        {
            get => (CloakStatus)(GetProperty(PropertyInt.CloakStatus) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.CloakStatus); else SetProperty(PropertyInt.CloakStatus, (int)value); }
        }

        public bool IgnorePortalRestrictions
        {
            get => GetProperty(PropertyBool.IgnorePortalRestrictions) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IgnorePortalRestrictions); else SetProperty(PropertyBool.IgnorePortalRestrictions, value); }
        }

        public bool Invincible
        {
            get => GetProperty(PropertyBool.Invincible) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Invincible); else SetProperty(PropertyBool.Invincible, value); }
        }

        public int? XpOverride
        {
            get => GetProperty(PropertyInt.XpOverride);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.XpOverride); else SetProperty(PropertyInt.XpOverride, value.Value); }
        }

        public int? MinLevel
        {
            get => GetProperty(PropertyInt.MinLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MinLevel); else SetProperty(PropertyInt.MinLevel, value.Value); }
        }

        public int? MaxLevel
        {
            get => GetProperty(PropertyInt.MaxLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MaxLevel); else SetProperty(PropertyInt.MaxLevel, value.Value); }
        }

        /// <summary>
        /// <para>Used to mark when EnterWorld has completed for first time for this object's instance.</para>
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
            get => (GetProperty(PropertyInt.Active) ?? 1) != 0;
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

        public Sound UseSound
        {
            get => (Sound)(GetProperty(PropertyDataId.UseSound) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyDataId.UseSound); else SetProperty(PropertyDataId.UseSound, (uint)value); }
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

        public int? UseCreateQuantity
        {
            get => GetProperty(PropertyInt.UseCreateQuantity);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.UseCreateQuantity); else SetProperty(PropertyInt.UseCreateQuantity, value.Value); }
        }

        public int? ResistLockpick
        {
            get => GetProperty(PropertyInt.ResistLockpick);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ResistLockpick); else SetProperty(PropertyInt.ResistLockpick, value.Value); }
        }

        public uint? VictimId
        {
            get => GetProperty(PropertyInstanceId.Victim);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Victim); else SetProperty(PropertyInstanceId.Victim, value.Value); }
        }

        public uint? KillerId
        {
            get => GetProperty(PropertyInstanceId.Killer);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Killer); else SetProperty(PropertyInstanceId.Killer, value.Value); }
        }

        /* Ratings */

        public int? DamageRating
        {
            get => GetProperty(PropertyInt.DamageRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.DamageRating); else SetProperty(PropertyInt.DamageRating, value.Value); }
        }

        public int? DamageResistRating
        {
            get => GetProperty(PropertyInt.DamageResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.DamageResistRating); else SetProperty(PropertyInt.DamageResistRating, value.Value); }
        }

        public int? CritDamageRating
        {
            get => GetProperty(PropertyInt.CritDamageRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CritDamageRating); else SetProperty(PropertyInt.CritDamageRating, value.Value); }
        }

        public int? CritDamageResistRating
        {
            get => GetProperty(PropertyInt.CritDamageResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CritDamageResistRating); else SetProperty(PropertyInt.CritDamageResistRating, value.Value); }
        }

        /// <summary>
        /// Increases the chance of landing a critical hit
        /// </summary>
        public int? CritRating
        {
            get => GetProperty(PropertyInt.CritRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CritRating); else SetProperty(PropertyInt.CritRating, value.Value); }
        }

        /// <summary>
        /// Decreases the chance of landing a critical hit
        /// </summary>
        public int? CritResistRating
        {
            get => GetProperty(PropertyInt.CritResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CritResistRating); else SetProperty(PropertyInt.CritResistRating, value.Value); }
        }

        public int? HealingBoostRating
        {
            get => GetProperty(PropertyInt.HealingBoostRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HealingBoostRating); else SetProperty(PropertyInt.HealingBoostRating, value.Value); }
        }

        public int? HealingResistRating
        {
            get => GetProperty(PropertyInt.HealingResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HealingResistRating); else SetProperty(PropertyInt.HealingResistRating, value.Value); }
        }

        public int? LifeResistRating
        {
            get => GetProperty(PropertyInt.LifeResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.LifeResistRating); else SetProperty(PropertyInt.LifeResistRating, value.Value); }
        }

        public int? DotResistRating
        {
            get => GetProperty(PropertyInt.DotResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.DotResistRating); else SetProperty(PropertyInt.DotResistRating, value.Value); }
        }

        public int? NetherResistRating
        {
            get => GetProperty(PropertyInt.NetherResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.NetherResistRating); else SetProperty(PropertyInt.NetherResistRating, value.Value); }
        }

        public int? PKDamageRating
        {
            get => GetProperty(PropertyInt.PKDamageRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PKDamageRating); else SetProperty(PropertyInt.PKDamageRating, value.Value); }
        }

        public int? PKDamageResistRating
        {
            get => GetProperty(PropertyInt.PKDamageResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PKDamageResistRating); else SetProperty(PropertyInt.PKDamageResistRating, value.Value); }
        }

        public int? Lifespan
        {
            get => GetProperty(PropertyInt.Lifespan);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Lifespan); else SetProperty(PropertyInt.Lifespan, value.Value); }
        }

        //public int? RemainingLifespan
        //{
        //    get => GetProperty(PropertyInt.RemainingLifespan);
        //    set { if (!value.HasValue) RemoveProperty(PropertyInt.RemainingLifespan); else SetProperty(PropertyInt.RemainingLifespan, value.Value); }
        //}

        public bool HearLocalSignals
        {
            get => (GetProperty(PropertyInt.HearLocalSignals) ?? 0) != 0;
            set { if (!value) RemoveProperty(PropertyInt.HearLocalSignals); else SetProperty(PropertyInt.HearLocalSignals, 1); }
        }

        public int HearLocalSignalsRadius
        {
            get => GetProperty(PropertyInt.HearLocalSignalsRadius) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.HearLocalSignalsRadius); else SetProperty(PropertyInt.HearLocalSignalsRadius, value); }
        }

        public string TinkerLog
        {
            get => GetProperty(PropertyString.TinkerLog);
            set { if (value == null) RemoveProperty(PropertyString.TinkerLog); else SetProperty(PropertyString.TinkerLog, value); }
        }
        
        public int? CreatureKills
        {
            get => GetProperty(PropertyInt.CreatureKills);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CreatureKills); else SetProperty(PropertyInt.CreatureKills, value.Value); }
        }

        public int? PlayerKillsPk
        {
            get => GetProperty(PropertyInt.PlayerKillsPk);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PlayerKillsPk); else SetProperty(PropertyInt.PlayerKillsPk, value.Value); }
        }

        public int? PlayerKillsPkl
        {
            get => GetProperty(PropertyInt.PlayerKillsPkl);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PlayerKillsPkl); else SetProperty(PropertyInt.PlayerKillsPkl, value.Value); }
        }

        public SummoningMastery? SummoningMastery
        {
            get => (SummoningMastery?)GetProperty(PropertyInt.SummoningMastery);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.SummoningMastery); else SetProperty(PropertyInt.SummoningMastery, (int)value.Value); }
        }

        public double? MaximumVelocity
        {
            get => GetProperty(PropertyFloat.MaximumVelocity);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.MaximumVelocity); else SetProperty(PropertyFloat.MaximumVelocity, value.Value); }
        }

        /// <summary>
        /// Indicates the maximum amount of items w/ this wcid
        /// a player can have in their possession
        /// </summary>
        public int? Unique
        {
            get => GetProperty(PropertyInt.Unique);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Unique); else SetProperty(PropertyInt.Unique, value.Value); }
        }

        /// <summary>
        /// In addition to setting StackSize, this will also set the EncumbranceVal and Value appropriately.
        /// </summary>
        /// <param name="value"></param>
        public void SetStackSize(int? value)
        {
            var isStackable = this is Stackable;
            if (!isStackable)
                return;

            StackSize = value;

            EncumbranceVal = (StackUnitEncumbrance ?? 0) * (StackSize ?? 1);
            Value = (StackUnitValue ?? 0) * (StackSize ?? 1);
        }

        /// <summary>
        /// 0x0E file id
        /// </summary>
        public uint? MutateFilter
        {
            get => GetProperty(PropertyDataId.MutateFilter);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.MutateFilter); else SetProperty(PropertyDataId.MutateFilter, value.Value); }
        }

        /// <summary>
        /// 0x38 file id
        /// </summary>
        public uint? TsysMutationFilter
        {
            get => GetProperty(PropertyDataId.TsysMutationFilter);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.TsysMutationFilter); else SetProperty(PropertyDataId.TsysMutationFilter, value.Value); }
        }

        /// <summary>
        /// Either 1 or 2 for cloaks
        /// 1 = spell proc
        /// 2 = damage reduction proc
        /// </summary>
        public int? CloakWeaveProc
        {
            get => GetProperty(PropertyInt.CloakWeaveProc);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CloakWeaveProc); else SetProperty(PropertyInt.CloakWeaveProc, value.Value); }
        }

        /// <summary>
        /// The Damage Rating on a non-creature item
        /// </summary>
        public int? GearDamage
        {
            get => GetProperty(PropertyInt.GearDamage);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearDamage); else SetProperty(PropertyInt.GearDamage, value.Value); }
        }

        /// <summary>
        /// The Damage Resistance Rating on a non-creature item
        /// </summary>
        public int? GearDamageResist
        {
            get => GetProperty(PropertyInt.GearDamageResist);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearDamageResist); else SetProperty(PropertyInt.GearDamageResist, value.Value); }
        }

        /// <summary>
        /// The Crit Damage Rating on a non-creature item
        /// </summary>
        public int? GearCritDamage
        {
            get => GetProperty(PropertyInt.GearCritDamage);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearCritDamage); else SetProperty(PropertyInt.GearCritDamage, value.Value); }
        }

        /// <summary>
        /// The Crit Damage Resistance Rating on a non-creature item
        /// </summary>
        public int? GearCritDamageResist
        {
            get => GetProperty(PropertyInt.GearCritDamageResist);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearCritDamageResist); else SetProperty(PropertyInt.GearCritDamageResist, value.Value); }
        }

        /// <summary>
        /// The Crit Chance Rating on a non-creature item
        /// </summary>
        public int? GearCrit
        {
            get => GetProperty(PropertyInt.GearCrit);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearCrit); else SetProperty(PropertyInt.GearCrit, value.Value); }
        }

        /// <summary>
        /// The Crit Chance Resistance Rating on a non-creature item
        /// </summary>
        public int? GearCritResist
        {
            get => GetProperty(PropertyInt.GearCritResist);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearCritResist); else SetProperty(PropertyInt.GearCritResist, value.Value); }
        }

        /// <summary>
        /// The Healing Boost Rating on a non-creature item
        /// </summary>
        public int? GearHealingBoost
        {
            get => GetProperty(PropertyInt.GearHealingBoost);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearHealingBoost); else SetProperty(PropertyInt.GearHealingBoost, value.Value); }
        }

        /// <summary>
        /// The MaxHealth Boost Rating on a creature or item
        /// </summary>
        public int? GearMaxHealth
        {
            get => GetProperty(PropertyInt.GearMaxHealth);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearMaxHealth); else SetProperty(PropertyInt.GearMaxHealth, value.Value); }
        }

        public int? GearPKDamageRating
        {
            get => GetProperty(PropertyInt.GearPKDamageRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearPKDamageRating); else SetProperty(PropertyInt.GearPKDamageRating, value.Value); }
        }

        public int? GearPKDamageResistRating
        {
            get => GetProperty(PropertyInt.GearPKDamageResistRating);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GearPKDamageResistRating); else SetProperty(PropertyInt.GearPKDamageResistRating, value.Value); }
        }

        public int? ResistItemAppraisal
        {
            get => GetProperty(PropertyInt.ResistItemAppraisal);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ResistItemAppraisal); else SetProperty(PropertyInt.ResistItemAppraisal, value.Value); }
        }

        public HookGroupType? HookGroup
        {
            get => (HookGroupType?)GetProperty(PropertyInt.HookGroup);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HookGroup); else SetProperty(PropertyInt.HookGroup, (int)value.Value); }
        }

        public PropertyAttribute? ItemAttributeLimit
        {
            get => (PropertyAttribute?)GetProperty(PropertyInt.ItemAttributeLimit);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemAttributeLimit); else SetProperty(PropertyInt.ItemAttributeLimit, (int)value.Value); }
        }

        public int? ItemAttributeLevelLimit
        {
            get => GetProperty(PropertyInt.ItemAttributeLevelLimit);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemAttributeLevelLimit); else SetProperty(PropertyInt.ItemAttributeLevelLimit, value.Value); }
        }

        public PropertyAttribute2nd? ItemAttribute2ndLimit
        {
            get => (PropertyAttribute2nd?)GetProperty(PropertyInt.ItemAttribute2ndLimit);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemAttribute2ndLimit); else SetProperty(PropertyInt.ItemAttribute2ndLimit, (int)value.Value); }
        }

        public int? ItemAttribute2ndLevelLimit
        {
            get => GetProperty(PropertyInt.ItemAttribute2ndLevelLimit);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemAttribute2ndLevelLimit); else SetProperty(PropertyInt.ItemAttribute2ndLevelLimit, value.Value); }
        }

        public double? SoldTimestamp
        {
            get => GetProperty(PropertyFloat.SoldTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.SoldTimestamp); else SetProperty(PropertyFloat.SoldTimestamp, value.Value); }
        }

        public bool AllowGive
        {
            get => GetProperty(PropertyBool.AllowGive) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AllowGive); else SetProperty(PropertyBool.AllowGive, value); }
        }

        public bool AiAcceptEverything
        {
            get => GetProperty(PropertyBool.AiAcceptEverything) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AiAcceptEverything); else SetProperty(PropertyBool.AiAcceptEverything, value); }
        }

        public ImbuedEffectType ImbuedEffect
        {
            get => (ImbuedEffectType)(GetProperty(PropertyInt.ImbuedEffect) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.ImbuedEffect); else SetProperty(PropertyInt.ImbuedEffect, (int)value); }
        }

        public bool DontTurnOrMoveWhenGiving
        {
            get => GetProperty(PropertyBool.DontTurnOrMoveWhenGiving) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.DontTurnOrMoveWhenGiving); else SetProperty(PropertyBool.DontTurnOrMoveWhenGiving, value); }
        }

        /// <summary>
        /// Determines the rotation speed for projectiles in global X
        /// </summary>
        public double? RotationSpeed
        {
            get => GetProperty(PropertyFloat.RotationSpeed);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.RotationSpeed); else SetProperty(PropertyFloat.RotationSpeed, value.Value); }
        }

        public bool HasMissileFlightPlacement => CSetup.HasMissileFlightPlacement;

        /// <summary>
        /// For items sold by vendors, StackSize of shop item profile from Vendor's CreateList.
        /// This value is only set by Vendor.LoadInventoryItem, and is almost always -1 which means the item has no supply limits per transaction.
        /// If not unlimited, client will only allow you to buy or add to buy list up this number of items for a single transaction.
        /// </summary>
        public int? VendorShopCreateListStackSize;
    }
}
