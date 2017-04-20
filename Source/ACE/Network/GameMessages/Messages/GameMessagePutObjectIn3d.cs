using ACE.Entity;
using ACE.Entity.Objects;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePutObjectIn3d : GameMessage
    {
        public GameMessagePutObjectIn3d(Session session, WorldObject worldObject, ObjectGuid itemGuid)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(session.GameEventSequence.NextValue);
            Writer.Write((uint)GameEvent.GameEventType.DropTemp);
            Writer.Write(itemGuid.Full);
            Writer.Align();
        }
    }
}