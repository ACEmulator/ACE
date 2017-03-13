
namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.TitleSet)]
    public class GameActionSetTitle : GameActionPacket
    {
        private uint title;

        public GameActionSetTitle(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            title = Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            Session.Player.SetTitle(title);
        }
    }
}
