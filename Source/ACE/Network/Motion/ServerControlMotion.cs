using System.Collections.Generic;
using System.IO;
using ACE.Network.Enum;
using ACE.Entity;

namespace ACE.Network.Motion
{
    using System;

    public class ServerControlMotion : MotionState
    {
        public bool HasTarget { get; set; } = false;
        public bool Jumping { get; set; } = false;
        
        public MotionStance Stance { get; } // command

        public Position Position { get; }

        public ServerControlMotion(MotionStance stance, WorldObject moveToTarget)
        {
            Stance = stance;
            Position = moveToTarget.PhysicsData.Position;
        }

       public override byte[] GetPayload(WorldObject moveToTarget)
       {
            const uint Flag = 0x0041EE4F;
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)MovementTypes.MoveToObject); // movement_type
            MotionFlags flags = MotionFlags.None;
            if (HasTarget)
                flags |= MotionFlags.HasTarget;
            if (Jumping)
                flags |= MotionFlags.Jumping;

            writer.Write((byte)flags); // these can be or and has sticky object | is long jump mode |
            writer.Write((ushort)Stance); // called command in the client
            writer.Write(moveToTarget.Guid.Full);
            Position.Serialize(writer, false);
            // TODO: Og Fix to real numbers 
            writer.Write(Flag);
            writer.Write((float)20);
            writer.Write((float)0);
            writer.Write(float.MaxValue);
            writer.Write((float)1);
            writer.Write((float)15);
            writer.Write((float)0);
            writer.Write((float)1.63461542);
            return stream.ToArray();
        }
    }
}
