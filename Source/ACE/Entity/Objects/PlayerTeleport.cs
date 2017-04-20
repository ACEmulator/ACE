using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Objects;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Objects
{
    public partial class Player
    {
        // TODO: link to Town Network marketplace portal destination in db, when db for that is finalized and implemented.
        private static readonly Position marketplaceDrop = new Position(23855548, 49.206f, -31.935f, 0.005f, 0f, 0f, -0.7071068f, 0.7071068f); // PCAP verified drop

        [GameAction(GameActionType.TeleToMarketPlace)]
        private void TeleToMarketPlaceAction(ClientMessage clientMessage)
        {
            string message = $"{Name} is recalling to the marketplace.";

            var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Recall);

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CombatMode, 1);

            var motionMarketplaceRecall = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.MarketplaceRecall));

            var animationEvent = new GameMessageUpdateMotion(this, Session, motionMarketplaceRecall);

            // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
            // FIX: Recall text isn't being broadcast yet, need to address
            Session.EnqueueSend(updateCombatMode, sysChatMessage);
            EnqueueMovementEvent(motionMarketplaceRecall, Session.Player.Guid);

            SetDelayedTeleport(TimeSpan.FromSeconds(14), marketplaceDrop);
        }

        [GameAction(GameActionType.TeleToLifestone)]
        private void TeleToLifestoneAction(ClientMessage message)
        {
            if (Positions.ContainsKey(PositionType.Sanctuary))
            {
                // Session.Player.Teleport(Session.Player.Positions[PositionType.Sanctuary]);
                string msg = $"{Session.Player.Name} is recalling to the lifestone.";

                var sysChatMessage = new GameMessageSystemChat(msg, ChatMessageType.Recall);

                Mana.Current = Mana.Current / 2;
                var updatePlayersMana = new GameMessagePrivateUpdateAttribute2ndLevel(Session, Vital.Mana, Mana.Current);

                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CombatMode, 1);

                var motionLifestoneRecall = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.LifestoneRecall));

                var animationEvent = new GameMessageUpdateMotion(this, Session, motionLifestoneRecall);

                // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
                // FIX: Recall text isn't being broadcast yet, need to address
                Session.EnqueueSend(updatePlayersMana, updateCombatMode, sysChatMessage);
                EnqueueMovementEvent(motionLifestoneRecall, Session.Player.Guid);

                SetDelayedTeleport(TimeSpan.FromSeconds(14), Session.Player.Positions[PositionType.Sanctuary]);
            }
            else
            {
                ChatPacket.SendServerMessage(Session, "Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast);
            }
        }
    }
}
