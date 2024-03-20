using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageDeleteObject : GameMessage
    {
        public GameMessageDeleteObject(WorldObject worldObject) : base(GameMessageOpcode.ObjectDelete, GameMessageGroup.SmartboxQueue, 12)
        {
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Align();
        }
    }
}
