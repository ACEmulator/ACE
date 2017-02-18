
namespace ACE.Network.GameEvent.Events
{
    public class GameEventHouseStatus : GameEventMessage
    {
        public GameEventHouseStatus(Session session) : base(GameEventType.HouseStatus, session)
        {
            writer.Write(2u);
        }
    }
}
