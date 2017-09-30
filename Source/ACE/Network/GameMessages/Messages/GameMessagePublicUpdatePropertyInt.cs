using ACE.Entity.Enum.Properties;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    using global::ACE.Entity;

    public class GameMessagePublicUpdatePropertyInt : GameMessage
    {
        public GameMessagePublicUpdatePropertyInt(SequenceManager sequences, PropertyInt property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt, GameMessageGroup.Group09)
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
        public GameMessagePublicUpdatePropertyInt(SequenceManager sequences, ObjectGuid sender, PropertyInt property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInt));
            Writer.Write(sender.Full);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
