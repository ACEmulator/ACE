
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionDeclineTrade
    {
        [GameAction(GameActionType.DeclineTrade)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionDeclineTrade(session);
        }
    }
}
