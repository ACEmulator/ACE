using ACE.Entity.Enum;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionChannelList
    {
        [GameAction(GameActionType.ListChannels)]
        public static void Handle(ClientMessage message, Session session)
        {
            var chatChannelID = (Channel)message.Payload.ReadUInt32();

            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            session.Network.EnqueueSend(new GameEventChannelList(session, chatChannelID));
        }
    }
}
