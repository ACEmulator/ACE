using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

using log4net;

namespace ACE.Server.Entity
{
    public class Fellowship
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The maximum # of fellowship members
        /// </summary>
        public static int MaxFellows = 9;

        public string FellowshipName;
        public uint FellowshipLeaderGuid;

        public bool DesiredShareXP;     // determined by the leader's 'ShareFellowshipExpAndLuminance' client option when fellowship is created
        public bool ShareLoot;          // determined by the leader's 'ShareFellowshipLoot' client option when fellowship is created

        public bool ShareXP;            // whether or not XP sharing is currently enabled, as determined by DesiredShareXP && level restrictions
        public bool EvenShare;          // true if all fellows are >= level 50, or all fellows are within 5 levels of the leader

        public bool Open;               // indicates if non-leaders can invite new fellowship members
        public bool IsLocked;           // only set through emotes. if a fellowship is locked, new fellowship members cannot be added

        public Dictionary<uint, WeakReference<Player>> FellowshipMembers;
        public Dictionary<uint, WeakReference<Player>> LockedMembers;

        // todo: fellows departed
        // if fellowship locked, and one of the fellows disconnects and reconnects,
        // they can rejoin the fellowship within a certain amount of time

        public QuestManager QuestManager;

        /// <summary>
        /// Called when a player first creates a Fellowship
        /// </summary>
        public Fellowship(Player leader, string fellowshipName, bool shareXP)
        {
            DesiredShareXP = shareXP;
            ShareXP = shareXP;

            // get loot sharing from leader's character options
            ShareLoot = leader.GetCharacterOption(CharacterOption.ShareFellowshipLoot);

            FellowshipLeaderGuid = leader.Guid.Full;
            FellowshipName = fellowshipName;
            EvenShare = false;

            FellowshipMembers = new Dictionary<uint, WeakReference<Player>>() { { leader.Guid.Full, new WeakReference<Player>(leader) } };

            Open = false;

            QuestManager = new QuestManager(this);
            IsLocked = false;
            LockedMembers = new Dictionary<uint, WeakReference<Player>>();
        }

        /// <summary>
        /// Called when a player clicks the 'add fellow' button
        /// </summary>
        public void AddFellowshipMember(Player inviter, Player newMember)
        {
            if (inviter == null || newMember == null)
                return;

            if (IsLocked && !LockedMembers.ContainsKey(newMember.Guid.Full))
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat("Fellowship is locked", ChatMessageType.Fellowship));
                return;
            }

            if (FellowshipMembers.Count == MaxFellows)
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat("Fellowship is already full", ChatMessageType.Fellowship));
                return;
            }

            if (newMember.Fellowship != null || FellowshipMembers.ContainsKey(newMember.Guid.Full))
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{newMember.Name} is already in a fellowship", ChatMessageType.Fellowship));
            }
            else
            {
                if (newMember.GetCharacterOption(CharacterOption.AutomaticallyAcceptFellowshipRequests))
                {
                    AddConfirmedMember(inviter, newMember, true);
                }
                else
                    newMember.ConfirmationManager.EnqueueSend(new Confirmation_Fellowship(inviter.Guid, newMember.Guid), inviter.Name);
            }
        }

        /// <summary>
        /// Finalizes the process of adding a player to the fellowship
        /// If the player doesn't have the 'automatically accept fellowship requests' option set,
        /// this would be after they responded to the popup window
        /// </summary>
        public void AddConfirmedMember(Player inviter, Player player, bool response)
        {
            if (inviter == null || inviter.Session == null || inviter.Session.Player == null || player == null) return;

            if (!response)
            {
                // player clicked 'no' on the fellowship popup
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} declines your invite", ChatMessageType.Fellowship));
                return;
            }

            if (FellowshipMembers.Count == 9)
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} cannot join as fellowship is full", ChatMessageType.Fellowship));
                return;
            }

            FellowshipMembers.TryAdd(player.Guid.Full, new WeakReference<Player>(player));
            player.Fellowship = inviter.Fellowship;

            CalculateXPSharing();

            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values.Where(i => i.Guid != player.Guid))
                member.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(member.Session, player, ShareXP));

            SendMessageAndUpdate($"{player.Name} joined the fellowship");
        }

        public void RemoveFellowshipMember(Player player)
        {
            if (player == null) return;

            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values)
            {
                member.Session.Network.EnqueueSend(new GameEventFellowshipDismiss(member.Session, player));
                member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} dismissed from fellowship", ChatMessageType.Fellowship));
            }

            FellowshipMembers.Remove(player.Guid.Full);
            player.Fellowship = null;

            CalculateXPSharing();

            UpdateAllMembers();
        }

        private void UpdateAllMembers()
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values)
                member.Session.Network.EnqueueSend(new GameEventFellowshipFullUpdate(member.Session));
        }

        private void SendMessageAndUpdate(string message)
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values)
            {
                member.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Fellowship));

                member.Session.Network.EnqueueSend(new GameEventFellowshipFullUpdate(member.Session));
            }
        }

        public void QuitFellowship(Player player, bool disband)
        {
            if (player == null) return;

            if (player.Guid.Full == FellowshipLeaderGuid)
            {
                if (disband)
                {
                    var fellowshipMembers = GetFellowshipMembers();

                    foreach (var member in fellowshipMembers.Values)
                    {
                        member.Session.Network.EnqueueSend(new GameEventFellowshipQuit(member.Session, member.Guid.Full));

                        if (member.Guid.Full == FellowshipLeaderGuid)
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat("You disband the fellowship", ChatMessageType.Fellowship));
                        else
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} disbanded the fellowship", ChatMessageType.Fellowship));

                        member.Fellowship = null;
                    }
                }
                else
                {
                    FellowshipMembers.Remove(player.Guid.Full);
                    player.Fellowship = null;
                    player.Session.Network.EnqueueSend(new GameEventFellowshipQuit(player.Session, player.Guid.Full));
                    AssignNewLeader(null);
                    CalculateXPSharing();
                    SendMessageAndUpdate($"{player.Name} left the fellowship");
                }
            }
            else if (!disband)
            {
                FellowshipMembers.Remove(player.Guid.Full);
                player.Session.Network.EnqueueSend(new GameEventFellowshipQuit(player.Session, player.Guid.Full));
                player.Fellowship = null;
                CalculateXPSharing();
                SendMessageAndUpdate($"{player.Name} left the fellowship");
            }
        }

        public void AssignNewLeader(Player p)
        {
            string newLeaderName = string.Empty;
            if (p != null)
            {
                FellowshipLeaderGuid = p.Guid.Full;
                newLeaderName = p.Name;
                SendMessageAndUpdate($"{newLeaderName} now leads the fellowship");

            }
            else
            {
                var fellowshipMembers = GetFellowshipMembers();

                if (fellowshipMembers.Count > 0)
                {
                    int newLeaderIndex = ThreadSafeRandom.Next(0, fellowshipMembers.Count - 1);
                    var fellowGuids = fellowshipMembers.Keys.ToList();
                    FellowshipLeaderGuid = fellowGuids[newLeaderIndex];
                    newLeaderName = fellowshipMembers[FellowshipLeaderGuid].Name;
                    SendMessageAndUpdate($"{newLeaderName} now leads the fellowship");
                }
            }
        }

        public void UpdateOpenness(bool isOpen)
        {
            Open = isOpen;
            string openness = Open ? "open" : "closed";
            SendMessageAndUpdate($"Fellowship is now {openness}");
        }

        public void UpdateLock(bool isLocked)
        {
            IsLocked = isLocked;
            string lockedness = IsLocked ? "locked" : "unlocked";
            SendMessageAndUpdate($"Fellowship is now {lockedness}");

            if (isLocked)
            {
                foreach (var fellow in GetFellowshipMembers().Values)
                {
                    LockedMembers.TryAdd(fellow.Guid.Full, new WeakReference<Player>(fellow));
                }
            }
            else
                LockedMembers.Clear();
        }

        /// <summary>
        /// Calculates fellowship XP sharing (ShareXP, EvenShare) from fellow levels
        /// </summary>
        private void CalculateXPSharing()
        {
            // - If all members of the fellowship are level 50 or above, all members will share XP equally

            // - If all members of the fellowship are within 5 levels of the founder, XP will be shared equally

            // - If members are all within ten levels of the founder, XP will be shared proportionally.

            var fellows = GetFellowshipMembers();

            var allOver50 = !fellows.Values.Any(f => (f.Level ?? 1) < 50);

            if (allOver50)
            {
                ShareXP = DesiredShareXP;
                EvenShare = true;
                return;
            }

            var leader = PlayerManager.GetOnlinePlayer(FellowshipLeaderGuid);
            if (leader == null)
                return;

            var maxLevelDiff = fellows.Values.Max(f => Math.Abs((leader.Level ?? 1) - (f.Level ?? 1)));

            if (maxLevelDiff <= 5)
            {
                ShareXP = DesiredShareXP;
                EvenShare = true;
            }
            else if (maxLevelDiff <= 10)
            {
                ShareXP = DesiredShareXP;
                EvenShare = false;
            }
            else
            {
                ShareXP = false;
                EvenShare = false;
            }
        }

        /// <summary>
        /// Splits XP amongst fellowship members, depending on XP type and fellow settings
        /// </summary>
        /// <param name="amount">The input amount of XP</param>
        /// <param name="xpType">The type of XP (quest XP is handled differently)</param>
        /// <param name="player">The fellowship member who originated the XP</param>
        public void SplitXp(ulong amount, XpType xpType, ShareType shareType, Player player)
        {
            // https://asheron.fandom.com/wiki/Announcements_-_2002/02_-_Fever_Dreams#Letter_to_the_Players_1

            var fellowshipMembers = GetFellowshipMembers();

            shareType &= ~ShareType.Fellowship;

            // quest turn-ins: flat share (retail default)
            if (xpType == XpType.Quest && !PropertyManager.GetBool("fellow_quest_bonus").Item)
            {
                var perAmount = (long)amount / fellowshipMembers.Count;

                foreach (var member in fellowshipMembers.Values)
                {
                    var fellowXpType = player == member ? XpType.Quest : XpType.Fellowship;

                    member.GrantXP(perAmount, fellowXpType, shareType);
                }
            }

            // divides XP evenly to all the sharable fellows within level range,
            // but with a significant boost to the amount of xp, based on # of fellowship members
            else if (EvenShare)
            {
                var totalAmount = (ulong)Math.Round(amount * GetMemberSharePercent());

                foreach (var member in fellowshipMembers.Values)
                {
                    var shareAmount = (ulong)Math.Round(totalAmount * GetDistanceScalar(player, member, xpType));

                    var fellowXpType = player == member ? xpType : XpType.Fellowship;

                    member.GrantXP((long)shareAmount, fellowXpType, shareType);
                }

                return;
            }

            // divides XP to all sharable fellows within level range
            // based on each fellowship member's level
            else
            {
                var levelXPSum = fellowshipMembers.Values.Select(p => p.GetXPToNextLevel(p.Level.Value)).Sum();

                foreach (var member in fellowshipMembers.Values)
                {
                    var levelXPScale = (double)member.GetXPToNextLevel(member.Level.Value) / levelXPSum;

                    var playerTotal = (ulong)Math.Round(amount * levelXPScale * GetDistanceScalar(player, member, xpType));

                    var fellowXpType = player == member ? xpType : XpType.Fellowship;

                    member.GrantXP((long)playerTotal, fellowXpType, shareType);
                }
            }
        }

        /// <summary>
        /// Splits luminance amongst fellowship members, depending on XP type and fellow settings
        /// </summary>
        /// <param name="amount">The input amount of luminance</param>
        /// <param name="xpType">The type of lumaniance (quest luminance is handled differently)</param>
        /// <param name="player">The fellowship member who originated the luminance</param>
        public void SplitLuminance(ulong amount, XpType xpType, ShareType shareType, Player player)
        {
            // https://asheron.fandom.com/wiki/Announcements_-_2002/02_-_Fever_Dreams#Letter_to_the_Players_1

            shareType &= ~ShareType.Fellowship;

            if (xpType == XpType.Quest)
            {
                // quest luminance is not shared
                player.GrantLuminance((long)amount, XpType.Quest, shareType);
            }
            else
            {
                // pre-filter: evenly divide between luminance-eligible fellows
                var shareableMembers = GetFellowshipMembers().Values.Where(f => f.MaximumLuminance != null).ToList();

                if (shareableMembers.Count == 0)
                    return;

                var perAmount = (long)Math.Round((double)(amount / (ulong)shareableMembers.Count));

                // further filter to fellows in radar range
                var inRange = shareableMembers.Intersect(WithinRange(player, true)).ToList();

                foreach (var member in inRange)
                {
                    var fellowXpType = player == member ? xpType : XpType.Fellowship;

                    member.GrantLuminance(perAmount, fellowXpType, shareType);
                }
            }
        }

        internal double GetMemberSharePercent()
        {
            var fellowshipMembers = GetFellowshipMembers();

            switch (fellowshipMembers.Count)
            {
                case 1:
                    return 1.0;
                case 2:
                    return .75;
                case 3:
                    return .6;
                case 4:
                    return .55;
                case 5:
                    return .5;
                case 6:
                    return .45;
                case 7:
                    return .4;
                case 8:
                    return .35;
                case 9:
                    return .3;
                    // TODO: handle fellowship mods with > 9 players?
            }
            return 1.0;
        }

        public static readonly int MaxDistance = 600;

        /// <summary>
        /// Returns the amount to scale the XP for a fellow
        /// based on distance from the earner
        /// </summary>
        public double GetDistanceScalar(Player earner, Player fellow, XpType xpType)
        {
            if (earner == null || fellow == null)
                return 0.0f;

            if (xpType == XpType.Quest)
                return 1.0f;

            // https://asheron.fandom.com/wiki/Announcements_-_2004/01_-_Mirror,_Mirror#Rollout_Article

            // If they are indoors while you are outdoors, or vice-versa.
            if (earner.Location.Indoors != fellow.Location.Indoors)
                return 0.0f;

            // If you are both indoors but in different landblocks.
            if (earner.Location.Indoors && fellow.Location.Indoors && earner.Location.Landblock != fellow.Location.Landblock)
                return 0.0f;

            var dist = earner.Location.Distance2D(fellow.Location);

            if (dist >= MaxDistance * 2.0f)
                return 0.0f;

            if (dist <= MaxDistance)
                return 1.0f;

            var scalar = 1.0f - (dist - MaxDistance) / MaxDistance;

            return Math.Max(0.0f, scalar);
        }

        /// <summary>
        /// Returns fellows within radar range (75 units outdoors, 25 units indoors)
        /// </summary>
        public List<Player> WithinRange(Player player, bool includeSelf = false)
        {
            var fellows = GetFellowshipMembers();

            var landblockRange = PropertyManager.GetBool("fellow_kt_landblock").Item;

            var results = new List<Player>();

            foreach (var fellow in fellows.Values)
            {
                if (player == fellow && !includeSelf)
                    continue;

                var shareable = player == fellow || landblockRange ?
                    player.CurrentLandblock == fellow.CurrentLandblock || player.Location.DistanceTo(fellow.Location) <= 192.0f :
                    player.Location.Distance2D(fellow.Location) <= player.CurrentRadarRange && player.ObjMaint.VisibleObjectsContainsKey(fellow.Guid.Full);      // 2d visible distance / radar range?

                if (shareable)
                    results.Add(fellow);
            }
            return results;
        }

        /// <summary>
        /// Called when someone in the fellowship levels up
        /// </summary>
        public void OnFellowLevelUp(Player player)
        {
            CalculateXPSharing();

            var fellowshipMembers = GetFellowshipMembers();

            foreach (var fellow in fellowshipMembers.Values)
            {
                if (fellow == player)
                    continue;

                fellow.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is now level {player.Level}!", ChatMessageType.Broadcast));
            }
        }

        public void OnVitalUpdate(Player player)
        {
            // cap max update interval?

            var fellowshipMembers = GetFellowshipMembers();

            foreach (var fellow in fellowshipMembers.Values)
                fellow.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(fellow.Session, player, ShareLoot, FellowUpdateType.Vitals));
        }

        public void OnDeath(Player player)
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var fellow in fellowshipMembers.Values)
            {
                if (fellow != player)
                    fellow.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your fellow {player.Name} has died!", ChatMessageType.Broadcast));
            }
        }

        public Dictionary<uint, Player> GetFellowshipMembers()
        {
            var results = new Dictionary<uint, Player>();
            var dropped = new HashSet<uint>();

            foreach (var kvp in FellowshipMembers)
            {
                var playerGuid = kvp.Key;
                var playerRef = kvp.Value;

                playerRef.TryGetTarget(out var player);

                if (player != null && player.Session != null && player.Session.Player != null && player.Fellowship != null)
                    results.Add(playerGuid, player);
                else
                    dropped.Add(playerGuid);
            }

            // TODO: process dropped list
            if (dropped.Count > 0)
                ProcessDropList(FellowshipMembers, dropped);

            return results;
        }

        public void ProcessDropList(Dictionary<uint, WeakReference<Player>> fellowshipMembers, HashSet<uint> fellowGuids)
        {
            foreach (var fellowGuid in fellowGuids)
            {
                var offlinePlayer = PlayerManager.FindByGuid(fellowGuid);
                var offlineName = offlinePlayer != null ? offlinePlayer.Name : "NULL";

                log.Warn($"Dropped fellow: {offlineName}");
                fellowshipMembers.Remove(fellowGuid);
            }
            if (fellowGuids.Contains(FellowshipLeaderGuid))
                AssignNewLeader(null);

            CalculateXPSharing();
            UpdateAllMembers();
        }
    }
}
