using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network.GameAction.Actions
{
    public class GameActionFellowshipUpdateRequest
    {
        [GameAction(GameActionType.FellowshipUpdateRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            if (session.Player.Fellowship != null)
            {
                session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(session));
            }
        }
    }
}
