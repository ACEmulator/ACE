using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public House House;

        /// <summary>
        /// Called when player clicks the 'Buy house' button,
        /// after adding the items required
        /// </summary>
        public void HandleActionBuyHouse(uint slumlord_id, List<uint> item_ids)
        {
            Console.WriteLine("\nHandleActionBuyHouse()");

            var slumlord = (SlumLord)CurrentLandblock.GetObject(slumlord_id);
            if (slumlord == null)
            {
                Console.WriteLine("Couldn't find slumlord!");
                return;
            }

            var verified = VerifyPurchase(slumlord, item_ids);
            if (!verified)
            {
                Console.WriteLine($"{Name} tried to purchase house {slumlord.Guid} without the required items!");
                return;
            }

            Console.WriteLine("\nInventory check passed!");

            // TODO: consume items for house purchase
            ConsumeItemsForPurchase(item_ids);

            SetHouseOwner(slumlord);
        }

        /// <summary>
        /// Sets this player as the owner of a house
        /// </summary>
        public void SetHouseOwner(SlumLord slumlord)
        {
            var house = slumlord.House;

            Console.WriteLine($"Setting {Name} as owner of {house.Name}");

            // set player properties
            HouseId = house.HouseId;
            HouseInstance = house.Guid.Full;
            HousePurchaseTimestamp = (int)Time.GetUnixTime();

            // set house properties
            house.HouseOwner = Guid.Full;
            house.SaveBiotaToDatabase();

            // notify client w/ HouseID

            // set house data
            //var house = new HouseData();
            HandleActionQueryHouse();
        }

        /// <summary>
        /// Removes verified items from inventory for house purchase
        /// </summary>
        public void ConsumeItemsForPurchase(List<uint> item_ids)
        {
            // TODO: return change?
        }

        /// <summary>
        /// Verifies the player inventory has required items to purchase house
        /// </summary>
        public bool VerifyPurchase(SlumLord slumlord, List<uint> item_ids)
        {
            Console.WriteLine($"{slumlord.Name} ({slumlord.Guid})");
            var buyItems = slumlord.GetBuyItems();
            Console.WriteLine("Required items:");
            foreach (var buyItem in buyItems)
            {
                var stackStr = buyItem.StackSize != null && buyItem.StackSize > 1 ? buyItem.StackSize.ToString() + " " : "";
                Console.WriteLine($"{stackStr}{buyItem.Name}");
            }

            Console.WriteLine("\nSent items:");
            var sentItems = new List<WorldObject>();
            foreach (var item_id in item_ids)
            {
                var item = GetInventoryItem(new ObjectGuid(item_id));
                if (item == null)
                {
                    Console.WriteLine($"Couldn't find inventory item {item_id:X8}");
                    continue;
                }
                var stackStr = item.StackSize != null && item.StackSize > 1 ? item.StackSize.ToString() + " " : "";
                Console.WriteLine($"{stackStr}{item.Name} ({item.Guid})");
                sentItems.Add(item);
            }
            Console.WriteLine();

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
            var stackStr = buyItem.StackSize != null && buyItem.StackSize > 1 ? buyItem.StackSize.ToString() + " ": "";
            Console.WriteLine($"Checking for item: {stackStr}{buyItem.Name}");

            // get all items of this wcid from inventory
            var itemMatches = sentItems.Where(i => i.WeenieClassId == buyItem.WeenieClassId).ToList();
            var totalStack = itemMatches.Select(i => (int)(i.StackSize ?? 1)).Sum();

            if (itemMatches.Count == 0)
            {
                Console.WriteLine("No matching items found.");
                return false;
            }
            var required = buyItem.StackSize ?? 1;
            if (totalStack < required)
            {
                Console.WriteLine($"Found {totalStack} items, requires {required}.");
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
            Console.WriteLine($"Checking for currency: {amount}");
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
            Console.WriteLine($"Total pyreals: {totalPyreals}");
            if (!useTradeNotes)
                return totalPyreals;

            var totalTradeNotes = GetTotalTradeNotes(sentItems);
            Console.WriteLine($"Total trade notes: {totalTradeNotes}");

            return totalPyreals + totalTradeNotes;
        }

        public uint GetTotalPyreals(List<WorldObject> sentItems)
        {
            var coinStacks = sentItems.Where(i => i.WeenieClassId == 273);    // pyreals

            uint totalPyreals = 0;
            foreach (var coinStack in coinStacks)
                totalPyreals += (uint)coinStack.CoinValue;

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

        public void HandleActionQueryHouse()
        {
            // no house owned - send 0x226 HouseStatus?
            if (HouseInstance == null)
            {
                Session.Network.EnqueueSend(new GameEventHouseStatus(Session));
                return;
            }

            // house owned - send 0x225 HouseData?
            var house = LoadHouse();
            if (house == null)
            {
                Session.Network.EnqueueSend(new GameEventHouseStatus(Session));
                return;
            }

            var houseData = house.GetHouseData(this);
            Session.Network.EnqueueSend(new GameEventHouseData(Session, houseData));
        }

        public House LoadHouse(bool forceLoad = false)
        {
            if (House != null && !forceLoad)
                return House;

            var houseGuid = HouseInstance.Value;
            var landblock = (ushort)((houseGuid >> 12) & 0xFFFF);

            var biota = DatabaseManager.Shard.GetBiota(houseGuid);
            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblock);

            if (instances == null || instances.Count == 0 || biota == null)
            {
                Console.WriteLine($"{Name} sent HouseInstance={HouseInstance:X8}, instances={instances}, biota={biota}");
                return null;
            }

            var objects = WorldObjectFactory.CreateNewWorldObjects(instances, new List<Biota>() { biota }, houseGuid);
            if (objects.Count == 0)
            {
                Console.WriteLine($"{Name} sent HouseInstance={HouseInstance:X8}, found instances and biota, but object count=0");
                return null;
            }

            var house = objects[0] as House;
            if (house == null)
            {
                Console.WriteLine($"{Name} sent HouseInstance={HouseInstance:X8}, found instances and biota, but type {objects[0].WeenieType}");
                return null;
            }

            house.ActivateLinks(instances, new List<Biota>() { biota });
            House = house;
            return house;
        }
    }
}
