using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventUseDone : GameEventMessage
    {
        public GameEventUseDone(Session session, WeenieError errorType = WeenieError.None)
            : base(GameEventType.UseDone, GameMessageGroup.UIQueue, session, 8)
        {
            Writer.Write((uint)errorType);
        }
    }
}
