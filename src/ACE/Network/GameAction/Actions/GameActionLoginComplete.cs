using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionLoginComplete
    {
        [GameAction(GameActionType.LoginComplete)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.InWorld = true;
            session.Player.SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);
        }
    }
}