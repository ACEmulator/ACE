using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using log4net;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionTell
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [GameAction(GameActionType.Tell)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            var message = clientMessage.Payload.ReadString16L(); // The client seems to do the trimming for us
            var target = clientMessage.Payload.ReadString16L(); // Needs to be trimmed because it may contain white spaces after the name and before the ,

            if (session.Player.IsGagged)
            {
                session.Player.SendGagError();
                return;
            }

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

            if (targetPlayer.SquelchManager.Squelches.Contains(session.Player, ChatMessageType.Tell))
            {
                session.Network.EnqueueSend(new GameEventWeenieErrorWithString(session, WeenieErrorWithString.MessageBlocked_,$"{target} has you squelched."));
                //log.Warn($"Tell from {session.Player.Name} (0x{session.Player.Guid.ToString()}) to {targetPlayer.Name} (0x{targetPlayer.Guid.ToString()}) blocked due to squelch");
                return;
            }

            if (targetPlayer.IsAfk)
            {
                session.Network.EnqueueSend(new GameEventWeenieErrorWithString(session, WeenieErrorWithString.AFK, $"{targetPlayer.Name} is away: " + (string.IsNullOrWhiteSpace(targetPlayer.AfkMessage) ? WorldObjects.Player.DefaultAFKMessage : targetPlayer.AfkMessage)));
                //return;
            }

            var tell = new GameEventTell(targetPlayer.Session, message, session.Player.Name, session.Player.Guid.Full, targetPlayer.Guid.Full, ChatMessageType.Tell);
            targetPlayer.Session.Network.EnqueueSend(tell);
        }
    }
}
