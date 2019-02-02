using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionDoAllegianceHouseAction
    {
        [GameAction(GameActionType.DoAllegianceHouseAction)]
        public static void Handle(ClientMessage message, Session session)
        {
            var action = (AllegianceHouseAction)message.Payload.ReadUInt32();

            session.Player.HandleActionDoAllegianceHouseAction(action);
        }
    }
}
