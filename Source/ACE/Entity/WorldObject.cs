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

        public ObjectType Type
        {
            get { return (ObjectType)AceObject.Type; }
            protected set { AceObject.Type = (uint)value; }
        }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public uint WeenieClassid
        {
            get { return AceObject.WeenieClassId; }
            protected set { AceObject.WeenieClassId = value; }
        }

        public uint? Icon
        {
            get { return AceObject.IconId; }
            set { AceObject.IconId = value; }
        }

        public string Name
        {
            get { return AceObject.Name; }
            protected set { AceObject.Name = value; }
        }

        public IActor CurrentParent { get; private set; }

        public Position ForcedLocation { get; private set; }
        public Position RequestedLocation { get; private set; }

        /// <summary>
        /// Should only be adjusted by LandblockManager -- default is null
        /// </summary>
        public Landblock CurrentLandblock
        {
            get
            {
                return CurrentParent as Landblock;
            }
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
            get { return AceObject.Location; }
            set
            {
                /*
                log.Debug($"{Name} moved to {Position}");

                Position = value;
                */
                if (AceObject.Location != null)
                    LastUpdatedTicks = WorldManager.PortalYearTicks;
                AceObject.Location = value;
                // FIXME(ddevec): When PhysicsData is factored out this can be deleted
                Position = value;
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

        public CombatMode CombatMode { get; private set; }

        public virtual void PlayScript(Session session) { }

        public virtual float ListeningRadius { get; protected set; } = 5f;

        // Logical Game Data
        public uint GameDataType
        {
            get { return AceObject.Type; }
            set { AceObject.Type = value; }
        }

        public ContainerType ContainerType
        {
            get
            {
                if (ItemCapacity == null || ItemCapacity == 0)
                {
                    if (Name.Contains("Foci"))
                        return ContainerType.Foci;
                    return ContainerType.NonContainer;
                }
                return ContainerType.Conatiner;
            }
        }

        public string NamePlural { get; set; }

        public byte? ItemCapacity
        {
            get { return AceObject.ItemsCapacity; }
            set { AceObject.ItemsCapacity = value; }
        }

        public byte? ContainerCapacity
        {
            get { return AceObject.ContainersCapacity; }
            set { AceObject.ContainersCapacity = value; }
        }

        public AmmoType? AmmoType
        {
            get { return (AmmoType?)AceObject.AmmoType; }
            set { AceObject.AmmoType = (uint?)value; }
        }

        public uint? Value
        {
            get { return AceObject.Value; }
            set { AceObject.Value = value; }
        }

        public Usable? Usable
        {
            get { return (Usable?)AceObject.ItemUseable; }
            set { AceObject.ItemUseable = (uint?)value; }
        }

        // FIXME(ddevec): Defaults to 25, so is unnecessarily sent always w/ weenieheaderflags.
        public float? UseRadius
        {
            get { return AceObject.UseRadius ?? 0.25f; }
            set { AceObject.UseRadius = value; }
        }

        public uint? TargetType
        {
            get { return AceObject.TargetTypeId; }
            set { AceObject.TargetTypeId = value; }
        }

        public UiEffects? UiEffects
        {
            get { return (UiEffects?)AceObject.UiEffects; }
            set { AceObject.UiEffects = (uint?)value; }
        }

        public CombatUse? CombatUse
        {
            get { return (CombatUse?)AceObject.CombatUse; }
            set { AceObject.CombatUse = (byte?)value; }
        }

        /// <summary>
        /// This is used to indicate the number of uses remaining.  Example 32 uses left out of 50 (MaxStructure)
        /// </summary>
        public ushort? Structure
        {
            get { return AceObject.Structure; }
            set { AceObject.Structure = value; }
        }

        /// <summary>
        /// Use Limit - example 50 use healing kit
        /// </summary>
        public ushort? MaxStructure
        {
            get { return AceObject.MaxStructure; }
            set { AceObject.MaxStructure = value; }
        }

        public ushort? StackSize
        {
            get { return AceObject.StackSize; }
            set { AceObject.StackSize = value; }
        }

        public ushort? MaxStackSize
        {
            get { return AceObject.MaxStackSize; }
            set { AceObject.MaxStackSize = value; }
        }

        public uint? ContainerId
        {
            get { return AceObject.ContainerId; }
            set { AceObject.ContainerId = value; }
        }

        // TODO: I think we need to store this in aceObject from pacps - example Paul's Axe (Blue Ox)
        public uint? Wielder
        {
            get { return AceObject.WielderId; }
            set { AceObject.WielderId = value; }
        }

        public EquipMask? ValidLocations
        {
            get { return (EquipMask?)AceObject.ValidLocations; }
            set { AceObject.ValidLocations = (uint?)value; }
        }

        /// <summary>
        /// Location - renamed for conflict with wo and to have a more descriptive name
        /// </summary>
        public EquipMask? CurrentWieldedLocation
        {
            get { return (EquipMask?)AceObject.CurrentWieldedLocation; }
            set { AceObject.CurrentWieldedLocation = (uint?)value; }
        }

        public CoverageMask? Priority
        {
            get { return (CoverageMask?)AceObject.Priority; }
            set { AceObject.Priority = (uint?)value; }
        }

        public RadarColor? RadarColor
        {
            get { return (RadarColor?)AceObject.BlipColor; }
            set { AceObject.BlipColor = (byte?)value; }
        }

        public RadarBehavior? RadarBehavior
        {
            get { return (RadarBehavior?)AceObject.Radar; }
            set { AceObject.Radar = (byte?)value; }
        }

        public ushort? Script
        {
            get { return AceObject.PhysicsScript; }
            set { AceObject.PhysicsScript = value; }
        }

        public float Workmanship
        {
            get { return AceObject.Workmanship; }
            set { AceObject.Workmanship = value; }
        }

        public ushort? Burden
        {
            get { return AceObject.Burden; }
            set { AceObject.Burden = value; }
        }

        public Spell? Spell
        {
            get { return (Spell?)AceObject.SpellId; }
            set { AceObject.SpellId = (ushort?)value; }
        }

        /// <summary>
        /// Housing links to another packet, that needs sent.. The HouseRestrictions ACL Control list that contains all the housing data
        /// </summary>
        public uint? HouseOwner { get; set; }

        public uint? HouseRestrictions { get; set; }

        public ushort? HookItemTypes
        {
            get { return AceObject.HookItemTypes; }
            set { AceObject.HookItemTypes = value; }
        }

        public uint? Monarch { get; set; }

        public ushort? HookType
        {
            get { return AceObject.HookType; }
            set { AceObject.HookType = value; }
        }

        public uint? IconOverlay
        {
            get { return AceObject.IconOverlayId; }
            set { AceObject.IconOverlayId = value; }
        }

        public uint? IconUnderlay
        {
            get { return AceObject.IconUnderlayId; }
            set { AceObject.IconUnderlayId = value; }
        }

        public Material? Material
        {
            get { return (Material?)AceObject.MaterialType; }
            set { AceObject.MaterialType = (byte?)value; }
        }

        public uint? PetOwner { get; set; }

       public uint? CooldownId
        {
            get { return AceObject.CooldownId; }
            set { AceObject.CooldownId = value; }
        }

        public double? CooldownDuration
        {
            get { return AceObject.CooldownDuration; }
            set { AceObject.CooldownDuration = value; }
        }

        // PhysicsData Logical
        /// <summary>
        /// setup_id in aclogviewer - used to get the correct model out of the dat file
        /// </summary>
        public uint? SetupTableId
        {
            get { return AceObject.SetupTableId; }
            set { AceObject.SetupTableId = value; }
        }

        public PhysicsDescriptionFlag PhysicsDescriptionFlag { get; set; }

        public PhysicsState PhysicsState
        {
            get { return (PhysicsState)AceObject.PhysicsState; }
            set { AceObject.PhysicsState = (uint)value; }
        }

        /// <summary>
        /// This is my physical location in the world
        /// </summary>
        public Position Position
        {
            get { return AceObject.Location; }
            set { AceObject.Location = value; }
        }
        /// <summary>
        /// mtable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint? MotionTableId
        {
            get { return AceObject.MotionTableId; }
            set { AceObject.MotionTableId = value; }
        }
        /// <summary>
        /// stable_id in aclogviewer This is the sound table for the object.   Looked up from dat file.
        /// </summary>
        public uint? SoundTableId
        {
            get { return AceObject.SoundTableId; }
            set { AceObject.SoundTableId = value; }
        }
        /// <summary>
        /// phstable_id in aclogviewer This is the physics table for the object.   Looked up from dat file.
        /// </summary>
        public uint? PhisicsTableId
        {
            get { return AceObject.PhysicsTableId; }
            set { AceObject.PhysicsTableId = value; }
        }

        public uint ItemsEquipedCount { get; set; }
        /// <summary>
        /// This is used for equiped items that are selectable.   Weapons or shields only.   Max 2
        /// </summary>
        public uint? Parent
        {
            get { return AceObject.Parent; }
            set { AceObject.Parent = value; }
        }

        public uint? ParentLocation
        {
            get { return AceObject.ParentLocation; }
            set { AceObject.ParentLocation = value; }
        }

        public EquipMask? EquipperPhysicsDescriptionFlag;

        public List<EquippedItem> Children { get; } = new List<EquippedItem>();

        public float? ObjScale
        {
            get { return AceObject.DefaultScale; }
            set { AceObject.DefaultScale = value; }
        }

        public float? Friction
        {
            get { return AceObject.Friction; }
            set { AceObject.Friction = value; }
        }

        public float? Elasticity
        {
            get { return AceObject.Elasticity; }
            set { AceObject.Elasticity = value; }
        }

        public uint? AnimationFrame
        {
            get { return AceObject.AnimationFrameId; }
            set { AceObject.AnimationFrameId = value; }
        }

        public AceVector3 Acceleration { get; set; }

        public float? Translucency
        {
            get { return AceObject.Translucency; }
            set { AceObject.Translucency = value; }
        }

        public AceVector3 Velocity = null;

        public AceVector3 Omega = null;

        private MotionState currentMotionState;

        public MotionState CurrentMotionState
        {
            get { return currentMotionState; }
            set { currentMotionState = value; }
        }

        public uint? DefaultScript
        {
            get { return AceObject.DefaultScript; }
            set { AceObject.DefaultScript = value; }
        }

        public float? DefaultScriptIntensity
        {
            get { return AceObject.PhysicsScriptIntensity; }
            set { AceObject.PhysicsScriptIntensity = value; }
        }

        // START of Logical Model Data

        public uint? PaletteGuid
        {
            get { return AceObject.PaletteId; }
            set { AceObject.PaletteId = value; }
        }

        public uint? ClothingBase
        {
            get { return AceObject.ClothingBase; }
            set { AceObject.ClothingBase = value; }
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
            var newmodel = new Model(index, modelresourceid);
            models.Add(newmodel);
        }

        public void Clear()
        {
            modelPalettes.Clear();
            modelTextures.Clear();
            models.Clear();
        }

        public SequenceManager Sequences { get; }

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            AceObject = new AceObject();
            Type = type;
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
            Sequences.AddOrSetSequence(SequenceType.Motion, new UShortSequence(1));
        }

        protected WorldObject(AceObject aceObject)
                : this((ObjectType)aceObject.Type, new ObjectGuid(aceObject.AceObjectId))
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
            PaletteGuid = aceObject.PaletteId;
        }

        protected WorldObject(ObjectGuid guid, AceObject aceObject)
            : this((ObjectType)aceObject.Type, guid)
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
            PaletteGuid = aceObject.PaletteId;
        }

        public void SetCombatMode(CombatMode newCombatMode)
        {
            log.InfoFormat("Changing combat mode for {0} to {1}", this.Guid, newCombatMode);
            // TODO: any sort of validation
            CombatMode = newCombatMode;
            switch (CombatMode)
            {
                case CombatMode.Peace:
                    SetMotionState(new UniversalMotion(MotionStance.Standing));
                    break;
                case CombatMode.Melee:
                    var gm = new UniversalMotion(MotionStance.UANoShieldAttack);
                    gm.MovementData.CurrentStyle = (ushort)MotionStance.UANoShieldAttack;
                    SetMotionState(gm);
                    break;
            }
        }

        internal void SetInventoryForWorld(WorldObject inventoryItem)
        {
            inventoryItem.Location = Position.InFrontOf(1.1f);
            inventoryItem.PositionFlag = UpdatePositionFlag.Contact
                                         | UpdatePositionFlag.Placement
                                         | UpdatePositionFlag.ZeroQy
                                         | UpdatePositionFlag.ZeroQx;

            inventoryItem.PhysicsDescriptionFlag = inventoryItem.SetPhysicsDescriptionFlag(inventoryItem);
            inventoryItem.ContainerId = null;
            inventoryItem.Wielder = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
        }

        internal void SetInventoryForOffWorld(WorldObject inventoryItem)
        {
            if (inventoryItem.Location != null)
                LandblockManager.RemoveObject(inventoryItem);
            inventoryItem.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Position;
            inventoryItem.PositionFlag = UpdatePositionFlag.None;
            inventoryItem.Position = null;
            inventoryItem.Location = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
        }

        public void SetMotionState(MotionState motionState)
        {
            var p = (Player)this;
            CurrentMotionState = motionState;
            var updateMotion = new GameMessageUpdateMotion(p.Guid,
                                                           p.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                                                           p.Sequences, motionState);
            p.Session.Network.EnqueueSend(updateMotion);
        }

        public void Examine(Session examiner)
        {
            var identifyResponse = new GameEventIdentifyObjectResponse(examiner, Guid, this);
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
                weenieHeaderFlag |= WeenieHeaderFlag.ItemCapacity;

            if (ContainerCapacity != null)
                weenieHeaderFlag |= WeenieHeaderFlag.ContainerCapacity;

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

            if (Wielder != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Wielder;

            if (ValidLocations != null)
                weenieHeaderFlag |= WeenieHeaderFlag.ValidLocations;

            // You can't be in a wielded location if you don't have a weilder.   This is a gurad against crap data. Og II
            if ((CurrentWieldedLocation != null) && (CurrentWieldedLocation != 0) && (Wielder != null) && (Wielder != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.CurrentlyWieldedLocation;

            if (Priority != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Priority;

            if (RadarColor != null)
                weenieHeaderFlag |= WeenieHeaderFlag.RadarBlipColor;

            if (RadarBehavior != null)
                weenieHeaderFlag |= WeenieHeaderFlag.RadarBehavior;

            if ((Script != null) && (Script != 0u))
                weenieHeaderFlag |= WeenieHeaderFlag.Script;

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

            if (HookItemTypes != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HookItemTypes;

            if (Monarch != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Monarch;

            if (HookType != null)
                weenieHeaderFlag |= WeenieHeaderFlag.HookType;

            if ((IconOverlay != null) && (IconOverlay != 0))
                weenieHeaderFlag |= WeenieHeaderFlag.IconOverlay;

            if (Material != null)
                weenieHeaderFlag |= WeenieHeaderFlag.Material;

            return weenieHeaderFlag;
        }

        public WeenieHeaderFlag2 SetWeenieHeaderFlag2()
        {
            var weenieHeaderFlag2 = WeenieHeaderFlag2.None;

            if ((IconUnderlay != null) && (IconUnderlay != 0))
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
                DescriptionFlags |= ObjectDescriptionFlag.AdditionFlags;
            writer.Write((uint)WeenieFlags);
            writer.WriteString16L(Name);
            writer.WritePackedDword(WeenieClassid);
            writer.WritePackedDwordOfKnownType(Icon ?? 0, 0x6000000);
            writer.Write((uint)Type);
            writer.Write((uint)DescriptionFlags);
            writer.Align();

            if ((DescriptionFlags & ObjectDescriptionFlag.AdditionFlags) != 0)
                writer.Write((uint)WeenieFlags2);

            if ((WeenieFlags & WeenieHeaderFlag.PluralName) != 0)
                writer.WriteString16L(NamePlural);

            if ((WeenieFlags & WeenieHeaderFlag.ItemCapacity) != 0)
                writer.Write(ItemCapacity ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.ContainerCapacity) != 0)
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
                writer.Write(Structure ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write(MaxStructure ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write(StackSize ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write(MaxStackSize ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(ContainerId ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(Wielder ?? 0u);

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

            if ((WeenieFlags & WeenieHeaderFlag.Script) != 0)
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
                writer.Write(HookItemTypes ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(Monarch ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HookType) != 0)
                writer.Write(HookType ?? 0);

            if ((WeenieFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.WritePackedDwordOfKnownType((IconOverlay ?? 0), 0x6000000);

            if ((WeenieFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.WritePackedDwordOfKnownType((IconUnderlay ?? 0), 0x6000000);

            if ((WeenieFlags & WeenieHeaderFlag.Material) != 0)
                writer.Write((uint)(Material ?? 0u));

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

            if ((modelPalettes.Count > 0) && (PaletteGuid != null))
                writer.WritePackedDwordOfKnownType((uint)PaletteGuid, 0x4000000);
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

            if (Position != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Position;

            // NOTE: While we fill with 0 the flag still has to reflect that we are not really making this entry for the client.
            if (MotionTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.MTable;

            if (SoundTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Stable;

            if (PhisicsTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Petable;

            if (SetupTableId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.CSetup;

            if (ItemsEquipedCount != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Children;

            if (Parent != null)
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

            if (DefaultScript != null)
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
                Position.Serialize(writer);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write(MotionTableId ?? 0u);

            // stable_id =  BYTE1(v12) & 8 )  =  8
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Stable) != 0)
                writer.Write(SoundTableId ?? 0u);

            // setup id
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Petable) != 0)
                writer.Write(PhisicsTableId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(SetupTableId ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Parent) != 0)
            {
                writer.Write((uint)Parent);
                writer.Write((uint)ParentLocation);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write(ItemsEquipedCount);
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
                writer.Write(DefaultScript ?? 0u);

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
