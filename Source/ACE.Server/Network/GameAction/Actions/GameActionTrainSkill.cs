using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionTrainSkill
    {
        [GameAction(GameActionType.TrainSkill)]
        public static void Handle(ClientMessage message, Session session)
        {
            var skill = (Skill)message.Payload.ReadUInt32();
            var creditsSpent = message.Payload.ReadInt32();

            session.Player.HandleActionTrainSkill(skill, creditsSpent);
        }
    }
}
