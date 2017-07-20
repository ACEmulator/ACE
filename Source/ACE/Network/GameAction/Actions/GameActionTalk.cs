using ACE.Command;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTalk
    {
        [GameAction(GameActionType.Talk)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            var message = clientMessage.Payload.ReadString16L();
            session.Player.HandleActionTalk(message);           
        }
    }
}
