using System;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionBookDeletePage
    {
        [GameAction(GameActionType.BookDeletePage)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint bookGuid = message.Payload.ReadUInt32();
            int page = message.Payload.ReadInt32();    // 0-based

            //Console.WriteLine($"0xAD - BookDeletePage({bookGuid:X8}, {page + 1})");

            session.Player.HandleActionBookDeletePage(bookGuid, page);
        }
    }
}
