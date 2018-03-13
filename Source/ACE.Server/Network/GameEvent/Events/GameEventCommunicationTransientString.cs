namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCommunicationTransientString : GameEventMessage
    {
        public GameEventCommunicationTransientString(Session session, string message)
            : base(GameEventType.CommunicationTransientString, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteString16L(message);
        }
    }
}
