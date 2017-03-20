
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

        // TODO: link to Town Network marketplace portal destination in db, when db for that is finalized and implemented.
        private static readonly Position marketplaceDrop = new Position(23855548, 49.16f, -31.62f, 0.10f, 0f, 0f, -0.71f, 0.71f); // Is this the right drop?

        public override void Handle()
        {
            string message = $"{Session.Player.Name} is recalling to the marketplace.";

            var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Recall);

            //TODO: This is missing the floaty animation wind up and appropriate pause before teleportation begins.

            // This is the pcap verified message sent to change just the current mana
            // Not needed in this command, leaving for example
            //var updatePlayersMana = new GameMessagePrivateUpdateAttribute2ndLevel(Session, Vital.Mana, Session.Player.Mana.Current / 2);

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.CombatMode, 1);


            Session.Network.EnqueueSend(updateCombatMode, sysChatMessage); //TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range


            // TODO: Wait until MovementEvent completes then send the following message
            Session.Player.Teleport(marketplaceDrop);
        }
    }
}
