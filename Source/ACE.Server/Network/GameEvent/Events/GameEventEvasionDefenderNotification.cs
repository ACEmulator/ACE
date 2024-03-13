namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventEvasionDefenderNotification : GameEventMessage
    {
        public GameEventEvasionDefenderNotification(Session session, string attackerName)
            : base(GameEventType.EvasionDefenderNotification, GameMessageGroup.UIQueue, session, 48) // 48 is the max seen in retail pcaps
        {
            Writer.WriteString16L(attackerName);
        }
    }
}
