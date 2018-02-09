using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.GameEvent
{
    public abstract class GameEventMessage : GameMessage
    {
        public GameEventType EventType { get; private set; }

        protected Session Session { get; private set; }

        protected GameEventMessage(GameEventType eventType, GameMessageGroup group, Session session) : base(GameMessageOpcode.GameEvent, group)
        {
            EventType = eventType;
            Session = session;

            // Force session to not be null -- due to races with player initialization
            session.WaitForPlayer();
            Writer.WriteGuid(session.Player.Guid);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)EventType);
        }
    }
}
