using ACE.Entity.Enum;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network
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
                session.Network.EnqueueSend(new GameMessageSystemChat(message, chatMessageType));
        }
    }
}
