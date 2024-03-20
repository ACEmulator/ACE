using ACE.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventResetTrade : GameEventMessage
    {
        public GameEventResetTrade(Session session, ObjectGuid whoReset)
            : base(GameEventType.ResetTrade, GameMessageGroup.UIQueue, session, 8)
        {
            Writer.WriteGuid(whoReset);
        }
    }
}
