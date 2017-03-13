
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.Tell)]
    public class GameActionTell : GameActionPacket
    {
        public GameActionTell(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        private string message;
        private string target;

        public override void Read()
        {
            message = Fragment.Payload.ReadString16L(); // The client seems to do the trimming for us
            target = Fragment.Payload.ReadString16L(); // Needs to be trimmed because it may contain white spaces after the name and before the ,
            target = target.Trim();
        }

        public override void Handle()
        {
            var targetSession = WorldManager.FindByPlayerName(target);

            if (targetSession == null)
            {
                var statusMessage = new GameEventDisplayStatusMessage(Session, StatusMessageType1.CharacterNotAvailable);
                Session.WorldSession.EnqueueSend(statusMessage);
            }
            else
            {
                if (Session.Player != targetSession.Player)
                    Session.WorldSession.EnqueueSend(new GameMessageSystemChat($"You tell {target}, \"{message}\"", ChatMessageType.OutgoingTell));

                var tell = new GameEventTell(targetSession, message, Session.Player.Name, Session.Player.Guid.Full, targetSession.Player.Guid.Full, ChatMessageType.Tell);
                targetSession.WorldSession.EnqueueSend(tell);
            }
        }
    }
}
