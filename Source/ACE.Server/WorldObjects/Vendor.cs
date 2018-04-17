using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.Motion;

using log4net;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// ** Usage Data Flow **
    ///     HandleActionApproachVendor
    /// ** Buy Data Flow **
    /// Player.HandleActionBuy->Vendor.BuyItemsValidateTransaction->Player.HandleActionBuyFinalTransaction->Vendor.BuyItemsFinalTransaction
    /// ** Sell Data Flow **
    /// Player.HandleActionSell->Vendor.SellItemsValidateTransaction->Player.HandleActionSellFinalTransaction->Vendor.SellItemsFinalTransaction
    /// </summary>
    public class Vendor : Creature
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<ObjectGuid, WorldObject> defaultItemsForSale = new Dictionary<ObjectGuid, WorldObject>();
        private Dictionary<ObjectGuid, WorldObject> uniqueItemsForSale = new Dictionary<ObjectGuid, WorldObject>();

        private bool inventoryloaded;

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
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the item being used.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(Player player)
        {
            //LoadInventory();

            var turnToMotion = new UniversalMotion(MotionStance.Standing, Location, Guid);
            turnToMotion.MovementTypes = MovementTypes.TurnToObject;

            ActionChain turnToTimer = new ActionChain();
            turnToTimer.AddAction(this, () => LoadInventory());
            turnToTimer.AddAction(player, () => player.CurrentLandblock.EnqueueBroadcastMotion(player, turnToMotion));
            turnToTimer.AddDelaySeconds(1);
            turnToTimer.AddAction(this, () => ApproachVendor(player));

            turnToTimer.EnqueueChain();

            //ApproachVendor(player);
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
        /// Sends Vendor Inventory to player
        /// </summary>
        /// <param name="player"></param>
        private void ApproachVendor(Player player)
        {
            // default inventory
            List<WorldObject> vendorlist = new List<WorldObject>();

            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
                vendorlist.Add(wo.Value);

            // unique inventory - itmes sold by other players
            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in uniqueItemsForSale)
                vendorlist.Add(wo.Value);

            player.TrackInteractiveObjects(vendorlist);
            player.ApproachVendor(this, vendorlist);
            OnAutonomousMove(player.Location, Sequences, MovementTypes.TurnToObject, player.Guid);
            DoVendorEmote(VendorType.Open, player);
        }


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

            // convert profile to wold objects / stack logic does not include unique items.
            foreach (ItemProfile fitem in filteredlist)
            {
                genlist.AddRange(ItemProfileToWorldObjects(fitem));
            }

            // calculate price. (both unique and item profile)
            foreach (WorldObject wo in uqlist)
            {
                goldcost = goldcost + (uint)Math.Ceiling((SellPrice ?? 1) * (wo.Value ?? 0) * (wo.StackSize ?? 1) - 0.1);
                wo.Value = wo.Value;                    // Also set the stack's value for unique items, using the builtin WO calculations
                wo.EncumbranceVal = wo.EncumbranceVal;  // Also set the stack's encumbrance for unique items, using the builtin WO calculations
            }

            foreach (WorldObject wo in genlist)
            {
                goldcost = goldcost + (uint)Math.Ceiling((SellPrice ?? 1) * (wo.Value ?? 0) * (wo.StackSize ?? 1) - 0.1);
                wo.Value = wo.Value;                    // Also set the stack's value for stock items, using the builtin WO calculations
                wo.EncumbranceVal = wo.EncumbranceVal;  // Also set the stack's encumbrance for stock items, using the builtin WO calculations
            }

            // send transaction to player for further processing and.
            player.FinalizeBuyTransaction(this, uqlist, genlist, true, goldcost);
        }

        /// <summary>
        /// Handles the final phase of the transaction.. removing unique items and updating players local
        /// from vendors items list.
        /// </summary>
        public void BuyItemsFinalTransaction(Player player, List<WorldObject> uqlist, bool valid)
        {
            if (!valid) // re-add unique temp stock items.
            {
                foreach (WorldObject wo in uqlist)
                {
                    if (!defaultItemsForSale.ContainsKey(wo.Guid))
                        uniqueItemsForSale.Add(wo.Guid, wo);
                }
            }

            ApproachVendor(player);
        }


        public void SellItemsValidateTransaction(Player player, List<WorldObject> items)
        {
            // todo: filter rejected / accepted send item spec result back to player
            uint payout = 0;
            List<WorldObject> accepted = new List<WorldObject>();
            List<WorldObject> rejected = new List<WorldObject>();

            foreach (WorldObject wo in items)
            {
                // payout scaled by the vendor's buy rate
                payout = payout + (uint)Math.Floor((BuyPrice ?? 1) * (wo.Value ?? 0) * (wo.StackSize ?? 1) + 0.1);

                if (!wo.MaxStackSize.HasValue & !wo.MaxStructure.HasValue)
                {
                    wo.Location = null;
                    wo.ContainerId = Guid.Full;
                    wo.PlacementPosition = null;
                    wo.WielderId = null;
                    wo.CurrentWieldedLocation = null;
                    // TODO: create enum for this once we understand this better.
                    // This is needed to make items lay flat on the ground.
                    wo.Placement = global::ACE.Entity.Enum.Placement.Resting;
                    uniqueItemsForSale.Add(wo.Guid, wo);
                }
                accepted.Add(wo);
            }

            ApproachVendor(player);
            player.FinalizeSellTransaction(this, true, accepted, payout);
        }

        public void DoVendorEmote(VendorType vendorType, WorldObject player)
        {
            switch (vendorType)
            {
                case VendorType.Open:
                    EmoteManager.DoVendorEmote(vendorType, player);
                    break;

                default:
                    log.Warn($"Vendor.DoVendorEmote - Encountered Unhandled VendorType {vendorType} for {Name} ({WeenieClassId})");
                    break;
            }
        }
    }
}
