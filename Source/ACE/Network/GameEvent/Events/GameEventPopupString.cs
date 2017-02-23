
namespace ACE.Network.GameEvent.Events
{
    public class GameEventPopupString : GameEventMessage
    {
        public GameEventPopupString(Session session, string message) : base(GameEventType.PopupString, 0x9, session)
        {
            Writer.WriteString16L(message);
        }
    }
}
