using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyInt : GameMessage
    {
        public GameMessagePublicUpdatePropertyInt(SequenceManager sequences, PropertyInt property, int value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt, GameMessageGroup.UIQueue)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInt));
            Writer.Write((uint)property);
            Writer.Write(value);
        }

        /// <summary>
        /// This is used to
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="sender"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyInt(SequenceManager sequences, ObjectGuid sender, PropertyInt property, int value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt, GameMessageGroup.UIQueue)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInt));
            Writer.Write(sender.Full);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
