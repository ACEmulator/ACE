namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRemoveSpellFavorite
    {
        [GameAction(GameActionType.RemoveSpellFavorite)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint spellId = message.Payload.ReadUInt32();
            uint spellBarId = message.Payload.ReadUInt32();
            session.Player.RemoveSpellToSpellBar(spellId, spellBarId);
        }
    }
}
