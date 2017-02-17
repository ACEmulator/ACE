
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.LoginComplete)]
    public class GameActionLoginComplete : GameActionPacket
    {
        public GameActionLoginComplete(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            session.Player.InWorld = true;
            session.Player.SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);
        }
    }
}
