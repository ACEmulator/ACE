using System.Threading.Tasks;

using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction
{
    public abstract class GameActionPacket
    {
        protected Session Session { get; private set; }

        protected ClientPacketFragment Fragment { get; private set; }

        public GameActionPacket(Session session, ClientPacketFragment fragment)
        {
            Session  = session;
            Fragment = fragment;
        }

        // not all action packets have a body
        public virtual void Read() { }

        public abstract void Handle();

        [GameMessageAttribute(GameMessageOpcode.GameAction, SessionState.WorldConnected)]
        #pragma warning disable 1998
        public static async Task HandleGameAction(ClientMessage message, Session session)
        {
            // TODO: verify sequence
            uint sequence = message.Payload.ReadUInt32();
            uint opcode   = message.Payload.ReadUInt32();

            InboundMessageManager.HandleGameAction((GameActionType)opcode, message, session);
        }
        #pragma warning restore 1998
    }
}
