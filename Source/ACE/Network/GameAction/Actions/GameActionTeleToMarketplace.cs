using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTeleToMarketPlace
    {
        [GameAction(GameActionType.TeleToMarketPlace)]
        public static async Task Handle(ClientMessage clientMessage, Session session)
        {
            await session.Player.TeleToMarketplace();
        }
    }
}
