
using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private static readonly Position MarketplaceDrop = DatabaseManager.World.GetCachedWeenie("portalmarketplace").GetPosition(PositionType.Destination);

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

        private static readonly UniversalMotion motionLifestoneRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.LifestoneRecall));

        /// <summary>
        /// Handles teleporting a player to the lifestone (/ls or /lifestone command)
        /// </summary>
        public void HandleActionTeleToLifestone()
        {
            if (Sanctuary != null)
            {
                // FIXME(ddevec): I should probably make a better interface for this
                UpdateVitalInternal(Mana, Mana.Current / 2);

                if (CombatMode != CombatMode.NonCombat)
                {
                    // this should be handled by a different thing, probably a function that forces player into peacemode
                    var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);
                    Session.Network.EnqueueSend(updateCombatMode);
                }
                 
                CurrentLandblock.EnqueueBroadcastSystemChat(this, $"{Name} is recalling to the lifestone.", ChatMessageType.Recall);
                CurrentLandblock.EnqueueBroadcastMotion(this, motionLifestoneRecall);

                // Wait for animation
                ActionChain lifestoneChain = new ActionChain();

                // Then do teleport
                lifestoneChain.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.LifestoneRecall));
                lifestoneChain.AddChain(GetTeleportChain(Sanctuary));

                lifestoneChain.EnqueueChain();
            }
            else
            {
                ChatPacket.SendServerMessage(Session, "Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast);
            }
        }

        private static readonly UniversalMotion motionMarketplaceRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.MarketplaceRecall));

        public void HandleActionTeleToMarketPlace()
        {
            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);

            CurrentLandblock.EnqueueBroadcastSystemChat(this, $"{Name} is recalling to the marketplace.", ChatMessageType.Recall);
            Session.Network.EnqueueSend(updateCombatMode); // this should be handled by a different thing, probably a function that forces player into peacemode
            CurrentLandblock.EnqueueBroadcastMotion(this, motionMarketplaceRecall);

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

            // should this be a thing? seems like when the client sends GameActionLoginComplete that should be our signal for InWorld=True ....
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
    }
}
