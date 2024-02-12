using ACE.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventDeclineTrade : GameEventMessage
    {
        public GameEventDeclineTrade(Session session, ObjectGuid whoDeclined)
            : base(GameEventType.DeclineTrade, GameMessageGroup.UIQueue, session, 8)
        {
            Writer.WriteGuid(whoDeclined);
        }
    }
}
