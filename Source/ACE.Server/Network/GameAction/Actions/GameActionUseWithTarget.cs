using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionUseWithTarget
    {
        [GameAction(GameActionType.UseWithTarget)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint sourceObjectId = message.Payload.ReadUInt32();
            uint targetObjectId = message.Payload.ReadUInt32();

            session.Player.HandleActionUseWithTarget(new ObjectGuid(sourceObjectId), new ObjectGuid(targetObjectId));
        }
    }
}
