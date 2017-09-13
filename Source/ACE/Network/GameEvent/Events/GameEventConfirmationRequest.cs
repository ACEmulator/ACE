namespace ACE.Network.GameEvent.Events
{
    public class GameEventConfirmationRequest : GameEventMessage
    {
        public GameEventConfirmationRequest(Session session, int confirmationType, uint context, string text)
            : base(GameEventType.ConfirmationRequest, GameMessageGroup.Group09, session)
        {
            Writer.Write(confirmationType);
            Writer.Write(context);
            Writer.WriteString16L(text);
        }
    }
}
