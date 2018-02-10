using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyInt : GameMessage
    {
        public GameMessagePrivateUpdatePropertyInt(SequenceManager sequences, PropertyInt property, int value)
            : base(GameMessageOpcode.PrivateUpdatePropertyInt, GameMessageGroup.UIQueue)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PrivateUpdatePropertyInt));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
