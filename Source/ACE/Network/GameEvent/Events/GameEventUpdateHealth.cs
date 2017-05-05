namespace ACE.Network.GameEvent.Events
{
    public class GameEventUpdateHealth : GameEventMessage
    {
        public GameEventUpdateHealth(Session session, uint objectid, float health)
            : base(GameEventType.UpdateHealth, GameMessageGroup.Group09, session)
        {
            Writer.Write(objectid);
            Writer.Write(health);
        }
    }
}
