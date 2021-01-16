using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageHearSpeech : GameMessage
    {
        public GameMessageHearSpeech(string messageText, string senderName, uint senderID, ChatMessageType chatMessageType)
            : base(GameMessageOpcode.HearSpeech, GameMessageGroup.UIQueue)
        {
            Writer.WriteString16L(messageText);
            Writer.WriteString16L(senderName);
            Writer.Write(senderID);
            Writer.Write((uint)chatMessageType);
        }
    }
}
