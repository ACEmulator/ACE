namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionClearAllegianceOfficers
    {
        [GameAction(GameActionType.ClearAllegianceOfficers)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionClearAllegianceOfficers();
        }
    }
}
