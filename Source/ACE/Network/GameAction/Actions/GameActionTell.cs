using System.Threading.Tasks;

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
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage clientMessage, Session session)
        {
            var message = clientMessage.Payload.ReadString16L(); // The client seems to do the trimming for us
            var target = clientMessage.Payload.ReadString16L(); // Needs to be trimmed because it may contain white spaces after the name and before the ,
            target = target.Trim();
            var targetsession = WorldManager.FindByPlayerName(target);

            if (targetsession == null)
            {
                var statusMessage = new GameEventDisplayStatusMessage(session, StatusMessageType1.CharacterNotAvailable);
                session.Network.EnqueueSend(statusMessage);
            }
            else
            {
                if (session.Player != targetsession.Player)
                    session.Network.EnqueueSend(new GameMessageSystemChat($"You tell {target}, \"{message}\"", ChatMessageType.OutgoingTell));

                var tell = new GameEventTell(targetsession, message, session.Player.Name, session.Player.Guid.Full, targetsession.Player.Guid.Full, ChatMessageType.Tell);
                targetsession.Network.EnqueueSend(tell);
            }
        }
        #pragma warning restore 1998
    }
}
