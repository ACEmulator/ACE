using System.Threading.Tasks;
using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionQueryItemMana
    {
        [GameAction(GameActionType.QueryItemMana)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint fullId = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(fullId);

            session.Player.QueryItemMana(guid);
        }
        #pragma warning restore 1998
    }
}
