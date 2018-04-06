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

            //Ability ability;

            //switch (vital)
            //{
            //    case Vital.MaxHealth:
            //        ability = Ability.Health;
            //        break;
            //    case Vital.MaxStamina:
            //        ability = Ability.Stamina;
            //        break;
            //    case Vital.MaxMana:
            //        ability = Ability.Mana;
            //        break;
            //    default:
            //        ChatPacket.SendServerMessage(session, $"Unable to Handle GameActionRaiseVital for vital {vital}", ChatMessageType.Broadcast);
            //        return;
            //}

            session.Player.HandleActionRaiseVital(vital, xpSpent);
        }
    }
}
