namespace ACE.Network.GameEvent
{
    public class GameEventPopupString : GameEventPacket
    {
        public override GameEventOpcode Opcode { get { return GameEventOpcode.PopupString; } }

        private string message;

        public GameEventPopupString(Session session, string message) : base(session) { this.message = message; }

        protected override void WriteEventBody()
        {
            fragment.Payload.WriteString16L(message);
        }
    }
}
