﻿namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageSoulEmote : GameMessage
    {
        public GameMessageSoulEmote(uint senderId, string senderName, string emoteText)
            : base(GameMessageOpcode.SoulEmote, GameMessageGroup.Group09)
        {
            Writer.Write(senderId);
            Writer.WriteString16L(senderName);
            Writer.WriteString16L(emoteText);
        }
    }
}
