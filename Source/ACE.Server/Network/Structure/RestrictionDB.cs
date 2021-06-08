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
        public uint HouseOwner;
        public uint Version = 0x10000002;   // If high word is not 0, this value indicates the version of the message.
        public bool OpenStatus;             // 0 = private dwelling, 1 = open to public
        public ObjectGuid MonarchID;        // Allegiance monarch (if allegiance access granted)
        public Dictionary<ObjectGuid, uint> Table;  // Set of permissions on a per user basis. Key is the character id,
                                                    // value is 0 = dwelling access only, 1 = storage access as well

        public RestrictionDB()
        {
            Table = new Dictionary<ObjectGuid, uint>();
            HouseOwner = 0;
        }

        public RestrictionDB(House house)
        {
            Table = new Dictionary<ObjectGuid, uint>();

            if (house == null) return;

            HouseOwner = house.HouseOwner ?? 0;

            OpenStatus = house.OpenStatus;

            if (house.MonarchId != null)
                MonarchID = new ObjectGuid(house.MonarchId.Value);      // for allegiance guest/storage access

            foreach (var guest in house.Guests)
            {
                if (guest.Key != MonarchID)
                    Table.Add(guest.Key, Convert.ToUInt32(guest.Value));
            }

            if (house.HouseOwner == null) return;

            // add in players on house owner's account
            var owner = PlayerManager.FindByGuid(house.HouseOwner.Value);

            // added for people deleting accounts from their account db...
            if (owner == null || owner.Account == null)
            {
                Console.WriteLine($"RestrictionDB({house.HouseInstance:X8}): couldn't find house owner {house.HouseOwner:X8}");
                return;
            }

            var accountPlayers = Player.GetAccountPlayers(owner.Account.AccountId);

            foreach (var accountPlayer in accountPlayers)
            {
                if (accountPlayer.Guid.Full == HouseOwner)
                    continue;

                Table.TryAdd(accountPlayer.Guid, 1);
            }
        }
    }

    public static class RestrictionDBExtensions
    {
        public static void Write(this BinaryWriter writer, RestrictionDB restrictions)
        {
            writer.Write(restrictions.Version);
            writer.Write(Convert.ToUInt32(restrictions.OpenStatus));
            writer.Write(restrictions.MonarchID.Full);
            writer.Write(restrictions.Table);
        }

        private static readonly ushort headerNumBuckets = 768;  // this # of buckets was sent over the wire in retail header
                                                                // however, this value ends up being unused, and the "real" # of buckets originates
                                                                // from a hardcoded value in g_bucketSizeArray in the client constant data

        private static readonly ushort actualNumBuckets = 89;   // in RestrictionDB constructor in acclient,
                                                                // client uses PHashTable for this (as opposed to the typical PackableHashTable)

                                                                // which inits an IntrusiveHashTable with size 64
                                                                // this gets bumped up to the next largest value in a hardcoded g_bucketSizeArray, which is 89

        private static readonly GuidComparer guidComparer = new GuidComparer(actualNumBuckets);

        public static void Write(this BinaryWriter writer, Dictionary<ObjectGuid, uint> db)
        {
            PackableHashTable.WriteHeader(writer, db.Count, headerNumBuckets);

            var sorted = new SortedDictionary<ObjectGuid, uint>(db, guidComparer);

            foreach (var kvp in sorted)
            {
                writer.Write(kvp.Key.Full);
                writer.Write(kvp.Value);
            }
        }
    }
}
