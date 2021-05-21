using System.Numerics;

using log4net;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public partial class Portal : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Portal(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Portal(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        protected void SetEphemeralValues()
        {
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Portal;

            UpdatePortalDestination(Destination);
        }

        public override bool EnterWorld()
        {
            var success = base.EnterWorld();

            if (!success)
            {
                log.Error($"{Name} ({Guid}) failed to spawn @ {Location?.ToLOCString()}");
                return false;
            }

            if (RelativeDestination != null && Location != null && Destination == null)
            {
                var relativeDestination = new Position(Location);
                relativeDestination.Pos += new Vector3(RelativeDestination.PositionX, RelativeDestination.PositionY, RelativeDestination.PositionZ);
                relativeDestination.Rotation = new Quaternion(RelativeDestination.RotationX, relativeDestination.RotationY, relativeDestination.RotationZ, relativeDestination.RotationW);
                relativeDestination.LandblockId = new LandblockId(relativeDestination.GetCell());

                UpdatePortalDestination(relativeDestination);
            }

            return true;
        }

        public void UpdatePortalDestination(Position destination)
        {
            Destination = destination;

            if (PortalShowDestination ?? true)
            {
                AppraisalPortalDestination = Name;

                if (Destination != null)
                {
                    var destCoords = Destination.GetMapCoordStr();
                    if (destCoords != null)
                        AppraisalPortalDestination += $" ({destCoords}).";
                }
            }
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            if (wo.IsLinkSpot)
                SetPosition(PositionType.Destination, new Position(wo.Location));
        }

        public bool IsGateway { get => WeenieClassId == 1955; }

        //public override void OnActivate(WorldObject activator)
        //{
        //    if (activator is Creature creature)
        //        EmoteManager.OnUse(creature);

        //    base.OnActivate(activator);
        //}

        public virtual void OnCollideObject(Player player)
        {
            OnActivate(player);
        }

        public override void OnCastSpell(WorldObject activator)
        {
            if (SpellDID.HasValue)
                base.OnCastSpell(activator);
            else
                ActOnUse(activator);
        }

        /// <summary>
        /// If a player tries to use 2 portals in under this amount of time,
        /// they receive an error message
        /// </summary>
        private static readonly float minTimeSinceLastPortal = 5.0f;

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            if (!(activator is Player player))
                return new ActivationResult(false);

            if (player.Teleporting)
                return new ActivationResult(false);

            if (Destination == null)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Portal destination for portal ID {WeenieClassId} not yet implemented!", ChatMessageType.System));
                return new ActivationResult(false);
            }

            if (player.LastPortalTeleportTimestamp != null)
            {
                var currentTime = Time.GetUnixTime();

                var timeSinceLastPortal = currentTime - player.LastPortalTeleportTimestamp.Value;

                if (timeSinceLastPortal < minTimeSinceLastPortal)
                {
                    // prevent message spam
                    if (player.LastPortalTeleportTimestampError != null)
                    {
                        var timeSinceLastPortalError = currentTime - player.LastPortalTeleportTimestampError.Value;

                        if (timeSinceLastPortalError < minTimeSinceLastPortal)
                            return new ActivationResult(false);
                    }

                    player.LastPortalTeleportTimestampError = currentTime;

                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouHaveBeenTeleportedTooRecently));
                }
            }

            if (player.PKTimerActive && !PortalIgnoresPkAttackTimer)
            {
                return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
            }

            if (!player.IgnorePortalRestrictions)
            {
                if (player.Level < MinLevel)
                {
                    // You are not powerful enough to interact with that portal!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouAreNotPowerfulEnoughToUsePortal));
                }

                if (player.Level > MaxLevel && MaxLevel != 0)
                {
                    // You are too powerful to interact with that portal!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouAreTooPowerfulToUsePortal));
                }

                //var playerPkLevel = player.PkLevel;

                //if (PropertyManager.GetBool("pk_server").Item)
                //    playerPkLevel = PKLevel.PK;
                //else if (PropertyManager.GetBool("pkl_server").Item)
                //    playerPkLevel = PKLevel.PKLite;

                if (PortalRestrictions == PortalBitmask.Undef)
                {
                    // Players may not interact with that portal.
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.PlayersMayNotUsePortal));
                }

                if (PortalRestrictions.HasFlag(PortalBitmask.NoPk) && player.PlayerKillerStatus == PlayerKillerStatus.PK)
                {
                    // Player killers may not interact with that portal!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.PKsMayNotUsePortal));
                }

                if (PortalRestrictions.HasFlag(PortalBitmask.NoPKLite) && player.PlayerKillerStatus == PlayerKillerStatus.PKLite)
                {
                    // Lite Player Killers may not interact with that portal!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.PKLiteMayNotUsePortal));
                }

                if (PortalRestrictions.HasFlag(PortalBitmask.NoNPK) && player.PlayerKillerStatus == PlayerKillerStatus.NPK)
                {
                    // Non-player killers may not interact with that portal!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.NonPKsMayNotUsePortal));
                }

                if (PortalRestrictions.HasFlag(PortalBitmask.OnlyOlthoiPCs) && !player.IsOlthoiPlayer())
                {
                    // Only Olthoi may pass through this portal!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.OnlyOlthoiMayUsePortal));
                }

                if (PortalRestrictions.HasFlag(PortalBitmask.NoOlthoiPCs) && player.IsOlthoiPlayer())
                {
                    // Olthoi may not pass through this portal!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.OlthoiMayNotUsePortal));
                }

                if (PortalRestrictions.HasFlag(PortalBitmask.NoVitae) && player.HasVitae)
                {
                    // You may not pass through this portal while Vitae weakens you!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouMayNotUsePortalWithVitae));
                }

                if (PortalRestrictions.HasFlag(PortalBitmask.NoNewAccounts) && !player.Account15Days)
                {
                    // This character must be two weeks old or have been created on an account at least two weeks old to use this portal!
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouMustBeTwoWeeksOldToUsePortal));
                }

                if (player.AccountRequirements < AccountRequirements)
                {
                    // You must purchase Asheron's Call -- Throne of Destiny to use this portal.
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.MustPurchaseThroneOfDestinyToUsePortal));
                }

                if ((AdvocateQuest ?? false) && !player.IsAdvocate)
                {
                    // You must be an Advocate to interact with that portal.
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouMustBeAnAdvocateToUsePortal));
                }
            }

            if (QuestRestriction != null && !player.IgnorePortalRestrictions)
            {
                var hasQuest = player.QuestManager.HasQuest(QuestRestriction);
                var canSolve = player.QuestManager.CanSolve(QuestRestriction);

                var success = hasQuest && !canSolve;

                if (!success)
                {
                    player.QuestManager.HandlePortalQuestError(QuestRestriction);
                    return new ActivationResult(false);
                }
            }

            // handle quest initial flagging
            if (Quest != null)
            {
                EmoteManager.OnQuest(player);
            }

            return new ActivationResult(true);
        }

        public override void ActOnUse(WorldObject activator)
        {
            var player = activator as Player;
            if (player == null) return;

#if DEBUG
            // player.Session.Network.EnqueueSend(new GameMessageSystemChat("Portal sending player to destination", ChatMessageType.System));
#endif
            var portalDest = new Position(Destination);
            AdjustDungeon(portalDest);

            WorldManager.ThreadSafeTeleport(player, portalDest, new ActionEventDelegate(() =>
            {
                // If the portal just used is able to be recalled to,
                // save the destination coordinates to the LastPortal character position save table
                if (!NoRecall)
                    player.LastPortalDID = OriginalPortal == null ? WeenieClassId : OriginalPortal; // if walking through a summoned portal

                EmoteManager.OnPortal(player);

                player.SendWeenieError(WeenieError.ITeleported);

            }), true);
        }
    }
}
