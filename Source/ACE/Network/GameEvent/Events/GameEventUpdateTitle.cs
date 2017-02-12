using ACE.Entity.Enum;
using System;

namespace ACE.Network.GameEvent
{
	public class GameEventUpdateTitle : GameEventPacket
	{
		private uint title;

		public override GameEventOpcode Opcode { get { return GameEventOpcode.UpdateTitle; } }

		public GameEventUpdateTitle(Session session, uint title)
			: base(session)
		{
			this.title = title;
		}

		protected override void WriteEventBody()
		{
			fragment.Payload.Write(this.title);
			fragment.Payload.Write(1u);
		}
	}
}
