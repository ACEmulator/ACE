namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageEmoteText : GameMessage
    {
        public GameMessageEmoteText(uint senderId, string senderName, string emoteText)
            : base(GameMessageOpcode.EmoteText, GameMessageGroup.UIQueue)
        {
            Writer.Write(senderId);
            Writer.WriteString16L(senderName);
            Writer.WriteString16L(emoteText);
        }
    }
}
