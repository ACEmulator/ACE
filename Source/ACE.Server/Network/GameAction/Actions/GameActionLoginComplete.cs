namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionLoginComplete
    {
        [GameAction(GameActionType.LoginComplete)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.InWorld = true;
            //session.Player.ReportCollision = true;
            //session.Player.IgnoreCollision = false;
            //session.Player.Hidden = false;
            session.Player.EnqueueBroadcastPhysicsState();
        }
    }
}
