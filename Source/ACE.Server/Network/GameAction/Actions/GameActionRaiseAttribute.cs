using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRaiseAttribute
    {
        [GameAction(GameActionType.RaiseAttribute)]
        public static void Handle(ClientMessage message, Session session)
        {
            var attribute = (PropertyAttribute)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();

            session.Player.HandleActionRaiseAttribute(attribute, xpSpent);
        }
    }
}
