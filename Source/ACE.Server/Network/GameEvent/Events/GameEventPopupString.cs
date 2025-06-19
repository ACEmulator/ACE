// UTF-8 BOM removed to ensure consistent encoding
namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventPopupString : GameEventMessage
    {
        public GameEventPopupString(Session session, string message)
            : base(GameEventType.PopupString, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteString16L(message);
        }
    }
}
