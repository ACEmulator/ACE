using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionEmote
    {
        [GameAction(GameActionType.Emote)]
        public static void Handle(ClientMessage message, Session session)
        {
            var emote = message.Payload.ReadString16L();
            // TODO: send emote text to other characters
            // The emote text comes from the client ready to broadcast.
            // For example: *afk* comes as "decides to rest for a while."
        }
    }
}
