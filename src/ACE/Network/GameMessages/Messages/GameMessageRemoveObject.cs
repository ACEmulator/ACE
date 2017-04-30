using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    using global::ACE.Network.Sequence;

    public class GameMessageRemoveObject : GameMessage
    {
        public GameMessageRemoveObject(WorldObject worldObject) : base(GameMessageOpcode.ObjectDelete, GameMessageGroup.Group0A)
        {
            // TODO: Verify.  this was done without referencing the protocol spec
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
        }
    }
}
