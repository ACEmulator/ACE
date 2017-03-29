
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;

namespace ACE.Network.Handlers
{
    public static class DDDHandler
    {
        [GameMessage(GameMessageOpcode.DDD_InterrogationResponse, SessionState.AuthConnected)]
        public static void DDD_InterrogationResponse(ClientMessage message, Session session)
        {
            GameMessageDDDEndDDD patchStatusMessage = new GameMessageDDDEndDDD();
            session.Network.EnqueueSend(patchStatusMessage);
        }

        [GameMessage(GameMessageOpcode.DDD_EndDDD, SessionState.AuthConnected)]
        public static void DDD_EndDDD(ClientMessage message, Session session)
        {
            // We don't need to reply to this message.
        }
    }
}
