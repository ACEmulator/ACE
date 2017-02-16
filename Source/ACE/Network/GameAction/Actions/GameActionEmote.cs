namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.Emote)]
    public class GameActionEmote : GameActionPacket
    {
        private string emote;

        public GameActionEmote(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            this.emote = fragment.Payload.ReadString16L();
        }

        public override void Handle()
        {
            // TODO: send emote text to other characters
            // The emote text comes from the client ready to broadcast.
            // For example: *afk* comes as "decides to rest for a while."
        }
    }
}
