using System.Diagnostics;

using ACE.Network.Messages;
using ACE.Network.Managers;

namespace ACE.Network.GameEvent
{
    public abstract class GameEventMessage : GameMessage
    {
        public GameEventType EventType { get; private set; }
        protected Session session;

        public GameEventMessage(GameEventType eventType, Session session) : base(GameMessageOpcode.GameEvent)
        {
            this.EventType = eventType;
            this.session = session;
            writer.WriteGuid(session.Player.Guid);
            writer.Write(session.GameEventSequence++);
            writer.Write((uint)EventType);
        }
    }
}
