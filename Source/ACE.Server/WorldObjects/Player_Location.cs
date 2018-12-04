using System;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private static readonly Position MarketplaceDrop = DatabaseManager.World.GetCachedWeenie("portalmarketplace").GetPosition(PositionType.Destination);

        /// <summary>
        /// Teleports the player to position
        /// </summary>
        /// <param name="positionType">PositionType to be teleported to</param>
        /// <returns>true on success (position is set) false otherwise</returns>
        public bool TeleToPosition(PositionType positionType)
        {
            var position = GetPosition(positionType);

            if (position != null)
            {
                Teleport(position);
                return true;
            }

            return false;
        }

        private static readonly Motion motionLifestoneRecall = new Motion(MotionStance.NonCombat, MotionCommand.LifestoneRecall);

        /// <summary>
        /// Handles teleporting a player to the lifestone (/ls or /lifestone command)
        /// </summary>
        public void HandleActionTeleToLifestone()
        {
            if (Sanctuary != null)
            {
                // FIXME(ddevec): I should probably make a better interface for this
                UpdateVital(Mana, Mana.Current / 2);

                if (CombatMode != CombatMode.NonCombat)
                {
                    // this should be handled by a different thing, probably a function that forces player into peacemode
                    var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);
                    SetCombatMode(CombatMode.NonCombat);
                    Session.Network.EnqueueSend(updateCombatMode);
                }

                EnqueueBroadcast(new GameMessageSystemChat($"{Name} is recalling to the lifestone.", ChatMessageType.Recall));
                EnqueueBroadcastMotion(motionLifestoneRecall);

                // Wait for animation
                ActionChain lifestoneChain = new ActionChain();

                // Then do teleport
                lifestoneChain.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.LifestoneRecall));
                lifestoneChain.AddAction(this, () => Teleport(Sanctuary));

                lifestoneChain.EnqueueChain();
            }
            else
            {
                ChatPacket.SendServerMessage(Session, "Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast);
            }
        }

        private static readonly Motion motionMarketplaceRecall = new Motion(MotionStance.NonCombat, MotionCommand.MarketplaceRecall);

        public void HandleActionTeleToMarketPlace()
        {
            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);

            EnqueueBroadcast(new GameMessageSystemChat($"{Name} is recalling to the marketplace.", ChatMessageType.Recall));
            Session.Network.EnqueueSend(updateCombatMode); // this should be handled by a different thing, probably a function that forces player into peacemode
            EnqueueBroadcastMotion(motionMarketplaceRecall);

            // TODO: (OptimShi): Actual animation length is longer than in retail. 18.4s
            // float mpAnimationLength = MotionTable.GetAnimationLength((uint)MotionTableId, MotionCommand.MarketplaceRecall);
            // mpChain.AddDelaySeconds(mpAnimationLength);
            ActionChain mpChain = new ActionChain();
            mpChain.AddDelaySeconds(14);

            // Then do teleport
            mpChain.AddAction(this, () => Teleport(MarketplaceDrop));

            // Set the chain to run
            mpChain.EnqueueChain();
        }

        public void Teleport(Position newPosition)
        {
            if (!InWorld)
                return;

            InWorld = false;
            Teleporting = true;

            Session.Network.EnqueueSend(new GameMessagePlayerTeleport(this));

            // load quickly, but player can load into landblock before server is finished loading
            //Location = newPosition;
            //SendUpdatePosition();

            UpdatePlayerPhysics(newPosition, true);

            Hidden = true;
            IgnoreCollisions = true;
            ReportCollisions = false;
            EnqueueBroadcastPhysicsState();

            // force out of hotspots
            PhysicsObj.report_collision_end(true);
        }

        public void OnTeleportComplete()
        {
            // set materialize physics state
            // this takes the player from pink bubbles -> fully materialized
            ReportCollisions = true;
            IgnoreCollisions = false;
            Hidden = false;

            EnqueueBroadcastPhysicsState();

            Teleporting = false;
            InWorld = true;
        }

        public void NotifyLandblocks()
        {
            // the original implementations of this were done on landblock heartbeat,
            // with checks for players in the current landblock, as well as adjacent outdoor landblocks

            // for performance reasons, this is being reimplemented in the reverse manner,
            // with players notifying landblocks of their activity

            // notify current landblock of player activity
            if (CurrentLandblock != null)
                CurrentLandblock?.SetActive();
        }
    }
}
