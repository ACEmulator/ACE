using System;
using System.Collections.Generic;
using System.IO;
using ACE.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// The restriction DB contains the access control list for a house
    /// </summary>
    public class RestrictionDB
    {
        public uint Version = 0x10000002;   // If high word is not 0, this value indicates the version of the message.
        public uint OpenStatus;             // 0 = private dwelling, 1 = open to public
        public ObjectGuid MonarchID;        // Allegiance monarch (if allegiance access granted)
        public Dictionary<ObjectGuid, uint> Table;  // Set of permissions on a per user basis. Key is the character id,
                                                    // value is 0 = dwelling access only, 1 = storage access as well

        public RestrictionDB()
        {
            Table = new Dictionary<ObjectGuid, uint>();
        }

        public RestrictionDB(House house)
        {
            OpenStatus = Convert.ToUInt32(house.OpenStatus);

            if (house.MonarchId != null)
                MonarchID = new ObjectGuid(house.MonarchId.Value);      // for allegiance guest/storage access

            Table = new Dictionary<ObjectGuid, uint>();
            foreach (var guest in house.Guests)
                Table.Add(guest.Key, Convert.ToUInt32(guest.Value));
        }
    }

    public static class RestrictionDBExtensions
    {
        public static void Write(this BinaryWriter writer, RestrictionDB restrictions)
        {
            writer.Write(restrictions.Version);
            writer.Write(restrictions.OpenStatus);
            writer.Write(restrictions.MonarchID.Full);
            writer.Write(restrictions.Table);
        }

        public static void Write(this BinaryWriter writer, Dictionary<ObjectGuid, uint> db)
        {
            PHashTable.WriteHeader(writer, db.Count);
            foreach (var entry in db)
            {
                writer.Write(entry.Key.Full);
                writer.Write(entry.Value);
            }
        }
    }
}
