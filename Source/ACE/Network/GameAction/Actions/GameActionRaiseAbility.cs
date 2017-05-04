using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRaiseAbility
    {
        [GameAction(GameActionType.RaiseAbility)]
        public static void Handle(ClientMessage message, Session session)
        {
            Entity.Enum.Ability ability = Entity.Enum.Ability.None;
            var networkAbility = (Ability)message.Payload.ReadUInt32();
            switch (networkAbility)
            {
                case Ability.Strength:
                    ability = Entity.Enum.Ability.Strength;
                    break;
                case Ability.Endurance:
                    ability = Entity.Enum.Ability.Endurance;
                    break;
                case Ability.Coordination:
                    ability = Entity.Enum.Ability.Coordination;
                    break;
                case Ability.Quickness:
                    ability = Entity.Enum.Ability.Quickness;
                    break;
                case Ability.Focus:
                    ability = Entity.Enum.Ability.Focus;
                    break;
                case Ability.Self:
                    ability = Entity.Enum.Ability.Self;
                    break;
                case Ability.Undefined:
                    return;
            }
            var xpSpent = message.Payload.ReadUInt32();
            session.Player.SpendXp(ability, xpSpent);
        }
    }
}
