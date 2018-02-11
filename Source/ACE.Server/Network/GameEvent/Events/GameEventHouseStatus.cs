namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventHouseStatus : GameEventMessage
    {
        public GameEventHouseStatus(Session session)
            : base(GameEventType.HouseStatus, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(2u);
        }
    }
}
