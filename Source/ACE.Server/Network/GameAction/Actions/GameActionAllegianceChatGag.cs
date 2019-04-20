using System;
using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAllegianceChatGag
    {
        [GameAction(GameActionType.AllegianceChatGag)]
        public static void Handle(ClientMessage message, Session session)
        {
            var playerName = message.Payload.ReadString16L();
            var enabled = Convert.ToBoolean(message.Payload.ReadUInt32());

            session.Player.HandleActionAllegianceChatGag(playerName, enabled);
        }
    }
}
