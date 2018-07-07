using ACE.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionNoLongerViewingContents
    {
        [GameAction(GameActionType.NoLongerViewingContents)]

        public static void Handle(ClientMessage message, Session session)
        {
            var objectGuid = new ObjectGuid(message.Payload.ReadUInt32());

            var wo = session.Player.CurrentLandblock?.GetObject(objectGuid) as Container;

            if (wo != null && wo.Viewer == session.Player.Guid.Full)
                wo.Close(session.Player);
        }
    }
}
