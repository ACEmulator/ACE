﻿
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
        public static void HandleGameAction(ClientPacketFragment fragement, Session session)
        {
            // TODO: verify sequence
            uint sequence = fragement.Payload.ReadUInt32();
            uint opcode   = fragement.Payload.ReadUInt32();

            InboundMessageManager.HandleGameAction((GameActionType)opcode, fragement, session);
        }
    }
}
