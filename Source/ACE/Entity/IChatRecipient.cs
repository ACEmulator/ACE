using ACE.Entity.Events;
using System;

namespace ACE.Entity
{
    public interface IChatRecipient
    {
        /// <summary>
        /// called when the object should receive chat.  sender may be null in the use case
        /// of system broadcasts
        /// </summary>
        void ReceiveChat(WorldObject sender, ChatMessageArgs e);
    }
}
