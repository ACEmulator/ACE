using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyFloat : GameMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyFloat(WorldObject worldObject, PropertyFloat property, double value)
            : base(GameMessageOpcode.PublicUpdatePropertyFloat, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.UpdatePropertyDouble, property));
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
