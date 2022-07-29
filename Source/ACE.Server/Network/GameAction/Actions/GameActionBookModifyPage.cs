using System;
using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionBookModifyPage
    {
        [GameAction(GameActionType.BookModifyPage)]
        public static void Handle(ClientMessage message, Session session)
        {
            var bookGuid = message.Payload.ReadUInt32();
            var page = message.Payload.ReadInt32();    // 0-based
            var text = message.Payload.ReadString16L();

            //Console.WriteLine($"0xAB - BookModifyPage({bookGuid:X8}, {page + 1}, {text})");

            session.Player.HandleActionBookModifyPage(bookGuid, page, text);
        }
    }
}
