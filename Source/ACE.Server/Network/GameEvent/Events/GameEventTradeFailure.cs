using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventTradeFailure : GameEventMessage
    {
        public GameEventTradeFailure(Session session, ObjectGuid item, WeenieError reason)
            : base(GameEventType.TradeFailure, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteGuid(item);
            Writer.Write((uint)reason);
        }
    }
}
