using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateInstanceId : GameMessage
    {
        public GameMessagePrivateUpdateInstanceId(WorldObject worldObject, PropertyInstanceId property, uint value)
            : base(GameMessageOpcode.PrivateUpdatePropertyInstanceID, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdatePropertyInstanceID));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
