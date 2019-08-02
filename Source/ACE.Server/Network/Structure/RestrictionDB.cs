using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Entity;
using ACE.Server.Managers;
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
            {
                if (guest.Key != MonarchID)
                    Table.Add(guest.Key, Convert.ToUInt32(guest.Value));
            }

            if (house.HouseOwner == null) return;

            // add in players on house owner's account
            var owner = PlayerManager.FindByGuid(house.HouseOwner.Value);

            if (owner == null)
            {
                Console.WriteLine($"RestrictionDB({house.HouseInstance:X8}): couldn't find house owner {house.HouseOwner:X8}");
                return;
            }

            var accountPlayers = Player.GetAccountPlayers(owner.Account.AccountId);

            foreach (var accountPlayer in accountPlayers)
                Table.TryAdd(accountPlayer.Guid, 1);
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
            //PHashTable.WriteHeader(writer, db.Count);

            writer.Write((ushort)db.Count);
            writer.Write((ushort)768);  // from retail pcaps, TODO: determine how this is calculated

            // reorder
            var _db = new List<Tuple<ObjectGuid, uint>>();
            foreach (var entry in db)
                _db.Add(new Tuple<ObjectGuid, uint>(entry.Key, entry.Value));

            // sort by client function - hashKey % tableSize - how it gets tableSize 89 from 768, no idea
            _db = _db.OrderBy(i => i.Item1.Full % 89).ToList();

            foreach (var entry in _db)
            {
                writer.Write(entry.Item1.Full);
                writer.Write(entry.Item2);
            }
        }
    }
}
