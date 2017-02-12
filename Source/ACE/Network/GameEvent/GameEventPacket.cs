using System.Diagnostics;

namespace ACE.Network.GameEvent
{
    public abstract class GameEventPacket
    {
        public abstract GameEventOpcode Opcode { get; }

        protected Session session;
        protected ServerPacketFragment fragment;

        public GameEventPacket(Session target)
        {
            Debug.Assert(target.Player != null);

            session  = target;
            fragment = new ServerPacketFragment(9, FragmentOpcode.GameEvent);
        }

        public void Send()
        {
            var gameEvent = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            fragment.Payload.WriteGuid(session.Player.Guid);
            fragment.Payload.Write(session.GameEventSequence++);
            fragment.Payload.Write((uint)Opcode);
            WriteEventBody();
            gameEvent.Fragments.Add(fragment);

            NetworkManager.SendPacket(ConnectionType.World, gameEvent, session);
        }

        protected virtual void WriteEventBody() { }
    }
}
