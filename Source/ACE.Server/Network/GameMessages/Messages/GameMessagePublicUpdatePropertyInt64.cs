using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePropertyInt64 : GameMessage
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="worldObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public GameMessagePublicUpdatePropertyInt64(WorldObject worldObject, PropertyInt64 property, long value)
            : base(GameMessageOpcode.PublicUpdatePropertyInt64, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.UpdatePropertyInt64, property));
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
