
using ACE.Network.Enum;
using ACE.Network.GameMessages;

namespace ACE.Network.Handlers
{
    public static class DDDHandler
    {
        [GameMessage(GameMessageOpcode.DDD_InterrogationResponse, SessionState.AuthConnected)]
        public static void DDD_InterrogationResponse(ClientPacketFragment fragment, Session session)
        {
            // Because we're not delivering any content at this time, we can instruct the client to end the DDD session.
        }

        [GameMessage(GameMessageOpcode.DDD_EndDDD, SessionState.AuthConnected)]
        public static void DDD_EndDDD(ClientPacketFragment fragment, Session session)
        {
            // We don't need to reply to this message.
        }
    }
}
