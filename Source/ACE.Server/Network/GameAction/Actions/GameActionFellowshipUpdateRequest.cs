using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public class GameActionFellowshipUpdateRequest
    {
        [GameAction(GameActionType.FellowshipUpdateRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            if (session.Player.Fellowship != null)
            {
                session.Network.EnqueueSend(new GameEventFellowshipFullUpdate(session));
            }
        }
    }
}
