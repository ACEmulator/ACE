using System;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionBookData
    {
        [GameAction(GameActionType.BookData)]
        public static void Handle(ClientMessage message, Session session)
        {
            var bookGuid = message.Payload.ReadUInt32();

            //Console.WriteLine($"0xAA - BookData({bookGuid:X8}) - unused?");

            session.Player.ReadBook(bookGuid);
        }
    }
}
