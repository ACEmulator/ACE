using System;
using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionBreakAllegianceBoot
    {
        [GameAction(GameActionType.BreakAllegianceBoot)]
        public static void Handle(ClientMessage message, Session session)
        {
            var playerName = message.Payload.ReadString16L();
            var accountBoot = Convert.ToBoolean(message.Payload.ReadUInt32());

            session.Player.HandleActionBreakAllegianceBoot(playerName, accountBoot);
        }
    }
}
