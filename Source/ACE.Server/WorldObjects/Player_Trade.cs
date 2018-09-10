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
        private bool TradeAccepted { get; set; } = false;
        private bool IsTrading = false;
        public ObjectGuid TradePartner;

        public bool HandleActionOpenTradeNegotiations(Session session, ObjectGuid tradePartner, bool initiator = false)
        {
            session.Player.TradePartner = tradePartner;

            var targetsession = WorldManager.Find(session.Player.TradePartner);
            var target = CurrentLandblock?.GetObject(tradePartner);

            //Check to see if partner is not allowing trades
            if ((initiator) && (targetsession.Player.GetCharacterOption(CharacterOption.IgnoreAllTradeRequests)))
            {
                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeIgnoringRequests));
                return false;
            }

            //Check to see if either party is already part of an in process trade session
            if ((session.Player.IsTrading) || (targetsession.Player.IsTrading))
            {
                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeAlreadyTrading));
                return false;
            }

            //Check to see if either party is in combat mode
            if ((session.Player.CombatMode != CombatMode.NonCombat) || (targetsession.Player.CombatMode != CombatMode.NonCombat))
            {
                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeNonCombatMode));
                return false;
            }

            //Check to see if trade partner is in range, if so, rotate and move to
            var valid = false;
            bool ret = CurrentLandblock != null ? !CurrentLandblock.WithinUseRadius(session.Player, tradePartner, out valid) : false;

            if (valid)
            {
                session.Player.ItemsInTradeWindow.Clear();

                session.Player.Rotate(target);
                session.Player.MoveTo(target);

                session.Network.EnqueueSend(new GameEventRegisterTrade(session, session.Player.Guid, tradePartner));

                if (!initiator)
                {
                    session.Player.IsTrading = true;
                    targetsession.Player.IsTrading = true;
                }
                
                return true;
            }
            else
            {
                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeMaxDistanceExceeded));
                return false;
            }
        }

        public void HandleActionCloseTradeNegotiations(Session session, EndTradeReason endTradeReason = EndTradeReason.Normal)
        {
            session.Player.IsTrading = false;
            session.Player.TradeAccepted = false;
            session.Player.ItemsInTradeWindow.Clear();
            session.Player.TradePartner = new ObjectGuid(0);

            session.Network.EnqueueSend(new GameEventCloseTrade(session, endTradeReason));
            session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeClosed));
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
                    WorldObject wo = GetInventoryItem(item);

                    if (wo != null)
                    {
                        session.Player.ItemsInTradeWindow.Add(item);

                        session.Network.EnqueueSend(new GameEventAddToTrade(session, item, TradeSide.Self));

                        targetsession.Player.TrackObject(wo);

                        targetsession.Network.EnqueueSend(new GameEventAddToTrade(targetsession, item, TradeSide.Partner));
                    }
                    
                }
                else
                {
                    session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, "You cannot trade that!"));
                    session.Network.EnqueueSend(new GameEventTradeFailure(session, item, WeenieError.AttunedItem));
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

            if (whoAccepted == session.Player.Guid)
                session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, "You have accepted the offer"));

            var targetsession = WorldManager.Find(session.Player.TradePartner);

            if (targetsession != null)
            {
                targetsession.Network.EnqueueSend(new GameEventAcceptTrade(targetsession, whoAccepted));

                if (whoAccepted == session.Player.Guid)
                    targetsession.Network.EnqueueSend(new GameEventCommunicationTransientString(targetsession, $"({session.Player.Name}) has accepted the offer"));

                if ((session.Player.TradeAccepted) && (targetsession.Player.TradeAccepted))
                {
                    session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, "The items are being traded"));
                    targetsession.Network.EnqueueSend(new GameEventCommunicationTransientString(targetsession, "The items are being traded"));

                    foreach (ObjectGuid itemGuid in session.Player.ItemsInTradeWindow)
                    {
                        WorldObject wo = GetInventoryItem(itemGuid);

                        if (wo != null)
                        {
                            session.Player.TryRemoveFromInventoryWithNetworking(wo);

                            targetsession.Player.TryCreateInInventoryWithNetworking(wo);

                        }
                    }

                    foreach (ObjectGuid itemGuid in targetsession.Player.ItemsInTradeWindow)
                    {
                        WorldObject wo = targetsession.Player.GetInventoryItem(itemGuid);

                        if (wo != null)
                        {
                            targetsession.Player.TryRemoveFromInventoryWithNetworking(wo);

                            session.Player.TryCreateInInventoryWithNetworking(wo);

                        }
                    }

                    session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeComplete));
                    targetsession.Network.EnqueueSend(new GameEventWeenieError(targetsession, WeenieError.TradeComplete));

                    session.Player.HandleActionResetTrade(session, new ObjectGuid(0));
                    targetsession.Player.HandleActionResetTrade(targetsession, new ObjectGuid(0));

                    session.Player.EnqueueSaveChain();
                    targetsession.Player.EnqueueSaveChain();
                }
            }
        }

        public void HandleActionDeclineTrade(Session session)
        {
            session.Player.TradeAccepted = false;

            session.Network.EnqueueSend(new GameEventDeclineTrade(session,session.Player.Guid));
            session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, "Trade confirmation failed"));
            
            var targetsession = WorldManager.Find(session.Player.TradePartner);

            if (targetsession != null)
            {
                targetsession.Network.EnqueueSend(new GameEventDeclineTrade(targetsession, session.Player.Guid));
                targetsession.Network.EnqueueSend(new GameEventCommunicationTransientString(targetsession, "Trade confirmation failed"));
            }
        }

        public void HandleActionTradeSwitchToCombatMode(Session session)
        {
            if (session.Player.CombatMode != CombatMode.NonCombat)
            {
                var targetsession = WorldManager.Find(session.Player.TradePartner);

                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeNonCombatMode));
                session.Player.HandleActionCloseTradeNegotiations(session, EndTradeReason.EnteredCombat);

                if (targetsession !=null)
                {
                    targetsession.Network.EnqueueSend(new GameEventWeenieError(targetsession, WeenieError.TradeNonCombatMode));
                    targetsession.Player.HandleActionCloseTradeNegotiations(targetsession, EndTradeReason.EnteredCombat);
                }
            }
        }
    }
}
