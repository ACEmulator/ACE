using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyDouble : GameMessage
    {
        public GameMessagePublicUpdatePropertyDouble(SequenceManager sequences, PropertyFloat property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyDouble, GameMessageGroup.UIQueue)
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
        public GameMessagePublicUpdatePropertyDouble(SequenceManager sequences, ObjectGuid sender, PropertyFloat property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyDouble, GameMessageGroup.UIQueue)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyDouble));
            Writer.Write(sender.Full);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
