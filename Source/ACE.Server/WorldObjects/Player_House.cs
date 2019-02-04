using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACE.Common;
using ACE.Database;
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

        public Dictionary<ObjectGuid, bool> Guests => House?.Guests;

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

            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

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

            // boot anyone who may have been wandering around inside...
            HandleActionBootAll(false);
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
            var stackStr = buyItem.StackSize != null && buyItem.StackSize > 1 ? buyItem.StackSize.ToString() + " " : "";
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
            House = House.Load(houseGuid);

            return House;
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

        public House GetDungeonHouse()
        {
            var landblockId = new LandblockId(House.DungeonLandblockID);
            var isLoaded = LandblockManager.IsLoaded(landblockId);

            if (!isLoaded)
                return null;

            var loaded = LandblockManager.GetLandblock(landblockId, false);
            var wos = loaded.GetWorldObjectsForPhysicsHandling();
            return wos.FirstOrDefault(wo => wo.WeenieClassId == House.WeenieClassId) as House;
        }

        public void AddHouseGuest(IPlayer guest, bool storage)
        {
            var house = GetHouse();
            house.AddGuest(guest, storage);

            Guests.Add(guest.Guid, storage);
            UpdateRestrictionDB();
        }

        public void ModifyHouseGuest(IPlayer guest, bool storage)
        {
            var house = GetHouse();
            house.UpdateGuest(guest, storage);

            Guests[guest.Guid] = storage;
            UpdateRestrictionDB();
        }

        public void RemoveHouseGuest(IPlayer guest)
        {
            var house = GetHouse();
            house.RemoveGuest(guest);

            Guests.Remove(guest.Guid);
            UpdateRestrictionDB();
        }

        public void Sync(House house)
        {
            house.Guests = House.Guests;
            house.OpenStatus = House.OpenStatus;

            house.MonarchId = House.MonarchId;
        }


        public void HandleActionAddGuest(string guestName)
        {
            //Console.WriteLine($"{Name}.HandleActionAddGuest({guestName})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

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

            if (Guests.ContainsKey(guest.Guid))
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
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

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
                if (OnProperty(onlineGuest))
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

            if (Guests.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guest list is empty.", ChatMessageType.Broadcast));
                return;
            }

            foreach (var guid in Guests.Keys.ToList())
            {
                var guest = PlayerManager.FindByGuid(guid);
                HandleActionRemoveGuest(guest.Name);
            }
        }

        public void HandleActionGuestList()
        {
            //Console.WriteLine($"{Name}.HandleActionGuestList()");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            if (Guests.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your house guest list is empty.", ChatMessageType.Broadcast));
                return;
            }

            var sb = new StringBuilder($"{Name}'s {House.SlumLord.Name} guest list:\n");
            foreach (var kvp in Guests)
            {
                var guest = PlayerManager.FindByGuid(kvp.Key);
                var guestName = guest.Name;
                if (House.MonarchId != null && guest.Guid.Full == House.MonarchId)
                    guestName += "'s Allegiance";

                var storage = kvp.Value ? "* " : "";
                sb.Append(storage + guestName + "\n");
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat(sb.ToString(), ChatMessageType.Broadcast));
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
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is already open.", ChatMessageType.Broadcast));
                else
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your house is already restricted to guests only.", ChatMessageType.Broadcast));

                return;
            }

            house.OpenStatus = openStatus;

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

            var state = PhysicsState.Ethereal | PhysicsState.IgnoreCollisions;
            if (!visible) state |= PhysicsState.NoDraw;

            foreach (var hook in house.Hooks.Where(i => i.Inventory.Count == 0))
            {
                var setState = new GameMessageSetState(hook, state);
                var update = new GameMessagePublicUpdatePropertyBool(hook, PropertyBool.UiHidden, !visible);

                house.EnqueueBroadcast(setState, update);
            }

            // if house has dungeon, repeat this process
            if (house.HasDungeon)
            {
                var dungeonHouse = GetDungeonHouse();
                if (dungeonHouse == null) return;

                foreach (var hook in dungeonHouse.Hooks.Where(i => i.Inventory.Count == 0))
                {
                    var setState = new GameMessageSetState(hook, state);
                    var update = new GameMessagePublicUpdatePropertyBool(hook, PropertyBool.UiHidden, !visible);

                    dungeonHouse.EnqueueBroadcast(setState, update);
                }
            }

            if (house.CurrentLandblock == null)
                house.SaveBiotaToDatabase();
        }

        public void HandleActionModifyStorage(string guestName, bool hasPermission)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyStorage({guestName}, {hasPermission})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

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
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

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
                HandleActionModifyStorage(guest.Name, true);
            }
        }

        public void HandleActionRemoveAllStorage()
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveAllStorage()");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

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
                HandleActionModifyStorage(guest.Name, false);
            }
        }

        public void HandleActionBoot(string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionBoot({playerName})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            var player = PlayerManager.GetOnlinePlayer(playerName);

            if (player == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} is not online.", ChatMessageType.Broadcast));
                return;
            }

            // is this player in the house landcell?
            if (!OnProperty(player))
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
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            // since it can be an open house, the guest list wouldn't be enough here?
            var players = PlayerManager.GetAllOnline();

            var houseLandblock = House.Location.Landblock;

            var booted = 0;
            foreach (var player in players)
            {
                // exclude self
                if (player.Equals(this)) continue;

                if (!OnProperty(player)) continue;

                // keep guests if closing house
                if (!guests && Guests.ContainsKey(player.Guid))
                    continue;

                HandleActionBoot(player.Name);
                booted++;
            }

            if (guests && booted == 0)
            {
                var elseStr = OnProperty(this) ? "else " : "";
                Session.Network.EnqueueSend(new GameMessageSystemChat($"There is no one {elseStr}on your property.", ChatMessageType.Broadcast));
            }
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

        public bool OnProperty(Player player)
        {
            var house = GetHouse();

            if (player.Location.GetOutdoorCell() == House.Location.GetOutdoorCell())
                return true;

            foreach (var linkedHouse in house.LinkedHouses)
                if (player.Location.GetOutdoorCell() == linkedHouse.Location.GetOutdoorCell())
                    return true;

            if (House.HasDungeon)
            {
                if ((player.Location.Cell | 0xFFFF) == House.DungeonLandblockID)
                    return true;
            }
            return false;
        }

        // allegiance guest permissions

        public byte HouseSequence;

        public void UpdateRestrictionDB()
        {
            var restrictions = new RestrictionDB(House);

            // update house
            var house = GetHouse();
            if (house.PhysicsObj != null)
                UpdateRestrictionDB(restrictions, house);

            // for mansions, update the linked houses
            foreach (var linkedHouse in house.LinkedHouses)
                UpdateRestrictionDB(restrictions, linkedHouse);

            // update house dungeon
            if (house.HasDungeon)
            {
                var dungeonHouse = GetDungeonHouse();
                if (dungeonHouse == null || dungeonHouse.PhysicsObj == null) return;

                UpdateRestrictionDB(restrictions, dungeonHouse);
            }
        }

        public void UpdateRestrictionDB(RestrictionDB restrictions, House house)
        {
            HouseSequence++;

            Sync(house);

            var nearbyPlayers = house.PhysicsObj.ObjMaint.VoyeurTable.Values.Select(v => (Player)v.WeenieObj.WorldObject).ToList();
            foreach (var player in nearbyPlayers)
                player.Session.Network.EnqueueSend(new GameEventHouseUpdateRestrictions(player.Session, house.Guid, restrictions, HouseSequence));
        }

        public void HandleActionModifyAllegianceGuestPermission(bool add)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyAllegianceGuestPermission({add})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
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
                    AddHouseGuest(Allegiance.Monarch.Player, false);
                else
                    ModifyHouseGuest(Allegiance.Monarch.Player, false);     // handle case: the monarch already has guest/storage access already, now adding allegiance

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

                RemoveHouseGuest(Allegiance.Monarch.Player);

                HandleActionBootAll(false);     // boot anyone who doesn't have guest access

                Session.Network.EnqueueSend(new GameMessageSystemChat($"You have revoked access to your dwelling to your monarchy.", ChatMessageType.Broadcast));
            }
        }

        public void HandleActionModifyAllegianceStoragePermission(bool add)
        {
            //Console.WriteLine($"{Name}.HandleActionModifyAllegianceStoragePermission({add})");
            if (House == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
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
                    AddHouseGuest(Allegiance.Monarch.Player, true);
                else
                    ModifyHouseGuest(Allegiance.Monarch.Player, true);     // handle case: the monarch already has guest/storage access already, now adding allegiance

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
                ModifyHouseGuest(Allegiance.Monarch.Player, false);

                Session.Network.EnqueueSend(new GameMessageSystemChat($"You have revoked storage access to your monarchy.", ChatMessageType.Broadcast));
            }
        }

        // TODO: clean up this royal mess below here...

        public void HandleActionDoAllegianceHouseAction(AllegianceHouseAction action)
        {
            Console.WriteLine($"{Name}.DoAllegianceHouseAction({action})");

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

            if (allegianceHouse.HouseType < ACE.Entity.Enum.HouseType.Villa)
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
            allegianceHouse.Guests.Add(Allegiance.Monarch.PlayerGuid, false);
            UpdateRestrictionDB_AllegianceHouse(allegianceHouse);

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
            allegianceHouse.Guests.Remove(Allegiance.Monarch.PlayerGuid);
            UpdateRestrictionDB_AllegianceHouse(allegianceHouse);

            // todo: move to House
            //HandleActionBootAll(false);     // boot anyone who doesn't have guest access

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
                allegianceHouse.Guests.Add(Allegiance.Monarch.PlayerGuid, true);
            }
            else
            {
                // handle guest -> storage access upgrade
                allegianceHouse.UpdateGuest(Allegiance.Monarch.Player, true);
                allegianceHouse.Guests[Allegiance.Monarch.PlayerGuid] = true;   
            }

            UpdateRestrictionDB_AllegianceHouse(allegianceHouse);

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
            allegianceHouse.UpdateGuest(Allegiance.Monarch.Player, false);
            allegianceHouse.Guests[Allegiance.Monarch.PlayerGuid] = false;

            UpdateRestrictionDB_AllegianceHouse(allegianceHouse);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have revoked your monarchy's access to the allegiance housing storage.", ChatMessageType.Broadcast));

        }

        public void UpdateRestrictionDB_AllegianceHouse(House allegianceHouse)
        {
            var restrictions = new RestrictionDB(allegianceHouse);

            // update house
            if (allegianceHouse.PhysicsObj != null)
                UpdateRestrictionDB_AllegianceHouse(restrictions, allegianceHouse, allegianceHouse);

            // for mansions, update the linked houses
            foreach (var linkedHouse in allegianceHouse.LinkedHouses)
                UpdateRestrictionDB_AllegianceHouse(restrictions, linkedHouse, allegianceHouse);

            // update house dungeon
            if (allegianceHouse.HasDungeon)
            {
                var dungeonHouse = GetDungeonHouse_AllegianceHouse(allegianceHouse);
                if (dungeonHouse == null || dungeonHouse.PhysicsObj == null) return;

                UpdateRestrictionDB_AllegianceHouse(restrictions, dungeonHouse, allegianceHouse);
            }
        }

        public void UpdateRestrictionDB_AllegianceHouse(RestrictionDB restrictions, House house, House allegianceHouse)
        {
            HouseSequence++;    // ??

            Sync_AllegianceHouse(house, allegianceHouse);

            var nearbyPlayers = house.PhysicsObj.ObjMaint.VoyeurTable.Values.Select(v => (Player)v.WeenieObj.WorldObject).ToList();
            foreach (var player in nearbyPlayers)
                player.Session.Network.EnqueueSend(new GameEventHouseUpdateRestrictions(player.Session, house.Guid, restrictions, HouseSequence));
        }

        public House GetDungeonHouse_AllegianceHouse(House allegianceHouse)
        {
            var landblockId = new LandblockId(allegianceHouse.DungeonLandblockID);
            var isLoaded = LandblockManager.IsLoaded(landblockId);

            if (!isLoaded)
                return null;

            var loaded = LandblockManager.GetLandblock(landblockId, false);
            var wos = loaded.GetWorldObjectsForPhysicsHandling();
            return wos.FirstOrDefault(wo => wo.WeenieClassId == allegianceHouse.WeenieClassId) as House;
        }

        public void Sync_AllegianceHouse(House house, House allegianceHouse)
        {
            house.Guests = allegianceHouse.Guests;
            house.OpenStatus = allegianceHouse.OpenStatus;

            house.MonarchId = allegianceHouse.MonarchId;
        }
    }
}
