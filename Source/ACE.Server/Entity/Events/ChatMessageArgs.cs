using System;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Events
{
    public class ChatMessageArgs : EventArgs
    {
        public string Message { get; set; }

        public ChatMessageType MessageType { get; set; }

        public ChatMessageArgs(string message, ChatMessageType type)
        {
            this.Message = message;
            this.MessageType = type;
        }
    }
}
