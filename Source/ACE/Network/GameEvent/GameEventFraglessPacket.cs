using System.Diagnostics;

namespace ACE.Network.GameEvent
{
    public abstract class GameEventFraglessPacket
    {
        public abstract GameEventOpcode Opcode { get; }

        protected Session session;
        protected ServerPacketFragment fragment;

        public GameEventFraglessPacket(Session target)
        {
            Debug.Assert(target.Player != null);

            session = target;
            fragment = new ServerPacketFragment(0x0A, GameMessageOpcode.None);
        }

        public void Send()
        {
            var gameEvent = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            fragment.Payload.Write((uint)Opcode);
            WriteEventBody();
            gameEvent.Fragments.Add(fragment);

            NetworkManager.SendPacket(ConnectionType.World, gameEvent, session);
        }

        protected virtual void WriteEventBody() { }
    }
}
