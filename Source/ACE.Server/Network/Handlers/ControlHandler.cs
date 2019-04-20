using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.Handlers
{
    public static class ControlHandler
    {
        [GameMessage(GameMessageOpcode.ForceObjectDescSend, SessionState.WorldConnected)]
        public static void ControlResponse(ClientMessage message, Session session)
        {
            var itemGuid = message.Payload.ReadUInt32();
            session.Player.HandleActionForceObjDescSend(itemGuid);
        }
    }
}
