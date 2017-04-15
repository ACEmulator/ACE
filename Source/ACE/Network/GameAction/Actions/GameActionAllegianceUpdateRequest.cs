using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionAllegianceUpdateRequest
    {
        [GameAction(GameActionType.AllegianceUpdateRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            var unknown1 = message.Payload.ReadUInt32();
            // TODO

            var allegianceUpdate = new GameEventAllegianceUpdate(session);

            session.EnqueueSend(allegianceUpdate);
        }
    }
}