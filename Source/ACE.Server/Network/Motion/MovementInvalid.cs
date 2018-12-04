using System.IO;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;

namespace ACE.Server.Network.Structure
{
    public class MovementInvalid
    {
        public MovementData MovementData;

        public InterpretedMotionState State;    // set of movement / animation data
        public ObjectGuid StickyObject;         // choose valid sections by masking against MotionFlags: 0x1 - object to stick to

        public MovementInvalid(MovementData movementData)
        {
            MovementData = movementData;
            State = new InterpretedMotionState(movementData);
        }

        public MovementInvalid(MovementData movementData, Motion motion)
        {
            MovementData = movementData;

            State = new InterpretedMotionState(movementData, motion);

            if ((motion.MotionFlags & MotionFlags.StickToObject) != 0)
                StickyObject = motion.TargetGuid;
        }

        public MovementInvalid(MovementData movementData, InterpretedMotionState state)
        {
            MovementData = movementData;
            State = state;
            state.BuildMovementFlags();
        }
    }

    public static class MovementInvalidExtensions
    {
        public static void Write(this BinaryWriter writer, MovementInvalid movement)
        {
            writer.Write(movement.State);

            if ((movement.MovementData.MotionFlags & MotionFlags.StickToObject) != 0)
                writer.WriteGuid(movement.StickyObject);
        }
    }
}
