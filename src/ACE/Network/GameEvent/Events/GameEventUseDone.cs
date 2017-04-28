namespace ACE.Network.GameEvent.Events
{
    public class GameEventUseDone : GameEventMessage
    {
        public GameEventUseDone(Session session)
            : base(GameEventType.UseDone, GameMessageGroup.Group09, session)
        {
            Writer.Write(0x00);
        }
    }
}