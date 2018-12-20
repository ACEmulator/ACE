using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyInt : GameMessage
    {
        public GameMessagePrivateUpdatePropertyInt(WorldObject worldObject, PropertyInt property, int value)
            : base(GameMessageOpcode.PrivateUpdatePropertyInt, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.UpdatePropertyInt, property));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
