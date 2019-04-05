using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAllegianceChatBoot
    {
        [GameAction(GameActionType.AllegianceChatBoot)]
        public static void Handle(ClientMessage message, Session session)
        {
            var playerName = message.Payload.ReadString16L();
            var reason = message.Payload.ReadString16L();

            session.Player.HandleActionAllegianceChatBoot(playerName, reason);
        }
    }
}
