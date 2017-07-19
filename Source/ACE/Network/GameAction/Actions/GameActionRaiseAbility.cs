using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRaiseAbility
    {
        [GameAction(GameActionType.RaiseAbility)]
        public static void Handle(ClientMessage message, Session session)
        {
            Entity.Enum.Ability ability = Entity.Enum.Ability.None;
            var networkAbility = (PropertyAttribute)message.Payload.ReadUInt32();
            switch (networkAbility)
            {
                case PropertyAttribute.Strength:
                    ability = Entity.Enum.Ability.Strength;
                    break;
                case PropertyAttribute.Endurance:
                    ability = Entity.Enum.Ability.Endurance;
                    break;
                case PropertyAttribute.Coordination:
                    ability = Entity.Enum.Ability.Coordination;
                    break;
                case PropertyAttribute.Quickness:
                    ability = Entity.Enum.Ability.Quickness;
                    break;
                case PropertyAttribute.Focus:
                    ability = Entity.Enum.Ability.Focus;
                    break;
                case PropertyAttribute.Self:
                    ability = Entity.Enum.Ability.Self;
                    break;
                case PropertyAttribute.Undef:
                    return;
            }
            var xpSpent = message.Payload.ReadUInt32();
            session.Player.SpendXp(ability, xpSpent);
        }
    }
}
