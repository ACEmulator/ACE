using System.Collections.Generic;
using System.IO;
using ACE.Entity;

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
        public uint Bitmask;                // 0 = private house, 1 = open to public
        public ObjectGuid MonarchID;        // populated when any allegiance access is specified
        public Dictionary<ObjectGuid, GuestInfo> GuestList; // Set of guests with their ID as the key and some additional info for them
        public List<ObjectGuid> Roommates;  // The ID list for all of your roommates
    }

    public static class HouseAccessExtensions
    {
        public static void Write(this BinaryWriter writer, HouseAccess har)
        {
            writer.Write(har.Version);
            writer.Write(har.Bitmask);
            writer.Write(har.MonarchID.Full);
            writer.Write(har.GuestList);
            writer.Write(har.Roommates);
        }

        public static void Write(this BinaryWriter writer, Dictionary<ObjectGuid, GuestInfo> guests)
        {
            PackableHashTable.WriteHeader(writer, guests.Count);
            foreach (var guest in guests)
            {
                writer.Write(guest.Key.Full);
                writer.Write(guest.Value);
            }
        }

        public static void Write(this BinaryWriter writer, List<ObjectGuid> roommates)
        {
            // protocol docs list this as a PList instead of a PackableList
            // the only difference seems to be whether the count is listed as uint or int,
            // which shouldn't make a difference..
            writer.Write(roommates.Count);
            foreach (var roommate in roommates)
                writer.Write(roommate.Full);
        }
    }
}
