using ACE.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAcceptTrade : GameEventMessage
    {
        public GameEventAcceptTrade(Session session, ObjectGuid whoAccepted)
            : base(GameEventType.AcceptTrade, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteGuid(whoAccepted);
        }
    }
}
