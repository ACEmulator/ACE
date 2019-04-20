using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAddChannel
    {
        [GameAction(GameActionType.AddChannel)]
        public static void Handle(ClientMessage message, Session session)
        {
            var chatChannelID = (Channel)message.Payload.ReadUInt32();

            if (session.AccessLevel == AccessLevel.Player && !session.Player.IsAdvocate)
                return;

            if (session.Player.ChannelsAllowed.HasValue && session.Player.ChannelsAllowed.Value.HasFlag(chatChannelID))
            {
                if (session.Player.ChannelsActive.HasValue)
                    session.Player.ChannelsActive |= chatChannelID;
                else
                    session.Player.ChannelsActive = chatChannelID;
                session.Network.EnqueueSend(new GameEvent.Events.GameEventWeenieErrorWithString(session, WeenieErrorWithString.YouHaveEnteredThe_Channel, chatChannelID.ToString()));
            }
            else
                return;
        }
    }
}
