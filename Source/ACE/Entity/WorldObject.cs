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

        public UpdatePositionFlag PositionFlag { get; protected set; } = UpdatePositionFlag.Contact;

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
            WorldObject obj;
            lock (this.inventoryMutex)
            {
                // Find the item in inventory
                if (!this.inventory.ContainsKey(objectGuid)) return;
                obj = this.inventory[objectGuid];

                // Remove from the inventory list.
                this.inventory.Remove(objectGuid);
            }

            // We are droping the item - let's keep track of change in burden
            this.GameData.Burden -= obj.GameData.Burden;

            // OK, now let's tell the world and our client what we have done.
            var targetContainer = new ObjectGuid(0);
            session.Network.EnqueueSend(
                new GameMessagePrivateUpdatePropertyInt(session,
                    PropertyInt.EncumbVal,
                    (uint)session.Player.GameData.Burden));
            GeneralMotion motion = new GeneralMotion(MotionStance.Standing, new MotionItem((MotionCommand)MovementTypes.General));
            motion.MovementData.ForwardCommand = 24;
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player, session, motion));

            // Set Container id to 0 - you are free
            session.Network.EnqueueSend(
                new GameMessageUpdateInstanceId(objectGuid, targetContainer));
                        
            motion.MovementData.ForwardCommand = 0;
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player, session, motion));

            // Ok, we can do the last 3 steps together.   Not sure if it is better to break this stuff our for clarity
            // Put the darn thing in 3d space
            // Make the thud sound
            // Send the container update again.   I have no idea why, but that is what they did in live.
            session.Network.EnqueueSend(
                new GameMessagePutObjectIn3d(session, session.Player, objectGuid),
                new GameMessageSound(session.Player.Guid, Sound.DropItem, (float)1.0),
                new GameMessageUpdateInstanceId(objectGuid, targetContainer));
            // Set the flags and determine a position.

            // TODO: I need to look at the PCAPS to see the delta in position from the dropper and the item dropped.   Temp position.
            obj.PositionFlag = UpdatePositionFlag.Contact | UpdatePositionFlag.Placement |
                UpdatePositionFlag.ZeroQy | UpdatePositionFlag.ZeroQx;
            obj.PhysicsData.Position = PhysicsData.Position.InFrontOf(0.50f);

            // TODO: need to find out if these are needed or if there is a better way to do this. This probably should have been set at object creation Og II
            obj.GameData.ContainerId = 0;
            obj.GameData.Wielder = 0;

            // Tell the landblock so it can tell everyone around what just hit the ground.
            // This is the sequence magic - adds back into 3d space seem to be treated like teleports.   
            obj.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            obj.Sequences.GetNextSequence(SequenceType.ObjectVector);
            LandblockManager.AddObject(obj);

            // Let the client know our response.
            session.Network.EnqueueSend(new GameMessageUpdatePosition(obj));
        }

        /// <summary>
        /// This is used to do the housekeeping on the server side to take an object from inventory into 3D world then tell the world
        /// </summary>
        public void HandlePutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid,  Session session)
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

            obj.PositionFlag = UpdatePositionFlag.Contact | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.ZeroQx;            

            session.Network.EnqueueSend(new GameMessageUpdatePosition(this));

            // Bend over and pick that puppy up.
            GeneralMotion motion = new GeneralMotion(MotionStance.Standing, new MotionItem((MotionCommand)MovementTypes.General));
            motion.MovementData.ForwardCommand = 24;

            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player, session, motion));
            session.Network.EnqueueSend(
                new GameMessageSound(session.Player.Guid, Sound.PickUpItem, (float)1.0));

            // Add to the inventory list.
            lock (this.inventoryMutex)
            {
                if (!this.inventory.ContainsKey(obj.Guid)) 
                    this.inventory.Add(obj.Guid, obj);
            }
            obj.GameData.ContainerId = session.Player.Guid.Full;

            session.Network.EnqueueSend(
               new GameMessagePrivateUpdatePropertyInt(session,
                   PropertyInt.EncumbVal,
                   (uint)session.Player.GameData.Burden));
            session.Network.EnqueueSend(new GameMessagePutObjectInContainer(session, session.Player, obj.Guid));
            
            motion.MovementData.ForwardCommand = 0;

            // Set Container id to the player - you belong to me
            session.Network.EnqueueSend(
                new GameMessageUpdateInstanceId(obj.Guid, session.Player.Guid));            
            
            // TODO: Finish out pick up item
            session.Network.EnqueueSend(new GameMessagePickupEvent(session, obj));
            // Tell the landblock so it can tell everyone around that the item has been picked up.

            LandblockManager.AddObject(obj);
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
