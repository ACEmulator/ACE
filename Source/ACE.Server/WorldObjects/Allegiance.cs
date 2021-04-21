using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

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
        // TODO now that the new biota model uses a dictionary for this, see if we can remove this duplicate dictionary
        public Dictionary<uint, PropertiesAllegiance> ApprovedVassals => Biota.PropertiesAllegiance.GetApprovedVassals(BiotaDatabaseLock);

        /// <summary>
        /// Handles booting players from allegiance chat
        /// </summary>
        public Dictionary<ObjectGuid, DateTime> ChatFilters { get; set; }

        /// <summary>
        /// A list of players who are banned from joining.
        /// </summary>
        //public HashSet<ObjectGuid> BanList { get; set; }
        // TODO now that the new biota model uses a dictionary for this, see if we can remove this duplicate dictionary
        public Dictionary<uint, PropertiesAllegiance> BanList => Biota.PropertiesAllegiance.GetBanList(BiotaDatabaseLock);

        /// <summary>
        /// Returns the list of allegiance members who are currently online
        /// </summary>
        public List<Player> OnlinePlayers
        {
            get
            {
                var onlinePlayers = new List<Player>();

                foreach (var member in Members)
                {
                    var player = PlayerManager.GetOnlinePlayer(member.Key);
                    if (player != null)
                        onlinePlayers.Add(player);
                }
                return onlinePlayers;
            }
        }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Allegiance(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            //Console.WriteLine($"Allegiance({weenie.ClassId}, {guid}): weenie constructor");

            InitializePropertyDictionaries();
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

            InitializePropertyDictionaries();
            Init(new ObjectGuid(MonarchId.Value));
        }

        public Allegiance(ObjectGuid monarch)
        {
            //Console.WriteLine($"Allegiance({monarch}): monarch constructor");

            Init(monarch);
        }

        private void InitializePropertyDictionaries()
        {
            if (Biota.PropertiesAllegiance == null)
                Biota.PropertiesAllegiance = new Dictionary<uint, PropertiesAllegiance>();
        }

        /// <summary>
        /// Constructs a new Allegiance from a Monarch
        /// </summary>
        public void Init(ObjectGuid monarch)
        {
            Monarch = new AllegianceNode(monarch, this);

            // find all players with this monarch
            var members = AllegianceManager.FindAllPlayers(monarch);

            var patronVassals = BuildPatronVassals(members);
            
            Monarch.BuildChain(this, members, patronVassals);
            BuildMembers(Monarch);

            //Console.WriteLine("TotalMembers: " + TotalMembers);
            BuildOfficers();

            ChatFilters = new Dictionary<ObjectGuid, DateTime>();
        }

        /// <summary>
        /// Build a mapping of patron guids => vassal guids
        /// </summary>
        public Dictionary<uint, List<IPlayer>> BuildPatronVassals(List<IPlayer> members)
        {
            var patronVassals = new Dictionary<uint, List<IPlayer>>();

            foreach (var member in members)
            {
                var patronId = member.PatronId;

                if (patronId == null)
                    continue;

                if (!patronVassals.TryGetValue(patronId.Value, out var vassals))
                {
                    vassals = new List<IPlayer>();
                    patronVassals.Add(patronId.Value, vassals);
                }
                vassals.Add(member);
            }
            return patronVassals;
        }

        /// <summary>
        /// Builds the lookup table of Players => AllegianceNodes
        /// </summary>
        public void BuildMembers(AllegianceNode node)
        {
            if (Monarch.PlayerGuid.Equals(node.PlayerGuid))
                Members = new Dictionary<ObjectGuid, AllegianceNode>();

            Members.Add(node.PlayerGuid, node);

            foreach (var vassal in node.Vassals.Values)
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
        /// Returns TRUE if playerGuid is a member
        /// </summary>
        public bool IsMember(ObjectGuid playerGuid)
        {
            return Members.ContainsKey(playerGuid);
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
                    return string.IsNullOrEmpty(AllegianceSpeakerTitle) ? "Speaker" : AllegianceSpeakerTitle;
                case AllegianceOfficerLevel.Seneschal:
                    return string.IsNullOrEmpty(AllegianceSeneschalTitle) ? "Seneschal" : AllegianceSeneschalTitle;
                case AllegianceOfficerLevel.Castellan:
                    return string.IsNullOrEmpty(AllegianceCastellanTitle) ? "Castellan" : AllegianceCastellanTitle;
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

        /// <summary>
        /// Updates any dynamic properties if they have changed for allegiance members
        /// </summary>
        public void UpdateProperties()
        {
            foreach (var member in Members)
            {
                var player = PlayerManager.FindByGuid(member.Key);
                var onlinePlayer = PlayerManager.GetOnlinePlayer(member.Key);

                if (player == null) continue;

                var updated = false;

                // if changed, update monarch id
                if ((player.MonarchId ?? 0) != member.Value.Allegiance.MonarchId)
                {
                    player.UpdateProperty(PropertyInstanceId.Monarch, member.Value.Allegiance.MonarchId, true);

                    updated = true;
                }

                // if changed, update rank
                if ((player.AllegianceRank ?? 0) != member.Value.Rank)
                {
                    player.AllegianceRank = (int)member.Value.Rank;

                    if (onlinePlayer != null)
                        onlinePlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(onlinePlayer, PropertyInt.AllegianceRank, player.AllegianceRank.Value));

                    updated = true;
                }

                if (updated)
                    player.SaveBiotaToDatabase();

                if (onlinePlayer != null)
                    onlinePlayer.Session.Network.EnqueueSend(new GameEventAllegianceUpdate(onlinePlayer.Session, this, member.Value), new GameEventAllegianceAllegianceUpdateDone(onlinePlayer.Session));
            }
        }

        public void ShowMembers()
        {
            Console.WriteLine($"Total members: {Members.Count}");

            foreach (var member in Members)
            {
                var player = PlayerManager.FindByGuid(member.Key, out bool isOnline);
                var prefix = isOnline ? "* " : "";

                Console.WriteLine($"{prefix}{player.Name}");
            }
        }

        public void ShowInfo()
        {
            Monarch.ShowInfo();
        }

        public void AddBan(uint playerGuid)
        {
            var entity = Biota.PropertiesAllegiance.GetFirstOrDefaultByCharacterId(playerGuid, BiotaDatabaseLock);

            if (entity == null)
                Biota.PropertiesAllegiance.AddOrUpdateAllegiance(playerGuid, true, false, BiotaDatabaseLock);
            else
                Biota.PropertiesAllegiance.AddOrUpdateAllegiance(playerGuid, true, entity.ApprovedVassal, BiotaDatabaseLock);

            // ChangesDetected = true doesn't work here,
            // since the Allegiance WO is not associated with a landblock

            SaveBiotaToDatabase();
        }

        public bool RemoveBan(uint playerGuid)
        {
            var entity = Biota.PropertiesAllegiance.GetFirstOrDefaultByCharacterId(playerGuid, BiotaDatabaseLock);

            if (entity == null)
                return false;

            if (entity.ApprovedVassal)
            {
                Biota.PropertiesAllegiance.AddOrUpdateAllegiance(playerGuid, false, true, BiotaDatabaseLock);
                SaveBiotaToDatabase();
                return true;
            }

            var removed = Biota.PropertiesAllegiance.TryRemoveAllegiance(playerGuid, BiotaDatabaseLock);

            if (removed)
                SaveBiotaToDatabase();

            return removed;
        }

        public bool IsBanned(uint playerGuid)
        {
            return BanList.ContainsKey(playerGuid);
        }

        public void AddApprovedVassal(uint playerGuid)
        {
            var entity = Biota.PropertiesAllegiance.GetFirstOrDefaultByCharacterId(playerGuid, BiotaDatabaseLock);

            if (entity == null)
                Biota.PropertiesAllegiance.AddOrUpdateAllegiance(playerGuid, false, true, BiotaDatabaseLock);
            else
                Biota.PropertiesAllegiance.AddOrUpdateAllegiance(playerGuid, entity.Banned, true, BiotaDatabaseLock);

            // ChangesDetected = true doesn't work here,
            // since the Allegiance WO is not associated with a landblock

            SaveBiotaToDatabase();
        }

        public bool RemoveApprovedVassal(uint playerGuid)
        {
            var entity = Biota.PropertiesAllegiance.GetFirstOrDefaultByCharacterId(playerGuid, BiotaDatabaseLock);

            if (entity == null)
                return false;

            if (entity.Banned)
            {
                Biota.PropertiesAllegiance.AddOrUpdateAllegiance(playerGuid, true, false, BiotaDatabaseLock);
                SaveBiotaToDatabase();
                return true;
            }

            var removed = Biota.PropertiesAllegiance.TryRemoveAllegiance(playerGuid, BiotaDatabaseLock);

            if (removed)
                SaveBiotaToDatabase();

            return removed;
        }

        public bool HasApprovedVassal(uint playerGuid)
        {
            return ApprovedVassals.ContainsKey(playerGuid);
        }
    }
}
