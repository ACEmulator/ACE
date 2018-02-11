using ACE.Entity;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageUpdateMotion : GameMessage
    {
        public GameMessageUpdateMotion(ObjectGuid animationTargetGuid, byte[] instance_timestamp, SequenceManager sequence, MotionState newState)
             : base(GameMessageOpcode.Motion, GameMessageGroup.SmartboxQueue)
        {
            Writer.WriteGuid(animationTargetGuid);
            // who is getting the message - the rest of the sequences are the target objects sequences -may be the same
            Writer.Write(instance_timestamp);
            Writer.Write(sequence.GetNextSequence(SequenceType.ObjectMovement));
            ushort autonomous = newState.IsAutonomous ? (ushort)1 : (ushort)0;
            if (autonomous == 0)
                Writer.Write(sequence.GetNextSequence(SequenceType.ObjectServerControl));
            else
                Writer.Write(sequence.GetCurrentSequence(SequenceType.ObjectServerControl));
            Writer.Write(autonomous);
            var movementData = newState.GetPayload(animationTargetGuid, sequence);
            Writer.Write(movementData);
            Writer.Align();
        }
    }
}
