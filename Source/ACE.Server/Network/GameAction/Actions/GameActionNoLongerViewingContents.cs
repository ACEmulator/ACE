using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionNoLongerViewingContents
    {
        [GameAction(GameActionType.NoLongerViewingContents)]

        public static void Handle(ClientMessage message, Session session)
        {
            var objectGuid = message.Payload.ReadUInt32();

            var container = session.Player.CurrentLandblock?.GetObject(objectGuid) as Container;

            if (container != null && container.Viewer == session.Player.Guid.Full)
                container.Close(session.Player);
        }
    }
}
