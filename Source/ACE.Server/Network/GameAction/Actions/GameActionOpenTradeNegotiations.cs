using ACE.Entity;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionOpenTradeNegotiations
    {
        [GameAction(GameActionType.OpenTradeNegotiations)]
        public static void Handle(ClientMessage message, Session session)
        {
            ObjectGuid tradePartner = new ObjectGuid(message.Payload.ReadUInt32());

            var targetsession = WorldManager.Find(tradePartner);

            if (targetsession != null)
            {
                //Open the trade window for the trade partner
                if (session.Player.HandleActionOpenTradeNegotiations(session, tradePartner, true))
                    //Trade partner met all criteria to initiate trade, open their window
                    targetsession.Player.HandleActionOpenTradeNegotiations(targetsession, session.Player.Guid);
            }
        }
    }
}
