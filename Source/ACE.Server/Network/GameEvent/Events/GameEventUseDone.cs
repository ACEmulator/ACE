namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventUseDone : GameEventMessage
    {
        public GameEventUseDone(Session session)
            : base(GameEventType.UseDone, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(0x00);
        }
    }
}