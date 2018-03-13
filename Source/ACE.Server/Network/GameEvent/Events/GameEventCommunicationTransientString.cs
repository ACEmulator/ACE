namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCommunicationTransientString : GameEventMessage
    {
        public GameEventCommunicationTransientString(Session session, string message)
            : base(GameEventType.CommunicationTransientString, GameMessageGroup.UIQueue, session)
        {
            // TODO: This seems to works, but compare with live pcap again
            Writer.WriteString16L(message);
        }
    }
}
