using ACE.Entity.Events;
using System;

namespace ACE.Entity
{
    public interface IChatSender
    {
        event EventHandler<ChatMessageArgs> OnChat;
    }
}
