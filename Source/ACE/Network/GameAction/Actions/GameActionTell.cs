using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTell
    {
        [GameAction(GameActionType.Tell)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            var message = clientMessage.Payload.ReadString16L(); // The client seems to do the trimming for us
            var target = clientMessage.Payload.ReadString16L(); // Needs to be trimmed because it may contain white spaces after the name and before the ,
            target = target.Trim();
            var targetsession = WorldManager.FindByPlayerName(target);

            if (targetsession == null)
            {
                var statusMessage = new GameEventDisplayStatusMessage(session, StatusMessageType1.CharacterNotAvailable);
                session.EnqueueSend(statusMessage);
            }
            else
            {
                if (session.Player != targetsession.Player)
                    session.EnqueueSend(new GameMessageSystemChat($"You tell {target}, \"{message}\"", ChatMessageType.OutgoingTell));

                var tell = new GameEventTell(targetsession, message, session.Player.Name, session.Player.Guid.Full, targetsession.Player.Guid.Full, ChatMessageType.Tell);
                targetsession.EnqueueSend(tell);
            }
        }
    }
}