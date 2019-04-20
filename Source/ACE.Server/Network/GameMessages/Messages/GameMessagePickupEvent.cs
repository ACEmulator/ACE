using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePickupEvent : GameMessage
    {
        public GameMessagePickupEvent(WorldObject targetItem)
            : base(GameMessageOpcode.PickupEvent, GameMessageGroup.SmartboxQueue)
        {
            Writer.Write(targetItem.Guid.Full);
            Writer.Write(targetItem.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(targetItem.Sequences.GetNextSequence(SequenceType.ObjectPosition));
        }
    }
}
