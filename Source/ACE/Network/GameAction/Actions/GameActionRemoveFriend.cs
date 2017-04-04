using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRemoveFriend
    {
        [GameAction(GameActionType.RemoveFriend)]
        public async static void Handle(ClientMessage message, Session session)
        {
            uint lowId = message.Payload.ReadUInt32() & 0xFFFFFF;
            var friendId = new ObjectGuid(lowId, GuidType.Player);
            var result = await session.Player.RemoveFriend(friendId);

            if (result == Enum.RemoveFriendResult.NotInFriendsList)
                ChatPacket.SendServerMessage(session, "That chracter is not in your friends list!", ChatMessageType.Broadcast);
        }
    }
}