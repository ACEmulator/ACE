using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAddChannel
    {
        [GameAction(GameActionType.AddChannel)]
        public static void Handle(ClientMessage message, Session session)
        {
            var chatChannelID = (Channel)message.Payload.ReadUInt32();
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            // TODO: Subscribe to channel (chatChannelID) and save to db. Channel subscriptions are meant to persist between sessions.
        }
    }
}
