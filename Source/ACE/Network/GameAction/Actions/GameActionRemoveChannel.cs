
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRemoveChannel
    {
        [GameAction(GameActionType.RemoveChannel)]
        public static void Handle(ClientMessage message, Session session)
        {
            var chatChannelID = (GroupChatType)message.Payload.ReadUInt32();
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            // TODO: Unsubscribe from channel (chatChannelID) and save to db. Channel subscriptions are meant to persist between sessions.
        }
    }
}