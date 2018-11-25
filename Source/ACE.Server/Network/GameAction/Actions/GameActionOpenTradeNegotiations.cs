using ACE.Entity;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionOpenTradeNegotiations
    {
        [GameAction(GameActionType.OpenTradeNegotiations)]
        public static void Handle(ClientMessage message, Session session)
        {
            var tradePartner = new ObjectGuid(message.Payload.ReadUInt32());

            var target = PlayerManager.GetOnlinePlayer(tradePartner);

            if (target != null)
            {
                //Open the trade window for the trade partner
                if (session.Player.HandleActionOpenTradeNegotiations(session, tradePartner, true))
                    //Trade partner met all criteria to initiate trade, open their window
                    target.HandleActionOpenTradeNegotiations(target.Session, session.Player.Guid);
            }
        }
    }
}
