
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionBookPageData
    {
        [GameAction(GameActionType.BookPageData)]
        public static void Handle(ClientMessage message, Session session)
        {
            var bookGuid = message.Payload.ReadUInt32();
            var pageNum = message.Payload.ReadUInt32();

            session.Player.ReadBookPage(bookGuid, pageNum);
        }
    }
}
