using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRemoveChannel
    {
        [GameAction(GameActionType.RemoveChannel)]
        public static void Handle(ClientMessage message, Session session)
        {
            var chatChannelID = (Channel)message.Payload.ReadUInt32();

            if (session.AccessLevel == AccessLevel.Player && !session.Player.IsAdvocate)
                return;

            if (session.Player.ChannelsActive.HasValue && session.Player.ChannelsActive.Value.HasFlag(chatChannelID))
            {
                session.Player.ChannelsActive &= ~chatChannelID;
                session.Network.EnqueueSend(new GameEvent.Events.GameEventWeenieErrorWithString(session, WeenieErrorWithString.YouHaveLeftThe_Channel, chatChannelID.ToString()));
            }
            else
                return;
        }
    }
}
