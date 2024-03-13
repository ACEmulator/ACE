namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventQueryAgeResponse : GameEventMessage
    {
        public GameEventQueryAgeResponse(Session session, string targetName, string age)
            : base(GameEventType.QueryAgeResponse, GameMessageGroup.UIQueue, session, 32) // 32 is the max seen in retail pcaps
        {
            Writer.WriteString16L(targetName);
            Writer.WriteString16L(age);
        }
    }
}
