namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventVictimNotification : GameEventMessage
    {
        public GameEventVictimNotification(Session session, string deathMessage)
            : base(GameEventType.VictimNotification, GameMessageGroup.UIQueue, session)
        {
            // sent to player when they die
            Writer.WriteString16L(deathMessage);
        }
    }
}
