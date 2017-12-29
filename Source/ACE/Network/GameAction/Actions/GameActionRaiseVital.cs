using System.Threading.Tasks;

using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRaiseVital
    {
        [GameAction(GameActionType.RaiseVital)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var vital = (Vital)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();
            Entity.Enum.Ability ability;

            switch (vital)
            {
                case Vital.MaxHealth:
                    ability = Entity.Enum.Ability.Health;
                    break;
                case Vital.MaxStamina:
                    ability = Entity.Enum.Ability.Stamina;
                    break;
                case Vital.MaxMana:
                    ability = Entity.Enum.Ability.Mana;
                    break;
                default:
                    ChatPacket.SendServerMessage(session, $"Unable to Handle GameActionRaiseVital for vital {vital}", ChatMessageType.Broadcast);
                    return;
            }

            session.Player.SpendXp(ability, xpSpent);
        }
        #pragma warning restore 1998
    }
}
