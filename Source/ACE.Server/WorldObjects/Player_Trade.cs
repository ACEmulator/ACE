using System.Collections.Generic;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public List<ObjectGuid> ItemsInTradeWindow = new List<ObjectGuid>();
        private bool TradeAccepted = false;
        public ObjectGuid TradePartner;

        public void HandleActionOpenTradeNegotiations(Session session, ObjectGuid tradePartner)
        {
            session.Player.ItemsInTradeWindow.Clear();

            session.Player.TradePartner = tradePartner;

            var targetsession = WorldManager.Find(session.Player.TradePartner);

            session.Network.EnqueueSend(new GameEventRegisterTrade(session, session.Player.Guid, tradePartner));
        }

        public void HandleActionCloseTradeNegotiations(Session session)
        {
            session.Player.ItemsInTradeWindow.Clear();

            session.Network.EnqueueSend(new GameEventCloseTrade(session, EndTradeReason.Normal));
        }

        public void HandleActionAddToTrade(Session session, ObjectGuid item, uint tradeWindowSlotNumber)
        {
            var targetsession = WorldManager.Find(session.Player.TradePartner);

            if (session.Player.TradeAccepted)
            {
                //If trade was previously accepted, cancel accept
                //TODO GameEventAction to notify clients of removal of acceptance
                session.Player.TradeAccepted = false;
            }
                                                
            if ((item != null) && (targetsession != null))
            {
                session.Player.ItemsInTradeWindow.Add(item);

                session.Network.EnqueueSend(new GameEventAddToTrade(session, item, TradeSide.Self));
                targetsession.Network.EnqueueSend(new GameEventAddToTrade(targetsession, item, TradeSide.Partner));
            }
        }
    }
}
