using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRemoveFriend
    {
        [GameAction(GameActionType.RemoveFriend)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint id = message.Payload.ReadUInt32(); // & 0xFFFFFF;
            var friendId = new ObjectGuid(id);
            await session.Player.RemoveFriend(friendId);
        }
    }
}
