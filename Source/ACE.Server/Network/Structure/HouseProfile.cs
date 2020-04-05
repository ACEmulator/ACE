using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class HouseProfile
    {
        public uint DwellingID;         // The house ID
        public ObjectGuid OwnerID;      // The object ID of the current owner
        public HouseBitfield Bitmask;
        public int MinLevel;            // The minimum level requirement to purchase this dwelling (-1 if no requirement)
        public int MaxLevel;            // The maximum level requirement to purchase this dewlling (-1 if no requirement)
        public int MinAllegRank;        // The minimum allegiance rank requirement to purchase this dwelling (-1 if no requirement)
        public int MaxAllegRank;        // The maximum allegiance rank requirement to purchase this dwelling (-1 if no requirement)
        public bool MaintenanceFree;    // Indicates maintenance is free this period, admin flag
        public HouseType Type;          // The type of dwelling (1=cottage, 2=villa, 3=mansion, 4=apartment)
        public string OwnerName;        // The name of the current owner
        public List<HousePayment> Buy;  // The list of items required for purchasing a house
        public List<HousePayment> Rent; // The list of items required for paying rent on a house

        public HouseProfile()
        {
            // set defaults
            MinLevel = -1;
            MaxLevel = -1;
            MinAllegRank = -1;
            MaxAllegRank = -1;
            Bitmask = HouseBitfield.Active;
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
        /// Sets the items that have already been paid for rent
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

    public static class HouseProfileExtensions
    {
        public static void Write(this BinaryWriter writer, HouseProfile profile)
        {
            writer.Write(profile.DwellingID);
            writer.Write(profile.OwnerID.Full);
            writer.Write((uint)profile.Bitmask);
            writer.Write(profile.MinLevel);
            writer.Write(profile.MaxLevel);
            writer.Write(profile.MinAllegRank);
            writer.Write(profile.MaxAllegRank);
            writer.Write(Convert.ToUInt32(profile.MaintenanceFree));
            writer.Write((uint)profile.Type);
            writer.WriteString16L(profile.OwnerName);
            writer.Write(profile.Buy);
            writer.Write(profile.Rent);
        }
    }
}
