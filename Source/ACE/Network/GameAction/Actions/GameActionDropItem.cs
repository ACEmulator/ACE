using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionDropItem
    {
        [GameAction(GameActionType.DropItem)]

        public static void Handle(ClientMessage message, Session session)
        {
            var objectGuid = new ObjectGuid(message.Payload.ReadUInt32());
            session.Player.HandleDropItem(objectGuid, session);
        }
    }
}