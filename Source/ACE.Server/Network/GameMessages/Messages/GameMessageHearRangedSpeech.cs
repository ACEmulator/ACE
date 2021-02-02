using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageHearRangedSpeech : GameMessage
    {
        public GameMessageHearRangedSpeech(string messageText, string senderName, uint senderID, float range, ChatMessageType chatMessageType)
            : base(GameMessageOpcode.HearRangedSpeech, GameMessageGroup.UIQueue)
        {
            Writer.WriteString16L(messageText);
            Writer.WriteString16L(senderName);
            Writer.Write(senderID);
            Writer.Write(range);
            Writer.Write((uint)chatMessageType);
        }
    }
}
