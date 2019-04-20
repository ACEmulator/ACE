using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionSoulEmote
    {
        [GameAction(GameActionType.SoulEmote)]
        public static void Handle(ClientMessage message, Session session)
        {
            var emote = message.Payload.ReadString16L();

            session.Player.HandleActionSoulEmote(emote);
        }
    }
}
