using System;
using System.Collections.Generic;
using System.Text;
using ACE.Server.Network.Structure;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Database.Models.World;
using ACE.Server.Factories;
using ACE.Common;

namespace ACE.Server.WorldObjects
{
    public class House
    {
        public HouseData HouseData;
        public SlumLord SlumLord;
        public Player Player;

        public House() { }

        public House(uint slumlord_id, Player player)
        {
            Player = player;

            var house = new HouseData();

            var instance = DatabaseManager.World.GetLandblockInstanceByGuid(slumlord_id);

            if (instance == null)
                return;

            house.Position = new Position(instance.ObjCellId, instance.OriginX, instance.OriginY, instance.OriginZ, instance.AnglesX, instance.AnglesY, instance.AnglesZ, instance.AnglesW);
            house.Type = HouseType.Cottage;

            var SlumLord = (SlumLord)WorldObjectFactory.CreateNewWorldObject(instance.WeenieClassId);
            if (SlumLord == null)
            {
                Console.WriteLine($"House constructor({slumlord_id:X8}): couldn't build slumlord");
                return;
            }

            house.SetBuyItems(SlumLord.GetBuyItems());
            house.SetRentItems(SlumLord.GetRentItems());

            house.BuyTime = (uint)(player.HousePurchaseTimestamp ?? 0);
            house.RentTime = GetRentTimestamp();

            HouseData = house;
        }

        /// <summary>
        /// The client automatically adds this amount of time to the beginning of the current maintenance period
        /// </summary>
        public static TimeSpan RentInterval = TimeSpan.FromDays(30);

        /// <summary>
        /// Returns the beginning of the current maintenance period
        /// </summary>
        public uint GetRentTimestamp()
        {
            // get the purchaseTime -> currentTime offset
            var purchaseTime = (uint)(Player.HousePurchaseTimestamp ?? 0);

            var currentTime = (uint)Time.GetUnixTime();
            var offset = currentTime - purchaseTime;

            // calculate # of full periods in offset
            var rentIntervalSecs = (uint)RentInterval.TotalSeconds;
            var periods = offset / rentIntervalSecs;

            // return beginning of current period
            return purchaseTime + (rentIntervalSecs * periods);
        }
    }
}
