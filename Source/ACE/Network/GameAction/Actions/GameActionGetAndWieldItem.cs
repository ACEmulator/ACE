using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionGetAndWieldItem
    {
        [GameAction(GameActionType.GetAndWieldItem)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint itemGuid = message.Payload.ReadUInt32();
            int location = message.Payload.ReadInt32();
            await session.Player.WeildItem(session.Player, itemGuid, location);
        }
    }
}
