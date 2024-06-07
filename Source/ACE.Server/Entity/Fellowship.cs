using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
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
        //public static int MaxFellows = 9;
        public static int MaxFellows = 21;

        public string FellowshipName;
        public uint FellowshipLeaderGuid;

        public bool DesiredShareXP;     // determined by the leader's 'ShareFellowshipExpAndLuminance' client option when fellowship is created
        public bool ShareLoot;          // determined by the leader's 'ShareFellowshipLoot' client option when fellowship is created

        public bool ShareXP;            // whether or not XP sharing is currently enabled, as determined by DesiredShareXP && level restrictions
        public bool EvenShare;          // true if all fellows are >= level 50, or all fellows are within 5 levels of the leader

        public bool Open;               // indicates if non-leaders can invite new fellowship members
        public bool IsLocked;           // only set through emotes. if a fellowship is locked, new fellowship members cannot be added

        public Dictionary<uint, WeakReference<Player>> FellowshipMembers;

        public Dictionary<uint, int> DepartedMembers;

        public Dictionary<string, FellowshipLockData> FellowshipLocks;

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
            DepartedMembers = new Dictionary<uint, int>();
            FellowshipLocks = new Dictionary<string, FellowshipLockData>();
        }

        /// <summary>
        /// Called when a player clicks the 'add fellow' button
        /// </summary>
        public void AddFellowshipMember(Player inviter, Player newMember)
        {
            if (inviter == null || newMember == null)
                return;

            if (IsLocked)
            {

                if (!DepartedMembers.TryGetValue(newMember.Guid.Full, out var timeDeparted))
                {
                    inviter.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(inviter.Session, WeenieErrorWithString.LockedFellowshipCannotRecruit_, newMember.Name));
                    //newMember.SendWeenieError(WeenieError.LockedFellowshipCannotRecruitYou);
                    return;
                }
                else
                {
                    var timeLimit = Time.GetDateTimeFromTimestamp(timeDeparted).AddSeconds(600);
                    if (DateTime.UtcNow > timeLimit)
                    {
                        inviter.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(inviter.Session, WeenieErrorWithString.LockedFellowshipCannotRecruit_, newMember.Name));
                        //newMember.SendWeenieError(WeenieError.LockedFellowshipCannotRecruitYou);
                        return;
                    }
                }
            }

            if (FellowshipMembers.Count >= MaxFellows)
            {
                inviter.Session.Network.EnqueueSend(new GameEventWeenieError(inviter.Session, WeenieError.YourFellowshipIsFull));
                return;
            }

            if (newMember.Fellowship != null || FellowshipMembers.ContainsKey(newMember.Guid.Full))
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{newMember.Name} is already a member of a Fellowship.", ChatMessageType.Broadcast));
            }
            else
            {
                if (PropertyManager.GetBool("fellow_busy_no_recruit").Item && newMember.IsBusy)
                {
                    inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{newMember.Name} is busy.", ChatMessageType.Broadcast));
                    return;
                }

                if (newMember.GetCharacterOption(CharacterOption.AutomaticallyAcceptFellowshipRequests))
                {
                    AddConfirmedMember(inviter, newMember, true);
                }
                else
                {
                    if (!newMember.ConfirmationManager.EnqueueSend(new Confirmation_Fellowship(inviter.Guid, newMember.Guid), inviter.Name))
                    {
                        inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{newMember.Name} is busy.", ChatMessageType.Broadcast));
                    }
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
                inviter.Session.Network.EnqueueSend(new GameEventWeenieError(inviter.Session, WeenieError.FellowshipDeclined));
                return;
            }

            if (FellowshipMembers.Count >= MaxFellows)
            {
                inviter.Session.Network.EnqueueSend(new GameEventWeenieError(inviter.Session, WeenieError.YourFellowshipIsFull));
                return;
            }

            FellowshipMembers.TryAdd(player.Guid.Full, new WeakReference<Player>(player));
            player.Fellowship = inviter.Fellowship;

            CalculateXPSharing();

            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values.Where(i => i.Guid != player.Guid))
                member.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(member.Session, player, ShareXP));

            if (ShareLoot)
            {
                foreach (var member in fellowshipMembers.Values.Where(i => i.Guid != player.Guid))
                {
                    member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has given you permission to loot his or her kills.", ChatMessageType.Broadcast));
                    member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} may now loot your kills.", ChatMessageType.Broadcast));

                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{member.Name} has given you permission to loot his or her kills.", ChatMessageType.Broadcast));
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{member.Name} may now loot your kills.", ChatMessageType.Broadcast));
                }
            }

            UpdateAllMembers();

            if (inviter.CurrentMotionState.Stance == MotionStance.NonCombat) // only do this motion if inviter is at peace, other times motion is skipped. 
                inviter.SendMotionAsCommands(MotionCommand.BowDeep, MotionStance.NonCombat);
        }

        public void RemoveFellowshipMember(Player player, Player leader)
        {
            if (player == null) return;

            var fellowshipMembers = GetFellowshipMembers();

            if (!fellowshipMembers.ContainsKey(player.Guid.Full))
            {
                log.Warn($"{leader.Name} tried to dismiss {player.Name} from the fellowship, but {player.Name} was not found in the fellowship");

                var done = true;

                if (player.Fellowship != null)
                {
                    if (player.Fellowship == this)
                    {
                        log.Warn($"{player.Name} still has a reference to this fellowship somehow. This shouldn't happen");
                        done = false;
                    }
                    else
                        log.Warn($"{player.Name} has a reference to a different fellowship. {leader.Name} is possibly sending crafted data!");
                }

                if (done) return;
            }

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

        private void SendBroadcastAndUpdate(string message)
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values)
            {
                member.Session.Network.EnqueueSend(new GameEventChannelBroadcast(member.Session, Channel.FellowBroadcast, "", message));

                member.Session.Network.EnqueueSend(new GameEventFellowshipFullUpdate(member.Session));
            }
        }

        public void BroadcastToFellow(string message)
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values)
                member.Session.Network.EnqueueSend(new GameEventChannelBroadcast(member.Session, Channel.FellowBroadcast, "", message));
        }

        public void TellFellow(WorldObject sender, string message)
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values)
                member.Session.Network.EnqueueSend(new GameEventChannelBroadcast(member.Session, Channel.Fellow, sender.Name, message));
        }

        private void SendWeenieErrorWithStringAndUpdate(WeenieErrorWithString error, string message)
        {
            var fellowshipMembers = GetFellowshipMembers();

            foreach (var member in fellowshipMembers.Values)
            {
                member.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(member.Session, error, message));

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
                        member.Session.Network.EnqueueSend(new GameEventFellowshipDisband(member.Session));

                        if (ShareLoot)
                        {
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat("You no longer have permission to loot anyone else's kills.", ChatMessageType.Broadcast));

                            // you would expect this occur, but it did not in retail pcaps
                            //foreach (var fellow in fellowshipMembers.Values)
                            //    member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{fellow.Name} does not have permission to loot your kills.", ChatMessageType.Broadcast));
                        }

                        member.Fellowship = null;
                    }
                }
                else
                {
                    FellowshipMembers.Remove(player.Guid.Full);

                    if (IsLocked)
                    {
                        var timestamp = (int)Time.GetUnixTime();
                        if (!DepartedMembers.TryAdd(player.Guid.Full, timestamp))
                            DepartedMembers[player.Guid.Full] = timestamp;
                    }

                    player.Fellowship = null;

                    player.Session.Network.EnqueueSend(new GameEventFellowshipQuit(player.Session, player.Guid.Full));
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat("You no longer have permission to loot anyone else's kills.", ChatMessageType.Broadcast));

                    var fellowshipMembers = GetFellowshipMembers();

                    foreach (var member in fellowshipMembers.Values)
                    {
                        member.Session.Network.EnqueueSend(new GameEventFellowshipQuit(member.Session, player.Guid.Full));

                        if (ShareLoot)
                        {
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have lost permission to loot the kills of {player.Name}.", ChatMessageType.Broadcast));
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{member.Name} does not have permission to loot your kills.", ChatMessageType.Broadcast));
                        }
                    }
                    AssignNewLeader(null, null);

                    CalculateXPSharing();
                }
            }
            else if (!disband)
            {
                FellowshipMembers.Remove(player.Guid.Full);

                if (IsLocked)
                {
                    var timestamp = (int)Time.GetUnixTime();

                    if (!DepartedMembers.TryAdd(player.Guid.Full, timestamp))
                        DepartedMembers[player.Guid.Full] = timestamp;
                }

                player.Session.Network.EnqueueSend(new GameEventFellowshipQuit(player.Session, player.Guid.Full));

                var fellowshipMembers = GetFellowshipMembers();

                foreach (var member in fellowshipMembers.Values)
                {
                    member.Session.Network.EnqueueSend(new GameEventFellowshipQuit(member.Session, player.Guid.Full));

                    if (ShareLoot)
                    {
                        member.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have lost permission to loot the kills of {player.Name}.", ChatMessageType.Broadcast));
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{member.Name} does not have permission to loot your kills.", ChatMessageType.Broadcast));
                    }
                }

                player.Fellowship = null;

                CalculateXPSharing();
            }
        }

        public void AssignNewLeader(Player oldLeader, Player newLeader)
        {
            if (newLeader != null)
            {
                FellowshipLeaderGuid = newLeader.Guid.Full;

                if (oldLeader != null)
                    oldLeader.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(oldLeader.Session, WeenieErrorWithString.YouHavePassedFellowshipLeadershipTo_, newLeader.Name));

                SendWeenieErrorWithStringAndUpdate(WeenieErrorWithString._IsNowLeaderOfFellowship, newLeader.Name);
            }
            else
            {
                // leader has dropped, assign new random leader
                var fellowshipMembers = GetFellowshipMembers();

                if (fellowshipMembers.Count == 0) return;

                var rng = ThreadSafeRandom.Next(0, fellowshipMembers.Count - 1);

                var fellowGuids = fellowshipMembers.Keys.ToList();

                FellowshipLeaderGuid = fellowGuids[rng];

                var newLeaderName = fellowshipMembers[FellowshipLeaderGuid].Name;

                if (oldLeader != null)
                    oldLeader.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(oldLeader.Session, WeenieErrorWithString.YouHavePassedFellowshipLeadershipTo_, newLeaderName));

                SendWeenieErrorWithStringAndUpdate(WeenieErrorWithString._IsNowLeaderOfFellowship, newLeaderName);
            }
        }

        public void UpdateOpenness(bool isOpen)
        {
            Open = isOpen;
            var openness = Open ? WeenieErrorWithString._IsNowOpenFellowship : WeenieErrorWithString._IsNowClosedFellowship;
            SendWeenieErrorWithStringAndUpdate(openness, FellowshipName);
        }

        public void UpdateLock(bool isLocked, string lockName)
        {
            // Unlocking a fellowship is not possible without disbanding in retail worlds, so in all likelihood, this is only firing for fellowships being locked by emotemanager

            IsLocked = isLocked;

            if (string.IsNullOrWhiteSpace(lockName))
                lockName = "Undefined";

            if (isLocked)
            {
                Open = false;

                DepartedMembers.Clear();

                var timestamp = Time.GetUnixTime();
                if (!FellowshipLocks.TryAdd(lockName, new FellowshipLockData(timestamp)))
                    FellowshipLocks[lockName].UpdateTimestamp(timestamp);

                SendBroadcastAndUpdate("Your fellowship is now locked.  You may not recruit new members.  If you leave the fellowship, you have 15 minutes to be recruited back into the fellowship.");
            }
            else
            {
                // Unlocking a fellowship is not possible without disbanding in retail worlds, so in all likelihood, this never occurs

                DepartedMembers.Clear();

                FellowshipLocks.Remove(lockName);

                SendBroadcastAndUpdate("Your fellowship is now unlocked.");
            }
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

            var allEvenShareLevel = PropertyManager.GetLong("fellowship_even_share_level").Item;
            var allOverEvenShareLevel = !fellows.Values.Any(f => (f.Level ?? 1) < allEvenShareLevel);

            if (allOverEvenShareLevel)
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
                var inRange = GetFellowshipMembers().Values.Intersect(WithinRange(player, true)).ToList();
                var totalAmount = (ulong)Math.Round(amount * GetMemberSharePercent(inRange.Count));

                foreach (var member in inRange) //fellowshipMembers.Values)
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
                // updated: retail supposedly did not do this
                //var shareableMembers = GetFellowshipMembers().Values.Where(f => f.MaximumLuminance != null).ToList();

                var shareableMembers = GetFellowshipMembers().Values.ToList();

                if (shareableMembers.Count == 0)
                    return;

                var inRange = shareableMembers.Intersect(WithinRange(player, true)).ToList();

                // further filter to fellows in radar range -- Scratch that. We don't want to restrict to radar range!

                var perAmount = (ulong)Math.Round(amount * GetMemberSharePercent(inRange.Count));

                foreach (var member in inRange)
                {
                    if (member.MaximumLuminance == null) continue;

                    var fellowXpType = player == member ? xpType : XpType.Fellowship;
                    perAmount = (ulong)Math.Round(perAmount * GetDistanceScalar(player, member, xpType));

                    member.GrantLuminance((long)perAmount, fellowXpType, shareType);
                }
            }
        }

        internal double GetMemberSharePercent(int FellowsInRange)
        {
            switch (FellowsInRange)
            {
                case 1:
                    return 1.000000;    //100%
                case 2:
                    return 0.750000;    //150%
                case 3:
                    return 0.600000;    //180%
                case 4:
                    return 0.550000;    //220%
                case 5:
                    return 0.500000;    //250%
                case 6:
                    return 0.450000;    //270%
                case 7:
                    return 0.400000;    //280%
                case 8:
                    return 0.362500;    //290%
                case 9:
                    return 0.333333;    //300%
                case 10:
                    return 0.300000;    //300%
                case 11:
                    return 0.272727;    //300%
                case 12:
                    return 0.250000;    //300%
                case 13:
                    return 0.230769;    //300%
                case 14:
                    return 0.214286;    //300%
                case 15:
                    return 0.200000;    //300%
                case 16:
                    return 0.187500;    //300%
                case 17:
                    return 0.176471;    //300%
                case 18:
                    return 0.166666;    //300%
                case 19:
                    return 0.157895;    //300%
                case 20:
                    return 0.150000;    //300%
                case 21:
                    return 0.142857;    //300%
            }
            return 1.0;
        }

        public static int MaxFellowDistance = (int)PropertyManager.GetLong("fellowship_max_share_dist").Item;

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

            if (dist >= MaxFellowDistance * 2.0f)
                return 0.0f;

            if (dist <= MaxFellowDistance)
                return 1.0f;

            var scalar = 1.0f - (dist - MaxFellowDistance) / MaxFellowDistance;

            return Math.Max(0.0f, scalar);
        }

        /// <summary>
        /// Returns fellows within radar range (75 units outdoors, 25 units indoors)
        /// </summary>
        public List<Player> WithinRange(Player player, bool includeSelf = false)
        {
            var fellows = GetFellowshipMembers();

            //var landblockRange = PropertyManager.GetBool("fellow_kt_landblock").Item;

            var results = new List<Player>();

            foreach (var fellow in fellows.Values)
            {
                if (player == fellow && !includeSelf)
                    continue;

                //var shareable = player == fellow || landblockRange ?
                //    player.CurrentLandblock == fellow.CurrentLandblock || player.Location.DistanceTo(fellow.Location) <= 192.0f :
                //    player.Location.Distance2D(fellow.Location) <= player.CurrentRadarRange && player.ObjMaint.VisibleObjectsContainsKey(fellow.Guid.Full);      // 2d visible distance / radar range?

                var shareable = (player == fellow) || ((fellow.CurrentLandblock == player.CurrentLandblock) || (player.Location.Distance2D(fellow.Location) < (MaxFellowDistance * 2)));
                //var msgInfo = $"{fellow.Name}_Distance: {player.Location.Distance2D(fellow.Location)},\n {player.Name}_LandBlock_Id: {player.CurrentLandblock.Id},\n {fellow.Name}_LandBlock_Id: {fellow.CurrentLandblock.Id} \n";

                //log.Info(msgInfo);
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
            {
                if (fellow.FellowshipPanelOpen)
                    fellow.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(fellow.Session, player, ShareLoot, FellowUpdateType.Vitals));
            }
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
                AssignNewLeader(null, null);

            CalculateXPSharing();
            UpdateAllMembers();
        }
    }

    public static class FellowshipExtensions
    {
        private static readonly HashComparer hashComparer = new HashComparer(32);

        public static void Write(this BinaryWriter writer, Dictionary<uint, int> departedFellows)
        {
            PackableHashTable.WriteHeader(writer, departedFellows.Count, hashComparer.NumBuckets);

            var sorted = new SortedDictionary<uint, int>(departedFellows, hashComparer);

            foreach (var departed in sorted)
            {
                writer.Write(departed.Key);
                writer.Write(departed.Value);
            }
        }
    }
}
