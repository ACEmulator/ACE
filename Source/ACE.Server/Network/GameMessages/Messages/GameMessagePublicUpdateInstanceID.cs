using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdateInstanceID : GameMessage
    {
        public GameMessagePublicUpdateInstanceID(WorldObject worldObject, PropertyInstanceId iidPropertyId, ObjectGuid instanceGuid)
            : base(GameMessageOpcode.PublicUpdateInstanceId, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInstanceId));  // wts
            Writer.WriteGuid(worldObject.Guid); // sender
            Writer.Write((uint)iidPropertyId);
            Writer.WriteGuid(instanceGuid); // new value of the container id
        }
    }
}
