using System.Threading.Tasks;

using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionHouseQuery
    {
        [GameAction(GameActionType.HouseQuery)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameEventHouseStatus(session));
        }
        #pragma warning restore 1998
    }
}
