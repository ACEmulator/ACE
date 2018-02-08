namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventConfirmationDone : GameEventMessage
    {
        public GameEventConfirmationDone(Session session, int confirmationType, uint context)
            : base(GameEventType.CharacterConfirmationDone, GameMessageGroup.Group09, session)
        {
            // TODO: implement
            // TODO: verify GameMessageGroup with pcap data
            Writer.Write(confirmationType);
            Writer.Write(context);
        }
    }
}
