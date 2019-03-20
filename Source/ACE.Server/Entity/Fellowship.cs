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

namespace ACE.Server.Entity
{
    public class Fellowship
    {
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

        public List<Player> FellowshipMembers = new List<Player>(MaxFellows);
        public List<Player> SharableMembers = new List<Player>(MaxFellows);

        private Dictionary<uint, DateTime> oldFellows = new Dictionary<uint, DateTime>();
        
        public Fellowship(Player leader, string fellowshipName, bool shareXP)
        {
            ShareXP = shareXP;

            // get loot sharing from leader's character options
            ShareLoot = leader.GetCharacterOption(CharacterOption.ShareFellowshipLoot);

            FellowshipLeaderGuid = leader.Guid.Full;
            FellowshipName = fellowshipName;
            EvenShare = false;

            FellowshipMembers = new List<Player> { leader };
            SharableMembers = new List<Player> { leader };

            Open = false;
        }

        public void AddFellowshipMember(Player inviter, Player newMember)
        {
            if (FellowshipMembers.Count == MaxFellows)
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat("Fellowship is already full", ChatMessageType.Fellowship));
                return;
            }
            if (newMember.Fellowship != null)
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

        public void AddConfirmedMember(Player inviter, Player player, bool response)
        {
            if (response)
            {
                if (FellowshipMembers.Count == 9)
                {
                    inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} cannot join as fellowship is full", ChatMessageType.Fellowship));
                }
                else
                {
                    FellowshipMembers.Add(player);
                    CalculateXPSharing();
                    foreach (var member in FellowshipMembers)
                    {
                        inviter.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(inviter.Session, player, ShareXP));
                        //inviter.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(inviter.Session));
                    }
                    player.Fellowship = inviter.Fellowship;
                    SendMessageAndUpdate($"{player.Name} joined the fellowship");
                }
            }
            else
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} declines your invite", ChatMessageType.Fellowship));
            }
        }
        
        public void RemoveFellowshipMember(Player player)
        {
            foreach (var member in FellowshipMembers)
            {
                member.Session.Network.EnqueueSend(new GameEventFellowshipDismiss(member.Session, player));
                member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} dismissed from fellowship", ChatMessageType.Fellowship));
            }
            FellowshipMembers.Remove(player);
            player.Fellowship = null;
            CalculateXPSharing();
            UpdateAllMembers();
        }

        private void UpdateAllMembers()
        {
            foreach (var member in FellowshipMembers)
            {
                member.Session.Network.EnqueueSend(new GameEventFellowshipFullUpdate(member.Session));
                //member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
            }
        }

        private void SendMessageAndUpdate(string message)
        {
            foreach (var member in FellowshipMembers)
            {
                member.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Fellowship));
                member.Session.Network.EnqueueSend(new GameEventFellowshipFullUpdate(member.Session));
                //member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
            }
        }

        public void QuitFellowship(Player player, bool disband)
        {
            if (player.Guid.Full == FellowshipLeaderGuid)
            {
                if (disband)
                {
                    foreach (var member in FellowshipMembers)
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
                    FellowshipMembers.Remove(player);
                    oldFellows.TryAdd(player.Guid.Full, DateTime.Now);
                    player.Session.Network.EnqueueSend(new GameEventFellowshipQuit(player.Session, player.Guid.Full));
                    //member.Session.Network.EnqueueSend(new GameMessageFellowshipQuit(member.Session, player.Guid.Full));
                    AssignNewLeader(null);
                    CalculateXPSharing();
                    SendMessageAndUpdate($"{player.Name} left the fellowship");
                }
            }
            else
            {
                FellowshipMembers.Remove(player);
                oldFellows.TryAdd(player.Guid.Full, DateTime.Now);
                player.Session.Network.EnqueueSend(new GameEventFellowshipQuit(player.Session, player.Guid.Full));
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
                    Random rand = new Random();
                    int newLeaderIndex = rand.Next(FellowshipMembers.Count);
                    FellowshipLeaderGuid = FellowshipMembers[newLeaderIndex].Guid.Full;
                    newLeaderName = FellowshipMembers[newLeaderIndex].Name;
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
            return FellowshipMembers.Where(f => f.Level >= 50).Count();
        }

        /// <summary>
        /// Builds the list of fellowship members who can share XP
        /// </summary>
        private void BuildSharable()
        {
            // - If a member tries to join a fellowship who is < level 50, and is NOT within 10 levels of the founder, how is this handled?

            if (CountPlayerAbove() != FellowshipMembers.Count)
            {
                var leader = PlayerManager.GetOnlinePlayer(FellowshipLeaderGuid);
                SharableMembers = FellowshipMembers.Where(fellow => LevelDifference(leader, fellow) <= 10 || (fellow.Level ?? 1) >= 50).ToList();
            }
            else
                SharableMembers = FellowshipMembers;
        }

        private static int LevelDifference(Player a, Player b)
        {
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

            if (CountPlayerAbove() != SharableMembers.Count)
            {
                var leader = PlayerManager.GetOnlinePlayer(FellowshipLeaderGuid);
                foreach (Player p in SharableMembers)
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
            // handle sharing quest XP with fellows
            if (xpType == XpType.Quest)
            {
                foreach (var member in SharableMembers)
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

                foreach (var member in SharableMembers)
                {
                    var shareAmount = (ulong)Math.Round(totalAmount * GetDistanceScalar(member));

                    var fellowXpType = player == member ? xpType : XpType.Fellowship;

                    member.GrantXP((long)shareAmount, fellowXpType, false);
                }

                return;
            }

            // divides XP to all sharable fellows within level range
            // based on each fellowship member's level
            else
            {
                var levelSum = SharableMembers.Select(p => p.Level ?? 1).Sum();

                foreach (var member in SharableMembers)
                {
                    var levelScale = (float)(member.Level ?? 1) / levelSum;

                    var playerTotal = (ulong)Math.Round(amount * levelScale * GetDistanceScalar(member));

                    var fellowXpType = player == member ? xpType : XpType.Fellowship;

                    member.GrantXP((long)playerTotal, fellowXpType, false);
                }
            }
        }

        internal double GetMemberSharePercent()
        {
            switch (SharableMembers.Count)
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
        private double GetDistanceScalar(Player player)
        {
            Position leaderPosition = PlayerManager.GetOnlinePlayer(FellowshipLeaderGuid).Location;
            Position memberPosition = player.Location;

            var dist = memberPosition.Distance2D(leaderPosition);

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
            foreach (var fellow in FellowshipMembers)
                fellow.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(fellow.Session, player, ShareXP, FellowUpdateType.Vitals));
        }

        public void OnDeath(Player player)
        {
            foreach (var fellow in FellowshipMembers)
            {
                if (fellow != player)
                    fellow.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your fellow {player.Name} has died!", ChatMessageType.Broadcast));
            }
        }
    }
}
