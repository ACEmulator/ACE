using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionBookPageData
    {
        [GameAction(GameActionType.BookPageData)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            var objectId = message.Payload.ReadUInt32();
            var pageNum = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(objectId);

            await session.Player.ReadBookPage(guid, pageNum);
        }
    }
}
