using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

using ACE.Database;
using ACE.Database.Models.Shard;
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

        public bool HandleActionOpenTradeNegotiations(Session session, Player tradePartner, bool initiator = false)
        {
            session.Player.TradePartner = tradePartner.Guid;

            //Check to see if partner is not allowing trades
            if ((initiator) && (tradePartner.GetCharacterOption(CharacterOption.IgnoreAllTradeRequests)))
            {
                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeIgnoringRequests));
                return false;
            }

            //Check to see if either party is already part of an in process trade session
            if ((session.Player.IsTrading) || (tradePartner.IsTrading))
            {
                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeAlreadyTrading));
                return false;
            }

            //Check to see if either party is in combat mode
            if ((session.Player.CombatMode != CombatMode.NonCombat) || (tradePartner.CombatMode != CombatMode.NonCombat))
            {
                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeNonCombatMode));
                return false;
            }

            //Check to see if trade partner is in range, if so, rotate and move to
            var valid = false;
            bool ret = CurrentLandblock != null ? !CurrentLandblock.WithinUseRadius(session.Player, tradePartner.Guid, out valid) : false;

            if (valid)
            {
                session.Player.ItemsInTradeWindow.Clear();

                session.Player.Rotate(tradePartner);
                session.Player.MoveTo(tradePartner);

                session.Network.EnqueueSend(new GameEventRegisterTrade(session, session.Player.Guid, tradePartner.Guid));

                if (!initiator)
                {
                    session.Player.IsTrading = true;
                    tradePartner.IsTrading = true;
                }
                
                return true;
            }

            session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeMaxDistanceExceeded));
            return false;
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

        public void HandleActionAddToTrade(Session session, uint itemGuid, uint tradeWindowSlotNumber)
        {
            var target = PlayerManager.GetOnlinePlayer(session.Player.TradePartner);

            session.Player.TradeAccepted = false;

            if (itemGuid != 0 && target != null)
            {
                WorldObject wo = GetInventoryItem(itemGuid);

                if (wo != null)
                {
                    if ((wo.Attuned ?? 0) >= 1)
                    {
                        session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, "You cannot trade that!"));
                        session.Network.EnqueueSend(new GameEventTradeFailure(session, itemGuid, WeenieError.AttunedItem));
                    }
                    else
                    {
                        session.Player.ItemsInTradeWindow.Add(new ObjectGuid(itemGuid));

                        session.Network.EnqueueSend(new GameEventAddToTrade(session, itemGuid, TradeSide.Self));

                        target.TrackObject(wo);

                        target.Session.Network.EnqueueSend(new GameEventAddToTrade(target.Session, itemGuid, TradeSide.Partner));
                    }
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

            var target = PlayerManager.GetOnlinePlayer(session.Player.TradePartner);

            if (target != null)
            {
                target.Session.Network.EnqueueSend(new GameEventAcceptTrade(target.Session, whoAccepted));

                if (whoAccepted == session.Player.Guid)
                    target.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(target.Session, $"({session.Player.Name}) has accepted the offer"));

                if (session.Player.TradeAccepted && target.TradeAccepted)
                {
                    session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, "The items are being traded"));
                    target.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(target.Session, "The items are being traded"));

                    var tradedItems = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();

                    foreach (ObjectGuid itemGuid in session.Player.ItemsInTradeWindow)
                    {
                        if (session.Player.TryRemoveFromInventoryWithNetworking(itemGuid, out var wo, RemoveFromInventoryAction.TradeItem) || session.Player.TryDequipObjectWithNetworking(itemGuid, out wo, DequipObjectAction.TradeItem))
                        {
                            target.TryCreateInInventoryWithNetworking(wo);

                            tradedItems.Add((wo.Biota, wo.BiotaDatabaseLock));
                        }
                    }

                    foreach (ObjectGuid itemGuid in target.ItemsInTradeWindow)
                    {
                        if (target.TryRemoveFromInventoryWithNetworking(itemGuid, out var wo, RemoveFromInventoryAction.TradeItem) || target.TryDequipObjectWithNetworking(itemGuid, out wo, DequipObjectAction.TradeItem))
                        {
                            session.Player.TryCreateInInventoryWithNetworking(wo);

                            tradedItems.Add((wo.Biota, wo.BiotaDatabaseLock));
                        }
                    }

                    session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeComplete));
                    target.Session.Network.EnqueueSend(new GameEventWeenieError(target.Session, WeenieError.TradeComplete));

                    session.Player.HandleActionResetTrade(session, new ObjectGuid(0));
                    target.HandleActionResetTrade(target.Session, new ObjectGuid(0));

                    DatabaseManager.Shard.SaveBiotasInParallel(tradedItems, null);
                }
            }
        }

        public void HandleActionDeclineTrade(Session session)
        {
            session.Player.TradeAccepted = false;

            session.Network.EnqueueSend(new GameEventDeclineTrade(session,session.Player.Guid));
            session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, "Trade confirmation failed"));
            
            var target = PlayerManager.GetOnlinePlayer(session.Player.TradePartner);

            if (target != null)
            {
                target.Session.Network.EnqueueSend(new GameEventDeclineTrade(target.Session, session.Player.Guid));
                target.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(target.Session, "Trade confirmation failed"));
            }
        }

        public void HandleActionTradeSwitchToCombatMode(Session session)
        {
            if (session.Player.CombatMode != CombatMode.NonCombat && session.Player.IsTrading)
            {
                var target = PlayerManager.GetOnlinePlayer(session.Player.TradePartner);

                session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.TradeNonCombatMode));
                session.Player.HandleActionCloseTradeNegotiations(session, EndTradeReason.EnteredCombat);

                if (target !=null)
                {
                    target.Session.Network.EnqueueSend(new GameEventWeenieError(target.Session, WeenieError.TradeNonCombatMode));
                    target.HandleActionCloseTradeNegotiations(target.Session, EndTradeReason.EnteredCombat);
                }
            }
        }
    }
}
