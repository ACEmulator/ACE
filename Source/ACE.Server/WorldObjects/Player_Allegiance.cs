using System;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public Allegiance Allegiance { get; set; }
        public AllegianceNode AllegianceNode { get; set; }

        public bool HasAllegiance { get => Allegiance != null && Allegiance.TotalMembers > 1; }

        public ulong AllegianceXPCached
        {
            get => (ulong)(GetProperty(PropertyInt64.AllegianceXPCached) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt64.AllegianceXPCached); else SetProperty(PropertyInt64.AllegianceXPCached, (long)value); }
        }

        public ulong AllegianceXPGenerated
        {
            get => (ulong)(GetProperty(PropertyInt64.AllegianceXPGenerated) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt64.AllegianceXPGenerated); else SetProperty(PropertyInt64.AllegianceXPGenerated, (long)value); }
        }

        public ulong AllegianceXPReceived
        {
            get => (ulong)(GetProperty(PropertyInt64.AllegianceXPReceived) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt64.AllegianceXPReceived); else SetProperty(PropertyInt64.AllegianceXPReceived, (long)value); }
        }

        public int? AllegianceOfficerRank
        {
            get => GetProperty(PropertyInt.AllegianceOfficerRank);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AllegianceOfficerRank); else SetProperty(PropertyInt.AllegianceOfficerRank, value.Value); }
        }

        /// <summary>
        /// Called when a player tries to Swear Allegiance to a target
        /// </summary>
        /// <param name="targetGuid">The target this player is attempting to swear allegiance to</param>
        public void HandleActionSwearAllegiance(uint targetGuid)
        {
            if (!IsPledgable(targetGuid)) return;

            var patron = PlayerManager.GetOnlinePlayer(targetGuid);

            PatronId = targetGuid;
            MonarchId = AllegianceManager.GetMonarch(patron).Guid.Full;

            //Console.WriteLine("Patron: " + PlayerManager.GetOfflinePlayerByGuidId(Patron.Value).Name);
            //Console.WriteLine("Monarch: " + PlayerManager.GetOfflinePlayerByGuidId(Monarch.Value).Name);

            // send message to patron:
            // %vassal% has sworn Allegiance to you.
            patron.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has sworn Allegiance to you.", ChatMessageType.Broadcast));

            // send message to vassal:
            // %patron% has accepted your oath of Allegiance!
            // Motion_Kneel
            Session.Network.EnqueueSend(new GameMessageSystemChat($"{patron.Name} has accepted your oath of Allegiance!", ChatMessageType.Broadcast));

            EnqueueBroadcastMotion(new Motion(MotionStance.NonCombat, MotionCommand.Kneel));

            // rebuild allegiance tree structure
            AllegianceManager.OnSwearAllegiance(this);

            AllegianceXPGenerated = 0;

            // refresh ui panel
            Session.Network.EnqueueSend(new GameEventAllegianceUpdate(Session, Allegiance, AllegianceNode), new GameEventAllegianceAllegianceUpdateDone(Session));
        }

        /// <summary>
        /// Called when a player tries to break Allegiance to a target
        /// </summary>
        /// <param name="targetGuid">The target this player is attempting to break allegiance from</param>
        public void HandleActionBreakAllegiance(uint targetGuid)
        {
            if (!IsBreakable(targetGuid)) return;

            var target = PlayerManager.FindByGuid(targetGuid, out var targetIsOnline);

            //Console.WriteLine(Name + " breaking allegiance to " + target.Name);

            // target can be either patron or vassal
            var isPatron = PatronId == target.Guid.Full;
            var isVassal = target.PatronId == Guid.Full;

            // break ties
            if (isVassal)
            {
                target.PatronId = null;
                target.MonarchId = null;
            }
            else
            {
                PatronId = null;
                MonarchId = null;
            }

            // send message to target if online
            if (targetIsOnline)
            {
                var onlineTarget = PlayerManager.GetOnlinePlayer(targetGuid);
                if (onlineTarget != null)
                    onlineTarget.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has broken their Allegiance to you!", ChatMessageType.Broadcast));
            }

            // send message to self
            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have broken your Allegiance to {target.Name}!", ChatMessageType.Broadcast));

            // rebuild allegiance tree structures
            AllegianceManager.OnBreakAllegiance(this, target);

            // refresh ui panel
            Session.Network.EnqueueSend(new GameEventAllegianceUpdate(Session, Allegiance, AllegianceNode), new GameEventAllegianceAllegianceUpdateDone(Session));
        }

        /// <summary>
        /// Returns TRUE if this player can swear to the target guid
        /// </summary>
        public bool IsPledgable(uint targetGuid)
        {
            // ensure target player is online, and within range
            var target = PlayerManager.FindByGuid(targetGuid);
            if (target == null)
            {
                //Console.WriteLine(Name + " tried to swear to an unknown player guid: " + targetGuid.Full.ToString("X8"));
                return false;
            }

            // player already sworn?
            if (PatronId != null)
            {
                //Console.WriteLine(Name + " tried to swear to " + target.Name + ", but is already sworn to " + PlayerManager.GetOfflinePlayerByGuidId(Patron.Value).Name);
                return false;
            }

            // player can't swear to themselves
            if (targetGuid == Guid.Full)
            {
                //Console.WriteLine(Name + " tried to swear to themselves");
                return false;
            }

            // patron must currently be greater or equal level
            if (target.Level < Level)
            {
                //Console.WriteLine(Name + " tried to swear to a lower level character");
                return false;
            }

            var selfNode = AllegianceNode;
            var targetNode = target.AllegianceNode;

            if (targetNode != null)
            {
                // maximum # of direct vassals = 11
                if (targetNode.TotalVassals >= 11)
                {
                    //Console.WriteLine(target.Name + " already has the maximum # of vassals");
                    return false;
                }

                // 2 players can't swear to each other
                // prevent any loops in the allegiance chain
                if (selfNode != null && selfNode.IsMonarch)
                {
                    if (selfNode.PlayerGuid == targetNode.Monarch.PlayerGuid)
                    {
                        //Console.WriteLine(Name + " tried to swear to someone already in Allegiance: " + target.Name);
                        return false;
                    }
                }
            }

            // check distance <= 4.0
            // check ignore allegiance requests

            return true;
        }

        /// <summary>
        /// Returns TRUE if this player can break allegiance to the target guid
        /// </summary>
        public bool IsBreakable(uint targetGuid)
        {
            // players can break from either vassals or patrons

            // ensure target player exists
            var target = PlayerManager.FindByGuid(targetGuid);
            if (target == null)
            {
                //Console.WriteLine(Name + " tried to break allegiance to an unknown player guid: " + targetGuid.Full.ToString("X8"));
                return false;
            }

            // verify patron or vassal
            var isPatron = PatronId == target.Guid.Full;
            var isVassal = target.PatronId == Guid.Full;

            if (!isPatron && !isVassal)
            {
                //Console.WriteLine(Name + " tried to break allegiance from " + target.Name + ", but they aren't patron or vassal");
                return false;
            }
            return true;
        }

        /// <summary>
        /// For an online patron, adds the pending allegiance XP stored in AllegianceXPCached
        /// to their total / unassigned xp
        /// </summary>
        /// <param name="showMsg">Set to TRUE if player is logging in</param>
        public void AddAllegianceXP(bool showMsg = false)
        {
            if (AllegianceXPCached == 0) return;

            if (showMsg)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(3.0f);
                actionChain.AddAction(this, () =>
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Your Vassals have produced experience points for you.\nTaking your skills as a leader into account, you gain {AllegianceXPCached:N0} xp.", ChatMessageType.Broadcast));
                    AddAllegianceXP_Receive();
                });
                actionChain.EnqueueChain();
            }
            else
                AddAllegianceXP_Receive();
        }

        private void AddAllegianceXP_Receive()
        {
            // TODO: handle ulong -> long?
            EarnXP((long)AllegianceXPCached, false);

            AllegianceXPReceived += AllegianceXPCached;

            AllegianceXPCached = 0;
        }

        // ============== MOTD ================

        public void HandleActionQueryMotd()
        {
            //Console.WriteLine($"{Name}.HandleActionQueryMotd()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (Allegiance.Motd == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your allegiance has not set a message of the day.", ChatMessageType.Broadcast));
                return;
            }

            var rank = AllegianceRank.GetTitle(HeritageGroup, (Gender)Gender, AllegianceNode.Rank);
            var msg = $"\"{Allegiance.Motd}\" -- {rank} {Name}";

            Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
        }

        public void HandleActionSetMotd(string motd)
        {
            //Console.WriteLine($"{Name}.HandleActionSetMotd({motd})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            Allegiance.Motd = motd;
            Allegiance.SaveBiotaToDatabase();

            Session.Network.EnqueueSend(new GameMessageSystemChat("Your message of the day has been set.", ChatMessageType.Broadcast));
        }

        public void HandleActionClearMotd()
        {
            //Console.WriteLine($"{Name}.HandleActionClearMotd()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            Allegiance.Motd = null;
            Allegiance.SaveBiotaToDatabase();

            Session.Network.EnqueueSend(new GameMessageSystemChat("Your message of the day has been cleared.", ChatMessageType.Broadcast));
        }

        // ============= Allegiance Name ==============

        public void HandleActionQueryAllegianceName()
        {
            //Console.WriteLine($"{Name}.HandleActionQueryAllegianceName()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (Allegiance.AllegianceName == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your allegiance has not set a name.", ChatMessageType.Broadcast));
                return;
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat(Allegiance.AllegianceName, ChatMessageType.Broadcast));
        }

        public void HandleActionSetAllegianceName(string allegianceName)
        {
            //Console.WriteLine($"{Name}.HandleActionSetAllegianceName({allegianceName}");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            // TODO: name verifications
            // - same name as current allegiance name
            // - no empty names
            // - allegiance name too long (40 chars max.)
            // - allegiance name already in use
            // - bad chars (space, single quote, hyphen, A-Z, a-z)
            // - banned words from portal.dat
            // - name change timer (1 day?)

            Allegiance.AllegianceName = allegianceName;
            Allegiance.SaveBiotaToDatabase();

            Session.Network.EnqueueSend(new GameMessageSystemChat("Your allegiance name has been set.", ChatMessageType.Broadcast));
        }

        public void HandleActionClearAllegianceName()
        {
            //Console.WriteLine($"{Name}.HandleActionClearAllegianceName()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            Allegiance.AllegianceName = null;
            Allegiance.SaveBiotaToDatabase();

            Session.Network.EnqueueSend(new GameMessageSystemChat("Your allegiance name has been cleared.", ChatMessageType.Broadcast));
        }

        public void HandleActionListAllegianceOfficers()
        {
            //Console.WriteLine($"{Name}.HandleActionListAllegianceOfficers()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            var officerList = "Allegiance Officers:\n";
            officerList += Allegiance.Monarch.Player.Name + " (Monarch)\n";

            foreach (var officer in Allegiance.Officers.Values.Select(i => i.Player).OrderBy(i => i.Name))
            {
                var title = Allegiance.GetOfficerTitle((AllegianceOfficerLevel)officer.AllegianceOfficerRank);
                officerList += $"{officer.Name} ({title})\n";
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat(officerList, ChatMessageType.Broadcast));
        }

        public void HandleActionListAllegianceOfficerTitles()
        {
            //Console.WriteLine($"{Name}.HandleActionListAllegianceOfficerTitles()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            var speakerTitle = Allegiance.AllegianceSpeakerTitle ?? "Speaker";
            var seneschalTitle = Allegiance.AllegianceSeneschalTitle ?? "Seneschal";
            var castellanTitle = Allegiance.AllegianceCastellanTitle ?? "Castellan";

            var officerTitles = "Allegiance Officer Titles:\n";
            officerTitles += $"1. {speakerTitle}\n";
            officerTitles += $"2. {seneschalTitle}\n";
            officerTitles += $"3. {castellanTitle}\n";

            Session.Network.EnqueueSend(new GameMessageSystemChat(officerTitles, ChatMessageType.Broadcast));
        }

        public void HandleActionSetAllegianceOfficerTitle(uint rank, string title)
        {
            //Console.WriteLine($"{Name}.HandleActionSetAllegianceOfficerTitle({rank}, {title})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            if (rank < 1 || rank > 3)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Please specify a valid officer level as a number between 1 and 3.", ChatMessageType.Broadcast));
                return;
            }

            switch (rank)
            {
                case 1:
                    Allegiance.AllegianceSpeakerTitle = title;
                    break;
                case 2:
                    Allegiance.AllegianceSeneschalTitle = title;
                    break;
                case 3:
                    Allegiance.AllegianceCastellanTitle = title;
                    break;
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat($"Your allegiance {(AllegianceOfficerLevel)rank} title has been set.", ChatMessageType.Broadcast));

            Allegiance.SaveBiotaToDatabase();
        }

        public void HandleActionClearAllegianceOfficerTitles()
        {
            //Console.WriteLine($"{Name}.HandleActionClearAllegianceOfficerTitles()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            Allegiance.AllegianceSpeakerTitle = null;
            Allegiance.AllegianceSeneschalTitle = null;
            Allegiance.AllegianceCastellanTitle = null;

            Allegiance.SaveBiotaToDatabase();

            Session.Network.EnqueueSend(new GameMessageSystemChat($"Your allegiance officer titles have been cleared.", ChatMessageType.Broadcast));
        }

        public void HandleActionSetAllegianceOfficer(string playerName, uint officerLevel)
        {
            //Console.WriteLine($"{Name}.HandleActionSetAllegianceOfficer({playerName}, {officerLevel})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            var player = PlayerManager.FindByName(playerName);
            if (player == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found", ChatMessageType.Broadcast));
                return;
            }

            if (!Allegiance.Members.ContainsKey(player.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found in this allegiance", ChatMessageType.Broadcast));
                return;
            }

            if (officerLevel < 1 || officerLevel > 3)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Please specify a valid officer level as a number between 1 and 3.", ChatMessageType.Broadcast));
                return;
            }

            player.AllegianceOfficerRank = (int)officerLevel;
            var title = Allegiance.GetOfficerTitle((AllegianceOfficerLevel)officerLevel);

            Allegiance.BuildOfficers();

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is now {title}.", ChatMessageType.Broadcast));
        }

        public void HandleActionRemoveAllegianceOfficer(string officerName)
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveAllegianceOfficer({officerName})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            var officer = PlayerManager.FindByName(officerName);
            if (officer == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{officerName} not found", ChatMessageType.Broadcast));
                return;
            }

            if (!Allegiance.Members.ContainsKey(officer.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{officerName} not found in this allegiance", ChatMessageType.Broadcast));
                return;
            }

            if (!Allegiance.Officers.ContainsKey(officer.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{officerName} not found in allegiance officers", ChatMessageType.Broadcast));
                return;
            }

            officer.AllegianceOfficerRank = null;
            Allegiance.BuildOfficers();

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{officerName} has been removed from allegiance officers.", ChatMessageType.Broadcast));
        }

        public void HandleActionAllegianceInfoRequest(string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveAllegianceOfficer({playerName})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // TODO: also check officer permissions
            if (Allegiance.MonarchId != Guid.Full)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            var player = PlayerManager.FindByName(playerName);
            if (player == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found", ChatMessageType.Broadcast));
                return;
            }

            Allegiance.Members.TryGetValue(player.Guid, out var allegianceNode);
            if (allegianceNode == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found in this allegiance", ChatMessageType.Broadcast));
                return;
            }
            var profile = new AllegianceProfile(Allegiance, allegianceNode);

            Session.Network.EnqueueSend(new GameEventAllegianceInfoResponse(Session, player.Guid.Full, profile));
        }
    }
}
