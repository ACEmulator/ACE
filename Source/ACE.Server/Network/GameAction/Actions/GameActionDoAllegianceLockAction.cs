using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionDoAllegianceLockAction
    {
        [GameAction(GameActionType.DoAllegianceLockAction)]
        public static void Handle(ClientMessage message, Session session)
        {
            var action = (AllegianceLockAction)message.Payload.ReadUInt32();

            session.Player.HandleActionDoAllegianceLockAction(action);
        }
    }
}
