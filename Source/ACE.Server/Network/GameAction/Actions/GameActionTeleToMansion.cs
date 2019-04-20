using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Teleports player to their allegiance housing, /house mansion_recall
    /// </summary>
    public static class GameActionTeleToMansion
    {
        [GameAction(GameActionType.TeleToMansion)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x278 - TeleToMansion");
            session.Player.HandleActionTeleToMansion();
        }
    }
}
