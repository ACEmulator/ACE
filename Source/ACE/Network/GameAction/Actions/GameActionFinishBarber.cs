using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionFinishBarber
    {
        [GameAction(GameActionType.FinishBarber)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            session.Player.FinishBarber(message);
        }
        #pragma warning restore 1998
    }
}
