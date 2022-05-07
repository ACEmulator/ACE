using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database;
using ACE.Database.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public static class HouseList
    {
        public static List<HouseListResults> AllHouses;

        public static Dictionary<HouseType, List<HouseListResults>> Available;

        public static void GetHouseList()
        {
            // PCAP Part 1\Dr_Doom_Random_Running_Around\pkt_2017-1-29_1485735833_log.pcap (18538)
            // PCAP Part 1\Fenn-pcap6\pkt_2017-1-31_1485869242_log.pcap (541)

            // get a list of all the houses in the world
            FindAllHouses();

            // get a list of which houses are occupied for this shard
            var housesOwned = GetHousesOwned();

            // build available
            BuildAvailable(housesOwned);

            var apartments = Available[HouseType.Apartment];
            var cottages = Available[HouseType.Cottage];
            var villas = Available[HouseType.Villa];
            var mansions = Available[HouseType.Mansion];

            Console.WriteLine($"Apartments: {apartments.Count}");
            Console.WriteLine($"Cottages: {cottages.Count}");
            Console.WriteLine($"Villas: {villas.Count}");
            Console.WriteLine($"Mansions: {mansions.Count}");
        }

        public static void FindAllHouses()
        {
            // select * from weenie inner join weenie_properties_int wint on weenie.class_Id=wint.object_Id inner join landblock_instance winst on weenie.class_Id=winst.weenie_Class_Id where weenie.`type`=53 and wint.`type`=155 order by wint.`value`;

            if (AllHouses == null)
                AllHouses = DatabaseManager.World.GetHousesAll();

            Console.WriteLine($"Total houses: {AllHouses.Count}");
        }

        public static HashSet<uint> GetHousesOwned()
        {
            // select * from biota where weenie_Type=53;

            var housesOwned = DatabaseManager.Shard.BaseDatabase.GetHousesOwned();

            Console.WriteLine($"Owned houses: {housesOwned.Count}");

            // build owned hashset
            var owned = new HashSet<uint>();

            foreach (var house in housesOwned)
            {
                if (owned.Contains(house.Id))
                {
                    Console.WriteLine($"HouseList.GetOwned(): duplicate owned house id {house.Id}");
                    continue;
                }
                owned.Add(house.Id);
            }
            return owned;
        }

        public static void BuildAvailable(HashSet<uint> housesOwned)
        {
            Available = new Dictionary<HouseType, List<HouseListResults>>();
            Available.Add(HouseType.Apartment, new List<HouseListResults>());
            Available.Add(HouseType.Cottage, new List<HouseListResults>());
            Available.Add(HouseType.Villa, new List<HouseListResults>());
            Available.Add(HouseType.Mansion, new List<HouseListResults>());

            foreach (var house in AllHouses)
            {
                if (housesOwned.Contains(house.LandblockInstance.Guid))
                    continue;

                Available[house.HouseType].Add(house);
            }
        }

        public static List<uint> GetAvailableLocations(HouseType houseType)
        {
            // cache results?
            if (Available == null) GetHouseList();

            return Available[houseType].Select(i => i.LandblockInstance.ObjCellId | 0x0001).ToList();
        }

        public static void RemoveFromAvailable(SlumLord slumLord)
        {
            if (Available == null) return; // no results cached, move on.

            var houseType = slumLord.House.HouseType;

            var house = Available[houseType].FirstOrDefault(i => i.LandblockInstance.Guid == slumLord.Guid.Full);
            if (house != null)
                Available[houseType].Remove(house);
        }

        public static void AddToAvailable(SlumLord slumLord)
        {
            if (Available == null) return; // no results cached, move on.

            var houseType = slumLord.House.HouseType;

            var house = Available[houseType].FirstOrDefault(i => i.LandblockInstance.Guid == slumLord.Guid.Full);
            if (house != null)
                Available[houseType].Remove(house);

            var weenie = DatabaseManager.World.GetWeenie(slumLord.WeenieClassId);
            var landblockInstance = slumLord.House.LinkedInstances.FirstOrDefault(i => i.Guid == slumLord.Guid.Full);

            if (weenie == null || landblockInstance == null) return;

            Available[houseType].Add(new HouseListResults(weenie, landblockInstance));
        }
    }
}
