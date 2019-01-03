using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;

using log4net;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// ** Buy Data Flow **
    ///
    /// Player.HandleActionBuyItem -> Vendor.BuyItems_ValidateTransaction -> Player.FinalizeBuyTransaction -> Vendor.BuyItems_FinalTransaction
    ///     
    /// ** Sell Data Flow **
    ///
    /// Player.HandleActionSellItem -> Vendor.SellItems_ValidateTransaction -> Player.FinalizeSellTransaction -> Vendor.SellItems_FinalTransaction
    /// 
    /// </summary>
    public class Vendor : Creature
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<ObjectGuid, WorldObject> defaultItemsForSale = new Dictionary<ObjectGuid, WorldObject>();
        private Dictionary<ObjectGuid, WorldObject> uniqueItemsForSale = new Dictionary<ObjectGuid, WorldObject>();

        private bool inventoryloaded;

        public Player LastPlayer;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Vendor(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Vendor(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Vendor;
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject wo)
        {
            var player = wo as Player;
            if (player == null) return;

            LoadInventory();

            var actionChain = new ActionChain();
            var rotateTime = Rotate(player);    // vendor rotates towards player
            actionChain.AddDelaySeconds(rotateTime);
            actionChain.AddAction(this, () => ApproachVendor(player, VendorType.Open));
            actionChain.EnqueueChain();

            if (LastPlayer == null)
            {
                var closeChain = new ActionChain();
                closeChain.AddDelaySeconds(CloseInterval);
                closeChain.AddAction(this, CheckClose);
                closeChain.EnqueueChain();
            }

            LastPlayer = player;
        }

        /// <summary>
        /// Load Inventory for default items from database table / assignes default objects.
        /// </summary>
        private void LoadInventory()
        {
            // Load Vendor Inventory from database.
            if (!inventoryloaded)
            {
                foreach (var item in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (int)DestinationType.Shop))
                {
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                    if (wo != null)
                    {
                        if (item.Palette > 0)
                            wo.PaletteTemplate = item.Palette;
                        if (item.Shade > 0)
                            wo.Shade = item.Shade;
                        wo.ContainerId = Guid.Full;
                        wo.CalculateObjDesc(); // i don't like firing this but this triggers proper icons, the way vendors load inventory feels off to me in this method.
                        defaultItemsForSale.Add(wo.Guid, wo);
                    }
                }
                inventoryloaded = true;
            }
        }

        /// <summary>
        /// Used to convert Weenie based objects / not used for unique items
        /// </summary>
        private List<WorldObject> ItemProfileToWorldObjects(ItemProfile itemprofile)
        {
            List<WorldObject> worldobjects = new List<WorldObject>();
            while (itemprofile.Amount > 0)
            {
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(itemprofile.WeenieClassId);
                // can we stack this ?
                if (wo.MaxStackSize.HasValue)
                {
                    if ((wo.MaxStackSize.Value != 0) & (wo.MaxStackSize.Value <= itemprofile.Amount))
                    {
                        wo.StackSize = wo.MaxStackSize.Value;
                        worldobjects.Add(wo);
                        itemprofile.Amount = itemprofile.Amount - wo.MaxStackSize.Value;
                    }
                    else // we cant stack this but its not a single item
                    {
                        wo.StackSize = (ushort)itemprofile.Amount;
                        worldobjects.Add(wo);
                        itemprofile.Amount = itemprofile.Amount - itemprofile.Amount;
                    }
                }
                else
                {
                    // if there multiple items of the same  type.. 
                    if (itemprofile.Amount > 0)
                    {
                        // single item with no stack options. 
                        itemprofile.Amount = itemprofile.Amount - 1;
                        wo.StackSize = null;
                        worldobjects.Add(wo);
                    }
                }
            }
            return worldobjects;
        }

        /// <summary>
        /// Sends the latest vendor inventory list to player,
        /// rotates vendor towards player, and performs the appropriate emote.
        /// </summary>
        /// <param name="action">The action performed by the player</param>
        private void ApproachVendor(Player player, VendorType action = VendorType.Undef)
        {
            // default inventory
            List<WorldObject> vendorlist = new List<WorldObject>();

            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
                vendorlist.Add(wo.Value);

            // unique inventory - items sold by other players
            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in uniqueItemsForSale)
                vendorlist.Add(wo.Value);

            player.TrackInteractiveObjects(vendorlist);
            player.ApproachVendor(this, vendorlist);

            var rotateTime = Rotate(player); // vendor rotates to player

            if (action != VendorType.Undef)
            {
                DoVendorEmote(action, player);
            }
        }


        /// <summary>
        /// Handles validation for player buying items from vendor
        /// </summary>
        public void BuyItems_ValidateTransaction(ObjectGuid vendorid, List<ItemProfile> items, Player player)
        {
            // queue transactions
            List<ItemProfile> filteredlist = new List<ItemProfile>();
            List<WorldObject> uqlist = new List<WorldObject>();
            List<WorldObject> genlist = new List<WorldObject>();

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

                // check unique items / add unique items to purchaselist / remove from vendor list
                if (uniqueItemsForSale.ContainsKey(item.Guid))
                {
                    if (uniqueItemsForSale.TryGetValue(item.Guid, out var wo))
                    {
                        uqlist.Add(wo);
                        uniqueItemsForSale.Remove(item.Guid);
                    }
                }
            }

            // convert profile to world objects / stack logic does not include unique items.
            foreach (ItemProfile fitem in filteredlist)
            {
                genlist.AddRange(ItemProfileToWorldObjects(fitem));
            }

            // calculate price. (both unique and item profile)
            foreach (WorldObject wo in uqlist)
            {
                var sellRate = SellPrice ?? 1.0;
                if (wo.ItemType == ItemType.PromissoryNote)
                    sellRate = 1.15;

                goldcost += (uint)Math.Ceiling((wo.Value ?? 0) * sellRate - 0.1);
            }

            foreach (WorldObject wo in genlist)
            {
                var sellRate = SellPrice ?? 1.0;
                if (wo.ItemType == ItemType.PromissoryNote)
                    sellRate = 1.15;

                goldcost += (uint)Math.Ceiling((wo.Value ?? 0) * sellRate - 0.1);
            }

            // send transaction to player for further processing and.
            player.FinalizeBuyTransaction(this, uqlist, genlist, true, goldcost);
        }

        /// <summary>
        /// Handles the final phase of the transaction
        ///  for player buying items from vendor
        /// </summary>
        public void BuyItems_FinalTransaction(Player player, List<WorldObject> uqlist, bool valid)
        {
            if (!valid) // re-add unique temp stock items.
            {
                foreach (WorldObject wo in uqlist)
                {
                    if (!defaultItemsForSale.ContainsKey(wo.Guid))
                        uniqueItemsForSale.Add(wo.Guid, wo);
                }
            }
            ApproachVendor(player, VendorType.Buy);
        }

        /// <summary>
        /// Handles validation for player selling items to vendor
        /// </summary>
        public void SellItems_ValidateTransaction(Player player, List<WorldObject> items)
        {
            // todo: filter rejected / accepted send item spec result back to player
            uint payout = 0;
            List<WorldObject> accepted = new List<WorldObject>();
            List<WorldObject> rejected = new List<WorldObject>();

            foreach (WorldObject wo in items)
            {
                var buyRate = BuyPrice ?? 1;
                if (wo.ItemType == ItemType.PromissoryNote)
                    buyRate = 1.0;

                // payout scaled by the vendor's buy rate
                payout += (uint)Math.Floor((wo.Value ?? 0) * buyRate + 0.1);

                if (!wo.MaxStackSize.HasValue & !wo.MaxStructure.HasValue)
                {
                    wo.Location = null;
                    wo.ContainerId = Guid.Full;
                    wo.PlacementPosition = null;
                    wo.WielderId = null;
                    wo.CurrentWieldedLocation = null;
                    wo.Placement = ACE.Entity.Enum.Placement.Resting;
                    uniqueItemsForSale.Add(wo.Guid, wo);
                }
                accepted.Add(wo);
            }

            ApproachVendor(player, VendorType.Sell);

            player.FinalizeSellTransaction(this, true, accepted, payout);
        }

        public void DoVendorEmote(VendorType vendorType, WorldObject player)
        {
            switch (vendorType)
            {
                case VendorType.Open:
                    EmoteManager.DoVendorEmote(vendorType, player);
                    break;

                case VendorType.Buy:    // player buys item from vendor
                    EmoteManager.DoVendorEmote(vendorType, player);
                    break;

                case VendorType.Sell:   // player sells item to vendor
                    EmoteManager.DoVendorEmote(vendorType, player);
                    break;

                default:
                    log.Warn($"Vendor.DoVendorEmote - Encountered Unhandled VendorType {vendorType} for {Name} ({WeenieClassId})");
                    break;
            }
        }

        public float CloseInterval = 1.5f;

        public void CheckClose()
        {
            if (LastPlayer == null)
                return;

            // handles player logging out at vendor
            if (LastPlayer.CurrentLandblock == null)
            {
                LastPlayer = null;
                return;
            }

            var dist = Vector3.Distance(Location.ToGlobal(), LastPlayer.Location.ToGlobal());
            if (dist > UseRadius)
            {
                EmoteManager.DoVendorEmote(VendorType.Close, LastPlayer);
                LastPlayer = null;

                return;
            }

            var closeChain = new ActionChain();
            closeChain.AddDelaySeconds(CloseInterval);
            closeChain.AddAction(this, CheckClose);
            closeChain.EnqueueChain();
        }
    }
}
