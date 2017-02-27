
using ACE.Entity.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;

namespace ACE.Network
{
    public static class ChatPacket
    {
        public static void SendServerMessage(Session session, string message, ChatMessageType chatMessageType)
        {
            if (session == null)
            {
                // TODO: broadcast
            }
            else
                session.WorldSession.EnqueueSend(new GameMessageSystemChat(message, chatMessageType));
        }
    }
}
