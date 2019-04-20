using ACE.Entity;
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

            var guid = session.Player != null ? session.Player.Guid : new ObjectGuid(0);

            Writer.WriteGuid(guid);
            //var debugMessage = $"GameEventSequence Update - {eventType} - GameEventSequence was {session.GameEventSequence}";
            Writer.Write(session.GameEventSequence++);
            //Console.WriteLine(debugMessage + $" and is now {session.GameEventSequence}");
            Writer.Write((uint)EventType);
        }
    }
}
