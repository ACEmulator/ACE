using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSetSquelchDB : GameEventMessage
    {
        public GameEventSetSquelchDB(Session session, SquelchDB db)
            : base(GameEventType.SetSquelchDB, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(db);
        }
    }
}
