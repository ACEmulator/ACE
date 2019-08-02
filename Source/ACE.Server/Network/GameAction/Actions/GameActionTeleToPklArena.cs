namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Teleports a player killer lite to the PKLite arena
    /// </summary>
    public static class GameActionTeleToPklArena
    {
        [GameAction(GameActionType.TeleToPklArena)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionTeleToPklArena();
        }
    }
}
