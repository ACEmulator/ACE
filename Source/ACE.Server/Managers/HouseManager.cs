using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
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

        /// <summary>
        /// A lookup table of HouseId => HouseGuid
        /// </summary>
        public static Dictionary<uint, List<uint>> HouseIdToGuid;

        public static bool IsBuilding;

        public static SortedSet<PlayerHouse> RentQueue;

        private static readonly RateLimiter updateHouseManagerRateLimiter = new RateLimiter(1, TimeSpan.FromMinutes(1));

        public static void Initialize()
        {
            BuildHouseIdToGuid();
            BuildRentQueue();
        }

        /// <summary>
        /// Builds the lookup table for HouseId => HouseGuid
        /// </summary>
        public static void BuildHouseIdToGuid()
        {
            using (var ctx = new WorldDbContext())
            {
                var query = from weenie in ctx.Weenie
                            join inst in ctx.LandblockInstance on weenie.ClassId equals inst.WeenieClassId
                            where weenie.Type == (int)WeenieType.House
                            select new
                            {
                                Weenie = weenie,
                                Instance = inst
                            };

                var results = query.ToList();

                HouseIdToGuid = new Dictionary<uint, List<uint>>();

                foreach (var result in results)
                {
                    var classname = result.Weenie.ClassName;
                    var guid = result.Instance.Guid;

                    if (!uint.TryParse(Regex.Match(classname, @"\d+").Value, out var houseId))
                    {
                        log.Error($"HouseManager.BuildHouseIdToGuid(): couldn't parse {classname}");
                        continue;
                    }

                    if (!HouseIdToGuid.TryGetValue(houseId, out var houseGuids))
                    {
                        houseGuids = new List<uint>();
                        HouseIdToGuid.Add(houseId, houseGuids);
                    }
                    houseGuids.Add(guid);
                }
                //log.Info($"BuildHouseIdToGuid: {HouseIdToGuid.Count}");
            }
        }

        public static void BuildRentQueue()
        {
            // todo: get rid of async, use proper slumlord inventory callbacks
            if (IsBuilding) return;

            IsBuilding = true;

            RentQueue = new SortedSet<PlayerHouse>();

            //var allPlayers = PlayerManager.GetAllPlayers();
            //var houseOwners = allPlayers.Where(i => i.HouseInstance != null);

            //foreach (var houseOwner in houseOwners)
                //AddRentQueue(houseOwner);

            var slumlordBiotas = DatabaseManager.Shard.GetBiotasByType(WeenieType.SlumLord);

            foreach (var slumlord in slumlordBiotas)
                AddRentQueue(slumlord);

            IsBuilding = false;

            //log.Info($"Loaded {RentQueue.Count} active houses.");
            QueryMultiHouse();
        }

        public static void AddRentQueue(Biota slumlord)
        {
            var biotaOwner = slumlord.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (ushort)PropertyInstanceId.HouseOwner);
            if (biotaOwner == null)
            {
                // this is fine. this is just a house that was purchased, and then later abandoned
                //Console.WriteLine($"HouseManager.AddRentQueue(): couldn't find owner for house {slumlord.Id:X8}");
                return;
            }
            var owner = PlayerManager.FindByGuid(biotaOwner.Value);
            if (owner == null)
            {
                Console.WriteLine($"HouseManager.AddRentQueue(): couldn't find owner {biotaOwner.Value:X8}");
                return;
            }
            var houseId = slumlord.BiotaPropertiesDID.FirstOrDefault(i => i.Type == (ushort)PropertyDataId.HouseId);
            if (houseId == null)
            {
                Console.WriteLine($"HouseManager.AddRentQueue(): couldn't find house id for {slumlord.Id:X8}");
                return;
            }
            if (!HouseIdToGuid.TryGetValue(houseId.Value, out var houseGuids))
            {
                Console.WriteLine($"HouseManager.AddRentQueue(): couldn't find house instance for {slumlord.Id:X8}");
                return;
            }
            var houseInstance = GetHouseGuid(slumlord.Id, houseGuids);
            if (houseInstance == 0)
            {
                Console.WriteLine($"HouseManager.AddRentQueue(): couldn't find house guid for {slumlord.Id:X8}");
                return;
            }
            if (RentQueueContainsHouse(houseInstance))
            {
                Console.WriteLine($"HouseManager.AddRentQueue(): rent queue already contains house {houseInstance}");
                return;
            }
            AddRentQueue(owner, houseInstance);
        }

        public static bool RentQueueContainsHouse(uint houseInstance)
        {
            return RentQueue.Any(i => i.House.HouseInstance == houseInstance);
        }

        public static void AddRentQueue(IPlayer player, uint houseGuid)
        {
            Console.WriteLine($"AddRentQueue({player.Name}, {houseGuid:X8})");

            var house = House.Load(houseGuid);
            if (house == null)      // this can happen for basement dungeons
                return;

            var purchaseTime = (uint)(player.HousePurchaseTimestamp ?? 0);

            if (player.HouseRentTimestamp == null)
                //player.HouseRentTimestamp = (int)house.GetRentDue(purchaseTime);
                return;

            var playerHouse = new PlayerHouse(player, house);

            RentQueue.Add(playerHouse);
        }

        public static void QueryMultiHouse()
        {
            var slumlordBiotas = DatabaseManager.Shard.GetBiotasByType(WeenieType.SlumLord);

            var playerHouses = new Dictionary<IPlayer, List<Biota>>();
            var accountHouses = new Dictionary<string, List<Biota>>();

            foreach (var slumlord in slumlordBiotas)
            {
                var biotaOwner = slumlord.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (ushort)PropertyInstanceId.HouseOwner);
                if (biotaOwner == null)
                {
                    // this is fine. this is just a house that was purchased, and then later abandoned
                    //Console.WriteLine($"HouseManager.QueryMultiHouse(): couldn't find owner for house {slumlord.Id:X8}");
                    continue;
                }
                var owner = PlayerManager.FindByGuid(biotaOwner.Value);
                if (owner == null)
                {
                    Console.WriteLine($"HouseManager.QueryMultiHouse(): couldn't find owner {biotaOwner.Value:X8}");
                    continue;
                }

                if (!playerHouses.TryGetValue(owner, out var houses))
                {
                    houses = new List<Biota>();
                    playerHouses.Add(owner, houses);
                }
                houses.Add(slumlord);

                var accountName = owner.Account != null ? owner.Account.AccountName : "NULL";

                if (!accountHouses.TryGetValue(accountName, out var aHouses))
                {
                    aHouses = new List<Biota>();
                    accountHouses.Add(accountName, aHouses);
                }
                aHouses.Add(slumlord);
            }

            Console.WriteLine("Multi-house owners:");

            if (PropertyManager.GetBool("house_per_char").Item)
            {
                foreach (var playerHouse in playerHouses.Where(i => i.Value.Count() > 1).OrderByDescending(i => i.Value.Count()))
                {
                    Console.WriteLine($"{playerHouse.Key.Name}: {playerHouse.Value.Count}");

                    for (var i = 0; i < playerHouse.Value.Count; i++)
                        Console.WriteLine($"{i + 1}. {GetCoords(playerHouse.Value[i])}");
                }
            }
            else
            {
                foreach (var accountHouse in accountHouses.Where(i => i.Value.Count() > 1).OrderByDescending(i => i.Value.Count()))
                {
                    Console.WriteLine($"{accountHouse.Key}: {accountHouse.Value.Count}");

                    for (var i = 0; i < accountHouse.Value.Count; i++)
                        Console.WriteLine($"{i + 1}. {GetCoords(accountHouse.Value[i])}");
                }
            }
        }

        public static string GetCoords(Biota biota)
        {
            var p = biota.BiotaPropertiesPosition.FirstOrDefault(i => i.PositionType == (ushort)PositionType.Location);

            return GetCoords(new Position(p.ObjCellId, p.OriginX, p.OriginY, p.OriginZ, p.AnglesX, p.AnglesY, p.AnglesZ, p.AnglesW));
        }

        public static string GetCoords(Position position)
        {
            var coords = position.GetMapCoordStr();

            if (coords == null)
            {
                // apartment slumlord?
                if (ApartmentBlocks.TryGetValue(position.Landblock, out var apartmentBlock))
                    coords = $"{apartmentBlock} - ";
                else
                    log.Error($"HouseManager.GetCoords({position}) - couldn't find apartment block");

                coords += position;
            }

            return coords;
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
            house.HouseOwnerName = null;

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

            // ensure allegiance is loaded
            var allegiance = AllegianceManager.GetAllegiance(player);

            AllegianceNode allegianceNode = null;
            if (allegiance != null)
                allegiance.Members.TryGetValue(player.Guid, out allegianceNode);

            var rank = allegianceNode != null ? allegianceNode.Rank : 0;

            if (allegiance == null || rank < allegianceMinLevel)
            {
                log.Info($"{playerHouse.PlayerName}.HasRequirements() - allegiance rank {rank} < {allegianceMinLevel}");
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

        public static List<House> GetAccountHouses(uint accountId)
        {
            return RentQueue.Where(i => i.AccountId == accountId).Select(i => i.House).ToList();
        }

        public static List<House> GetCharacterHouses(uint playerGuid)
        {
            return RentQueue.Where(i => i.PlayerGuid == playerGuid).Select(i => i.House).ToList();
        }

        public static uint GetHouseGuid(uint slumlord_guid, List<uint> house_guids)
        {
            var slumlord_prefix = slumlord_guid >> 12;

            return house_guids.FirstOrDefault(i => slumlord_prefix == (i >> 12));
        }

        public static Dictionary<uint, string> ApartmentBlocks = new Dictionary<uint, string>()
        {
            { 0x5360, "Sanctum Residential Halls - Alvan Court" },
            { 0x5361, "Sanctum Residential Halls - Caerna Dwellings" },
            { 0x5362, "Sanctum Residential Halls - Illsin Veranda" },
            { 0x5363, "Sanctum Residential Halls - Marin Court" },
            { 0x5364, "Sanctum Residential Halls - Ruadnar Court" },
            { 0x5365, "Sanctum Residential Halls - Senmai Court" },
            { 0x5366, "Sanctum Residential Halls - Sigil Veranda" },
            { 0x5367, "Sanctum Residential Halls - Sorveya Court" },
            { 0x5368, "Sanctum Residential Halls - Sylvan Dwellings" },
            { 0x5369, "Sanctum Residential Halls - Treyval Veranda" },
            { 0x7200, "Atrium Residential Halls - Winthur Gate" },
            { 0x7300, "Atrium Residential Halls - Larkspur Gardens" },
            { 0x7400, "Atrium Residential Halls - Mellas Court" },
            { 0x7500, "Atrium Residential Halls - Vesper Gate" },
            { 0x7600, "Atrium Residential Halls - Gajin Dwellings" },
            { 0x7700, "Atrium Residential Halls - Valorya Gate" },
            { 0x7800, "Atrium Residential Halls - Heartland Yard" },
            { 0x7900, "Atrium Residential Halls - Ivory Gate" },
            { 0x7A00, "Atrium Residential Halls - Alphas Court" },
            { 0x7B00, "Atrium Residential Halls - Hasina Gardens" },
            { 0x7C00, "Oriel Residential Halls - Sorac Gate" },
            { 0x7D00, "Oriel Residential Halls - Maru Veranda" },
            { 0x7E00, "Oriel Residential Halls - Forsythian Gardens" },
            { 0x7F00, "Oriel Residential Halls - Vindalan Dwellings" },
            { 0x8000, "Oriel Residential Halls - Syrah Dwellings" },
            { 0x8100, "Oriel Residential Halls - Allain Court" },
            { 0x8200, "Oriel Residential Halls - White Lotus Gate" },
            { 0x8300, "Oriel Residential Halls - Autumn Moon Gardens" },
            { 0x8400, "Oriel Residential Halls - Trellyn Gardens" },
            { 0x8500, "Oriel Residential Halls - Endara Gate" },
            { 0x8600, "Haven Residential Halls - Celcynd Grotto" },
            { 0x8700, "Haven Residential Halls - Trothyr Hollow" },
            { 0x8800, "Haven Residential Halls - Jojii Gardens" },
            { 0x8900, "Haven Residential Halls - Cedraic Court" },
            { 0x8A00, "Haven Residential Halls - Ben Ten Lodge" },
            { 0x8B00, "Haven Residential Halls - Dulok Court" },
            { 0x8C00, "Haven Residential Halls - Crescent Moon Veranda" },
            { 0x8D00, "Haven Residential Halls - Jade Gate" },
            { 0x8E00, "Haven Residential Halls - Ispar Yard" },
            { 0x8F00, "Haven Residential Halls - Xao Wu Gardens" },
            { 0x9000, "Victory Residential Halls - Accord Veranda" },
            { 0x9100, "Victory Residential Halls - Candeth Court" },
            { 0x9200, "Victory Residential Halls - Celdiseth Court" },
            { 0x9300, "Victory Residential Halls - Festivus Court" },
            { 0x9400, "Victory Residential Halls - Hibiscus Gardens" },
            { 0x9500, "Victory Residential Halls - Meditation Gardens" },
            { 0x9600, "Victory Residential Halls - Setera Gardens" },
            { 0x9700, "Victory Residential Halls - Spirit Gate" },
            { 0x9800, "Victory Residential Halls - Triumphal Gardens" },
            { 0x9900, "Victory Residential Halls - Wilamil Court" },
        };
    }
}
