using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using System;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionOpenTradeNegotiations
    {
        [GameAction(GameActionType.OpenTradeNegotiations)]
        public static void Handle(ClientMessage message, Session session)
        {
            ObjectGuid tradePartner = new ObjectGuid(message.Payload.ReadUInt32());

            Console.WriteLine("GameAction Open Trade Negotiations Called. Partner GUID " + tradePartner);

            var targetsession = WorldManager.Find(tradePartner);

            if (targetsession != null)
            {
                session.Player.HandleActionOpenTradeNegotiations(session, tradePartner);

                //Open the trade window for the trade partner
                targetsession.Player.HandleActionOpenTradeNegotiations(targetsession, session.Player.Guid);
            }
        }
    }
}
