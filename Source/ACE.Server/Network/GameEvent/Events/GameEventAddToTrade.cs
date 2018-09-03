using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAddToTrade : GameEventMessage
    {
        public GameEventAddToTrade(Session session, ObjectGuid item, TradeSide tradeSide)
            : base(GameEventType.AddToTrade, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteGuid(item);
            Writer.Write((uint)tradeSide);
        }
    }
}
