
using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventDisplayStatusMessage : GameEventMessage
    {
        public GameEventDisplayStatusMessage(Session session, StatusMessageType1 statusMessageType1) : base(GameEventType.DisplayStatusMessage, session)
        {
            Writer.Write((uint)statusMessageType1);
        }
    }
}
