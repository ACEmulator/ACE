using ACE.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventTradeFailure : GameEventMessage
    {
        public GameEventTradeFailure(Session session, ObjectGuid item)
            : base(GameEventType.TradeFailure, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteGuid(item);
        }
    }
}
