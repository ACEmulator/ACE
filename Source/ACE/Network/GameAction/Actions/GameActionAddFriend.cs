using System.Threading.Tasks;

using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionAddFriend
    {
        [GameAction(GameActionType.AddFriend)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            var friendName = message.Payload.ReadString16L().Trim();
            await session.Player.AddFriend(friendName);
        }
    }
}
