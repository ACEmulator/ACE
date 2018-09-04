using ACE.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventResetTrade : GameEventMessage
    {
        public GameEventResetTrade(Session session, ObjectGuid whoReset)
            : base(GameEventType.ResetTrade, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteGuid(whoReset);
        }
    }
}
