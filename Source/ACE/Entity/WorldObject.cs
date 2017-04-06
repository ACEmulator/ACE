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

    public abstract class WorldObject
    {
        public ObjectGuid Guid { get; private set; }

        public ObjectType Type { get; protected set; }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public ushort WeenieClassid { get; protected set; }

        public ushort Icon { get; set; }

        public string Name { get; protected set; }

        /// <summary>
        /// Default False equal to Never Die.
        /// </summary>
        public bool DieFlag = false;
        public DateTime CreatedOn { get; private set; }
        public DateTime DieOn { get; private set; }

        /// <summary>
        /// tick-stamp for the last time this object changed in any way.
        /// </summary>
        public double LastUpdatedTicks { get; set; }

        public virtual Position Position
        {
            get { return PhysicsData.Position; }
            protected set { PhysicsData.Position = value; }
        }

        public ObjectDescriptionFlag DescriptionFlags { get; protected set; }

        public WeenieHeaderFlag WeenieFlags { get; protected set; }

        public WeenieHeaderFlag2 WeenieFlags2 { get; protected set; }

        public UpdatePositionFlag PositionFlag { get; protected set; } = UpdatePositionFlag.Contact;

        public virtual void PlayScript(Session session) { }

        public ushort MovementIndex
        {
            get { return PhysicsData.PositionSequence; }
            set { PhysicsData.PositionSequence = value; }
        }

        public ushort TeleportIndex
        {
            get { return PhysicsData.PortalSequence; }
            set { PhysicsData.PortalSequence = value; }
        }

        public ushort ForcePositionIndex
        {
            get { return PhysicsData.ForcePositionSequence; }
            set { PhysicsData.ForcePositionSequence = value; }
        }

        private bool IsContainer { get; set; } = false;

        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        private readonly object inventoryMutex = new object();

        public virtual float ListeningRadius { get; protected set; } = 5f;

        public ModelData ModelData { get; private set; }

        public PhysicsData PhysicsData { get; private set; }

        public GameData GameData { get; private set; }

        public SequenceManager Sequences { get; private set; }

        protected WorldObject(ObjectType type, ObjectGuid guid, double secondstolive)
        {
            Initialize(type, guid);
            // calculate what time the word obect will on expire.
            if (secondstolive > 0)
            {
                DieOn = DateTime.Now.AddSeconds(secondstolive);
                DieFlag = true;
            }
        }

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            Initialize(type, guid);
        }

        protected void Initialize(ObjectType type, ObjectGuid guid)
        {
            CreatedOn = DateTime.Now;

            Type = type;
            Guid = guid;

            GameData = new GameData();
            ModelData = new ModelData();
            PhysicsData = new PhysicsData();

            Sequences = new SequenceManager();
            Sequences.AddSequence(SequenceType.MotionMessage, new UShortSequence(2));
            Sequences.AddSequence(SequenceType.MotionMessageAutonomous, new UShortSequence(2));
            Sequences.AddSequence(SequenceType.Motion, new UShortSequence(1));
        }

        public void AddToInventory(WorldObject worldObject)
        {
            lock (this.inventoryMutex)
            {
                if (inventory.ContainsKey(worldObject.Guid))
                    return;

                inventory.Add(worldObject.Guid, worldObject);
            }
        }

        public void RemoveFromInventory(ObjectGuid objectGuid)
        {
            lock (inventoryMutex)
            {
                if (inventory.ContainsKey(objectGuid))
                    inventory.Remove(objectGuid);
            }
        }

        /// <summary>
        /// This is used to do the housekeeping on the server side to take an object from inventory into 3D world then tell the world
        /// </summary>
        public void HandleDropItem(ObjectGuid objectGuid, Session session)
        {
            lock (this.inventoryMutex)
            {
                // Find the item in inventory
                if (!this.inventory.ContainsKey(objectGuid)) return;
                var obj = this.inventory[objectGuid];

                // We are droping the item - let's keep track of change in burden
                this.GameData.Burden -= obj.GameData.Burden;

                // Set the flags and determine a position.

                // TODO: I need to look at the PCAPS to see the delta in position from the dropper and the item dropped.   Temp position.
                obj.PositionFlag = UpdatePositionFlag.Contact | UpdatePositionFlag.Placement |
                   UpdatePositionFlag.ZeroQy | UpdatePositionFlag.ZeroQx;
                obj.PhysicsData.Position = PhysicsData.Position.InFrontOf(1.50f);

                // TODO: need to find out if these are needed or if there is a better way to do this. This probably should have been set at object creation Og II
                obj.GameData.ContainerId = 0;
                obj.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Position;
                obj.PhysicsData.PhysicsState = PhysicsState.Gravity;
                obj.GameData.RadarBehavior = RadarBehavior.ShowAlways;
                obj.GameData.RadarColour = RadarColor.White;

                // Let the client know our response.
                session.Network.EnqueueSend(new GameMessageUpdatePosition(obj));

                // Tell the landblock so it can tell everyone around what just hit the ground.
                LandblockManager.AddObject(obj);
                // Remove from the inventory list.
                this.inventory.Remove(objectGuid);

                // OK, now let's tell the world and our client what we have done.
                var targetContainer = new ObjectGuid(0);
                session.Network.EnqueueSend(
                   new GameMessagePrivateUpdatePropertyInt(session,
                       PropertyInt.EncumbVal,
                       (uint)session.Player.GameData.Burden));
                var movement1 = new MovementData { ForwardCommand = 24, MovementStateFlag = MovementStateFlag.ForwardCommand };
                session.Network.EnqueueSend(new GameMessageMotion(session.Player, session, MotionAutonomous.False, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing, movement1));

                // Set Container id to 0 - you are free
                session.Network.EnqueueSend(
                    new GameMessageUpdateInstanceId(objectGuid, targetContainer));

                var movement2 = new MovementData { ForwardCommand = 0, MovementStateFlag = MovementStateFlag.NoMotionState };
                session.Network.EnqueueSend(new GameMessageMotion(session.Player, session, MotionAutonomous.False, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing, movement2));

                // Ok, we can do the last 3 steps together.   Not sure if it is better to break this stuff our for clarity
                // Put the darn thing in 3d space
                // Make the thud sound
                // Send the container update again.   I have no idea why, but that is what they did in live.
                session.Network.EnqueueSend(
                    new GameMessagePutObjectIn3d(session, session.Player, objectGuid),
                    new GameMessageSound(session.Player.Guid, Sound.DropItem, (float)1.0),
                    new GameMessageUpdateInstanceId(objectGuid, targetContainer));
            }
        }

        /// <summary>
        /// This is used to do the housekeeping on the server side to take an object from inventory into 3D world then tell the world
        /// </summary>
        public void HandlePutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid,  Session session)
        {
            lock (this.inventoryMutex)
            {
                // Find the item we want to pick up
                var obj = LandblockManager.GetWorldObject(session, itemGuid);
                if (obj == null)
                {
                    return;
                    // TODO: yea, this is probably not how you do this
                }
                // We are picking up the  the item - let's keep track of change in burden
                // TODO: Add check for too encumbered and send "You are too encumbered to pick that up.  Also check pack capacity
                this.GameData.Burden += obj.GameData.Burden;

                // let's move to pick up the item
                this.PositionFlag = UpdatePositionFlag.Contact |
                   UpdatePositionFlag.ZeroQy | UpdatePositionFlag.ZeroQx;
                this.Position.PositionX = obj.Position.PositionX;
                this.Position.PositionY = obj.Position.PositionY;
                this.Position.PositionZ = obj.Position.PositionZ;
                this.Position.RotationW = obj.Position.RotationW;
                this.Position.RotationZ = obj.Position.RotationZ;

                session.Network.EnqueueSend(new GameMessageUpdatePosition(this));
                // TODO: Finish out pick up item
            }
        }

        public void GetInventoryItem(ObjectGuid objectGuid, out WorldObject worldObject)
        {
            lock (inventoryMutex)
            {
                inventory.TryGetValue(objectGuid, out worldObject);
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
            PhysicsData.Serialize(writer);

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
            Position.Serialize(writer, PositionFlag);

            var player = Guid.IsPlayer() ? this as Player : null;
            writer.Write((ushort)(player?.TotalLogins ?? 1)); // instance sequence
            writer.Write((ushort)++MovementIndex);
            writer.Write((ushort)TeleportIndex);
            writer.Write((ushort)ForcePositionIndex);
        }
    }
}