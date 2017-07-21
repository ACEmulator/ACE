using ACE.Common.Extensions;

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
