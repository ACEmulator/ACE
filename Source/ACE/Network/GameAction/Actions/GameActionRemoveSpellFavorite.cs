using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRemoveSpellFavorite
    {
        [GameAction(GameActionType.RemoveSpellFavorite)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint spellId = message.Payload.ReadUInt32();
            uint spellBarId = message.Payload.ReadUInt32();
            session.Player.RemoveSpellToSpellBar(spellId, spellBarId);
        }
        #pragma warning restore 1998
    }
}
