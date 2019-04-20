using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageParentEvent : GameMessage
    {
        public GameMessageParentEvent(WorldObject player, WorldObject targetItem, int childLocation, int placementId)
            : base(GameMessageOpcode.ParentEvent, GameMessageGroup.SmartboxQueue)
        {
            // Fix File Name
            Writer.Write(player.Guid.Full);
            Writer.Write(targetItem.Guid.Full);
            Writer.Write(childLocation);
            Writer.Write(placementId);
            Writer.Write(player.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(targetItem.Sequences.GetNextSequence(SequenceType.ObjectPosition));
        }
    }
}
