using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAddFriend
    {
        [GameAction(GameActionType.AddFriend)]
        public static void Handle(ClientMessage message, Session session)
        {
            var friendName = message.Payload.ReadString16L().Trim();

            session.Player.HandleActionAddFriend(friendName);
        }
    }
}
