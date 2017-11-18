using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventGameOver : GameEventMessage
    {
        public GameEventGameOver(Session session, uint gameId, int teamWinner)
            : base(GameEventType.GameOver, GameMessageGroup.Group09, session)
        {
            Writer.Write(gameId);
            Writer.Write(teamWinner);
        }
    }
}
