using System;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionBookPageData
    {
        [GameAction(GameActionType.BookPageData)]
        public static void Handle(ClientMessage message, Session session)
        {
            var bookGuid = message.Payload.ReadUInt32();
            var pageNum = message.Payload.ReadInt32();     // 0-based

            Console.WriteLine($"0xAE - BookPageData({bookGuid:X8}, {pageNum}) - unused?");

            session.Player.ReadBookPage(bookGuid, pageNum);
        }
    }
}
