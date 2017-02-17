
using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.RaiseAbility)]
    public class GameActionRaiseAbility : GameActionPacket
    {
        private Entity.Enum.Ability ability;
        private uint xpSpent;

        public GameActionRaiseAbility(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            var networkAbility = (Ability)fragment.Payload.ReadUInt32();
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
            xpSpent = fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            session.Player.SpendXp(ability, xpSpent);
        }
    }
}
