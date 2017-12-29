using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionQueryHealth
    {
        [GameAction(GameActionType.QueryHealth)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint fullId = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(fullId);

            await session.Player.QueryHealth(guid);
        }
    }
}
