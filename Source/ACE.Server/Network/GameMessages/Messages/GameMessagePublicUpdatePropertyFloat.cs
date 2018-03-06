using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyFloat : GameMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="sender"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyFloat(SequenceManager sequences, ObjectGuid sender, PropertyFloat property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyFloat, GameMessageGroup.UIQueue)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyDouble));
            Writer.Write(sender.Full);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
