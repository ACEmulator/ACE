using ACE.Entity.Enum;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageParentEvent : GameMessage
    {
        public GameMessageParentEvent(WorldObject parent, WorldObject child, ParentLocation? parentLocation = null, Placement? placement = null)
            : base(GameMessageOpcode.ParentEvent, GameMessageGroup.SmartboxQueue)
        {
            Writer.WriteGuid(parent.Guid);
            Writer.WriteGuid(child.Guid);
            Writer.Write(parentLocation != null ? (int)parentLocation : (int?)child.ParentLocation ?? 0);
            Writer.Write(placement != null ? (int)placement : (int?)child.Placement ?? 0);
            Writer.Write(parent.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(child.Sequences.GetNextSequence(SequenceType.ObjectPosition));
        }
    }
}
