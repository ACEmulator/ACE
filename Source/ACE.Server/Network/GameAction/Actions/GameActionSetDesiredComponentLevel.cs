namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Handles the spell 'Components' tab for /fillcomps
    /// </summary>
    public static class GameActionSetDesiredComponentLevel
    {
        [GameAction(GameActionType.SetDesiredComponentLevel)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint component_wcid = message.Payload.ReadUInt32();
            uint amount = message.Payload.ReadUInt32();

            session.Player.HandleSetDesiredComponentLevel(component_wcid, amount);
        }
    }
}
