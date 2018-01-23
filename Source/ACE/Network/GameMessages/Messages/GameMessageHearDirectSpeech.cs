using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageHearDirectSpeech : GameMessage
    {
        public GameMessageHearDirectSpeech(string messageText, string senderName, uint senderID, uint targetID, ChatMessageType chatMessageType)
            : base(GameMessageOpcode.HearDirectSpeech, GameMessageGroup.Group09)
        {
            Writer.WriteString16L(messageText);
            Writer.WriteString16L(senderName);
            Writer.Write(senderID);
            Writer.Write(targetID);
            Writer.Write((uint)chatMessageType);
        }
    }
}
