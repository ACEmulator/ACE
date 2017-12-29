using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionPutItemInContainer
    {
        [GameAction(GameActionType.PutItemInContainer)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            var itemGuid = new ObjectGuid(message.Payload.ReadUInt32());
            var containerGuid = new ObjectGuid(message.Payload.ReadUInt32());
            var placement = message.Payload.ReadInt32();
            await session.Player.PutItemInContainer(itemGuid, containerGuid, placement);
        }
    }
}
