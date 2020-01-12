using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRaiseVital
    {
        [GameAction(GameActionType.RaiseVital)]
        public static void Handle(ClientMessage message, Session session)
        {
            var vital = (PropertyAttribute2nd)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();

            session.Player.HandleActionRaiseVital(vital, xpSpent);
        }
    }
}
