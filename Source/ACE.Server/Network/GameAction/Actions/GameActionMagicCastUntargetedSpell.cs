namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionMagicCastUnTargetedSpell
    {
        [GameAction(GameActionType.CastUntargetedSpell)]
        public static void Handle(ClientMessage message, Session session)
        {
            var spellId = message.Payload.ReadUInt32();

            session.Player.HandleActionMagicCastUnTargetedSpell(spellId);
        }
    }
}
