using System.Threading.Tasks;

using ACE.Entity.Enum;
using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionChannelList
    {
        [GameAction(GameActionType.ListChannels)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var chatChannelID = (GroupChatType)message.Payload.ReadUInt32();
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            session.Network.EnqueueSend(new GameEventChannelList(session, chatChannelID));
        }
        #pragma warning restore 1998
    }
}
