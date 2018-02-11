namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventGameOver : GameEventMessage
    {
        public GameEventGameOver(Session session, uint gameId, int teamWinner)
            : base(GameEventType.GameOver, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(gameId);
            Writer.Write(teamWinner);
        }
    }
}
