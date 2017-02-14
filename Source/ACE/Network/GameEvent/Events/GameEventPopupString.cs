namespace ACE.Network.GameEvent
{
    public class GameEventPopupString : GameEventMessage
    {
        public override GameEventOpcode EventType { get { return GameEventOpcode.PopupString; } }

        private string message;

        public GameEventPopupString(Session session, string message) : base(session) { this.message = message; }

        protected override void WriteEventBody()
        {
            writer.WriteString16L(message);
        }
    }
}
