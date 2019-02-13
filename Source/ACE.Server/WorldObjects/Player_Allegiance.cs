using System;
using System.Linq;

using ACE.Entity;
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

        public int? AllegianceRank
        {
            get => GetProperty(PropertyInt.AllegianceRank);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AllegianceRank); else SetProperty(PropertyInt.AllegianceRank, value.Value); }
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

            // handle special case: monarch swearing into another allegiance
            if (Allegiance != null && Allegiance.MonarchId == Guid.Full)
                HandleMonarchSwear();

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
            AllegianceOfficerRank = null;

            // refresh ui panel
            Session.Network.EnqueueSend(new GameEventAllegianceUpdate(Session, Allegiance, AllegianceNode), new GameEventAllegianceAllegianceUpdateDone(Session));
        }


        /// <summary>
        /// Handle monarch swearing into another allegiance
        /// </summary>
        public void HandleMonarchSwear()
        {
            // TODO: allegiance officers should probably be stored in their own table
            foreach (var kvp in Allegiance.Officers)
            {
                var officer = PlayerManager.FindByGuid(kvp.Key);
                if (officer != null)
                    officer.AllegianceOfficerRank = null;
            }
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
            var target = PlayerManager.GetOnlinePlayer(targetGuid);
            if (target == null)
            {
                //Console.WriteLine(Name + " tried to swear to an unknown player guid: " + targetGuid.Full.ToString("X8"));
                return false;
            }

            // check ignore allegiance requests
            if (target.GetCharacterOption(CharacterOption.IgnoreAllegianceRequests))
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YourOfferOfAllegianceWasIgnored));
                return false;
            }

            // player already sworn?
            if (PatronId != null)
            {
                //Console.WriteLine(Name + " tried to swear to " + target.Name + ", but is already sworn to " + PlayerManager.GetOfflinePlayerByGuidId(Patron.Value).Name);
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouveAlreadySwornAllegiance));
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

            // check distance <= 4.0
            if (Location.DistanceTo(target.Location) > 4.0f)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.AllegianceMaxDistanceExceeded));
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

                if (targetNode.Allegiance.IsLocked && !targetNode.Allegiance.ApprovedVassals.Contains(Guid)
                    && (targetNode.Player.AllegianceOfficerRank ?? 0) < (int)AllegianceOfficerLevel.Castellan)
                {
                    //Console.WriteLine(Name + "tried to join locked allegiance, not in approved vassals list");
                    return false;
                }

                if (targetNode.Allegiance.BanList.Contains(Guid))
                {
                    //Console.WriteLine(Name + "tried to join allegiance, but was banned!");
                    return false;
                }
            }

            // ensure this player doesn't own a mansion
            if (House != null && House.HouseType == ACE.Entity.Enum.HouseType.Mansion)
            {
                //Console.WriteLine(Name + "monarch tried to pledge allegiance, already owns a mansion");
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.CannotSwearAllegianceWhileOwningMansion));
                return false;
            }

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
        /// Called when a player logs in to handle allegiance events on login
        /// </summary>
        public void HandleAllegianceOnLogin()
        {
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(3.0f);
            actionChain.AddAction(this, () =>
            {
                if (Allegiance != null && Allegiance.AllegianceMotd != null)
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"\"{Allegiance.AllegianceMotd}\" -- {Allegiance.AllegianceMotdSetBy}", ChatMessageType.Broadcast));

                if (AllegianceXPCached != 0)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"Your Vassals have produced experience points for you.\nTaking your skills as a leader into account, you gain {AllegianceXPCached:N0} xp.", ChatMessageType.Broadcast));
                    AddAllegianceXP();
                }
            });
            actionChain.EnqueueChain();

            if (Allegiance != null)
            {
                foreach (var member in Allegiance.OnlinePlayers)
                {
                    if (member.Guid != Guid && member.GetCharacterOption(CharacterOption.ShowAllegianceLogons))
                        member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} is online.", ChatMessageType.Allegiance));
                }
            }
        }

        public void HandleAllegianceOnLogout()
        {
            if (Allegiance != null)
            {
                foreach (var member in Allegiance.OnlinePlayers)
                {
                    if (member.Guid != Guid && member.GetCharacterOption(CharacterOption.ShowAllegianceLogons))
                        member.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} is offline.", ChatMessageType.Allegiance));
                }
            }
        }

        /// <summary>
        /// For an online patron, adds the pending allegiance XP stored in AllegianceXPCached
        /// to their total / unassigned xp
        /// </summary>
        public void AddAllegianceXP()
        {
            if (AllegianceXPCached == 0) return;

            // TODO: handle ulong -> long?
            EarnXP((long)AllegianceXPCached, false);

            AllegianceXPReceived += AllegianceXPCached;

            AllegianceXPCached = 0;
        }

        public void HandleActionQueryMotd()
        {
            //Console.WriteLine($"{Name}.HandleActionQueryMotd()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (Allegiance.AllegianceMotd == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your allegiance has not set a message of the day.", ChatMessageType.Broadcast));
                return;
            }

            var msg = $"\"{Allegiance.AllegianceMotd}\" -- {Allegiance.AllegianceMotdSetBy}";

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

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Speaker)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            Allegiance.AllegianceMotd = motd;

            var rank = AllegianceTitle.GetTitle(HeritageGroup, (Gender)Gender, AllegianceNode.Rank);
            Allegiance.AllegianceMotdSetBy = $"{rank} {Name}";

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

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Speaker)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            Allegiance.AllegianceMotd = null;

            var rank = AllegianceTitle.GetTitle(HeritageGroup, (Gender)Gender, AllegianceNode.Rank);
            Allegiance.AllegianceMotdSetBy = $"{rank} {Name}";

            Allegiance.SaveBiotaToDatabase();

            Session.Network.EnqueueSend(new GameMessageSystemChat("Your message of the day has been cleared.", ChatMessageType.Broadcast));
        }

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

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Castellan)
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

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Castellan)
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

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Castellan)
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

            // castellans can clear all titles?
            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Castellan)
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
            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Seneschal)
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

            if (player.Guid.Full == Allegiance.MonarchId)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            // seneschals can only promote/demote speakers
            if (AllegiancePermissionLevel == AllegiancePermissionLevel.Seneschal)
            {
                if (officerLevel > 1 || (AllegianceOfficerLevel)(player.AllegianceOfficerRank ?? 0) > AllegianceOfficerLevel.Speaker)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                    return;
                }
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

            // send message to online target player?
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

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Seneschal)
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
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{officer.Name} not found in allegiance officers", ChatMessageType.Broadcast));
                return;
            }

            // seneschals can only promote/demote speakers and non-officers
            if (AllegiancePermissionLevel == AllegiancePermissionLevel.Seneschal)
            {
                if ((AllegianceOfficerLevel)(officer.AllegianceOfficerRank ?? 0) > AllegianceOfficerLevel.Speaker)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                    return;
                }
            }

            officer.AllegianceOfficerRank = null;
            Allegiance.BuildOfficers();

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{officer.Name} has been removed from allegiance officers.", ChatMessageType.Broadcast));

            // send message to online target player?
        }

        public void HandleActionClearAllegianceOfficers()
        {
            //Console.WriteLine($"{Name}.HandleActionClearAllegianceOfficers()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            // could castellans perform this action?
            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Monarch)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            foreach (var kvp in Allegiance.Officers)
            {
                var officer = PlayerManager.FindByGuid(kvp.Key);
                if (officer != null)
                    officer.AllegianceOfficerRank = null;
            }

            Allegiance.BuildOfficers();

            Session.Network.EnqueueSend(new GameMessageSystemChat($"The list of officers has been cleared.", ChatMessageType.Broadcast));
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

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Seneschal)
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

        public void HandleActionDoAllegianceLockAction(AllegianceLockAction action)
        {
            //Console.WriteLine($"{Name}.HandleActionDoAllegianceLockAction({action})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            var lockStatus = Allegiance.IsLocked ? "locked" : "unlocked";

            // no permissions for checks?
            if (action == AllegianceLockAction.Check)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The allegiance is currently {lockStatus}.", ChatMessageType.Broadcast));
                return;
            }

            if (action == AllegianceLockAction.CheckApproved)
            {
                if (Allegiance.ApprovedVassals.Count == 0)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"The approved vassals list is currently empty.", ChatMessageType.Broadcast));
                    return;
                }

                var list = "Approved vassals:";
                foreach (var guid in Allegiance.ApprovedVassals)
                {
                    var approvedVassal = PlayerManager.FindByGuid(guid);
                    if (approvedVassal == null)
                        continue;

                    list += $"\n{approvedVassal.Name}";
                }

                Session.Network.EnqueueSend(new GameMessageSystemChat(list, ChatMessageType.Broadcast));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Seneschal)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                return;
            }

            if (action == AllegianceLockAction.ClearApproved)
            {
                Allegiance.ApprovedVassals.Clear();
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The approved vassals list has been cleared.", ChatMessageType.Broadcast));
                return;
            }

            if (action == AllegianceLockAction.On && Allegiance.IsLocked || action== AllegianceLockAction.Off && !Allegiance.IsLocked)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The allegiance is already {lockStatus}.", ChatMessageType.Broadcast));
                return;
            }

            if (action == AllegianceLockAction.On)
                Allegiance.IsLocked = true;
            else if (action == AllegianceLockAction.Off)
                Allegiance.IsLocked = false;
            else if (action == AllegianceLockAction.Toggle)
                Allegiance.IsLocked = !Allegiance.IsLocked;

            Allegiance.SaveBiotaToDatabase();

            lockStatus = Allegiance.IsLocked ? "locked" : "unlocked";

            Session.Network.EnqueueSend(new GameMessageSystemChat($"The allegiance is now {lockStatus}.", ChatMessageType.Broadcast));
        }

        public void HandleActionSetAllegianceApprovedVassal(string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionSetAllegianceApprovedVassal({playerName})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Castellan)
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

            if (player.Allegiance != null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is already in an allegiance.", ChatMessageType.Broadcast));
                return;
            }

            if (Allegiance.Members.ContainsKey(player.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is already in the allegiance.", ChatMessageType.Broadcast));
                return;
            }

            if (Allegiance.ApprovedVassals.Contains(player.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is already an approved vassal.", ChatMessageType.Broadcast));
                return;
            }

            Allegiance.ApprovedVassals.Add(player.Guid);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is now an approved vassal.", ChatMessageType.Broadcast));
        }

        public void HandleActionAllegianceChatBoot(string playerName, string reason)
        {
            //Console.WriteLine($"{Name}.HandleActionAllegianceChatBoot({playerName}, {reason})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Speaker)
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
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} not found in allegiance", ChatMessageType.Broadcast));
                return;
            }

            if (player.Guid == Guid)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot boot yourself from allegiance chat.", ChatMessageType.Broadcast));
                return;
            }

            if (player.Guid.Full == Allegiance.MonarchId)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot boot the monarch from allegiance chat.", ChatMessageType.Broadcast));
                return;
            }

            if (Allegiance.ChatFilters.TryGetValue(player.Guid, out var existing))
            {
                if (existing == DateTime.MaxValue)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has already been booted from allegiance chat.", ChatMessageType.Broadcast));
                    return;
                }

                Allegiance.ChatFilters[player.Guid] = DateTime.MaxValue;
            }
            else
                Allegiance.ChatFilters.Add(player.Guid, DateTime.MaxValue);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been booted from allegiance chat.", ChatMessageType.Broadcast));

            var onlinePlayer = PlayerManager.GetOnlinePlayer(player.Guid);

            if (onlinePlayer != null)
                onlinePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been booted from Allegiance chat ({reason})", ChatMessageType.Broadcast));
        }

        public static TimeSpan AllegianceChat_GagTime = TimeSpan.FromMinutes(5);

        public void HandleActionAllegianceChatGag(string playerName, bool enabled)
        {
            if (enabled)
                HandleActionAllegianceChatGag_Enabled(playerName);
            else
                HandleActionAllegianceChatGag_Disabled(playerName);
        }

        public void HandleActionAllegianceChatGag_Enabled(string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionAllegianceChatGag_Enabled({playerName})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Speaker)
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
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} not found in allegiance", ChatMessageType.Broadcast));
                return;
            }

            if (player.Guid == Guid)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot gag yourself.", ChatMessageType.Broadcast));
                return;
            }

            if (player.Guid.Full == Allegiance.MonarchId)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot gag the monarch.", ChatMessageType.Broadcast));
                return;
            }

            if (Allegiance.ChatFilters.TryGetValue(player.Guid, out var existing))
            {
                if (existing == DateTime.MaxValue)
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has already been booted from allegiance chat.", ChatMessageType.Broadcast));
                    return;
                }

                Allegiance.ChatFilters[player.Guid] = DateTime.UtcNow + AllegianceChat_GagTime;
            }
            else
                Allegiance.ChatFilters.Add(player.Guid, DateTime.UtcNow + AllegianceChat_GagTime);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been gagged.", ChatMessageType.Broadcast));

            var onlinePlayer = PlayerManager.GetOnlinePlayer(player.Guid);

            if (onlinePlayer != null)
                onlinePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been gagged in allegiance chat.", ChatMessageType.Broadcast));
        }

        public void HandleActionAllegianceChatGag_Disabled(string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionAllegianceChatGag_Disabled({playerName})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Speaker)
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
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} not found in allegiance", ChatMessageType.Broadcast));
                return;
            }

            if (player.Guid == Guid)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot ungag yourself.", ChatMessageType.Broadcast));
                return;
            }

            if (!Allegiance.ChatFilters.TryGetValue(player.Guid, out var existing))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has not been gagged.", ChatMessageType.Broadcast));
                return;
            }

            if (existing == DateTime.MaxValue)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been booted from allegiance chat.", ChatMessageType.Broadcast));
                return;
            }

            Allegiance.ChatFilters.Remove(player.Guid);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been ungagged.", ChatMessageType.Broadcast));

            var onlinePlayer = PlayerManager.GetOnlinePlayer(player.Guid);

            if (onlinePlayer != null)
                onlinePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been ungagged in allegiance chat.", ChatMessageType.Broadcast));
        }

        public void HandleActionListAllegianceBans()
        {
            //Console.WriteLine($"{Name}.HandleActionListAllegianceBans()");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (Allegiance.BanList.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"The ban list is currently empty.", ChatMessageType.Broadcast));
                return;
            }

            var list = "Allegiance ban list:";
            foreach (var guid in Allegiance.BanList)
            {
                var player = PlayerManager.FindByGuid(guid);
                if (player != null)
                    list += $"\n{player.Name}";
            }

            Session.Network.EnqueueSend(new GameMessageSystemChat(list, ChatMessageType.Broadcast));
        }

        public void HandleActionAddAllegianceBan(string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionAddAllegianceBan({playerName})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Seneschal)
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

            // any other restrictions?
            if (player.Guid.Full == Allegiance.MonarchId)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} cannot be banned from the allegiance!", ChatMessageType.Broadcast));
                return;
            }

            if (Allegiance.BanList.Contains(player.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is already banned from the allegiance.", ChatMessageType.Broadcast));
                return;
            }

            Allegiance.BanList.Add(player.Guid);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been banned from the allegiance.", ChatMessageType.Broadcast));

            // were they already a member? if so, boot them...
            if (Allegiance.Members.ContainsKey(player.Guid))
                HandleActionBreakAllegianceBoot(player.Name, false);
        }

        public void HandleActionRemoveAllegianceBan(string playerName)
        {
            //Console.WriteLine($"{Name}.HandleActionRemoveAllegianceBan({playerName})");

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Seneschal)
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

            if (!Allegiance.BanList.Contains(player.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is not banned from the allegiance.", ChatMessageType.Broadcast));
                return;
            }

            Allegiance.BanList.Remove(player.Guid);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is no longer banned from the allegiance.", ChatMessageType.Broadcast));
        }

        public void HandleActionBreakAllegianceBoot(string playerName, bool accountBoot)
        {
            //Console.WriteLine($"{Name}.HandleActionBreakAllegianceBoot({playerName}, {accountBoot})");

            // TODO: handle account boot

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (AllegiancePermissionLevel < AllegiancePermissionLevel.Seneschal)
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
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found in allegiance", ChatMessageType.Broadcast));
                return;
            }

            if (player.Guid == Guid)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot boot yourself from the allegiance.", ChatMessageType.Broadcast));
                return;
            }

            if (player.Guid.Full == Allegiance.MonarchId)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot boot the monarch from the allegiance!", ChatMessageType.Broadcast));
                return;
            }

            var patron = PlayerManager.FindByGuid(new ObjectGuid(player.PatronId ?? 0));
            if (patron == null)
            {
                Console.WriteLine($"{Name}.HandleActionBreakAllegianceBoot({player.Name}, {accountBoot}): couldn't find patron id {player.PatronId}");
                return;
            }

            player.PatronId = null;
            player.MonarchId = null;

            // rebuild allegiance tree structures
            AllegianceManager.OnBreakAllegiance(player, patron);

            // update allegiance ui panels?

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been removed from the allegiance.", ChatMessageType.Broadcast));

            var onlinePlayer = PlayerManager.GetOnlinePlayer(player.Guid);
            if (onlinePlayer != null)
                onlinePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat("You have been booted from the allegiance!", ChatMessageType.Broadcast));
        }

        public AllegiancePermissionLevel AllegiancePermissionLevel
        {
            // https://asheron.fandom.com/wiki/Allegiance_Officers

            // Level 1: Speaker

            // - Allegiance chat kick.
            // - Allegiance chat gag.
            // - Allegiance broadcast.
            // - Set/clear the MOTD.

            // Level 2: Seneschal

            // - Promote/demote members under own rank (i.e. can promote/demote speakers)
            // - Allegiance boot.
            // - Allegiance ban.
            // - Access allegiance info.
            // - Lock/unlock the allegiance.

            // Level 3: Castellan

            // - Promote/demote members to any rank, including other Castellans.
            // - Change officer titles.
            // - Set/clear the allegiance name.
            // - Set allegiance bindstone.
            // - Change mansion allegiance access/storage permissions.
            // - Bypass allegiance lock with own vassals.
            // - Bypass allegiance lock by approving particular vassals.

            get
            {
                if (Allegiance == null)
                    return AllegiancePermissionLevel.None;

                if (Allegiance.MonarchId == Guid.Full)
                    return AllegiancePermissionLevel.Monarch;

                return (AllegiancePermissionLevel)(AllegianceOfficerRank ?? 0);
            }
        }
    }
}
