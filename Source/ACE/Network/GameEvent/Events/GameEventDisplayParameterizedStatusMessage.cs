using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventDisplayParameterizedStatusMessage : GameEventMessage
    {
        public GameEventDisplayParameterizedStatusMessage(Session session, StatusMessageType2 statusMessageType2, string message) 
            : base(GameEventType.DisplayParameterizedStatusMessage, GameMessageGroup.Group09, session)
        {
            Writer.Write((uint)statusMessageType2);
            Writer.WriteString16L(message);
        }
    }
}
