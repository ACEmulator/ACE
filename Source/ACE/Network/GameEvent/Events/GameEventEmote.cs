
namespace ACE.Network.GameEvent.Events
{
    public class GameEventEmote : GameEventPacket
    {
        private uint senderId;
        private string senderName;
        private string emote;

        public override GameEventOpcode Opcode { get { return GameEventOpcode.Emote; } }

        public GameEventEmote(Session session, uint senderId, string senderName, string emote)
            : base(session)
        {
            this.senderId = senderId;
            this.senderName = senderName;
            this.emote = emote;
        }

        protected override void WriteEventBody()
        {
            fragment.Payload.Write(this.senderId);
            fragment.Payload.Write(this.senderName);
            fragment.Payload.Write(this.emote);
        }
    }
}
