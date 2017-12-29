using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionUseItem
    {
        [GameAction(GameActionType.Use)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint fullId = message.Payload.ReadUInt32();
            await session.Player.ActionUse(new ObjectGuid(fullId));
        }
    }
}
