using ACE.Entity;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePickupEvent : GameMessage
    {
        public GameMessagePickupEvent(WorldObject targetItem)
            : base(GameMessageOpcode.PickupEvent, GameMessageGroup.Group0A)
        {
            Writer.Write(targetItem.Guid.Full);
            Writer.Write(targetItem.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(targetItem.Sequences.GetNextSequence(SequenceType.ObjectPosition));
        }
    }
}