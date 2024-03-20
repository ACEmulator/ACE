namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventKillerNotification : GameEventMessage
    {
        public GameEventKillerNotification(Session session, string deathMessage)
            : base(GameEventType.KillerNotification, GameMessageGroup.UIQueue, session, 120) // 120 is the max seen in retail pcaps
        {
            // sent to player when they kill something
            Writer.WriteString16L(deathMessage);
        }
    }
}
