using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Requests your full guest list, /house guest list
    /// </summary>
    public static class GameActionHouseRequestFullGuestList
    {
        [GameAction(GameActionType.RequestFullGuestList)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x24D - House - RequestFullGuestList");

            session.Player.HandleActionGuestList();
        }
    }
}
