using System;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Query your house info
    /// </summary>
    public static class GameActionHouseQuery
    {
        [GameAction(GameActionType.HouseQuery)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x21E - GameActionHouseQuery");

            session.Network.EnqueueSend(new GameEventHouseStatus(session));
        }
    }
}
