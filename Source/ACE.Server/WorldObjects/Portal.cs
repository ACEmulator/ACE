using System;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public partial class Portal : WorldObject
    {
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
            BaseDescriptionFlags |= ObjectDescriptionFlag.Portal;

            UpdatePortalDestination(Destination);
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

            if (!player.IgnorePortalRestrictions)
            {
#if DEBUG
                // player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Checking requirements for {Name}", ChatMessageType.System));
#endif

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
            }

            // handle quest requirements
            if (Quest != null)
            {
                /*if (player.QuestManager.CanSolve(Quest))
                {
                    player.QuestManager.Update(Quest);
                }
                else
                {
                    player.QuestManager.HandleSolveError(Quest);
                    return;
                }*/

                // only for initial flagging?
                if (!player.QuestManager.HasQuest(Quest))
                    player.QuestManager.Update(Quest);
            }

            if (QuestRestriction != null && !player.QuestManager.HasQuest(QuestRestriction) && !player.IgnorePortalRestrictions)
            {
                player.QuestManager.HandleNoQuestError(this);
                return new ActivationResult(false);
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
            player.AdjustDungeon(portalDest);

            player.Teleport(portalDest);

            // If the portal just used is able to be recalled to,
            // save the destination coordinates to the LastPortal character position save table
            if (!NoRecall)
                player.LastPortalDID = OriginalPortal == null ? WeenieClassId : OriginalPortal;     // if walking through a summoned portal

            EmoteManager.OnPortal(player);
        }
    }
}
