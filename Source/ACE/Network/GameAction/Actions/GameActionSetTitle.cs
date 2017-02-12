namespace ACE.Network.GameAction.Actions
{
	[GameAction(GameActionOpcode.TitleSet)]
	public class GameActionSetTitle : GameActionPacket
	{
		private uint title;

		public GameActionSetTitle(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

		public override void Read()
		{
			this.title = fragment.Payload.ReadUInt32();
		}

		public override void Handle()
		{
			session.Player.SetTitle(title);
		}
	}
}
