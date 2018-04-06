using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionEmote
    {
        [GameAction(GameActionType.Emote)]
        public static void Handle(ClientMessage message, Session session)
        {
            var emote = message.Payload.ReadString16L();

            session.Player.HandleActionEmote(emote);
        }
    }
}
