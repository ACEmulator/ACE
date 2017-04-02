
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
            var result = await session.Player.AddFriend(friendName);

            switch (result)
            {
                case Enum.AddFriendResult.AlreadyInList:
                    ChatPacket.SendServerMessage(session, "That character is already in your friends list", ChatMessageType.Broadcast);
                    break;

                case Enum.AddFriendResult.FriendWithSelf:
                    ChatPacket.SendServerMessage(session, "Sorry, but you can't be friends with yourself.", ChatMessageType.Broadcast);
                    break;

                case Enum.AddFriendResult.CharacterDoesNotExist:
                    ChatPacket.SendServerMessage(session, "That character does not exist", ChatMessageType.Broadcast);
                    break;
            }
        }
    }
}
