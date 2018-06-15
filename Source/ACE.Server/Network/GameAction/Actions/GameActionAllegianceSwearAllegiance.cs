using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAllegianceSwearAllegiance
    {
        [GameAction(GameActionType.SwearAllegiance)]
        public static void Handle(ClientMessage message, Session session)
        {
            var target = message.Payload.ReadUInt32();
            var targetGuid = new ObjectGuid(target);

            session.Player.HandleActionSwearAllegiance(targetGuid);
        }
    }
}
