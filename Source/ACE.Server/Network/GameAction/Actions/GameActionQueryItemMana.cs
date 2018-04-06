using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionQueryItemMana
    {
        [GameAction(GameActionType.QueryItemMana)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint fullId = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(fullId);

            session.Player.HandleActionQueryItemMana(guid);
        }
    }
}
