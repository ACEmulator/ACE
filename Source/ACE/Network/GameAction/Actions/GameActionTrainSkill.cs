using System.Threading.Tasks;

using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTrainSkill
    {
        [GameAction(GameActionType.TrainSkill)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var skill = (Skill)message.Payload.ReadUInt32();
            var creditsSpent = message.Payload.ReadInt32();
            // train skills
            session.Player.TrainSkill(skill, creditsSpent);
        }
        #pragma warning restore 1998
    }
}
