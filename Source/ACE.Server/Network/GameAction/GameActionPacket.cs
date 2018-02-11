using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.Managers;

namespace ACE.Server.Network.GameAction
{
    public abstract class GameActionPacket
    {
        protected Session Session { get; private set; }

        protected ClientPacketFragment Fragment { get; private set; }

        protected GameActionPacket(Session session, ClientPacketFragment fragment)
        {
            Session  = session;
            Fragment = fragment;
        }

        // not all action packets have a body
        public virtual void Read() { }

        public abstract void Handle();

        [GameMessage(GameMessageOpcode.GameAction, SessionState.WorldConnected)]
        public static void HandleGameAction(ClientMessage message, Session session)
        {
            // TODO: verify sequence
            uint sequence = message.Payload.ReadUInt32();
            uint opcode   = message.Payload.ReadUInt32();

            InboundMessageManager.HandleGameAction((GameActionType)opcode, message, session);
        }
    }
}
