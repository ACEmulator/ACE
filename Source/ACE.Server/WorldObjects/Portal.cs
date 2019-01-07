using System;
using System.Numerics;
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
    public class Portal : WorldObject
    {
        // private byte portalSocietyId;

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

            MinLevel = MinLevel ?? 0;
            MaxLevel = MaxLevel ?? 0;

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

        public string AppraisalPortalDestination
        {
            get => GetProperty(PropertyString.AppraisalPortalDestination);
            set { if (value == null) RemoveProperty(PropertyString.AppraisalPortalDestination); else SetProperty(PropertyString.AppraisalPortalDestination, value); }
        }

        public bool? PortalShowDestination
        {
            get => GetProperty(PropertyBool.PortalShowDestination);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.PortalShowDestination); else SetProperty(PropertyBool.PortalShowDestination, value.Value); }
        }

        public int? MinLevel
        {
            get => GetProperty(PropertyInt.MinLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MinLevel); else SetProperty(PropertyInt.MinLevel, value.Value); }
        }

        public int? MaxLevel
        {
            get => GetProperty(PropertyInt.MaxLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MaxLevel); else SetProperty(PropertyInt.MaxLevel, value.Value); }
        }

        public PortalBitmask PortalRestrictions
        {
            get => (PortalBitmask)(GetProperty(PropertyInt.PortalBitmask) ?? (int)PortalBitmask.Unrestricted);
            set { if (value == PortalBitmask.Undef) RemoveProperty(PropertyInt.PortalBitmask); else SetProperty(PropertyInt.PortalBitmask, (int)value); }
        }

        public int SocietyId => 0;

        public bool NoRecall => (PortalRestrictions & PortalBitmask.NoRecall) != 0;

        public bool NoSummon => (PortalRestrictions & PortalBitmask.NoSummon) != 0;

        public bool NoTie => NoRecall;

        private void ActivatePortal(Player player)
        {
            if (player.Teleporting) return;

            if (Destination == null)
            {
                var msg = new GameMessageSystemChat($"Portal destination for portal ID {WeenieClassId} not yet implemented!", ChatMessageType.System);
                player.Session.Network.EnqueueSend(msg);
                return;
            }

            if (!player.IgnorePortalRestrictions)
            {
#if DEBUG
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Checking requirements for {Name}", ChatMessageType.System));
#endif

                if (player.Level < MinLevel)
                {
                    // You are not powerful enough to interact with that portal!
                    var failedUsePortalMessage = new GameEventWeenieError(player.Session, WeenieError.YouAreNotPowerfulEnoughToUsePortal);
                    player.Session.Network.EnqueueSend(failedUsePortalMessage);
                    return;
                }

                if (player.Level > MaxLevel && MaxLevel != 0)
                {
                    // You are too powerful to interact with that portal!
                    var failedUsePortalMessage = new GameEventWeenieError(player.Session, WeenieError.YouAreTooPowerfulToUsePortal);
                    player.Session.Network.EnqueueSend(failedUsePortalMessage);
                    return;
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
                return;
            }

            // everything looks good, teleport
            EmoteManager.OnUse(player);

#if DEBUG
            player.Session.Network.EnqueueSend(new GameMessageSystemChat("Portal sending player to destination", ChatMessageType.System));
#endif
            var portalDest = new Position(Destination);
            player.AdjustDungeon(portalDest);

            player.Teleport(portalDest);

            // If the portal just used is able to be recalled to,
            // save the destination coordinates to the LastPortal character position save table
            if (!NoRecall)
                player.LastPortalDID = WeenieClassId;

            EmoteManager.OnPortal(player);
        }

        public virtual void OnCollideObject(Player player)
        {
            ActivatePortal(player);
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            if (worldObject is Player player)
            {
                if (ReportCollisions == false)
                    ActivatePortal(player);

                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            }
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            if (wo.IsLinkSpot)
                SetPosition(PositionType.Destination, new Position(wo.Location));
        }
    }
}
