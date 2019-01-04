using System;

using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionHouseAbandon
    {
        [GameAction(GameActionType.AbandonHouse)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x21F - AbandonHouse");

            session.Player.HandleActionAbandonHouse();
        }
    }
}
