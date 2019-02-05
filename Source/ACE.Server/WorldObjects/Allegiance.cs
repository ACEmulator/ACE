using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    public class Allegiance: WorldObject
    {
        /// <summary>
        /// The top of the AllegianceNode tree
        /// </summary>
        public AllegianceNode Monarch;

        /// <summary>
        /// The total # of players in the Allegiance
        /// </summary>
        public int TotalMembers => Members.Count;

        /// <summary>
        /// A lookup table of Players => AllegianceNodes
        /// </summary>
        public Dictionary<ObjectGuid, AllegianceNode> Members;

        public Dictionary<ObjectGuid, AllegianceNode> Officers;

        /// <summary>
        /// Approved vassals for adding to locked allegiances
        /// </summary>
        public HashSet<ObjectGuid> ApprovedVassals;

        /// <summary>
        /// Handles booting players from allegiance chat
        /// </summary>
        public Dictionary<ObjectGuid, DateTime> ChatFilters;

        /// <summary>
        /// A list of players who are banned from joining.
        /// </summary>
        public HashSet<ObjectGuid> BanList;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Allegiance(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            //Console.WriteLine($"Allegiance({weenie.ClassId}, {guid}): weenie constructor");
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Allegiance(Biota biota) : base(biota)
        {
            //Console.WriteLine($"Allegiance({biota.Id:X8}): biota constructor");

            if (MonarchId == null)
            {
                Console.WriteLine($"Allegiance({biota.Id:X8}): constructor called with no monarch");
                return;
            }

            Init(new ObjectGuid(MonarchId.Value));
        }

        public Allegiance(ObjectGuid monarch)
        {
            //Console.WriteLine($"Allegiance({monarch}): monarch constructor");

            Init(monarch);
        }

        /// <summary>
        /// Constructs a new Allegiance from a Monarch
        /// </summary>
        public void Init(ObjectGuid monarch)
        {
            Monarch = new AllegianceNode(monarch, this);

            // find all players with this monarch
            var members = AllegianceManager.FindAllPlayers(monarch);

            Monarch.BuildChain(this, members);
            BuildMembers(Monarch);

            //Console.WriteLine("TotalMembers: " + TotalMembers);
            BuildOfficers();

            ApprovedVassals = new HashSet<ObjectGuid>();

            BanList = new HashSet<ObjectGuid>();

            ChatFilters = new Dictionary<ObjectGuid, DateTime>();
        }

        /// <summary>
        /// Builds the lookup table of Players => AllegianceNodes
        /// </summary>
        public void BuildMembers(AllegianceNode node)
        {
            if (Monarch.PlayerGuid.Equals(node.PlayerGuid))
                Members = new Dictionary<ObjectGuid, AllegianceNode>();

            Members.Add(node.PlayerGuid, node);

            foreach (var vassal in node.Vassals)
                BuildMembers(vassal);
        }

        public void BuildOfficers()
        {
            Officers = Members.Where(i => i.Value.Player.AllegianceOfficerRank != null).ToDictionary(i => i.Key, i => i.Value);
        }

        /// <summary>
        /// An allegiance is defined by its monarch
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Allegiance allegiance)
                return Monarch.PlayerGuid.Full == allegiance.Monarch.PlayerGuid.Full;

            return false;
        }

        public override int GetHashCode()
        {
            return Monarch.PlayerGuid.Full.GetHashCode();
        }

        public string AllegianceName
        {
            get => GetProperty(PropertyString.AllegianceName);
            set { if (value == null) RemoveProperty(PropertyString.AllegianceName); else SetProperty(PropertyString.AllegianceName, value); }
        }

        public string AllegianceMotd
        {
            get => GetProperty(PropertyString.AllegianceMotd);
            set { if (value == null) RemoveProperty(PropertyString.AllegianceMotd); else SetProperty(PropertyString.AllegianceMotd, value); }
        }

        public string AllegianceMotdSetBy
        {
            get => GetProperty(PropertyString.AllegianceMotdSetBy);
            set { if (value == null) RemoveProperty(PropertyString.AllegianceMotdSetBy); else SetProperty(PropertyString.AllegianceMotdSetBy, value); }
        }

        public string AllegianceSpeakerTitle
        {
            get => GetProperty(PropertyString.AllegianceSpeakerTitle);
            set { if (value == null) RemoveProperty(PropertyString.AllegianceSpeakerTitle); else SetProperty(PropertyString.AllegianceSpeakerTitle, value); }
        }

        public string AllegianceSeneschalTitle
        {
            get => GetProperty(PropertyString.AllegianceSeneschalTitle);
            set { if (value == null) RemoveProperty(PropertyString.AllegianceSeneschalTitle); else SetProperty(PropertyString.AllegianceSeneschalTitle, value); }
        }

        public string AllegianceCastellanTitle
        {
            get => GetProperty(PropertyString.AllegianceCastellanTitle);
            set { if (value == null) RemoveProperty(PropertyString.AllegianceCastellanTitle); else SetProperty(PropertyString.AllegianceCastellanTitle, value); }
        }

        /// <summary>
        /// Returns TRUE if playerGuid is an officer
        /// </summary>
        public bool IsOfficer(ObjectGuid playerGuid)
        {
            return Officers.ContainsKey(playerGuid);
        }

        /// <summary>
        /// Returns TRUE if playerGuid is an officer of minimum rank
        /// </summary>
        public bool IsOfficerRank(ObjectGuid playerGuid, int officerRank)
        {
            Officers.TryGetValue(playerGuid, out var node);
            return node != null && node.Player.AllegianceOfficerRank >= officerRank;
        }

        public bool IsSpeaker(ObjectGuid playerGuid)
        {
            return IsOfficerRank(playerGuid, 1);
        }

        public bool IsSeneschal(ObjectGuid playerGuid)
        {
            return IsOfficerRank(playerGuid, 2);
        }

        public bool IsCastellan(ObjectGuid playerGuid)
        {
            return IsOfficerRank(playerGuid, 3);
        }

        public string GetOfficerTitle(AllegianceOfficerLevel officerRank)
        {
            switch (officerRank)
            {
                case AllegianceOfficerLevel.Speaker:
                    return AllegianceSpeakerTitle ?? "Speaker";
                case AllegianceOfficerLevel.Seneschal:
                    return AllegianceSeneschalTitle ?? "Seneschal";
                case AllegianceOfficerLevel.Castellan:
                    return AllegianceCastellanTitle ?? "Castellan";
                default:
                    return "";
            }
        }

        public bool HasCustomTitles => AllegianceSpeakerTitle != null || AllegianceSeneschalTitle != null || AllegianceCastellanTitle != null;

        public House GetHouse()
        {
            if (Monarch.Player.HouseInstance == null)
                return null;

            // is landblock loaded?
            var houseGuid = Monarch.Player.HouseInstance.Value;
            var landblock = (ushort)((houseGuid >> 12) & 0xFFFF);

            var landblockId = new LandblockId((uint)(landblock << 16 | 0xFFFF));
            var isLoaded = LandblockManager.IsLoaded(landblockId);

            if (isLoaded)
            {
                var loaded = LandblockManager.GetLandblock(landblockId, false);
                return loaded.GetObject(new ObjectGuid(houseGuid)) as House;
            }

            // load an offline copy
            return House.Load(Monarch.Player.HouseInstance.Value);
        }

        /// <summary>
        /// Returns TRUE if input player guid has an active chat filter
        /// </summary>
        public bool IsFiltered(ObjectGuid playerGuid)
        {
            if (!ChatFilters.TryGetValue(playerGuid, out var filter))
                return false;

            if (filter > DateTime.UtcNow)
                return true;

            // filter has expired
            ChatFilters.Remove(playerGuid);

            return false;
        }
    }
}
