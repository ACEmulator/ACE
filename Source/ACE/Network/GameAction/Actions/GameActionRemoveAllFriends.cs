using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRemoveAllFriends
    {
        [GameAction(GameActionType.RemoveAllFriends)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            await session.Player.RemoveAllFriends();
        }
    }
}
