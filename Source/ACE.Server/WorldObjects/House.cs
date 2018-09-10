using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    public class House: WorldObject
    {
        public SlumLord SlumLord { get => (SlumLord)ChildLinks.FirstOrDefault(l => l as SlumLord != null); }
        public List<Hook> Hooks { get => (List<Hook>)ChildLinks.Where(l => l as Hook != null); }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public House(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public House(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        /// <summary>
        /// Builds a HouseData structure for this house
        /// </summary>
        public HouseData GetHouseData(Player owner)
        {
            var houseData = new HouseData();
            houseData.Position = Location;
            houseData.Type = HouseType.Cottage;

            if (SlumLord == null)
            {
                Console.WriteLine($"No slumlord found for {Name} ({Guid})");
            }
            else
            {
                houseData.SetBuyItems(SlumLord.GetBuyItems());
                houseData.SetRentItems(SlumLord.GetRentItems());
            }

            if (owner != null)
            {
                houseData.BuyTime = (uint)(owner.HousePurchaseTimestamp ?? 0);
                houseData.RentTime = GetRentTimestamp(owner);
            }
            return houseData;
        }

        /// <summary>
        /// The client automatically adds this amount of time to the beginning of the current maintenance period
        /// </summary>
        public static TimeSpan RentInterval = TimeSpan.FromDays(30);

        /// <summary>
        /// Returns the beginning of the current maintenance period
        /// </summary>
        public uint GetRentTimestamp(Player owner)
        {
            // get the purchaseTime -> currentTime offset
            var purchaseTime = (uint)(owner.HousePurchaseTimestamp ?? 0);

            var currentTime = (uint)Time.GetUnixTime();
            var offset = currentTime - purchaseTime;

            // calculate # of full periods in offset
            var rentIntervalSecs = (uint)RentInterval.TotalSeconds;
            var periods = offset / rentIntervalSecs;

            // return beginning of current period
            return purchaseTime + (rentIntervalSecs * periods);
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            wo.HouseId = HouseId;
            wo.HouseOwner = HouseOwner;
            wo.HouseInstance = HouseInstance;

            //Console.WriteLine($"{Name}.SetLinkProperties({wo.Name}) - houseID: {HouseId}, owner: {HouseOwner}, instance: {HouseInstance}");
        }
    }
}
