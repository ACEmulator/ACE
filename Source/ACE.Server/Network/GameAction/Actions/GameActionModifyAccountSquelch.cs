using System;
using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionModifyAccountSquelch
    {
        [GameAction(GameActionType.ModifyAccountSquelch)]
        public static void Handle(ClientMessage message, Session session)
        {
            var squelch = Convert.ToBoolean(message.Payload.ReadUInt32());
            var playerName = message.Payload.ReadString16L();

            session.Player.SquelchManager.HandleActionModifyAccountSquelch(squelch, playerName);
        }
    }
}
