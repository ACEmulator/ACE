namespace ACE.Network.GameAction
{
    public abstract class GameActionMessage
    {
        protected Session session;
        protected ClientPacketFragment fragment;
        
        public GameActionMessage(Session session, ClientPacketFragment fragment)
        {
            this.session  = session;
            this.fragment = fragment;
        }

        // not all action packets have a body
        public virtual void Read() { }
        public abstract void Handle();

        [Fragment(GameMessageOpcode.GameAction, SessionState.WorldConnected)]
        public static void HandleGameAction(ClientPacketFragment fragement, Session session)
        {
            // TODO: verify sequence
            uint sequence = fragement.Payload.ReadUInt32();
            uint opcode   = fragement.Payload.ReadUInt32();

            PacketManager.HandleGameAction((GameActionOpcode)opcode, fragement, session);
        }
    }
}
