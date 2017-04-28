namespace ACE.Network.GameEvent.Events
{
    public class GameEventHouseStatus : GameEventMessage
    {
        public GameEventHouseStatus(Session session)
            : base(GameEventType.HouseStatus, GameMessageGroup.Group09, session)
        {
            Writer.Write(2u);
        }
    }
}
