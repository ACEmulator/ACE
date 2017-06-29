using ACE.Entity;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageParentEvent : GameMessage
    {
        public GameMessageParentEvent(Session session, WorldObject targetItem)
            : base(GameMessageOpcode.ParentEvent, GameMessageGroup.Group0A)
        {
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(targetItem.Guid.Full);
            Writer.Write(1u);
            Writer.Write(1u);
            Writer.Write(session.Player.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(session.Player.Sequences.GetCurrentSequence(SequenceType.ObjectVisualDesc));
        }
    }
}