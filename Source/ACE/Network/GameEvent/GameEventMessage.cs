using System.Diagnostics;

namespace ACE.Network.GameEvent
{
    public abstract class GameEventMessage : GameMessage
    {
        public abstract GameEventOpcode EventType { get; } 

        public GameEventMessage(Session target) : base(target, GameMessageOpcode.GameEvent)
        {
            //fragment = new ServerPacketFragment(9, GameMessageOpcode.GameEvent);
        }

        //public void Send()
        //{
        //    var gameEvent = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);

        //    gameEvent.Fragments.Add(fragment);

        //    NetworkManager.SendPacket(ConnectionType.World, gameEvent, session);
        //}

        protected override void WriteBody()
        {
            writer.WriteGuid(session.Player.Guid);
            writer.Write(session.GameEventSequence++);
            writer.Write((uint)EventType);
            WriteEventBody();
        }

        protected virtual void WriteEventBody() { }
    }
}
