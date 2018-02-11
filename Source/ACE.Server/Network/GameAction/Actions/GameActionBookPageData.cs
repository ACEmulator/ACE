using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionBookPageData
    {
        [GameAction(GameActionType.BookPageData)]
        public static void Handle(ClientMessage message, Session session)
        {
            var objectId = message.Payload.ReadUInt32();
            var pageNum = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(objectId);

            session.Player.ReadBookPage(guid, pageNum);
        }
    }
}
