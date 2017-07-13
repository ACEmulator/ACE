using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameMessages;
using ACE.Network.GameEvent.Events;
using ACE.Network.Sequence;
using ACE.Network.Motion;
using System.Collections.Generic;
using System.IO;
using ACE.Managers;
using log4net;
using System;
using System.Diagnostics;
using System.Linq;

namespace ACE.Entity
{
    using Enum.Properties;
    using System.Windows.Forms;

    public abstract class WorldObject : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static float MaxObjectTrackingRange { get; } = 20000f;

        public ObjectGuid Guid
        {
            get { return new ObjectGuid(AceObject.AceObjectId); }
            private set { AceObject.AceObjectId = value.Full; }
        }

        protected AceObject AceObject { get; set; }

        // we need to expose this read only for examine to work. Og II
        public List<AceObjectPropertiesInt> PropertiesInt
        {
            get { return AceObject.IntProperties; }
        }

        public List<AceObjectPropertiesInt64> PropertiesInt64
        {
            get { return AceObject.Int64Properties; }
        }

        public List<AceObjectPropertiesBool> PropertiesBool
        {
            get { return AceObject.BoolProperties; }
        }

        public List<AceObjectPropertiesString> PropertiesString
        {
            get { return AceObject.StringProperties; }
        }

        public List<AceObjectPropertiesDouble> PropertiesDouble
        {
            get { return AceObject.DoubleProperties; }
        }

        public List<AceObjectPropertiesDataId> PropertiesDid
        {
            get { return AceObject.DataIdProperties; }
        }

        public List<AceObjectPropertiesInstanceId> PropertiesIid
        {
            get { return AceObject.InstanceIdProperties; }
        }

        public List<AceObjectPropertiesSpell> PropertiesSpellId
        {
            get { return AceObject.SpellIdProperties; }
        }

        public ItemType ItemType
        {
            get { return (ItemType)AceObject.GetIntProperty(PropertyInt.ItemType); }
            protected set { AceObject.SetIntProperty(PropertyInt.ItemType, (uint)value); }
        }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public uint WeenieClassId
        {
            get { return AceObject.WeenieClassId; }
            protected set { AceObject.WeenieClassId = value; }
        }

        public uint? IconId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.Icon); }
            set { AceObject.SetDataIdProperty(PropertyDataId.Icon, value); }
        }

        public string Name
        {
            get { return AceObject.GetStringProperty(PropertyString.Name); }
            protected set { AceObject.SetStringProperty(PropertyString.Name, value); }
        }

        public IActor CurrentParent { get; private set; }

        public Position ForcedLocation { get; private set; }

        public Position RequestedLocation { get; private set; }

        /// <summary>
        /// Should only be adjusted by LandblockManager -- default is null
        /// </summary>
        public Landblock CurrentLandblock
        {
            get { return CurrentParent as Landblock; }
        }

        /// <summary>
        /// tick-stamp for the last time this object changed in any way.
        /// </summary>
        public double LastUpdatedTicks { get; set; }

        /// <summary>
        /// Time when this object will despawn, -1 is never.
        /// </summary>
        public double DespawnTime { get; set; } = -1;

        private readonly NestedActionQueue actionQueue = new NestedActionQueue();

        public virtual Position Location
        {
            get { return AceObject.GetPosition(PositionType.Location); }
            set
            {
                /*
                log.Debug($"{Name} moved to {Position}");

                Position = value;
                */
                if (AceObject.GetPosition(PositionType.Location) != null)
                    LastUpdatedTicks = WorldManager.PortalYearTicks;
                AceObject.SetPosition(PositionType.Location, value);
            }
        }

        /// <summary>
        /// tick-stamp for the last time a movement update was sent
        /// </summary>
        public double LastMovementBroadcastTicks { get; set; }

        /// <summary>
        /// tick-stamp for the server time of the last time the player moved.
        /// TODO: implement
        /// </summary>
        public double LastAnimatedTicks { get; set; }

        public ObjectDescriptionFlag DescriptionFlags
        {
            get { return (ObjectDescriptionFlag)AceObject.AceObjectDescriptionFlags; }
            protected internal set { AceObject.AceObjectDescriptionFlags = (uint)value; }
        }

        public WeenieHeaderFlag WeenieFlags
        {
            get { return (WeenieHeaderFlag)AceObject.WeenieHeaderFlags; }
            protected internal set { AceObject.WeenieHeaderFlags = (uint)value; }
        }

        public WeenieHeaderFlag2 WeenieFlags2 { get; protected set; }

        public UpdatePositionFlag PositionFlag { get; set; }

        public virtual void PlayScript(Session session) { }

        public virtual float ListeningRadius { get; protected set; } = 5f;

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

        public string NamePlural
        {
            get { return AceObject.GetStringProperty(PropertyString.PluralName); }
            set { AceObject.SetStringProperty(PropertyString.PluralName, value); }
        }

        public byte? ItemCapacity
        {
            get { return (byte?)AceObject.GetIntProperty(PropertyInt.ItemsCapacity); }
            set { AceObject.SetIntProperty(PropertyInt.ItemsCapacity, value); }
        }

        public byte? ContainerCapacity
        {
            get { return (byte?)AceObject.GetIntProperty(PropertyInt.ContainersCapacity); }
            set { AceObject.SetIntProperty(PropertyInt.ContainersCapacity, value); }
        }

        public AmmoType? AmmoType
        {
            get { return (AmmoType?)AceObject.GetIntProperty(PropertyInt.AmmoType); }
            set { AceObject.SetIntProperty(PropertyInt.AmmoType, (uint?)value); }
        }

        public uint? Value
        {
            get { return AceObject.GetIntProperty(PropertyInt.Value); }
            set { AceObject.SetIntProperty(PropertyInt.Value, value); }
        }

        public Usable? Usable
        {
            get { return (Usable?)AceObject.GetIntProperty(PropertyInt.ItemUseable); }
            set { AceObject.SetIntProperty(PropertyInt.ItemUseable, (uint?)value); }
        }

        public float? UseRadius
        {
            get { return (float?)AceObject.GetDoubleProperty(PropertyDouble.UseRadius); }
            set { AceObject.SetDoubleProperty(PropertyDouble.UseRadius, value); }
        }

        public uint? TargetType
        {
            get { return AceObject.GetIntProperty(PropertyInt.TargetType); }
            set { AceObject.SetIntProperty(PropertyInt.TargetType, value); }
        }

        public UiEffects? UiEffects
        {
            get { return (UiEffects?)AceObject.GetIntProperty(PropertyInt.UiEffects); }
            set { AceObject.SetIntProperty(PropertyInt.UiEffects, (uint?)value); }
        }

        public CombatUse? CombatUse
        {
            get { return (CombatUse?)AceObject.GetIntProperty(PropertyInt.CombatUse); }
            set { AceObject.SetIntProperty(PropertyInt.CombatUse, (byte?)value); }
        }

        public MotionStance? DefaultCombatStyle
        {
            get { return (MotionStance?)AceObject.GetIntProperty(PropertyInt.DefaultCombatStyle); }
            set { AceObject.SetIntProperty(PropertyInt.DefaultCombatStyle, (uint?)value); }
        }

        /// <summary>
        /// This is used to indicate the number of uses remaining.  Example 32 uses left out of 50 (MaxStructure)
        /// </summary>
        public ushort? Structure
        {
            get { return (ushort?)AceObject.GetIntProperty(PropertyInt.Structure); }
            set { AceObject.SetIntProperty(PropertyInt.Structure, value); }
        }

        /// <summary>
        /// Use Limit - example 50 use healing kit
        /// </summary>
        public ushort? MaxStructure
        {
            get { return (ushort?)AceObject.GetIntProperty(PropertyInt.MaxStructure); }
            set { AceObject.SetIntProperty(PropertyInt.MaxStructure, value); }
        }

        public ushort? StackSize
        {
            get { return (ushort?)AceObject.GetIntProperty(PropertyInt.StackSize); }
            set { AceObject.SetIntProperty(PropertyInt.StackSize, value); }
        }

        public ushort? MaxStackSize
        {
            get { return (ushort?)AceObject.GetIntProperty(PropertyInt.MaxStackSize); }
            set { AceObject.SetIntProperty(PropertyInt.MaxStackSize, value); }
        }

        public uint? ContainerId
        {
            get { return AceObject.GetInstanceIdProperty(PropertyInstanceId.Container); }
            set { AceObject.SetInstanceIdProperty(PropertyInstanceId.Container, value); }
        }

        // TODO: I think we need to store this in aceObject from pacps - example Paul's Axe (Blue Ox)
        public uint? WielderId
        {
            get { return AceObject.GetInstanceIdProperty(PropertyInstanceId.Wielder); }
            set { AceObject.SetInstanceIdProperty(PropertyInstanceId.Wielder, value); }
        }

        public uint? GeneratorId
        {
            get { return AceObject.GetInstanceIdProperty(PropertyInstanceId.Generator); }
            set { AceObject.SetInstanceIdProperty(PropertyInstanceId.Generator, value); }
        }

        public EquipMask? ValidLocations
        {
            get { return (EquipMask?)AceObject.GetIntProperty(PropertyInt.ValidLocations); }
            set { AceObject.SetIntProperty(PropertyInt.ValidLocations, (uint?)value); }
        }

        /// <summary>
        /// Location - renamed for conflict with wo and to have a more descriptive name
        /// </summary>
        public EquipMask? CurrentWieldedLocation
        {
            get { return (EquipMask?)AceObject.GetIntProperty(PropertyInt.CurrentWieldedLocation); }
            set { AceObject.SetIntProperty(PropertyInt.CurrentWieldedLocation, (uint?)value); }
        }

        public CoverageMask? Priority
        {
            get { return (CoverageMask?)AceObject.GetIntProperty(PropertyInt.ClothingPriority); }
            set { AceObject.SetIntProperty(PropertyInt.ClothingPriority, (uint?)value); }
        }

        public RadarColor? RadarColor
        {
            get { return (RadarColor?)AceObject.GetIntProperty(PropertyInt.RadarBlipColor); }
            set { AceObject.SetIntProperty(PropertyInt.RadarBlipColor, (byte?)value); }
        }

        public RadarBehavior? RadarBehavior
        {
            get { return (RadarBehavior?)AceObject.GetIntProperty(PropertyInt.ShowableOnRadar); }
            set { AceObject.SetIntProperty(PropertyInt.ShowableOnRadar, (byte?)value); }
        }

        public ushort? Script
        {
            get { return (ushort?)AceObject.GetDataIdProperty(PropertyDataId.PhysicsScript); }
            set { AceObject.SetDataIdProperty(PropertyDataId.PhysicsScript, value); }
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
            get { return AceObject.GetIntProperty(PropertyInt.ItemWorkmanship); }
            set { AceObject.SetIntProperty(PropertyInt.ItemWorkmanship, value); }
        }

        public ushort? Burden
        {
            get { return (ushort?)AceObject.GetIntProperty(PropertyInt.EncumbranceVal); }
            set { AceObject.SetIntProperty(PropertyInt.EncumbranceVal, value); }
        }

        public Spell? Spell
        {
            get { return (Spell?)AceObject.GetDataIdProperty(PropertyDataId.Spell); }
            set { AceObject.SetDataIdProperty(PropertyDataId.Spell, (ushort?)value); }
        }

        /// <summary>
        /// Housing links to another packet, that needs sent.. The HouseRestrictions ACL Control list that contains all the housing data
        /// </summary>
        public uint? HouseOwner { get; set; }

        public uint? HouseRestrictions { get; set; }

        public ushort? HookItemType
        {
            get { return (ushort?)AceObject.GetIntProperty(PropertyInt.HookItemType); }
            set { AceObject.SetIntProperty(PropertyInt.HookItemType, value); }
        }

        public uint? Monarch { get; set; }

        public ushort? HookType
        {
            get { return (ushort?)AceObject.GetIntProperty(PropertyInt.HookType); }
            set { AceObject.SetIntProperty(PropertyInt.HookType, value); }
        }

        public uint? IconOverlayId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.IconOverlay); }
            set { AceObject.SetDataIdProperty(PropertyDataId.IconOverlay, value); }
        }

        public uint? IconUnderlayId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.IconUnderlay); }
            set { AceObject.SetDataIdProperty(PropertyDataId.IconUnderlay, value); }
        }

        public Material? MaterialType
        {
            get { return (Material?)AceObject.GetIntProperty(PropertyInt.MaterialType); }
            set { AceObject.SetIntProperty(PropertyInt.MaterialType, (byte?)value); }
        }

        public uint? PetOwner { get; set; }

        public uint? CooldownId
        {
            get { return AceObject.GetIntProperty(PropertyInt.SharedCooldown); }
            set { AceObject.SetIntProperty(PropertyInt.SharedCooldown, value); }
        }

        public double? CooldownDuration
        {
            get { return AceObject.GetDoubleProperty(PropertyDouble.CooldownDuration); }
            set { AceObject.SetDoubleProperty(PropertyDouble.CooldownDuration, value); }
        }

        // PhysicsData Logical
        /// <summary>
        /// setup_id in aclogviewer - used to get the correct model out of the dat file
        /// </summary>
        public uint? SetupTableId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.Setup); }
            set { AceObject.SetDataIdProperty(PropertyDataId.Setup, value); }
        }

        public PhysicsDescriptionFlag PhysicsDescriptionFlag { get; set; }

        public PhysicsState PhysicsState
        {
            get { return (PhysicsState)AceObject.GetIntProperty(PropertyInt.PhysicsState); }
            set { AceObject.SetIntProperty(PropertyInt.PhysicsState, (uint)value); }
        }

        /// <summary>
        /// mtable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint? MotionTableId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.MotionTable); }
            set { AceObject.SetDataIdProperty(PropertyDataId.MotionTable, value); }
        }
        /// <summary>
        /// stable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint? SoundTableId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.SoundTable); }
            set { AceObject.SetDataIdProperty(PropertyDataId.SoundTable, value); }
        }
        /// <summary>
        /// phstable_id in aclogviewer This is the physics table for the object.   Looked up from dat file.
        /// </summary>
        public uint? PhysicsTableId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.PhysicsEffectTable); }
            set { AceObject.SetDataIdProperty(PropertyDataId.PhysicsEffectTable, value); }
        }

        /// <summary>
        /// This is used for equiped items that are selectable.   Weapons or shields only.   Max 2
        /// </summary>
        public uint? ParentId
        {
            get { return AceObject.GetInstanceIdProperty(PropertyInstanceId.Wielder); }
            set { AceObject.SetInstanceIdProperty(PropertyInstanceId.Wielder, value); }
        }

        public uint? ParentLocation
        {
            get { return AceObject.GetIntProperty(PropertyInt.ParentLocation); }
            set { AceObject.SetIntProperty(PropertyInt.ParentLocation, value); }
        }

        ////public EquipMask? EquipperPhysicsDescriptionFlag;

        public List<EquippedItem> Children { get; } = new List<EquippedItem>();

        public float? ObjScale
        {
            get { return (float?)AceObject.GetDoubleProperty(PropertyDouble.DefaultScale); }
            set { AceObject.SetDoubleProperty(PropertyDouble.DefaultScale, value); }
        }

        public float? Friction
        {
            get { return (float?)AceObject.GetDoubleProperty(PropertyDouble.Friction); }
            set { AceObject.SetDoubleProperty(PropertyDouble.Friction, value); }
        }

        public float? Elasticity
        {
            get { return (float?)AceObject.GetDoubleProperty(PropertyDouble.Elasticity); }
            set { AceObject.SetDoubleProperty(PropertyDouble.Elasticity, value); }
        }

        public uint? AnimationFrame
        {
            get { return AceObject.GetIntProperty(PropertyInt.PlacementPosition); }
            set { AceObject.SetIntProperty(PropertyInt.PlacementPosition, value); }
        }

        public AceVector3 Acceleration { get; set; }

        public float? Translucency
        {
            get { return (float?)AceObject.GetDoubleProperty(PropertyDouble.Translucency); }
            set { AceObject.SetDoubleProperty(PropertyDouble.Translucency, value); }
        }

        public AceVector3 Velocity = null;

        public AceVector3 Omega = null;

        public MotionState CurrentMotionState { get; set; }

        public uint? DefaultScriptId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.PhysicsScript); }
            set { AceObject.SetDataIdProperty(PropertyDataId.PhysicsScript, value); }
        }

        public float? DefaultScriptIntensity
        {
            get { return (float?)AceObject.GetDoubleProperty(PropertyDouble.PhysicsScriptIntensity); }
            set { AceObject.SetDoubleProperty(PropertyDouble.PhysicsScriptIntensity, value); }
        }

        // START of Logical Model Data

        public uint? PaletteBaseId
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.PaletteBase); }
            set { AceObject.SetDataIdProperty(PropertyDataId.PaletteBase, value); }
        }

        public uint? ClothingBase
        {
            get { return AceObject.GetDataIdProperty(PropertyDataId.ClothingBase); }
            set { AceObject.SetDataIdProperty(PropertyDataId.ClothingBase, value); }
        }

        private readonly List<ModelPalette> modelPalettes = new List<ModelPalette>();

        private readonly List<ModelTexture> modelTextures = new List<ModelTexture>();

        private readonly List<Model> models = new List<Model>();

        public List<ModelPalette> GetPalettes
        {
            get { return modelPalettes.ToList(); }
        }

        public List<ModelTexture> GetTextures
        {
            get { return modelTextures.ToList(); }
        }

        public List<Model> GetModels
        {
            get { return models.ToList(); }
        }

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

        public void Clear()
        {
            modelPalettes.Clear();
            modelTextures.Clear();
            models.Clear();
        }

        public SequenceManager Sequences { get; }

        protected WorldObject(ObjectGuid guid)
        {
            AceObject = new AceObject();
            AceObject.AceObjectId = guid.Full;
            Guid = guid;

            Sequences = new SequenceManager();
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
        }

        protected WorldObject(AceObject aceObject)
                : this(new ObjectGuid(aceObject.AceObjectId))
        {
            AceObject = aceObject;
            if (aceObject.CurrentMotionState == "0" || aceObject.CurrentMotionState == null)
                CurrentMotionState = null;
            else
                CurrentMotionState = new UniversalMotion(Convert.FromBase64String(aceObject.CurrentMotionState));

            SetPhysicsDescriptionFlag(this);
            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();

            aceObject.AnimationOverrides.ForEach(ao => AddModel(ao.Index, ao.AnimationId));
            aceObject.TextureOverrides.ForEach(to => AddTexture(to.Index, to.OldId, to.NewId));
            aceObject.PaletteOverrides.ForEach(po => AddPalette(po.SubPaletteId, po.Offset, po.Length));
        }

        protected WorldObject(ObjectGuid guid, AceObject aceObject)
            : this(guid)
        {
            Guid = guid;
            AceObject = aceObject;
            if (aceObject.CurrentMotionState == "0" || aceObject.CurrentMotionState == null)
                CurrentMotionState = null;
            else
                CurrentMotionState = new UniversalMotion(Convert.FromBase64String(aceObject.CurrentMotionState));

            SetPhysicsDescriptionFlag(this);
            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();

            aceObject.AnimationOverrides.ForEach(ao => AddModel(ao.Index, ao.AnimationId));
            aceObject.TextureOverrides.ForEach(to => AddTexture(to.Index, to.OldId, to.NewId));
            aceObject.PaletteOverrides.ForEach(po => AddPalette(po.SubPaletteId, po.Offset, po.Length));
        }

        internal void SetInventoryForWorld(WorldObject inventoryItem)
        {
            inventoryItem.Location = Location.InFrontOf(1.1f);
            inventoryItem.PositionFlag = UpdatePositionFlag.Contact
                                         | UpdatePositionFlag.Placement
                                         | UpdatePositionFlag.ZeroQy
                                         | UpdatePositionFlag.ZeroQx;

            inventoryItem.PhysicsDescriptionFlag = inventoryItem.SetPhysicsDescriptionFlag(inventoryItem);
            inventoryItem.ContainerId = null;
            inventoryItem.WielderId = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
        }

        internal void SetInventoryForOffWorld(WorldObject inventoryItem)
        {
            if (inventoryItem.Location != null)
                LandblockManager.RemoveObject(inventoryItem);
            inventoryItem.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Position;
            inventoryItem.PositionFlag = UpdatePositionFlag.None;
            inventoryItem.Location = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
        }

        public void Examine(Session examiner)
        {
            GameEventIdentifyObjectResponse identifyResponse = new GameEventIdentifyObjectResponse(examiner, this);
            examiner.Network.EnqueueSend(identifyResponse);
        }

        public virtual void SerializeUpdateObject(BinaryWriter writer)
        {
            // content of these 2 is the same? TODO: Validate that?
            SerializeCreateObject(writer);
        }

        public WeenieHeaderFlag SetWeenieHeaderFlag()
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

            if (Value != null)
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

            if ((uint)Workmanship != 0u)
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

            return weenieHeaderFlag;
        }

        public WeenieHeaderFlag2 SetWeenieHeaderFlag2()
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

        public virtual void SerializeCreateObject(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);

            SerializeModelData(writer);
            Serialize(this, writer);
            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();
            if (WeenieFlags2 != WeenieHeaderFlag2.None)
                DescriptionFlags |= ObjectDescriptionFlag.IncludesSecondHeader;
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
                writer.Write(Value ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Usable) != 0)
                writer.Write((uint?)Usable ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(UseRadius ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write(TargetType ?? 0u);

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
                writer.Write(Workmanship);

            if ((WeenieFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write(Burden ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((ushort?)Spell ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(HouseOwner ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
            {
                writer.Write(HouseRestrictions ?? 0u);
            }

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
                writer.Write(CooldownId ?? 0u);

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
            Location.Serialize(writer, PositionFlag);
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            writer.Write(Sequences.GetNextSequence(SequenceType.ObjectPosition));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectTeleport));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition));
        }

        /// <summary>
        /// Records some game-logic based desired position update (e.g. teleport), for use by physics engine
        /// </summary>
        /// <param name="newPosition"></param>
        protected void ForceUpdatePosition(Position newPosition)
        {
            ForcedLocation = newPosition;
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
        /// <param name="action"></param>
        /// <returns></returns>
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

        public AceObject NewAceObjectFromCopy()
        {
            return (AceObject)AceObject.Clone(GuidManager.NewItemGuid());
        }

        /// <summary>
        /// Runs all actions pending on this WorldObject
        /// </summary>
        public void RunActions()
        {
            actionQueue.RunActions();
        }

        public PhysicsDescriptionFlag SetPhysicsDescriptionFlag(WorldObject wo)
        {
            var physicsDescriptionFlag = PhysicsDescriptionFlag.None;

            if (CurrentMotionState != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Movement;

            if ((AnimationFrame != null) && (AnimationFrame != 0))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;

            if (Location != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Position;

            // NOTE: While we fill with 0 the flag still has to reflect that we are not really making this entry for the client.
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

            if (ParentId != null)
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
        public void Serialize(WorldObject wo, BinaryWriter writer)
        {
            PhysicsDescriptionFlag = SetPhysicsDescriptionFlag(wo);

            writer.Write((uint)PhysicsDescriptionFlag);

            writer.Write((uint)PhysicsState);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            {
                if (CurrentMotionState != null)
                {
                    var movementData = CurrentMotionState.GetPayload(wo.Guid, wo.Sequences);
                    writer.Write(movementData.Length); // number of bytes in movement object
                    writer.Write(movementData);
                    uint autonomous = CurrentMotionState.IsAutonomous ? (ushort)1 : (ushort)0;
                    writer.Write(autonomous);
                }
                else // create a new current motion state and send it.
                {
                    CurrentMotionState = new UniversalMotion(MotionStance.Standing);
                    var movementData = CurrentMotionState.GetPayload(wo.Guid, wo.Sequences);
                    writer.Write(movementData.Length);
                    writer.Write(movementData);
                    uint autonomous = CurrentMotionState.IsAutonomous ? (ushort)1 : (ushort)0;
                    writer.Write(autonomous);
                }
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.AnimationFrame) != 0)
                writer.Write((AnimationFrame ?? 0u));

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Position) != 0)
                Location.Serialize(writer);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write(MotionTableId ?? 0u);

            // stable_id =  BYTE1(v12) & 8 )  =  8
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.STable) != 0)
                writer.Write(SoundTableId ?? 0u);

            // setup id
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.PeTable) != 0)
                writer.Write(PhysicsTableId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(SetupTableId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Parent) != 0)
            {
                writer.Write((uint)ParentId);
                writer.Write((uint)ParentLocation);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write(Children.Count);
                foreach (var child in Children)
                {
                    writer.Write(child.Guid);
                    writer.Write(1u); // This is going to be child.ParentLocation when we get to it
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
                Debug.Assert(Velocity != null, "Velocity != null");
                // We do a null check above and unset the flag so this has to be good.
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

            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectPosition));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectMovement));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectState));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectVector));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectTeleport));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectServerControl));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectVisualDesc));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));

            writer.Align();
        }
    }
}
