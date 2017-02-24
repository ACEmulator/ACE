namespace ACE.Network.GameEvent.Events
{
    public class GameEventHouseStatus : GameEventMessage
    {
        public GameEventHouseStatus(Session session)
            : base(GameEventType.HouseStatus, 0x9, session)
        {
            Writer.Write(2u);
        }
    }
}
