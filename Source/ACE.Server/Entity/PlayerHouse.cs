using System;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class PlayerHouse: IComparable<PlayerHouse>
    {
        public uint AccountId;
        public uint PlayerGuid;
        public string PlayerName;
        public House House;
        public DateTime RentDue;

        public PlayerHouse(IPlayer player, House house)
        {
            if (player.Account != null)
                AccountId = player.Account.AccountId;
            else
                Console.WriteLine($"PlayerHouse({player.Name}, {house.Name} ({house.Guid})) - couldn't find account id");

            PlayerGuid = player.Guid.Full;
            PlayerName = player.Name;

            House = house;

            RentDue = DateTimeOffset.FromUnixTimeSeconds(player.HouseRentTimestamp.Value).UtcDateTime;
        }

        public int CompareTo(PlayerHouse playerHouse)
        {
            var result = RentDue.CompareTo(playerHouse.RentDue);

            if (result == 0)
                result = House.Guid.Full.CompareTo(playerHouse.House.Guid.Full);

            return result;
        }
    }
}
