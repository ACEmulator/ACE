namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventEvasionAttackerNotification : GameEventMessage
    {
        public GameEventEvasionAttackerNotification(Session session, string defenderName)
            : base(GameEventType.EvasionAttackerNotification, GameMessageGroup.UIQueue, session, 48) // 48 is the max seen in retail pcaps
        {
            Writer.WriteString16L(defenderName);
        }
    }
}
