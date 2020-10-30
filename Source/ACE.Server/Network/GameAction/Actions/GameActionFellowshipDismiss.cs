namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionFellowshipDismiss
    {
        [GameAction(GameActionType.FellowshipDismiss)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint playerIdToDismiss = message.Payload.ReadUInt32();

            if (session.Player.Fellowship != null)
                session.Player.FellowshipDismissPlayer(playerIdToDismiss);
        }
    }
}
