using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Database.Models.Shard;

using log4net;

using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Entity.WorldObjects;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;

using AceObjectGeneratorProfile = ACE.Entity.AceObjectGeneratorProfile;
using AceObjectInventory = ACE.Entity.AceObjectInventory;
using AceObjectPropertiesBook = ACE.Entity.AceObjectPropertiesBook;
using AceObjectPropertiesBool = ACE.Entity.AceObjectPropertiesBool;
using AceObjectPropertiesDouble = ACE.Entity.AceObjectPropertiesDouble;
using AceObjectPropertiesInt = ACE.Entity.AceObjectPropertiesInt;
using AceObjectPropertiesSpell = ACE.Entity.AceObjectPropertiesSpell;
using AceObjectPropertiesString = ACE.Entity.AceObjectPropertiesString;
using Position = ACE.Entity.Position;

namespace ACE.Server.Entity
{
    public abstract class WorldObject : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This is base weenie that should have come from the world db.
        /// You should not manipulate any of these values. This weenie object is shared by all other instances.
        /// </summary>
        public Weenie Weenie { get; }

        /// <summary>
        /// This is object property overrides that should have come from the shard db (or init to defaults of object is new to this instance).
        /// You should not manipulate these values directly. To manipulate this use the exposed SetProperty and RemoveProperty functions instead.
        /// </summary>
        public Biota Biota { get; }

        /// <summary>
        /// This is object property overrides that only exist during this session.
        /// You should not manipulate these values directly. To manipulate this use the exposed SetProperty and RemoveProperty functions instead.
        /// </summary>
        public Biota Ephemeral { get; } = new Biota();

        /// <summary>
        /// You should only set this once, after initial object creation. Changing this value a 2nd time will most likely cause the end of days.
        /// This is just a wrapper around Biota.Id
        /// </summary>
        public ObjectGuid Guid
        {
            get => new ObjectGuid(Biota.Id);
            set => Biota.Id = value.Full;
        }

        public ObjectDescriptionFlag DescriptionFlags { get; protected set; }

        public PhysicsDescriptionFlag PhysicsDescriptionFlag { get; protected set; }

        public UpdatePositionFlag PositionFlag { get; protected set; }

        public MotionState CurrentMotionState { get; set; }

        public SequenceManager Sequences { get; } = new SequenceManager();

        public virtual float ListeningRadius { get; protected set; } = 5f;

        /// <summary>
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        protected WorldObject(Weenie weenie, Biota biota = null)
        {
            Weenie = weenie;

            Biota = biota;

            if (Biota == null) // If no biota was passed our base (THATS US!) will instantiate one, and we will initialize it with appropriate default values
            {
                Biota = new Biota();

                // If we restore this biota from the db, we'll need these values to give it the appropriate weenie
                Biota.WeenieClassId = weenie.ClassId;
                Biota.WeenieType = weenie.Type;
            }

            Sequences.AddOrSetSequence(SequenceType.ObjectPosition, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectMovement, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectState, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectVector, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectTeleport, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectServerControl, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectForcePosition, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectVisualDesc, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectInstance, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.Motion, new UShortSequence(1, 0x7FFF)); // MSB is reserved, so set max value to exclude it.

            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevel, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelHealth, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelStamina, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateAttribute2ndLevelMana, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdateSkill, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyBool, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyInt64, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyDouble, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyString, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PrivateUpdatePropertyDataID, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyBool, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyInt, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyInt64, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyDouble, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyString, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyDataID, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.PublicUpdatePropertyInstanceId, new ByteSequence(false));

            Sequences.AddOrSetSequence(SequenceType.SetStackSize, new ByteSequence(false));
            Sequences.AddOrSetSequence(SequenceType.Confirmation, new ByteSequence(false));

            return;

            SetObjectDescriptionBools();
            SetPhysicsStateBools();

            if (Placement == null)
                Placement = global::ACE.Entity.Enum.Placement.Resting;

            GetClothingBase();

            SelectGeneratorProfiles();
            UpdateGeneratorInts();
            QueueGenerator();

            QueueNextHeartBeat();

            GenerateWieldList();
        }

        // We try to get from the Ephemeral first, then the Biota and lastly the Weenie.
        public bool? GetProperty(PropertyBool property) { return Ephemeral.GetProperty(property) ?? Biota.GetProperty(property) ?? Weenie.GetProperty(property); }
        public uint? GetProperty(PropertyDataId property) { return Ephemeral.GetProperty(property) ?? Biota.GetProperty(property) ?? Weenie.GetProperty(property); }
        public double? GetProperty(PropertyDouble property) { return Ephemeral.GetProperty(property) ?? Biota.GetProperty(property) ?? Weenie.GetProperty(property); }
        public int? GetProperty(PropertyInstanceId property) { return Ephemeral.GetProperty(property) ?? Biota.GetProperty(property) ?? Weenie.GetProperty(property); }
        public int? GetProperty(PropertyInt property) { return Ephemeral.GetProperty(property) ?? Biota.GetProperty(property) ?? Weenie.GetProperty(property); }
        public long? GetProperty(PropertyInt64 property) { return Ephemeral.GetProperty(property) ?? Biota.GetProperty(property) ?? Weenie.GetProperty(property); }
        public string GetProperty(PropertyString property) { return Ephemeral.GetProperty(property) ?? Biota.GetProperty(property) ?? Weenie.GetProperty(property); }

        // When we try to set a property, if we're setting the default of the Weenie we'll just remove the value from the Biota and Ephemeral
        public void SetProperty(PropertyBool property, bool value) { if (Weenie.GetProperty(property) == value) RemoveProperty(property); else if (EphemeralProperties.PropertiesBool.Contains((ushort)property)) Ephemeral.SetProperty(property, value); else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyDataId property, uint value) { if (Weenie.GetProperty(property) == value) RemoveProperty(property); else if (EphemeralProperties.PropertiesDataId.Contains((ushort)property)) Ephemeral.SetProperty(property, value); else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyDouble property, double value) { if (Weenie.GetProperty(property) == value) RemoveProperty(property); else if (EphemeralProperties.PropertiesDouble.Contains((ushort)property)) Ephemeral.SetProperty(property, value); else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInstanceId property, int value) { if (Weenie.GetProperty(property) == value) RemoveProperty(property); else if (EphemeralProperties.PropertiesInstanceId.Contains((ushort)property)) Ephemeral.SetProperty(property, value); else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInt property, int value) { if (Weenie.GetProperty(property) == value) RemoveProperty(property); else if (EphemeralProperties.PropertiesInt.Contains((ushort)property)) Ephemeral.SetProperty(property, value); else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyInt64 property, long value) { if (Weenie.GetProperty(property) == value) RemoveProperty(property); else if (EphemeralProperties.PropertiesInt64.Contains((ushort)property)) Ephemeral.SetProperty(property, value); else Biota.SetProperty(property, value); }
        public void SetProperty(PropertyString property, string value) { if (Weenie.GetProperty(property) == value) RemoveProperty(property); else if (EphemeralProperties.PropertiesString.Contains((ushort)property)) Ephemeral.SetProperty(property, value); else Biota.SetProperty(property, value); }

        // We only remove from Ephemeral and the Biota.
        public void RemoveProperty(PropertyBool property) { Ephemeral.RemoveProperty(property); Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyDataId property) { Ephemeral.RemoveProperty(property); Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyDouble property) { Ephemeral.RemoveProperty(property); Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInstanceId property) { Ephemeral.RemoveProperty(property); Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInt property) { Ephemeral.RemoveProperty(property); Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyInt64 property) { Ephemeral.RemoveProperty(property); Biota.RemoveProperty(property); }
        public void RemoveProperty(PropertyString property) { Ephemeral.RemoveProperty(property); Biota.RemoveProperty(property); }

        // When we get all properties, we first get the properties from the weenie, then we get the overrides from the biota and lastly override with Ephemeral
        public Dictionary<PropertyBool, bool> GetAllPropertyBools()
        {
            var results = new Dictionary<PropertyBool, bool>();

            foreach (var property in Weenie.WeeniePropertiesBool)
                results[(PropertyBool)property.Type] = property.Value;

            foreach (var property in Biota.BiotaPropertiesBool)
                results[(PropertyBool)property.Type] = property.Value;

            foreach (var property in Ephemeral.BiotaPropertiesBool)
                results[(PropertyBool)property.Type] = property.Value;

            return results;
        }

        public Dictionary<PropertyDataId, uint> GetAllPropertyDataId()
        {
            var results = new Dictionary<PropertyDataId, uint>();

            foreach (var property in Weenie.WeeniePropertiesDID)
                results[(PropertyDataId)property.Type] = property.Value;

            foreach (var property in Biota.BiotaPropertiesDID)
                results[(PropertyDataId)property.Type] = property.Value;

            foreach (var property in Ephemeral.BiotaPropertiesDID)
                results[(PropertyDataId)property.Type] = property.Value;

            return results;
        }

        public Dictionary<PropertyDouble, double> GetAllPropertyDouble()
        {
            var results = new Dictionary<PropertyDouble, double>();

            foreach (var property in Weenie.WeeniePropertiesFloat)
                results[(PropertyDouble)property.Type] = property.Value;

            foreach (var property in Biota.BiotaPropertiesFloat)
                results[(PropertyDouble)property.Type] = property.Value;

            foreach (var property in Ephemeral.BiotaPropertiesFloat)
                results[(PropertyDouble)property.Type] = property.Value;

            return results;
        }

        public Dictionary<PropertyInstanceId, int> GetAllPropertyInstanceId()
        {
            var results = new Dictionary<PropertyInstanceId, int>();

            foreach (var property in Weenie.WeeniePropertiesIID)
                results[(PropertyInstanceId)property.Type] = property.Value;

            foreach (var property in Biota.BiotaPropertiesIID)
                results[(PropertyInstanceId)property.Type] = property.Value;

            foreach (var property in Ephemeral.BiotaPropertiesIID)
                results[(PropertyInstanceId)property.Type] = property.Value;

            return results;
        }

        public Dictionary<PropertyInt, int> GetAllPropertyInt()
        {
            var results = new Dictionary<PropertyInt, int>();

            foreach (var property in Weenie.WeeniePropertiesInt)
                results[(PropertyInt)property.Type] = property.Value;

            foreach (var property in Biota.BiotaPropertiesInt)
                results[(PropertyInt)property.Type] = property.Value;

            foreach (var property in Ephemeral.BiotaPropertiesInt)
                results[(PropertyInt)property.Type] = property.Value;

            return results;
        }

        public Dictionary<PropertyInt64, long> GetAllPropertyInt64()
        {
            var results = new Dictionary<PropertyInt64, long>();

            foreach (var property in Weenie.WeeniePropertiesInt64)
                results[(PropertyInt64)property.Type] = property.Value;

            foreach (var property in Biota.BiotaPropertiesInt64)
                results[(PropertyInt64)property.Type] = property.Value;

            foreach (var property in Ephemeral.BiotaPropertiesInt64)
                results[(PropertyInt64)property.Type] = property.Value;

            return results;
        }

        public Dictionary<PropertyString, string> GetAllPropertyString()
        {
            var results = new Dictionary<PropertyString, string>();

            foreach (var property in Weenie.WeeniePropertiesString)
                results[(PropertyString)property.Type] = property.Value;

            foreach (var property in Biota.BiotaPropertiesString)
                results[(PropertyString)property.Type] = property.Value;

            foreach (var property in Ephemeral.BiotaPropertiesString)
                results[(PropertyString)property.Type] = property.Value;

            return results;
        }


        private void SetObjectDescriptionBools()
        {
            // TODO these are backwards. In the new model, we should be setting the flags for PhysicsDescriptionFlag based on properties that are set
            // Several flags are set in the ctor, but if these flags can come from the shard we need to update the descriptionFlag. Is that even possible?
            // If these values are not modified after the original ctor, then there's no need for this fn

            // TODO: More uncommentting and wiring up for other flags
            ////None                   = 0x00000000,
            ////Openable               = 0x00000001,
            // if (AceObject.Openable ?? false)
            //    Openable = true;
            ////Inscribable            = 0x00000002,
            if (AceObject.Inscribable.HasValue)
                Inscribable = (bool)AceObject.Inscribable;
            ////Stuck                  = 0x00000004,
            if (AceObject.Stuck.HasValue)
                Stuck = (bool)AceObject.Stuck;
            ////Player                 = 0x00000008,
            // if (AceObject.Player ?? false)
            //    Player = true;
            ////Attackable             = 0x00000010,
            if (AceObject.Attackable.HasValue)
                Attackable = (bool)AceObject.Attackable;
            ////PlayerKiller           = 0x00000020,
            // if (AceObject.PlayerKiller ?? false)
            //    PlayerKiller = true;
            ////HiddenAdmin            = 0x00000040,
            if (AceObject.HiddenAdmin.HasValue)
                HiddenAdmin = (bool)AceObject.HiddenAdmin;
            ////UiHidden               = 0x00000080,
            if (AceObject.UiHidden.HasValue)
                UiHidden = (bool)AceObject.UiHidden;
            ////Book                   = 0x00000100,
            // if (AceObject.Book ?? false)
            //    Book = true;
            ////Vendor                 = 0x00000200,
            // if (AceObject.Vendor ?? false)
            //    Vendor = true;
            ////PkSwitch               = 0x00000400,
            // if (AceObject.PkSwitch ?? false)
            //    PkSwitch = true;
            ////NpkSwitch              = 0x00000800,
            // if (AceObject.NpkSwitch ?? false)
            //    NpkSwitch = true;
            ////Door                   = 0x00001000,
            // if (AceObject.Door ?? false)
            //    Door = true;
            ////Corpse                 = 0x00002000,
            // if (AceObject.Corpse ?? false)
            //    Corpse = true;
            ////LifeStone              = 0x00004000,
            // if (AceObject.LifeStone ?? false)
            //    LifeStone = true;
            ////Food                   = 0x00008000,
            // if (AceObject.Food ?? false)
            //    Food = true;
            ////Healer                 = 0x00010000,
            // if (AceObject.Healer ?? false)
            //    Healer = true;
            ////Lockpick               = 0x00020000,
            // if (AceObject.Lockpick ?? false)
            //    Lockpick = true;
            ////Portal                 = 0x00040000,
            // if (AceObject.Portal ?? false)
            //    Portal = true;
            ////Admin                  = 0x00100000,
            // if (AceObject.Admin ?? false)
            //    Admin = true;
            ////FreePkStatus           = 0x00200000,
            // if (AceObject.FreePkStatus ?? false)
            //    FreePkStatus = true;
            ////ImmuneCellRestrictions = 0x00400000,
            if (AceObject.IgnoreHouseBarriers.HasValue)
                ImmuneCellRestrictions = (bool)AceObject.IgnoreHouseBarriers;
            ////RequiresPackSlot       = 0x00800000,
            if (AceObject.RequiresBackpackSlot.HasValue)
                RequiresPackSlot = (bool)AceObject.RequiresBackpackSlot;
            ////Retained               = 0x01000000,
            if (AceObject.Retained.HasValue)
                Retained = (bool)AceObject.Retained;
            ////PkLiteStatus           = 0x02000000,
            // if (AceObject.PkLiteStatus ?? false)
            //    PkLiteStatus = true;
            ////IncludesSecondHeader   = 0x04000000,
            // if (AceObject.IncludesSecondHeader ?? false)
            //    IncludesSecondHeader = true;
            ////BindStone              = 0x08000000,
            // if (AceObject.BindStone ?? false)
            //    BindStone = true;
            ////VolatileRare           = 0x10000000,
            // if (AceObject.VolatileRare ?? false)
            //    VolatileRare = true;
            ////WieldOnUse             = 0x20000000,
            if (AceObject.WieldOnUse.HasValue)
                WieldOnUse = (bool)AceObject.WieldOnUse;
            ////WieldLeft              = 0x40000000,
            if (AceObject.AutowieldLeft.HasValue)
                WieldLeft = (bool)AceObject.AutowieldLeft;
        }

        private void SetPhysicsStateBools()
        {
            // TODO these are backwards. In the new model, we should be setting the flags for DescriptionFlags based on properties that are set
            // Several flags are set in the ctor, but if these flags can come from the shard we need to update the descriptionFlag. Is that even possible?
            // If these values are not modified after the original ctor, then there's no need for this fn

            // TODO: More uncommentting and wiring up for other flags

            ////Static                      = 0x00000001,
            // if (AceObject.Static ?? false)
            //    Static = true;
            ////Unused1                     = 0x00000002,
            ////Ethereal                    = 0x00000004,
            if (AceObject.Ethereal.HasValue)
                Ethereal = (bool)AceObject.Ethereal;
            ////ReportCollision             = 0x00000008,
            if (AceObject.ReportCollisions.HasValue)
                ReportCollision = (bool)AceObject.ReportCollisions;
            ////IgnoreCollision             = 0x00000010,
            if (AceObject.IgnoreCollisions.HasValue)
                IgnoreCollision = (bool)AceObject.IgnoreCollisions;
            ////NoDraw                      = 0x00000020,
            if (AceObject.NoDraw.HasValue)
                NoDraw = (bool)AceObject.NoDraw;
            ////Missile                     = 0x00000040,
            // if (AceObject.Missile ?? false)
            //    Missile = true;
            ////Pushable                    = 0x00000080,
            // if (AceObject.Pushable ?? false)
            //    Pushable = true;
            ////AlignPath                   = 0x00000100,
            // if (AceObject.AlignPath ?? false)
            //    AlignPath = true;
            ////PathClipped                 = 0x00000200,
            // if (AceObject.PathClipped ?? false)
            //    PathClipped = true;
            ////Gravity                     = 0x00000400,
            if (AceObject.GravityStatus.HasValue)
                Gravity = (bool)AceObject.GravityStatus;
            ////LightingOn                  = 0x00000800,
            if (AceObject.LightsStatus.HasValue)
                LightingOn = (bool)AceObject.LightsStatus;
            ////ParticleEmitter             = 0x00001000,
            // if (AceObject.ParticleEmitter ?? false)
            //    ParticleEmitter = true;
            ////Unused2                     = 0x00002000,
            ////Hidden                      = 0x00004000,
            // if (AceObject.Hidden ?? false) // Probably PropertyBool.Visibility which would make me think if true, Hidden is false... Opposite of most other bools
            //    Hidden = true;
            ////ScriptedCollision           = 0x00008000,
            if (AceObject.ScriptedCollision.HasValue)
                ScriptedCollision = (bool)AceObject.ScriptedCollision;
            ////HasPhysicsBsp               = 0x00010000,
            // if (AceObject.HasPhysicsBsp ?? false)
            //    HasPhysicsBsp = true;
            ////Inelastic                   = 0x00020000,
            if (AceObject.Inelastic.HasValue)
                Inelastic = (bool)AceObject.Inelastic;
            ////HasDefaultAnim              = 0x00040000,
            // if (AceObject.HasDefaultAnim ?? false)
            //    HasDefaultAnim = true;
            ////HasDefaultScript            = 0x00080000,
            // if (AceObject.HasDefaultScript ?? false) // Probably based on PhysicsDescriptionFlag
            //    HasDefaultScript = true;
            ////Cloaked                     = 0x00100000,
            // if (AceObject.Cloaked ?? false) // PropertyInt.CloakStatus probably plays in to this.
            //    Cloaked = true;
            ////ReportCollisionAsEnviroment = 0x00200000,
            if (AceObject.ReportCollisionsAsEnvironment.HasValue)
                ReportCollisionAsEnviroment = (bool)AceObject.ReportCollisionsAsEnvironment;
            ////EdgeSlide                   = 0x00400000,
            if (AceObject.AllowEdgeSlide.HasValue)
                EdgeSlide = (bool)AceObject.AllowEdgeSlide;
            ////Sledding                    = 0x00800000,
            // if (AceObject.Sledding ?? false)
            //    Sledding = true;
            ////Frozen                      = 0x01000000,
            if (AceObject.IsFrozen.HasValue)
                Frozen = (bool)AceObject.IsFrozen;
        }













        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        // this is just temp so code compiles, remove it later
        public Dictionary<PositionType, Position> Positions = new Dictionary<PositionType, Position>();

        public static float MaxObjectTrackingRange { get; } = 20000f;


        protected AceObject AceObject { get; set; }

        protected internal Dictionary<ObjectGuid, WorldObject> WieldedObjects { get; set; }



        protected internal Dictionary<ObjectGuid, AceObject> Inventory => AceObject.Inventory;

        // This dictionary is only used to load WieldedObjects and to save them.   Other than the load and save, it should never be added to or removed from.
        protected internal Dictionary<ObjectGuid, AceObject> WieldedItems => AceObject.WieldedItems;

        // we need to expose this read only for examine to work. Og II
        public List<AceObjectPropertiesInt> PropertiesInt => AceObject.IntProperties;

        public List<AceObjectPropertiesInt64> PropertiesInt64 => AceObject.Int64Properties;

        public List<AceObjectPropertiesBool> PropertiesBool => AceObject.BoolProperties;

        public List<AceObjectPropertiesString> PropertiesString => AceObject.StringProperties;

        public List<AceObjectPropertiesDouble> PropertiesDouble => AceObject.DoubleProperties;

        public List<AceObjectPropertiesDataId> PropertiesDid => AceObject.DataIdProperties;

        public List<AceObjectPropertiesInstanceId> PropertiesIid => AceObject.InstanceIdProperties;

        public List<AceObjectPropertiesSpell> PropertiesSpellId => AceObject.SpellIdProperties;

        public Dictionary<uint, AceObjectPropertiesBook> PropertiesBook => AceObject.BookProperties;

        #region ObjDesc
        private readonly List<ModelPalette> modelPalettes = new List<ModelPalette>();

        private readonly List<ModelTexture> modelTextures = new List<ModelTexture>();

        private readonly List<Model> models = new List<Model>();

        // subpalettes
        public List<ModelPalette> GetPalettes => modelPalettes.ToList();

        // tmChanges
        public List<ModelTexture> GetTextures => modelTextures.ToList();

        // apChanges
        public List<Model> GetModels => models.ToList();

        public void AddPalette(uint paletteId, ushort offset, ushort length)
        {
            ModelPalette newpalette = new ModelPalette(paletteId, offset, length);
            modelPalettes.Add(newpalette);
        }

        public void AddTexture(byte index, uint oldtexture, uint newtexture)
        {
            ModelTexture nextTexture = new ModelTexture(index, oldtexture, newtexture);
            modelTextures.Add(nextTexture);
        }

        public void AddModel(byte index, uint modelresourceid)
        {
            Model newmodel = new Model(index, modelresourceid);
            models.Add(newmodel);
        }

        public void ClearObjDesc()
        {
            modelPalettes.Clear();
            modelTextures.Clear();
            models.Clear();
        }
        // START of Logical Model Data

        public uint? PaletteBaseId
        {
            get => AceObject.PaletteBaseDID;
            set { AceObject.PaletteBaseDID = value; }
        }
        #endregion

        #region PhysicsDesc
        // PhysicsData Logical

        // bitfield

        // state
        public PhysicsState PhysicsState
        {
            get => (PhysicsState)AceObject.PhysicsState;
            set { AceObject.PhysicsState = (int)value; }
        }

        /// <summary>
        /// setup_id in aclogviewer - used to get the correct model out of the dat file
        /// </summary>
        public uint? SetupTableId
        {
            get => AceObject.SetupDID;
            set { AceObject.SetupDID = value; }
        }

        /// <summary>
        /// mtable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint? MotionTableId
        {
            get => AceObject.MotionTableDID;
            set { AceObject.MotionTableDID = value; }
        }
        /// <summary>
        /// stable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint? SoundTableId
        {
            get => AceObject.SoundTableDID;
            set { AceObject.SoundTableDID = value; }
        }
        /// <summary>
        /// phstable_id in aclogviewer This is the physics table for the object.   Looked up from dat file.
        /// </summary>
        public uint? PhysicsTableId
        {
            get => AceObject.PhysicsEffectTableDID;
            set { AceObject.PhysicsEffectTableDID = value; }
        }

        public int? ParentLocation
        {
            get => AceObject.ParentLocation;
            set { AceObject.ParentLocation = value; }
        }

        public List<HeldItem> Children { get; } = new List<HeldItem>();

        public float? ObjScale
        {
            get => AceObject.DefaultScale;
            set { AceObject.DefaultScale = value; }
        }

        public float? Friction
        {
            get => AceObject.Friction;
            set { AceObject.Friction = value; }
        }

        public float? Elasticity
        {
            get => AceObject.Elasticity;
            set { AceObject.Elasticity = value; }
        }

        public Placement? Placement // Sometimes known as AnimationFrame
        {
            get => (Placement?)AceObject.Placement;
            set { AceObject.Placement = (int?)value; }
        }

        public AceVector3 Acceleration { get; set; }

        public float? Translucency
        {
            get => AceObject.Translucency;
            set { AceObject.Translucency = value; }
        }

        public AceVector3 Velocity = null;

        public AceVector3 Omega = null;

        // movement_buffer


        public uint? DefaultScriptId
        {
            get => Script;
            set { Script = (ushort?)value; }
        }

        public float? DefaultScriptIntensity
        {
            get => AceObject.PhysicsScriptIntensity;
            set { AceObject.PhysicsScriptIntensity = value; }
        }

        // pos
        public virtual Position Location
        {
            get => AceObject.Location;
            set
            {
                /*
                log.Debug($"{Name} moved to {Position}");

                Position = value;
                */
                if (AceObject.Location != null)
                    LastUpdatedTicks = WorldManager.PortalYearTicks;
                AceObject.Location = value;
            }
        }

        #endregion

        #region WDesc
        #region always present
        // bitfield
        public WeenieHeaderFlag WeenieFlags
        {
            get => SetWeenieHeaderFlag();
            protected internal set { AceObject.WeenieHeaderFlags = (uint)value; }
        }

        // bitfield2
        public WeenieHeaderFlag2 WeenieFlags2
        {
            get
            {
                WeenieHeaderFlag2 flags = SetWeenieHeaderFlag2();
                if (flags != WeenieHeaderFlag2.None)
                    IncludesSecondHeader = true;
                return flags;
            }
            protected internal set => AceObject.WeenieHeaderFlags2 = (uint)value;
        }

        public string Name
        {
            get => AceObject.Name;
            protected set { AceObject.Name = value; }
        }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public uint WeenieClassId
        {
            get => AceObject.WeenieClassId;
            protected set { AceObject.WeenieClassId = value; }
        }

        public uint? IconId
        {
            get => AceObject.IconDID;
            set { AceObject.IconDID = value; }
        }

        // type
        public ItemType ItemType
        {
            get => (ItemType?)AceObject.ItemType ?? 0;
            protected set { AceObject.ItemType = (int)value; }
        }


        #endregion
        #region optional
        public string NamePlural
        {
            get => AceObject.PluralName;
            set { AceObject.PluralName = value; }
        }

        public byte? ItemCapacity
        {
            get => AceObject.ItemsCapacity;
            set { AceObject.ItemsCapacity = value; }
        }

        public byte? ContainerCapacity
        {
            get => AceObject.ContainersCapacity;
            set { AceObject.ContainersCapacity = value; }
        }

        public AmmoType? AmmoType
        {
            get => (AmmoType?)AceObject.AmmoType;
            set { AceObject.AmmoType = (int?)value; }
        }

        public virtual int? Value
        {
            get => (StackUnitValue * (StackSize ?? 1));
            set { AceObject.Value = value; }
        }

        public virtual int? StackUnitValue => Weenie.GetProperty(PropertyInt.StackUnitValue) ?? 0;

        public Usable? Usable
        {
            get => (Usable?)AceObject.ItemUseable;
            set { AceObject.ItemUseable = (int?)value; }
        }

        public float? UseRadius
        {
            get => AceObject.UseRadius;
            set { AceObject.UseRadius = value; }
        }

        public int? TargetType
        {
            get => AceObject.TargetType;
            set { AceObject.TargetType = value; }
        }

        public UiEffects? UiEffects
        {
            get => (UiEffects?)AceObject.UiEffects;
            set { AceObject.UiEffects = (int?)value; }
        }

        public CombatUse? CombatUse
        {
            get => (CombatUse?)AceObject.CombatUse;
            set { AceObject.CombatUse = (byte?)value; }
        }

        /// <summary>
        /// This is used to indicate the number of uses remaining.  Example 32 uses left out of 50 (MaxStructure)
        /// </summary>
        public ushort? Structure
        {
            get => AceObject.Structure;
            set { AceObject.Structure = value; }
        }

        /// <summary>
        /// Use Limit - example 50 use healing kit
        /// </summary>
        public ushort? MaxStructure
        {
            get => AceObject.MaxStructure;
            set { AceObject.MaxStructure = value; }
        }

        public virtual ushort? StackSize
        {
            get => AceObject.StackSize;
            set { AceObject.StackSize = value; }
        }

        public ushort? MaxStackSize
        {
            get => AceObject.MaxStackSize;
            set { AceObject.MaxStackSize = value; }
        }

        public uint? ContainerId
        {
            get => AceObject.ContainerIID;
            set { AceObject.ContainerIID = value; }
        }

        public int? PlacementPosition
        {
            get => AceObject.PlacementPosition;
            set { AceObject.PlacementPosition = value; }
        }

        public uint? WielderId
        {
            get => AceObject.WielderIID;
            set { AceObject.WielderIID = value; }
        }

        // Locations
        public EquipMask? ValidLocations
        {
            get => (EquipMask?)AceObject.ValidLocations;
            set { AceObject.ValidLocations = (int?)value; }
        }

        public EquipMask? CurrentWieldedLocation
        {
            get => (EquipMask?)AceObject.CurrentWieldedLocation;
            set { AceObject.CurrentWieldedLocation = (int?)value; }
        }

        public CoverageMask? Priority
        {
            get => (CoverageMask?)AceObject.ClothingPriority;
            set { AceObject.ClothingPriority = (int?)value; }
        }

        public RadarColor? RadarColor
        {
            get => (RadarColor?)AceObject.RadarBlipColor;
            set { AceObject.RadarBlipColor = (byte?)value; }
        }

        public RadarBehavior? RadarBehavior
        {
            get => (RadarBehavior?)AceObject.ShowableOnRadar;
            set { AceObject.ShowableOnRadar = (byte?)value; }
        }

        public ushort? Script
        {
            get => AceObject.PhysicsScriptDID;
            set { AceObject.PhysicsScriptDID = value; }
        }

        public float? Workmanship
        {
            get
            {
                if ((ItemWorkmanship != null) && (Structure != null) && (Structure != 0))
                {
                    return (float)Convert.ToDouble(ItemWorkmanship / (10000 * Structure));
                }
                return (ItemWorkmanship);
            }
            set
            {
                if ((Structure != null) && (Structure != 0))
                {
                    ItemWorkmanship = (int)Convert.ToInt32(value * 10000 * Structure);
                }
                else
                {
                    ItemWorkmanship = (int)Convert.ToInt32(value);
                }
            }
        }

        private int? ItemWorkmanship
        {
            get => AceObject.ItemWorkmanship;
            set { AceObject.ItemWorkmanship = value; }
        }

        public virtual ushort? Burden
        {
            get => (ushort)(StackUnitBurden * (StackSize ?? 1));
            set { AceObject.EncumbranceVal = value; }
        }

        public virtual ushort? StackUnitBurden => (ushort?)(Weenie.GetProperty(PropertyInt.EncumbranceVal) ?? 0);

        public Spell? Spell
        {
            get => (Spell?)AceObject.SpellDID;
            set { AceObject.SpellDID = (ushort?)value; }
        }

        /// <summary>
        /// Housing links to another packet, that needs sent.. The HouseRestrictions ACL Control list that contains all the housing data
        /// </summary>
        public uint? HouseOwner { get; set; }

        public uint? HouseRestrictions { get; set; }

        public ushort? HookItemType
        {
            get => AceObject.HookItemType;
            set { AceObject.HookItemType = value; }
        }

        public uint? Monarch { get; set; }

        public ushort? HookType
        {
            get => (ushort?)AceObject.HookType;
            set { AceObject.HookType = value; }
        }

        public uint? IconOverlayId
        {
            get => AceObject.IconOverlayDID;
            set { AceObject.IconOverlayDID = value; }
        }

        public uint? IconUnderlayId
        {
            get => AceObject.IconUnderlayDID;
            set { AceObject.IconUnderlayDID = value; }
        }

        public Material? MaterialType
        {
            get => (Material?)AceObject.MaterialType;
            set { AceObject.MaterialType = (byte?)value; }
        }

        public uint? PetOwner { get; set; }

        public int? CooldownId
        {
            get => AceObject.SharedCooldown;
            set { AceObject.SharedCooldown = value; }
        }

        public double? CooldownDuration
        {
            get => AceObject.CooldownDuration;
            set { AceObject.CooldownDuration = value; }
        }
        #endregion
        #endregion

        #region ObjectDescription Bools
        ////None                   = 0x00000000,
        ////Openable               = 0x00000001,
        public bool Openable
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Openable);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Openable;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Openable;
                // AceObject.Openable = value;
            }
        }
        ////Inscribable            = 0x00000002,
        public bool Inscribable
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Inscribable);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Inscribable;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Inscribable;
                AceObject.Inscribable = value;
            }
        }
        ////Stuck                  = 0x00000004,
        public bool Stuck
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Stuck);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Stuck;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Stuck;
                AceObject.Stuck = value;
            }
        }
        ////Player                 = 0x00000008,
        public bool Player
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Player);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Player;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Player;
                // AceObject.Player = value;
            }
        }
        ////Attackable             = 0x00000010,
        public bool Attackable
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Attackable);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Attackable;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Attackable;
                AceObject.Attackable = value;
            }
        }
        ////PlayerKiller           = 0x00000020,
        public bool PlayerKiller
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.PlayerKiller);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.PlayerKiller;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.PlayerKiller;
                // AceObject.PlayerKiller = value;
            }
        }
        ////HiddenAdmin            = 0x00000040,
        public bool HiddenAdmin
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.HiddenAdmin);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.HiddenAdmin;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.HiddenAdmin;
                AceObject.HiddenAdmin = value;
            }
        }
        ////UiHidden               = 0x00000080,
        public bool UiHidden
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.UiHidden);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.UiHidden;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.UiHidden;
                AceObject.UiHidden = value;
            }
        }
        ////Book                   = 0x00000100,
        public bool Book
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Book);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Book;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Book;
                // AceObject.Book = value;
            }
        }
        ////Vendor                 = 0x00000200,
        public bool Vendor
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Vendor);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Vendor;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Vendor;
                // AceObject.Vendor = value;
            }
        }
        ////PkSwitch               = 0x00000400,
        public bool PkSwitch
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.PkSwitch);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.PkSwitch;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.PkSwitch;
                // AceObject.PkSwitch = value;
            }
        }
        ////NpkSwitch              = 0x00000800,
        public bool NpkSwitch
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.NpkSwitch);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.NpkSwitch;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.NpkSwitch;
                // AceObject.NpkSwitch = value;
            }
        }
        ////Door                   = 0x00001000,
        public bool Door
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Door);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Door;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Door;
                // AceObject.Door = value;
            }
        }
        ////Corpse                 = 0x00002000,
        public bool Corpse
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Corpse);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Corpse;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Corpse;
                // AceObject.Corpse = value;
            }
        }
        ////LifeStone              = 0x00004000,
        public bool LifeStone
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.LifeStone);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.LifeStone;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.LifeStone;
                // AceObject.LifeStone = value;
            }
        }
        ////Food                   = 0x00008000,
        public bool Food
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Food);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Food;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Food;
                // AceObject.Food = value;
            }
        }
        ////Healer                 = 0x00010000,
        public bool Healer
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Healer);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Healer;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Healer;
                // AceObject.Healer = value;
            }
        }
        ////Lockpick               = 0x00020000,
        public bool Lockpick
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Lockpick);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Lockpick;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Lockpick;
                // AceObject.Lockpick = value;
            }
        }
        ////Portal                 = 0x00040000,
        public bool Portal
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Portal);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Portal;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Portal;
                // AceObject.Portal = value;
            }
        }
        ////Admin                  = 0x00100000,
        public bool Admin
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Admin);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Admin;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Admin;
                // AceObject.Admin = value;
            }
        }
        ////FreePkStatus           = 0x00200000,
        public bool FreePkStatus
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.FreePkStatus);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.FreePkStatus;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.FreePkStatus;
                // AceObject.FreePkStatus = value;
            }
        }
        ////ImmuneCellRestrictions = 0x00400000,
        public bool ImmuneCellRestrictions
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.ImmuneCellRestrictions);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.ImmuneCellRestrictions;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.ImmuneCellRestrictions;
                AceObject.IgnoreHouseBarriers = value;
            }
        }
        ////RequiresPackSlot       = 0x00800000,
        public bool RequiresPackSlot
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.RequiresPackSlot);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.RequiresPackSlot;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.RequiresPackSlot;
                AceObject.RequiresBackpackSlot = value;
            }
        }

        public bool UseBackpackSlot => AceObject.UseBackpackSlot;

        ////Retained               = 0x01000000,
        public bool Retained
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.Retained);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.Retained;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.Retained;
                AceObject.Retained = value;
            }
        }
        ////PkLiteStatus           = 0x02000000,
        public bool PkLiteStatus
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.PkLiteStatus);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.PkLiteStatus;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.PkLiteStatus;
                // AceObject.PkLiteStatus = value;
            }
        }
        ////IncludesSecondHeader   = 0x04000000,
        public bool IncludesSecondHeader
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.IncludesSecondHeader);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.IncludesSecondHeader;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.IncludesSecondHeader;
                // AceObject.IncludesSecondHeader = value;
            }
        }
        ////BindStone              = 0x08000000,
        public bool BindStone
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.BindStone);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.BindStone;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.BindStone;
                // AceObject.BindStone = value;
            }
        }
        ////VolatileRare           = 0x10000000,
        public bool VolatileRare
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.VolatileRare);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.VolatileRare;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.VolatileRare;
                // AceObject.VolatileRare = value;
            }
        }
        ////WieldOnUse             = 0x20000000,
        public bool WieldOnUse
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.WieldOnUse);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.WieldOnUse;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.WieldOnUse;
                AceObject.WieldOnUse = value;
            }
        }
        ////WieldLeft              = 0x40000000,
        public bool WieldLeft
        {
            get => DescriptionFlags.HasFlag(ObjectDescriptionFlag.WieldLeft);
            set
            {
                if (value)
                    DescriptionFlags |= ObjectDescriptionFlag.WieldLeft;
                else
                    DescriptionFlags &= ~ObjectDescriptionFlag.WieldLeft;
                AceObject.AutowieldLeft = value;
            }
        }
        #endregion

        #region PhysicsState Bools
        ////Static                      = 0x00000001,
        public bool Static
        {
            get => PhysicsState.HasFlag(PhysicsState.Static);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Static;
                else
                    PhysicsState &= ~PhysicsState.Static;
                // AceObject.Static = value;
            }
        }
        ////Unused1                     = 0x00000002,
        ////Ethereal                    = 0x00000004,
        public bool Ethereal
        {
            get => PhysicsState.HasFlag(PhysicsState.Ethereal);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Ethereal;
                else
                    PhysicsState &= ~PhysicsState.Ethereal;
                AceObject.Ethereal = value;
            }
        }
        ////ReportCollision             = 0x00000008,
        public bool ReportCollision
        {
            get => PhysicsState.HasFlag(PhysicsState.ReportCollision);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.ReportCollision;
                else
                    PhysicsState &= ~PhysicsState.ReportCollision;
                AceObject.ReportCollisions = value;
            }
        }
        ////IgnoreCollision             = 0x00000010,
        public bool IgnoreCollision
        {
            get => PhysicsState.HasFlag(PhysicsState.IgnoreCollision);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.IgnoreCollision;
                else
                    PhysicsState &= ~PhysicsState.IgnoreCollision;
                AceObject.IgnoreCollisions = value;
            }
        }
        ////NoDraw                      = 0x00000020,
        public bool NoDraw
        {
            get => PhysicsState.HasFlag(PhysicsState.NoDraw);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.NoDraw;
                else
                    PhysicsState &= ~PhysicsState.NoDraw;
                AceObject.NoDraw = value;
            }
        }
        ////Missile                     = 0x00000040,
        public bool Missile
        {
            get => PhysicsState.HasFlag(PhysicsState.Missile);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Missile;
                else
                    PhysicsState &= ~PhysicsState.Missile;
                ////AceObject.Missile = value;
            }
        }
        ////Pushable                    = 0x00000080,
        public bool Pushable
        {
            get => PhysicsState.HasFlag(PhysicsState.Pushable);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Pushable;
                else
                    PhysicsState &= ~PhysicsState.Pushable;
                ////AceObject.AlignPath = value;
            }
        }
        ////AlignPath                   = 0x00000100,
        public bool AlignPath
        {
            get => PhysicsState.HasFlag(PhysicsState.AlignPath);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.AlignPath;
                else
                    PhysicsState &= ~PhysicsState.AlignPath;
                ////AceObject.AlignPath = value;
            }
        }
        ////PathClipped                 = 0x00000200,
        public bool PathClipped
        {
            get => PhysicsState.HasFlag(PhysicsState.PathClipped);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.PathClipped;
                else
                    PhysicsState &= ~PhysicsState.PathClipped;
                ////AceObject.PathClipped = value;
            }
        }
        ////Gravity                     = 0x00000400,
        public bool Gravity
        {
            get => PhysicsState.HasFlag(PhysicsState.Gravity);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Gravity;
                else
                    PhysicsState &= ~PhysicsState.Gravity;
                AceObject.GravityStatus = value;
            }
        }
        ////LightingOn                  = 0x00000800,
        public bool LightingOn
        {
            get => PhysicsState.HasFlag(PhysicsState.LightingOn);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.LightingOn;
                else
                    PhysicsState &= ~PhysicsState.LightingOn;
                AceObject.LightsStatus = value;
            }
        }
        ////ParticleEmitter             = 0x00001000,
        public bool ParticleEmitter
        {
            get => PhysicsState.HasFlag(PhysicsState.ParticleEmitter);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.ParticleEmitter;
                else
                    PhysicsState &= ~PhysicsState.ParticleEmitter;
                ////AceObject.HasPhysicsBsp = value;
            }
        }
        ////Unused2                     = 0x00002000,
        ////Hidden                      = 0x00004000,
        public bool Hidden
        {
            get => PhysicsState.HasFlag(PhysicsState.Hidden);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Hidden;
                else
                    PhysicsState &= ~PhysicsState.Hidden;
                // AceObject.Hidden = value;
            }
        }
        ////ScriptedCollision           = 0x00008000,
        public bool ScriptedCollision
        {
            get => PhysicsState.HasFlag(PhysicsState.ScriptedCollision);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.ScriptedCollision;
                else
                    PhysicsState &= ~PhysicsState.ScriptedCollision;
                AceObject.ScriptedCollision = value;
            }
        }
        ////HasPhysicsBsp               = 0x00010000,
        public bool HasPhysicsBsp
        {
            get => PhysicsState.HasFlag(PhysicsState.HasPhysicsBsp);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.HasPhysicsBsp;
                else
                    PhysicsState &= ~PhysicsState.HasPhysicsBsp;
                ////AceObject.HasPhysicsBsp = value;
            }
        }
        ////Inelastic                   = 0x00020000,
        public bool Inelastic
        {
            get => PhysicsState.HasFlag(PhysicsState.Inelastic);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Inelastic;
                else
                    PhysicsState &= ~PhysicsState.Inelastic;
                AceObject.Inelastic = value;
            }
        }
        ////HasDefaultAnim              = 0x00040000,
        public bool HasDefaultAnim
        {
            get => PhysicsState.HasFlag(PhysicsState.HasDefaultAnim);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.HasDefaultAnim;
                else
                    PhysicsState &= ~PhysicsState.HasDefaultAnim;
                ////AceObject.HasDefaultAnim = value;
            }
        }
        ////HasDefaultScript            = 0x00080000,
        public bool HasDefaultScript
        {
            get => PhysicsState.HasFlag(PhysicsState.HasDefaultScript);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.HasDefaultScript;
                else
                    PhysicsState &= ~PhysicsState.HasDefaultScript;
                ////AceObject.HasDefaultScript = value;
            }
        }
        ////Cloaked                     = 0x00100000,
        public bool Cloaked
        {
            get => PhysicsState.HasFlag(PhysicsState.Cloaked);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Cloaked;
                else
                    PhysicsState &= ~PhysicsState.Cloaked;
                ////AceObject.Cloaked = value;
            }
        }
        ////ReportCollisionAsEnviroment = 0x00200000,
        public bool ReportCollisionAsEnviroment
        {
            get => PhysicsState.HasFlag(PhysicsState.ReportCollisionAsEnviroment);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.ReportCollisionAsEnviroment;
                else
                    PhysicsState &= ~PhysicsState.ReportCollisionAsEnviroment;
                AceObject.ReportCollisionsAsEnvironment = value;
            }
        }
        ////EdgeSlide                   = 0x00400000,
        public bool EdgeSlide
        {
            get => PhysicsState.HasFlag(PhysicsState.EdgeSlide);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.EdgeSlide;
                else
                    PhysicsState &= ~PhysicsState.EdgeSlide;
                AceObject.AllowEdgeSlide = value;
            }
        }
        ////Sledding                    = 0x00800000,
        public bool Sledding
        {
            get => PhysicsState.HasFlag(PhysicsState.Sledding);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Sledding;
                else
                    PhysicsState &= ~PhysicsState.Sledding;
                ////AceObject.Sledding = value;
            }
        }
        ////Frozen                      = 0x01000000,
        public bool Frozen
        {
            get => PhysicsState.HasFlag(PhysicsState.Frozen);
            set
            {
                if (value)
                    PhysicsState |= PhysicsState.Frozen;
                else
                    PhysicsState &= ~PhysicsState.Frozen;
                AceObject.IsFrozen = value;
            }
        }

        ////public bool? Visibility
        ////{
        ////    get { return AceObject.Visibility; }
        ////    set { AceObject.Visibility = value; }
        ////}
        #endregion

        public WeenieType WeenieType
        {
            get => (WeenieType?)AceObject.WeenieType ?? WeenieType.Undef;
            protected set { AceObject.WeenieType = (int)value; }
        }

        public IActor CurrentParent { get; private set; }

        public Position ForcedLocation { get; private set; }

        public Position RequestedLocation { get; private set; }

        /// <summary>
        /// Should only be adjusted by LandblockManager -- default is null
        /// </summary>
        public Landblock CurrentLandblock => CurrentParent as Landblock;

        /// <summary>
        /// tick-stamp for the last time this object changed in any way.
        /// </summary>
        public double LastUpdatedTicks { get; set; }

        /// <summary>
        /// Time when this object will despawn, -1 is never.
        /// </summary>
        public double DespawnTime { get; set; } = -1;

        private readonly NestedActionQueue actionQueue = new NestedActionQueue();

        /// <summary>
        /// tick-stamp for the last time a movement update was sent
        /// </summary>
        public double LastMovementBroadcastTicks { get; set; }

        /// <summary>
        /// tick-stamp for the server time of the last time the player moved.
        /// TODO: implement
        /// </summary>
        public double LastAnimatedTicks { get; set; }

        public virtual void PlayScript(Session session) { }


        ////// Logical Game Data
        public ContainerType ContainerType
        {
            get
            {
                if (ItemCapacity != null && ItemCapacity != 0)
                    return ContainerType.Container;
                if (Name.Contains("Foci"))
                    return ContainerType.Foci;
                return ContainerType.NonContainer;
            }
        }

        public CombatStyle? DefaultCombatStyle
        {
            get => (CombatStyle?)AceObject.DefaultCombatStyle;
            set { AceObject.DefaultCombatStyle = (int?)value; }
        }

        public uint? GeneratorId
        {
            get => AceObject.GeneratorIID;
            set { AceObject.GeneratorIID = value; }
        }

        public uint? ClothingBase
        {
            get => AceObject.ClothingBaseDID;
            set { AceObject.ClothingBaseDID = value; }
        }

        public int? ItemCurMana
        {
            get => AceObject.ItemCurMana;
            set { AceObject.ItemCurMana = value; }
        }

        public int? ItemMaxMana
        {
            get => AceObject.ItemMaxMana;
            set { AceObject.ItemMaxMana = value; }
        }

        public bool? NpcLooksLikeObject
        {
            get => AceObject.NpcLooksLikeObject;
            set { AceObject.NpcLooksLikeObject = value; }
        }

        public bool? SuppressGenerateEffect
        {
            get => AceObject.SuppressGenerateEffect;
            set { AceObject.SuppressGenerateEffect = value; }
        }

        public CreatureType? CreatureType
        {
            get => (CreatureType?)AceObject.CreatureType;
            set { AceObject.CreatureType = (int)value; }
        }

        public SetupModel CSetup => DatManager.PortalDat.ReadFromDat<SetupModel>(SetupTableId.Value);

        /// <summary>
        /// This is used to determine how close you need to be to use an item.
        /// NOTE: cheat factor added for items with null use radius.   Og II
        /// </summary>
        public float UseRadiusSquared => ((UseRadius ?? 2) + CSetup.Radius) * ((UseRadius ?? 2) + CSetup.Radius);

        public bool IsWithinUseRadiusOf(WorldObject wo)
        {
            if (Location.SquaredDistanceTo(wo.Location) >= wo.UseRadiusSquared)
                return false;
            return true;
        }

        public string LongDesc
        {
            get => AceObject.LongDesc;
            set { AceObject.LongDesc = value; }
        }

        public string Use
        {
            get => AceObject.Use;
            set { AceObject.Use = value; }
        }

        public string Inscription
        {
            get => AceObject.Inscription;
            set { AceObject.Inscription = value; }
        }

        public string ScribeAccount
        {
            get => AceObject.ScribeAccount;
            set { AceObject.ScribeAccount = value; }
        }

        public string ScribeName
        {
            get => AceObject.ScribeName;
            set { AceObject.ScribeName = value; }
        }

        public int? Boost
        {
            get => AceObject.Boost;
            set { AceObject.Boost = value; }
        }

        public uint? SpellDID
        {
            get => AceObject.SpellDID ?? null;
            set { AceObject.SpellDID = value; }
        }

        public int? BoostEnum
        {
            get => AceObject.BoostEnum ?? 0;
            set { AceObject.BoostEnum = value; }
        }

        public double? HealkitMod
        {
            get => AceObject.HealkitMod;
            set { AceObject.HealkitMod = value; }
        }

        public virtual int? CoinValue
        {
            get => AceObject.CoinValue;
            set { AceObject.CoinValue = value; }
        }


        internal void SetInventoryForVendor(WorldObject inventoryItem)
        {
            inventoryItem.Location = null;
            inventoryItem.PositionFlag = UpdatePositionFlag.None;
            inventoryItem.ContainerId = null;
            inventoryItem.PlacementPosition = null;
            inventoryItem.WielderId = null;
            inventoryItem.CurrentWieldedLocation = null;
            // TODO: create enum for this once we understand this better.
            // This is needed to make items lay flat on the ground.
            inventoryItem.Placement = global::ACE.Entity.Enum.Placement.Resting;
        }

        internal void SetInventoryForWorld(WorldObject inventoryItem)
        {
            inventoryItem.Location = Location.InFrontOf(1.1f);
            inventoryItem.PositionFlag = UpdatePositionFlag.Contact
                                         | UpdatePositionFlag.Placement
                                         | UpdatePositionFlag.ZeroQy
                                         | UpdatePositionFlag.ZeroQx;

            inventoryItem.ContainerId = null;
            inventoryItem.PlacementPosition = null;
            inventoryItem.WielderId = null;
            inventoryItem.CurrentWieldedLocation = null;
            // TODO: create enum for this once we understand this better.
            // This is needed to make items lay flat on the ground.
            inventoryItem.Placement = global::ACE.Entity.Enum.Placement.Resting;
        }

        internal void SetInventoryForContainer(WorldObject inventoryItem, int placement)
        {
            if (inventoryItem.Location != null)
                LandblockManager.RemoveObject(inventoryItem);
            inventoryItem.PositionFlag = UpdatePositionFlag.None;
            // TODO: Create enums for this.
            inventoryItem.Placement = global::ACE.Entity.Enum.Placement.RightHandCombat; // FIXME: Is this right? Should this be Default or Resting instead?
            inventoryItem.PlacementPosition = placement;
            inventoryItem.Location = null;
            inventoryItem.ParentLocation = null;
            inventoryItem.CurrentWieldedLocation = null;
            inventoryItem.WielderId = null;
        }

        public void Examine(Session examiner)
        {
            // TODO : calculate if we were successful
            bool successfulId = true;
            GameEventIdentifyObjectResponse identifyResponse = new GameEventIdentifyObjectResponse(examiner, this, successfulId);
            examiner.Network.EnqueueSend(identifyResponse);

#if DEBUG
            examiner.Network.EnqueueSend(new GameMessageSystemChat("", ChatMessageType.System));
            examiner.Network.EnqueueSend(new GameMessageSystemChat($"{DebugOutputString(GetType(), this)}", ChatMessageType.System));
#endif
        }

        public void ReadBookPage(Session reader, uint pageNum)
        {
            PageData pageData = new PageData();
            AceObjectPropertiesBook bookPage = PropertiesBook[pageNum];

            pageData.AuthorID = bookPage.AuthorId;
            pageData.AuthorName = bookPage.AuthorName;
            pageData.AuthorAccount = bookPage.AuthorAccount;
            pageData.PageIdx = pageNum;
            pageData.PageText = bookPage.PageText;
            pageData.IgnoreAuthor = false;
            // TODO - check for PropertyBool.IgnoreAuthor flag

            var bookDataResponse = new GameEventBookPageDataResponse(reader, Guid.Full, pageData);
            reader.Network.EnqueueSend(bookDataResponse);
        }

        public virtual void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            // Excluding some times that are sent later as weapon status Og II
            var propertiesInt = PropertiesInt.Where(x => x.PropertyId < 9000
                                                          && x.PropertyId != (uint)PropertyInt.Damage
                                                          && x.PropertyId != (uint)PropertyInt.DamageType
                                                          && x.PropertyId != (uint)PropertyInt.WeaponSkill
                                                          && x.PropertyId != (uint)PropertyInt.WeaponTime).ToList();

            if (propertiesInt.Count > 0)
            {
                flags |= IdentifyResponseFlags.IntStatsTable;
            }

            var propertiesInt64 = PropertiesInt64.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesInt64.Count > 0)
            {
                flags |= IdentifyResponseFlags.Int64StatsTable;
            }

            var propertiesBool = PropertiesBool.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesBool.Count > 0)
            {
                flags |= IdentifyResponseFlags.BoolStatsTable;
            }

            // the float values 13 - 19 + 165 (nether added way later) are armor resistance and is shown in a different list. Og II
            // 21-22, 26, 62-63 are all sent as part of the weapons profile and not duplicated.
            var propertiesDouble = PropertiesDouble.Where(x => x.PropertyId < 9000
                                                               && (x.PropertyId < (uint)PropertyDouble.ArmorModVsSlash
                                                               || x.PropertyId > (uint)PropertyDouble.ArmorModVsElectric)
                                                               && x.PropertyId != (uint)PropertyDouble.WeaponLength
                                                               && x.PropertyId != (uint)PropertyDouble.DamageVariance
                                                               && x.PropertyId != (uint)PropertyDouble.MaximumVelocity
                                                               && x.PropertyId != (uint)PropertyDouble.WeaponOffense
                                                               && x.PropertyId != (uint)PropertyDouble.DamageMod
                                                               && x.PropertyId != (uint)PropertyDouble.ArmorModVsNether).ToList();
            if (propertiesDouble.Count > 0)
            {
                flags |= IdentifyResponseFlags.FloatStatsTable;
            }

            var propertiesDid = PropertiesDid.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesDid.Count > 0)
            {
                flags |= IdentifyResponseFlags.DidStatsTable;
            }

            var propertiesString = PropertiesString.Where(x => x.PropertyId < 9000).ToList();

            var propertiesSpellId = PropertiesSpellId.ToList();

            if (propertiesSpellId.Count > 0)
            {
                flags |= IdentifyResponseFlags.SpellBook;
            }

            // TODO: Move to Armor class
            var propertiesArmor = PropertiesDouble.Where(x => (x.PropertyId < 9000
                                                         && (x.PropertyId >= (uint)PropertyDouble.ArmorModVsSlash
                                                         && x.PropertyId <= (uint)PropertyDouble.ArmorModVsElectric))
                                                         || x.PropertyId == (uint)PropertyDouble.ArmorModVsNether).ToList();
            if (propertiesArmor.Count > 0)
            {
                flags |= IdentifyResponseFlags.ArmorProfile;
            }

            // TODO: Move to Weapon class
            // Weapons Profile
            var propertiesWeaponsD = PropertiesDouble.Where(x => x.PropertyId < 9000
                                                            && (x.PropertyId == (uint)PropertyDouble.WeaponLength
                                                            || x.PropertyId == (uint)PropertyDouble.DamageVariance
                                                            || x.PropertyId == (uint)PropertyDouble.MaximumVelocity
                                                            || x.PropertyId == (uint)PropertyDouble.WeaponOffense
                                                            || x.PropertyId == (uint)PropertyDouble.DamageMod)).ToList();

            var propertiesWeaponsI = PropertiesInt.Where(x => x.PropertyId < 9000
                                                         && (x.PropertyId == (uint)PropertyInt.Damage
                                                         || x.PropertyId == (uint)PropertyInt.DamageType
                                                         || x.PropertyId == (uint)PropertyInt.WeaponSkill
                                                         || x.PropertyId == (uint)PropertyInt.WeaponTime)).ToList();

            if (propertiesWeaponsI.Count + propertiesWeaponsD.Count > 0)
            {
                flags |= IdentifyResponseFlags.WeaponProfile;
            }

            if (propertiesString.Count > 0)
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
            }

            // Ok Down to business - let's identify all of this stuff.
            WriteIdentifyObjectHeader(writer, flags, success);
            WriteIdentifyObjectIntProperties(writer, flags, propertiesInt);
            WriteIdentifyObjectInt64Properties(writer, flags, propertiesInt64);
            WriteIdentifyObjectBoolProperties(writer, flags, propertiesBool);
            WriteIdentifyObjectDoubleProperties(writer, flags, propertiesDouble);
            WriteIdentifyObjectStringsProperties(writer, flags, propertiesString);
            WriteIdentifyObjectDidProperties(writer, flags, propertiesDid);
            WriteIdentifyObjectSpellIdProperties(writer, flags, propertiesSpellId);

            // TODO: Move to Armor class
            WriteIdentifyObjectArmorProfile(writer, flags, propertiesArmor);

            // TODO: Move to Weapon class
            WriteIdentifyObjectWeaponsProfile(writer, flags, propertiesWeaponsD, propertiesWeaponsI);
        }

        private string DebugOutputString(Type type, WorldObject obj)
        {
            string debugOutput = "ACE Debug Output:\n";
            debugOutput += "ACE Class File: " + type.Name + ".cs" + "\n";
            debugOutput += "AceObjectId: " + obj.Guid.Full + " (0x" + obj.Guid.Full.ToString("X") + ")" + "\n";

            debugOutput += "-Private Fields-\n";
            foreach (var prop in obj.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                if (prop.GetValue(obj) == null)
                    continue;

                debugOutput += $"{prop.Name.Replace("<", "").Replace(">k__BackingField", "")} = {prop.GetValue(obj)}" + "\n";
            }

            debugOutput += "-Public Properties-\n";
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.GetValue(obj, null) == null)
                    continue;

                switch (prop.Name.ToLower())
                {
                    case "guid":
                        debugOutput += $"{prop.Name} = {obj.Guid.Full} (GuidType.{obj.Guid.Type.ToString()})" + "\n";
                        break;
                    case "descriptionflags":
                        debugOutput += $"{prop.Name} = {obj.DescriptionFlags.ToString()}" + " (" + (uint)obj.DescriptionFlags + ")" + "\n";
                        break;
                    case "weenieflags":
                        debugOutput += $"{prop.Name} = {obj.WeenieFlags.ToString()}" + " (" + (uint)obj.WeenieFlags + ")" + "\n";
                        break;
                    case "weenieflags2":
                        debugOutput += $"{prop.Name} = {obj.WeenieFlags2.ToString()}" + " (" + (uint)obj.WeenieFlags2 + ")" + "\n";
                        break;
                    case "positionflag":
                        debugOutput += $"{prop.Name} = {obj.PositionFlag.ToString()}" + " (" + (uint)obj.PositionFlag + ")" + "\n";
                        break;
                    case "itemtype":
                        debugOutput += $"{prop.Name} = {obj.ItemType.ToString()}" + " (" + (uint)obj.ItemType + ")" + "\n";
                        break;
                    case "creaturetype":
                        debugOutput += $"{prop.Name} = {obj.CreatureType.ToString()}" + " (" + (uint)obj.CreatureType + ")" + "\n";
                        break;
                    case "containertype":
                        debugOutput += $"{prop.Name} = {obj.ContainerType.ToString()}" + " (" + (uint)obj.ContainerType + ")" + "\n";
                        break;
                    case "usable":
                        debugOutput += $"{prop.Name} = {obj.Usable.ToString()}" + " (" + (uint)obj.Usable + ")" + "\n";
                        break;
                    case "radarbehavior":
                        debugOutput += $"{prop.Name} = {obj.RadarBehavior.ToString()}" + " (" + (uint)obj.RadarBehavior + ")" + "\n";
                        break;
                    case "physicsdescriptionflag":
                        debugOutput += $"{prop.Name} = {obj.PhysicsDescriptionFlag.ToString()}" + " (" + (uint)obj.PhysicsDescriptionFlag + ")" + "\n";
                        break;
                    case "physicsstate":
                        debugOutput += $"{prop.Name} = {obj.PhysicsState.ToString()}" + " (" + (uint)obj.PhysicsState + ")" + "\n";
                        break;
                    case "propertiesint":
                        foreach (var item in obj.PropertiesInt)
                        {
                            debugOutput += $"PropertyInt.{System.Enum.GetName(typeof(PropertyInt), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesint64":
                        foreach (var item in obj.PropertiesInt64)
                        {
                            debugOutput += $"PropertyInt64.{System.Enum.GetName(typeof(PropertyInt64), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesbool":
                        foreach (var item in obj.PropertiesBool)
                        {
                            debugOutput += $"PropertyBool.{System.Enum.GetName(typeof(PropertyBool), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesstring":
                        foreach (var item in obj.PropertiesString)
                        {
                            debugOutput += $"PropertyString.{System.Enum.GetName(typeof(PropertyString), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesdouble":
                        foreach (var item in obj.PropertiesDouble)
                        {
                            debugOutput += $"PropertyDouble.{System.Enum.GetName(typeof(PropertyDouble), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesdid":
                        foreach (var item in obj.PropertiesDid)
                        {
                            debugOutput += $"PropertyDataId.{System.Enum.GetName(typeof(PropertyDataId), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesiid":
                        foreach (var item in obj.PropertiesIid)
                        {
                            debugOutput += $"PropertyInstanceId.{System.Enum.GetName(typeof(PropertyInstanceId), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesspellid":
                        foreach (var item in obj.PropertiesSpellId)
                        {
                            debugOutput += $"PropertySpellId.{System.Enum.GetName(typeof(Spell), item.SpellId)} ({item.SpellId})" + "\n";
                        }
                        break;
                    case "validlocations":
                        debugOutput += $"{prop.Name} = {obj.ValidLocations}" + " (" + (uint)obj.ValidLocations + ")" + "\n";
                        break;
                    case "currentwieldedlocation":
                        debugOutput += $"{prop.Name} = {obj.CurrentWieldedLocation}" + " (" + (uint)obj.CurrentWieldedLocation + ")" + "\n";
                        break;
                    case "priority":
                        debugOutput += $"{prop.Name} = {obj.Priority}" + " (" + (uint)obj.Priority + ")" + "\n";
                        break;
                    case "radarcolor":
                        debugOutput += $"{prop.Name} = {obj.RadarColor}" + " (" + (uint)obj.RadarColor + ")" + "\n";
                        break;
                    case "location":
                        debugOutput += $"{prop.Name} = {obj.Location.ToLOCString()}" + "\n";
                        break;
                    default:
                        debugOutput += $"{prop.Name} = {prop.GetValue(obj, null)}" + "\n";
                        break;
                }
            }

            return debugOutput;
        }

        protected static void WriteIdentifyObjectHeader(BinaryWriter writer, IdentifyResponseFlags flags, bool success)
        {
            writer.Write((uint)flags); // Flags
            writer.Write(Convert.ToUInt32(success)); // Success bool
        }

        protected static void WriteIdentifyObjectIntProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesInt> propertiesInt)
        {
            const ushort tableSize = 16;
            var notNull = propertiesInt.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.IntStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesInt x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        protected static void WriteIdentifyObjectProperties(BinaryWriter writer, IdentifyResponseFlags flags, Dictionary<PropertyInt, int> properties)
        {
            const ushort tableSize = 16;

            if ((flags & IdentifyResponseFlags.IntStatsTable) == 0 || (properties.Count == 0))
                return;

            writer.Write((ushort)properties.Count);
            writer.Write(tableSize);

            foreach (var property in properties)
            {
                writer.Write((uint)property.Key);
                writer.Write(property.Value);
            }
        }

        protected static void WriteIdentifyObjectInt64Properties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesInt64> propertiesInt64)
        {
            const ushort tableSize = 8;
            var notNull = propertiesInt64.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.Int64StatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesInt64 x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        protected static void WriteIdentifyObjectBoolProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesBool> propertiesBool)
        {
            const ushort tableSize = 8;
            var notNull = propertiesBool.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.BoolStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesBool x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(Convert.ToUInt32(x.PropertyValue.Value));
            }
        }

        protected static void WriteIdentifyObjectDoubleProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesDouble)
        {
            const ushort tableSize = 8;
            var notNull = propertiesDouble.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.FloatStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesDouble x in notNull)
            {
                writer.Write((uint)x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        protected static void WriteIdentifyObjectStringsProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesString> propertiesStrings)
        {
            const ushort tableSize = 8;
            var notNull = propertiesStrings.Where(p => !string.IsNullOrWhiteSpace(p.PropertyValue)).ToList();
            if ((flags & IdentifyResponseFlags.StringStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesString x in notNull)
            {
                writer.Write((uint)x.PropertyId);
                writer.WriteString16L(x.PropertyValue);
            }
        }

        protected static void WriteIdentifyObjectDidProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDataId> propertiesDid)
        {
            const ushort tableSize = 16;
            var notNull = propertiesDid.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.DidStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesDataId x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        protected static void WriteIdentifyObjectSpellIdProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesSpell> propertiesSpellId)
        {
            if ((flags & IdentifyResponseFlags.SpellBook) == 0 || (propertiesSpellId.Count == 0)) return;
            writer.Write((uint)propertiesSpellId.Count);

            foreach (AceObjectPropertiesSpell x in propertiesSpellId)
            {
                writer.Write(x.SpellId);
            }
        }

        // TODO: Move to Armor class
        protected static void WriteIdentifyObjectArmorProfile(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesArmor)
        {
            var notNull = propertiesArmor.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.ArmorProfile) == 0 || (notNull.Count == 0)) return;

            foreach (AceObjectPropertiesDouble x in notNull)
            {
                writer.Write((float)x.PropertyValue.Value);
            }
        }

        // TODO: Move to Weapon class
        protected static void WriteIdentifyObjectWeaponsProfile(
            BinaryWriter writer,
            IdentifyResponseFlags flags,
            List<AceObjectPropertiesDouble> propertiesWeaponsD,
            List<AceObjectPropertiesInt> propertiesWeaponsI)
        {
            if ((flags & IdentifyResponseFlags.WeaponProfile) == 0) return;
            writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.DamageType)?.PropertyValue ?? 0);
            // Signed
            writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.WeaponTime)?.PropertyValue ?? 0);
            writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.WeaponSkill)?.PropertyValue ?? 0);
            // Signed
            writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.Damage)?.PropertyValue ?? 0);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.DamageVariance)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.DamageMod)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.WeaponLength)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.MaximumVelocity)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.WeaponOffense)?.PropertyValue ?? 0.00);
            // This one looks to be 0 - I did not find one with this calculated.   It is called Max Velocity Calculated
            writer.Write(0u);
        }

        public void QueryHealth(Session examiner)
        {
            float healthPercentage = 1f;

            if (Guid.IsPlayer())
            {
                Player tmpTarget = (Player)this;
                healthPercentage = (float)tmpTarget.Health.Current / (float)tmpTarget.Health.MaxValue;
            }
            else// if (Guid.IsCreature())
            {
                throw new NotImplementedException(); // We can't use the GUID to see if this is a creature, we need another way
                Creature tmpTarget = (Creature)this;
                healthPercentage = (float)tmpTarget.Health.Current / (float)tmpTarget.Health.MaxValue;
            }

            var updateHealth = new GameEventUpdateHealth(examiner, Guid.Full, healthPercentage);
            examiner.Network.EnqueueSend(updateHealth);
        }

        public void QueryItemMana(Session examiner)
        {
            float manaPercentage = 1f;
            uint success = 0;

            if (ItemCurMana != null && ItemMaxMana != null)
            {
                manaPercentage = (float)ItemCurMana / (float)ItemMaxMana;
                success = 1;
            }

            if (success == 0) // according to retail PCAPs, if success = 0, mana = 0.
                manaPercentage = 0;

            var updateMana = new GameEventQueryItemManaResponse(examiner, Guid.Full, manaPercentage, success);
            examiner.Network.EnqueueSend(updateMana);
        }

        public virtual void SerializeUpdateObject(BinaryWriter writer)
        {
            // content of these 2 is the same? TODO: Validate that?
            SerializeCreateObject(writer);
        }

        // This fully replaces the PhysicsState of the WO, use sparingly?
        public void SetPhysicsState(PhysicsState state, bool packet = true)
        {
            PhysicsState = state;

            if (packet)
            {
                EnqueueBroadcastPhysicsState();
            }
        }

        public void EnqueueBroadcastPhysicsState()
        {
            if (CurrentLandblock != null)
            {
                GameMessage msg = new GameMessageSetState(this, PhysicsState);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, msg);
            }
        }

        public void EnqueueBroadcastUpdateObject()
        {
            if (CurrentLandblock != null)
            {
                GameMessage msg = new GameMessageUpdateObject(this);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, msg);
            }
        }

        private WeenieHeaderFlag SetWeenieHeaderFlag()
        {
            WeenieHeaderFlag weenieHeaderFlag = WeenieHeaderFlag.None;
            if (NamePlural != null)
                weenieHeaderFlag |= WeenieHeaderFlag.PluralName;

            if (ItemCapacity != null)
                weenieHeaderFlag |= WeenieHeaderFlag.ItemsCapacity;

            if (ContainerCapacity != null)
                weenieHeaderFlag |= WeenieHeaderFlag.ContainersCapacity;

            if (AmmoType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.AmmoType;

            if (Value != null && (Value > 0))
                weenieHeaderFlag |= WeenieHeaderFlag.Value;

            if (Usable != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Usable;

            if (UseRadius != null)
                weenieHeaderFlag |= WeenieHeaderFlag.UseRadius;

            if (TargetType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.TargetType;

            if (UiEffects != null)
                weenieHeaderFlag |= WeenieHeaderFlag.UiEffects;

            if (CombatUse != null)
                weenieHeaderFlag |= WeenieHeaderFlag.CombatUse;

            if (Structure != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Structure;

            if (MaxStructure != null)
                weenieHeaderFlag |= WeenieHeaderFlag.MaxStructure;

            if (StackSize != null)
                weenieHeaderFlag |= WeenieHeaderFlag.StackSize;

            if (MaxStackSize != null)
                weenieHeaderFlag |= WeenieHeaderFlag.MaxStackSize;

            if (ContainerId != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Container;

            if (WielderId != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Wielder;

            if (ValidLocations != null)
                weenieHeaderFlag |= WeenieHeaderFlag.ValidLocations;

            // You can't be in a wielded location if you don't have a wielder.   This is a gurad against crap data. Og II
            if ((CurrentWieldedLocation != null) && (CurrentWieldedLocation != 0) && (WielderId != null) && (WielderId != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.CurrentlyWieldedLocation;

            if (Priority != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Priority;

            if (RadarColor != null)
                weenieHeaderFlag |= WeenieHeaderFlag.RadarBlipColor;

            if (RadarBehavior != null)
                weenieHeaderFlag |= WeenieHeaderFlag.RadarBehavior;

            if ((Script != null) && (Script != 0u))
                weenieHeaderFlag |= WeenieHeaderFlag.PScript;

            if ((Workmanship != null) && (uint?)Workmanship != 0u)
                weenieHeaderFlag |= WeenieHeaderFlag.Workmanship;

            if (Burden != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Burden;

            if ((Spell != null) && (Spell != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.Spell;

            if (HouseOwner != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HouseOwner;

            if (HouseRestrictions != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HouseRestrictions;

            if (HookItemType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HookItemTypes;

            if (Monarch != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Monarch;

            if (HookType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HookType;

            if ((IconOverlayId != null) && (IconOverlayId != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.IconOverlay;

            if (MaterialType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.MaterialType;

            SetWeenieHeaderFlag2();

            return weenieHeaderFlag;
        }

        private WeenieHeaderFlag2 SetWeenieHeaderFlag2()
        {
            var weenieHeaderFlag2 = WeenieHeaderFlag2.None;

            if ((IconUnderlayId != null) && (IconUnderlayId != 0))
                weenieHeaderFlag2 |= WeenieHeaderFlag2.IconUnderlay;

            if ((CooldownId != null) && (CooldownId != 0))
                weenieHeaderFlag2 |= WeenieHeaderFlag2.Cooldown;

            if ((CooldownDuration != null) && Math.Abs((float)CooldownDuration) >= 0.001)
                weenieHeaderFlag2 |= WeenieHeaderFlag2.CooldownDuration;

            if ((PetOwner != null) && (PetOwner != 0))
                weenieHeaderFlag2 |= WeenieHeaderFlag2.PetOwner;

            return weenieHeaderFlag2;
        }

        public virtual void SendPartialUpdates(Session targetSession, List<AceObjectPropertyId> properties)
        {
            foreach (var property in properties)
            {
                switch (property.PropertyType)
                {
                    case AceObjectPropertyType.PropertyInt:
                        int? value = this.AceObject.GetProperty((PropertyInt)property.PropertyId);
                        if (value != null)
                            targetSession.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(targetSession.Player.Sequences, (PropertyInt)property.PropertyId, value.Value));
                        break;
                    default:
                        log.Debug($"Unsupported property in SendPartialUpdates: id {property.PropertyId}, type {property.PropertyType}.");
                        break;
                }
            }
        }

        public virtual void SerializeCreateObject(BinaryWriter writer)
        {
            SerializeCreateObject(writer, false);
        }

        public virtual void SerializeGameDataOnly(BinaryWriter writer)
        {
            SerializeCreateObject(writer, true);
        }

        public virtual void SerializeCreateObject(BinaryWriter writer, bool gamedataonly)
        {
            writer.WriteGuid(Guid);
            if (!gamedataonly)
            {
                SerializeModelData(writer);
                SerializePhysicsData(writer);
            }
            writer.Write((uint)WeenieFlags);
            writer.WriteString16L(Name);
            writer.WritePackedDword(WeenieClassId);
            writer.WritePackedDwordOfKnownType(IconId ?? 0, 0x6000000);
            writer.Write((uint)ItemType);
            writer.Write((uint)DescriptionFlags);
            writer.Align();

            if ((DescriptionFlags & ObjectDescriptionFlag.IncludesSecondHeader) != 0)
                writer.Write((uint)WeenieFlags2);

            if ((WeenieFlags & WeenieHeaderFlag.PluralName) != 0)
                writer.WriteString16L(NamePlural);

            if ((WeenieFlags & WeenieHeaderFlag.ItemsCapacity) != 0)
                writer.Write(ItemCapacity ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.ContainersCapacity) != 0)
                writer.Write(ContainerCapacity ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort?)AmmoType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write(Value ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Usable) != 0)
                writer.Write((uint?)Usable ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(UseRadius ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write(TargetType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write((uint?)UiEffects ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((sbyte?)CombatUse ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Structure) != 0)
                writer.Write(Structure ?? (ushort)0);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write(MaxStructure ?? (ushort)0);

            if ((WeenieFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write(StackSize ?? (ushort)0);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write(MaxStackSize ?? (ushort)0);

            if ((WeenieFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(ContainerId ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(WielderId ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.ValidLocations) != 0)
                writer.Write((uint?)ValidLocations ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.CurrentlyWieldedLocation) != 0)
                writer.Write((uint?)CurrentWieldedLocation ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Priority) != 0)
                writer.Write((uint?)Priority ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.RadarBlipColor) != 0)
                writer.Write((byte?)RadarColor ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.RadarBehavior) != 0)
                writer.Write((byte?)RadarBehavior ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.PScript) != 0)
                writer.Write(Script ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write(Workmanship ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write(Burden ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((ushort?)Spell ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(HouseOwner ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
                writer.Write(HouseRestrictions ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write(HookItemType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(Monarch ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HookType) != 0)
                writer.Write(HookType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.WritePackedDwordOfKnownType((IconOverlayId ?? 0), 0x6000000);

            if ((WeenieFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.WritePackedDwordOfKnownType((IconUnderlayId ?? 0), 0x6000000);

            if ((WeenieFlags & WeenieHeaderFlag.MaterialType) != 0)
                writer.Write((uint)(MaterialType ?? 0u));

            if ((WeenieFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                writer.Write(CooldownId ?? 0);

            if ((WeenieFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                writer.Write((double?)CooldownDuration ?? 0u);

            if ((WeenieFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
                writer.Write(PetOwner ?? 0u);

            writer.Align();
        }

        /// <summary>
        /// This is the function used for the GameMessage.ObjDescEvent
        /// </summary>
        /// <param name="writer">Passed from the GameMessageEvent</param>
        public virtual void SerializeUpdateModelData(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);
            SerializeModelData(writer);
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            writer.Write(Sequences.GetNextSequence(SequenceType.ObjectPosition));
        }

        public void SerializeModelData(BinaryWriter writer)
        {
            writer.Write((byte)0x11);
            writer.Write((byte)modelPalettes.Count);
            writer.Write((byte)modelTextures.Count);
            writer.Write((byte)models.Count);

            if ((modelPalettes.Count > 0) && (PaletteBaseId != null))
                writer.WritePackedDwordOfKnownType((uint)PaletteBaseId, 0x4000000);
            foreach (var palette in modelPalettes)
            {
                writer.WritePackedDwordOfKnownType(palette.PaletteId, 0x4000000);
                writer.Write((byte)palette.Offset);
                writer.Write((byte)palette.Length);
            }

            foreach (var texture in modelTextures)
            {
                writer.Write((byte)texture.Index);
                writer.WritePackedDwordOfKnownType(texture.OldTexture, 0x5000000);
                writer.WritePackedDwordOfKnownType(texture.NewTexture, 0x5000000);
            }

            foreach (var model in models)
            {
                writer.Write((byte)model.Index);
                writer.WritePackedDwordOfKnownType(model.ModelID, 0x1000000);
            }

            writer.Align();
        }

        public void WriteUpdatePositionPayload(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);
            Location.Serialize(writer, PositionFlag, (int)(Placement ?? global::ACE.Entity.Enum.Placement.Default));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            writer.Write(Sequences.GetNextSequence(SequenceType.ObjectPosition));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectTeleport));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition));
        }

        /// <summary>
        /// Records where the client thinks we are, for use by physics engine later
        /// </summary>
        /// <param name="newPosition"></param>
        protected void PrepUpdatePosition(Position newPosition)
        {
            RequestedLocation = newPosition;
        }

        public void ClearRequestedPositions()
        {
            ForcedLocation = null;
            RequestedLocation = null;
        }

        /// <summary>
        /// Alerts clients of change in position
        /// </summary>
        protected virtual void SendUpdatePosition()
        {
            LastMovementBroadcastTicks = WorldManager.PortalYearTicks;
            GameMessage msg = new GameMessageUpdatePosition(this);
            if (CurrentLandblock != null)
            {
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, msg);
            }
        }

        /// <summary>
        /// Used by physics engine to actually update the entities position
        /// Automatically notifies clients of updated position
        /// </summary>
        /// <param name="newPosition"></param>
        public void PhysicsUpdatePosition(Position newPosition)
        {
            Location = newPosition;
            SendUpdatePosition();

            ForcedLocation = null;
            RequestedLocation = null;
        }

        /// <summary>
        /// Manages action/broadcast infrastructure
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(IActor parent)
        {
            CurrentParent = parent;
            actionQueue.RemoveParent();
            actionQueue.SetParent(parent);
        }

        /// <summary>
        /// Prepare new action to run on this object
        /// </summary>
        public LinkedListNode<IAction> EnqueueAction(IAction action)
        {
            return actionQueue.EnqueueAction(action);
        }

        /// <summary>
        /// Satisfies action interface
        /// </summary>
        /// <param name="node"></param>
        public void DequeueAction(LinkedListNode<IAction> node)
        {
            actionQueue.DequeueAction(node);
        }

        public AceObject SnapShotOfAceObject(bool clearDirtyFlags = false)
        {
            AceObject snapshot = (AceObject)AceObject.Clone();
            if (clearDirtyFlags)
                AceObject.ClearDirtyFlags();
            return snapshot;
        }

        public void InitializeAceObjectForSave()
        {
            AceObject.SetDirtyFlags();
        }

        /// <summary>
        /// Runs all actions pending on this WorldObject
        /// </summary>
        public void RunActions()
        {
            actionQueue.RunActions();
        }

        private PhysicsDescriptionFlag SetPhysicsDescriptionFlag()
        {
            var physicsDescriptionFlag = PhysicsDescriptionFlag.None;

            var movementData = CurrentMotionState?.GetPayload(Guid, Sequences);

            if (CurrentMotionState != null && movementData.Length > 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Movement;

            if (Placement != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;

            if (Location != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Position;

            if (MotionTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.MTable;

            if (SoundTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.STable;

            if (PhysicsTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.PeTable;

            if (SetupTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.CSetup;

            if (Children.Count != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Children;

            if (WielderId != null && ParentLocation != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Parent;

            if ((ObjScale != null) && (Math.Abs((float)ObjScale) >= 0.001))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.ObjScale;

            if (Friction != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Friction;

            if (Elasticity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Elasticity;

            if ((Translucency != null) && (Math.Abs((float)Translucency) >= 0.001))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Translucency;

            if (Velocity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Velocity;

            if (Acceleration != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Acceleration;

            if (Omega != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Omega;

            if (DefaultScriptId != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.DefaultScript;

            if (DefaultScriptIntensity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.DefaultScriptIntensity;

            return physicsDescriptionFlag;
        }

        // todo: return bytes of data for network write ? ?
        public void SerializePhysicsData(BinaryWriter writer)
        {
            writer.Write((uint)PhysicsDescriptionFlag);

            writer.Write((uint)PhysicsState);

            // PhysicsDescriptionFlag.Movement takes priorty over PhysicsDescription.FlagAnimationFrame
            // If both are set, only Movement is written.
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            {
                if (CurrentMotionState != null)
                {
                    var movementData = CurrentMotionState.GetPayload(Guid, Sequences);
                    if (movementData.Length > 0)
                    {
                        writer.Write((uint)movementData.Length); // May not need this cast from int to uint, but the protocol says uint Og II
                        writer.Write(movementData);
                        uint autonomous = CurrentMotionState.IsAutonomous ? (ushort)1 : (ushort)0;
                        writer.Write(autonomous);
                    }
                    else
                    {
                        // Adding these debug lines - don't think we can hit these, but want to make sure. Og II
                        log.Debug($"Our flag is set but we have no data length. {this.Guid.Full:X}");
                        writer.Write(0u);
                    }
                }
                else
                {
                    log.Debug($"Our flag is set but our current motion state is null. {this.Guid.Full:X}");
                    writer.Write(0u);
                }
            }
            else if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.AnimationFrame) != 0)
                writer.Write(((uint)(Placement ?? global::ACE.Entity.Enum.Placement.Default)));
            // TODO: Keep an eye on this, are we sure the client does not just ignore it?   I would think they way it reads by buffer length that this would blow up.
            // probably an edge case - just watch this - Og II

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Position) != 0)
                Location.Serialize(writer);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write(MotionTableId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.STable) != 0)
                writer.Write(SoundTableId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.PeTable) != 0)
                writer.Write(PhysicsTableId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(SetupTableId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Parent) != 0)
            {
                writer.Write(WielderId ?? 0u);
                writer.Write(ParentLocation ?? 0);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write(Children.Count);
                foreach (var child in Children)
                {
                    writer.Write(child.Guid);
                    writer.Write(child.LocationId);
                }
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.ObjScale) != 0)
                writer.Write(ObjScale ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(Friction ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Elasticity) != 0)
                writer.Write(Elasticity ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Translucency) != 0)
                writer.Write(Translucency ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Velocity) != 0)
            {
                Velocity.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                Acceleration.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Omega) != 0)
            {
                Omega.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScript) != 0)
                writer.Write(DefaultScriptId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                writer.Write(DefaultScriptIntensity ?? 0u);

            // timestamps
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectPosition));        // 0
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectMovement));        // 1
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectState));           // 2
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectVector));          // 3
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectTeleport));        // 4
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectServerControl));   // 5
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition));   // 6
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectVisualDesc));      // 7
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));        // 8

            writer.Align();
        }

        private void RecallAndSetObjectDescriptionBools()
        {
            // TODO: More uncommentting and wiring up for other flags
            ////None                   = 0x00000000,
            ////Openable               = 0x00000001,
            // if (AceObject.Openable ?? false)
            //    Openable = true;
            ////Inscribable            = 0x00000002,
            if (AceObject.Inscribable ?? false)
                Inscribable = true;
            ////Stuck                  = 0x00000004,
            if (AceObject.Stuck ?? false)
                Stuck = true;
            ////Player                 = 0x00000008,
            // if (AceObject.Player ?? false)
            //    Player = true;
            ////Attackable             = 0x00000010,
            if (AceObject.Attackable ?? false)
                Attackable = true;
            ////PlayerKiller           = 0x00000020,
            // if (AceObject.PlayerKiller ?? false)
            //    PlayerKiller = true;
            ////HiddenAdmin            = 0x00000040,
            if (AceObject.HiddenAdmin ?? false)
                HiddenAdmin = true;
            ////UiHidden               = 0x00000080,
            if (AceObject.UiHidden ?? false)
                UiHidden = true;
            ////Book                   = 0x00000100,
            // if (AceObject.Book ?? false)
            //    Book = true;
            ////Vendor                 = 0x00000200,
            // if (AceObject.Vendor ?? false)
            //    Vendor = true;
            ////PkSwitch               = 0x00000400,
            // if (AceObject.PkSwitch ?? false)
            //    PkSwitch = true;
            ////NpkSwitch              = 0x00000800,
            // if (AceObject.NpkSwitch ?? false)
            //    NpkSwitch = true;
            ////Door                   = 0x00001000,
            // if (AceObject.Door ?? false)
            //    Door = true;
            ////Corpse                 = 0x00002000,
            // if (AceObject.Corpse ?? false)
            //    Corpse = true;
            ////LifeStone              = 0x00004000,
            // if (AceObject.LifeStone ?? false)
            //    LifeStone = true;
            ////Food                   = 0x00008000,
            // if (AceObject.Food ?? false)
            //    Food = true;
            ////Healer                 = 0x00010000,
            // if (AceObject.Healer ?? false)
            //    Healer = true;
            ////Lockpick               = 0x00020000,
            // if (AceObject.Lockpick ?? false)
            //    Lockpick = true;
            ////Portal                 = 0x00040000,
            // if (AceObject.Portal ?? false)
            //    Portal = true;
            ////Admin                  = 0x00100000,
            // if (AceObject.Admin ?? false)
            //    Admin = true;
            ////FreePkStatus           = 0x00200000,
            // if (AceObject.FreePkStatus ?? false)
            //    FreePkStatus = true;
            ////ImmuneCellRestrictions = 0x00400000,
            if (AceObject.IgnoreHouseBarriers ?? false)
                ImmuneCellRestrictions = true;
            ////RequiresPackSlot       = 0x00800000,
            if (AceObject.RequiresBackpackSlot ?? false)
                RequiresPackSlot = true;
            ////Retained               = 0x01000000,
            if (AceObject.Retained ?? false)
                Retained = true;
            ////PkLiteStatus           = 0x02000000,
            // if (AceObject.PkLiteStatus ?? false)
            //    PkLiteStatus = true;
            ////IncludesSecondHeader   = 0x04000000,
            // if (AceObject.IncludesSecondHeader ?? false)
            //    IncludesSecondHeader = true;
            ////BindStone              = 0x08000000,
            // if (AceObject.BindStone ?? false)
            //    BindStone = true;
            ////VolatileRare           = 0x10000000,
            // if (AceObject.VolatileRare ?? false)
            //    VolatileRare = true;
            ////WieldOnUse             = 0x20000000,
            if (AceObject.WieldOnUse ?? false)
                WieldOnUse = true;
            ////WieldLeft              = 0x40000000,
            if (AceObject.AutowieldLeft ?? false)
                WieldLeft = true;
        }

        private void RecallAndSetPhysicsStateBools()
        {
            // TODO: More uncommentting and wiring up for other flags

            ////Static                      = 0x00000001,
            // if (AceObject.Static ?? false)
            //    Static = true;
            ////Unused1                     = 0x00000002,
            ////Ethereal                    = 0x00000004,
            if (AceObject.Ethereal ?? false)
                Ethereal = true;
            ////ReportCollision             = 0x00000008,
            if (AceObject.ReportCollisions ?? false)
                ReportCollision = true;
            ////IgnoreCollision             = 0x00000010,
            if (AceObject.IgnoreCollisions ?? false)
                IgnoreCollision = true;
            ////NoDraw                      = 0x00000020,
            if (AceObject.NoDraw ?? false)
                NoDraw = true;
            ////Missile                     = 0x00000040,
            // if (AceObject.Missile ?? false)
            //    Missile = true;
            ////Pushable                    = 0x00000080,
            // if (AceObject.Pushable ?? false)
            //    Pushable = true;
            ////AlignPath                   = 0x00000100,
            // if (AceObject.AlignPath ?? false)
            //    AlignPath = true;
            ////PathClipped                 = 0x00000200,
            // if (AceObject.PathClipped ?? false)
            //    PathClipped = true;
            ////Gravity                     = 0x00000400,
            if (AceObject.GravityStatus ?? false)
                Gravity = true;
            ////LightingOn                  = 0x00000800,
            if (AceObject.LightsStatus ?? false)
                LightingOn = true;
            ////ParticleEmitter             = 0x00001000,
            // if (AceObject.ParticleEmitter ?? false)
            //    ParticleEmitter = true;
            ////Unused2                     = 0x00002000,
            ////Hidden                      = 0x00004000,
            // if (AceObject.Hidden ?? false) // Probably PropertyBool.Visibility which would make me think if true, Hidden is false... Opposite of most other bools
            //    Hidden = true;
            ////ScriptedCollision           = 0x00008000,
            if (AceObject.ScriptedCollision ?? false)
                ScriptedCollision = true;
            ////HasPhysicsBsp               = 0x00010000,
            // if (AceObject.HasPhysicsBsp ?? false)
            //    HasPhysicsBsp = true;
            ////Inelastic                   = 0x00020000,
            if (AceObject.Inelastic ?? false)
                Inelastic = true;
            ////HasDefaultAnim              = 0x00040000,
            // if (AceObject.HasDefaultAnim ?? false)
            //    HasDefaultAnim = true;
            ////HasDefaultScript            = 0x00080000,
            // if (AceObject.HasDefaultScript ?? false) // Probably based on PhysicsDescriptionFlag
            //    HasDefaultScript = true;
            ////Cloaked                     = 0x00100000,
            // if (AceObject.Cloaked ?? false) // PropertyInt.CloakStatus probably plays in to this.
            //    Cloaked = true;
            ////ReportCollisionAsEnviroment = 0x00200000,
            if (AceObject.ReportCollisionsAsEnvironment ?? false)
                ReportCollisionAsEnviroment = true;
            ////EdgeSlide                   = 0x00400000,
            if (AceObject.AllowEdgeSlide ?? false)
                EdgeSlide = true;
            ////Sledding                    = 0x00800000,
            // if (AceObject.Sledding ?? false)
            //    Sledding = true;
            ////Frozen                      = 0x01000000,
            if (AceObject.IsFrozen ?? false)
                Frozen = true;
        }

        public virtual void ActOnUse(ObjectGuid playerId)
        {
            // Do Nothing by default
            if (CurrentLandblock != null)
            {
                Player player = CurrentLandblock.GetObject(playerId) as Player;
                if (player == null)
                {
                    return;
                }

#if DEBUG
                var errorMessage = new GameMessageSystemChat($"Default HandleActionOnUse reached, this object ({Name}) not programmed yet.", ChatMessageType.System);
                player.Session.Network.EnqueueSend(errorMessage);
#endif

                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            }
        }

        public virtual void OnUse(Session session)
        {
            // Do Nothing by default
#if DEBUG
            var errorMessage = new GameMessageSystemChat($"Default OnUse reached, this object ({Name}) not programmed yet.", ChatMessageType.System);
            session.Network.EnqueueSend(errorMessage);
#endif

            var sendUseDoneEvent = new GameEventUseDone(session);
            session.Network.EnqueueSend(sendUseDoneEvent);
        }

        public virtual void HandleActionOnCollide(ObjectGuid playerId)
        {
            // todo: implement.  default is probably to do nothing.
        }

        public int? ChessGamesLost
        {
            get => AceObject.ChessGamesLost;
            set { AceObject.ChessGamesLost = value; }
        }

        public int? ChessGamesWon
        {
            get => AceObject.ChessGamesWon;
            set { AceObject.ChessGamesWon = value; }
        }

        public int? ChessRank
        {
            get => AceObject.ChessRank;
            set { AceObject.ChessRank = value; }
        }

        public int? ChessTotalGames
        {
            get => AceObject.ChessTotalGames;
            set { AceObject.ChessTotalGames = value; }
        }

        public void HandleActionMotion(UniversalMotion motion)
        {
            if (CurrentLandblock != null)
            {
                DoMotion(motion);
            }
        }

        public void DoMotion(UniversalMotion motion)
        {
            CurrentLandblock.EnqueueBroadcastMotion(this, motion);
        }

        public void ApplyVisualEffects(PlayScript effect)
        {
            // new ActionChain(this, () => PlayParticleEffect(effect, Guid)).EnqueueChain();
            if (CurrentLandblock != null)
            {
                PlayParticleEffect(effect, Guid);
            }
        }

        // plays particle effect like spell casting or bleed etc..
        public void PlayParticleEffect(PlayScript effectId, ObjectGuid targetId)
        {
            if (CurrentLandblock != null)
            {
                var effectEvent = new GameMessageScript(targetId, effectId);
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, effectEvent);
            }
        }

        public List<AceObjectInventory> CreateList => AceObject.CreateList;

        public List<AceObjectInventory> WieldList
        {
            get { return CreateList.Where(x => x.DestinationType == (uint)DestinationType.Wield).ToList(); }
        }

        public List<AceObjectInventory> ShopList
        {
            get { return CreateList.Where(x => x.DestinationType == (uint)DestinationType.Shop).ToList(); }
        }

        public int? MerchandiseItemTypes
        {
            get => AceObject.MerchandiseItemTypes;
            set { AceObject.MerchandiseItemTypes = value; }
        }

        public int? MerchandiseMinValue
        {
            get => AceObject.MerchandiseMinValue;
            set { AceObject.MerchandiseMinValue = value; }
        }

        public int? MerchandiseMaxValue
        {
            get => AceObject.MerchandiseMaxValue;
            set { AceObject.MerchandiseMaxValue = value; }
        }

        public double? BuyPrice
        {
            get => AceObject.BuyPrice;
            set { AceObject.BuyPrice = (double)value; }
        }

        public double? SellPrice
        {
            get => AceObject.SellPrice;
            set { AceObject.SellPrice = (double)value; }
        }

        public bool? DealMagicalItems
        {
            get => AceObject.DealMagicalItems;
            set { AceObject.DealMagicalItems = value; }
        }

        public uint? AlternateCurrencyDID
        {
            get => AceObject.AlternateCurrencyDID;
            set { AceObject.AlternateCurrencyDID = value; }
        }

        public List<AceObjectGeneratorProfile> GeneratorProfiles => AceObject.GeneratorProfiles;

        public double? HeartbeatInterval
        {
            get => AceObject.HeartbeatInterval;
            set { AceObject.HeartbeatInterval = (double)value; }
        }

        public void EnterWorld()
        {
            if (Location != null)
            {
                LandblockManager.AddObject(this);
                if (SuppressGenerateEffect != true)
                    ApplyVisualEffects(global::ACE.Entity.Enum.PlayScript.Create);
            }
        }

        public Dictionary<uint, GeneratorRegistryNode> GeneratorRegistry = new Dictionary<uint, GeneratorRegistryNode>();

        public List<GeneratorQueueNode> GeneratorQueue = new List<GeneratorQueueNode>();

        public int? InitGeneratedObjects
        {
            get => AceObject.InitGeneratedObjects;
            set { AceObject.InitGeneratedObjects = value; }
        }

        public int? MaxGeneratedObjects
        {
            get => AceObject.MaxGeneratedObjects;
            set { AceObject.MaxGeneratedObjects = value; }
        }

        public double? RegenerationInterval
        {
            get => AceObject.RegenerationInterval;
            set { AceObject.RegenerationInterval = (double)value; }
        }

        public bool? GeneratorEnteredWorld
        {
            get => AceObject.GeneratorEnteredWorld;
            set { AceObject.GeneratorEnteredWorld = value; }
        }

        public virtual void HeartBeat()
        {
            // Do Stuff

            if (GeneratorQueue.Count > 0)
                ProcessGeneratorQueue();

            QueueNextHeartBeat();
        }

        public void QueueNextHeartBeat()
        {
            ActionChain nextHeartBeat = new ActionChain();
            nextHeartBeat.AddDelaySeconds(HeartbeatInterval ?? 5);
            nextHeartBeat.AddAction(this, () => HeartBeat());
            nextHeartBeat.EnqueueChain();
        }

        public List<int> GeneratorProfilesActive = new List<int>();

        public void SelectGeneratorProfiles()
        {
            GeneratorProfilesActive.Clear();

            Random random = new Random((int)DateTime.UtcNow.Ticks);

            if (GeneratorProfiles.Count > 0)
            {
                foreach (var profile in GeneratorProfiles)
                {
                    int slot = GeneratorProfiles.IndexOf(profile);

                    if (random.NextDouble() < profile.Probability)
                    {
                        GeneratorProfilesActive.Add(slot);
                    }
                }

            }
        }

        public void UpdateGeneratorInts()
        {
            bool initZero = (InitGeneratedObjects == 0);
            bool maxZero = (MaxGeneratedObjects == 0);

            foreach (int slot in GeneratorProfilesActive)
            {
                if (initZero)
                {
                    InitGeneratedObjects += (int?)GeneratorProfiles[slot].InitCreate;
                }

                if (maxZero)
                {
                    MaxGeneratedObjects += (int?)GeneratorProfiles[slot].MaxCreate;
                }
            }
        }

        public void QueueGenerator()
        {
            foreach(int slot in GeneratorProfilesActive)
            {
                bool slotInUse = false;
                foreach (var obj in GeneratorRegistry)
                {
                    if (obj.Value.Slot == slot)
                    {
                        slotInUse = true;
                        break;
                    }
                }

                foreach (var obj in GeneratorQueue)
                {
                    if (obj.Slot == slot)
                    {
                        slotInUse = true;
                        break;
                    }
                }

                if (slotInUse)
                    continue;

                var queue = new GeneratorQueueNode();

                queue.Slot = (uint)slot;
                double when = Common.Time.GetFutureTimestamp((RegenerationInterval ?? 0) + GeneratorProfiles[slot].Delay);

                if (GeneratorRegistry.Count < InitGeneratedObjects && !(GeneratorEnteredWorld ?? false))
                    when = Common.Time.GetTimestamp();

                queue.When = when;

                // System.Diagnostics.Debug.WriteLine($"Adding {queue.Slot} @ {queue.When} to GeneratorQueue for {Guid.Full}");
                GeneratorQueue.Add(queue);

                if (GeneratorQueue.Count >= InitGeneratedObjects)
                    GeneratorEnteredWorld = true;
            }
        }

        public void ProcessGeneratorQueue()
        {
            var index = 0;
            while (index < GeneratorQueue.Count)
            {
                double ts = Common.Time.GetTimestamp();
                if (ts >= GeneratorQueue[index].When)
                {
                    if (GeneratorRegistry.Count >= MaxGeneratedObjects)
                    {
                        // System.Diagnostics.Debug.WriteLine($"GeneratorRegistry for {Guid.Full} is at MaxGeneratedObjects {MaxGeneratedObjects}");
                        // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full}");
                        GeneratorQueue.RemoveAt(index);
                        index++;
                        continue;
                    }
                    var profile = GeneratorProfiles[(int)GeneratorQueue[index].Slot];

                    var rNode = new GeneratorRegistryNode();

                    rNode.WeenieClassId = profile.WeenieClassId;
                    rNode.Timestamp = Common.Time.GetTimestamp();
                    rNode.Slot = GeneratorQueue[index].Slot;

                    var wo = WorldObjectFactory.CreateNewWorldObject(profile.WeenieClassId);

                    if (wo != null)
                    {
                        switch (profile.WhereCreate)
                        {
                            case 4:
                                wo.Location = new Position(profile.LandblockRaw,
                                    profile.PositionX, profile.PositionY, profile.PositionZ,
                                    profile.RotationX, profile.RotationY, profile.RotationZ, profile.RotationW);
                                break;
                            default:
                                wo.Location = Location;
                                break;
                        }

                        wo.GeneratorId = Guid.Full;

                        // System.Diagnostics.Debug.WriteLine($"Adding {wo.Guid.Full} | {rNode.Slot} in GeneratorRegistry for {Guid.Full}");
                        GeneratorRegistry.Add(wo.Guid.Full, rNode);
                        // System.Diagnostics.Debug.WriteLine($"Spawning {GeneratorQueue[index].Slot} in GeneratorQueue for {Guid.Full}");
                        wo.EnterWorld();
                        // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full}");
                        GeneratorQueue.RemoveAt(index);
                    }
                    else
                    {
                        // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full} because wcid {rNode.WeenieClassId} is not in the database");
                        GeneratorQueue.RemoveAt(index);
                    }
                }
                else
                    index++;
            }
        }

        public void NotifyGeneratorOfPickup(uint guid)
        {
            if (GeneratorRegistry.ContainsKey(guid))
            {
                int slot = (int)GeneratorRegistry[guid].Slot;

                if (GeneratorProfiles[slot].WhenCreate == (uint)RegenerationType.PickUp)
                {
                    GeneratorRegistry.Remove(guid);
                    QueueGenerator();
                }
            }
        }

        public bool? Visibility
        {
            get => AceObject.Visibility;
            set { AceObject.Visibility = value; }
        }

        public int? PaletteTemplate
        {
            get => AceObject.PaletteTemplate;
            set { AceObject.PaletteTemplate = value; }
        }

        public double? Shade
        {
            get => AceObject.Shade;
            set { AceObject.Shade = value; }
        }

        public void GetClothingBase()
        {
            ClothingTable item;
            if (ClothingBase.HasValue)
                item = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)ClothingBase);
            else
            {
                return;
            }

            if (SetupTableId != null && item.ClothingBaseEffects.ContainsKey((uint)SetupTableId))
            // Check if the player model has data. Gear Knights, this is usually you.
            {
                // Add the model and texture(s)
                ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[(uint)SetupTableId];
                foreach (CloObjectEffect t in clothingBaseEffec.CloObjectEffects)
                {
                    byte partNum = (byte)t.Index;
                    AddModel((byte)t.Index, (ushort)t.ModelId);
                    foreach (CloTextureEffect t1 in t.CloTextureEffects)
                        AddTexture((byte)t.Index, (ushort)t1.OldTexture, (ushort)t1.NewTexture);
                }

                if (item.ClothingSubPalEffects.Count > 0)
                {
                    int size = item.ClothingSubPalEffects.Count;
                    int palCount = size;

                    CloSubPalEffect itemSubPal;
                    int palOption = 0;
                    if (PaletteTemplate.HasValue)
                        palOption = (int)PaletteTemplate;
                    if (item.ClothingSubPalEffects.ContainsKey((uint)palOption))
                    {
                        itemSubPal = item.ClothingSubPalEffects[(uint)palOption];
                    }
                    else
                    {
                        itemSubPal = item.ClothingSubPalEffects[item.ClothingSubPalEffects.Keys.ElementAt(0)];
                    }

                    if (itemSubPal.Icon > 0)
                        IconId = itemSubPal.Icon;

                    float shade = 0;
                    if (Shade.HasValue)
                        shade = (float)Shade;
                    for (int i = 0; i < itemSubPal.CloSubPalettes.Count; i++)
                    {
                        var itemPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(itemSubPal.CloSubPalettes[i].PaletteSet);
                        ushort itemPal = (ushort)itemPalSet.GetPaletteID(shade);

                        for (int j = 0; j < itemSubPal.CloSubPalettes[i].Ranges.Count; j++)
                        {
                            uint palOffset = itemSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                            uint numColors = itemSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                            AddPalette(itemPal, (ushort)palOffset, (ushort)numColors);
                        }
                    }
                }
            }
        }

        public void GenerateWieldList()
        {
            foreach (var item in WieldList)
            {
                if (WieldedObjects == null)
                    WieldedObjects = new Dictionary<ObjectGuid, WorldObject>();
                throw new System.NotImplementedException();/* Create the object, THEN set the palette/shade on the new object, not via this ctor
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId, item.Palette, item.Shade);

                wo.CurrentWieldedLocation = wo.ValidLocations;
                wo.WielderId = Guid.Full;

                WieldedObjects.Add(wo.Guid, wo);   */           
            }

            if (WieldedObjects != null)
                UpdateBaseAppearance();
        }

        public void UpdateBaseAppearance()
        {
            ClearObjDesc();
            AddBaseModelData(); // Add back in the facial features, hair and skin palette

            var coverage = new List<uint>();

            var clothing = new Dictionary<int, WorldObject>();
            foreach (var wo in WieldedObjects.Values)
            {
                if ((wo.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0)
                    clothing.Add((int)wo.Priority, wo);
            }
            foreach (var w in clothing.OrderBy(i => i.Key))
            {
                // We can wield things that are not part of our model, only use those items that can cover our model.
                if ((w.Value.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0)
                {
                    ClothingTable item;
                    if (w.Value.ClothingBase != null)
                        item = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)w.Value.ClothingBase);
                    else
                    {
                        return;
                    }

                    if (SetupTableId != null && item.ClothingBaseEffects.ContainsKey((uint)SetupTableId))
                    // Check if the player model has data. Gear Knights, this is usually you.
                    {
                        // Add the model and texture(s)
                        ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[(uint)SetupTableId];
                        foreach (CloObjectEffect t in clothingBaseEffec.CloObjectEffects)
                        {
                            byte partNum = (byte)t.Index;
                            AddModel((byte)t.Index, (ushort)t.ModelId);
                            coverage.Add(partNum);
                            foreach (CloTextureEffect t1 in t.CloTextureEffects)
                                AddTexture((byte)t.Index, (ushort)t1.OldTexture, (ushort)t1.NewTexture);
                        }

                        if (item.ClothingSubPalEffects.Count > 0)
                        {
                            int size = item.ClothingSubPalEffects.Count;
                            int palCount = size;

                            CloSubPalEffect itemSubPal;
                            int palOption = 0;
                            if (w.Value.PaletteTemplate.HasValue)
                                palOption = (int)w.Value.PaletteTemplate;
                            if (item.ClothingSubPalEffects.ContainsKey((uint)palOption))
                            {
                                itemSubPal = item.ClothingSubPalEffects[(uint)palOption];
                            }
                            else
                            {
                                itemSubPal = item.ClothingSubPalEffects[0];
                            }

                            float shade = 0;
                            if (w.Value.Shade.HasValue)
                                shade = (float)w.Value.Shade;
                            for (int i = 0; i < itemSubPal.CloSubPalettes.Count; i++)
                            {
                                var itemPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(itemSubPal.CloSubPalettes[i].PaletteSet);
                                ushort itemPal = (ushort)itemPalSet.GetPaletteID(shade);

                                for (int j = 0; j < itemSubPal.CloSubPalettes[i].Ranges.Count; j++)
                                {
                                    uint palOffset = itemSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                                    uint numColors = itemSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                                    AddPalette(itemPal, (ushort)palOffset, (ushort)numColors);
                                }
                            }
                        }
                    }
                }
            }
            // Add the "naked" body parts. These are the ones not already covered.
            if (SetupTableId != null)
            {
                var baseSetup = DatManager.PortalDat.ReadFromDat<SetupModel>((uint)SetupTableId);
                for (byte i = 0; i < baseSetup.Parts.Count; i++)
                {
                    if (!coverage.Contains(i) && i != 0x10) // Don't add body parts for those that are already covered. Also don't add the head, that was already covered by AddCharacterBaseModelData()
                        AddModel(i, baseSetup.Parts[i]);
                }
            }
        }

        private void AddBaseModelData()
        {
            if (WeenieType == WeenieType.Creature || WeenieType == WeenieType.Vendor)
                if (CreatureType == global::ACE.Entity.Enum.CreatureType.Human && !(WeenieClassId == 1 || WeenieClassId == 4))
                    RandomizeFace();

            if (PaletteBaseId == null)
                PaletteBaseId = 0x0400007e; // Default BasePalette
        }

        public int? Heritage
        {
            get => AceObject.Heritage;
            set { AceObject.Heritage = value; }
        }

        public int? Gender
        {
            get => AceObject.Gender;
            set { AceObject.Gender = value; }
        }

        public string HeritageGroup
        {
            get => AceObject.HeritageGroup;
            set { AceObject.HeritageGroup = value; }
        }

        public string Sex
        {
            get => AceObject.Sex;
            set { AceObject.Sex = value; }
        }

        public uint? HeadObjectDID
        {
            get => AceObject.HeadObjectDID ?? null;
            set { AceObject.HeadObjectDID = value; }
        }

        public uint? HairTextureDID
        {
            get => AceObject.HairTextureDID ?? null;
            set { AceObject.HairTextureDID = value; }
        }

        public uint? DefaultHairTextureDID
        {
            get => AceObject.DefaultHairTextureDID ?? null;
            set { AceObject.DefaultHairTextureDID = value; }
        }

        public uint? HairPaletteDID
        {
            get => AceObject.HairPaletteDID ?? null;
            set { AceObject.HairPaletteDID = value; }
        }

        public uint? SkinPaletteDID
        {
            get => AceObject.SkinPaletteDID ?? null;
            set { AceObject.SkinPaletteDID = value; }
        }

        public uint? EyesPaletteDID
        {
            get => AceObject.EyesPaletteDID ?? null;
            set { AceObject.EyesPaletteDID = value; }
        }

        public uint? EyesTextureDID
        {
            get => AceObject.EyesTextureDID ?? null;
            set { AceObject.EyesTextureDID = value; }
        }

        public uint? DefaultEyesTextureDID
        {
            get => AceObject.DefaultEyesTextureDID ?? null;
            set { AceObject.DefaultEyesTextureDID = value; }
        }

        public uint? NoseTextureDID
        {
            get => AceObject.NoseTextureDID ?? null;
            set { AceObject.NoseTextureDID = value; }
        }

        public uint? DefaultNoseTextureDID
        {
            get => AceObject.DefaultNoseTextureDID ?? null;
            set { AceObject.DefaultNoseTextureDID = value; }
        }

        public uint? MouthTextureDID
        {
            get => AceObject.MouthTextureDID ?? null;
            set { AceObject.MouthTextureDID = value; }
        }

        public uint? DefaultMouthTextureDID
        {
            get => AceObject.DefaultMouthTextureDID ?? null;
            set { AceObject.DefaultMouthTextureDID = value; }
        }

        public void RandomizeFace()
        {
            var cg = DatManager.PortalDat.CharGen;

            if (!Heritage.HasValue)
            {
                if (HeritageGroup != "")
                {
                    HeritageGroup parsed = (HeritageGroup)System.Enum.Parse(typeof(HeritageGroup), HeritageGroup.Replace("'", ""));
                    if (parsed != 0)
                        Heritage = (int)parsed;
                }
            }

            if (!Gender.HasValue)
            {
                if (Sex != "")
                {
                    Gender parsed = (Gender)System.Enum.Parse(typeof(Gender), Sex);
                    if (parsed != 0)
                        Gender = (int)parsed;
                }
            }

            SexCG sex = cg.HeritageGroups[(uint)Heritage].Genders[(int)Gender];

            PaletteBaseId = sex.BasePalette;

            Appearance appearance = new Appearance();

            appearance.HairStyle = 1;
            appearance.HairColor = 1;
            appearance.HairHue = 1;

            appearance.EyeColor = 1;
            appearance.Eyes = 1;

            appearance.Mouth = 1;
            appearance.Nose = 1;

            appearance.SkinHue = 1;

            // Get the hair first, because we need to know if you're bald, and that's the name of that tune!
            int size = sex.HairStyleList.Count / 3; // Why divide by 3 you ask? Because AC runtime generated characters didn't have much range in hairstyles.
            Random rand = new Random();
            appearance.HairStyle = (uint)rand.Next(size);

            HairStyleCG hairstyle = sex.HairStyleList[Convert.ToInt32(appearance.HairStyle)];
            bool isBald = hairstyle.Bald;

            size = sex.HairColorList.Count;
            appearance.HairColor = (uint)rand.Next(size);
            appearance.HairHue = rand.NextDouble();

            size = sex.EyeColorList.Count;
            appearance.EyeColor = (uint)rand.Next(size);
            size = sex.EyeStripList.Count;
            appearance.Eyes = (uint)rand.Next(size);

            size = sex.MouthStripList.Count;
            appearance.Mouth = (uint)rand.Next(size);

            size = sex.NoseStripList.Count;
            appearance.Nose = (uint)rand.Next(size);

            appearance.SkinHue = rand.NextDouble();

            //// Certain races (Undead, Tumeroks, Others?) have multiple body styles available. This is controlled via the "hair style".
            ////if (hairstyle.AlternateSetup > 0)
            ////    character.SetupTableId = hairstyle.AlternateSetup;

            if (!EyesTextureDID.HasValue)
                EyesTextureDID = sex.GetEyeTexture(appearance.Eyes, isBald);
            if (!DefaultEyesTextureDID.HasValue)
                DefaultEyesTextureDID = sex.GetDefaultEyeTexture(appearance.Eyes, isBald);
            if (!NoseTextureDID.HasValue)
                NoseTextureDID = sex.GetNoseTexture(appearance.Nose);
            if (!DefaultNoseTextureDID.HasValue)
                DefaultNoseTextureDID = sex.GetDefaultNoseTexture(appearance.Nose);
            if (!MouthTextureDID.HasValue)
                MouthTextureDID = sex.GetMouthTexture(appearance.Mouth);
            if (!DefaultMouthTextureDID.HasValue)
                DefaultMouthTextureDID = sex.GetDefaultMouthTexture(appearance.Mouth);
            if (!HairTextureDID.HasValue)
                HairTextureDID = sex.GetHairTexture(appearance.HairStyle);
            if (!DefaultHairTextureDID.HasValue)
                DefaultHairTextureDID = sex.GetDefaultHairTexture(appearance.HairStyle);
            if (!HeadObjectDID.HasValue)
                HeadObjectDID = sex.GetHeadObject(appearance.HairStyle);

            // Skin is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            var skinPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.SkinPalSet);
            if (!SkinPaletteDID.HasValue)
                SkinPaletteDID = skinPalSet.GetPaletteID(appearance.SkinHue);

            // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            var hairPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.HairColorList[Convert.ToInt32(appearance.HairColor)]);
            if (!HairPaletteDID.HasValue)
                HairPaletteDID = hairPalSet.GetPaletteID(appearance.HairHue);

            // Eye Color
            if (!EyesPaletteDID.HasValue)
                EyesPaletteDID = sex.EyeColorList[Convert.ToInt32(appearance.EyeColor)];

            // Hair/head
            AddModel(0x10, (uint)HeadObjectDID);
            AddTexture(0x10, (uint)DefaultHairTextureDID, (uint)HairTextureDID);
            AddPalette((uint)HairPaletteDID, 0x18, 0x8);

            // Skin
            //// PaletteBaseId = Character.PaletteId;
            AddPalette((uint)SkinPaletteDID, 0x0, 0x18);

            // Eyes
            AddTexture(0x10, (uint)DefaultEyesTextureDID, (uint)EyesTextureDID);
            AddPalette((uint)EyesPaletteDID, 0x20, 0x8);

            // Nose & Mouth
            AddTexture(0x10, (uint)DefaultNoseTextureDID, (uint)NoseTextureDID);
            AddTexture(0x10, (uint)DefaultMouthTextureDID, (uint)MouthTextureDID);
        }
    }
}
