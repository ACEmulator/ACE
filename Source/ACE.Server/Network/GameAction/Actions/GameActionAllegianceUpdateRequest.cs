using System;

using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAllegianceUpdateRequest
    {
        [GameAction(GameActionType.AllegianceUpdateRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            var uiPanel = Convert.ToBoolean(message.Payload.ReadUInt32());

            var player = session.Player;

            var allegiance = player != null ? player.Allegiance : null;
            var allegianceNode = player != null ? player.AllegianceNode : null;

            var allegianceUpdate = new GameEventAllegianceUpdate(session, allegiance, allegianceNode);

            session.Network.EnqueueSend(allegianceUpdate, new GameEventAllegianceAllegianceUpdateDone(session));
        }
    }
}
