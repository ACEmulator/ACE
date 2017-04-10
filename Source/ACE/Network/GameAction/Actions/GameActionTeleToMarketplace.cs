using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTeleToMarketPlace
    {
        // TODO: link to Town Network marketplace portal destination in db, when db for that is finalized and implemented.
        private static readonly Position marketplaceDrop = new Position(23855548, 49.206f, -31.935f, 0.005f, 0f, 0f, -0.7071068f, 0.7071068f); // PCAP verified drop

        [GameAction(GameActionType.TeleToMarketPlace)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            string message = $"{session.Player.Name} is recalling to the marketplace.";

            var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Recall);

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(session, PropertyInt.CombatMode, 1);

            var motionMarketplaceRecall = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.MarketplaceRecall));

            var animationEvent = new GameMessageUpdateMotion(session.Player, session, motionMarketplaceRecall);

            // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
            // FIX: I think this is only broadcasting to client, not to any other connected players. Likely animationEvent and sysChatMessage need to be broadcast to
            //      clients that are currently tracking this player and the chat message should be within local chat hearing range.
            // session.Network.EnqueueSend(updateCombatMode, animationEvent, sysChatMessage);
            session.Network.EnqueueSend(updateCombatMode);
            // session.Player.ActionAnimationEffect(motionMarketplaceRecall, session.Player);
            // session.Player.MovementEvent(motionMarketplaceRecall, session.Player);
            session.Player.ActionMovementEvent(motionMarketplaceRecall, session.Player);
            session.Network.EnqueueSend(sysChatMessage);

            session.Player.SetDelayedTeleport(TimeSpan.FromSeconds(14), marketplaceDrop);
        }
    }
}