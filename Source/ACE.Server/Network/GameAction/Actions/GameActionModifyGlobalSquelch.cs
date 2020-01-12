using System;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Changes the global filters, /filter -type as well as /chat and /notell
    /// </summary>
    public static class GameActionModifyGlobalSquelch
    {
        [GameAction(GameActionType.ModifyGlobalSquelch)]
        public static void Handle(ClientMessage message, Session session)
        {
            var squelch = Convert.ToBoolean(message.Payload.ReadUInt32());
            var messageType = (ChatMessageType)message.Payload.ReadUInt32();

            session.Player.SquelchManager.HandleActionModifyGlobalSquelch(squelch, messageType);
        }
    }
}
