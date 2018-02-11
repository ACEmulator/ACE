using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRaiseVital
    {
        [GameAction(GameActionType.RaiseVital)]
        public static void Handle(ClientMessage message, Session session)
        {
            var vital = (Vital)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();
            global::ACE.Entity.Enum.Ability ability;

            switch (vital)
            {
                case Vital.MaxHealth:
                    ability = global::ACE.Entity.Enum.Ability.Health;
                    break;
                case Vital.MaxStamina:
                    ability = global::ACE.Entity.Enum.Ability.Stamina;
                    break;
                case Vital.MaxMana:
                    ability = global::ACE.Entity.Enum.Ability.Mana;
                    break;
                default:
                    ChatPacket.SendServerMessage(session, $"Unable to Handle GameActionRaiseVital for vital {vital}", ChatMessageType.Broadcast);
                    return;
            }

            session.Player.SpendXp(ability, xpSpent);
        }
    }
}
