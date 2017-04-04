using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;

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