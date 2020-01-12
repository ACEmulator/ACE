using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAddToTrade : GameEventMessage
    {
        public GameEventAddToTrade(Session session, uint objectGuid, TradeSide tradeSide)
            : base(GameEventType.AddToTrade, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(objectGuid);
            Writer.Write((uint)tradeSide);
            Writer.Write(0);    // location / slot
        }
    }
}
