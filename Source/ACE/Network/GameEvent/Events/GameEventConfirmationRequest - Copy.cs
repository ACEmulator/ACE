namespace ACE.Network.GameEvent.Events
{
    public class GameEventConfirmationDone : GameEventMessage
    {
        public GameEventConfirmationDone(Session session, int confirmationType, uint context)
            : base(GameEventType.ConfirmationDone, GameMessageGroup.Group09, session)
        {
            Writer.Write(confirmationType);
            Writer.Write(context);
        }
    }
}
