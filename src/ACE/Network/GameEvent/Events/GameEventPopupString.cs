namespace ACE.Network.GameEvent.Events
{
    public class GameEventPopupString : GameEventMessage
    {
        public GameEventPopupString(Session session, string message)
            : base(GameEventType.PopupString, GameMessageGroup.Group09, session)
        {
            Writer.WriteString16L(message);
        }
    }
}
