namespace ACE.Network.GameEvent.Events
{
    public class GameEventDeathNotice : GameEventMessage
    {
        public GameEventDeathNotice(Session session, string message)
            : base(GameEventType.KillerNotification, GameMessageGroup.Group09, session)
        {
            // TODO: This seems to works, but compare with live pcap again
            Writer.WriteString16L(message);
        }
    }
}
