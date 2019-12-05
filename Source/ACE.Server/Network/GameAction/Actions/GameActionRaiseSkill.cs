using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRaiseSkill
    {
        [GameAction(GameActionType.RaiseSkill)]
        public static void Handle(ClientMessage message, Session session)
        {
            var skill = (Skill)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();

            session.Player.HandleActionRaiseSkill(skill, xpSpent);
        }
    }
}
