using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using ACE.Network.Sequence;
using System.IO;
using ACE.Managers;
using ACE.Network.GameMessages.Messages;

namespace ACE.Entity
{
    public abstract class WorldObject
    {
        public ObjectGuid Guid { get; }

        public ObjectType Type { get; protected set; }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public WeenieClass WeenieClassid { get; protected set; }

        public ushort Icon { get; set; }

        public string Name { get; protected set; }

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

        public virtual float ListeningRadius { get; protected set; } = 5f;

        public ModelData ModelData { get; }

        public PhysicsData PhysicsData { get; }

        public GameData GameData { get; }

        public bool IsContainer { get; set; } = false;

        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        private readonly object inventoryMutex = new object();

        public SequenceManager Sequences { get; }

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            Type = type;
            Guid = guid;

            GameData = new GameData();
            ModelData = new ModelData();
            PhysicsData = new PhysicsData();

            Sequences = new SequenceManager();
            Sequences.AddSequence(SequenceType.MotionMessage, new UShortSequence());
            Sequences.AddSequence(SequenceType.Motion, new UShortSequence());
            Sequences.AddSequence(SequenceType.ServerControl, new UShortSequence());
        }

        public void AddToInventory(WorldObject worldObject)
        {
            lock (inventoryMutex)
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

        public void HandleDropItem(ObjectGuid objectGuid, Session session)
        {
            lock (this.inventoryMutex)
            {
                if (!this.inventory.ContainsKey(objectGuid)) return;
                var obj = this.inventory[objectGuid];
                this.GameData.Burden = obj.GameData.Burden;
                // TODO: Not sure if the next two lines need to be here.
                obj.GameData.ContainerId = 0;
                obj.PhysicsData.Position = PhysicsData.Position.InFrontOf(1.50f);
                obj.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Position;

                // TODO: Send the last part of the sequence.   GameMessageUpdatePosition - I am doing something wrong here 
                // session.Network.EnqueueSend(new GameMessageUpdatePosition(obj));

                LandblockManager.AddObject(obj);
                this.inventory.Remove(objectGuid);
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
                writer.Write(GameData.Spell);

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
            var updatePositionFlags = UpdatePositionFlag.Contact;
            writer.WriteGuid(Guid);
            writer.Write((uint)updatePositionFlags);

            Position.Serialize(writer, false);

            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                writer.Write(Position.Facing.W);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                writer.Write(Position.Facing.X);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                writer.Write(Position.Facing.Y);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                writer.Write(Position.Facing.Z);

            if ((updatePositionFlags & UpdatePositionFlag.Velocity) != 0)
            {
                // velocity would go here
            }

            var player = Guid.IsPlayer() ? this as Player : null;
            writer.Write((ushort)(player?.TotalLogins ?? 0));
            writer.Write((ushort)++MovementIndex);
            writer.Write((ushort)TeleportIndex);

            // TODO: this is the "contact" flag, which is a flag for whether or not
            // the player is "in contact" with the ground.  Sending the 0 here causes
            // others to see the player being update with arms horizontal and falling.
            writer.Write((ushort)0);
        }
    }
}