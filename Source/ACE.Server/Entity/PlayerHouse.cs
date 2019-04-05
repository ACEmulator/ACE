using System;
using ACE.Common;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class PlayerHouse: IComparable<PlayerHouse>
    {
        public uint PlayerGuid;
        public string PlayerName;
        public House House;
        public DateTime RentDue;

        public PlayerHouse(IPlayer player, House house)
        {
            PlayerGuid = player.Guid.Full;
            PlayerName = player.Name;

            House = house;

            RentDue = DateTimeOffset.FromUnixTimeSeconds(player.HouseRentTimestamp.Value).UtcDateTime;
        }

        public int CompareTo(PlayerHouse playerHouse)
        {
            return RentDue.CompareTo(playerHouse.RentDue);
        }
    }
}
