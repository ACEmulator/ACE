using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;

using AceObjectGeneratorProfile = ACE.Entity.AceObjectGeneratorProfile;
using AceObjectInventory = ACE.Entity.AceObjectInventory;
using Position = ACE.Entity.Position;

namespace ACE.Server.WorldObjects
{
    public abstract partial class WorldObject : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This is object property overrides that should have come from the shard db (or init to defaults of object is new to this instance).
        /// You should not manipulate these values directly. To manipulate this use the exposed SetProperty and RemoveProperty functions instead.
        /// </summary>
        public Biota Biota { get; }

        /// <summary>
        /// This is just a wrapper around Biota.Id
        /// </summary>
        public ObjectGuid Guid => new ObjectGuid(Biota.Id);

        public ObjectDescriptionFlag BaseDescriptionFlags { get; protected set; }

        public UpdatePositionFlag PositionFlag { get; protected set; }

        public SequenceManager Sequences { get; } = new SequenceManager();

        public virtual float ListeningRadius { get; protected set; } = 5f;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        protected WorldObject(Weenie weenie, ObjectGuid guid)
        {
            Biota = weenie.CreateCopyAsBiota(guid.Full);

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// Any properties tagged as Ephemeral will be removed from the biota.
        /// </summary>
        protected WorldObject(Biota biota)
        {
            Biota = biota;

            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        { 
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

            foreach (var x in EphemeralProperties.PropertiesBool.ToList())
                EphemeralPropertyBools.TryAdd((PropertyBool)x, null);
            foreach (var x in EphemeralProperties.PropertiesDataId.ToList())
                EphemeralPropertyDataIds.TryAdd((PropertyDataId)x, null);
            foreach (var x in EphemeralProperties.PropertiesDouble.ToList())
                EphemeralPropertyFloats.TryAdd((PropertyFloat)x, null);
            foreach (var x in EphemeralProperties.PropertiesInstanceId.ToList())
                EphemeralPropertyInstanceIds.TryAdd((PropertyInstanceId)x, null);
            foreach (var x in EphemeralProperties.PropertiesInt.ToList())
                EphemeralPropertyInts.TryAdd((PropertyInt)x, null);
            foreach (var x in EphemeralProperties.PropertiesInt64.ToList())
                EphemeralPropertyInt64s.TryAdd((PropertyInt64)x, null);
            foreach (var x in EphemeralProperties.PropertiesString.ToList())
                EphemeralPropertyStrings.TryAdd((PropertyString)x, null);

            foreach (var x in Biota.BiotaPropertiesBool.Where(i => EphemeralProperties.PropertiesBool.Contains(i.Type)).ToList())
                EphemeralPropertyBools[(PropertyBool)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesDID.Where(i => EphemeralProperties.PropertiesDataId.Contains(i.Type)).ToList())
                EphemeralPropertyDataIds[(PropertyDataId)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesFloat.Where(i => EphemeralProperties.PropertiesDouble.Contains(i.Type)).ToList())
                EphemeralPropertyFloats[(PropertyFloat)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesIID.Where(i => EphemeralProperties.PropertiesInstanceId.Contains(i.Type)).ToList())
                EphemeralPropertyInstanceIds[(PropertyInstanceId)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesInt.Where(i => EphemeralProperties.PropertiesInt.Contains(i.Type)).ToList())
                EphemeralPropertyInts[(PropertyInt)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesInt64.Where(i => EphemeralProperties.PropertiesInt64.Contains(i.Type)).ToList())
                EphemeralPropertyInt64s[(PropertyInt64)x.Type] = x.Value;
            foreach (var x in Biota.BiotaPropertiesString.Where(i => EphemeralProperties.PropertiesString.Contains(i.Type)).ToList())
                EphemeralPropertyStrings[(PropertyString)x.Type] = x.Value;

            BaseDescriptionFlags = ObjectDescriptionFlag.Attackable | ObjectDescriptionFlag.Stuck;

            UpdateBaseAppearance();

            if (Placement == null)
                Placement = ACE.Entity.Enum.Placement.Resting;

            GetClothingBase();

            return;

            SelectGeneratorProfiles();
            UpdateGeneratorInts();
            QueueGenerator();

            QueueNextHeartBeat();

            GenerateWieldList();
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


        [Obsolete]
        protected AceObject AceObject { get; set; }
        [Obsolete]
        protected internal Dictionary<ObjectGuid, WorldObject> WieldedObjects { get; set; } = new Dictionary<ObjectGuid, WorldObject>();


        [Obsolete]
        //protected internal Dictionary<ObjectGuid, AceObject> Inventory => AceObject.Inventory;
        public Dictionary<ObjectGuid, AceObject> Inventory = new Dictionary<ObjectGuid, AceObject>();

        // This dictionary is only used to load WieldedObjects and to save them.   Other than the load and save, it should never be added to or removed from.
        [Obsolete]
        //protected internal Dictionary<ObjectGuid, AceObject> WieldedItems => AceObject.WieldedItems;
        public Dictionary<ObjectGuid, AceObject> WieldedItems = new Dictionary<ObjectGuid, AceObject>();

        // we need to expose this read only for examine to work. Og II
        //[Obsolete]
        //public List<AceObjectPropertiesInt> PropertiesInt => AceObject.IntProperties;
        //[Obsolete]
        //public List<AceObjectPropertiesInt64> PropertiesInt64 => AceObject.Int64Properties;
        //[Obsolete]
        //public List<AceObjectPropertiesBool> PropertiesBool => AceObject.BoolProperties;
        //[Obsolete]
        //public List<AceObjectPropertiesString> PropertiesString => AceObject.StringProperties;
        //[Obsolete]
        //public List<AceObjectPropertiesDouble> PropertiesDouble => AceObject.DoubleProperties;
        //[Obsolete]
        //public List<AceObjectPropertiesDataId> PropertiesDid => AceObject.DataIdProperties;
        //[Obsolete]
        //public List<AceObjectPropertiesInstanceId> PropertiesIid => AceObject.InstanceIdProperties;
        //[Obsolete]
        //public List<AceObjectPropertiesSpell> PropertiesSpellId => AceObject.SpellIdProperties;
        //[Obsolete]
        //public Dictionary<uint, AceObjectPropertiesBook> PropertiesBook => AceObject.BookProperties;

        public Dictionary<PropertyInt, int> PropertiesInt => GetAllPropertyInt();
        public Dictionary<PropertyInt64, long> PropertiesInt64 => GetAllPropertyInt64();
        public Dictionary<PropertyBool, bool> PropertiesBool => GetAllPropertyBools();
        public Dictionary<PropertyString, string> PropertiesString => GetAllPropertyString();
        public Dictionary<PropertyFloat, double> PropertiesDouble => GetAllPropertyFloat();
        public Dictionary<PropertyDataId, uint> PropertiesDid => GetAllPropertyDataId();
        public Dictionary<PropertyInstanceId, int> PropertiesIid => GetAllPropertyInstanceId();


        // subpalettes
        [Obsolete]
        public List<ModelPalette> GetPalettes => modelPalettes.ToList();

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
            get => GetProperty(PropertyDataId.PaletteBase);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.PaletteBase); else SetProperty(PropertyDataId.PaletteBase, value.Value); }
        }




        public ParentLocation? ParentLocation
        {
            get => (ParentLocation?)GetProperty(PropertyInt.ParentLocation);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ParentLocation); else SetProperty(PropertyInt.ParentLocation, (int)value.Value); }
        }

   




        

        // movement_buffer






        // pos





        public virtual int? StackUnitValue => Biota.GetProperty(PropertyInt.StackUnitValue) ?? 0;

        public int? PlacementPosition
        {
            get => GetProperty(PropertyInt.PlacementPosition);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PlacementPosition); else SetProperty(PropertyInt.PlacementPosition, value.Value); }
        }




        public virtual ushort? StackUnitBurden => (ushort?)(Biota.GetProperty(PropertyInt.StackUnitEncumbrance) ?? 0);




        public IActor CurrentParent { get; private set; }

        public Position ForcedLocation { get; private set; }

        public Position RequestedLocation { get; private set; }

        /// <summary>
        /// Should only be adjusted by LandblockManager -- default is null
        /// </summary>
        public Landblock CurrentLandblock => CurrentParent as Landblock;


        /// <summary>
        /// Time when this object will despawn, -1 is never.
        /// </summary>
        public double DespawnTime { get; set; } = -1;

        private readonly NestedActionQueue actionQueue = new NestedActionQueue();



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
            get => (CombatStyle?)GetProperty(PropertyInt.DefaultCombatStyle);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.DefaultCombatStyle); else SetProperty(PropertyInt.DefaultCombatStyle, (int)value.Value); }
        }

        public int? GeneratorId
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
            set { if (!value.HasValue) RemoveProperty(PropertyInt.BoostValue); else SetProperty(PropertyInt.BoostValue, (int)value.Value); }
        }

        public uint? SpellDID
        {
            get => GetProperty(PropertyDataId.Spell);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.Spell); else SetProperty(PropertyDataId.Spell, value.Value); }
        }

        public int? BoostEnum
        {
            get => GetProperty(PropertyInt.BoosterEnum);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.BoosterEnum); else SetProperty(PropertyInt.BoosterEnum, (int)value.Value); }
        }

        public double? HealkitMod
        {
            get => GetProperty(PropertyFloat.HealkitMod);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.HealkitMod); else SetProperty(PropertyFloat.HealkitMod, value.Value); }
        }

        public virtual int? CoinValue
        {
            get => GetProperty(PropertyInt.CoinValue);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CoinValue); else SetProperty(PropertyInt.CoinValue, (int)value.Value); }
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
            //PageData pageData = new PageData();
            //AceObjectPropertiesBook bookPage = PropertiesBook[pageNum];

            //pageData.AuthorID = bookPage.AuthorId;
            //pageData.AuthorName = bookPage.AuthorName;
            //pageData.AuthorAccount = bookPage.AuthorAccount;
            //pageData.PageIdx = pageNum;
            //pageData.PageText = bookPage.PageText;
            //pageData.IgnoreAuthor = false;
            //// TODO - check for PropertyBool.IgnoreAuthor flag

            //var bookDataResponse = new GameEventBookPageDataResponse(reader, Guid.Full, pageData);
            //reader.Network.EnqueueSend(bookDataResponse);
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
                        var descriptionFlags = CalculatedDescriptionFlag();
                        debugOutput += $"{prop.Name} = {descriptionFlags.ToString()}" + " (" + (uint)descriptionFlags + ")" + "\n";
                        break;
                    case "weenieflags":
                        var weenieFlags = CalculatedWeenieHeaderFlag();
                        debugOutput += $"{prop.Name} = {weenieFlags.ToString()}" + " (" + (uint)weenieFlags + ")" + "\n";
                        break;
                    case "weenieflags2":
                        var weenieFlags2 = CalculatedWeenieHeaderFlag2();
                        debugOutput += $"{prop.Name} = {weenieFlags2.ToString()}" + " (" + (uint)weenieFlags2 + ")" + "\n";
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
                        var physicsDescriptionFlag = CalculatedPhysicsDescriptionFlag();
                        debugOutput += $"{prop.Name} = {physicsDescriptionFlag.ToString()}" + " (" + (uint)physicsDescriptionFlag + ")" + "\n";
                        break;
                    case "physicsstate":
                        var physicsState = CalculatedPhysicsState();
                        debugOutput += $"{prop.Name} = {physicsState.ToString()}" + " (" + (uint)physicsState + ")" + "\n";
                        break;
                    case "propertiesint":
                        foreach (var item in obj.GetAllPropertyInt())
                        {
                            debugOutput += $"PropertyInt.{System.Enum.GetName(typeof(PropertyInt), item.Key)} ({(int)item.Key}) = {item.Value}" + "\n";
                        }
                        break;
                    case "propertiesint64":
                        foreach (var item in obj.GetAllPropertyInt64())
                        {
                            debugOutput += $"PropertyInt64.{System.Enum.GetName(typeof(PropertyInt64), item.Key)} ({(int)item.Key}) = {item.Value}" + "\n";
                        }
                        break;
                    case "propertiesbool":
                        foreach (var item in obj.GetAllPropertyBools())
                        {
                            debugOutput += $"PropertyBool.{System.Enum.GetName(typeof(PropertyBool), item.Key)} ({(int)item.Key}) = {item.Value}" + "\n";
                        }
                        break;
                    case "propertiesstring":
                        foreach (var item in obj.GetAllPropertyString())
                        {
                            debugOutput += $"PropertyString.{System.Enum.GetName(typeof(PropertyString), item.Key)} ({(int)item.Key}) = {item.Value}" + "\n";
                        }
                        break;
                    case "propertiesdouble":
                        foreach (var item in obj.GetAllPropertyFloat())
                        {
                            debugOutput += $"PropertyDouble.{System.Enum.GetName(typeof(PropertyFloat), item.Key)} ({(int)item.Key}) = {item.Value}" + "\n";
                        }
                        break;
                    case "propertiesdid":
                        foreach (var item in obj.GetAllPropertyDataId())
                        {
                            debugOutput += $"PropertyDataId.{System.Enum.GetName(typeof(PropertyDataId), item.Key)} ({(int)item.Key}) = {item.Value}" + "\n";
                        }
                        break;
                    case "propertiesiid":
                        foreach (var item in obj.GetAllPropertyInstanceId())
                        {
                            debugOutput += $"PropertyInstanceId.{System.Enum.GetName(typeof(PropertyInstanceId), item.Key)} ({(int)item.Key}) = {item.Value}" + "\n";
                        }
                        break;
                    //case "propertiesspellid":
                    //    foreach (var item in obj.PropertiesSpellId)
                    //    {
                    //        debugOutput += $"PropertySpellId.{System.Enum.GetName(typeof(Spell), item.SpellId)} ({item.SpellId})" + "\n";
                    //    }
                    //    break;
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


        // This fully replaces the PhysicsState of the WO, use sparingly?
        //public void SetPhysicsState(PhysicsState state, bool packet = true)
        //{
        //    PhysicsState = state;

        //    if (packet)
        //    {
        //        EnqueueBroadcastPhysicsState();
        //    }
        //}

        public void EnqueueBroadcastPhysicsState()
        {
            if (CurrentLandblock != null)
            {
                var physicsState = CalculatedPhysicsState();
                GameMessage msg = new GameMessageSetState(this, physicsState);
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

        //public List<AceObjectInventory> CreateList => AceObject.CreateList;
        public List<AceObjectInventory> CreateList { get; set; } = new List<AceObjectInventory>();

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

        //public List<AceObjectGeneratorProfile> GeneratorProfiles => AceObject.GeneratorProfiles;
        public List<AceObjectGeneratorProfile> GeneratorProfiles { get; set; } = new List<AceObjectGeneratorProfile>();

        public double? HeartbeatInterval
        {
            get => GetProperty(PropertyFloat.HeartbeatInterval);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.SellPrice); else SetProperty(PropertyFloat.HeartbeatInterval, value.Value); }
        }

        public void EnterWorld()
        {
            if (Location != null)
            {
                LandblockManager.AddObject(this);
                if (SuppressGenerateEffect != true)
                    ApplyVisualEffects(ACE.Entity.Enum.PlayScript.Create);
            }
        }

        public Dictionary<uint, GeneratorRegistryNode> GeneratorRegistry = new Dictionary<uint, GeneratorRegistryNode>();

        public List<GeneratorQueueNode> GeneratorQueue = new List<GeneratorQueueNode>();

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

                        wo.GeneratorId = (int)Guid.Full;

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

        public void GetClothingBase()
        {
            ClothingTable item;
            if (ClothingBase.HasValue)
                item = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)ClothingBase);
            else
            {
                return;
            }

            if (item.ClothingBaseEffects.ContainsKey(SetupTableId))
            // Check if the player model has data. Gear Knights, this is usually you.
            {
                // Add the model and texture(s)
                ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[SetupTableId];
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

                    if (item.ClothingBaseEffects.ContainsKey(SetupTableId))
                    // Check if the player model has data. Gear Knights, this is usually you.
                    {
                        // Add the model and texture(s)
                        ClothingBaseEffect clothingBaseEffec = item.ClothingBaseEffects[SetupTableId];
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
            if (SetupTableId > 0)
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

        public void RandomizeFace()
        {
            var cg = DatManager.PortalDat.CharGen;

            if (!Heritage.HasValue)
            {
                if (!String.IsNullOrEmpty(HeritageGroup))
                {
                    HeritageGroup parsed = (HeritageGroup)Enum.Parse(typeof(HeritageGroup), HeritageGroup.Replace("'", ""));
                    if (parsed != 0)
                        Heritage = (int)parsed;
                }
            }

            if (!Gender.HasValue)
            {
                if (!String.IsNullOrEmpty(Sex))
                {
                    Gender parsed = (Gender)Enum.Parse(typeof(Gender), Sex);
                    if (parsed != 0)
                        Gender = (int)parsed;
                }
            }

            if (!Heritage.HasValue || !Gender.HasValue)
                return;

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
