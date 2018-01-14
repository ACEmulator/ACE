using ACE.Common;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Fellowship
    {
        public string FellowshipName;
        public uint FellowshipLeaderGuid;

        public bool ShareXP; // XP sharing: 0=no, 1=yes
        public bool EvenShare;
        public bool Open; // open fellowship: 0=no, 1=yes

        public List<Player> FellowshipMembers = new List<Player>(9);

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
            if (newMember.Fellowship != null)
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{newMember.Name} is already in a fellowship", Enum.ChatMessageType.Fellowship));
            }
            else
            {
                if (newMember.GetCharacterOption(Enum.CharacterOption.AutomaticallyAcceptFellowshipRequests))
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
                FellowshipMembers.Add(player);
                CalculateEvenSplit();
                Parallel.ForEach(FellowshipMembers, (member) =>
                {
                    inviter.Session.Network.EnqueueSend(new GameEventFellowshipUpdateFellow(inviter.Session, player, ShareXP));
                    inviter.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(inviter.Session));
                });
                player.Fellowship = inviter.Fellowship;
                Parallel.ForEach(FellowshipMembers, member =>
                {
                    member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} joined the fellowship", Enum.ChatMessageType.Fellowship));
                    member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                    member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
                });
            }
            else
            {
                inviter.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} declines your invite", Enum.ChatMessageType.Fellowship));
            }
        }
        
        public void RemoveFellowshipMember(Player player)
        {
            Parallel.ForEach(FellowshipMembers, member =>
            {
                member.Session.Network.EnqueueSend(new GameEventFellowshipDismiss(member.Session, player));
                member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} dismissed from fellowship", Enum.ChatMessageType.Fellowship));
            });
            FellowshipMembers.Remove(player);
            player.Fellowship = null;
            CalculateEvenSplit();
            Parallel.ForEach(FellowshipMembers, member =>
            {
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
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat("You disband the fellowship", Enum.ChatMessageType.Fellowship));
                        }
                        else
                        {
                            member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} disbanded the fellowship", Enum.ChatMessageType.Fellowship));
                            member.Fellowship = null;
                        }
                        
                    });
                }
                else
                {                 
                    FellowshipMembers.Remove(player);
                    player.Session.Network.EnqueueSend(new GameMessageFellowshipQuit(player.Session, player.Guid.Full));
                    CalculateEvenSplit();
                    AssignNewLeader(null);
                    Parallel.ForEach(FellowshipMembers, member =>
                    {
                        member.Session.Network.EnqueueSend(new GameMessageFellowshipQuit(member.Session, player.Guid.Full));
                        member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} left the fellowship", Enum.ChatMessageType.Fellowship));
                        member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                       // member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
                    });
                }
            }
            else
            {
                FellowshipMembers.Remove(player);
                player.Session.Network.EnqueueSend(new GameMessageFellowshipQuit(player.Session, player.Guid.Full));
                CalculateEvenSplit();
                Parallel.ForEach(FellowshipMembers, member =>
                {
                    member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} left the fellowship", Enum.ChatMessageType.Fellowship));
                    member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                    member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
                });
            }
        }

        public void AssignNewLeader(Player p)
        {
            string newLeaderName = string.Empty;
            if (p != null)
            {
                FellowshipLeaderGuid = p.Guid.Full;
                newLeaderName = p.Name;
                Parallel.ForEach(FellowshipMembers, member =>
                {
                    member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{newLeaderName} now leads the fellowship", Enum.ChatMessageType.Fellowship));
                    member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                    member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
                });
            }
            else
            {
                if (p != null && p.Fellowship.FellowshipLeaderGuid == this.FellowshipLeaderGuid)
                {
                    FellowshipLeaderGuid = p.Guid.Full;
                    Parallel.ForEach(FellowshipMembers, member =>
                    {
                        member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{p.Name} now leads the fellowship", Enum.ChatMessageType.Fellowship));
                        member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                        member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
                    });
                }
                else if (FellowshipMembers.Count > 0)
                {
                    Random rand = new Random();
                    int newLeaderIndex = rand.Next(FellowshipMembers.Count);
                    FellowshipLeaderGuid = FellowshipMembers[newLeaderIndex].Guid.Full;
                    newLeaderName = FellowshipMembers[newLeaderIndex].Name;
                    Parallel.ForEach(FellowshipMembers, member =>
                    {
                        member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{newLeaderName} now leads the fellowship", Enum.ChatMessageType.Fellowship));
                        member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                        member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
                    });

                }
            }
        }

        public void UpdateOpenness(bool isOpen)
        {
            Open = isOpen;
            string openness = Open ? "open" : "closed";

            Parallel.ForEach(FellowshipMembers, member =>
            {
                member.Session.Network.EnqueueSend(new GameMessageSystemChat($"Fellowship is now {openness}", Enum.ChatMessageType.Fellowship));
                member.Session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(member.Session));
                member.Session.Network.EnqueueSend(new GameEventFellowshipFellowUpdateDone(member.Session));
            });
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
    }
}
