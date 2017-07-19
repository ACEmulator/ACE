using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRemoveFriend
    {
        [GameAction(GameActionType.RemoveFriend)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint id = message.Payload.ReadUInt32(); // & 0xFFFFFF;
            var friendId = new ObjectGuid(id);
            session.Player.RemoveFriend(friendId);
        }
    }
}