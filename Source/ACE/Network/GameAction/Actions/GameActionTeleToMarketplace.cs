
using ACE.Entity.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.TeleTo_MarketPlace)]
    public class GameActionTeleToMarketPlace : GameActionPacket
    {
        public GameActionTeleToMarketPlace(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            Entity.Position marketplaceDrop = new Entity.Position(23855548, 49.16f, -31.62f, 0.10f, 0f, 0f, -0.71f, 0.71f); // Is this the right drop?
            string message = $"{Session.Player.Name} is recalling to the marketplace.";

            var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Recall);

            //TODO: This is missing the floaty animation wind up and appropriate pause before teleportation begins.


            // Is this the right way to do the half mana depletion, I feel like there's probably a better/smarter way?
            //Session.Player.Mana.Current = Session.Player.Mana.Current / 2;
            //var player = new GameEventPlayerDescription(Session);

            // Is THIS the right way to do the half mana depletion, seems smarter to me...
            var updatePlayersMana = new GameMessagePrivateUpdateVital(Session, Ability.Mana, Session.Player.Mana.Ranks, Session.Player.Mana.Base, Session.Player.Mana.ExperienceSpent, Session.Player.Mana.Current / 2);

            Session.WorldSession.EnqueueSend(updatePlayersMana, sysChatMessage); //TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range

            Session.Player.Teleport(marketplaceDrop);
        }
    }
}