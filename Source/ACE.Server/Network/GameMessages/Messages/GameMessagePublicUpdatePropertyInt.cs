using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyInt : GameMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyInt(WorldObject worldObject, PropertyInt property, int value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.UpdatePropertyInt, property));
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
