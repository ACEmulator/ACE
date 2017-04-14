using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionChannelList
    {
        [GameAction(GameActionType.ListChannels)]
        public static void Handle(ClientMessage message, Session session)
        {
            var chatChannelID = (GroupChatType)message.Payload.ReadUInt32();
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            session.EnqueueSend(new GameEventChannelList(session, chatChannelID));
        }
    }
}