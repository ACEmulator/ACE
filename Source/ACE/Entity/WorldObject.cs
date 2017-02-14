using ACE.Network;

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

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            Type = type;
            Guid = guid;
        }

        public ServerPacket BuildObjectCreate()
        {
            var player = Guid.IsPlayer() ? this as Player : null;

            var objectCreate         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var objectCreateFragment = new ServerPacketFragment(0x0A, GameMessageOpcode.ObjectCreate);
            objectCreateFragment.Payload.WriteGuid(Guid);

            // TODO: model information
            objectCreateFragment.Payload.Write((byte)0x11);
            objectCreateFragment.Payload.Write((byte)0);
            objectCreateFragment.Payload.Write((byte)0);
            objectCreateFragment.Payload.Write((byte)0);

            objectCreateFragment.Payload.Align();

            PhysicsDescriptionFlag flags = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position;
            objectCreateFragment.Payload.Write((uint)flags);
            objectCreateFragment.Payload.Write((uint)PhysicsState);

            /*if ((flags & PhysicsDescriptionFlag.Movement) != 0)
            {
            }*/

            /*if ((flags & PhysicsDescriptionFlag.AnimationFrame) != 0)
            {
            }*/

            if ((flags & PhysicsDescriptionFlag.Position) != 0)
                Position.Write(objectCreateFragment.Payload);

            // TODO:
            if ((flags & PhysicsDescriptionFlag.MTable) != 0)
                objectCreateFragment.Payload.Write(0x09000001u);

            if ((flags & PhysicsDescriptionFlag.Stable) != 0)
                objectCreateFragment.Payload.Write(0x20000001u);

            if ((flags & PhysicsDescriptionFlag.Petable) != 0)
                objectCreateFragment.Payload.Write(0x34000004u);

            if ((flags & PhysicsDescriptionFlag.CSetup) != 0)
                objectCreateFragment.Payload.Write(0x02000001u);

            /*if ((flags & PhysicsDescriptionFlag.Parent) != 0)
            {
            }*/

            /*if ((flags & PhysicsDescriptionFlag.Children) != 0)
            {
            }*/

            if ((flags & PhysicsDescriptionFlag.ObjScale) != 0)
                objectCreateFragment.Payload.Write(0.0f);

            if ((flags & PhysicsDescriptionFlag.Friction) != 0)
                objectCreateFragment.Payload.Write(0.0f);

            if ((flags & PhysicsDescriptionFlag.Elastcity) != 0)
                objectCreateFragment.Payload.Write(0.0f);

            if ((flags & PhysicsDescriptionFlag.Translucency) != 0)
                objectCreateFragment.Payload.Write(0.0f);

            if ((flags & PhysicsDescriptionFlag.Velocity) != 0)
            {
                objectCreateFragment.Payload.Write(0.0f);
                objectCreateFragment.Payload.Write(0.0f);
                objectCreateFragment.Payload.Write(0.0f);
            }

            if ((flags & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                objectCreateFragment.Payload.Write(0.0f);
                objectCreateFragment.Payload.Write(0.0f);
                objectCreateFragment.Payload.Write(0.0f);
            }

            if ((flags & PhysicsDescriptionFlag.Omega) != 0)
            {
                objectCreateFragment.Payload.Write(0.0f);
                objectCreateFragment.Payload.Write(0.0f);
                objectCreateFragment.Payload.Write(0.0f);
            }

            if ((flags & PhysicsDescriptionFlag.DefaultScript) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((flags & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                objectCreateFragment.Payload.Write(0.0f);

            objectCreateFragment.Payload.Write((ushort)MovementIndex);
            objectCreateFragment.Payload.Write((ushort)1);
            objectCreateFragment.Payload.Write((ushort)(player?.PortalIndex ?? 0));
            objectCreateFragment.Payload.Write((ushort)0);
            objectCreateFragment.Payload.Write((ushort)TeleportIndex);
            objectCreateFragment.Payload.Write((ushort)0);
            objectCreateFragment.Payload.Write((ushort)0);
            objectCreateFragment.Payload.Write((ushort)0);
            objectCreateFragment.Payload.Write((ushort)(player?.TotalLogins ?? 0));

            objectCreateFragment.Payload.Align();

            var weenieHeaderFlags = WeenieHeaderFlag.ItemCapacity | WeenieHeaderFlag.ContainerCapacity | WeenieHeaderFlag.Useability | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar;
            objectCreateFragment.Payload.Write((uint)weenieHeaderFlags);
            objectCreateFragment.Payload.WriteString16L(Name);
            objectCreateFragment.Payload.Write((ushort)1);
            objectCreateFragment.Payload.Write((ushort)0x1036);
            objectCreateFragment.Payload.Write((uint)Type);
            objectCreateFragment.Payload.Write((uint)DescriptionFlags);

            if ((DescriptionFlags & ObjectDescriptionFlag.AdditionFlags) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.PuralName) != 0)
                objectCreateFragment.Payload.WriteString16L("");

            if ((weenieHeaderFlags & WeenieHeaderFlag.ItemCapacity) != 0)
                objectCreateFragment.Payload.Write((byte)102);

            if ((weenieHeaderFlags & WeenieHeaderFlag.ContainerCapacity) != 0)
                objectCreateFragment.Payload.Write((byte)7);

            if ((weenieHeaderFlags & WeenieHeaderFlag.AmmoType) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Value) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Useability) != 0)
                objectCreateFragment.Payload.Write(1u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.UseRadius) != 0)
                objectCreateFragment.Payload.Write(0.0f);

            if ((weenieHeaderFlags & WeenieHeaderFlag.TargetType) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.UiEffects) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.CombatUse) != 0)
                objectCreateFragment.Payload.Write((byte)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Struture) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.MaxStructure) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.StackSize) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Container) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Wielder) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.ValidLocations) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Location) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Priority) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.BlipColour) != 0)
                objectCreateFragment.Payload.Write((byte)9);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Radar) != 0)
                objectCreateFragment.Payload.Write((byte)4);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Script) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Workmanship) != 0)
                objectCreateFragment.Payload.Write(0.0f);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Burden) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Spell) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.HouseOwner) != 0)
                objectCreateFragment.Payload.Write(0u);

            /*if ((weenieHeaderFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
            {
            }*/

            if ((weenieHeaderFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.Monarch) != 0)
                objectCreateFragment.Payload.Write(0u);

            if ((weenieHeaderFlags & WeenieHeaderFlag.HookType) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            if ((weenieHeaderFlags & WeenieHeaderFlag.IconOverlay) != 0)
                objectCreateFragment.Payload.Write((ushort)0);

            /*if ((weenieHeaderFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                objectCreateFragment.Payload.Write((ushort)0);*/

            if ((weenieHeaderFlags & WeenieHeaderFlag.Material) != 0)
                objectCreateFragment.Payload.Write(0u);

            /*if ((weenieHeaderFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                objectCreateFragment.Payload.Write(0u);*/

            /*if ((weenieHeaderFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                objectCreateFragment.Payload.Write(0.0d);*/

            /*if ((weenieHeaderFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
                objectCreateFragment.Payload.Write(0u);*/

            objectCreateFragment.Payload.Align();

            objectCreate.Fragments.Add(objectCreateFragment);
            return objectCreate;
        }

        public void UpdatePosition(Position newPosition)
        {
            // TODO: sanity checks
            Position = newPosition;

            var updatePositionFlags = UpdatePositionFlag.Contact;
            /*if (newPosition.Facing.W == 0.0f || newPosition.Facing.W == Position.Facing.W)
                updatePositionFlags |= UpdatePositionFlag.NoQuaternionW;
            if (newPosition.Facing.X == 0.0f || newPosition.Facing.X == Position.Facing.X)
                updatePositionFlags |= UpdatePositionFlag.NoQuaternionX;
            if (newPosition.Facing.Y == 0.0f || newPosition.Facing.Y == Position.Facing.Y)
                updatePositionFlags |= UpdatePositionFlag.NoQuaternionY;
            if (newPosition.Facing.Z == 0.0f || newPosition.Facing.Z == Position.Facing.Z)
                updatePositionFlags |= UpdatePositionFlag.NoQuaternionZ;*/

            var updatePosition         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var updatePositionFragment = new ServerPacketFragment(0x0A, GameMessageOpcode.UpdatePosition);
            updatePositionFragment.Payload.WriteGuid(Guid);
            updatePositionFragment.Payload.Write((uint)updatePositionFlags);

            newPosition.Write(updatePositionFragment.Payload, false);

            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                updatePositionFragment.Payload.Write(newPosition.Facing.W);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                updatePositionFragment.Payload.Write(newPosition.Facing.X);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                updatePositionFragment.Payload.Write(newPosition.Facing.Y);
            if ((updatePositionFlags & UpdatePositionFlag.NoQuaternionW) == 0)
                updatePositionFragment.Payload.Write(newPosition.Facing.Z);

            /*if ((updatePositionFlags & UpdatePositionFlag.Velocity) != 0)
            {
            }*/

            /*if ((updatePositionFlags & UpdatePositionFlag.Placement) != 0)
            {
            }*/

            var player = Guid.IsPlayer() ? this as Player : null;
            updatePositionFragment.Payload.Write((ushort)(player?.TotalLogins ?? 0));
            updatePositionFragment.Payload.Write((ushort)++MovementIndex);
            updatePositionFragment.Payload.Write((ushort)TeleportIndex);
            updatePositionFragment.Payload.Write((ushort)0);

            updatePosition.Fragments.Add(updatePositionFragment);

            // TODO: this packet needs to be broadcast to the grid system, just send to self for now
            if (Guid.IsPlayer())
                NetworkManager.SendPacket(ConnectionType.World, updatePosition, (this as Player).Session);
        }
    }
}
