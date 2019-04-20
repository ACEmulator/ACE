using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyInt64 : GameMessage
    {
        public GameMessagePrivateUpdatePropertyInt64(WorldObject worldObject, PropertyInt64 property, long value)
            : base(GameMessageOpcode.PrivateUpdatePropertyInt64, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdatePropertyInt64, property));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
