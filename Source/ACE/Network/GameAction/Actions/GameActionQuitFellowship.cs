using System.Threading.Tasks;

using ACE.Network.GameMessages.Messages;

namespace ACE.Network.GameAction.Actions
{
    public class GameActionQuitFellowship
    {
        [GameAction(GameActionType.QuitFellowship)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameMessageFellowshipQuit(session));
        }
        #pragma warning restore 1998
    }
}
