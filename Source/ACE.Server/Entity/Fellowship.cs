using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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

        public bool ShareXP;    // XP sharing: 0=no, 1=yes
        public bool ShareLoot;  // Loot sharing: 0=no, 1=yes
        public bool EvenShare;  // XP equal sharing: 0=proportional to level, 1=even
        public bool Open;       // Open fellowship: 0=no, 1=yes

        public Dictionary<uint, WeakReference<Player>> FellowshipMembers;
        public Dictionary<uint, WeakReference<Player>> ShareableMembers;

        /// <summary>
        /// Called when a player first creatures a Fellowship
        /// </summary>
        public Fellowship(Player leader, string fellowshipName, bool shareXP)
        {
            ShareXP = shareXP;

            // get loot sharing from leader's character options
            ShareLoot = leader.GetCharacterOption(CharacterOption.ShareFellowshipLoot);

            FellowshipLeaderGuid = leader.Guid.Full;
            FellowshipName = fellowshipName;
            EvenShare = false;

            FellowshipMembers = new Dictionary<uint, WeakReference<Player>>() { { leader.Guid.Full, new WeakReference<Player>(leader) } };
            ShareableMembers = new Dictionary<uint, WeakReference<Player>>() { { leader.Guid.Full, new WeakReference<Player>(leader) } };

            Open = false;
        }

        /// <summary>
        /// Called when a player clicks the 'add fellow' button
        /// </summary>
        public void AddFellowshipMember(Player inviter, Player newMember)
        {
            if (inviter == null || newMember == null)
                return;

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
                {
                    var confirm = new Confirmation(ConfirmationType.Fellowship, $"{inviter.Name} invites to you join a fellowship.", inviter, newMember);
                    ConfirmationManager.AddConfirmation(confirm);

                    newMember.Session.Network.EnqueueSend(new GameEventConfirmationRequest(newMember.Session, ConfirmationType.Fellowship,
                        confirm.ConfirmationID, confirm.Message));
                }
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
                        {
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat("You disband the fellowship", ChatMessageType.Fellowship));
                        }
                        else
                        {
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} disbanded the fellowship", ChatMessageType.Fellowship));
                            member.Fellowship = null;
                        }
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
                if (p != null && p.Fellowship.FellowshipLeaderGuid == FellowshipLeaderGuid)
                {
                    FellowshipLeaderGuid = p.Guid.Full;
                    SendMessageAndUpdate($"{newLeaderName} now leads the fellowship");
                }
                else if (FellowshipMembers.Count > 0)
                {
                    var fellowshipMembers = GetFellowshipMembers();

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

        /// <summary>
        /// Determines which fellows share XP, and how it is divied up
        /// Based on current player levels
        /// </summary>
        private void CalculateXPSharing()
        {
            BuildSharable();
            CalculateEvenSplit();
        }

        /// <summary>
        /// Returns the # of fellowship members who are at least level 50
        /// </summary>
        private int CountPlayerAbove()
        {
            var fellowshipMembers = GetFellowshipMembers();

            return fellowshipMembers.Values.Where(f => f.Level >= 50).Count();
        }

        /// <summary>
        /// Builds the list of fellowship members who can share XP
        /// </summary>
        private void BuildSharable()
        {
            // - If a member tries to join a fellowship who is < level 50, and is NOT within 10 levels of the founder, how is this handled?
            var fellowshipMembers = GetFellowshipMembers();

            if (CountPlayerAbove() != fellowshipMembers.Count)
            {
                var leader = PlayerManager.GetOnlinePlayer(FellowshipLeaderGuid);
                if (leader == null)
                    return;

                ShareableMembers = fellowshipMembers.Where(i => LevelDifference(leader, i.Value) <= 10 || (i.Value.Level ?? 1) >= 50).ToDictionary(i => i.Key, i => new WeakReference<Player>(i.Value));
            }
            else
                ShareableMembers = FellowshipMembers;
        }

        private static int LevelDifference(Player a, Player b)
        {
            if (a == null || b == null)
                return 0;

            return Math.Abs((a.Level ?? 1) - (b.Level ?? 1));
        }

        /// <summary>
        /// Determines if the fellowship uses Equal or Proportional XP sharing
        /// </summary>
        private void CalculateEvenSplit()
        {
            // XP sharing:

            // - If all members of the fellowship are level 50 or above, all members will share XP equally, and there will be no limit to the levels of the members involved.

            // - If all members of the fellowship are within 5 levels of the founder, XP will be shared equally.
            // - If members are all within ten levels of the founder, XP will be shared proportionally.

            var shareableMembers = GetShareableMembers();

            if (CountPlayerAbove() != shareableMembers.Count)
            {
                var leader = PlayerManager.GetOnlinePlayer(FellowshipLeaderGuid);
                if (leader == null)
                    return;

                foreach (var p in shareableMembers.Values)
                {
                    if (Math.Abs((leader.Level ?? 1) - (p.Level ?? 1)) > 5)
                    {
                        EvenShare = false;
                        return;
                    }
                }
            }
            EvenShare = true;
        }

        /// <summary>
        /// Splits XP amongst fellowship members, depending on XP type and fellow settings
        /// </summary>
        /// <param name="amount">The input amount of XP</param>
        /// <param name="xpType">The type of XP (quest XP is handled differently)</param>
        /// <param name="player">The fellowship member who originated the XP</param>
        public void SplitXp(ulong amount, XpType xpType, Player player)
        {
            var shareableMembers = GetShareableMembers();

            // handle sharing quest XP with fellows
            if (xpType == XpType.Quest)
            {
                foreach (var member in shareableMembers.Values)
                {
                    var fellowXpType = player == member ? XpType.Quest : XpType.Fellowship;

                    member.GrantXP((long)amount, fellowXpType, false);
                }
            }

            // divides XP evenly to all the sharable fellows within level range,
            // but with a significant boost to the amount of xp, based on # of fellowship members
            else if (EvenShare)
            {
                var totalAmount = (ulong)Math.Round(amount * GetMemberSharePercent());

                foreach (var member in shareableMembers.Values)
                {
                    var shareAmount = (ulong)Math.Round(totalAmount * GetDistanceScalar(player, member));

                    var fellowXpType = player == member ? xpType : XpType.Fellowship;

                    member.GrantXP((long)shareAmount, fellowXpType, false);
                }

                return;
            }

            // divides XP to all sharable fellows within level range
            // based on each fellowship member's level
            else
            {
                var levelSum = shareableMembers.Values.Select(p => p.Level ?? 1).Sum();

                foreach (var member in shareableMembers.Values)
                {
                    var levelScale = (float)(member.Level ?? 1) / levelSum;

                    var playerTotal = (ulong)Math.Round(amount * levelScale * GetDistanceScalar(player, member));

                    var fellowXpType = player == member ? xpType : XpType.Fellowship;

                    member.GrantXP((long)playerTotal, fellowXpType, false);
                }
            }
        }

        internal double GetMemberSharePercent()
        {
            var shareableMembers = GetShareableMembers();

            switch (shareableMembers.Count)
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
        /// based on distance from the leader
        /// </summary>
        private double GetDistanceScalar(Player earner, Player fellow)
        {
            if (earner == null || fellow == null)
                return 0.0f;

            var earnerPosition = earner.Location;
            var fellowPosition = fellow.Location;

            var dist = fellowPosition.Distance2D(earnerPosition);

            if (dist >= MaxDistance * 2.0f)
                return 0.0f;

            if (dist <= MaxDistance)
                return 1.0f;

            var scalar = 1 - (dist - MaxDistance) / MaxDistance;

            return Math.Max(0.0f, scalar);
        }

        /// <summary>
        /// Called when someone in the fellowship levels up
        /// </summary>
        public void OnFellowLevelUp()
        {
            CalculateXPSharing();
        }

        public void OnVitalUpdate(Player player)
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var fellow in fellowshipMembers.Values)
                fellow.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(fellow.Session, player, ShareXP, FellowUpdateType.Vitals));
        }

        public void OnDeath(Player player)
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var fellow in fellowshipMembers.Values)
                fellow.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your fellow {player.Name} has died!", ChatMessageType.Broadcast));
        }


        public Dictionary<uint, Player> GetFellowshipMembers()
        {
            return GetFellowPlayers(FellowshipMembers);
        }

        public Dictionary<uint, Player> GetShareableMembers()
        {
            return GetFellowPlayers(ShareableMembers);
        }

        public Dictionary<uint, Player> GetFellowPlayers(Dictionary<uint, WeakReference<Player>> fellowshipMembers)
        {
            var results = new Dictionary<uint, Player>();
            var dropped = new HashSet<uint>();

            foreach (var kvp in fellowshipMembers)
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
                ProcessDropList(fellowshipMembers, dropped);

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
            CalculateXPSharing();
            UpdateAllMembers();
        }
    }
}
