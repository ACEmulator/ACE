using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// ** Buy Data Flow **
    ///
    /// Player.HandleActionBuyItem -> Vendor.BuyItems_ValidateTransaction -> Player.FinalizeBuyTransaction -> Vendor.BuyItems_FinalTransaction
    ///     
    /// </summary>
    public class Vendor : Creature
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly uint CoinStackWCID = DatabaseManager.World.GetCachedWeenie("coinstack").WeenieClassId;

        public readonly Dictionary<ObjectGuid, WorldObject> DefaultItemsForSale = new Dictionary<ObjectGuid, WorldObject>();

        // unique items purchased from other players
        public readonly Dictionary<ObjectGuid, WorldObject> UniqueItemsForSale = new Dictionary<ObjectGuid, WorldObject>();

        //public Dictionary<ObjectGuid, WorldObject> AllItemsForSale => DefaultItemsForSale.Concat(UniqueItemsForSale).ToDictionary(i => i.Key, i => i.Value);

        public Dictionary<ObjectGuid, WorldObject> AllItemsForSale
        {
            get
            {
                var allItems = new Dictionary<ObjectGuid, WorldObject>();

                foreach (var item in DefaultItemsForSale)
                    allItems.TryAdd(item.Key, item.Value);

                foreach (var item in UniqueItemsForSale)
                {
                    if (!allItems.TryAdd(item.Key, item.Value))
                        log.Error($"{Name} ({Guid}) AllItemsForSale: has duplicate item {item.Value.Name} ({item.Value.Guid})");
                }

                return allItems;
            }
        }

        private bool inventoryloaded;

        public Player LastPlayer;

        public uint? AlternateCurrency
        {
            get => GetProperty(PropertyDataId.AlternateCurrency);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.AlternateCurrency); else SetProperty(PropertyDataId.AlternateCurrency, value.Value); }
        }

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
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Vendor;

            if (!PropertyManager.GetBool("vendor_shop_uses_generator").Item)
            {
                GeneratorProfiles.RemoveAll(p => p.Biota.WhereCreate.HasFlag(RegenLocationType.Shop));
            }
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

        /// <summary>
        /// Sends the latest vendor inventory list to player, rotates vendor towards player, and performs the appropriate emote.
        /// </summary>
        /// <param name="action">The action performed by the player</param>
        private void ApproachVendor(Player player, VendorType action = VendorType.Undef, uint altCurrencySpent = 0)
        {
            var vendorList = AllItemsForSale.Values.ToList();

            vendorList = RotUniques(vendorList);

            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, this, vendorList, altCurrencySpent));

            var rotateTime = Rotate(player); // vendor rotates to player

            if (action != VendorType.Undef)
                DoVendorEmote(action, player);

            player.LastOpenedContainerId = Guid;
        }

        private List<WorldObject> RotUniques(List<WorldObject> worldObjects)
        {
            var results = new List<WorldObject>();

            foreach(var wo in worldObjects)
            {
                var soldTime = wo.GetProperty(PropertyFloat.SoldTimestamp);
                if (!soldTime.HasValue)
                {
                    results.Add(wo);
                    continue;
                }

                var rottime = Common.Time.GetDateTimeFromTimestamp(soldTime.Value);

                rottime = rottime.AddSeconds(PropertyManager.GetDouble("vendor_unique_rot_time", 300).Item);

                if (DateTime.UtcNow < rottime)
                    results.Add(wo);
                else
                {
                    UniqueItemsForSale.Remove(wo.Guid);
                    log.Debug($"[VENDOR] Vendor {Name} has discontinued sale of {wo.Name} and removed it from its UniqueItemsForSale list.");
                }
            }

            return results;
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

            var rotateTime = Rotate(player);    // vendor rotates towards player

            // TODO: remove this when DelayManager is not forward propagating current tick time

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(0.001f);  // force to run after rotate.EnqueueBroadcastAction
            actionChain.AddAction(this, LoadInventory);
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
            if (inventoryloaded)
                return;

            var itemsForSale = new Dictionary<(uint weenieClassId, int paletteTemplate, double shade), uint>();

            foreach (var item in Biota.PropertiesCreateList.Where(x => x.DestinationType == DestinationType.Shop))
            {
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                if (wo != null)
                {
                    if (item.Palette > 0)
                        wo.PaletteTemplate = item.Palette;
                    if (item.Shade > 0)
                        wo.Shade = item.Shade;
                    wo.ContainerId = Guid.Full;
                    wo.CalculateObjDesc();

                    if (!itemsForSale.ContainsKey((wo.WeenieClassId, wo.PaletteTemplate ?? 0, wo.Shade ?? 0))) // lets skip dupes if there are any
                    {
                        DefaultItemsForSale.Add(wo.Guid, wo);
                        itemsForSale.Add((wo.WeenieClassId, wo.PaletteTemplate ?? 0, wo.Shade ?? 0), wo.Guid.Full);
                    }
                    else
                        wo.Destroy(false);
                }
            }

            if (Biota.PropertiesGenerator != null && !PropertyManager.GetBool("vendor_shop_uses_generator").Item)
            {
                foreach (var item in Biota.PropertiesGenerator.Where(x => x.WhereCreate.HasFlag(RegenLocationType.Shop)))
                {
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                    if (wo != null)
                    {
                        if (item.PaletteId > 0)
                            wo.PaletteTemplate = (int)item.PaletteId;
                        if (item.Shade > 0)
                            wo.Shade = item.Shade;
                        wo.ContainerId = Guid.Full;
                        wo.CalculateObjDesc();

                        if (!itemsForSale.ContainsKey((wo.WeenieClassId, wo.PaletteTemplate ?? 0, wo.Shade ?? 0))) 
                        {
                            DefaultItemsForSale.Add(wo.Guid, wo);
                            itemsForSale.Add((wo.WeenieClassId, wo.PaletteTemplate ?? 0, wo.Shade ?? 0), wo.Guid.Full);
                        }
                        else
                            wo.Destroy(false);
                    }
                }
            }

            inventoryloaded = true;
        }

        public void AddDefaultItem(WorldObject item)
        {
            var existing = GetDefaultItemsByWcid(item.WeenieClassId);

            // add to existing stack?
            if (existing.Count > 0)
            {
                var stackLeft = existing.FirstOrDefault(i => (i.StackSize ?? 1) < (i.MaxStackSize ?? 1));
                if (stackLeft != null)
                {
                    stackLeft.SetStackSize((stackLeft.StackSize ?? 1) + 1);
                    return;
                }
            }

            // create new item
            item.ContainerId = Guid.Full;

            item.CalculateObjDesc();

            DefaultItemsForSale.Add(item.Guid, item);
        }

        public List<WorldObject> GetDefaultItemsByWcid(uint wcid)
        {
            return DefaultItemsForSale.Values.Where(i => i.WeenieClassId == wcid).ToList();
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

            var dist = GetCylinderDistance(LastPlayer);

            if (dist > UseRadius)
            {
                if (LastPlayer.LastOpenedContainerId == Guid)
                    LastPlayer.LastOpenedContainerId = ObjectGuid.Invalid;

                EmoteManager.DoVendorEmote(VendorType.Close, LastPlayer);
                LastPlayer = null;

                return;
            }

            var closeChain = new ActionChain();
            closeChain.AddDelaySeconds(CloseInterval);
            closeChain.AddAction(this, CheckClose);
            closeChain.EnqueueChain();
        }


        // =========================
        // Helper Functions - Buying
        // =========================

        /// <summary>
        /// Used to convert Weenie based objects / not used for unique items
        /// </summary>
        private List<WorldObject> ItemProfileToWorldObjects(ItemProfile itemprofile)
        {
            List<WorldObject> worldobjects = new List<WorldObject>();

            while (itemprofile.Amount > 0)
            {
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(itemprofile.WeenieClassId);

                if (itemprofile.Palette.HasValue)
                    wo.PaletteTemplate = itemprofile.Palette;
                if (itemprofile.Shade.HasValue)
                    wo.Shade = itemprofile.Shade;

                // can we stack this ?
                if (wo.MaxStackSize.HasValue)
                {
                    if ((wo.MaxStackSize.Value != 0) & (wo.MaxStackSize.Value <= itemprofile.Amount))
                    {
                        wo.SetStackSize(wo.MaxStackSize.Value);
                        worldobjects.Add(wo);
                        itemprofile.Amount = itemprofile.Amount - wo.MaxStackSize.Value;
                    }
                    else // we cant stack this but its not a single item
                    {
                        wo.SetStackSize((int)itemprofile.Amount);
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
        /// Handles validation for player buying items from vendor
        /// </summary>
        /// <param name="items">Item Profile, Ammount and ID</param>
        /// <param name="player"></param>
        public void BuyItems_ValidateTransaction(List<ItemProfile> items, Player player)
        {
            // queue transactions
            List<ItemProfile> filteredlist = new List<ItemProfile>();
            List<WorldObject> uqlist = new List<WorldObject>();
            List<WorldObject> genlist = new List<WorldObject>();

            uint goldcost = 0;
            uint altcost = 0;

            // filter items out vendor no longer has in stock or never had in stock
            foreach (ItemProfile item in items)
            {
                // check default items for id
                if (DefaultItemsForSale.ContainsKey(new ObjectGuid(item.ObjectGuid)))
                {
                    item.WeenieClassId = DefaultItemsForSale[new ObjectGuid(item.ObjectGuid)].WeenieClassId;
                    item.Palette = DefaultItemsForSale[new ObjectGuid(item.ObjectGuid)].PaletteTemplate;
                    item.Shade = DefaultItemsForSale[new ObjectGuid(item.ObjectGuid)].Shade;
                    filteredlist.Add(item);
                }

                // check unique items / add unique items to purchaselist / remove from vendor list
                if (UniqueItemsForSale.ContainsKey(new ObjectGuid(item.ObjectGuid)))
                {
                    if (UniqueItemsForSale.TryGetValue(new ObjectGuid(item.ObjectGuid), out var wo))
                    {
                        //wo.RemoveProperty(PropertyFloat.SoldTimestamp);
                        uqlist.Add(wo);
                        UniqueItemsForSale.Remove(new ObjectGuid(item.ObjectGuid));
                    }
                }
            }

            var playerFreeInventorySlots = player.GetFreeInventorySlots();
            var playerFreeContainerSlots = player.GetFreeContainerSlots();
            var playerAvailableBurden = player.GetAvailableBurden();

            var playerOutOfInventorySlots = false;
            var playerOutOfContainerSlots = false;
            var playerExceedsAvailableBurden = false;

            foreach (ItemProfile item in filteredlist)
            {
                var itemAmount = player.PreCheckItem(item.WeenieClassId, (int)item.Amount, playerFreeContainerSlots, playerFreeInventorySlots, playerAvailableBurden, out var itemEncumberance, out bool itemRequiresBackpackSlot);

                if (itemRequiresBackpackSlot)
                {
                    playerFreeContainerSlots -= itemAmount;
                    playerAvailableBurden -= itemEncumberance;

                    playerOutOfContainerSlots = playerFreeContainerSlots < 0;
                }
                else
                {
                    playerFreeInventorySlots -= itemAmount;
                    playerAvailableBurden -= itemEncumberance;

                    playerOutOfInventorySlots = playerFreeInventorySlots < 0;
                }
                               
                playerExceedsAvailableBurden = playerAvailableBurden < 0;

                if (playerOutOfInventorySlots || playerOutOfContainerSlots || playerExceedsAvailableBurden)
                    break;
            }

            if (playerOutOfInventorySlots || playerOutOfContainerSlots || playerExceedsAvailableBurden)
            {
                if (playerExceedsAvailableBurden)
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You are too encumbered to buy that!"));
                else if (playerOutOfInventorySlots)
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You do not have enough pack space to buy that!"));
                else //if (playerOutOfContainerSlots)
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You do not have enough container slots to buy that!"));

                player.Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(player.Session, player.Guid.Full));

                if (uqlist.Count > 0)
                {
                    foreach (var item in uqlist)
                    {
                        //item.SetProperty(PropertyFloat.SoldTimestamp, Common.Time.GetUnixTime());
                        UniqueItemsForSale.Add(item.Guid, item);
                    }
                }

                return;
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

                var cost = Math.Max(1, (uint)Math.Ceiling(((float)sellRate * (wo.Value ?? 0)) - 0.1));

                if (AlternateCurrency == null)
                    goldcost += cost;
                else
                    altcost += cost;
            }

            foreach (WorldObject wo in genlist)
            {
                var sellRate = SellPrice ?? 1.0;
                if (wo.ItemType == ItemType.PromissoryNote)
                    sellRate = 1.15;

                var cost = Math.Max(1, (uint)Math.Ceiling(((float)sellRate * (wo.Value ?? 0)) - 0.1));

                if (AlternateCurrency == null)
                    goldcost += cost;
                else
                    altcost += cost;
            }

            if (IsBusy && genlist.Any(i => i.GetProperty(PropertyBool.VendorService) == true))
            {
                player.SendWeenieErrorWithString(WeenieErrorWithString._IsTooBusyToAcceptGifts, Name);
                return;
            }

            // send transaction to player for further processing and.
            player.FinalizeBuyTransaction(this, uqlist, genlist, goldcost, altcost);
        }

        /// <summary>
        /// Handles the final phase of the transaction
        ///  for player buying items from vendor
        /// </summary>
        public void BuyItems_FinalTransaction(Player player, List<WorldObject> uqlist, bool valid, uint altCurrencySpent)
        {
            if (!valid) // re-add unique temp stock items.
            {
                foreach (WorldObject wo in uqlist)
                {
                    if (!DefaultItemsForSale.ContainsKey(wo.Guid))
                        UniqueItemsForSale.Add(wo.Guid, wo);
                }
            }
            ApproachVendor(player, VendorType.Buy, altCurrencySpent);
        }

        // ==========================
        // Helper Functions - Selling
        // ==========================

        public int CalculatePayoutCoinAmount(List<WorldObject> items)
        {
            int payout = 0;

            foreach (WorldObject wo in items)
            {
                var buyRate = BuyPrice ?? 1;

                if (wo.ItemType == ItemType.PromissoryNote)
                    buyRate = 1.0;

                // payout scaled by the vendor's buy rate
                payout += Math.Max(1, (int)Math.Floor(((float)buyRate * (wo.Value ?? 0)) + 0.1));
            }

            return payout;
        }

        /// <summary>
        /// This will either add the item to the vendors temporary sellables, or destroy it.<para />
        /// In both cases, the item will be removed from the database.<para />
        /// The item should already have been removed from the players inventory
        /// </summary>
        public void ProcessItemsForPurchase(Player player, List<WorldObject> items)
        {
            foreach (var item in items)
            {
                bool resellItem = true;

                // don't resell DestroyOnSell
                if (item.GetProperty(PropertyBool.DestroyOnSell) ?? false)
                    resellItem = false;

                // don't resell Attuned items that can be sold
                if (item.Attuned == AttunedStatus.Attuned)
                    resellItem = false;

                // don't resell stackables?
                if (item.MaxStackSize != null || item.MaxStructure != null)
                    resellItem = false;

                if (resellItem)
                {
                    item.ContainerId = Guid.Full;

                    if (!UniqueItemsForSale.TryAdd(item.Guid, item))
                    {
                        log.Error($"{Name}.ProcessItemsForPurchase({player.Name}): duplicate item found");
                        foreach (var i in items)
                            log.Error($"{i.Name} ({i.Guid})");
                    }

                    item.SetProperty(PropertyFloat.SoldTimestamp, Common.Time.GetUnixTime());

                    // remove object from shard db, but keep a reference to it in memory
                    // for DestroyOnSell items, these will effectively be destroyed immediately
                    // for other items, if a player re-purchases, it will be added to the shard db again
                    item.RemoveBiotaFromDatabase();
                }
                else
                {
                    item.Destroy();
                }
            }

            ApproachVendor(player, VendorType.Sell);
        }
    }
}
