using System.Threading.Tasks;

using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionChannelIndex
    {
        [GameAction(GameActionType.IndexChannels)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            session.Network.EnqueueSend(new GameEventChannelIndex(session));
        }
        #pragma warning restore 1998
    }
}
