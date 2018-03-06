using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAllegianceUpdateRequest
    {
        [GameAction(GameActionType.AllegianceUpdateRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            var unknown1 = message.Payload.ReadUInt32();
            // TODO

            var allegianceUpdate = new GameEventAllegianceUpdate(session);

            session.Network.EnqueueSend(allegianceUpdate, new GameEventAllegianceAllegianceUpdateDone(session));
        }
    }
}
