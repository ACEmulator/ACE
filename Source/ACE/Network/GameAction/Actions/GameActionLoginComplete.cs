
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.LoginComplete)]
    public class GameActionLoginComplete : GameActionPacket
    {
        public GameActionLoginComplete(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            Session.Player.InWorld = true;
            Session.Player.SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);
        }
    }
}
