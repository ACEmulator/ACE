using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyDataID : GameMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyDataID(WorldObject worldObject, PropertyDataId property, uint value)
            : base(GameMessageOpcode.PublicUpdatePropertyDataID, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.UpdatePropertyDataID, property));
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
