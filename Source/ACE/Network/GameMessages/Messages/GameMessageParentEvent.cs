using ACE.Entity;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageParentEvent : GameMessage
    {
        public GameMessageParentEvent(WorldObject player, ObjectGuid targetItem, uint childLocation, uint placementId)
            : base(GameMessageOpcode.ParentEvent, GameMessageGroup.Group0A)
        {
            // Fix File Name
            Writer.Write(player.Guid.Full);
            Writer.Write(targetItem.Full);
            Writer.Write(childLocation);
            Writer.Write(placementId);
            Writer.Write(player.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(player.Sequences.GetNextSequence(SequenceType.ObjectState));
        }
    }
}
