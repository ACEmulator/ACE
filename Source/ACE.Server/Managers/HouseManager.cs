using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public static class HouseManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static SortedSet<PlayerHouse> RentQueue;

        private static readonly RateLimiter updateHouseManagerRateLimiter = new RateLimiter(1, TimeSpan.FromMinutes(1));

        public static void Initialize()
        {
            BuildRentQueue();
        }

        public static void BuildRentQueue()
        {
            var allPlayers = PlayerManager.GetAllPlayers();
            var houseOwners = allPlayers.Where(i => i.HouseInstance != null);

            RentQueue = new SortedSet<PlayerHouse>();

            foreach (var houseOwner in houseOwners)
                AddRentQueue(houseOwner);

            //log.Info($"Loaded {RentQueue.Count} active houses.");
        }

        public static void AddRentQueue(IPlayer player)
        {
            var houseGuid = player.HouseInstance.Value;

            var house = House.Load(houseGuid);
            var purchaseTime = (uint)(player.HousePurchaseTimestamp ?? 0);

            if (player.HouseRentTimestamp == null)
                //player.HouseRentTimestamp = (int)house.GetRentDue(purchaseTime);
                return;

            var playerHouse = new PlayerHouse(player, house);

            RentQueue.Add(playerHouse);
        }

        public static void Tick()
        {
            if (updateHouseManagerRateLimiter.GetSecondsToWaitBeforeNextEvent() > 0)
                return;

            updateHouseManagerRateLimiter.RegisterEvent();

            //log.Info($"HouseManager.Tick()");

            var nextEntry = RentQueue.FirstOrDefault();

            if (nextEntry == null)
                return;

            var currentTime = DateTime.UtcNow;

            while (currentTime > nextEntry.RentDue)
            {
                ProcessRent(nextEntry);

                RentQueue.Remove(nextEntry);

                nextEntry = RentQueue.FirstOrDefault();

                if (nextEntry == null)
                    return;

                currentTime = DateTime.UtcNow;
            }
        }

        public static async void ProcessRent(PlayerHouse playerHouse)
        {
            // load the most up-to-date house info from db
            playerHouse.House = House.GetHouse(playerHouse.House.Guid.Full);

            // todo: slumlord inventory callback
            await Task.Delay(3000);

            var isPaid = IsRentPaid(playerHouse);
            var hasRequirements = HasRequirements(playerHouse);
            //log.Info($"{playerHouse.PlayerName}.ProcessRent(): isPaid = {isPaid}");

            if (isPaid && hasRequirements)
                HandleRentPaid(playerHouse);
            else
                HandleEviction(playerHouse);
        }

        public static async void HandleRentPaid(PlayerHouse playerHouse)
        {
            var player = PlayerManager.FindByGuid(playerHouse.PlayerGuid);
            if (player == null)
            {
                log.Warn($"HouseManager.HandleRentPaid({playerHouse.PlayerName}): couldn't find player");
                return;
            }

            var purchaseTime = (uint)(player.HousePurchaseTimestamp ?? 0);
            var rentTime = (uint)(player.HouseRentTimestamp ?? 0);

            var nextRentTime = playerHouse.House.GetRentDue(purchaseTime);

            if (nextRentTime <= rentTime)
            {
                log.Warn($"HouseManager.HandleRentPaid({playerHouse.PlayerName}): nextRentTime {nextRentTime} <= rentTime {rentTime}");
                return;
            }

            player.HouseRentTimestamp = (int)nextRentTime;

            // clear out slumlord inventory
            var slumlord = playerHouse.House.SlumLord;
            slumlord.ClearInventory(true);

            log.Info($"HouseManager.HandleRentPaid({playerHouse.PlayerName}): rent payment successful!");

            BuildRentQueue();

            var onlinePlayer = PlayerManager.GetOnlinePlayer(playerHouse.PlayerGuid);
            if (onlinePlayer != null)
            {
                await Task.Delay(3000);     // wait for slumlord inventory biotas above to save

                onlinePlayer.HandleActionQueryHouse();
            }
        }

        public static async void HandleEviction(PlayerHouse playerHouse)
        {
            // todo: copied from Player_House.HandleActionAbandonHouse, move to House.Abandon()
            // todo: get online copy of house
            var house = playerHouse.House;

            // clear out slumlord inventory
            // todo: get online copy of house
            var slumlord = house.SlumLord;
            slumlord.ClearInventory(true);

            var player = PlayerManager.FindByGuid(playerHouse.PlayerGuid, out bool isOnline);

            if (!PropertyManager.GetBool("house_rent_enabled", true).Item)
            {
                // rent disabled, push forward
                var purchaseTime = (uint)(player.HousePurchaseTimestamp ?? 0);
                var nextRentTime = house.GetRentDue(purchaseTime);
                player.HouseRentTimestamp = (int)nextRentTime;

                log.Info($"HouseManager.HandleRentPaid({playerHouse.PlayerName}): house rent disabled via config");

                BuildRentQueue();
                return;
            }

            house.HouseOwner = null;
            house.MonarchId = null;

            house.ClearPermissions();

            house.SaveBiotaToDatabase();

            // relink
            house.UpdateLinks();

            // player slumlord 'off' animation
            var off = new Motion(MotionStance.Invalid, MotionCommand.Off);

            slumlord.CurrentMotionState = off;
            slumlord.EnqueueBroadcastMotion(off);

            // reset slumlord name
            var weenie = DatabaseManager.World.GetCachedWeenie(slumlord.WeenieClassId);
            var wo = WorldObjectFactory.CreateWorldObject(weenie, ObjectGuid.Invalid);
            slumlord.Name = wo.Name;

            slumlord.EnqueueBroadcast(new GameMessagePublicUpdatePropertyString(slumlord, PropertyString.Name, wo.Name));
            slumlord.SaveBiotaToDatabase();

            player.HouseId = null;
            player.HouseInstance = null;
            //player.HousePurchaseTimestamp = null;
            player.HouseRentTimestamp = null;

            house.ClearRestrictions();

            log.Info($"HouseManager.HandleRentEviction({playerHouse.PlayerName})");

            BuildRentQueue();

            if (!isOnline)
            {
                // inform player of eviction when they log in
                var offlinePlayer = PlayerManager.GetOfflinePlayer(playerHouse.PlayerGuid);
                if (offlinePlayer == null)
                {
                    log.Warn($"{playerHouse.PlayerName}.HandleEviction(): couldn't find offline player");
                    return;
                }
                offlinePlayer.SetProperty(PropertyBool.HouseEvicted, true);
                offlinePlayer.SaveBiotaToDatabase();
                return;
            }

            var onlinePlayer = PlayerManager.GetOnlinePlayer(playerHouse.PlayerGuid);

            onlinePlayer.House = null;

            // send text message
            onlinePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat("You abandon your house!", ChatMessageType.Broadcast));
            onlinePlayer.RemoveDeed();

            await Task.Delay(3000);     // wait for slumlord inventory biotas above to save

            onlinePlayer.HandleActionQueryHouse();
        }

        public static bool IsRentPaid(PlayerHouse playerHouse)
        {
            var houseData = GetHouseData(playerHouse.House);

            foreach (var rentItem in houseData.Rent)
            {
                if (rentItem.Paid < rentItem.Num)
                {
                    log.Info($"{playerHouse.PlayerName}.IsRentPaid() - required wcid {rentItem.WeenieID} amount {rentItem.Num:N0}, found {rentItem.Paid:N0}");
                    return false;
                }
            }
            return true;
        }

        public static bool HasRequirements(PlayerHouse playerHouse)
        {
            if (!PropertyManager.GetBool("house_purchase_requirements").Item)
                return true;

            var slumlord = playerHouse.House.SlumLord;
            if (slumlord.AllegianceMinLevel == null)
                return true;

            var allegianceMinLevel = PropertyManager.GetLong("mansion_min_rank", -1).Item;
            if (allegianceMinLevel == -1)
                allegianceMinLevel = slumlord.AllegianceMinLevel.Value;

            var player = PlayerManager.FindByGuid(playerHouse.PlayerGuid);

            if (player == null)
            {
                log.Info($"{playerHouse.PlayerName}.HasRequirements() - couldn't find player");
                return false;
            }

            if (player.Allegiance == null || player.AllegianceNode.Rank < allegianceMinLevel)
            {
                log.Info($"{playerHouse.PlayerName}.HasRequirements() - allegiance rank {player.AllegianceNode.Rank} < {allegianceMinLevel}");
                return false;
            }
            return true;
        }

        public static HouseData GetHouseData(House house)
        {
            var houseData = new HouseData();
            houseData.SetRentItems(house.SlumLord.GetRentItems());
            houseData.SetPaidItems(house.SlumLord);

            return houseData;
        }

        public static async void HandlePlayerDelete(uint playerGuid)
        {
            var player = PlayerManager.FindByGuid(playerGuid);
            if (player == null)
            {
                Console.WriteLine($"HouseManager.HandlePlayerDelete({playerGuid:X8}): couldn't find player guid");
                return;
            }

            if (player.HouseInstance == null)
                return;
            var playerHouse = FindPlayerHouse(playerGuid);
            if (playerHouse == null)
                return;

            // truncated ProcessRent / HandleEviction

            // load the most up-to-date house info from db
            playerHouse.House = House.GetHouse(playerHouse.House.Guid.Full);

            // todo: slumlord inventory callback
            await Task.Delay(3000);

            HandleEviction(playerHouse);
        }

        public static PlayerHouse FindPlayerHouse(uint playerGuid)
        {
            return RentQueue.FirstOrDefault(i => i.PlayerGuid == playerGuid);
        }
    }
}
