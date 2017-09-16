using ACE.Entity.Enum.Properties;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyInt : GameMessage
    {
        public GameMessagePublicUpdatePropertyInt(SequenceManager sequences, PropertyInt property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInt));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
