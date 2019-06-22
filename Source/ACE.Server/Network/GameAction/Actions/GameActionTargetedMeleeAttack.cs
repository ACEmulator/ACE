namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionTargetedMeleeAttack
    {
        [GameAction(GameActionType.TargetedMeleeAttack)]
        public static void Handle(ClientMessage message, Session session)
        {
            var targetGuid = message.Payload.ReadUInt32();
            var attackHeight = message.Payload.ReadUInt32();
            var powerLevel = message.Payload.ReadSingle();

            session.Player.HandleActionTargetedMeleeAttack(targetGuid, attackHeight, powerLevel);
        }
    }
}
