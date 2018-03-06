using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ACE.Entity;
using ACE.Server.WorldObjects;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Entity
{
    public class Fellowship
    {
        public string FellowshipName;
        public uint FellowshipLeaderGuid;

        public bool ShareXP; // XP sharing: 0=no, 1=yes
        public bool EvenShare;
        public bool Open; // open fellowship: 0=no, 1=yes

        public List<Player> FellowshipMembers = new List<Player>(9);

        private Dictionary<uint, DateTime> oldFellows = new Dictionary<uint, DateTime>();
        
        public Fellowship(Player leader, string fellowshipName, bool shareXP)
        {
            ShareXP = shareXP;
            FellowshipLeaderGuid = leader.Guid.Full;
            FellowshipName = fellowshipName;
            EvenShare = false;

            FellowshipMembers = new List<Player> { leader };

            Open = false;
        }

        public void AddFellowshipMember(Player inviter, Player newMember)
        {
            if (FellowshipMembers.Count == 9)
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat("Fellowship is already full", ACE.Entity.Enum.ChatMessageType.Fellowship));
                return;
            }
            if (newMember.Fellowship != null)
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{newMember.Name} is already in a fellowship", ACE.Entity.Enum.ChatMessageType.Fellowship));
            }
            else
            {
                if (newMember.GetCharacterOption(global::ACE.Entity.Enum.CharacterOption.AutomaticallyAcceptFellowshipRequests))
                {
                    AddConfirmedMember(inviter, newMember, true);
                }
                else
                {
                    Confirmation confirm = new Confirmation(Network.Enum.ConfirmationType.Fellowship,
                            $"{inviter.Name} invites to you join a fellowship.", inviter.Guid.Full,
                            newMember.Guid.Full);

                    ConfirmationManager.AddConfirmation(confirm);

                    newMember.Session.Network.EnqueueSend(new GameEventConfirmationRequest(newMember.Session, Network.Enum.ConfirmationType.Fellowship,
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
                    inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} cannot join as fellowship is full", ACE.Entity.Enum.ChatMessageType.Fellowship));
                }
                else
                {
                    FellowshipMembers.Add(player);
                    CalculateEvenSplit();
                    Parallel.ForEach(FellowshipMembers, (member) =>
                    {
                        inviter.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(inviter.Session, player, ShareXP));
                        inviter.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(inviter.Session));
                    });
                    player.Fellowship = inviter.Fellowship;
                    SendMessageAndUpdate($"{player.Name} joined the fellowship");
                }
            }
            else
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} declines your invite", ACE.Entity.Enum.ChatMessageType.Fellowship));
            }
        }
        
        public void RemoveFellowshipMember(Player player)
        {
            Parallel.ForEach(FellowshipMembers, member =>
            {
                member.Session.Network.EnqueueSend(new GameEventFellowshipDismiss(member.Session, player));
                member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} dismissed from fellowship", ACE.Entity.Enum.ChatMessageType.Fellowship));
            });
            FellowshipMembers.Remove(player);
            player.Fellowship = null;
            CalculateEvenSplit();
            UpdateAllMembers();
        }

        private void UpdateAllMembers()
        {
            Parallel.ForEach(FellowshipMembers, member =>
            {
                member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
            });
        }

        private void SendMessageAndUpdate(string message)
        {
            Parallel.ForEach(FellowshipMembers, member =>
            {
                member.Session.Network.EnqueueSend(new GameMessageSystemChat(message, global::ACE.Entity.Enum.ChatMessageType.Fellowship));
                member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
            });
        }

        public void QuitFellowship(Player player, bool disband)
        {
            if (player.Guid.Full == FellowshipLeaderGuid)
            {
                if (disband)
                {
                    Parallel.ForEach(FellowshipMembers, member =>
                    {
                        member.Session.Network.EnqueueSend(new GameMessageFellowshipQuit(member.Session, member.Guid.Full));
                        if (member.Guid.Full == FellowshipLeaderGuid)
                        {
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat("You disband the fellowship", ACE.Entity.Enum.ChatMessageType.Fellowship));
                        }
                        else
                        {
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} disbanded the fellowship", ACE.Entity.Enum.ChatMessageType.Fellowship));
                            member.Fellowship = null;
                        }
                        
                    });
                }
                else
                {                 
                    FellowshipMembers.Remove(player);
                    oldFellows.Add(player.Guid.Full, DateTime.Now);
                    player.Session.Network.EnqueueSend(new GameMessageFellowshipQuit(player.Session, player.Guid.Full));
                    //member.Session.Network.EnqueueSend(new GameMessageFellowshipQuit(member.Session, player.Guid.Full));
                    CalculateEvenSplit();
                    AssignNewLeader(null);
                    SendMessageAndUpdate($"{player.Name} left the fellowship");
                }
            }
            else
            {
                FellowshipMembers.Remove(player);
                oldFellows.Add(player.Guid.Full, DateTime.Now);
                player.Session.Network.EnqueueSend(new GameMessageFellowshipQuit(player.Session, player.Guid.Full));
                CalculateEvenSplit();
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
                if (p != null && p.Fellowship.FellowshipLeaderGuid == this.FellowshipLeaderGuid)
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

        private void CalculateEvenSplit()
        {
            var countPlayerAbove = (from d in FellowshipMembers
                                    where d.Level > 50
                                    select d).Count();

            if (countPlayerAbove != FellowshipMembers.Count)
            {
                var nonLeaderMembers = from d in FellowshipMembers
                                       where d.Guid.Full != FellowshipLeaderGuid
                                       select d;

                Player leader = WorldManager.GetPlayerByGuidId(FellowshipLeaderGuid);
                foreach (Player p in nonLeaderMembers)
                {
                    if (Math.Abs(leader.Level - p.Level) > 5)
                    {
                        EvenShare = false;
                        return;
                    }
                }
            }
            EvenShare = true;
        }

        internal void SplitXp(UInt64 amount, bool fixedAmount)
        {
            if (EvenShare)
            {
                UInt64 shareAmount = amount;

                if (!fixedAmount)
                    shareAmount = (UInt64)((double)shareAmount * GetMemberSharePercent());
                else
                    shareAmount = (amount / (UInt64)FellowshipMembers.Count);

                Parallel.ForEach(FellowshipMembers, member =>
                {
                    if (!IsPlayerInside(member) && !fixedAmount)
                        shareAmount = (UInt64)(shareAmount * InRangeOfLeader(member));

                    member.EarnXP((long)shareAmount, false);
                });
            }
            else
            {
                // Calc distrubtion %
                double totalLevels = 0;
                foreach (Player p in FellowshipMembers)
                {
                    totalLevels += p.Level;
                }
                double percentPerLevel = totalLevels / FellowshipMembers.Count;
                Parallel.ForEach(FellowshipMembers, member =>
                {
                    if (!IsPlayerInside(member))
                    {
                        UInt64 playerTotal = (UInt64)(member.Level * percentPerLevel * InRangeOfLeader(member));
                        member.EarnXP((long)playerTotal, false);
                    }
                });
            }
        }

        internal double GetMemberSharePercent()
        {
            switch (FellowshipMembers.Count)
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
            }
            return 1.0;
        }

        internal double InRangeOfLeader(Player player)
        {
            Position leaderPosition = WorldManager.GetPlayerByGuidId(FellowshipLeaderGuid).Location;
            Position memberPosition = player.Location;

            if (Math.Abs(memberPosition.DistanceTo(leaderPosition)) <= 600)
            {
                return 1;
            }

            return 1 - ((Math.Abs(memberPosition.DistanceTo(leaderPosition))-600) / 600);
        }

        internal bool IsPlayerInside(Player player)
        {
            if(player.Location.Indoors)
                return true;
            return false;
        }
    }
}
