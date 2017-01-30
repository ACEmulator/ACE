namespace ACE.Network.GameAction
{
    public abstract class GameActionPacket
    {
        protected Session session;
        protected ClientPacketFragment fragment;
        
        public GameActionPacket(Session session, ClientPacketFragment fragment)
        {
            this.session  = session;
            this.fragment = fragment;
        }

        // not all action packets have a body
        public virtual void Read() { }
        public abstract void Handle();

        [Fragment(FragmentOpcode.GameAction)]
        public static void HandleGameAction(ClientPacketFragment fragement, Session session)
        {
            uint sequence = fragement.Payload.ReadUInt32();
            uint opcode   = fragement.Payload.ReadUInt32();

            PacketManager.HandleGameAction((GameActionOpcode)opcode, fragement, session);
        }
    }
}
