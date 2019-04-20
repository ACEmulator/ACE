using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyString : GameMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyString(WorldObject worldObject, PropertyString property, string value)
            : base(GameMessageOpcode.PublicUpdatePropertyString, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.UpdatePropertyString, property));
            Writer.Write((uint)property);
            Writer.WriteGuid(worldObject.Guid);
            Writer.Align();
            Writer.WriteString16L(value);
        }
    }
}
