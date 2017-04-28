using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventQueryAgeResponse : GameEventMessage
    {
        public GameEventQueryAgeResponse(Session session, string targetName, string age)
            : base(GameEventType.QueryAgeResponse, GameMessageGroup.Group09, session)
        {
            Writer.WriteString16L(targetName);
            Writer.WriteString16L(age);
        }
    }
}
