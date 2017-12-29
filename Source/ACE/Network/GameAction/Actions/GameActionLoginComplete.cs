using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionLoginComplete
    {
        [GameAction(GameActionType.LoginComplete)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            session.Player.InWorld = true;
            session.Player.ReportCollision = true;
            session.Player.IgnoreCollision = false;
            session.Player.Hidden = false;
            session.Player.EnqueueBroadcastPhysicsState();
        }
        #pragma warning restore 1998
    }
}
