
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAddSpellFavorite
    {
        [GameAction(GameActionType.AddSpellFavorite)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint spellId = message.Payload.ReadUInt32();
            uint spellBarPositionId = message.Payload.ReadUInt32();
            uint spellBarId = message.Payload.ReadUInt32();

            session.Player.HandleActionAddSpellFavorite(spellId, spellBarPositionId, spellBarId);
        }
    }
}
