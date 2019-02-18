using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionTell
    {
        [GameAction(GameActionType.Tell)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            var message = clientMessage.Payload.ReadString16L(); // The client seems to do the trimming for us
            var target = clientMessage.Payload.ReadString16L(); // Needs to be trimmed because it may contain white spaces after the name and before the ,

            target = target.Trim();
            var targetPlayer = PlayerManager.GetOnlinePlayer(target);

            if (targetPlayer == null)
            {
                var statusMessage = new GameEventWeenieError(session, WeenieError.CharacterNotAvailable);
                session.Network.EnqueueSend(statusMessage);
                return;
            }

            if (session.Player != targetPlayer)
                session.Network.EnqueueSend(new GameMessageSystemChat($"You tell {targetPlayer.Name}, \"{message}\"", ChatMessageType.OutgoingTell));

            if (targetPlayer.Squelches.Contains(session.Player))
                return;

            var tell = new GameEventTell(targetPlayer.Session, message, session.Player.Name, session.Player.Guid.Full, targetPlayer.Guid.Full, ChatMessageType.Tell);
            targetPlayer.Session.Network.EnqueueSend(tell);
        }
    }
}
