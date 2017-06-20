using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionGetAndWieldItem
    {
        [GameAction(GameActionType.EvtInventoryGetAndWieldItem)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint itemGuid = message.Payload.ReadUInt32();
            uint location = message.Payload.ReadUInt32();
            session.Player.EquipItem(itemGuid, location);
            // TODO: Og II Pass this to player to process and let them call GameEventWieldItem response.
            session.Network.EnqueueSend(new GameEventWieldItem(session, itemGuid, location));
        }
    }
}
