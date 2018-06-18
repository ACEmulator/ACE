using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public Allegiance Allegiance;
        public AllegianceNode AllegianceNode;

        // TODO: write to db
        public ulong CPTithed;
        public ulong CPCached;
        public ulong CPPoolToUnload { get => (ulong)(AllegianceCPPool ?? 0); set => AllegianceCPPool = (int)value; }

        public bool HasAllegiance { get => Allegiance != null && Allegiance.TotalMembers > 1; }

        /// <summary>
        /// Called when a player tries to Swear Allegiance to a target
        /// </summary>
        /// <param name="targetGuid">The target this player is attempting to swear allegiance to</param>
        public void HandleActionSwearAllegiance(ObjectGuid targetGuid)
        {
            if (!IsPledgable(targetGuid)) return;

            var patron = WorldManager.GetPlayerByGuidId(targetGuid.Full);

            Patron = targetGuid.Full;
            Monarch = AllegianceManager.GetMonarch(patron).Guid.Full;
            WorldManager.SyncOffline(this);

            Console.WriteLine("Patron: " + WorldManager.GetOfflinePlayerByGuidId(Patron.Value).Name);
            Console.WriteLine("Monarch: " + WorldManager.GetOfflinePlayerByGuidId(Monarch.Value).Name);

            // send message to patron:
            // %vassal% has sworn Allegiance to you.
            patron.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has sworn Allegiance to you.", ChatMessageType.Broadcast));

            // send message to vassal:
            // %patron% has accepted your oath of Allegiance!
            // Motion_Kneel
            Session.Network.EnqueueSend(new GameMessageSystemChat($"{patron.Name} has accepted your oath of Allegiance!", ChatMessageType.Broadcast));

            var motion = new UniversalMotion(CurrentMotionState.Stance, new MotionItem(MotionCommand.Kneel));
            CurrentLandblock.EnqueueBroadcastMotion(this, motion);

            // rebuild allegiance tree structure
            AllegianceManager.OnSwearAllegiance(this);

            // refresh ui panel
            Session.Network.EnqueueSend(new GameEventAllegianceUpdate(Session, Allegiance, AllegianceNode), new GameEventAllegianceAllegianceUpdateDone(Session));
        }

        /// <summary>
        /// Called when a player tries to break Allegiance to a target
        /// </summary>
        /// <param name="targetGuid">The target this player is attempting to break allegiance from</param>
        public void HandleActionBreakAllegiance(ObjectGuid targetGuid)
        {
            if (!IsBreakable(targetGuid)) return;

            var target = WorldManager.GetOfflinePlayerByGuidId(targetGuid.Full);

            Console.WriteLine(Name + " breaking allegiance to " + target.Name);

            // target can be either patron or vassal
            var isPatron = Patron == target.Guid.Full;
            var isVassal = target.Patron == Guid.Full;

            // break ties
            if (isVassal)
            {
                target.Patron = null;
                target.Monarch = null;
                WorldManager.SyncOffline(target);
            }
            else
            {
                Patron = null;
                Monarch = null;
                WorldManager.SyncOffline(this);
            }

            // send message to target if online
            var onlineTarget = WorldManager.GetPlayerByGuidId(targetGuid.Full, true);
            if (onlineTarget != null)
                onlineTarget.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has broken their Allegiance to you!", ChatMessageType.Broadcast));

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
        public bool IsPledgable(ObjectGuid targetGuid)
        {
            // ensure target player is online, and within range
            var target = WorldManager.GetPlayerByGuidId(targetGuid.Full);
            if (target == null)
            {
                Console.WriteLine(Name + " tried to swear to an unknown player guid: " + targetGuid.Full.ToString("X8"));
                return false;
            }

            // player already sworn?
            if (Patron != null)
            {
                Console.WriteLine(Name + " tried to swear to " + target.Name + ", but is already sworn to " + WorldManager.GetOfflinePlayerByGuidId(Patron.Value).Name);
                return false;
            }

            // player can't swear to themselves
            if (targetGuid.Full == Guid.Full)
            {
                Console.WriteLine(Name + " tried to swear to themselves");
                return false;
            }

            // patron must currently be greater or equal level
            if (target.Level < Level)
            {
                Console.WriteLine(Name + " tried to swear to a lower level character");
                return false;
            }

            var selfNode = AllegianceNode;
            var targetNode = target.AllegianceNode;

            if (targetNode != null)
            {
                // maximum # of direct vassals = 11
                if (targetNode.TotalVassals >= 11)
                {
                    Console.WriteLine(target.Name + " already has the maximum # of vassals");
                    return false;
                }

                // 2 players can't swear to each other
                // prevent any loops in the allegiance chain
                if (selfNode != null && selfNode.IsMonarch)
                {
                    if (selfNode.Player.Guid.Full == targetNode.Monarch.Player.Guid.Full)
                    {
                        Console.WriteLine(Name + " tried to swear to someone already in Allegiance: " + target.Name);
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
        public bool IsBreakable(ObjectGuid targetGuid)
        {
            // players can break from either vassals or patrons

            // ensure target player exists
            var target = WorldManager.GetOfflinePlayerByGuidId(targetGuid.Full);
            if (target == null)
            {
                Console.WriteLine(Name + " tried to break allegiance to an unknown player guid: " + targetGuid.Full.ToString("X8"));
                return false;
            }

            // verify patron or vassal
            var isPatron = Patron == target.Guid.Full;
            var isVassal = target.Patron == Guid.Full;

            if (!isPatron && !isVassal)
            {
                Console.WriteLine(Name + " tried to break allegiance from " + target.Name + ", but they aren't patron or vassal");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds any pending XP in CPPoolToUnload to the player's total XP
        /// </summary>
        /// <param name="showMsg">Set to TRUE if player is logging in</param>
        public void AddCPPoolToUnload(bool showMsg = false)
        {
            var patron = WorldManager.GetPlayerByGuidId(Guid.Full);

            // is player logged in?
            if (patron == null || !patron.IsOnline) return;

            if (CPPoolToUnload == 0) return;

            // FIXME: should be uint?
            patron.EarnXP((long)CPPoolToUnload, false);

            if (showMsg)
                patron.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your Vassals have produced experience points for you.\nTaking your skills as a leader into account, you gain {CPPoolToUnload} xp.", ChatMessageType.Broadcast));

            CPPoolToUnload = 0;     // force save?
        }
    }
}
