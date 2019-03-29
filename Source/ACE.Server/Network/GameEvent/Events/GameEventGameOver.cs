using ACE.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// End of chess game
    /// </summary>
    public class GameEventGameOver : GameEventMessage
    {
        public GameEventGameOver(Session session, ObjectGuid boardGuid, int teamWinner)
            : base(GameEventType.GameOver, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(boardGuid.Full);
            Writer.Write(teamWinner);
        }
    }
}
