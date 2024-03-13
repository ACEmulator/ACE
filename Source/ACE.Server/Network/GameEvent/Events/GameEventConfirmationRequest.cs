using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventConfirmationRequest : GameEventMessage
    {
        public GameEventConfirmationRequest(Session session, ConfirmationType confirmationType, uint context, string text)
            : base(GameEventType.CharacterConfirmationRequest, GameMessageGroup.UIQueue, session, 172) // 172 is the max seen in retail pcaps
        {
            Writer.Write((uint)confirmationType);
            Writer.Write(context);
            Writer.WriteString16L(text);
        }
    }
}
