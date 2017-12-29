using System.Threading.Tasks;

using ACE.Network.GameMessages.Messages;

namespace ACE.Network.GameAction.Actions
{
    public class GameActionFellowshipUpdateRequest
    {
        [GameAction(GameActionType.FellowshipUpdateRequest)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            if (session.Player.Fellowship != null)
            {
                session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(session));
            }
        }
        #pragma warning restore 1998
    }
}
