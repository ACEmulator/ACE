using System.Threading.Tasks;

using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Entity;

namespace ACE.Network.Handlers
{
    public static class ControlHandler
    {
        [GameMessage(GameMessageOpcode.ForceObjectDescSend, SessionState.WorldConnected)]
        #pragma warning disable 1998
        public static async Task ControlResponse(ClientMessage message, Session session)
        {
            var item = new ObjectGuid(message.Payload.ReadUInt32());
            session.Player.ForceObjDescSend(item);
        }
        #pragma warning restore 1998
    }
}
