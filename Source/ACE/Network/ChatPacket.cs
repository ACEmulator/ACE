
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
            {
                session.WorldSession.EnqueueSend(new GameMessageSystemChat(message, chatMessageType));
                session.WorldSession.Flush();
            }
        }
        //Overloaded SendServerMessage to include a flush boolean
        //This was necessary for chat window packets; the packets were not being handled by the client correctly
        // when they were being sent without the flush. Strange chars and line breaks were inconsistently visible in the chat
        // window, despite the packet structure looking correct.
        public static void SendServerMessage(Session session, string message, ChatMessageType chatMessageType, bool flush)
        {
            if (session == null)
            {
                // TODO: broadcast
            }
            else
            {
                if (flush)
                {
                    session.WorldSession.EnqueueSend(new GameMessageSystemChat(message, chatMessageType));
                    session.WorldSession.Flush();
                }
                else
                {
                    session.WorldSession.EnqueueSend(new GameMessageSystemChat(message, chatMessageType));
                }
            }
        }
    }
}
