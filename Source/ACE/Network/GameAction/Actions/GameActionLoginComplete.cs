using ACE.Entity;

namespace ACE.Network.GameAction
{
    [GameAction(GameActionOpcode.LoginComplete)]
    public class GameActionLoginComplete : GameActionPacket
    {
        public GameActionLoginComplete(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            session.Character.InWorld = true;
            session.Character.SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);
        }
    }
}
