using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyString : GameMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="sender"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyString(SequenceManager sequences, ObjectGuid sender, PropertyString property, string value)
            : base(GameMessageOpcode.PublicUpdatePropertyString, GameMessageGroup.UIQueue)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyString));
            Writer.Write((uint)property);
            Writer.Write(sender.Full);
            Writer.Align();
            Writer.WriteString16L(value);
        }
    }
}
