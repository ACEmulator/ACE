using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTeleToLifestone
    {
        [GameAction(GameActionType.TeleToLifestone)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            await session.Player.TeleToLifestone();
        }
    }
}
