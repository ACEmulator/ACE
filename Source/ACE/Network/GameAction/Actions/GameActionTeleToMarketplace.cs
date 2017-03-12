
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.TeleToMarketPlace)]
    public class GameActionTeleToMarketPlace : GameActionPacket
    {
        public GameActionTeleToMarketPlace(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            Position marketplaceDrop = new Position(23855548, 49.16f, -31.62f, 0.10f, 0f, 0f, -0.71f, 0.71f); // Is this the right drop?
            string message = $"{Session.Player.Name} is recalling to the marketplace.";

            var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Recall);

            //TODO: This is missing the floaty animation wind up and appropriate pause before teleportation begins.


            // Is this the right way to do the half mana depletion, I feel like there's probably a better/smarter way?
            //Session.Player.Mana.Current = Session.Player.Mana.Current / 2;
            //var player = new GameEventPlayerDescription(Session);

            // Is THIS the right way to do the half mana depletion, seems smarter to me...
            //var updatePlayersMana = new GameMessagePrivateUpdateVital(Session, Ability.Mana, Session.Player.Mana.Ranks, Session.Player.Mana.Base, Session.Player.Mana.ExperienceSpent, Session.Player.Mana.Current / 2);


            // This is the pcap verified message sent to change just the current mana
            //var updatePlayersMana = new GameMessagePrivateUpdateAttribute2ndLevel(Session, Vital.Mana, Session.Player.Mana.Current / 2);


            // I'm now thinking there was no mana cost to teleport to marketplace

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CombatMode, 1);

            //var motionMareketPlaceTeleTo = new GameMessageMovementEvent(Session.Player.Guid, 0, 0, 0, 0, Enum.MovementType.Invalid, new MovementParameters(0, 0, 0, 0, 0, 0, 0), Enum.MotionStyle.Motion_NonCombat, new InterpretedMotionState(128, Enum.MotionStyle.Motion_NonCombat, Enum.MotionStyle.Motion_Ready, Enum.MotionStyle.Command_Invalid, Enum.MotionStyle.Command_Invalid, 1, 1, 1), 0, 0, new Position(0, 0, 0, 0, 0, 0, 0, 1), 0, 0, 0);
            //var motionMareketPlaceTeleTo = new GameMessageMovementEvent(Session.Player.Guid, 0, 0, 0, 0, Enum.MovementType.Invalid, new MovementParameters(0, 0, 0, 0, 0, 0, 0), Enum.MotionStyle.Motion_NonCombat, new InterpretedMotionState(128, Enum.MotionStyle.Motion_NonCombat, Enum.MotionStyle.Motion_Ready, Enum.MotionStyle.Command_Invalid, Enum.MotionStyle.Command_Invalid, 1, 1, 1), 0, 0, new Position(0, 0, 0, 0, 0, 0, 0, 1), 0, 0, 0);

            //Session.WorldSession.EnqueueSend(motionMareketPlaceTeleTo);

            //Session.WorldSession.EnqueueSend(updateCombatMode, motionMareketPlaceTeleTo, sysChatMessage); //TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range

            //Session.WorldSession.EnqueueSend(updateCombatMode, updatePlayersMana, sysChatMessage); //TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range


            Session.WorldSession.EnqueueSend(updateCombatMode, sysChatMessage); //TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range


            // TODO: Wait until MovementEvent completes then send the following message
            Session.Player.Teleport(marketplaceDrop);
        }
    }
}
