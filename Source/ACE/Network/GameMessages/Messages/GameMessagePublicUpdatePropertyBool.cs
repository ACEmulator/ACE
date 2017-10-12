using ACE.Entity.Enum.Properties;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    using global::ACE.Entity;

    public class GameMessagePublicUpdatePropertyBool : GameMessage
    {
        public GameMessagePublicUpdatePropertyBool(SequenceManager sequences, PropertyBool property, bool value)
            : base(GameMessageOpcode.PublicUpdatePropertyBool, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyBool));
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
        public GameMessagePublicUpdatePropertyBool(SequenceManager sequences, ObjectGuid sender, PropertyBool property, bool value)
            : base(GameMessageOpcode.PublicUpdatePropertyBool, GameMessageGroup.Group09)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyBool));
            Writer.Write(sender.Full);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
