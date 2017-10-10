using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using ACE.Entity.Enum.Properties;
using System;
using ACE.DatLoader.FileTypes;
using ACE.Network.GameMessages.Messages;
using ACE.Common;
using System.Collections.Generic;
using ACE.Database;
using ACE.Factories;

namespace ACE.Entity
{
    /// <summary>
    /// ** Usage Data Flow **
    ///     HandleActionApproachVendor
    /// ** Buy Data Flow **
    /// Player.HandleActionBuy->Vendor.BuyItemsValidateTransaction->Player.HandleActionBuyFinalTransaction->Vendor.BuyItemsFinalTransaction
    /// ** Sell Data Flow **
    /// Player.HandleActionSell->Vendor.SellItemsValidateTransaction->Player.HandleActionSellFinalTransaction->Vendor.SellItemsFinalTransaction
    /// </summary>
    public class Vendor : WorldObject
    {
        private Dictionary<ObjectGuid, WorldObject> defaultItemsForSale = new Dictionary<ObjectGuid, WorldObject>();
        private bool inventoryloaded = false;

        // todo : SO : Turning to player movement states  - looks at @og
        public Vendor(AceObject aceO)
            : base(aceO)
        {
        }

        public double BuyRate
        {
            get { return AceObject.BuyRate ?? 0.8; }
            set { AceObject.BuyRate = value; }
        }

        public double SellRate
        {
            get { return AceObject.SellRate ?? 1.0; }
            set { AceObject.SellRate = value; }
        }

        #region General Vendor functions

        /// <summary>
        /// Fired when Client clicks on the Vendor World Object
        /// </summary>
        /// <param name="playerId"></param>
        public override void HandleActionOnUse(ObjectGuid playerId)
        {
            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
            {
                Player player = wo as Player;
                if (player == null)
                {
                    return;
                }

                if (!player.IsWithinUseRadiusOf(this))
                    player.DoMoveTo(this);
                else
                {
                    chain.AddAction(this, () =>
                    {
                        LoadInventory();
                        ApproachVendor(player);
                    });
                }
            });

            chain.EnqueueChain();
        }

        /// <summary>
        /// Load Inventory for default items from database table / assignes default objects.
        /// </summary>
        private void LoadInventory()
        {
            // Load Vendor Inventory from database.
            if (!inventoryloaded)
            {
                List<VendorItems> items = new List<VendorItems>();
                items = DatabaseManager.World.GetVendorWeenieInventoryById(AceObject.WeenieClassId, DestinationType.Shop);
                foreach (VendorItems item in items)
                {
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);
                    if (wo != null)
                    {
                        wo.ContainerId = Guid.Full;
                        defaultItemsForSale.Add(wo.Guid, wo);
                    }
                }
                inventoryloaded = true;
            }
        }

        /// <summary>
        /// Sends Vendor Inventory to player
        /// </summary>
        /// <param name="player"></param>
        private void ApproachVendor(Player player)
        {
            List<WorldObject> vendorlist = new List<WorldObject>();
            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
            {
                vendorlist.Add(wo.Value);
            }

            // todo: send more then default items.
            player.TrackInteractiveObjects(vendorlist);
            player.HandleActionApproachVendor(this, vendorlist);
        }
        #endregion

        #region BuyTransaction

        /// <summary>
        /// Player has started a buy transaction
        /// Create objects, send object to player for final validation.
        /// </summary>
        /// <param name="vendorid">GUID of Vendor</param>
        /// <param name="items">Item Profile, Ammount and ID</param>
        /// <param name="player"></param>
        public void BuyValidateTransaction(ObjectGuid vendorid, List<ItemProfile> items, Player player)
        {
            // que transactions.
            List<ItemProfile> filteredlist = new List<ItemProfile>();
            List<WorldObject> purchaselist = new List<WorldObject>();
            uint goldcost = 0;

            // filter items out vendor no longer has in stock or never had in stock
            foreach (ItemProfile item in items)
            {
                // check default items for id
                if (defaultItemsForSale.ContainsKey(item.Guid))
                {
                    item.WeenieClassId = defaultItemsForSale[item.Guid].WeenieClassId;
                    filteredlist.Add(item);
                }
                // todo: check stock
            }

            // convert profile to wold objects / stack logic
            foreach (ItemProfile fitem in filteredlist)
            {
                // todo: refactor this loop and just create enough of the items in a tighter loop, then do
                // whatever calculations are necessary outside it
                while (fitem.Amount > 0)
                {
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject(fitem.WeenieClassId);

                    // todo: alternate path for purchasing actual items, not just weenie-based items (like spell comps)

                    // can we stack this ?
                    if (wo.MaxStackSize.HasValue)
                    {
                        if ((wo.MaxStackSize.Value != 0) & (wo.MaxStackSize.Value <= fitem.Amount))
                        {
                            wo.StackSize = wo.MaxStackSize.Value;
                            purchaselist.Add(wo);
                            fitem.Amount = fitem.Amount - wo.MaxStackSize.Value;
                        }
                        // else we cant stack but its not a single item.
                        else
                        {
                            // client automatically scales the price on it's end for the cost, but we need
                            // to scale it here to make sure the player has the right amount of pyreals

                            if (fitem.Amount > 0)
                            {
                                // pile on the stacks before rounding
                                goldcost = goldcost + (uint)Math.Floor(SellRate * (wo.Value ?? 0) * fitem.Amount + 0.1);
                            }
                            else
                            {
                                fitem.Amount = 0;
                                goldcost = goldcost + (uint)Math.Floor(SellRate * (wo.Value ?? 0) + 0.1);
                            }
                            wo.StackSize = (ushort)fitem.Amount;
                            purchaselist.Add(wo);
                            fitem.Amount = fitem.Amount - fitem.Amount;
                        }
                    }
                    else
                    {
                        // if there multiple items of the same  type.. 
                        if (fitem.Amount > 0)
                        {
                            // single item with no stack options. 
                            fitem.Amount = fitem.Amount - 1;
                            wo.StackSize = null;
                            goldcost = goldcost + (uint)Math.Floor(SellRate * (wo.Value ?? 0) + 0.1);
                            purchaselist.Add(wo);
                        }
                    }
                }
            }
            
            // send transaction to player for further processing and.
            player.HandleActionBuyFinalTransaction(this, purchaselist, true, goldcost);
        }

        /// <summary>
        /// Handles the final phase of the transaction.. removing unique items and updating players local
        /// from vendors items list.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="items"></param>
        public void BuyItemsFinalTransaction(Player player, List<WorldObject> items, bool valid)
        {
            List<WorldObject> vendorlist = new List<WorldObject>();

            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
            {
                vendorlist.Add(wo.Value);
            }

            if (valid)
            {
                // todo: remove unique temp stock items.
            }
            else
            {
                // todo: re-add unique temp stock items.
            }
            player.HandleActionApproachVendor(this, vendorlist);
        }
        #endregion

        #region Sell Transactions

        public void SellItemsValidateTransaction(Player player, List<WorldObject> items)
        {
            // todo: calculate payment based on multipliers correctly
            uint payout = 0;

            foreach (WorldObject wo in items)
            {
                // payout scaled by the vendor's buy rate
                payout = payout + (uint)Math.Ceiling(BuyRate * (wo.Value ?? 0) * (wo.StackSize ?? 1) - 0.1);
            }
            
            player.HandleActionSellFinalTransaction(this, items, true, payout);
        }

        public void SellItemsFinalTransaction(Player player, List<WorldObject> items, bool valid)
        {
            // todo Add logic to add unique items to vendor list.
            List<WorldObject> vendorlist = new List<WorldObject>();
            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
            {
                vendorlist.Add(wo.Value);
            }
            player.HandleActionApproachVendor(this, vendorlist);
        }
        #endregion
    }
}