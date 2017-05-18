using System.Collections;
using ACE.Entity.Enum;
using ACE.Entity.PlayerActions;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionLoginComplete
    {
        [GameAction(GameActionType.LoginComplete)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.RequestAction(new DelegateAction(() => { return UpdatePlayerPos(session); }));
        }

        public static IEnumerator UpdatePlayerPos(Session session)
        {
            session.Player.InWorld = true;
            session.Player.SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);
            yield return null;
        }
    }
}