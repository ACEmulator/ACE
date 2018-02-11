using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
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