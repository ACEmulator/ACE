using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRaiseAbility
    {
        [GameAction(GameActionType.RaiseAbility)]
        public static void Handle(ClientMessage message, Session session)
        {
            var networkAbility = (PropertyAttribute)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();

            Ability ability = Ability.None;

            switch (networkAbility)
            {
                case PropertyAttribute.Strength:
                    ability = Ability.Strength;
                    break;
                case PropertyAttribute.Endurance:
                    ability = Ability.Endurance;
                    break;
                case PropertyAttribute.Coordination:
                    ability = Ability.Coordination;
                    break;
                case PropertyAttribute.Quickness:
                    ability = Ability.Quickness;
                    break;
                case PropertyAttribute.Focus:
                    ability =Ability.Focus;
                    break;
                case PropertyAttribute.Self:
                    ability = Ability.Self;
                    break;
                case PropertyAttribute.Undef:
                    return;
            }

            session.Player.RaiseAttributeGameAction(ability, xpSpent);
        }
    }
}
