namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionLoginComplete
    {
        [GameAction(GameActionType.LoginComplete)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.InWorld = true;
            session.Player.ReportCollisions = true;
            session.Player.IgnoreCollisions = false;
            session.Player.Hidden = false;
            session.Player.EnqueueBroadcastPhysicsState();
            session.Player.Teleporting = false;
        }
    }
}
