using System;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

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

        /// <summary>
        /// Called when a player tries to Swear Allegiance to a target
        /// </summary>
        /// <param name="targetGuid">The target this player is attempting to swear allegiance to</param>
        public void HandleActionSwearAllegiance(uint targetGuid)
        {
            if (!IsPledgable(targetGuid)) return;

            var patron = PlayerManager.GetOnlinePlayer(targetGuid);

            Patron = targetGuid;
            Monarch = AllegianceManager.GetMonarch(patron).Guid.Full;

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
            var isPatron = Patron == target.Guid.Full;
            var isVassal = target.Patron == Guid.Full;

            // break ties
            if (isVassal)
            {
                target.Patron = null;
                target.Monarch = null;
            }
            else
            {
                Patron = null;
                Monarch = null;
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
            if (Patron != null)
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
            var isPatron = Patron == target.Guid.Full;
            var isVassal = target.Patron == Guid.Full;

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
    }
}
