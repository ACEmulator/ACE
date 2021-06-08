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
    /// Set of information related to house access
    /// </summary>
    public class HouseAccess
    {
        public uint Version = 0x10000002;   // 0x10000002, seems to be some kind of version.
                                            // Older version started with bitmask, so starting with 0x10000000
                                            // allows them to determine if this is V1 or V2.
                                            // The latter half appears to indicate whether there is a roommate list.
        public HARBitfield Bitmask;         // 0 = private house, 1 = open to public, 2 = Allegiance has access to house, 4 = Allegiance has access to house and storage
        public ObjectGuid MonarchID;        // populated when any allegiance access is specified
        public Dictionary<ObjectGuid, GuestInfo> GuestList; // Set of guests with their ID as the key and some additional info for them
        public List<ObjectGuid> Roommates;  // The ID list for all of your roommates

        public HouseAccess()
        {
            Bitmask = 0;
            MonarchID = ObjectGuid.Invalid;
            GuestList = new Dictionary<ObjectGuid, GuestInfo>();
            Roommates = new List<ObjectGuid>();
        }

        public HouseAccess(House house)
        {
            Bitmask = 0;
            MonarchID = ObjectGuid.Invalid;
            GuestList = new Dictionary<ObjectGuid, GuestInfo>();
            Roommates = new List<ObjectGuid>();

            if (house == null) return;

            if (house.MonarchId != null)
                MonarchID = new ObjectGuid(house.MonarchId.Value);      // for allegiance guest/storage access

            if (house.OpenToEveryone)
                Bitmask |= HARBitfield.OpenHouse;

            foreach (var guest in house.Guests)
            {
                var player = PlayerManager.FindByGuid(guest.Key);

                if (player != null && player.Guid != MonarchID)
                    GuestList.Add(guest.Key, new GuestInfo(guest.Value, player.Name));

                if (player.Guid == MonarchID)
                {                    
                    if (guest.Value)
                        Bitmask |= HARBitfield.AllegianceStorage;
                    else
                        Bitmask |= HARBitfield.AllegianceGuests;
                }
            }

            if (house.HouseOwner == null) return;

            // add in players on house owner's account
            var owner = PlayerManager.FindByGuid(house.HouseOwner.Value);

            // added for people deleting accounts from their account db...
            if (owner == null || owner.Account == null)
            {
                Console.WriteLine($"HouseAccess({house.HouseInstance:X8}): couldn't find house owner {house.HouseOwner:X8}");
                return;
            }

            var accountPlayers = Player.GetAccountPlayers(owner.Account.AccountId);

            foreach (var accountPlayer in accountPlayers)
            {
                if (owner.Guid != accountPlayer.Guid)
                    Roommates.Add(accountPlayer.Guid);
            }
        }
    }

    public static class HouseAccessExtensions
    {
        public static void Write(this BinaryWriter writer, HouseAccess har)
        {
            writer.Write(har.Version);
            writer.Write((uint)har.Bitmask);
            writer.Write(har.MonarchID.Full);
            writer.Write(har.GuestList);
            writer.Write(har.Roommates);
        }

        private static readonly ushort headerNumBuckets = 768;  // from retail pcaps, constant

        private static readonly ushort actualNumBuckets = 89;   // why this is different from what it sends in header, i have no idea
                                                                // investigate further how client derives 89 from 768

        private static readonly GuidComparer GuidComparer = new GuidComparer(actualNumBuckets);

        public static void Write(this BinaryWriter writer, Dictionary<ObjectGuid, GuestInfo> guestList)
        {
            PackableHashTable.WriteHeader(writer, guestList.Count, headerNumBuckets);

            var sorted = new SortedDictionary<ObjectGuid, GuestInfo>(guestList, GuidComparer);

            foreach (var kvp in sorted)
            {
                writer.Write(kvp.Key.Full);
                writer.Write(kvp.Value);
            }
        }

        public static void Write(this BinaryWriter writer, List<ObjectGuid> roommates)
        {
            // investigate -- odd structure, seeing possible 64 in client

            PackableHashTable.WriteHeader(writer, roommates.Count, headerNumBuckets);

            var sorted = roommates.OrderBy(i => i.Full % actualNumBuckets);

            foreach (var roommate in sorted)
                writer.Write(roommate.Full);
        }
    }
}
