
namespace ACE.Network.GameEvent.Events
{
    public class GameEventPopupString : GameEventMessage
    {
        private string message;

        public GameEventPopupString(Session session, string message) : base(GameEventType.PopupString, session)
        {
            this.message = message;
            writer.WriteString16L(message);
        }
    }
}
