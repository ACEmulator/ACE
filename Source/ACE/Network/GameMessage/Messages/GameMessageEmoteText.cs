
namespace ACE.Network.Messages
{
    public class GameMessageEmoteText : GameMessage
    {
        public uint SenderId { get; private set; }
        public string SenderName { get; private set; }
        public string EmoteText { get; private set; }

        public GameMessageEmoteText(uint senderId, string senderName, string emoteText) : base(GameMessageOpcode.EmoteText)
        {
            SenderId = senderId;
            SenderName = senderName;
            EmoteText = emoteText;
            writer.Write(this.SenderId);
            writer.Write(this.SenderName);
            writer.Write(this.EmoteText);
        }
    }
}
