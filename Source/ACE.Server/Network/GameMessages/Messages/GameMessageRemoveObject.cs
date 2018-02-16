using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageRemoveObject : GameMessage
    {
        public GameMessageRemoveObject(WorldObject worldObject) : base(GameMessageOpcode.ObjectDelete, GameMessageGroup.SmartboxQueue)
        {
            // TODO: Verify.  this was done without referencing the protocol spec
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
        }
    }
}
