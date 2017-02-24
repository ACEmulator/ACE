namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageEmoteText : GameMessage
    {
        public GameMessageEmoteText(uint senderId, string senderName, string emoteText) 
            : base(GameMessageOpcode.EmoteText, 0x9)
        {
            Writer.Write(senderId);
            Writer.Write(senderName);
            Writer.Write(emoteText);
        }
    }
}
