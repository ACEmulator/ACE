using ACE.Entity.Enum.Properties;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    using global::ACE.Entity;

    public class GameMessagePublicUpdatePropertyInt64 : GameMessage
    {
        public GameMessagePublicUpdatePropertyInt64(SequenceManager sequences, PropertyInt64 property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt64, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInt64));
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
        public GameMessagePublicUpdatePropertyInt64(SequenceManager sequences, ObjectGuid sender, PropertyInt64 property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt64, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInt64));
            Writer.Write(sender.Full);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
