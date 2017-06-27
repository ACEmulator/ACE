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
            get { return (ObjectType)AceObject.ItemType; }
            protected set { AceObject.ItemType = (uint)value; }
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
                log.Debug($"{Name} moved to {PhysicsData.Position}");

                PhysicsData.Position = value;
                */
                if (AceObject.Location != null)
                    LastUpdatedTicks = WorldManager.PortalYearTicks;
                AceObject.Location = value;
                // FIXME(ddevec): When PhysicsData is factored out this can be deleted
                PhysicsData.Position = value;
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

        public ObjectDescriptionFlag DescriptionFlags { get; protected set; }

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

        public ModelData ModelData { get; }

        public PhysicsData PhysicsData { get; }

        // Logical Game Data
        public uint GameDataType
        {
            get { return AceObject.ItemType; }
            set { AceObject.ItemType = value; }
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
            set { this.AceObject.SpellId = (ushort?)value; }
        }
        public uint? ClothingBase
        {
            get { return AceObject.ClothingBase; }
            set { AceObject.ClothingBase = value; }
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

        // WeenieHeaderFlag2
        public uint? Cooldown
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

        public uint? CSetup
        {
            get { return AceObject.ModelTableId; }
            set { AceObject.ModelTableId = value; }
        }

        // apply default for back compat with player object
        public PhysicsDescriptionFlag PhysicsDescriptionFlag { get; set; }

        public PhysicsState PhysicsState
        {
            get { return (PhysicsState)AceObject.PhysicsState; }
            set { AceObject.PhysicsState = (uint)value; }
        }

        public Position Position
        {
            get { return AceObject.Location; }
            set { AceObject.Location = value; }
        }

        public uint? MTableResourceId
        {
            get { return AceObject.MotionTableId; }
            set { AceObject.MotionTableId = value; }
        }
        public uint? SoundsResourceId;
        public uint? Stable
        {
            get { return AceObject.SoundTableId; }
            set { AceObject.SoundTableId = value; }
        }
        public uint? Petable
        {
            get { return AceObject.PhysicsTableId; }
            set { AceObject.PhysicsTableId = value; }
        }

        // these are all related
        public uint ItemsEquipedCount { get; set; }
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

        // these are all related
        public EquipMask? EquipperPhysicsDescriptionFlag;
        private readonly List<EquippedItem> children = new List<EquippedItem>();

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

        public MotionState CurrentMotionState
        {
            get
            {
                if (AceObject.CurrentMotionState == "0" || AceObject.CurrentMotionState == null)
                    return null;
                return new UniversalMotion(Convert.FromBase64String(AceObject.CurrentMotionState));
            }
            set { AceObject.CurrentMotionState = value.ToString(); }
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

        public SequenceManager Sequences { get; }

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            // Kludge until I get everything refactored correctly Og II
            if (AceObject == null)
                AceObject = new AceObject(guid.Full);
            Type = type;
            Guid = guid;

            ModelData = new ModelData();

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

            PhysicsData = new PhysicsData(Sequences);
        }

        protected WorldObject(AceObject aceObject)
                : this((ObjectType)aceObject.ItemType, new ObjectGuid(aceObject.AceObjectId))
        {
            this.AceObject = aceObject;
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
            inventoryItem.Location = PhysicsData.Position.InFrontOf(1.1f);
            inventoryItem.PositionFlag = UpdatePositionFlag.Contact
                                         | UpdatePositionFlag.Placement
                                         | UpdatePositionFlag.ZeroQy
                                         | UpdatePositionFlag.ZeroQx;

            inventoryItem.PhysicsData.PhysicsDescriptionFlag = inventoryItem.PhysicsData.SetPhysicsDescriptionFlag(inventoryItem);
            inventoryItem.ContainerId = null;
            inventoryItem.Wielder = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
        }

        internal void SetInventoryForOffWorld(WorldObject inventoryItem)
        {
            if (inventoryItem.Location != null)
                LandblockManager.RemoveObject(inventoryItem);
            inventoryItem.PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Position;
            inventoryItem.PositionFlag = UpdatePositionFlag.None;
            inventoryItem.PhysicsData.Position = null;
            inventoryItem.Location = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
        }

        public void SetMotionState(MotionState motionState)
        {
            var p = (Player)this;
            PhysicsData.CurrentMotionState = motionState;
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

            if ((Cooldown != null) && (Cooldown != 0))
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

            ModelData.Serialize(writer);
            PhysicsData.Serialize(this, writer);
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
                writer.Write(Cooldown ?? 0u);

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
            ModelData.Serialize(writer);
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            writer.Write(Sequences.GetNextSequence(SequenceType.ObjectPosition));
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
    }
}
