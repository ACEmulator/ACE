namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventCommunicationTransientString : GameEventMessage
    {
        public GameEventCommunicationTransientString(Session session, string message)
            : base(GameEventType.CommunicationTransientString, GameMessageGroup.UIQueue, session, 156) // 156 is the max seen in retail pcaps
        {
            Writer.WriteString16L(message);
        }
    }
}
