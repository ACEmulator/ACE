using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdateInstanceID : GameMessage
    {
        public GameMessagePublicUpdateInstanceID(WorldObject worldObject, PropertyInstanceId property, ObjectGuid instanceGuid)
            : base(GameMessageOpcode.PublicUpdateInstanceId, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.UpdatePropertyInstanceID, property));
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write((uint)property);
            Writer.WriteGuid(instanceGuid);
        }
    }
}
