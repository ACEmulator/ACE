using ACE.Entity;
using ACE.Network.GameEvent;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePickupEvent : GameMessage
    {
        public GameMessagePickupEvent(Session session,  WorldObject targetItem)
            : base(GameMessageOpcode.PickupEvent, GameMessageGroup.Group0A)
        {
            Writer.Write(targetItem.Guid.Full);
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(targetItem.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(targetItem.Sequences.GetCurrentSequence(SequenceType.ObjectPosition));
        }
    }
}
