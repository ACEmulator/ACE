using System.Threading.Tasks;

using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionRaiseSkill
    {
        [GameAction(GameActionType.RaiseSkill)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var skill = (Skill)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();
            session.Player.SpendXp(skill, xpSpent);
        }
        #pragma warning restore 1998
    }
}
