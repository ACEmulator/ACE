
namespace ACE.Server.Network.GameAction.Actions
{
    // Death feels is less morbid then suicide as a human, used "Die" instead.
    public static class GameActionDie
    {
        [GameAction(GameActionType.Suicide)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionDie();
        }
    }
}
