
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionNoLongerViewingContents
    {
        [GameAction(GameActionType.NoLongerViewingContents)]
        public static void Handle(ClientMessage message, Session session)
        {
            var objectGuid = message.Payload.ReadUInt32();

            session.Player.HandleActionNoLongerViewingContents(objectGuid);
        }
    }
}
