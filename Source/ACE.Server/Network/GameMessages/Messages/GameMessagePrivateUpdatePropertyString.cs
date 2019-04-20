using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyString : GameMessage
    {
        public GameMessagePrivateUpdatePropertyString(WorldObject worldObject, PropertyString property, string value)
            : base(GameMessageOpcode.PrivateUpdatePropertyString, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdatePropertyString, property));
            Writer.Write((uint)property);
            Writer.Align();
            Writer.WriteString16L(value);
        }
    }
}
