using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRaiseAbility
    {
        [GameAction(GameActionType.RaiseAbility)]
        public static void Handle(ClientMessage message, Session session)
        {
            global::ACE.Entity.Enum.Ability ability = global::ACE.Entity.Enum.Ability.None;
            var networkAbility = (PropertyAttribute)message.Payload.ReadUInt32();
            switch (networkAbility)
            {
                case PropertyAttribute.Strength:
                    ability = global::ACE.Entity.Enum.Ability.Strength;
                    break;
                case PropertyAttribute.Endurance:
                    ability = global::ACE.Entity.Enum.Ability.Endurance;
                    break;
                case PropertyAttribute.Coordination:
                    ability = global::ACE.Entity.Enum.Ability.Coordination;
                    break;
                case PropertyAttribute.Quickness:
                    ability = global::ACE.Entity.Enum.Ability.Quickness;
                    break;
                case PropertyAttribute.Focus:
                    ability = global::ACE.Entity.Enum.Ability.Focus;
                    break;
                case PropertyAttribute.Self:
                    ability = global::ACE.Entity.Enum.Ability.Self;
                    break;
                case PropertyAttribute.Undef:
                    return;
            }
            var xpSpent = message.Payload.ReadUInt32();
            session.Player.SpendXp(ability, xpSpent);
        }
    }
}
