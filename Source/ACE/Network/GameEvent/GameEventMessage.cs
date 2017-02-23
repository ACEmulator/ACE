
using ACE.Network.GameMessages;

namespace ACE.Network.GameEvent
{
    public abstract class GameEventMessage : GameMessage
    {
        public GameEventType EventType { get; private set; }

        protected Session Session { get; private set; }

        public GameEventMessage(GameEventType eventType, ushort group, Session session) : base(GameMessageOpcode.GameEvent, group)
        {
            EventType = eventType;
            Session = session;

            Writer.WriteGuid(session.Player.Guid);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)EventType);
        }
    }
}
