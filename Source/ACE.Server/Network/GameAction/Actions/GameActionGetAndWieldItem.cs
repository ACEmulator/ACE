using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionGetAndWieldItem
    {
        [GameAction(GameActionType.GetAndWieldItem)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint itemGuid = message.Payload.ReadUInt32();
            var location = (EquipMask)message.Payload.ReadInt32();

            session.Player.HandleActionGetAndWieldItem(itemGuid, location);
        }
    }
}
