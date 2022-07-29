using System;
using System.IO;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Network.Sequence;
using ACE.Server.Physics;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// A position with sequences
    /// </summary>
    public class PositionPack
    {
        public WorldObject WorldObject;

        public PositionFlags Flags;
        public Origin Origin;       // the location of the object in the world
        public Quaternion Rotation;

        public Vector3 Velocity;
        public Placement? PlacementID;

        // really just a bunch of ushorts for these particular sequences,
        // but for type safety, there appear to be some other sequences that could be either uints or ulongs,
        // so GetCurrentSequence/GetNextSequence returns byte arrays here...

        public byte[] InstanceSequence;
        public byte[] PositionSequence;
        public byte[] TeleportSequence;
        public byte[] ForcePositionSequence;

        public PositionPack() { }

        public PositionPack(WorldObject wo, bool adminMove = false)
        {
            WorldObject = wo;

            Origin = new Origin(wo.Location.Cell, wo.Location.Pos);
            Rotation = wo.Location.Rotation;
            Velocity = wo.PhysicsObj != null ? wo.PhysicsObj.Velocity : Vector3.Zero;  // average or instantaneous?
            PlacementID = wo.Placement;

            // note that this constructor increments the wo position sequence
            InstanceSequence = wo.Sequences.GetCurrentSequence(SequenceType.ObjectInstance);
            PositionSequence = wo.Sequences.GetNextSequence(SequenceType.ObjectPosition);

            if (adminMove)
                TeleportSequence = wo.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            else
                TeleportSequence = wo.Sequences.GetCurrentSequence(SequenceType.ObjectTeleport);

            ForcePositionSequence = wo.Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition);

            Flags = BuildFlags();
        }

        /// <summary>
        /// Returns the PositionFlags based on the current state
        /// </summary>
        public PositionFlags BuildFlags()
        {
            var flags = PositionFlags.None;

            if (Velocity != Vector3.Zero)
                flags |= PositionFlags.HasVelocity;

            if (PlacementID != null)
                flags |= PositionFlags.HasPlacementID;

            if (WorldObject.PhysicsObj != null && (WorldObject.PhysicsObj.TransientState & TransientStateFlags.OnWalkable) != 0)
                flags |= PositionFlags.IsGrounded;

            if (Rotation.W == 0.0f)
                flags |= PositionFlags.OrientationHasNoW;
            if (Rotation.X == 0.0f)
                flags |= PositionFlags.OrientationHasNoX;
            if (Rotation.Y == 0.0f)
                flags |= PositionFlags.OrientationHasNoY;
            if (Rotation.Z == 0.0f)
                flags |= PositionFlags.OrientationHasNoZ;

            return flags;
        }
    }

    public static class PositionPackExtensions
    {
        public static void Write(this BinaryWriter writer, PositionPack position)
        {
            writer.Write((uint)position.Flags);
            writer.Write(position.Origin);

            // choose valid sections by masking against flags
            if ((position.Flags & PositionFlags.OrientationHasNoW) == 0)
                writer.Write(position.Rotation.W);
            if ((position.Flags & PositionFlags.OrientationHasNoX) == 0)
                writer.Write(position.Rotation.X);
            if ((position.Flags & PositionFlags.OrientationHasNoY) == 0)
                writer.Write(position.Rotation.Y);
            if ((position.Flags & PositionFlags.OrientationHasNoZ) == 0)
                writer.Write(position.Rotation.Z);

            if ((position.Flags & PositionFlags.HasVelocity) != 0)
                writer.Write(position.Velocity);

            if ((position.Flags & PositionFlags.HasPlacementID) != 0)
                writer.Write((uint)position.PlacementID);

            writer.Write(position.InstanceSequence);
            writer.Write(position.PositionSequence);
            writer.Write(position.TeleportSequence);
            writer.Write(position.ForcePositionSequence);
        }
    }
}
