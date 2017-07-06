using ACE.Entity.Enum.Properties;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyInt : GameMessage
    {
        public GameMessagePrivateUpdatePropertyInt(SequenceManager sequences, PropertyInt property, uint value)
            : base(GameMessageOpcode.PrivateUpdatePropertyInt, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PrivateUpdatePropertyInt));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
