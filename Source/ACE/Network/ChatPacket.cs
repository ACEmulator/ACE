
using ACE.Entity.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;

namespace ACE.Network
{
    //TODO Refactor this into Session?  This is used in alot of places currently.
    public static class ChatPacket
    {
        public static void SendServerMessage(Session session, string message, ChatMessageType chatMessageType)
        {
            if (session == null)
            {
                // TODO: broadcast
            }
            else
                session.WorldSession.Enqueue(new GameMessageSystemChat(message, chatMessageType));
        }
    }
}
