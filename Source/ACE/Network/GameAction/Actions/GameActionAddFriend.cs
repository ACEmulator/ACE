using ACE.Common.Extensions;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionAddFriend
    {
        [GameAction(GameActionType.AddFriend)]
        public static async void Handle(ClientMessage message, Session session)
        {
            var friendName = message.Payload.ReadString16L().Trim();
            session.Player.AddFriend(friendName);
        }
    }
}