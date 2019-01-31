namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionClearAllegianceName
    {
        [GameAction(GameActionType.ClearAllegianceName)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionClearAllegianceName();
        }
    }
}
