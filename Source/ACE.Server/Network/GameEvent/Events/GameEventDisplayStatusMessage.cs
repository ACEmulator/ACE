using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventDisplayStatusMessage : GameEventMessage
    {
        public GameEventDisplayStatusMessage(Session session, StatusMessageType1 statusMessageType1)
            : base(GameEventType.DisplayStatusMessage, GameMessageGroup.UIQueue, session)
        {
            Writer.Write((uint)statusMessageType1);
        }
    }
}
