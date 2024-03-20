namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageSoulEmote : GameMessage
    {
        public GameMessageSoulEmote(uint senderId, string senderName, string emoteText)
            : base(GameMessageOpcode.SoulEmote, GameMessageGroup.UIQueue, 88) // 88 is the max seen in retail pcaps
        {
            Writer.Write(senderId);
            Writer.WriteString16L(senderName);
            Writer.WriteString16L(emoteText);
        }
    }
}
