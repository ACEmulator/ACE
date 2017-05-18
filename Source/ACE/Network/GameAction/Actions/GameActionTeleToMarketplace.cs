using ACE.Entity;
using ACE.Entity.PlayerActions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
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
            session.Player.RequestAction(() => session.Player.ActDelayedTeleport($"{session.Player.Name} is recalling to the marketplace.",
                new MotionItem(MotionCommand.MarketplaceRecall), marketplaceDrop));
            // Old event-based architecture
            /*
            string message = $"{session.Player.Name} is recalling to the marketplace.";

            var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Recall);

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(session, PropertyInt.CombatMode, 1);

            var motionMarketplaceRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.MarketplaceRecall));

            var animationEvent = new GameMessageUpdateMotion(session.Player.Guid,
                                                             session.Player.Sequences.GetCurrentSequence(Sequence.SequenceType.ObjectInstance),
                                                             session.Player.Sequences, motionMarketplaceRecall);

            // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
            // FIX: Recall text isn't being broadcast yet, need to address
            session.Network.EnqueueSend(updateCombatMode, sysChatMessage);
            session.Player.EnqueueMovementEvent(motionMarketplaceRecall, session.Player.Guid);

            session.Player.SetDelayedTeleport(TimeSpan.FromSeconds(14), marketplaceDrop);
            */
        }
    }
}