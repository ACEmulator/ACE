using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Boots everyone from your house, /house boot -all
    /// </summary>
    public static class GameActionHouseBootEveryone
    {
        [GameAction(GameActionType.BootEveryone)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x25F - House - BootEveryone");

            session.Player.HandleActionBootAll();
        }
    }
}
