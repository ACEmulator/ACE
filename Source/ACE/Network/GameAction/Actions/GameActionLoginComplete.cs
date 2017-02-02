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

            var setState         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var setStateFragment = new ServerPacketFragment(0x0A, FragmentOpcode.SetState);
            setStateFragment.Payload.Write(session.Character.Guid.Full);
            setStateFragment.Payload.Write((uint)(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide));
            setStateFragment.Payload.Write((ushort)session.Character.LoginIndex);
            setStateFragment.Payload.Write((ushort)++session.Character.PortalIndex);
            setState.Fragments.Add(setStateFragment);

            // TODO: should be broadcast
            NetworkManager.SendPacket(ConnectionType.World, setState, session);
        }
    }
}
