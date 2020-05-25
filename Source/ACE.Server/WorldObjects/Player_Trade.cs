using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Managers;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public HashSet<ObjectGuid> ItemsInTradeWindow = new HashSet<ObjectGuid>();

        public ObjectGuid TradePartner;

        public bool IsTrading { get; private set; }

        private bool TradeAccepted;

        public bool TradeTransferInProgress;

        public void HandleActionOpenTradeNegotiations(uint tradePartnerGuid, bool initiator = false)
        {
            var tradePartner = PlayerManager.GetOnlinePlayer(tradePartnerGuid);
            if (tradePartner == null) return;

            TradePartner = tradePartner.Guid;

            //Check to see if partner is not allowing trades
            if (initiator && tradePartner.GetCharacterOption(CharacterOption.IgnoreAllTradeRequests))
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.TradeIgnoringRequests));
                return;
            }

            //Check to see if either party is already part of an in process trade session
            if (IsTrading || tradePartner.IsTrading)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.TradeAlreadyTrading));
                return;
            }

            //Check to see if either party is in combat mode
            if (CombatMode != CombatMode.NonCombat || tradePartner.CombatMode != CombatMode.NonCombat)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.TradeNonCombatMode));
                return;
            }

            //Check to see if trade partner is in range, if so, rotate and move to
            if (initiator)
            {
                CreateMoveToChain(tradePartner, (success) =>
                {
                    if (!success)
                    {
                        Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.TradeMaxDistanceExceeded));
                        return;
                    }

                    ItemsInTradeWindow.Clear();

                    Session.Network.EnqueueSend(new GameEventRegisterTrade(Session, Guid, tradePartner.Guid));

                    // this fixes current version of DoThingsBot
                    // ideally future version of DTB should be updated to be based on RegisterTrade event, instead of ResetTrade
                    Session.Network.EnqueueSend(new GameEventResetTrade(Session, Guid));

                    tradePartner.HandleActionOpenTradeNegotiations(Guid.Full, false);
                });
            }
            else
            {
                IsTrading = true;
                tradePartner.IsTrading = true;
                TradeTransferInProgress = false;
                tradePartner.TradeTransferInProgress = false;

                ItemsInTradeWindow.Clear();

                Session.Network.EnqueueSend(new GameEventRegisterTrade(Session, tradePartner.Guid, tradePartner.Guid));

                // this fixes current version of DoThingsBot
                // ideally future version of DTB should be updated to be based on RegisterTrade event, instead of ResetTrade
                Session.Network.EnqueueSend(new GameEventResetTrade(Session, tradePartner.Guid));
            }
        }

        public void HandleActionCloseTradeNegotiations(EndTradeReason endTradeReason = EndTradeReason.Normal)
        {
            if (TradeTransferInProgress) return;

            IsTrading = false;
            TradeAccepted = false;
            TradeTransferInProgress = false;
            ItemsInTradeWindow.Clear();
            TradePartner = ObjectGuid.Invalid;

            Session.Network.EnqueueSend(new GameEventCloseTrade(Session, endTradeReason));
            Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.TradeClosed));
        }

        public void HandleActionAddToTrade(uint itemGuid, uint tradeWindowSlotNumber)
        {
            if (TradeTransferInProgress)
                return;

            TradeAccepted = false;

            var target = PlayerManager.GetOnlinePlayer(TradePartner);

            if (target == null || itemGuid == 0)
                return;

            target.TradeAccepted = false;

            WorldObject wo = GetInventoryItem(itemGuid);

            if (wo == null)
            {
                wo = GetEquippedItem(itemGuid);

                if (wo == null)
                    return;
            }

            if (wo.IsAttunedOrContainsAttuned)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You cannot trade that!"));
                Session.Network.EnqueueSend(new GameEventTradeFailure(Session, itemGuid, WeenieError.AttunedItem));
                return;
            }

            if (wo.IsUniqueOrContainsUnique && !target.CheckUniques(wo, this))
            {
                // WeenieError.TooManyUniqueItems / WeenieErrorWithString._CannotCarryAnymore?
                Session.Network.EnqueueSend(new GameEventTradeFailure(Session, itemGuid, WeenieError.None));
                return;
            }

            ItemsInTradeWindow.Add(new ObjectGuid(itemGuid));

            Session.Network.EnqueueSend(new GameEventAddToTrade(Session, itemGuid, TradeSide.Self));

            target.AddKnownTradeObj(Guid, wo.Guid);
            target.TrackObject(wo);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(0.001f);
            actionChain.AddAction(target, () =>
            {
                target.Session.Network.EnqueueSend(new GameEventAddToTrade(target.Session, itemGuid, TradeSide.Partner));
            });
            actionChain.EnqueueChain();
        }

        public void HandleActionResetTrade(ObjectGuid whoReset)
        {
            if (TradeTransferInProgress)
                return;

            ItemsInTradeWindow.Clear();
            TradeAccepted = false;

            Session.Network.EnqueueSend(new GameEventResetTrade(Session, whoReset));
        }

        public void ClearTradeAcceptance()
        {
            ItemsInTradeWindow.Clear();
            TradeAccepted = false;

            Session.Network.EnqueueSend(new GameEventClearTradeAcceptance(Session));
        }

        public void HandleActionAcceptTrade()
        {
            if (TradeTransferInProgress)
                return;

            TradeAccepted = true;

            Session.Network.EnqueueSend(new GameEventAcceptTrade(Session, Guid));
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You have accepted the offer"));

            var target = PlayerManager.GetOnlinePlayer(TradePartner);

            if (target == null)
                return;

            target.Session.Network.EnqueueSend(new GameEventAcceptTrade(target.Session, Guid));
            target.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(target.Session, $"{Name} has accepted the offer"));

            if (target.TradeAccepted)
                FinalizeTrade(target);
        }

        private void FinalizeTrade(Player target)
        {
            if (!VerifyTrade_BusyState(target) || !VerifyTrade_Inventory(target))
                return;

            IsBusy = true;
            target.IsBusy = true;

            TradeTransferInProgress = true;
            target.TradeTransferInProgress = true;

            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "The items are being traded"));
            target.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(target.Session, "The items are being traded"));

            var tradedItems = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();

            var myEscrow = new List<WorldObject>();
            var targetEscrow = new List<WorldObject>();

            foreach (ObjectGuid itemGuid in ItemsInTradeWindow)
            {
                if (TryRemoveFromInventoryWithNetworking(itemGuid, out var wo, RemoveFromInventoryAction.TradeItem) || TryDequipObjectWithNetworking(itemGuid, out wo, DequipObjectAction.TradeItem))
                {
                    targetEscrow.Add(wo);

                    tradedItems.Add((wo.Biota, wo.BiotaDatabaseLock));
                }
            }

            foreach (ObjectGuid itemGuid in target.ItemsInTradeWindow)
            {
                if (target.TryRemoveFromInventoryWithNetworking(itemGuid, out var wo, RemoveFromInventoryAction.TradeItem) || target.TryDequipObjectWithNetworking(itemGuid, out wo, DequipObjectAction.TradeItem))
                {
                    myEscrow.Add(wo);

                    tradedItems.Add((wo.Biota, wo.BiotaDatabaseLock));
                }
            }

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(0.5f);
            actionChain.AddAction(CurrentLandblock, () =>
            {
                foreach (var wo in myEscrow)
                    TryCreateInInventoryWithNetworking(wo);

                foreach (var wo in targetEscrow)
                    target.TryCreateInInventoryWithNetworking(wo);

                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.TradeComplete));
                target.Session.Network.EnqueueSend(new GameEventWeenieError(target.Session, WeenieError.TradeComplete));

                TradeTransferInProgress = false;
                target.TradeTransferInProgress = false;

                IsBusy = false;
                target.IsBusy = false;

                DatabaseManager.Shard.SaveBiotasInParallel(tradedItems, null);

                HandleActionResetTrade(Guid);
                target.HandleActionResetTrade(target.Guid);
            });

            actionChain.EnqueueChain();
        }

        private List<WorldObject> GetItemsInTradeWindow(Player player)
        {
            var results = new List<WorldObject>();

            foreach (ObjectGuid itemGuid in player.ItemsInTradeWindow)
            {
                var wo = player.GetInventoryItem(itemGuid);

                if (wo == null)
                    wo = player.GetEquippedItem(itemGuid);

                if (wo != null)
                    results.Add(wo);
            }

            return results;
        }

        public void HandleActionDeclineTrade(Session session)
        {
            if (session.Player.TradeTransferInProgress) return;

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
                session.Player.HandleActionCloseTradeNegotiations(EndTradeReason.EnteredCombat);

                if (target != null)
                {
                    target.Session.Network.EnqueueSend(new GameEventWeenieError(target.Session, WeenieError.TradeNonCombatMode));
                    target.HandleActionCloseTradeNegotiations(EndTradeReason.EnteredCombat);
                }
            }
        }

        private bool VerifyTrade_BusyState(Player partner)
        {
            if (!IsBusy && !partner.IsBusy)
                return true;

            var selfBusy = "You are too busy to complete the trade!";
            var otherBusy = "Your trading partner is too busy to complete the trade!";

            var selfMsg = IsBusy ? selfBusy : otherBusy;
            var partnerMsg = IsBusy ? otherBusy : selfBusy;

            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, selfMsg));
            partner.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(partner.Session, partnerMsg));

            ClearTradeAcceptance();
            partner.ClearTradeAcceptance();

            return false;
        }

        private bool VerifyTrade_Inventory(Player partner)
        {
            var self_items = GetItemsInTradeWindow(this);
            var partner_items = GetItemsInTradeWindow(partner);

            var playerACanAddToInventory = CanAddToInventory(partner_items, out var selfEncumbered, out var selfPackSpace);
            var playerBCanAddToInventory = partner.CanAddToInventory(self_items, out var partnerEncumbered, out var partnerPackSpace);

            if (playerACanAddToInventory && playerBCanAddToInventory)
                return true;

            var selfReason = "";
            var partnerReason = "";

            if (!playerACanAddToInventory)
            {
                selfReason = "You ";
                partnerReason = "Your trading partner ";

                if (selfEncumbered)
                {
                    selfReason += "are too encumbered to complete the trade!";
                    partnerReason += "is too encumbered to complete the trade!";
                }
                else if (selfPackSpace)
                {
                    selfReason += "do not have enough free slots to complete the trade!";
                    partnerReason += "does not have enough free slots to complete the trade!";
                }
            }
            else if (!playerBCanAddToInventory)
            {
                selfReason = "Your trading partner ";
                partnerReason = "You ";

                if (partnerEncumbered)
                {
                    selfReason += "is too encumbered to complete the trade!";
                    partnerReason += "are too encumbered to complete the trade!";
                }
                else if (partnerPackSpace)
                {
                    selfReason += "does not have enough free slots to complete the trade!";
                    partnerReason += "do not have enough free slots to complete the trade!";
                }
            }

            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, selfReason));
            partner.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(partner.Session, partnerReason));

            ClearTradeAcceptance();
            partner.ClearTradeAcceptance();

            return false;
        }

        public Dictionary<ObjectGuid, HashSet<ObjectGuid>> KnownTradeObjs = new Dictionary<ObjectGuid, HashSet<ObjectGuid>>();

        public void AddKnownTradeObj(ObjectGuid playerGuid, ObjectGuid itemGuid)
        {
            if (!KnownTradeObjs.TryGetValue(playerGuid, out var knownTradeItems))
            {
                knownTradeItems = new HashSet<ObjectGuid>();
                KnownTradeObjs.Add(playerGuid, knownTradeItems);
            }
            knownTradeItems.Add(itemGuid);
        }

        public Player GetKnownTradeObj(ObjectGuid itemGuid)
        {
            if (KnownTradeObjs.Count() == 0)
                return null;

            PruneKnownTradeObjs();

            foreach (var knownTradeObj in KnownTradeObjs)
            {
                if (knownTradeObj.Value.Contains(itemGuid))
                {
                    var playerGuid = knownTradeObj.Key;
                    var player = ObjMaint.GetKnownObject(playerGuid.Full)?.WeenieObj?.WorldObject as Player;
                    if (player != null && player.Location != null && Location.DistanceTo(player.Location) <= LocalBroadcastRange)
                        return player;
                    else
                        return null;
                }
            }
            return null;
        }

        public void PruneKnownTradeObjs()
        {
            foreach (var playerGuid in KnownTradeObjs.Keys.ToList())
            {
                if (ObjMaint.GetKnownObject(playerGuid.Full) == null)
                    KnownTradeObjs.Remove(playerGuid);
            }
        }
    }
}
