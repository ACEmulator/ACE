using System;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Teleports the player to position
        /// </summary>
        /// <param name="position">PositionType to be teleported to</param>
        /// <returns>true on success (position is set) false otherwise</returns>
        public bool TeleToPosition(PositionType position)
        {
            if (Positions.ContainsKey(position))
            {
                Position dest = Positions[position];
                Teleport(dest);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handles teleporting a player to the lifestone (/ls or /lifestone command)
        /// </summary>
        public void TeleToLifestone()
        {
            if (Positions.ContainsKey(PositionType.Sanctuary))
            {
                // FIXME(ddevec): I should probably make a better interface for this
                UpdateVitalInternal(Mana, Mana.Current / 2);

                if (CombatMode != CombatMode.NonCombat)
                {
                    var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.CombatMode, (int)CombatMode.NonCombat);
                    Session.Network.EnqueueSend(updateCombatMode);
                }

                var motionLifestoneRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.LifestoneRecall));
                // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
                CurrentLandblock.EnqueueBroadcastSystemChat(this, $"{Name} is recalling to the lifestone.", ChatMessageType.Recall);
                // FIX: Recall text isn't being broadcast yet, need to address
                DoMotion(motionLifestoneRecall);

                // Wait for animation
                ActionChain lifestoneChain = new ActionChain();
                var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
                float lifestoneAnimationLength = motionTable.GetAnimationLength(MotionCommand.LifestoneRecall);

                // Then do teleport
                lifestoneChain.AddDelaySeconds(lifestoneAnimationLength);
                lifestoneChain.AddChain(GetTeleportChain(Positions[PositionType.Sanctuary]));

                lifestoneChain.EnqueueChain();
            }
            else
            {
                ChatPacket.SendServerMessage(Session, "Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast);
            }
        }

        public void TeleToMarketplace()
        {
            string message = $"{Name} is recalling to the marketplace.";

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.CombatMode, (int)CombatMode.NonCombat);

            var motionMarketplaceRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.MarketplaceRecall));

            var animationEvent = new GameMessageUpdateMotion(Guid, Sequences.GetCurrentSequence(SequenceType.ObjectInstance), Sequences, motionMarketplaceRecall);

            // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
            // FIX: Recall text isn't being broadcast yet, need to address
            CurrentLandblock.EnqueueBroadcastSystemChat(this, message, ChatMessageType.Recall);
            Session.Network.EnqueueSend(updateCombatMode);
            DoMotion(motionMarketplaceRecall);

            // TODO: (OptimShi): Actual animation length is longer than in retail. 18.4s
            // float mpAnimationLength = MotionTable.GetAnimationLength((uint)MotionTableId, MotionCommand.MarketplaceRecall);
            // mpChain.AddDelaySeconds(mpAnimationLength);
            ActionChain mpChain = new ActionChain();
            mpChain.AddDelaySeconds(14);

            // Then do teleport
            mpChain.AddChain(GetTeleportChain(MarketplaceDrop));

            // Set the chain to run
            mpChain.EnqueueChain();
        }

        public void Teleport(Position newPosition)
        {
            ActionChain chain = GetTeleportChain(newPosition);
            chain.EnqueueChain();
        }

        private ActionChain GetTeleportChain(Position newPosition)
        {
            ActionChain teleportChain = new ActionChain();

            teleportChain.AddAction(this, () => TeleportInternal(newPosition));

            teleportChain.AddDelaySeconds(3);
            // Once back in world we can start listening to the game's request for positions
            teleportChain.AddAction(this, () => InWorld = true);

            return teleportChain;
        }

        private void TeleportInternal(Position newPosition)
        {
            if (!InWorld)
                return;

            Hidden = true;
            IgnoreCollisions = true;
            ReportCollisions = false;
            EnqueueBroadcastPhysicsState();
            ExternalUpdatePosition(newPosition);
            InWorld = false;

            Teleporting = true;

            Session.Network.EnqueueSend(new GameMessagePlayerTeleport(this));
            //CurrentLandblock.RemoveWorldObject(Guid, false); // Reasonably sure this is the culprit of the failed teleports.

            lock (clientObjectList)
                clientObjectList.Clear();
        }

        private Position PositionSanctuary
        {
            get
            {
                if (Positions.ContainsKey(PositionType.Sanctuary))
                {
                    return Positions[PositionType.Sanctuary];
                }
                return null;
            }
            set => Positions[PositionType.Sanctuary] = value;
        }

        private Position PositionLastPortal
        {
            get
            {
                if (Positions.ContainsKey(PositionType.LastPortal))
                {
                    return Positions[PositionType.LastPortal];
                }
                return null;
            }
            set => Positions[PositionType.LastPortal] = value;
        }
    }
}
