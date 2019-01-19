using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionSpellbookFilter
    {
        [GameAction(GameActionType.SpellbookFilter)]
        public static void Handle(ClientMessage message, Session session)
        {
            var filters = (SpellBookFilterOptions)message.Payload.ReadUInt32();

            session.Player.HandleSpellbookFilters(filters);
        }
    }
}
