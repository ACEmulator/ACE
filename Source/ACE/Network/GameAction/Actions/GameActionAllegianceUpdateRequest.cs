using System.Threading.Tasks;

using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionAllegianceUpdateRequest
    {
        [GameAction(GameActionType.AllegianceUpdateRequest)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var unknown1 = message.Payload.ReadUInt32();
            // TODO

            var allegianceUpdate = new GameEventAllegianceUpdate(session);

            session.Network.EnqueueSend(allegianceUpdate);
        }
        #pragma warning restore 1998
    }
}
