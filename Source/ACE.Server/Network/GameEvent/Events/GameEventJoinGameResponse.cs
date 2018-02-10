namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventJoinGameResponse : GameEventMessage
    {
        public GameEventJoinGameResponse(Session session, uint gameId, int team)
            : base(GameEventType.JoinGameResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(gameId);
            Writer.Write(team);
        }
    }
}
