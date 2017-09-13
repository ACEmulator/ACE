using ACE.Entity;
using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionConfirmationResponse
    {
        [GameAction(GameActionType.ConfirmationResponse)]
        public static void Handle(ClientMessage message, Session session)
        {
            int confirmType = message.Payload.ReadInt32();
            uint context = message.Payload.ReadUInt32();
            int response = message.Payload.ReadInt32();
            session.Player.HandleActionConfirmationResponse((ConfirmationType)confirmType, context, response);
        }
    }
}
