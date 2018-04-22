using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionTargetedMissileAttack
    {
        [GameAction(GameActionType.TargetedMissileAttack)]
        public static void Handle(ClientMessage message, Session session)
        {
            var objectId = message.Payload.ReadUInt32();
            var attackHeight = message.Payload.ReadUInt32();
            var accuracyLevel = message.Payload.ReadSingle();

            ObjectGuid guid = new ObjectGuid(objectId);

            session.Player.HandleActionTargetedMissileAttack(guid, attackHeight, accuracyLevel);
        }
    }
}
