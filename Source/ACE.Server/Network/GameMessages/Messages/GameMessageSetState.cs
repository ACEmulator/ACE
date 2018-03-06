using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageSetState : GameMessage
    {
        public GameMessageSetState(WorldObject worldObject, PhysicsState state)
            : base(GameMessageOpcode.SetState, GameMessageGroup.SmartboxQueue)
        {
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write((uint)state);
            Writer.Write(worldObject.Sequences.GetCurrentSequence(Sequence.SequenceType.ObjectInstance));
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.ObjectState));
        }
    }
}
