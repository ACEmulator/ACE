using System.Threading.Tasks;

using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;

namespace ACE.Network.Handlers
{
    public static class DDDHandler
    {
        [GameMessage(GameMessageOpcode.DDD_InterrogationResponse, SessionState.AuthConnected)]
        #pragma warning disable 1998
        public static async Task DDD_InterrogationResponse(ClientMessage message, Session session)
        {
            GameMessageDDDEndDDD patchStatusMessage = new GameMessageDDDEndDDD();
            session.Network.EnqueueSend(patchStatusMessage);
        }
        #pragma warning restore 1998

        [GameMessage(GameMessageOpcode.DDD_EndDDD, SessionState.AuthConnected)]
        #pragma warning disable 1998
        public static async Task DDD_EndDDD(ClientMessage message, Session session)
        {
            // We don't need to reply to this message.
        }
        #pragma warning restore 1998
    }
}
