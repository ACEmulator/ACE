
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

        public ObjectType Type { get; }

        public string Name { get; protected set; }

        public Position Position { get; protected set; }

        public ObjectDescriptionFlag DescriptionFlags { get; protected set; }

        public PhysicsState PhysicsState { get; protected set; }

        public uint MovementIndex { get; set; }

        public uint TeleportIndex { get; set; }

        public virtual float ListeningRadius { get; protected set; } = 0f;

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            Type = type;
            Guid = guid;
        }

        public virtual void WriteCreateObjectPayload(BinaryWriter writer)
        {
            var player = Guid.IsPlayer() ? this as Player : null;
            writer.WriteGuid(Guid);

            // TODO: model information
            writer.Write((byte)0x11);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)0);

            writer.Align();

            PhysicsDescriptionFlag flags = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position;
            writer.Write((uint)flags);
            writer.Write((uint)PhysicsState);

            /*if ((flags & PhysicsDescriptionFlag.Movement) != 0)
            {
            }*/

            /*if ((flags & PhysicsDescriptionFlag.AnimationFrame) != 0)
            {
            }*/

            if ((flags & PhysicsDescriptionFlag.Position) != 0)
                Position.Write(writer);

                // TODO:
            if ((flags & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write(0x09000001u);

            if ((flags & PhysicsDescriptionFlag.Stable) != 0)
                writer.Write(0x20000001u);

            if ((flags & PhysicsDescriptionFlag.Petable) != 0)
                writer.Write(0x34000004u);

            if ((flags & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(0x02000001u);

            /*if ((flags & PhysicsDescriptionFlag.Parent) != 0)
            {
            }*/

            /*if ((flags & PhysicsDescriptionFlag.Children) != 0)
            {
            }*/

            if ((flags & PhysicsDescriptionFlag.ObjScale) != 0)
                writer.Write(0.0f);

            if ((flags & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(0.0f);

            if ((flags & PhysicsDescriptionFlag.Elastcity) != 0)
                writer.Write(0.0f);

            if ((flags & PhysicsDescriptionFlag.Translucency) != 0)
                writer.Write(0.0f);

            if ((flags & PhysicsDescriptionFlag.Velocity) != 0)
            {
                writer.Write(0.0f);
                writer.Write(0.0f);
                writer.Write(0.0f);
            }

            if ((flags & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                writer.Write(0.0f);
                writer.Write(0.0f);
                writer.Write(0.0f);
            }

            if ((flags & PhysicsDescriptionFlag.Omega) != 0)
            {
                writer.Write(0.0f);
                writer.Write(0.0f);
                writer.Write(0.0f);
            }

            if ((flags & PhysicsDescriptionFlag.DefaultScript) != 0)
                writer.Write(0u);

            if ((flags & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                writer.Write(0.0f);

            writer.Write((ushort)MovementIndex);
            writer.Write((ushort)1);
            writer.Write((ushort)(player?.PortalIndex ?? 0));
            writer.Write((ushort)0);
            writer.Write((ushort)TeleportIndex);
            writer.Write((ushort)0);
            writer.Write((ushort)0);
            writer.Write((ushort)0);
            writer.Write((ushort)(player?.TotalLogins ?? 0));

            writer.Align();

            var weenieHeaderFlags = WeenieHeaderFlag.ItemCapacity | WeenieHeaderFlag.ContainerCapacity | WeenieHeaderFlag.Useability | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar;
            writer.Write((uint)weenieHeaderFlags);
            writer.WriteString16L(Name);
            writer.Write((ushort)1);
            writer.Write((ushort)0x1036);
            writer.Write((uint)Type);
            writer.Write((uint)DescriptionFlags);

            if ((DescriptionFlags & ObjectDescriptionFlag.AdditionFlags) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.PuralName) != 0)
                writer.WriteString16L("");

            if ((weenieHeaderFlags & WeenieHeaderFlag.ItemCapacity) != 0)
                writer.Write((byte)102);

            if ((weenieHeaderFlags & WeenieHeaderFlag.ContainerCapacity) != 0)
                writer.Write((byte)7);

            if ((weenieHeaderFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Useability) != 0)
                writer.Write(1u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(0.0f);

            if ((weenieHeaderFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((byte)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Struture) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.ValidLocations) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Location) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Priority) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.BlipColour) != 0)
                writer.Write((byte)9);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Radar) != 0)
                writer.Write((byte)4);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Script) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write(0.0f);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(0u);

            /*if ((weenieHeaderFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
            {
            }*/

            if ((weenieHeaderFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.HookType) != 0)
                writer.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.Write((ushort)0);

            /*if ((weenieHeaderFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.Write((ushort)0);*/

            if ((weenieHeaderFlags & WeenieHeaderFlag.Material) != 0)
                writer.Write(0u);

            /*if ((weenieHeaderFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                writer.Write(0u);*/

            /*if ((weenieHeaderFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                writer.Write(0.0d);*/

            /*if ((weenieHeaderFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
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
