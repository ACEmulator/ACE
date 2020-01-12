using System;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionConfirmationResponse
    {
        [GameAction(GameActionType.ConfirmationResponse)]
        public static void Handle(ClientMessage message, Session session)
        {
            var confirmType = (ConfirmationType)message.Payload.ReadInt32();
            var context = message.Payload.ReadUInt32();
            var response = Convert.ToBoolean(message.Payload.ReadInt32());

            session.Player.ConfirmationManager.HandleResponse(confirmType, context, response);
        }
    }
}
