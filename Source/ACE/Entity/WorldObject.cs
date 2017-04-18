using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;
using ACE.Network.Sequence;
using System.IO;

namespace ACE.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Messaging;

    using global::ACE.Entity.Enum.Properties;
    using global::ACE.Managers;
    using Network.Motion;
    using log4net;

    public abstract class WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ObjectGuid Guid { get; }

        public ObjectType Type { get; protected set; }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public ushort WeenieClassid { get; protected set; }

        public ushort Icon { get; set; }

        public string Name { get; protected set; }

        /// <summary>
        /// tick-stamp for the last time this object changed in any way.
        /// </summary>
        public double LastUpdatedTicks { get; set; }

        public virtual Position Location
        {
            get { return PhysicsData.Position; }
            protected set { PhysicsData.Position = value; }
        }

        public ObjectDescriptionFlag DescriptionFlags { get; protected set; }

        public WeenieHeaderFlag WeenieFlags { get; protected set; }

        public WeenieHeaderFlag2 WeenieFlags2 { get; protected set; }

        public UpdatePositionFlag PositionFlag { get; protected set; } 

        public CombatMode CombatMode { get; private set; }

        public virtual void PlayScript(Session session) { }

        private bool IsContainer { get; set; } = false;

        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        private readonly object inventoryMutex = new object();

        public virtual float ListeningRadius { get; protected set; } = 5f;

        public ModelData ModelData { get; }

        public PhysicsData PhysicsData { get; }

        public GameData GameData { get; }

        public SequenceManager Sequences { get; }

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            Type = type;
            Guid = guid;

            GameData = new GameData();
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

        public void SetCombatMode(CombatMode newCombatMode)
        {
            log.InfoFormat("Changing combat mode for {0} to {1}", this.Guid, newCombatMode);
            // TODO: any sort of validation
            CombatMode = newCombatMode;
            switch (CombatMode)
            {
                case CombatMode.Peace:
                    SetMotionState(new GeneralMotion(MotionStance.Standing));
                    break;
                case CombatMode.Melee:
                    var gm = new GeneralMotion(MotionStance.UANoShieldAttack);
                    gm.MovementData.CurrentStyle = (ushort)MotionStance.UANoShieldAttack;
                    SetMotionState(gm);
                    break;
            }
        }

        public void SetMotionState(MotionState motionState)
        {
            Player p = (Player)this;
            PhysicsData.CurrentMotionState = motionState;
            var updateMotion = new GameMessageUpdateMotion(this, p.Session, motionState);
            p.Session.Network.EnqueueSend(updateMotion);
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem)
        {
            lock (inventoryMutex)
            {
                if (!inventory.ContainsKey(inventoryItem.Guid))
                    inventory.Add(inventoryItem.Guid, inventoryItem);
                GameData.Burden += inventoryItem.GameData.Burden;
                inventoryItem.PositionFlag = UpdatePositionFlag.Contact | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.ZeroQx;
                inventoryItem.GameData.ContainerId = Guid.Full;
                inventoryItem.PhysicsData.PhysicsDescriptionFlag &= PhysicsDescriptionFlag.Position;
            }
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            var inventoryItem = GetInventoryItem(inventoryItemGuid);
            GameData.Burden -= inventoryItem.GameData.Burden;
            inventoryItem.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable
                                                                | PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.AnimationFrame
                                                                | PhysicsDescriptionFlag.Position;
            inventoryItem.PositionFlag = UpdatePositionFlag.Contact
                                           | UpdatePositionFlag.Placement
                                           | UpdatePositionFlag.ZeroQy
                                           | UpdatePositionFlag.ZeroQx;
            inventoryItem.PhysicsData.Position = PhysicsData.Position.InFrontOf(0.50f);
            inventoryItem.GameData.ContainerId = 0;
            inventoryItem.GameData.Wielder = 0;

            lock (inventoryMutex)
            {
                if (this.inventory.ContainsKey(inventoryItemGuid))                    
                    this.inventory.Remove(inventoryItemGuid);
            }
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            lock (inventoryMutex)
            {
                if (this.inventory.ContainsKey(objectGuid))
                    return this.inventory[objectGuid];
                else
                    return null;                
            }
        }
 
        public virtual void SerializeUpdateObject(BinaryWriter writer)
        {
            // content of these 2 is the same? TODO: Validate that?
            SerializeCreateObject(writer);
        }

        public virtual void SerializeCreateObject(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);

            ModelData.Serialize(writer);
            PhysicsData.Serialize(this, writer);

            writer.Write((uint)WeenieFlags);
            writer.WriteString16L(Name);
            writer.Write((ushort)WeenieClassid);
            writer.Write((ushort)Icon);
            writer.Write((uint)Type);
            writer.Write((uint)DescriptionFlags);

            if ((DescriptionFlags & ObjectDescriptionFlag.AdditionFlags) != 0)
                writer.Write((uint)WeenieFlags2);

            if ((WeenieFlags & WeenieHeaderFlag.PuralName) != 0)
                writer.WriteString16L(GameData.NamePlural);

            if ((WeenieFlags & WeenieHeaderFlag.ItemCapacity) != 0)
                writer.Write(GameData.ItemCapacity);

            if ((WeenieFlags & WeenieHeaderFlag.ContainerCapacity) != 0)
                writer.Write(GameData.ContainerCapacity);

            if ((WeenieFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort)GameData.AmmoType);

            if ((WeenieFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write(GameData.Value);

            if ((WeenieFlags & WeenieHeaderFlag.Usable) != 0)
                writer.Write((uint)GameData.Usable);

            if ((WeenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(GameData.UseRadius);

            if ((WeenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write((uint)GameData.TargetType);

            if ((WeenieFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write((uint)GameData.UiEffects);

            if ((WeenieFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((byte)GameData.CombatUse);

            if ((WeenieFlags & WeenieHeaderFlag.Struture) != 0)
                writer.Write((ushort)GameData.Struture);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write((ushort)GameData.MaxStructure);

            if ((WeenieFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write(GameData.StackSize);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write(GameData.MaxStackSize);

            if ((WeenieFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(GameData.ContainerId);

            if ((WeenieFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(GameData.Wielder);

            if ((WeenieFlags & WeenieHeaderFlag.ValidLocations) != 0)
                writer.Write((uint)GameData.ValidLocations);

            if ((WeenieFlags & WeenieHeaderFlag.Location) != 0)
                writer.Write((uint)GameData.Location);

            if ((WeenieFlags & WeenieHeaderFlag.Priority) != 0)
                writer.Write((uint)GameData.Priority);
  
            if ((WeenieFlags & WeenieHeaderFlag.BlipColour) != 0)
                writer.Write((byte)GameData.RadarColour);

            if ((WeenieFlags & WeenieHeaderFlag.Radar) != 0)
                writer.Write((byte)GameData.RadarBehavior);

            if ((WeenieFlags & WeenieHeaderFlag.Script) != 0)
                writer.Write(GameData.Script);

            if ((WeenieFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write(GameData.Workmanship);

            if ((WeenieFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write(GameData.Burden);

            if ((WeenieFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((uint)GameData.Spell);

            if ((WeenieFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(GameData.HouseOwner);

            /*if ((WeenieFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
            {
            }*/

            if ((WeenieFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write(GameData.HookItemTypes);

            if ((WeenieFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(GameData.Monarch);

            if ((WeenieFlags & WeenieHeaderFlag.HookType) != 0)
                writer.Write(GameData.HookType);

            if ((WeenieFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.Write(GameData.IconOverlay);

            /*if ((WeenieFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.Write((ushort)0);*/

            if ((WeenieFlags & WeenieHeaderFlag.Material) != 0)
                writer.Write((uint)GameData.Material);

            /*if ((WeenieFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                writer.Write(0u);*/

            /*if ((WeenieFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                writer.Write(0.0d);*/

            /*if ((WeenieFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
                writer.Write(0u);*/

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
    }
}
