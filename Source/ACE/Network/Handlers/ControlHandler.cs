using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Entity;

namespace ACE.Network.Handlers
{
    public static class ControlHandler
    {
        [GameMessage(GameMessageOpcode.ForceObjectDescSend, SessionState.WorldConnected)]
        public static void ControlResponse(ClientMessage message, Session session)
        {
            var item = new ObjectGuid(message.Payload.ReadUInt32());
            session.Player.HandleActionForceObjDescSend(item);
        }
    }
}