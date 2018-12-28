using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
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

        public Dictionary<IPlayer, bool> Guests => House.Guests;

        /// <summary>
        /// Called when player clicks the 'Buy house' button,
        /// after adding the items required
        /// </summary>
        public void HandleActionBuyHouse(uint slumlord_id, List<uint> item_ids)
        {
            Console.WriteLine($"\n{Name}.HandleActionBuyHouse()");

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

        public void HandleActionAbandonHouse()
        {
            Console.WriteLine($"\n{Name}.HandleActionAbandonHouse()");

            var house = GetHouse();
            if (house != null)
            {
                house.HouseOwner = null;
                house.SaveBiotaToDatabase();

                // relink
                house.UpdateLinks();

                // player slumlord 'off' animation
                var slumlord = house.SlumLord;
                slumlord.EnqueueBroadcastMotion(new Motion(MotionStance.Invalid, MotionCommand.Off));

                // reset slumlord name
                var weenie = DatabaseManager.World.GetCachedWeenie(slumlord.WeenieClassId);
                var wo = WorldObjectFactory.CreateWorldObject(weenie, new ObjectGuid(0));
                slumlord.Name = wo.Name;

                slumlord.EnqueueBroadcast(new GameMessagePublicUpdatePropertyString(slumlord, PropertyString.Name, wo.Name));
            }

            HouseId = null;
            HouseInstance = null;
            HousePurchaseTimestamp = null;

            House = null;

            // send text message
            Session.Network.EnqueueSend(new GameMessageSystemChat("You abandon your house!", ChatMessageType.Broadcast));

            HandleActionQueryHouse();
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

            // relink
            house.UpdateLinks();

            // notify client w/ HouseID
            Session.Network.EnqueueSend(new GameMessageSystemChat("Congratulations!  You now own this dwelling.", ChatMessageType.Broadcast));

            // player slumlord 'on' animation
            slumlord.EnqueueBroadcastMotion(new Motion(MotionStance.Invalid, MotionCommand.On));

            // set house name
            slumlord.Name = $"{Name}'s {slumlord.Name}";
            slumlord.EnqueueBroadcast(new GameMessagePublicUpdatePropertyString(slumlord, PropertyString.Name, slumlord.Name));

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

        public House GetHouse()
        {
            if (HouseInstance == null)
                return House;

            var houseGuid = HouseInstance.Value;
            var landblock = (ushort)((houseGuid >> 12) & 0xFFFF);

            var landblockId = new LandblockId((uint)(landblock << 16 | 0xFFFF));
            var isLoaded = LandblockManager.IsLoaded(landblockId);

            if (!isLoaded)
                return House;

            var loaded = LandblockManager.GetLandblock(landblockId, false);
            return loaded.GetObject(new ObjectGuid(houseGuid)) as House;
        }

        public void AddHouseGuest(IPlayer guest, bool storage)
        {
            var house = GetHouse();
            house.AddGuest(guest, storage);

            Guests.Add(guest, storage);
            UpdateRestrictionDB();
        }

        public void ModifyHouseGuest(IPlayer guest, bool storage)
        {
            var house = GetHouse();
            house.UpdateGuest(guest, storage);

            Guests[guest] = storage;
            UpdateRestrictionDB();
        }

        public void RemoveHouseGuest(IPlayer guest)
        {
            var house = GetHouse();
            house.RemoveGuest(guest);

            Guests.Remove(guest);
            UpdateRestrictionDB();
        }

        public void Sync(House house)
        {
            house.Guests = House.Guests;
            house.OpenStatus = House.OpenStatus;
        }


        public void HandleActionAddGuest(string guestName)
        {
            //Console.WriteLine($"{Name}.HandleActionAddGuest({guestName})");

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

            if (Guests.ContainsKey(guest))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{guest.Name} is already on your guest list.", ChatMessageType.Broadcast));
                return;
            }

            if (Guests.Count == House.MaxGuests)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your guest list has already reached the maximum limit ({House.MaxGuests})", ChatMessageType.Broadcast));
                return;
            }

            AddHouseGuest(guest, false);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{guest.Name} added to your guest list.", ChatMessageType.Broadcast));

            // notify online guest for addition
            if (isOnline)
            {
                var onlineGuest = PlayerManager.GetOnlinePlayer(guest.Guid);
                onlineGuest.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has added you to their house guest list.", ChatMessageType.Broadcast));
            }
        }

        public void HandleActionRemoveGuest(string guestName)
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveGuest({guestName})");

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

            if (!Guests.ContainsKey(guest))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{guest.Name} isn't on your guest list.", ChatMessageType.Broadcast));
                return;
            }

            RemoveHouseGuest(guest);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{guest.Name} removed from your guest list.", ChatMessageType.Broadcast));

            // notify online guest removed
            if (isOnline)
            {
                var onlineGuest = PlayerManager.GetOnlinePlayer(guest.Guid);
                onlineGuest.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has removed you from their house guest list.", ChatMessageType.Broadcast));

                // if guest access is removed while player is in house,
                // they will be stuck in restriction space
                if (onlineGuest.Location.GetOutdoorCell() == House.Location.GetOutdoorCell())
                    HandleActionBoot(onlineGuest.Name);
            }
        }

        public void HandleActionRemoveAllGuests()
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveAllGuests()");

            if (Guests.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guest list is empty.", ChatMessageType.Broadcast));
                return;
            }

            foreach (var guest in Guests.Keys)
                HandleActionRemoveGuest(guest.Name);
        }

        public void HandleActionGuestList()
        {
            //Console.WriteLine($"{Name}.HandleActionGuestList()");

            if (Guests.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guest list is empty.", ChatMessageType.Broadcast));
                return;
            }

            var sb = new StringBuilder($"{Name}'s {House.SlumLord.Name} guest list:\n");
            foreach (var guest in Guests)
            {
                var storage = guest.Value ? "* " : "";
                sb.Append(storage + guest.Key.Name + "\n");
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat(sb.ToString(), ChatMessageType.Broadcast));
            return;
        }

        public void HandleActionSetOpenStatus(bool openStatus)
        {
            //Console.WriteLine($"{Name}.HandleActionSetOpenStatus({openStatus})");

            if (openStatus == House.OpenStatus)
            {
                if (openStatus)
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is already open.", ChatMessageType.Broadcast));
                else
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is already restricted to guests only.", ChatMessageType.Broadcast));

                return;
            }

            House.OpenStatus = openStatus;

            UpdateRestrictionDB();

            if (openStatus)
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is open to everyone now.", ChatMessageType.Broadcast));
            else
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is restricted to guests only.", ChatMessageType.Broadcast));

                // boot anyone not on the guest list,
                // else they will be stuck in restricted space
                HandleActionBootAll(false);
            }
        }

        public void HandleActionSetHooksVisible(bool visible)
        {
            //Console.WriteLine($"{Name}.HandleActionSetHooksVisible({visible})");

            var visibleStr = visible ? "visible" : "invisible";
            Session.Network.EnqueueSend(new GameMessageSystemChat($"Your hooks are set to {visibleStr}.", ChatMessageType.Broadcast));

            var house = GetHouse();

            if (visible == (house.HouseHooksVisible ?? true)) return;

            house.HouseHooksVisible = visible;

            var state = PhysicsState.Ethereal | PhysicsState.IgnoreCollisions;
            if (!visible) state |= PhysicsState.NoDraw;

            foreach (var hook in house.Hooks.Where(i => i.Inventory.Count == 0))
            {
                var setState = new GameMessageSetState(hook, state);
                var update = new GameMessagePublicUpdatePropertyBool(hook, PropertyBool.UiHidden, !visible);

                house.SlumLord.EnqueueBroadcast(setState, update);
            }
        }

        public void HandleActionModifyStorage(string guestName, bool hasPermission)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyStorage({guestName}, {hasPermission})");

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

            var existing = Guests.TryGetValue(storage, out var storageAccess);
            if (hasPermission)
            {
                if (!existing)
                {
                    if (Guests.Count == House.MaxGuests)
                    {
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"Your guest list has already reached the maximum limit ({House.MaxGuests})", ChatMessageType.Broadcast));
                        return;
                    }
                    AddHouseGuest(storage, true);
                }
                else
                {
                    if (storageAccess)
                    {
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"{storage.Name} already has access to your house storage.", ChatMessageType.Broadcast));
                        return;
                    }
                    ModifyHouseGuest(storage, true);
                }

                var andStr = !existing ? "and " : "";
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{storage.Name} granted access to your house {andStr}storage.", ChatMessageType.Broadcast));

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

                ModifyHouseGuest(storage, false);

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

            foreach (var guest in noStorage)
                HandleActionModifyStorage(guest.Key.Name, true);
        }

        public void HandleActionRemoveAllStorage()
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveAllStorage()");

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

            foreach (var guest in storage)
                HandleActionModifyStorage(guest.Key.Name, false);
        }

        public void HandleActionBoot(string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionBoot({playerName})");

            var player = PlayerManager.GetOnlinePlayer(playerName);

            if (player == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} is not online.", ChatMessageType.Broadcast));
                return;
            }

            // is this player in the house landcell?
            if (player.Location.GetOutdoorCell() != House.Location.GetOutdoorCell())
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is not on your property.", ChatMessageType.Broadcast));
                return;
            }

            // play script?
            player.Teleport(House.BootSpot.Location);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been booted from your house.", ChatMessageType.Broadcast));

            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has booted you from their house.", ChatMessageType.Broadcast));
        }

        public void HandleActionBootAll(bool guests = true)
        {
            //Console.WriteLine($"{Name}.HandleActionBootAll()");

            // since it can be an open house, the guest list wouldn't be enough here?
            var players = PlayerManager.GetAllOnline();

            var houseLandblock = House.Location.Landblock;
            var houseCell = House.Location.GetOutdoorCell();

            var booted = 0;
            foreach (var player in players)
            {
                // exclude self
                if (player.Equals(this)) continue;

                // quick test
                if (player.Location.Landblock != houseLandblock)
                    continue;

                if (player.Location.GetOutdoorCell() != houseCell)
                    continue;

                // keep guests if closing house
                if (!guests && Guests.ContainsKey(player))
                    continue;

                HandleActionBoot(player.Name);
                booted++;
            }

            if (guests && booted == 0)
            {
                var elseStr = Location.GetOutdoorCell() == houseCell ? "else " : "";
                Session.Network.EnqueueSend(new GameMessageSystemChat($"There is no one {elseStr}on your property.", ChatMessageType.Broadcast));
            }
        }

        public void HandleActionListAvailable(HouseType houseType)
        {
            Console.WriteLine($"{Name}.HandleActionListAvailable({houseType})");
        }

        // allegiance guest permissions

        public byte HouseSequence;

        public void UpdateRestrictionDB()
        {
            // fix event broadcast so it doesn't require a player session beforehand
            var house = GetHouse();
            if (house.PhysicsObj == null) return;

            HouseSequence++;

            Sync(house);

            var restrictions = new RestrictionDB(house);

            var nearbyPlayers = house.PhysicsObj.ObjMaint.VoyeurTable.Values.Select(v => (Player)v.WeenieObj.WorldObject).ToList();
            foreach (var player in nearbyPlayers)
                player.Session.Network.EnqueueSend(new GameEventHouseUpdateRestrictions(player.Session, house.Guid, restrictions, HouseSequence));
        }
    }
}
