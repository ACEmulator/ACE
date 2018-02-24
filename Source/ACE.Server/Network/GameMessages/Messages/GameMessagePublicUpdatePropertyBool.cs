using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using System;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyBool : GameMessage
    {
        /// <summary>
        /// Public Update of PropertyBool
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="sender"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyBool(SequenceManager sequences, ObjectGuid sender, PropertyBool property, bool value)
            : base(GameMessageOpcode.PublicUpdatePropertyBool, GameMessageGroup.UIQueue)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyBool));
            Writer.Write(sender.Full);
            Writer.Write((uint)property);
            Writer.Write(Convert.ToInt32(value));
        }
    }
}
