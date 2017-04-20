using log4net;
using ACE.Network.Enum;
using ACE.Entity.Objects;
using ACE.Network.Sequence;
using ACE.Network.Motion;
using System.Collections.Generic;

namespace ACE.Network.GameMessages.Messages
{
    using System.Diagnostics.Eventing.Reader;
    using System.Runtime.Remoting.Messaging;

    using global::ACE.Entity;

    public class GameMessageUpdateMotion : GameMessage
    {
        public GameMessageUpdateMotion(WorldObject animationTarget, Session session, MotionState newState)
            : base(GameMessageOpcode.Motion, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(animationTarget.Guid); // Object_Id (uint)
            Writer.Write((ushort)session.Player.TotalLogins); // Instance_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.ObjectMovement)); // Movement_Timestamp
            if (!newState.IsAutonomous)
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp
            else
                Writer.Write(animationTarget.Sequences.GetCurrentSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp

            ushort autonomous = newState.IsAutonomous ? (ushort)1 : (ushort)0;
            Writer.Write(autonomous); // autonomous flag - 1 or 0.   I think this is set if you have are holding the run key or some other autonomous movement
            var movementData = newState.GetPayload(animationTarget);
            Writer.Write(movementData);
            Writer.Align();
        }

        public GameMessageUpdateMotion(WorldObject animationTarget, MotionState newState) : base(GameMessageOpcode.Motion, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(animationTarget.Guid); // Object_Id (uint)
            Writer.Write(animationTarget.Sequences.GetCurrentSequence(SequenceType.ObjectInstance)); // Instance_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.ObjectMovement)); // Movement_Timestamp
            if (!newState.IsAutonomous)
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp
            else
                Writer.Write(animationTarget.Sequences.GetCurrentSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp

            ushort autonomous = newState.IsAutonomous ? (ushort)1 : (ushort)0;
            Writer.Write(autonomous); // autonomous flag - 1 or 0.   I think this is set if you have are holding the run key or some other autonomous movement
            var movementData = newState.GetPayload(animationTarget);
            Writer.Write(movementData);
            Writer.Align();
        }
    }
}
