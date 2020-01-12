using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Set of information related to owning a house
    /// </summary>
    public class HouseData
    {
        public uint BuyTime;            // When the house was purchased (Unix timestmap)
        public uint RentTime;           // When the current maintenance period began (Unix timestmap)
        public HouseType Type;          // The type of house (1=cottage, 2=villa, 3=mansion, 4=apartment)
        public bool MaintenanceFree;    // Indicates maintenance is free this period, admin flag
        public List<HousePayment> Buy;  // The list of items required for purchasing a house
        public List<HousePayment> Rent; // The list of items required for paying rent on a house
        public Position Position;       // House location

        public HouseData()
        {
            Buy = new List<HousePayment>();
            Rent = new List<HousePayment>();

            if (!PropertyManager.GetBool("house_rent_enabled", true).Item)
                MaintenanceFree = true;
        }

        /// <summary>
        /// Sets the list of items required to purchase this dwelling
        /// </summary>
        public void SetBuyItems(List<WorldObject> buyItems)
        {
            Buy = new List<HousePayment>();
            foreach (var buyItem in buyItems)
                Buy.Add(new HousePayment(buyItem));
        }

        /// <summary>
        /// Sets the list of items required to pay rent for this dwelling
        /// </summary>
        public void SetRentItems(List<WorldObject> rentItems)
        {
            Rent = new List<HousePayment>();
            foreach (var rentItem in rentItems)
                Rent.Add(new HousePayment(rentItem));
        }

        /// <summary>
        /// Sets the items that haven already been paid for rent
        /// </summary>
        public void SetPaidItems(SlumLord slumlord)
        {
            foreach (var item in slumlord.Inventory.Values)
            {
                var wcid = item.WeenieClassId;
                var value = item.StackSize ?? 1;
                if (item.WeenieClassName.StartsWith("tradenote"))
                {
                    wcid = 273;
                    value = item.Value.Value;
                }
                var rentItem = Rent.FirstOrDefault(i => i.WeenieID == wcid);
                if (rentItem == null)
                {
                    Console.WriteLine($"HouseData.SetPaidItems({slumlord.Name}): couldn't find rent item {item.WeenieClassId}");
                    continue;
                }
                rentItem.Paid = Math.Min(rentItem.Num, rentItem.Paid + value);
            }
        }
    }

    public static class HouseDataExtensions
    {
        public static void Write(this BinaryWriter writer, HouseData data)
        {
            writer.Write(data.BuyTime);
            writer.Write(data.RentTime);
            writer.Write((uint)data.Type);
            writer.Write(Convert.ToUInt32(data.MaintenanceFree));
            writer.Write(data.Buy);
            writer.Write(data.Rent);
            writer.Write(data.Position);
        }
    }
}
