using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionHouseQuery
    {
        [GameAction(GameActionType.HouseQuery)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameEventHouseStatus(session));
        }
    }
}