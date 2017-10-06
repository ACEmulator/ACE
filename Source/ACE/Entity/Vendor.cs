﻿using ACE.Entity.Enum;
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
                items = DatabaseManager.World.GetVendorWeenieInventoryById(AceObject.WeenieClassId);
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
            player.HandleActionApproachVendor(this, vendorlist, SellRate);
        }
        #endregion

        #region BuyTransactions

        /// <summary>
        /// Items have been purchased from vendor by player but we need to validate it.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="items"></param>
        public void BuyItemsValidateTransaction(Player player, List<WorldObject> items)
        {
            // todo: add logic for temp stock items.
            // remove items, if transaction fails then move temp stock items back into temp stock
            player.HandleActionBuyFinalTransaction(this, items, true);
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
            player.HandleActionApproachVendor(this, vendorlist, SellRate);
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

        public void SellItemsFinalTransaction(Player player, List<WorldObject> items)
        {
            // todo Add logic to add unique items to vendor list.
            List<WorldObject> vendorlist = new List<WorldObject>();
            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
            {
                vendorlist.Add(wo.Value);
            }
            player.HandleActionApproachVendor(this, vendorlist, SellRate);
        }
        #endregion
    }
}