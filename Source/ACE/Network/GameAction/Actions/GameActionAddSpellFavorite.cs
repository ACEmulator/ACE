using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionAddSpellFavorite
    {
        [GameAction(GameActionType.AddSpellFavorite)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint spellId = message.Payload.ReadUInt32();
            uint spellBarPositionId = message.Payload.ReadUInt32();
            uint spellBarId = message.Payload.ReadUInt32();
            session.Player.AddSpellToSpellBar(spellId, spellBarPositionId, spellBarId);
        }
        #pragma warning restore 1998
    }
}
