using ACE.Entity.Enum;
using ACE.Network.GameMessages.Messages;

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
                session.EnqueueSend(new GameMessageSystemChat(message, chatMessageType));
        }
    }
}
