using System;
using System.Collections.Generic;
using System.Text;
using ACE.Server.Network.Structure;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Database.Models.World;
using ACE.Server.Factories;

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

            house.BuyTime = (uint)player.HouseBuyTimestamp;
            house.RentTime = (uint)player.HouseRentTimestamp;

            HouseData = house;
        }
    }
}
