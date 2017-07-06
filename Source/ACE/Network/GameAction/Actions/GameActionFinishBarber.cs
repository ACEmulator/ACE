using ACE.Entity.Actions;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionFinishBarber
    {
        [GameAction(GameActionType.FinishBarber)]
        public static void Handle(ClientMessage message, Session session)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(session.Player, () => session.Player.HandleActionFinishBarber(message));
            chain.EnqueueChain();
        }
    }
}
