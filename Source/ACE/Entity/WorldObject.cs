
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;
using System.IO;

namespace ACE.Entity
{
    public abstract class WorldObject
    {
        public ObjectGuid Guid { get; }

        public ObjectType Type { get; protected set; }

        public ushort GameDataType { get; protected set; }

        public ushort Icon { get; protected set; }

        public string Name { get; protected set; }

        public Position Position
        { 
            get { return PhysicsData.Position; }
            protected set { PhysicsData.Position = value; }
        }

        public ObjectDescriptionFlag DescriptionFlags { get; protected set; }
        
        public WeenieHeaderFlag WeenieFlags { get; protected set; }

        public WeenieHeaderFlag2 WeenieFlags2 { get; protected set; }

        public uint MovementIndex { get; set; }

        public uint TeleportIndex { get; set; }

        public virtual float ListeningRadius { get; protected set; } = 5f;

        public ModelData ModelData { get; }

        public PhysicsData PhysicsData { get; }

        public GameData GameData { get; }

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            Type = type;
            Guid = guid;

            GameData = new GameData();
            ModelData = new ModelData();
            PhysicsData = new PhysicsData();
        }

        public virtual void SerializeCreateObject(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);

            ModelData.Serialize(writer);
            PhysicsData.Serialize(writer);
            
            writer.Write((uint)WeenieFlags);
            writer.WriteString16L(Name);
            writer.Write((ushort)GameDataType);
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

            if ((WeenieFlags & WeenieHeaderFlag.Useability) != 0)
                writer.Write(GameData.Useability);

            if ((WeenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(GameData.UseRadius);

            if ((WeenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write(GameData.TargetType);

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

        public void UpdatePosition(Position newPosition)
        {
            // TODO: sanity checks
            Position = newPosition;

            // TODO: this packet needs to be broadcast to the grid system, just send to self for now
            if (Guid.IsPlayer())
            {
                Session session = (this as Player).Session;
                session.WorldSession.EnqueueSend(new GameMessageUpdatePosition(this));
            }
        }

        public void WriteUpdatePositionPayload(BinaryWriter writer)
        {
            var updatePositionFlags = UpdatePositionFlag.Contact;
            writer.WriteGuid(Guid);
            writer.Write((uint)updatePositionFlags);

            Position.Write(writer, false);

            /*if (newPosition.Facing.W == 0.0f || newPosition.Facing.W == Position.Facing.W)
                updatePositionFlags |= UpdatePositionFlag.NoQuaternionW;
            if (newPosition.Facing.X == 0.0f || newPosition.Facing.X == Position.Facing.X)
                updatePositionFlags |= UpdatePositionFlag.NoQuaternionX;
            if (newPosition.Facing.Y == 0.0f || newPosition.Facing.Y == Position.Facing.Y)
                updatePositionFlags |= UpdatePositionFlag.NoQuaternionY;
            if (newPosition.Facing.Z == 0.0f || newPosition.Facing.Z == Position.Facing.Z)
                updatePositionFlags |= UpdatePositionFlag.NoQuaternionZ;*/

            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                writer.Write(Position.Facing.W);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                writer.Write(Position.Facing.X);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                writer.Write(Position.Facing.Y);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                writer.Write(Position.Facing.Z);

            /*if ((updatePositionFlags & UpdatePositionFlag.Velocity) != 0)
            {
            }*/

            /*if ((updatePositionFlags & UpdatePositionFlag.Placement) != 0)
            {
            }*/

            var player = Guid.IsPlayer() ? this as Player : null;
            writer.Write((ushort)(player?.TotalLogins ?? 0));
            writer.Write((ushort)++MovementIndex);
            writer.Write((ushort)TeleportIndex);
            writer.Write((ushort)0);
        }
    }
}
