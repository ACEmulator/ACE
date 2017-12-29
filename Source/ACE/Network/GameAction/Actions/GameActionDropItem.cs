using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionDropItem
    {
        [GameAction(GameActionType.DropItem)]

        public static async Task Handle(ClientMessage message, Session session)
        {
            var objectGuid = new ObjectGuid(message.Payload.ReadUInt32());
            await session.Player.HandleActionDropItem(objectGuid);
        }
    }
}
