using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionTargetedMeleeAttack
    {
        [GameAction(GameActionType.TargetedMeleeAttack)]
        public static void Handle(ClientMessage message, Session session)
        {
            var objectId = message.Payload.ReadUInt32();
            var attackHeight = message.Payload.ReadUInt32();
            var powerLevel = message.Payload.ReadSingle();

            ObjectGuid guid = new ObjectGuid(objectId);

            session.Player.HandleActionTargetedMeleeAttack(guid, attackHeight, powerLevel);
        }
    }
}
