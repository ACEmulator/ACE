using ACE.Entity.Enum.Properties;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    using global::ACE.Entity;

    public class GameMessagePublicUpdatePropertyDouble : GameMessage
    {
        public GameMessagePublicUpdatePropertyDouble(SequenceManager sequences, PropertyDouble property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyDouble, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyDouble));
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
        public GameMessagePublicUpdatePropertyDouble(SequenceManager sequences, ObjectGuid sender, PropertyDouble property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyDouble, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyDouble));
            Writer.Write(sender.Full);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
