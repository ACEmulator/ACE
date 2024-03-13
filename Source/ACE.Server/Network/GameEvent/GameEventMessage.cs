using System;

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

        /// <param name="dataInitialCapacity">
        /// This is an optimization to help us seed the Data MemoryStream with an initial capacity.<para />
        /// MemoryStream starts off as 0 capacity, then the first use it initailizes an array of 256 bytes, then doubles each type capacity is reached.<para />
        /// By using the out of the box method for all, we can be over allocating for some opcodes, and re-allocating (via array doubling) for others<para />
        /// We're only helping Data with an initial capacity. If the MemoryStream needs more, it will still double itself and work as intended.
        /// </param>
        protected GameEventMessage(GameEventType eventType, GameMessageGroup group, Session session, int dataInitialCapacity) : base(GameMessageOpcode.GameEvent, group, dataInitialCapacity)
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
