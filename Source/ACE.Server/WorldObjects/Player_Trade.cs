using System;
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
            session.Player.TradeAccepted = false;
            session.Player.ItemsInTradeWindow.Clear();
            session.Player.TradePartner = new ObjectGuid(0);

            session.Network.EnqueueSend(new GameEventCloseTrade(session, EndTradeReason.Normal));
        }

        public void HandleActionAddToTrade(Session session, ObjectGuid item, uint tradeWindowSlotNumber)
        {
            var targetsession = WorldManager.Find(session.Player.TradePartner);

            session.Player.TradeAccepted = false;

            if ((item != null) && (targetsession != null))
            {
                IsAttuned(item, out bool isAttuned);

                if (!isAttuned)
                {
                    session.Player.ItemsInTradeWindow.Add(item);

                    session.Network.EnqueueSend(new GameEventAddToTrade(session, item, TradeSide.Self));

                    WorldObject wo = GetInventoryItem(item);

                    targetsession.Player.TrackObject(wo);

                    targetsession.Network.EnqueueSend(new GameEventAddToTrade(targetsession, item, TradeSide.Partner));
                }
                else
                {
                    session.Network.EnqueueSend(new GameEventTradeFailure(session, item));
                }
            }
        }

        public void HandleActionResetTrade(Session session, ObjectGuid whoReset)
        {
            session.Player.ItemsInTradeWindow.Clear();
            session.Player.TradeAccepted = false;

            session.Network.EnqueueSend(new GameEventResetTrade(session, whoReset));
        }

        public void HandleActionAcceptTrade(Session session, ObjectGuid whoAccepted)
        {
            session.Player.TradeAccepted = true;

            session.Network.EnqueueSend(new GameEventAcceptTrade(session, whoAccepted));

            var targetsession = WorldManager.Find(session.Player.TradePartner);

            if (targetsession != null)
            {
                targetsession.Network.EnqueueSend(new GameEventAcceptTrade(targetsession, whoAccepted));

                if ((session.Player.TradeAccepted) && (targetsession.Player.TradeAccepted))
                {
                    foreach (ObjectGuid itemGuid in session.Player.ItemsInTradeWindow)
                    {
                        WorldObject wo = GetInventoryItem(itemGuid);

                        if (wo != null)
                        {
                            Console.WriteLine(wo.Name);
                            session.Player.TryRemoveFromInventoryWithNetworking(wo);

                            targetsession.Player.TryCreateInInventoryWithNetworking(wo);
                        }
                    }

                    foreach (ObjectGuid itemGuid in targetsession.Player.ItemsInTradeWindow)
                    {
                        WorldObject wo = GetInventoryItem(itemGuid);

                        if (wo != null)
                        {
                            Console.WriteLine(wo.Name);
                            targetsession.Player.TryRemoveFromInventoryWithNetworking(wo);

                            session.Player.TryCreateInInventoryWithNetworking(wo);
                        }
                    }

                    session.Player.HandleActionCloseTradeNegotiations(session);
                    targetsession.Player.HandleActionCloseTradeNegotiations(targetsession);
                }


            }
        }

        public void HandleActionDeclineTrade(Session session)
        {
            session.Player.TradeAccepted = false;

            session.Network.EnqueueSend(new GameEventClearTradeAcceptance(session));

            var targetsession = WorldManager.Find(session.Player.TradePartner);

            if (targetsession != null)
            {
                targetsession.Network.EnqueueSend(new GameEventClearTradeAcceptance(targetsession));
            }
        }
    }
}
