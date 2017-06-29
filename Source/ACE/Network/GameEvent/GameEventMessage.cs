using ACE.Network.GameMessages;
using System.Threading.Tasks;

namespace ACE.Network.GameEvent
{
    public abstract class GameEventMessage : GameMessage
    {
        public GameEventType EventType { get; private set; }

        protected Session Session { get; private set; }

        public GameEventMessage(GameEventType eventType, GameMessageGroup group, Session session) : base(GameMessageOpcode.GameEvent, group)
        {
            EventType = eventType;
            Session = session;

            // session.Player is null under race conditions of login and async DB loading.  Task this out.
            var t = new Task(() =>
            {
                while (session.Player == null)
                {
                    Task.Delay(10).Wait();
                }

                Writer.WriteGuid(session.Player.Guid);
                Writer.Write(session.GameEventSequence++);
                Writer.Write((uint)EventType);
            });

            t.Start();
        }
    }
}