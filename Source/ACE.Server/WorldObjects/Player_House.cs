using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public House House;

        public Dictionary<ObjectGuid, bool> Guests => House?.Guests;

        /// <summary>
        /// Called when player clicks the 'Buy house' button,
        /// after adding the items required
        /// </summary>
        public void HandleActionBuyHouse(uint slumlord_id, List<uint> item_ids)
        {
            //Console.WriteLine($"\n{Name}.HandleActionBuyHouse()");
            log.Info($"[HOUSE] {Name}.HandleActionBuyHouse()");

            // verify player doesn't already own a house
            var houseInstance = GetHouseInstance();

            if (houseInstance != null)
            {
                //Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.HouseAlreadyOwned));
                Session.Network.EnqueueSend(new GameMessageSystemChat("You already own a house!", ChatMessageType.Broadcast));
                log.Info($"[HOUSE] {Name}.HandleActionBuyHouse(): Failed pre-purchase requirement - Already owns another house");
                return;
            }

            var slumlord = (SlumLord)CurrentLandblock.GetObject(slumlord_id);
            if (slumlord == null)
            {
                log.Error($"[HOUSE] {Name}.HandleActionBuyHouse: Couldn't find slumlord 0x{slumlord_id:X8}!");
                return;
            }

            if (slumlord.MinLevel != null)
            {
                var playerLevel = Level ?? 1;
                if (playerLevel < slumlord.MinLevel)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouMustBeAboveLevel_ToBuyHouse, slumlord.MinLevel.ToString()));
                    log.Info($"[HOUSE] {Name}.HandleActionBuyHouse(): Failed pre-purchase requirement - MinLevel");
                    return;
                }
            }

            if (slumlord.HouseRequiresMonarch)
            {
                if (Allegiance == null || Allegiance.MonarchId != Guid.Full)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustBeMonarchToPurchaseDwelling));
                    log.Info($"[HOUSE] {Name}.HandleActionBuyHouse(): Failed pre-purchase requirement - HouseRequiresMonarch");
                    return;
                }
            }

            if (slumlord.AllegianceMinLevel != null)
            {
                var allegianceMinLevel = PropertyManager.GetLong("mansion_min_rank", -1).Item;
                if (allegianceMinLevel == -1)
                    allegianceMinLevel = slumlord.AllegianceMinLevel.Value;

                if (allegianceMinLevel > 0 && (Allegiance == null || AllegianceNode.Rank < allegianceMinLevel))
                {
                    Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouMustBeAboveAllegianceRank_ToBuyHouse, allegianceMinLevel.ToString()));
                    log.Info($"[HOUSE] {Name}.HandleActionBuyHouse(): Failed pre-purchase requirement - AllegianceMinLevel");
                    return;
                }
            }

            if (slumlord.House.HouseType != HouseType.Apartment)
            {
                if (PropertyManager.GetBool("house_15day_account").Item && !Account15Days)
                {
                    var accountTimeSpan = DateTime.UtcNow - Account.CreateTime;
                    if (accountTimeSpan.TotalDays < 15)
                    {
                        var msg = "Your account must be at least 15 days old to purchase this dwelling. This applies to all housing except apartments.";
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                        log.Info($"[HOUSE] {Name}.HandleActionBuyHouse(): Failed pre-purchase requirement - house_15day_account");
                        return;
                    }
                }

                if (PropertyManager.GetBool("house_30day_cooldown").Item)
                {
                    // fix gap
                    if (!Account15Days) ManageAccount15Days_HousePurchaseTimestamp();

                    var lastPurchaseTime = Time.GetDateTimeFromTimestamp(HousePurchaseTimestamp ?? 0);
                    var lastPurchaseTimePlus30 = lastPurchaseTime.AddDays(30);

                    if (lastPurchaseTimePlus30 > DateTime.UtcNow)
                    {
                        Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustWaitToPurchaseHouse));
                        log.Info($"[HOUSE] {Name}.HandleActionBuyHouse(): Failed pre-purchase requirement - house_30day_cooldown");
                        return;
                    }
                }
            }

            var verified = VerifyPurchase(slumlord, item_ids);
            if (!verified)
            {
                log.Warn($"[HOUSE] {Name} tried to purchase house {slumlord.Guid} without the required items!");
                return;
            }

            //Console.WriteLine("\nInventory check passed!");
            log.Info($"[HOUSE] {Name}.HandleActionBuyHouse(): Inventory check passed!");

            // get the list of items / amounts to consume for purchase
            var houseProfile = slumlord.GetHouseProfile();
            var items = GetInventoryItems(item_ids);

            var consumeItems = GetConsumeItems(houseProfile.Buy, items);

            if (!TryConsumePurchaseItems(consumeItems))
            {
                var item_id_list = string.Join(", ", item_ids.Select(i => i.ToString("X8")));
                var consumeItemsList = string.Join(", ", consumeItems.Select(i => $"{i.Name} ({i.Guid}) x{i.Value}"));

                log.Error($"[HOUSE] {Name}.HandleActionBuyHouse({slumlord_id:X8}, {item_id_list}) - TryConsumePurchaseItems failed with {consumeItemsList}");

                return;
            }

            SetHouseOwner(slumlord);

            GiveDeed(slumlord);
        }

        public void GiveDeed(SlumLord slumLord)
        {
            var deed = WorldObjectFactory.CreateNewWorldObject("deed");

            var title = CharacterTitleId != null ? GetTitle((CharacterTitle)CharacterTitleId) : null;
            var titleStr = title != null ? $", {title}" : "";

            var derethDateTime = DerethDateTime.UtcNowToLoreTime;
            var date = derethDateTime.DateToString();
            var time = derethDateTime.TimeToString();
            var location = slumLord.Location.GetMapCoordStr();
            if (location == null)
            {
                if (!HouseManager.ApartmentBlocks.TryGetValue(slumLord.Location.Landblock, out location))
                    log.Error($"{Name}.GiveDeed() - couldn't find location {slumLord.Location.ToLOCString()}");
            }

            deed.LongDesc = $"Bought by {Name}{titleStr} on {date} at {time}\n\nPurchased at {location}";

            TryCreateInInventoryWithNetworking(deed);
        }

        /// <summary>
        /// Removes the house deed from the player's inventory when they abandon the house
        /// </summary>
        public void RemoveDeed()
        {
            var deeds = GetInventoryItemsOfWCID(9549);
            if (deeds == null)
            {
                log.Warn($"[HOUSE] {Name}.RemoveDeed(): couldn't find inventory deed");
                return;
            }
            foreach (var deed in deeds)
                TryConsumeFromInventoryWithNetworking(deed);
        }

        public void HandleActionRentHouse(uint slumlord_id, List<uint> item_ids)
        {
            //Console.WriteLine($"{Name}.HandleActionRentHouse({slumlord_id:X8}, {string.Join(", ", item_ids.Select(i => i.ToString("X8")))})");
            log.Info($"[HOUSE] {Name}.HandleActionRentHouse({slumlord_id:X8}, {string.Join(", ", item_ids.Select(i => i.ToString("X8")))})");

            var slumlord = FindObject(slumlord_id, SearchLocations.Landblock) as SlumLord;
            if (slumlord == null)
                return;

            if (slumlord.IsRentPaid())
            {
                //Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.HouseRentFailed));  // WeenieError.HouseRentFailed == blank message
                Session.Network.EnqueueSend(new GameMessageSystemChat("The maintenance has already been paid for this period.\nYou may not prepay next period's maintenance.", ChatMessageType.Broadcast));
                return;
            }

            var owner = PlayerManager.FindByGuid(slumlord.HouseOwner ?? 0);
            if (owner != null)
            {
                var characterHouses = HouseManager.GetCharacterHouses(owner.Guid.Full);
                var accountHouses = HouseManager.GetAccountHouses(owner.Account.AccountId);

                var ownerHouses = PropertyManager.GetBool("house_per_char").Item ? characterHouses : accountHouses;

                if (ownerHouses.Count() > 1)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat("The owner of this house currently owns multiple houses. Maintenance cannot be paid until they only own 1 house.", ChatMessageType.Broadcast));
                    return;
                }
            }
            else
                log.Error($"[HOUSE] {Name}.HandleActionRentHouse({slumlord_id:X8}): couldn't find house owner {slumlord.HouseOwner}");


            var logLine = $"[HOUSE] HandleActionRentHouse:" + Environment.NewLine;
            logLine += $"{slumlord.Name} ({slumlord.Guid})" + Environment.NewLine;
            var rentItems = slumlord.GetRentItems();
            //Console.WriteLine("Required items:");
            logLine += "Required items:";
            foreach (var buyItem in rentItems)
            {
                var stackStr = buyItem.StackSize != null && buyItem.StackSize > 1 ? buyItem.StackSize.ToString() + " " : "";
                //Console.WriteLine($"{stackStr}{buyItem.Name}");
                logLine += $"{stackStr}{buyItem.Name}" + Environment.NewLine;
            }

            //Console.WriteLine("\nSent items:");
            logLine += Environment.NewLine + "Sent items:" + Environment.NewLine;
            var sentItems = new List<WorldObject>();
            foreach (var item_id in item_ids)
            {
                var item = GetInventoryItem(new ObjectGuid(item_id));
                if (item == null)
                {
                    //Console.WriteLine($"Couldn't find inventory item {item_id:X8}");
                    logLine += $"Couldn't find inventory item {item_id:X8}" + Environment.NewLine;
                    continue;
                }
                var stackStr = item.StackSize != null && item.StackSize > 1 ? item.StackSize.ToString() + " " : "";
                //Console.WriteLine($"{stackStr}{item.Name} ({item.Guid})");
                logLine += $"{stackStr}{item.Name} ({item.Guid})" + Environment.NewLine;

                if (IsTrading && item.IsBeingTradedOrContainsItemBeingTraded(ItemsInTradeWindow))
                {
                    //Console.WriteLine($"{stackStr}{item.Name} ({item.Guid}) is currently being traded, skipping.");
                    logLine += $"{stackStr}{item.Name} ({item.Guid}) is currently being traded, skipping." + Environment.NewLine;
                    continue;
                }
                sentItems.Add(item);
            }
            //Console.WriteLine();
            logLine += Environment.NewLine;
            log.Info(logLine);

            // filter to items found in player's inventory
            var items = GetInventoryItems(item_ids);

            // get the list of items / amounts to consume for rent remaining
            var houseProfile = slumlord.GetHouseProfile();
            var consumeItems = GetConsumeItems(houseProfile.Rent, items);

            if (IsTrading)
            {
                foreach (var item in consumeItems.ToList())
                {
                    if (ItemsInTradeWindow.Contains(item.Guid))
                        consumeItems.Remove(item);
                }
            }

            if (consumeItems.Count == 0)
                return;

            foreach (var consumeItem in consumeItems)
                TryConsumeItemForRent(slumlord, consumeItem);

            slumlord.MergeAllStackables();

            // force save to database
            slumlord.SaveBiotaToDatabase();

            foreach (var item in slumlord.Inventory.Values)
                item.SaveBiotaToDatabase();

            slumlord.ActOnUse(this);

            HandleActionQueryHouse();

            Session.Network.EnqueueSend(new GameMessageSystemChat($"Maintenance {(slumlord.IsRentPaid() ? "" : "partially ")}paid.", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Returns the WorldObjects for item_ids that are in the player's inventory
        /// </summary>
        public List<WorldObject> GetInventoryItems(List<uint> item_ids)
        {
            var inventoryItems = new List<WorldObject>();

            foreach (var item_id in item_ids)
            {
                var item = FindObject(item_id, SearchLocations.MyInventory);

                if (item != null)
                    inventoryItems.Add(item);
                else
                    log.Error($"{Name}.GetInventoryItems() - couldn't find {item_id:X8}");
            }

            return inventoryItems;
        }

        /// <summary>
        /// Returns the amount of items to consume for a house purchase / maintenance payment
        /// </summary>
        public List<WorldObjectInfo<int>> GetConsumeItems(List<HousePayment> houseItems, List<WorldObject> playerItems)
        {
            var consumeItems = new List<WorldObjectInfo<int>>();

            foreach (var houseItem in houseItems)
            {
                consumeItems.AddRange(houseItem.GetConsumeItems(playerItems));
            }

            return consumeItems;
        }

        /// <summary>
        /// Moves or splits an item from player inventory => slumlord inventory
        /// </summary>
        public bool TryConsumeItemForRent(SlumLord slumlord, WorldObjectInfo<int> itemInfo)
        {
            var item = itemInfo.TryGetWorldObject();
            if (item == null)
            {
                log.Error($"[HOUSE] {Name}.TryConsumeItemForRent({itemInfo.Guid:X8}) - couldn't get item");
                return false;
            }

            var amount = itemInfo.Value;
            var stackSize = item.StackSize ?? 1;

            if (amount > stackSize)
            {
                log.Error($"[HOUSE] {Name}.TryConsumeItemForRent({item.Name} ({item.Guid}) - amount {amount} > stacksize {stackSize}");
                return false;
            }

            var success = false;

            if (amount == stackSize)
                success = TryMoveItemForRent(slumlord, item);
            else
                success = TrySplitItemForRent(slumlord, item, amount);

            return success;
        }

        /// <summary>
        /// Moves an item from player inventory => slumlord inventory
        /// </summary>
        public bool TryMoveItemForRent(SlumLord slumlord, WorldObject item)
        {
            // verify slumlord can add item to inventory
            if (!slumlord.CanAddToInventory(item))
            {
                log.Error($"[HOUSE] {Name}.TryMoveItemForRent({slumlord.Name} ({slumlord.Guid}), {item.Name} ({item.Guid}) ) - CanAddToInventory failed!");
                return false;
            }

            // remove entire item from player's inventory
            if (!TryRemoveFromInventoryWithNetworking(item.Guid, out _, RemoveFromInventoryAction.SpendItem))
            {
                log.Error($"[HOUSE] {Name}.TryMoveItemForRent({slumlord.Name} ({slumlord.Guid}), {item.Name} ({item.Guid}) ) - TryRemoveFromInventoryWithNetworking failed!");
                return false;
            }

            // add to slumlord inventory
            if (!slumlord.TryAddToInventory(item))
            {
                log.Error($"[HOUSE] {Name}.TryMoveItemForRent({slumlord.Name} ({slumlord.Guid}), {item.Name} ({item.Guid}) ) - TryAddToInventory failed!");
                return false;
            }

            log.Debug($"[HOUSE] {Name}.TryMoveItemForRent({slumlord.Name} ({slumlord.Guid}), {((item.StackSize ?? 1) > 1 ? $"{item.StackSize}x " : "")}{item.Name} ({item.Guid})) - Successfully moved to Slumlord.");
            return true;
        }

        /// <summary>
        /// Splits an item from player inventory => slumlord inventory
        /// </summary>
        public bool TrySplitItemForRent(SlumLord slumlord, WorldObject item, int amount)
        {
            // create a new item w/ stacksize = amount for the slumlord's inventory
            var newItem = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);
            newItem.SetStackSize(amount);

            // verify it can be added to slumlord's inventory
            if (!slumlord.CanAddToInventory(newItem))
            {
                log.Error($"[HOUSE] {Name}.TrySplitItemForRent({slumlord.Name} ({slumlord.Guid}), {item.Name} ({item.Guid}), {amount}) - CanAddToInventory failed for split item {newItem.Name} ({newItem.Guid})!");
                newItem.Destroy();
                return false;
            }

            // fetch container for AdjustStack
            if (GetInventoryItem(item.Guid, out var container) == null)
            {
                log.Error($"[HOUSE] {Name}.TrySplitItemForRent({slumlord.Name} ({slumlord.Guid}), {item.Name} ({item.Guid}), {amount}) - GetInventoryItem failed!");
                newItem.Destroy();
                return false;
            }

            // subtract amount from player's item stacksize
            if (!AdjustStack(item, -amount, container, this))
            {
                log.Error($"[HOUSE] {Name}.TrySplitItemForRent({slumlord.Name} ({slumlord.Guid}), {item.Name} ({item.Guid}), {amount}) - failed to adjust stack!");
                newItem.Destroy();
                return false;
            }

            // force save of new player stack
            item.SaveBiotaToDatabase();

            // send network updates
            Session.Network.EnqueueSend(new GameMessageSetStackSize(item));
            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (item.WeenieType == WeenieType.Coin)
                UpdateCoinValue();

            if (!slumlord.TryAddToInventory(newItem))
            {
                log.Error($"[HOUSE] {Name}.TrySplitItemForRent({slumlord.Name} ({slumlord.Guid}), {item.Name} ({item.Guid}), {amount}) - TryAddToInventory failed for split item {newItem.Name} ({newItem.Guid})!");
                newItem.Destroy();
                return false;
            }

            log.Debug($"[HOUSE] {Name}.TrySplitItemForRent({slumlord.Name} ({slumlord.Guid}), {item.Name} ({item.Guid}), {amount}) - Created new item {((newItem.StackSize ?? 1) > 1 ? $"{newItem.StackSize}x " : "")}{newItem.Name} ({newItem.Guid}) and moved to Slumlord.");

            // force save of new slumlord stack
            newItem.SaveBiotaToDatabase();

            return true;
        }
        
        public void HandleActionAbandonHouse()
        {
            //Console.WriteLine($"\n{Name}.HandleActionAbandonHouse()");
            log.Info($"[HOUSE] {Name}.HandleActionAbandonHouse()");

            var houseInstance = GetHouseInstance();

            if (houseInstance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            // only the character who directly owns the house can use /house abandon
            if (HouseInstance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.OnlyHouseOwnerCanUseCommand));
                return;
            }

            var house = GetHouse();
            if (house != null)
            {
                house.HouseOwner = null;
                house.MonarchId = null;
                house.HouseOwnerName = null;
                house.ClearPermissions();

                house.SaveBiotaToDatabase();

                // relink
                house.UpdateLinks();

                if (house.HasDungeon)
                {
                    var dungeonHouse = house.GetDungeonHouse();
                    if (dungeonHouse != null)
                        dungeonHouse.UpdateLinks();
                }

                // player slumlord 'off' animation
                var slumlord = house.SlumLord;
                slumlord.ClearInventory();
                slumlord.Off();

                // reset slumlord name
                slumlord.SetAndBroadcastName();

                slumlord.SaveBiotaToDatabase();

                HouseList.AddToAvailable(slumlord);
            }

            HouseId = null;
            HouseInstance = null;
            //HousePurchaseTimestamp = null;
            HouseRentTimestamp = null;

            House = null;
            houseRentWarnTimestamp = 0;

            SaveBiotaToDatabase();

            // send text message
            Session.Network.EnqueueSend(new GameMessageSystemChat("You abandon your house!", ChatMessageType.Broadcast));

            HouseManager.RemoveRentQueue(house.Guid.Full);

            house.ClearRestrictions();

            RemoveDeed();

            HandleActionQueryHouse();
        }

        public void HandleHouseOnLogin()
        {
            var evicted = GetProperty(PropertyBool.HouseEvicted) ?? false;
            if (evicted)
            {
                var evictChain = new ActionChain();
                evictChain.AddDelaySeconds(5.0f);   // todo: need inventory callback
                evictChain.AddAction(this, HandleEviction);
                evictChain.EnqueueChain();
                return;
            }

            var houseInstance = GetHouseInstance();

            if (House == null) LoadHouse(houseInstance);
            if (House == null || House.SlumLord == null) return;

            var houseOwner = GetHouseOwner();

            // var purchaseTime = (uint)(houseOwner.HousePurchaseTimestamp ?? 0);

            if (HousePurchaseTimestamp != houseOwner.HousePurchaseTimestamp)
            {
                HousePurchaseTimestamp = houseOwner.HousePurchaseTimestamp;
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.HousePurchaseTimestamp, HousePurchaseTimestamp ?? 0), new GameMessageSystemChat("Updating housing information...", ChatMessageType.Broadcast), new GameEventHouseStatus(Session, WeenieError.HouseEvicted));
            }

            // var rentTime = (uint)(houseOwner.HouseRentTimestamp ?? 0);

            if (HouseRentTimestamp != houseOwner.HouseRentTimestamp)
                HouseRentTimestamp  = houseOwner.HouseRentTimestamp;

            if (!House.SlumLord.InventoryLoaded)
            {
                HouseManager.RegisterCallback(House, (house) => HandleHouseOnLogin_Inner());
            }
            else
                HandleHouseOnLogin_Inner();
        }

        public void HandleHouseOnLogin_Inner()
        {
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(5.0f);
            actionChain.AddAction(this, () =>
            {
                if (House == null || House.SlumLord == null) return;

                if (House.HouseStatus == HouseStatus.Active && !House.SlumLord.IsRentPaid() && PropertyManager.GetBool("house_rent_enabled", true).Item)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Warning!  You have not paid your maintenance costs for the last {(House.IsApartment ? "90" : "30")} day maintenance period.  Please pay these costs by this deadline or you will lose your house, and all your items within it.", ChatMessageType.System));
                }

                if (House.HouseOwner == Guid.Full && !House.SlumLord.HasRequirements(this) && PropertyManager.GetBool("house_purchase_requirements").Item)
                {
                    var rankStr = AllegianceNode != null ? $"{AllegianceNode.Rank}" : "";
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Warning!  Your allegiance rank {rankStr} is now below the requirements for owning a mansion.  Please raise your allegiance rank to {House.SlumLord.GetAllegianceMinLevel()} before the end of the maintenance period or you will lose your mansion, and all your items within it.", ChatMessageType.System));
                }

                // TODO: for account houses, run this even if char doesn't own house
                IsMultiHouseOwner();
            });
            actionChain.EnqueueChain();
        }

        public void HandleEviction()
        {
            Session.Network.EnqueueSend(new GameMessageSystemChat("Your house has reverted due to non-payment of the maintenance costs.  All items stored in the house have been lost.", ChatMessageType.Broadcast));
            RemoveDeed();

            RemoveProperty(PropertyBool.HouseEvicted);
        }

        /// <summary>
        /// Sets this player as the owner of a house
        /// </summary>
        public void SetHouseOwner(SlumLord slumlord)
        {
            var house = slumlord.House;

            //Console.WriteLine($"Setting {Name} as owner of {house.Name}");
            log.Info($"[HOUSE] Setting {Name} (0x{Guid}) as owner of {house.Name} (0x{house.Guid:X8})");

            // set player properties
            HouseId = house.HouseId;
            HouseInstance = house.Guid.Full;

            var housePurchaseTimestamp = Time.GetUnixTime();
            if (house.HouseType != HouseType.Apartment)
                HousePurchaseTimestamp = (int)housePurchaseTimestamp;
            HouseRentTimestamp = (int)house.GetRentDue((uint)housePurchaseTimestamp);
            houseRentWarnTimestamp = 0;

            // set house properties
            house.HouseOwner = Guid.Full;
            house.HouseOwnerName = Name;
            house.OpenToEveryone = false;
            house.SaveBiotaToDatabase();
            
            // relink
            house.UpdateLinks();

            if (house.HasDungeon)
            {
                var dungeonHouse = house.GetDungeonHouse();
                if (dungeonHouse != null)
                    dungeonHouse.UpdateLinks();
            }

            // notify client w/ HouseID
            Session.Network.EnqueueSend(new GameMessageSystemChat("Congratulations!  You now own this dwelling.", ChatMessageType.Broadcast));

            // player slumlord 'on' animation
            slumlord.On();

            // set house name
            slumlord.SetAndBroadcastName(Name);

            slumlord.ClearInventory();

            slumlord.SaveBiotaToDatabase();

            HouseList.RemoveFromAvailable(slumlord);

            SaveBiotaToDatabase();

            if (house.HouseType != HouseType.Apartment)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.HousePurchaseTimestamp, HousePurchaseTimestamp ?? 0));

            // set house data
            // why has this changed? use callback?
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(3.0f);
            actionChain.AddAction(this, () =>
            {
                HandleActionQueryHouse();
                house.UpdateRestrictionDB();

                // boot anyone who may have been wandering around inside...
                HandleActionBootAll(false);

                HouseManager.AddRentQueue(this, house.Guid.Full);
                slumlord.ActOnUse(this);
            });
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Removes verified items from inventory for house purchase
        /// </summary>
        public bool TryConsumePurchaseItems(List<WorldObjectInfo<int>> purchaseItems)
        {
            foreach (var purchaseItem in purchaseItems)
            {
                var item = FindObject(purchaseItem.Guid.Full, SearchLocations.MyInventory);

                if (item == null)
                {
                    // this should never happen, due to previous verifications
                    log.Error($"[HOUSE] {Name}.ConsumeItemsForHousePurchase(): couldn't find {purchaseItem.Guid}!");
                    return false;
                }

                var amount = purchaseItem.Value;
                var stackSize = item.StackSize ?? 1;

                if (amount > stackSize)
                {
                    // this should also never happen, due to previous checks
                    log.Error($"[HOUSE] {Name}.ConsumeItemsForHousePurchase(): {item.Name} ({item.Guid}) amount({amount}) > stackSize({stackSize})!");
                    return false;
                }

                if (!TryConsumeFromInventoryWithNetworking(item, amount))
                {
                    // all of these things should never happen, just being absolutely certain...
                    log.Error($"[HOUSE] {Name}.ConsumeItemsForHousePurchase(): TryConsumeFromInventoryWithNetworking({item.Name} ({item.Guid}), {amount}) failed!");
                    return false;
                }

                // force save partial stack reductions
                item.SaveBiotaToDatabase();
            }
            return true;
        }

        /// <summary>
        /// Verifies the player inventory has required items to purchase house
        /// </summary>
        public bool VerifyPurchase(SlumLord slumlord, List<uint> item_ids)
        {
            // verify house is not already owned
            if (slumlord.HouseOwner != null)
                return false;

            //Console.WriteLine($"{slumlord.Name} ({slumlord.Guid})");
            var logLine = $"[HOUSE] VerifyPurchase:" + Environment.NewLine;
            logLine += $"{slumlord.Name} ({slumlord.Guid})" + Environment.NewLine;
            var buyItems = slumlord.GetBuyItems();
            //Console.WriteLine("Required items:");
            logLine += "Required items:";
            foreach (var buyItem in buyItems)
            {
                var stackStr = buyItem.StackSize != null && buyItem.StackSize > 1 ? buyItem.StackSize.ToString() + " " : "";
                //Console.WriteLine($"{stackStr}{buyItem.Name}");
                logLine += $"{stackStr}{buyItem.Name}" + Environment.NewLine;
            }

            //Console.WriteLine("\nSent items:");
            logLine += Environment.NewLine + "Sent items:" + Environment.NewLine;
            var sentItems = new List<WorldObject>();
            foreach (var item_id in item_ids)
            {
                var item = GetInventoryItem(new ObjectGuid(item_id));
                if (item == null)
                {
                    //Console.WriteLine($"Couldn't find inventory item {item_id:X8}");
                    logLine += $"Couldn't find inventory item {item_id:X8}" + Environment.NewLine;
                    continue;
                }
                var stackStr = item.StackSize != null && item.StackSize > 1 ? item.StackSize.ToString() + " " : "";
                //Console.WriteLine($"{stackStr}{item.Name} ({item.Guid})");
                logLine += $"{stackStr}{item.Name} ({item.Guid})" + Environment.NewLine;

                if (IsTrading && item.IsBeingTradedOrContainsItemBeingTraded(ItemsInTradeWindow))
                {
                    //Console.WriteLine($"{stackStr}{item.Name} ({item.Guid}) is currently being traded, skipping.");
                    logLine += $"{stackStr}{item.Name} ({item.Guid}) is currently being traded, skipping." + Environment.NewLine;
                    continue;
                }
                sentItems.Add(item);
            }
            //Console.WriteLine();
            logLine += Environment.NewLine;
            log.Info(logLine);

            // compare list of input items
            // to required items for purchase
            return HasItems(sentItems, buyItems);
        }

        /// <summary>
        /// Returns TRUE if player inventory contains the required items to purchase house
        /// </summary>
        /// <param name="items">The items required to purchase a house</param>
        public bool HasItems(List<WorldObject> sentItems, List<WorldObject> buyItems)
        {
            // requires: no duplicate individual items in list,
            // ie. items have already been stacked
            foreach (var buyItem in buyItems)
            {
                // special handling for currency
                if (buyItem.Name.Equals("Pyreal"))
                {
                    if (!HasCurrency(sentItems, (uint)(buyItem.StackSize ?? 1)))
                        return false;
                }
                else if (!HasItem(sentItems, buyItem))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns TRUE if player inventory contains an item required to purchase house
        /// </summary>
        /// <param name="item">An item to search for, using stack size as the minimum amount</param>
        public bool HasItem(List<WorldObject> sentItems, WorldObject buyItem)
        {
            var stackStr = buyItem.StackSize != null && buyItem.StackSize > 1 ? buyItem.StackSize.ToString() + " " : "";
            //Console.WriteLine($"Checking for item: {stackStr}{buyItem.Name}");
            log.Info($"[HOUSE] Checking for item: {stackStr}{buyItem.Name}");

            // get all items of this wcid from inventory
            var itemMatches = sentItems.Where(i => i.WeenieClassId == buyItem.WeenieClassId).ToList();
            var totalStack = itemMatches.Select(i => (int)(i.StackSize ?? 1)).Sum();

            if (itemMatches.Count == 0)
            {
                //Console.WriteLine("No matching items found.");
                log.Info($"[HOUSE] No matching items found.");
                return false;
            }
            var required = buyItem.StackSize ?? 1;
            if (totalStack < required)
            {
                //Console.WriteLine($"Found {totalStack} items, requires {required}.");
                log.Info($"[HOUSE] Found {totalStack} items, requires {required}.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if a player has at least some amount of currency in their inventory
        /// </summary>
        /// <param name="amount">The minimum amount of currency required</param>
        /// <param name="useTradeNotes">if TRUE, trade note will also be evaluated</param>
        public bool HasCurrency(List<WorldObject> sentItems, uint amount, bool useTradeNotes = true)
        {
            //Console.WriteLine($"Checking for currency: {amount}");
            log.Info($"[HOUSE] Checking for currency: {amount}");
            var totalCurrency = GetTotalCurrency(sentItems, useTradeNotes);
            return totalCurrency >= amount;
        }

        /// <summary>
        /// Returns the total amount of currency in the player's inventory
        /// </summary>
        /// <param name="useTradeNotes">if TRUE, uses pyreals + trade notes. if FALSE, uses pyreals only</param>
        public uint GetTotalCurrency(List<WorldObject> sentItems, bool useTradeNotes = true)
        {
            var totalPyreals = GetTotalPyreals(sentItems);
            //Console.WriteLine($"Total pyreals: {totalPyreals}");
            log.Info($"[HOUSE] Total pyreals: {totalPyreals}");
            if (!useTradeNotes)
                return totalPyreals;

            var totalTradeNotes = GetTotalTradeNotes(sentItems);
            //Console.WriteLine($"Total trade notes: {totalTradeNotes}");
            log.Info($"[HOUSE] Total trade notes: {totalTradeNotes}");

            return totalPyreals + totalTradeNotes;
        }

        public uint GetTotalPyreals(List<WorldObject> sentItems)
        {
            var coinStacks = sentItems.Where(i => i.WeenieClassId == 273);    // pyreals

            uint totalPyreals = 0;
            foreach (var coinStack in coinStacks)
                totalPyreals += (uint)(coinStack.Value ?? 0);

            return totalPyreals;
        }

        public uint GetTotalTradeNotes(List<WorldObject> sentItems)
        {
            var tradeNotes = sentItems.Where(i => i.WeenieClassName.StartsWith("tradenote")).ToList();

            uint totalValue = 0;
            foreach (var tradeNote in tradeNotes)
                totalValue += (uint)(tradeNote.Value ?? 0);

            return totalValue;
        }

        public IPlayer GetHouseOwner()
        {
            // if this character owns a house, always use that
            if (HouseInstance != null)
                return this;

            // if server is running house_per_char mode (non-default),
            // only use the HouseInstance for the current character
            if (PropertyManager.GetBool("house_per_char").Item)
                return this;

            // else return the account house owner
            var accountHouseOwner = GetAccountHouseOwner();

            return accountHouseOwner;
        }

        public uint? GetHouseInstance()
        {
            return GetHouseOwner()?.HouseInstance;
        }

        public void HandleActionQueryHouse()
        {
            var houseOwner = GetHouseOwner();

            var houseInstance = houseOwner?.HouseInstance;

            // no house owned - send 0x226 HouseStatus?
            if (houseInstance == null)
            {
                Session.Network.EnqueueSend(new GameEventHouseStatus(Session));
                return;
            }

            // house owned - send 0x225 HouseData?
            if (House == null)
                LoadHouse(houseInstance);

            HouseManager.GetHouse(houseInstance.Value, (house) =>
            {
                var houseData = house.GetHouseData(houseOwner);
                Session.Network.EnqueueSend(new GameEventHouseData(Session, houseData));
            });
        }

        public House LoadHouse(uint? houseInstance, bool forceLoad = false)
        {
            if (House != null && !forceLoad)
                return House;

            if (houseInstance == null)
                return House;

            House = House.Load(houseInstance.Value);

            return House;
        }

        public House GetHouse()
        {
            var houseInstance = GetHouseInstance();

            return GetHouse(houseInstance);
        }

        public House GetHouse(uint? houseInstance)
        {
            if (houseInstance == null)
                return null;

            var houseGuid = houseInstance.Value;
            var landblock = (ushort)((houseGuid >> 12) & 0xFFFF);

            var landblockId = new LandblockId((uint)(landblock << 16 | 0xFFFF));
            var isLoaded = LandblockManager.IsLoaded(landblockId);

            if (!isLoaded)
                return House = House.Load(houseGuid);

            var loaded = LandblockManager.GetLandblock(landblockId, false);
            return House = loaded.GetObject(new ObjectGuid(houseGuid)) as House;
        }

        public void HandleActionAddGuest(string guestName)
        {
            //Console.WriteLine($"{Name}.HandleActionAddGuest({guestName})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = GetHouse();
            var guest = PlayerManager.FindByName(guestName, out bool isOnline);

            if (guest == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{guestName} not found", ChatMessageType.Broadcast));
                return;
            }

            if (guest.Equals(this))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("You already have access to your house.", ChatMessageType.Broadcast));
                return;
            }

            var accountPlayers = Player.GetAccountPlayers(Account.AccountId).Select(p => p.Guid).ToList();
            if (Guests.ContainsKey(guest.Guid) || accountPlayers.Contains(guest.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{guest.Name} is already on your guest list.", ChatMessageType.Broadcast));
                return;
            }

            if (Guests.Count == House.MaxGuests)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your guest list has already reached the maximum limit ({House.MaxGuests})", ChatMessageType.Broadcast));
                return;
            }

            house.AddGuest(guest, false);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{guest.Name} has been added to your guest list.", ChatMessageType.Broadcast));

            // notify online guest for addition
            if (isOnline)
            {
                var onlineGuest = PlayerManager.GetOnlinePlayer(guest.Guid);
                onlineGuest.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has added you to their house guest list.", ChatMessageType.Broadcast));
            }
        }

        public void HandleActionRemoveGuest(string guestName, bool sendMsgToOwner = true)
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveGuest({guestName})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = GetHouse();
            var guest = PlayerManager.FindByName(guestName, out bool isOnline);

            if (guest == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{guestName} not found", ChatMessageType.Broadcast));
                return;
            }

            if (guest.Equals(this))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("You have permanent access to your house.", ChatMessageType.Broadcast));
                return;
            }

            if (!Guests.ContainsKey(guest.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{guest.Name} is not on your guest list.", ChatMessageType.Broadcast));
                return;
            }

            house.RemoveGuest(guest);

            if (sendMsgToOwner)
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{guest.Name} removed from your guest list.", ChatMessageType.Broadcast));

            // notify online guest removed
            if (isOnline)
            {
                var onlineGuest = PlayerManager.GetOnlinePlayer(guest.Guid);
                onlineGuest.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has removed you from their house guest list.", ChatMessageType.Broadcast));

                // if guest access is removed while player is in house,
                // they will be stuck in restriction space
                if (house.OnProperty(onlineGuest))
                    HandleActionBoot(onlineGuest.Name);
            }
        }

        public void HandleActionRemoveAllGuests()
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveAllGuests()");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = GetHouse();

            if (Guests.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guest list is empty.", ChatMessageType.Broadcast));
                return;
            }

            foreach (var guid in Guests.Keys.ToList())
            {
                var guest = PlayerManager.FindByGuid(guid);
                HandleActionRemoveGuest(guest.Name, false);
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have removed all the guests from your house.", ChatMessageType.Broadcast));
        }

        public void HandleActionGuestList()
        {
            //Console.WriteLine($"{Name}.HandleActionGuestList()");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = GetHouse();

            if (house == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            Session.Network.EnqueueSend(new GameEventUpdateHAR(Session, house));
            return;
        }

        public void HandleActionSetOpenStatus(bool openStatus)
        {
            //Console.WriteLine($"{Name}.HandleActionSetOpenStatus({openStatus})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = GetHouse();

            if (openStatus == house.OpenStatus)
            {
                if (openStatus)
                    Session.Network.EnqueueSend(new GameMessageSystemChat("You already have an open house.", ChatMessageType.Broadcast));
                else
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is already closed to the public.", ChatMessageType.Broadcast));

                return;
            }

            house.OpenStatus = openStatus;
            house.Biota.SetProperty(PropertyBool.Open, house.OpenStatus, house.BiotaDatabaseLock, out _);
            house.ChangesDetected = true;
            house.UpdateRestrictionDB();

            if (openStatus)
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is now open to the public.", ChatMessageType.Broadcast));
            else
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is now closed to the public.", ChatMessageType.Broadcast));

                // boot anyone not on the guest list,
                // else they will be stuck in restricted space
                HandleActionBootAll(false);
            }

            if (house.CurrentLandblock == null)
                house.SaveBiotaToDatabase();
        }

        public void HandleActionSetHooksVisible(bool visible)
        {
            //Console.WriteLine($"{Name}.HandleActionSetHooksVisible({visible})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var visibleStr = visible ? "visible" : "invisible";
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Your hooks are set to {visibleStr}.", ChatMessageType.Broadcast));

            var house = GetHouse();

            if (visible == (house.HouseHooksVisible ?? true)) return;

            house.HouseHooksVisible = visible;

            foreach (var hook in house.Hooks.Where(i => i.Inventory.Count == 0))
            {
                hook.UpdateHookVisibility();
            }

            // if house has dungeon, repeat this process
            if (house.HasDungeon)
            {
                var dungeonHouse = house.GetDungeonHouse();
                if (dungeonHouse == null) return;

                dungeonHouse.HouseHooksVisible = visible;

                foreach (var hook in dungeonHouse.Hooks.Where(i => i.Inventory.Count == 0))
                {
                    hook.UpdateHookVisibility();
                }

                if (dungeonHouse.CurrentLandblock == null)
                    dungeonHouse.SaveBiotaToDatabase();
            }

            if (house.CurrentLandblock == null)
                house.SaveBiotaToDatabase();
        }

        public void HandleActionModifyStorage(string guestName, bool hasPermission, bool sendMsgToOwner = true)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyStorage({guestName}, {hasPermission})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = GetHouse();
            var storage = PlayerManager.FindByName(guestName, out bool isOnline);

            if (storage == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{guestName} not found", ChatMessageType.Broadcast));
                return;
            }

            if (storage.Equals(this))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("You have permanent access to your house storage.", ChatMessageType.Broadcast));
                return;
            }

            var existing = Guests.TryGetValue(storage.Guid, out var storageAccess);
            if (hasPermission)
            {
                if (!existing)
                {
                    if (Guests.Count == House.MaxGuests)
                    {
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"Your guest list has already reached the maximum limit ({House.MaxGuests})", ChatMessageType.Broadcast));
                        return;
                    }
                    house.AddGuest(storage, true);
                }
                else
                {
                    if (storageAccess)
                    {
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"{storage.Name} already has access to your home's storage.", ChatMessageType.Broadcast));
                        return;
                    }
                    house.ModifyGuest(storage, true);
                }

                var andStr = !existing ? "and " : "";
                if (sendMsgToOwner)
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"You have granted {storage.Name} access to your home's storage.  This is denoted by  the asterisk next to their name in the guest list.", ChatMessageType.Broadcast)); // spacing from PCAP

                // notify online storage guest added
                if (isOnline)
                {
                    var onlineGuest = PlayerManager.GetOnlinePlayer(storage.Guid);
                    onlineGuest.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has granted you access to their house {andStr}storage.", ChatMessageType.Broadcast));
                }
            }
            else
            {
                if (!existing || !storageAccess)
                {
                    var storageStr = existing ? " storage" : "";
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{storage.Name} doesn't have access to your house{storageStr}.", ChatMessageType.Broadcast));
                    return;
                }

                house.ModifyGuest(storage, false);

                Session.Network.EnqueueSend(new GameMessageSystemChat($"{storage.Name} no longer has access to your house storage.", ChatMessageType.Broadcast));

                // notify online storage guest added
                if (isOnline)
                {
                    var onlineGuest = PlayerManager.GetOnlinePlayer(storage.Guid);
                    onlineGuest.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has revoked access to their house storage.", ChatMessageType.Broadcast));

                    // if they are in house, and have storage opened?
                }
            }
        }

        public void HandleActionAllStorage()
        {
            //Console.WriteLine($"{Name}.HandleActionAllStorage()");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = GetHouse();

            if (Guests.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guest list is empty.", ChatMessageType.Broadcast));
                return;
            }

            var noStorage = Guests.Where(g => !g.Value).ToList();

            if (noStorage.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guests already have access to your storage.", ChatMessageType.Broadcast));
                return;
            }

            foreach (var guid in noStorage)
            {
                var guest = PlayerManager.FindByGuid(guid.Key);
                HandleActionModifyStorage(guest.Name, true, false);
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You grant item storage permission to all your guests", ChatMessageType.Broadcast));
        }

        public void HandleActionRemoveAllStorage()
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveAllStorage()");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = GetHouse();

            if (Guests.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guest list is empty.", ChatMessageType.Broadcast));
                return;
            }

            var storage = Guests.Where(g => g.Value).ToList();

            if (storage.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guests don't have storage access.", ChatMessageType.Broadcast));
                return;
            }

            foreach (var guid in storage)
            {
                var guest = PlayerManager.FindByGuid(guid.Key);
                HandleActionModifyStorage(guest.Name, false, false);
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You remove item storage permission from all your guests", ChatMessageType.Broadcast));
        }

        public void HandleActionBoot(string playerName, bool allegianceHouse = false)
        {
            //Console.WriteLine($"{Name}.HandleActionBoot({playerName})");
            if (House == null && !allegianceHouse)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var house = allegianceHouse ? Allegiance.GetHouse() : GetHouse();
            var player = PlayerManager.GetOnlinePlayer(playerName);

            if (player == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} is not online.", ChatMessageType.Broadcast));
                return;
            }

            // is this player in the house landcell?
            var owner = allegianceHouse ? "allegiance" : "your";
            if (!house.OnProperty(player))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is not on {owner} property.", ChatMessageType.Broadcast));
                return;
            }

            // play script?
            player.Teleport(house.BootSpot.Location);

            owner = allegianceHouse ? "the allegiance" : "your";
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Booted {player.Name} from {owner} house.", ChatMessageType.Broadcast));

            owner = allegianceHouse ? "the allegiance" : "their";
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has booted you from {owner} house.", ChatMessageType.Broadcast));
        }

        public void HandleActionBootAll(bool guests = true)
        {
            //Console.WriteLine($"{Name}.HandleActionBootAll()");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            // since it can be an open house, the guest list wouldn't be enough here?
            var house = GetHouse();

            var booted = house.BootAll(this, guests);

            if (guests && booted == 0)
            {
                var elseStr = house.OnProperty(this) ? "else " : "";
                Session.Network.EnqueueSend(new GameMessageSystemChat($"There is no one {elseStr}on your property.", ChatMessageType.Broadcast));
            }
        }

        /// <summary>
        /// Called when player is exiting portal space
        /// </summary>
        /// <returns>TRUE if player was booted from house property they don't have access to, upon exiting portal space</returns>
        public bool CheckHouse()
        {
            if (CurrentLandblock == null || IgnoreHouseBarriers)
                return false;

            foreach (var house in CurrentLandblock.Houses)
            {
                var rootHouse = house.RootHouse;

                if (!rootHouse.OnProperty(this))
                    continue;

                if (rootHouse.HouseOwner != null && !rootHouse.HasPermission(this, false))
                {
                    if (!rootHouse.IsOpen || (rootHouse.HouseType != HouseType.Apartment && CurrentLandblock.HasDungeon))
                    {
                        Teleport(rootHouse.BootSpot.Location);
                        return true;
                    }
                }

                //if (rootHouse.HouseOwner == null && rootHouse.HouseType != HouseType.Apartment && CurrentLandblock.HasDungeon)
                //{
                //    Teleport(rootHouse.BootSpot.Location);
                //    return true;
                //}
            }
            return false;
        }

        /// <summary>
        /// Handles the @hslist [housetype] command
        /// </summary>
        public void HandleActionListAvailable(HouseType houseType)
        {
            //Console.WriteLine($"{Name}.HandleActionListAvailable({houseType})");

            var locations = HouseList.GetAvailableLocations(houseType);
            var unique = locations.Distinct().ToList();

            Session.Network.EnqueueSend(new GameEventHouseAvailableHouses(Session, houseType, unique, locations.Count));
        }


        //=========================
        // allegiance permissions
        //=========================

        public void HandleActionModifyAllegianceGuestPermission(bool add)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyAllegianceGuestPermission({add})");
            var houseInstance = GetHouseInstance();

            if (houseInstance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            // only the character who directly owns the house can use /house guest add_allegiance / remove_allegiance
            if (HouseInstance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.OnlyHouseOwnerCanUseCommand));
                return;
            }

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            var house = GetHouse();

            if (add)
            {
                if (house.MonarchId != null)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"The monarchy already has access to your dwelling.", ChatMessageType.Broadcast));
                    return;
                }

                if (Guests.Count == House.MaxGuests)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Your guest list has already reached the maximum limit ({House.MaxGuests})", ChatMessageType.Broadcast));
                    return;
                }

                house.MonarchId = Allegiance.MonarchId;

                if (!Guests.ContainsKey(Allegiance.Monarch.PlayerGuid))
                    house.AddGuest(Allegiance.Monarch.Player, false);
                else
                    house.ModifyGuest(Allegiance.Monarch.Player, false);    // handle case: the monarch already has guest/storage access already, now adding allegiance

                Session.Network.EnqueueSend(new GameMessageSystemChat($"You have granted your monarchy access to your dwelling.", ChatMessageType.Broadcast));
            }
            else
            {
                if (house.MonarchId == null)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"The monarchy did not have access to your dwelling.", ChatMessageType.Broadcast));
                    return;
                }

                house.MonarchId = null;

                house.RemoveGuest(Allegiance.Monarch.Player);

                HandleActionBootAll(false);     // boot anyone who doesn't have guest access

                Session.Network.EnqueueSend(new GameMessageSystemChat($"You have revoked access to your dwelling to your monarchy.", ChatMessageType.Broadcast));
            }
        }

        public void HandleActionModifyAllegianceStoragePermission(bool add)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyAllegianceStoragePermission({add})");
            var houseInstance = GetHouseInstance();

            if (houseInstance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            // only the character who directly owns the house can use /house storage add_allegiance / remove_allegiance
            if (HouseInstance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.OnlyHouseOwnerCanUseCommand));
                return;
            }

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            var house = GetHouse();

            if (add)
            {
                if (house.MonarchId != null && Guests.TryGetValue(new ObjectGuid(house.MonarchId.Value), out bool storage) && storage)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"The monarchy already has storage access in your dwelling.", ChatMessageType.Broadcast));
                    return;
                }

                if (house.MonarchId == null && Guests.Count == House.MaxGuests)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Your guest list has already reached the maximum limit ({House.MaxGuests})", ChatMessageType.Broadcast));
                    return;
                }

                house.MonarchId = Allegiance.MonarchId;

                if (!Guests.ContainsKey(Allegiance.Monarch.PlayerGuid))
                    house.AddGuest(Allegiance.Monarch.Player, true);
                else
                    house.ModifyGuest(Allegiance.Monarch.Player, true);     // handle case: the monarch already has guest/storage access already, now adding allegiance

                Session.Network.EnqueueSend(new GameMessageSystemChat($"You have granted your monarchy access to your storage.", ChatMessageType.Broadcast));
            }
            else
            {
                if (house.MonarchId == null || Guests.TryGetValue(new ObjectGuid(house.MonarchId.Value), out bool storage) && !storage)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"The monarchy did not have storage access to your dwelling.", ChatMessageType.Broadcast));
                    return;
                }

                // downgrade to guest access
                house.ModifyGuest(Allegiance.Monarch.Player, false);

                Session.Network.EnqueueSend(new GameMessageSystemChat($"You have revoked storage access to your monarchy.", ChatMessageType.Broadcast));
            }
        }

        //=============================
        // /allegiance house commands
        //=============================

        public void HandleActionDoAllegianceHouseAction(AllegianceHouseAction action)
        {
            //Console.WriteLine($"{Name}.DoAllegianceHouseAction({action})");

            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Castellan)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            var allegianceHouse = Allegiance.GetHouse();

            if (allegianceHouse == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YourMonarchDoesNotOwnAMansionOrVilla));
                return;
            }

            if (allegianceHouse.HouseType < HouseType.Villa)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YourMonarchsHouseIsNotAMansionOrVilla));
                return;
            }

            if (action == AllegianceHouseAction.Help)
            {
                var help = "Note: You may substitute a forward slash(/) for the at symbol(@).\n" +
                           "@allegiance house guest open - Adds your allegiance to the allegiance house guest list.\n" +
                           "@allegiance house guest close - Removes your allegiance from the allegiance house guest list.\n" +
                           "@allegiance house storage open - Adds your allegiance to the allegiance house storage list.\n" +
                           "@allegiance house storage close - Removes your allegiance from the allegiance house storage list.";

                var status = "";
                if (allegianceHouse.MonarchId == null)
                    status = "\nYour monarchy currently does not have guest or storage access to allegiance housing.";
                else
                {
                    allegianceHouse.Guests.TryGetValue(Allegiance.Monarch.PlayerGuid, out bool storage);
                    if (!storage)
                        status = "\nYour monarchy currently has guest access to allegiance housing.";
                    else
                        status = "\nYour monarchy currently has guest and storage access to allegiance housing.";
                }

                Session.Network.EnqueueSend(new GameMessageSystemChat(help + status, ChatMessageType.Broadcast));
                return;
            }

            switch (action)
            {
                case AllegianceHouseAction.GuestOpen:
                    HandleActionDoAllegianceHouseAction_GuestOpen(allegianceHouse);
                    break;

                case AllegianceHouseAction.GuestClose:
                    HandleActionDoAllegianceHouseAction_GuestClose(allegianceHouse);
                    break;

                case AllegianceHouseAction.StorageOpen:
                    HandleActionDoAllegianceHouseAction_StorageOpen(allegianceHouse);
                    break;

                case AllegianceHouseAction.StorageClose:
                    HandleActionDoAllegianceHouseAction_StorageClose(allegianceHouse);
                    break;
            }

            if (allegianceHouse.CurrentLandblock == null)
                allegianceHouse.SaveBiotaToDatabase();
        }

        public void HandleActionDoAllegianceHouseAction_GuestOpen(House allegianceHouse)
        {
            if (allegianceHouse.MonarchId != null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The monarchy already has access to the allegiance dwelling.", ChatMessageType.Broadcast));
                return;
            }

            if (allegianceHouse.Guests.Count == House.MaxGuests)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The allegiance house guest list has already reached the maximum limit ({House.MaxGuests})", ChatMessageType.Broadcast));
                return;
            }

            allegianceHouse.MonarchId = Allegiance.MonarchId;

            // AddHouseGuest
            allegianceHouse.AddGuest(Allegiance.Monarch.Player, false);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have granted your monarchy access to the allegiance dwelling.", ChatMessageType.Broadcast));
        }

        public void HandleActionDoAllegianceHouseAction_GuestClose(House allegianceHouse)
        {
            if (allegianceHouse.MonarchId == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The monarchy already does not have access to the allegiance dwelling.", ChatMessageType.Broadcast));
                return;
            }

            allegianceHouse.MonarchId = null;

            // RemoveHouseGuest
            allegianceHouse.RemoveGuest(Allegiance.Monarch.Player);

            var booted = allegianceHouse.BootAll(this, false, true);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have revoked allegiance access to the allegiance dwelling.", ChatMessageType.Broadcast));
        }

        public void HandleActionDoAllegianceHouseAction_StorageOpen(House allegianceHouse)
        {
            if (allegianceHouse.MonarchId != null && allegianceHouse.Guests.TryGetValue(new ObjectGuid(allegianceHouse.MonarchId.Value), out bool storage) && storage)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The monarchy already has storage access in the allegiance dwelling.", ChatMessageType.Broadcast));
                return;
            }

            if (allegianceHouse.MonarchId == null && allegianceHouse.Guests.Count == House.MaxGuests)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your allegiance house guest list has already reached the maximum limit ({House.MaxGuests})", ChatMessageType.Broadcast));
                return;
            }

            allegianceHouse.MonarchId = Allegiance.MonarchId;

            // AddHouseGuest
            if (!allegianceHouse.Guests.ContainsKey(Allegiance.Monarch.PlayerGuid))
            {
                allegianceHouse.AddGuest(Allegiance.Monarch.Player, true);
            }
            else
            {
                // handle guest -> storage access upgrade
                allegianceHouse.ModifyGuest(Allegiance.Monarch.Player, true);
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have granted your monarchy access to allegiance storage.", ChatMessageType.Broadcast));
        }

        public void HandleActionDoAllegianceHouseAction_StorageClose(House allegianceHouse)
        {
            if (allegianceHouse.MonarchId == null || allegianceHouse.Guests.TryGetValue(new ObjectGuid(allegianceHouse.MonarchId.Value), out bool storage) && !storage)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The monarchy already does not have storage access to the allegiance dwelling.", ChatMessageType.Broadcast));
                return;
            }

            // ModifyHouseGuest - downgrade to guest access
            allegianceHouse.ModifyGuest(Allegiance.Monarch.Player, false);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have revoked your monarchy's access to the allegiance housing storage.", ChatMessageType.Broadcast));
        }

        public static List<IPlayer> GetAccountPlayers(uint accountID)
        {
            return PlayerManager.GetAllPlayers().Where(i => i.Account != null && i.Account.AccountId == accountID).ToList();
        }

        public IPlayer GetAccountHouseOwner()
        {
            var accountPlayers = GetAccountPlayers(Account.AccountId);

            var accountHouseOwners = accountPlayers.Where(i => i.HouseInstance != null);

            return accountHouseOwners.OrderBy(i => i.HousePurchaseTimestamp).FirstOrDefault();
        }

        public House GetAccountHouse()
        {
            if (HouseInstance != null)
                return GetHouse(HouseInstance);

            var accountHouseOwner = GetAccountHouseOwner();

            if (accountHouseOwner != null)
                return null;

            //Console.WriteLine($"Account House Owner: {accountHouseOwner.Name}");

            return GetHouse(accountHouseOwner);
        }

        public House GetHouse(IPlayer player)
        {
            if (player?.HouseInstance == null)
                return null;

            // is landblock loaded?
            var houseGuid = player.HouseInstance.Value;
            var landblock = (ushort)((houseGuid >> 12) & 0xFFFF);

            var landblockId = new LandblockId((uint)(landblock << 16 | 0xFFFF));
            var isLoaded = LandblockManager.IsLoaded(landblockId);

            if (isLoaded)
            {
                var loaded = LandblockManager.GetLandblock(landblockId, false);
                return loaded.GetObject(new ObjectGuid(houseGuid)) as House;
            }

            // load an offline copy
            return House.Load(houseGuid);
        }

        public bool IsMultiHouseOwner(bool showMsg = true)
        {
            var characterHouses = HouseManager.GetCharacterHouses(Guid.Full);
            var accountHouses = HouseManager.GetAccountHouses(Account.AccountId);

            //if (showMsg)
                //Session.Network.EnqueueSend(new GameMessageSystemChat($"AccountHouses: {accountHouses.Count}, CharacterHouses: {characterHouses.Count}", ChatMessageType.Broadcast));

            if (PropertyManager.GetBool("house_per_char").Item)
            {
                // 1 house per character
                if (characterHouses.Count > 1 && showMsg)
                    ShowMultiHouseWarning(characterHouses, "character");

                return characterHouses.Count > 1;
            }
            else
            {
                // 1 house per account (retail default)
                if (accountHouses.Count > 1 && showMsg)
                    ShowMultiHouseWarning(accountHouses, "account");

                return accountHouses.Count > 1;
            }
        }

        public List<House> GetMultiHouses()
        {
            if (PropertyManager.GetBool("house_per_char").Item)
                return HouseManager.GetCharacterHouses(Guid.Full);
            else
                return HouseManager.GetAccountHouses(Account.AccountId);
        }

        public void ShowMultiHouseWarning(List<House> houses, string type)
        {
            // this is a dangerous situation, and we want to clean it up asap
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Warning! You currently own {houses.Count} different houses on this {type}.", ChatMessageType.Broadcast));
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Each {type} is only allowed to own 1 house, so you will have to choose which house you want to keep.", ChatMessageType.Broadcast));
            Session.Network.EnqueueSend(new GameMessageSystemChat($"You currently own houses at:", ChatMessageType.Broadcast));

            for (var i = 0; i < houses.Count; i++)
            {
                var house = houses[i];
                var slumlord = house.SlumLord;
                if (slumlord == null)
                {
                    log.Error($"{Name}.IsMultiHouseOwner(): {house.Guid} slumlord is null!");
                    continue;
                }
                var coords = HouseManager.GetCoords(slumlord.Location);
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{i + 1}. {coords}", ChatMessageType.Broadcast));
            }
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Please choose the house you want to keep with /house-select # , where # is 1-{houses.Count}", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// When house_15day_account is true (retail server default),
        /// new characters logging in on accounts less than 15 days old
        /// have their HousePurchaseTimestamp set to 15 days before account creation.
        /// 
        /// This is for the House panel to show the correct date the character may purchase a house,
        /// which the client automatically calculates as 30 days after HousePurchaseTimestamp
        /// </summary>
        private int FifteenDaysBeforeAccountCreation => (int)Time.GetUnixTime(Account.CreateTime.AddDays(-15));

        /// <summary>
        /// Munges the HousePurchaseTimestamp for new accounts for correct display on House panel
        /// </summary>
        private void ManageAccount15Days_HousePurchaseTimestamp()
        {
            // http://acpedia.org/wiki/Housing_FAQ#Purchase_timer

            if (HouseRentTimestamp != null) return;

            if (PropertyManager.GetBool("house_15day_account").Item && !Account15Days)
            {
                // this is set so the next purchase time displays properly on house tab
                HousePurchaseTimestamp = FifteenDaysBeforeAccountCreation;
            }
            else if (HousePurchaseTimestamp == FifteenDaysBeforeAccountCreation)
            {
                // account is now 15+ days old and still has not purchased a house, remove unneeded HousePurchaseTimestamp
                // also if server admin sets house_15day_account to false, this corrects the next purchase time on the House panel 
                HousePurchaseTimestamp = null;
            }
        }
    }
}
